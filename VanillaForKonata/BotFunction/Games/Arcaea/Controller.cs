using Konata.Core.Events.Model;
using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public static class Controller
    {
        public static MessageBuilder bind(string cmd, GroupMessageEvent e) {
            // /v arc bind 029143294
            try
            {
                string[] args = cmd.Split(' ', 4);
                if (args.Length < 4)
                {
                    return new MessageBuilder().Text("缺少参数");
                }
                var t = new BotFunction.Games.Arcaea.Arcaea.ArcaeaRecentInfo(
                    Arcaea.ArcaeaRecentInfo.buildPara(
                        Arcaea.ArcaeaRecentInfo.RecentParaType.user, args[3]
                        )
                    );
                t.GetInfoAsync().Wait();
                if (t.getRecentInfo().status != 0)
                {
                    return new MessageBuilder().Text("失败的绑定了你的Arcaea账号,错误码:" + t.getRecentInfo().status);
                }
                else {
                    var a = new Util.Database($"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.bindDB}");
                    if (a.Select("ArcBind",$"Uin={e.MemberUin}").Rows.Count!=0)
                    {
                        a.Delete("ArcBind", $"Uin={e.MemberUin}");
                    }
                    a.Insert("ArcBind", new KeyValuePair<string, string>[]{
                        new ("Uin",e.MemberUin.ToString()),
                        new("Code",t.getRecentInfo().content.account_info.code.ToString())
                    }) ;
                    return new MessageBuilder().Text("成功将您的账号绑定到了:" + t.getRecentInfo().content.account_info.name);
                }
            }
            catch (Exception ex )
            {

                return new MessageBuilder().Text(ex.ToString());
            }
        }
        public static MessageBuilder generalQueryEntry(string cmd, GroupMessageEvent e) {
            try
            {
                var a = new Util.Database($"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.bindDB}");
                
                // /v arc query iLLness LiLin Maximum
                string[] args = cmd.Split(' ', 3);// v arc query+xxxxxx

                if (args.Length<3)
                {
                    return null;
                }
                if (args[2].ToLower()=="query")
                {
                    var rs = a.Select("ArcBind", $"Uin={e.MemberUin}").Rows;
                    if (rs.Count == 0)
                    {
                        return new MessageBuilder().Text("在查询之前请绑定您的Arcaea账号");
                    }
                    string arcid = rs[0]["Code"].ToString();
                    return queryRecent(arcid);
                }
                else
                {
                    var rs = a.Select("ArcBind", $"Uin={e.MemberUin}").Rows;
                    if (rs.Count == 0)
                    {
                        return new MessageBuilder().Text("在查询之前请绑定您的Arcaea账号");
                    }
                    string arcid = rs[0]["Code"].ToString();
                    var infoArg=args[2].Split(' ', 2);
                    return queryBest(arcid,infoArg[1]);
                }
            }
            catch (Exception ex)
            {

                return new MessageBuilder().Text(ex.ToString());
            }
        }

        private static MessageBuilder queryBest(string? arcid, string v)
        {
            bool includeDiff = false;
            var args=v.Split(' ');//iLLness LiLin Maximum
            var diff = new string[] { "pst","prs","ftr","byd","past","present","future","beyond","蓝","绿","紫","红"};
            string diffId = "2";
            switch (args[args.Length - 1].ToLower())
            {
                case "pst":
                case "past":
                case "蓝":
                    diffId = "0";
                    includeDiff = true;
                    break;
                case "present":
                case "prs":
                case "绿":
                    diffId = "1";
                    includeDiff = true;
                    break;
                case "ftr":
                case "future":
                case "紫":
                    diffId = "2";
                    includeDiff = true;
                    break;
                case "byd":
                case "beyond":
                case "红":
                    diffId = "3";
                    includeDiff = true;
                    break ;
                default:
                    break;
            }
            int i = 0;
            string songid = "";
            foreach (var a in args)
            {
                if (i != args.Length - 1)
                {
                    songid += a;
                }
                else
                {
                    if (!includeDiff)
                    {
                        songid += a;
                    }
                }
                i+=1;
            }
            var t = new BotFunction.Games.Arcaea.Arcaea.ArcaeaSongBestInfo(
                Arcaea.ArcaeaSongBestInfo.buildPara(
                    Arcaea.ArcaeaSongBestInfo.UserIdType.user
                    , Arcaea.ArcaeaSongBestInfo.SongIdType.name
                    , arcid, songid, diffId)
                    );
            t.GetInfoAsync().Wait();
            return new BotFunction.Games.Arcaea.Pictures.BestImage(t.getSongBestInfo()).getBest().Result;
        }

        internal static MessageBuilder? best(string commandString, GroupMessageEvent e)
        {
            try
            {
                var a = new Util.Database($"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.bindDB}");
                var rs = a.Select("ArcBind", $"Uin={e.MemberUin}").Rows;
                if (rs.Count == 0)
                {
                    return new MessageBuilder().Text("在查询之前请绑定您的Arcaea账号");
                }
                var id=rs[0]["Code"].ToString();
                var t=new BotFunction.Games.Arcaea.Arcaea.ArcaeaBest30Info(
                    Arcaea.ArcaeaBest30Info.buildPara(
                        Arcaea.ArcaeaBest30Info.UserIdType.code
                        ,id,10,false)
                    );
                t.GetInfoAsync().Wait();
                return new BotFunction.Games.Arcaea.Pictures.Best30Image(t.getBest30Info()).B30().Result;
            }
            catch (Exception ex)
            {

                return new MessageBuilder().Text(ex.ToString());
            }
        }

        private static MessageBuilder queryRecent(string? arcid)
        {
            var t = new BotFunction.Games.Arcaea.Arcaea.ArcaeaRecentInfo(
                    Arcaea.ArcaeaRecentInfo.buildPara(
                        Arcaea.ArcaeaRecentInfo.RecentParaType.user, arcid
                        )
                    );
            t.GetInfoAsync().Wait();
            return new Pictures.RecentImage(t.getRecentInfo()).getRecent().Result;
        }
        public static string getNickCommand(string commandStringO)
        {
            string commandString = commandStringO.ToLower();
            

            if (commandString == "/a")
            {
                return "/v arc query";
            }


            if (commandString == "/b30"
                || commandString == "/b40"
                || commandString == "/a b40"
                || commandString == "/a b30"
                || commandString == "/arc b40"
                || commandString == "/arc b30"
                || commandString == "/ab"
                || commandString == "/a best")
            {
                return "/v arc best";
            }
            if (commandString.StartsWith("/a info ")
                || commandString.StartsWith("/arc info "))
            {
                string[] cmd = commandString.Split(' ', 3);
                if (cmd.Length == 3)
                {
                    return $"/v arc query {cmd[2]}";
                }
            }
            if (commandString.StartsWith("/a bind ")
                || commandString.StartsWith("/arc bind "))
            {
                string[] cmd = commandString.Split(' ', 3);
                if (cmd.Length == 3)
                {
                    return $"/v arc bind {cmd[2]}";
                }
            }

            if (commandString.StartsWith("/a "))
            {
                string[] cmd = commandString.Split(' ', 2);
                return $"/v arc query {cmd[1]}";
            }

            if (commandString.StartsWith("/arc"))
            {
                string[] cmd = commandString.Split(' ', 2);
                if (cmd.Length > 1)
                {
                    if (cmd[1] == "best")
                    {
                        return "/v arc best";
                    }
                    else
                    {
                        return "/v arc query " + cmd[1];
                    }
                }
                else
                {
                    return "/v arc query";
                }
            }

            return commandStringO;
        }
    }
}
