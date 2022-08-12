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
                
                //Something must be done at first=================================
                if (e.MemberUin == bot.Uin)
                    return;
                if (e.Message == null)
                    return;
                (bool,MessageBuilder) Reply;
                Reply.Item1 = false;
                Reply.Item2 = null;

                if (BotFunction.Sys.AntiAbuse.IsAbused(e.MemberUin))
                    return;
                string commandString = e.Message.Chain.ToString();
                //Something to do before detect command===========================
                
                if (commandString.Contains("[KQ:at"))
                {
                    Task.Run(() => {
                        BotFunction.Tools.whoatme.checkAdd(e, bot);
                    });
                    
                }



                //Commands detection==============================================
                string ImmersionStatus = BotInternal.ImmersionMode.ReadImmersion(e.MemberUin,"group");
               
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
                    else if (commandString == "/v sdvx rand" && CanBeUse.test("sdvx", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Games.SDVX.randSong(bot, e));
                    //Module Arcaea
                    else if (commandString == "/v arc update" && CanBeUse.test("arcaea", e))
                        Reply.Item2 = BotFunction.Games.Arcaea.AutoUpdate.UpdateArcaea(bot, e);
                    else if (commandString.StartsWith("/v arc query") && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot, e, BotFunction.Games.Arcaea.Controller.generalQueryEntry(commandString, e)));
                    else if (commandString.StartsWith("/v arc bind ") && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot, e, BotFunction.Games.Arcaea.Controller.bind(commandString, e)));
                    else if (commandString.StartsWith("/v arc chart ") && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot, e, BotFunction.Games.Arcaea.Controller.chartPreview(commandString)));
                    else if (commandString == "/v arc best" && CanBeUse.test("arcaea", e))
                        Reply = AntiAbuseCounterAdder(addAt(bot, e, BotFunction.Games.Arcaea.Controller.best(commandString, e)));
                    //Module Tool
                    else if (commandString.StartsWith("/v choose ") && CanBeUse.test("选择", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.ChooseForMe.Choose(commandString));
                    else if (commandString.StartsWith("/v dragon encode ") && CanBeUse.test("龙吟", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.DragonEncoding.Encode(commandString));
                    else if (commandString.StartsWith("/v dragon decode ") && CanBeUse.test("龙吟", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.DragonEncoding.Decode(commandString));
                    else if (commandString == "/v openbox rand" && CanBeUse.test("openbox", e))
                        Reply = BotFunction.Tools.box.QQBox.randBox(e, bot);
                    else if (commandString.StartsWith("/v openbox ") && CanBeUse.test("openbox", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.box.QQBox.main(commandString));
                    else if (commandString == "/v fortune" && CanBeUse.test("运势", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.Fortune.main(e, bot));
                    else if (commandString == "/v whoatme" && CanBeUse.test("谁at我", e))
                        Reply = BotFunction.Tools.whoatme.send(e.GroupUin,e.MemberUin,bot);
                    //Module Bottle
                    else if (commandString.StartsWith("/v bottle ") && CanBeUse.test("bottle", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Tools.Bottle.bottleController.main(commandString, e));
                    //Function Of Immersion
                    else if (commandString == "/help")
                        Reply.Item2 = BotFunction.Sys.Help.Start(e);
                    else if (commandString == "龙图来" && CanBeUse.test("龙图", e))
                        Reply = AntiAbuseCounterAdder(BotFunction.Pictures.DragonPicture());
                    else if ((commandString.StartsWith($"[KQ:at,qq={bot.Uin}]" )||commandString.StartsWith(GlobalScope.Cfgs.BotName)) && CanBeUse.test("自动回复", e))
                        Reply.Item2 = BotFunction.AutoReply.autoReply(e.Message.Chain.ToString(),bot);
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
            commandString = BotFunction.Tools.Bottle.bottleController.getNick(commandString);
            commandString = BotFunction.Tools.Fortune.getNick(commandString);
            commandString = BotFunction.Tools.whoatme.getNickcommand(commandString);
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
