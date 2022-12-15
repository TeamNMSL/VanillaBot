using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Konata.Core.Interfaces.Api;
using System.Net;

namespace VanillaForKonata.BotFunction.Tools
{
    internal static class Fortune
    {
        static public MessageBuilder main(Konata.Core.Events.Model.GroupMessageEvent e, Konata.Core.Bot bot) {
            //string Songname = "";
            //string Songcover = "";
            //string Icon = "";
            //string QName = "";
            //string Fortune = "";
            //string Date = "";

            var dtt = DateTime.Now;
            int seed = (int)((dtt.Date.Month*dtt.Date.Year*dtt.Date.Day)/1145+(int)e.MemberUin);
            string tpl = File.ReadAllText($"{GlobalScope.Path.templates}\\Fortune.html");
            Util.PicHTMLBuilder h = new(tpl);
            h.setValue("QName",e.MemberCard);
            h.setValue("Date", dtt.ToString("D") +"   "+ dtt.ToString("dddd"));
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-FFFFFFF");
            
            
            //SDVX start
                int songcount=Games.SDVX.SongIdIndex.Count;
                int index = Util.Rand.GetRandomNumber(seed,0, songcount - 1);

            var aa= (Games.SDVX.getSongDetail(Games.SDVX.Songlist[index])["CoverPath"]).Split("\n");
                h.setValue("Songcover", Util.PicHTMLBuilder.pathConverter(aa[aa.Length-1]));
                h.setValue("Songname", Games.SDVX.Songlist[index]["info"]["title_name"].ToString());
            //SDVX end

            h.setValue("Fortune", (new Random(seed).Next(1, 101)*10).ToString());
            getUserIcon(e.MemberUin);
            h.setValue("Icon",Util.PicHTMLBuilder.pathConverter($"{GlobalScope.Path.temp}\\{e.MemberUin}.jpg"));

            tpl=h.getHTML();
            File.WriteAllText(GlobalScope.Path.temp + "\\Fortune" + fn + ".html", tpl);
            var a=Util.HTML2Pic.generate(GlobalScope.Path.temp + "\\Fortune" + fn + ".html", GlobalScope.Path.temp + "\\Fortune" + fn + ".png",500,800);
            if (a.Result)
            {
                return new MessageBuilder().Image(GlobalScope.Path.temp + "\\Fortune" + fn + ".png");
            }
            return null;
        }
        static public void getUserIcon(uint uin) {
            //http://q1.qlogo.cn/g?b=qq&nk=1848200159&s=640&rrrr=114514
            WebClient webClient= new();
            var a=webClient.DownloadData($"http://q1.qlogo.cn/g?b=qq&nk={uin}&s=640&rrrr={DateTime.Now.ToString("FFFFF")}");
            File.WriteAllBytes($"{GlobalScope.Path.temp}\\{uin}.jpg", a);
        }

        internal static string getNick(string commandString)
        {
            if (commandString == "今日运势")
                return "/v fortune";
            return commandString;
        }
    }
}
