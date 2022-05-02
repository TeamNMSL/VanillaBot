using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.PictureDrawer
{
    public class Drawer:IDisposable
    {
        int PicWide;
        int PicHigh;
        Bitmap bitmap;
        Graphics g;
        Image Background;
        Locations DrawLocation;
        Fonts ThemeFonts;
        Colour color;
        private bool Alreadydisposed=false;

        public class Locations {
            public Rectangle Title;
            public Rectangle Context;
            public Rectangle Others;
            public int ContextDropSize;

            
        }
        public class Fonts {
            public Font TitleFont;
            public Font ContextFont;
            public Font OthersFont;
            private bool Alreadydisposed=false;

            public void Dispose()
            {
                if (Alreadydisposed) return;
                
                    TitleFont?.Dispose();
                    TitleFont = null;
                
                
                    ContextFont?.Dispose();
                    ContextFont = null;
                
                    OthersFont?.Dispose();


                    OthersFont = null;
                
                GC.SuppressFinalize(this);
                Alreadydisposed = true;
            }
            ~Fonts() {

                Dispose();
            }

            
        }
        public class Colour {
            public Color TitleColor;
            public Color ContextColor;
            public Color ContextDropColor;
            public Color OthersColor;
            
        }
        public enum Themes
        {
            Ver1
        }
        public enum Direction { 
            I,P
        }
        public Drawer(Themes type,Direction direction)
        {
            if (type==Themes.Ver1)
            {
                if (direction == Direction.I) 
                {
                    PicWide = 640;
                    PicHigh = 1136;
                    Background = GlobalScope.Images.ThemeBackgroundImageVer1_i;
                    DrawLocation = new Locations() {
                        Title = new Rectangle(35, 35, 578, 100),
                        Context = new Rectangle(20, 160, 590, 770),
                        Others = new Rectangle(15, 970, 600, 150),
                        ContextDropSize = 2
                    };
                    ThemeFonts = new Fonts() {
                        TitleFont=new Font("微软雅黑",30),
                        ContextFont= new Font("微软雅黑", 20),
                        OthersFont=new Font("微软雅黑", 15)
                    };
                    color = new Colour() {
                        TitleColor = Color.White,
                        ContextDropColor = Color.FromArgb(46,49,90),
                        ContextColor=Color.White,
                        OthersColor=Color.White
                    };
                }
            }

            bitmap = new Bitmap(PicWide, PicHigh);
            g= Graphics.FromImage(bitmap);
        }
        public void DrawBack() { 
        g.DrawImage(Background, 0, 0);
        }
        public void DrawTitle(string text) {
            
            g.DrawString(text, ThemeFonts.TitleFont,new SolidBrush(color.TitleColor),DrawLocation.Title);
        }
        private GraphicsPath GetStringPath(string s, Font font, RectangleF rect)
        {
            GraphicsPath path = new GraphicsPath();
            
            path.AddString(s,font.FontFamily,(int)font.Style,font.Size,rect,StringFormat.GenericTypographic);

            return path;
        }
        public void DrawContext(string text) {
            using (GraphicsPath path = GetStringPath(text,ThemeFonts.ContextFont,DrawLocation.Context)) {
                g.SmoothingMode = SmoothingMode.AntiAlias;//设置字体质量
                g.DrawPath(new Pen(color.ContextDropColor), path);//绘制轮廓（描边）
                g.FillPath(new SolidBrush(color.ContextColor), path);//填充轮廓（填充）
            }
               /* DrawLocation.Context.X = DrawLocation.Context.X - DrawLocation.ContextDropSize;
            DrawLocation.Context.Y = DrawLocation.Context.Y - DrawLocation.ContextDropSize;
            g.DrawString(text, ThemeFonts.ContextFont, new SolidBrush(color.ContextDropColor), DrawLocation.Context);
            DrawLocation.Context.X = DrawLocation.Context.X + 2*DrawLocation.ContextDropSize;
            DrawLocation.Context.Y = DrawLocation.Context.Y + 2*DrawLocation.ContextDropSize;
            g.DrawString(text, ThemeFonts.ContextFont, new SolidBrush(color.ContextDropColor), DrawLocation.Context);
            DrawLocation.Context.X = DrawLocation.Context.X - DrawLocation.ContextDropSize;
            DrawLocation.Context.Y = DrawLocation.Context.Y - DrawLocation.ContextDropSize;
            g.DrawString(text, ThemeFonts.ContextFont, new SolidBrush(color.ContextColor), DrawLocation.Context);*/
        }
        public void DrawOtherText(string text)
        {
            g.DrawString(text, ThemeFonts.OthersFont, new SolidBrush(color.OthersColor), DrawLocation.Others);
        }
        public byte[] Save() {
            g.Save();
            MemoryStream ms=new MemoryStream();
            bitmap.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
            byte[] result = ms.GetBuffer();
            ms.Dispose();
            return result;
        }

        public void Dispose()
        {
            if (Alreadydisposed) return;
           
                bitmap?.Dispose();
                bitmap = null;
           
           
                g?.Dispose();
                g = null;
           

            
                ThemeFonts?.Dispose();
                ThemeFonts = null;
            GC.Collect();
            GC.SuppressFinalize(this);
            Alreadydisposed = true;

        }
        ~Drawer() { 
        
            Dispose();
        }
    }
}
