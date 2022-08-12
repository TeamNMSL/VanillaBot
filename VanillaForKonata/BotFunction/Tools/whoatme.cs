using Konata.Core;
using Konata.Core.Interfaces.Api;
using Konata.Core.Events.Model;
using Konata.Core.Message.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konata.Core.Message;

namespace VanillaForKonata.BotFunction.Tools
{
    static public class whoatme
    {
        static readonly private string tableName = "AtTable";
        static readonly private string groupUin = "groupUin";
        static readonly private string memberUin = "memberUin";
        static readonly private string beAtMemberUin = "beAtMemberUin";
        static readonly private string messageCode = "messageCode";
        static readonly private string time = "time";
        public static void Init() {
            if (!File.Exists($"{GlobalScope.Path.DatabasePath}\\AtDB"))
            {
                Util.Database db = new($"{GlobalScope.Path.DatabasePath}\\AtDB");
                db.Create(tableName,groupUin,memberUin,beAtMemberUin,messageCode,time);
            }
            
        }
        public static void checkAdd(GroupMessageEvent e, Bot bot)
        {
            List<uint> atlist = new();
            Konata.Core.Message.Model.AtChain atChain=null;
            foreach (var chain in e.Message.Chain)
            {
                if (chain.Type==Konata.Core.Message.BaseChain.ChainType.At)
                {
                    atChain = (Konata.Core.Message.Model.AtChain)chain;
                    if (!atlist.Contains(atChain.AtUin))
                    {
                        recordAt(e.GroupUin, e.MemberUin, atChain, e);
                        atlist.Add(atChain.AtUin);
                    }
                    
                }
            }
        }

        private static void recordAt(uint groupUin, uint memberUin, AtChain atChain, GroupMessageEvent e)
        {   
            string time = e.EventTime.ToString();
            string messageCode = e.Message.Chain.ToString();
            string beAtMember = atChain.AtUin.ToString();
            Util.Database db = new($"{GlobalScope.Path.DatabasePath}\\AtDB");
            db.Insert(
                tableName,
                new[] {
                    new KeyValuePair<string,string>(
                        whoatme.groupUin ,groupUin.ToString()),
                    new KeyValuePair<string,string>(
                        whoatme.memberUin,memberUin.ToString()),
                    new KeyValuePair<string, string>(
                        whoatme.beAtMemberUin ,beAtMember),
                    new KeyValuePair<string, string>(
                        whoatme.messageCode,messageCode),
                    new KeyValuePair<string, string>(
                        whoatme.time,time)
                });
        }
        public static string getNickcommand(string ori) {
            if (ori.ToLower() == "/v whoatme")
                return "/v whoatme";
            if (ori == "谁at我")
                return "/v whoatme";
            return ori;
        }
        public static (bool,MessageBuilder) send(uint groupuin,uint memberuin, Bot bot) {
            Util.Database db = new($"{GlobalScope.Path.DatabasePath}\\AtDB");
            var list=db.Select(tableName, $"{groupUin}={groupuin} and {beAtMemberUin}={memberuin}").Rows;
            db.Delete(tableName, $"{groupUin}={groupuin} and {beAtMemberUin}={memberuin}");
            Konata.Core.Message.Model.MultiMsgChain messages = new();
            try
            {
                var a = list[0];
                foreach (System.Data.DataRow single in list)
                {
                    uint aterUin = uint.Parse(single[memberUin].ToString());
                    (uint, string) aa = (aterUin, bot.GetGroupMemberInfo(groupuin, aterUin).Result.NickName);
                    var c = MessageBuilder.Eval(single[messageCode].ToString()).Build();
                    messages.Add((aa, c));//smjb
                }
                bot.SendGroupMessage(groupuin, messages);
                return (false,null);
            }
            catch (Exception)
            {

                return (false,new MessageBuilder().Text("没人at你"));
            }
            
        }
        static DateTime getTime(string timestr)
            => DateTime.Parse(timestr);
    }
}
