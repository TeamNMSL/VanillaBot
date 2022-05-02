using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction
{
    partial class Sys
    {
        public static class Stat {
            public static MessageBuilder GetStat() {
                
                string rpl = $"======BotInfo======\n" +
                    $"NUIK Bot Branch:Vanilla\n" +
                    $"BotName:{GlobalScope.Cfgs.BotName}\n" +
                    $"======AssemblyInfo======\n" +
                    $"KonataVersion:{Util.BuildStamp.Version}\n" +
                    $"VanillaVersion:{Assembly.GetExecutingAssembly().GetName().Version}\n" +
                    $"======Others======\n" +
                    $"Status:Online\n" +
                    $"DataPath:{GlobalScope.Path.AppPath}";
                PictureDrawer.Drawer drawer = new PictureDrawer.Drawer(PictureDrawer.Drawer.Themes.Ver1,PictureDrawer.Drawer.Direction.I);
                drawer.DrawBack();
                drawer.DrawTitle("Vanilla Status");
                drawer.DrawContext(rpl);
                drawer.DrawOtherText("This is a message showing the status of bot");
                byte[] b=drawer.Save();
                drawer.Dispose();
                return new MessageBuilder().Image(b);

            }
        
        }
    }
}
