using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Drawing;

namespace VanillaForKonata
{
    public static class GlobalScope
    {
        public static void Initialization()
        {
            Encodings.init();
            if (!File.Exists(@".\BotSettings"))
            {
                File.WriteAllText(@".\BotSettings", "Path=\nAdminUin=");
                Console.WriteLine("BotSettings Created,Edit it and run bot again\n[Help]\nPath:The path of bot data like C:\\User\\Konata\\Document\\Kagami\nAdminUin:The QQUin of Admin,like Admin=114514,1919,810");
                Console.ReadKey();
                Environment.Exit(0);
            }
            var BotCfgContent = File.ReadAllText(@".\BotSettings");
            foreach (var item in BotCfgContent.Split("\n"))
            {
                if (item.StartsWith("Path="))
                {
                    string t = item;
                    Path.AppPath = t.Replace("Path=", "").Replace("\r", "").Replace("\n", "");
                }
                else if (item.StartsWith("AdminUin="))
                {
                    string[] t = item.Replace("AdminUin=", "").Replace("\r", "").Replace("\n", "").Split(",");
                    foreach (var i in t)
                    {
                        Cfgs.BotAdmins.Add(ulong.Parse(i));
                    }
                }
            }
            if (!Directory.Exists(Path.AppPath))
                Directory.CreateDirectory(Path.AppPath);
            Path.Manual = $"{Path.AppPath}\\Manual";
            if (!Directory.Exists(Path.Manual))
                Directory.CreateDirectory(Path.Manual);
            Path.DatabasePath = $"{Path.AppPath}\\Databases";
            if (!Directory.Exists(Path.DatabasePath))
                Directory.CreateDirectory(Path.DatabasePath);
            if (!File.Exists($"{Path.DatabasePath}\\Switches"))
                BotInternal.CanBeUse.Initial();
            if (!File.Exists($"{Path.DatabasePath}\\ImmersionMode"))
                BotInternal.ImmersionMode.Initial();
            Path.ImagesPath = $"{Path.AppPath}\\Images";
            if (!Directory.Exists(Path.ImagesPath))
                Directory.CreateDirectory(Path.ImagesPath);
            Images.Init();
            Path.temp = $"{Path.AppPath}\\temp";
            if (!Directory.Exists(Path.temp))
                Directory.CreateDirectory(Path.temp);
            Path.DataFromOthers = $"{Path.AppPath}\\DataFromOthers";
            if (!Directory.Exists(Path.DataFromOthers))
                Directory.CreateDirectory(Path.DataFromOthers);
            Path.SDVX = $"{Path.DataFromOthers}\\SDVX_ROOT";
            if (!Directory.Exists(Path.SDVX))
                Directory.CreateDirectory(Path.SDVX);
            BotFunction.Games.SDVX.Init();
            Path.Bin = $"{Path.AppPath}\\Bin";
            if (!Directory.Exists(Path.Bin))
                Directory.CreateDirectory(Path.Bin);
            Path.Arcaea = $"{Path.AppPath}\\Arcaea";
            if (!Directory.Exists(Path.Arcaea))
                Directory.CreateDirectory(Path.Arcaea);
            Arc.Init();
            BotFunction.Tools.Bottle.bottleController.initBox();
            Path.configs = $"{Path.AppPath}\\configs";
            if (!Directory.Exists(Path.configs))
                Directory.CreateDirectory(Path.configs);
            BotFunction.Tools.box.QQBox.init();
        }
        public static class Arc {
            public static string Api;
            public static string UserAgent;

            public static void Init()
            {
                Api=File.ReadAllText($"{Path.Arcaea}\\api").Replace("\r", "").Replace("\n", "");
                UserAgent= File.ReadAllText($"{Path.Arcaea}\\user-agent").Replace("\r", "").Replace("\n", "");
                if (!File.Exists($"{Path.Arcaea}{PartPath.bindDB}"))
                {
                    var a = new Util.Database($"{Path.Arcaea}{PartPath.bindDB}");
                    a.Create("ArcBind","Uin","Code");
                }
            }
            public static class PartPath {
                public static string Assets = @"\assets";
                public static string SongsFold = @"\songs";
                public static string bindDB = @"\userdb";
            }
        }
        public static class Path
        {
            public static string AppPath;
            public static string DatabasePath;
            public static string ImagesPath;
            public static string DataFromOthers;
            public static string SDVX;
            public static string temp;
            public static string Bin;
            public static string Manual;
            public static string Arcaea;
            public static string configs;
        }
        public static class Cfgs
        {
            public static string BotName = "java.lang.NullPointerException";
            public static List<ulong> BotAdmins = new List<ulong>();
            public static List<string> FunctionList = new List<string>() {
            "复读","龙图","选择","sdvx","arcaea","龙吟","bottle","openbox"
            };
            public static int AbuseLimit=15;
            public static long AbuseClearTime=60000;
        }
        public static class Images {
            static public Image ThemeBackgroundImageVer1_i;
            static public void Init() {
                ThemeBackgroundImageVer1_i=Image.FromStream(new FileStream($"{GlobalScope.Path.ImagesPath}\\ThemeBack\\Ver1\\back_i.png", FileMode.Open, FileAccess.Read));
            }
        }
        public static class Encodings {
            public static void init() {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                shiftjis = Encoding.GetEncoding(932);


            }
            public static Encoding shiftjis;

        }
    }
}
