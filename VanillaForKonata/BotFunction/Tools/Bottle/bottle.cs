using Konata.Core.Events.Model;
using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Tools.Bottle
{
    static public class bottleController
    {
        static public string getNick(string v1) {
            var v2 = v1.ToLower();
            if (v2.StartsWith("丢漂流瓶 "))
            {
                return $"/v bottle throw {v1.Substring("丢漂流瓶".Length)}";
            }
            else if (v2.StartsWith("/v bottle throw")) {
                return $"/v bottle throw {v1.Substring("/v bottle throw".Length)}";
            }
            else if (v2=="捞漂流瓶")
            {
                return "/v bottle pick";
            }
            else if (v2 == "/v bottle pick")
            {
                return "/v bottle pick";
            }
            else
            {
                return v1;
            }
        }
        static private Bottle openBox() {
            Util.Database diskDestroyer = new($"{GlobalScope.Path.DatabasePath}\\Bottles");
            var table=diskDestroyer.Select("bottles").Select();
            List<System.Data.DataRow> r = new(from x in table where (string)x["enable"] != "false" select x);
            int count=r.Count();
            var index = Util.Rand.GetRandomNumber(0,count-1);
            return new Bottle { 
                content =r[index]["content"].ToString(),
                Time = r[index]["time"].ToString(),
                fromGroup=r[index]["fromgroup"].ToString(),
                fromMember=r[index]["frommember"].ToString(),
                enable=r[index]["enable"].ToString()
            };

        }
        static private void fuckBox(string v, uint groupUin, uint memberUin, string v1) {
            Util.Database diskDestroyer = new($"{GlobalScope.Path.DatabasePath}\\Bottles");
            diskDestroyer.Insert("bottles",new KeyValuePair<string, string>[] {
                new( "time",v1),
                new("content",v),
                new("fromgroup",groupUin.ToString()),
                new("frommember",memberUin.ToString()),
                new ("enable","true")
            });
        }
        static public void initBox() {
            if (!File.Exists($"{GlobalScope.Path.DatabasePath}\\Bottles"))
            {
                Util.Database diskDestroyer = new($"{GlobalScope.Path.DatabasePath}\\Bottles");
                diskDestroyer.Create("bottles","time","content","fromgroup","frommember","enable");
            }
        }

        internal static MessageBuilder main(string commandString, GroupMessageEvent e)
        {
            //v bottle fuck ctt
            //v bottle pussy
            string[] args = commandString.Split(' ', 3);
            if (args.Length==3)
            {
                if (args[2].ToLower()=="pick")
                {
                    var bt = openBox();
                    return new MessageBuilder().Text($"[Bottle]\nAccount:{bt.fromMember}\nTime:{bt.Time}\nContent:\n").Add(bt.getMessage().Build());
                }
                else
                {
                    var aarg=args[2].Split(' ', 2);
                    if (aarg.Length != 2) {

                        return null;
                    }
                    else
                    {
                        if (aarg[0]!="throw")
                        {
                            return null;
                        }
                        else
                        {
                            fuckBox(aarg[1], groupUin: e.GroupUin, memberUin: e.MemberUin, v1: DateTime.Now.ToString("G"));
                            return new MessageBuilder().Text("成功地将瓶子丢到了水中");
                        }
                    }
                }
            }
            return null;
        }
    }
    public class Bottle {
        public string content;
        public string fromMember;
        public string fromGroup;
        public string Time;
        public string enable;
        public MessageBuilder getMessage()
            => MessageBuilder.Eval(content);
    }
}
