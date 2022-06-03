using Konata.Core;
using Konata.Core.Events.Model;
using Konata.Core.Interfaces.Api;
using Konata.Core.Message;
using Konata.Core.Message.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaForKonata.BotInternal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace VanillaForKonata
{
    public static partial class GroupMessage
    {
        internal static void Main(object? sender, GroupMessageEvent e, Bot bot)
        {
            try
            {
                

                if (e.MemberUin == bot.Uin)
                    return;
                if (e.Message == null)
                    return;
                (bool,MessageBuilder) Reply;
                Reply.Item1 = false;
                Reply.Item2 = null;

                if (BotFunction.Sys.AntiAbuse.IsAbused(e.MemberUin))
                    return;

                
                string ImmersionStatus = BotInternal.ImmersionMode.ReadImmersion(e.MemberUin,"group");
                string commandString = e.Message.Chain.ToString();
                if (commandString=="/v clear immersion")
                {
                    ImmersionStatus = "null";
                    BotInternal.ImmersionMode.WriteImmersion(e.MemberUin.ToString(),"group","null");
                    Reply.Item2 = new MessageBuilder().Text("已清除");
                }
                if (ImmersionStatus!="null")
                {
                    try
                    {
                        var immsJson=(JObject)JsonConvert.DeserializeObject(ImmersionStatus);
                        if (immsJson["key"].ToString()=="help")
                        {
                            Reply.Item2 = BotFunction.Sys.Help.Main(e, ImmersionStatus);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                    
                    
                }
                else
                {
                    
                    commandString = NickCommand(commandString);
                    var textChain = e.Message.Chain.GetChain<TextChain>();

                    //Module Sys
                    if (commandString == "/ping")
                        Reply.Item2 = BotFunction.Sys.Ping();
                    else if (commandString == "/v restart another")
                        Reply.Item2 = BotFunction.Sys.RestartAnotherBot();
                    else if (commandString.StartsWith("/v module "))
                        Reply.Item2 = BotFunction.Sys.Switches.SwitchMain(e, bot);
                    else if (commandString == "/v stat")
                        Reply.Item2 = BotFunction.Sys.Stat.GetStat();
                    else if (commandString == "*test func")
                        Reply.Item2 = TempFunc();
                    //Module SDVX
                    else if (commandString.StartsWith("/v sdvx info ") && CanBeUse.test("sdvx", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Games.SDVX.Main(e, commandString, bot));
                    else if (commandString.StartsWith("/v sdvx song ") && CanBeUse.test("sdvx", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Games.SDVX.SendSound(e, commandString, bot));
                    //Module Arcaea
                    else if (commandString == "/v arc update" && CanBeUse.test("arcaea", e))
                        Reply.Item2 = BotFunction.Games.Arcaea.AutoUpdate.UpdateArcaea(bot, e);
                    else if (commandString.StartsWith("/v arc query") && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot,e,BotFunction.Games.Arcaea.Controller.generalQueryEntry(commandString, e)));
                    else if (commandString.StartsWith("/v arc bind ") && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot,e,BotFunction.Games.Arcaea.Controller.bind(commandString, e)));
                    else if (commandString == "/v arc best" && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot, e, BotFunction.Games.Arcaea.Controller.best(commandString, e)));
                    //Module Tool
                    else if (commandString.StartsWith("/v choose ") && CanBeUse.test("选择", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.ChooseForMe.Choose(commandString));
                    else if (commandString.StartsWith("/v dragon encode ") && CanBeUse.test("龙吟", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.DragonEncoding.Encode(commandString));
                    else if (commandString.StartsWith("/v dragon decode ") && CanBeUse.test("龙吟", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.DragonEncoding.Decode(commandString));
                    //Function Of Immersion
                    else if (commandString == "/help")
                        Reply.Item2 = BotFunction.Sys.Help.Start(e);
                    else if (commandString == "龙图来" && CanBeUse.test("龙图", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Pictures.DragonPicture());
                    //Module Auto
                    else if (Util.Rand.CanIDo(0.05f) && CanBeUse.test("复读", e))
                        Reply.Item2 = BotFunction.Sys.Repeat(e.Message.Chain);


                   
                }
                if (Reply.Item2 != null)
                    bot.SendGroupMessage(e.GroupUin, Reply.Item2);
                if (Reply.Item1)
                {
                    //CounterAdd
                    BotFunction.Sys.AntiAbuse.Counter(e,bot);
                }
                return;
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        }

        private static MessageBuilder? TempFunc()
        {


            return null;
            
        }

        private static string NickCommand(string commandString)
        {
            commandString = BotFunction.Games.Arcaea.Controller.getNickCommand(commandString);
            return commandString;
        }
        private static (bool,MessageBuilder) AntiAbuseCounterAdder(MessageBuilder m) {
            (bool, MessageBuilder) r;
            r.Item1 = true;
            r.Item2 = m;
            if (m==null)
            {
                r.Item1 = false;
            }
            return r;
        
        }
        private static MessageBuilder addAt(Bot bot, GroupMessageEvent e, MessageBuilder p) 
            => new MessageBuilder().At(e.MemberUin) + p;
    }
}
