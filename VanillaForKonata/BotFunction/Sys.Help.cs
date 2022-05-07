using Konata.Core.Events.Model;
using Konata.Core.Message;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VanillaForKonata.BotInternal;

namespace VanillaForKonata.BotFunction
{
    public static partial class Sys
    {
        public static partial class Help {

            public static MessageBuilder Start(Konata.Core.Events.Model.GroupMessageEvent e) {
                ImmersionMode.WriteImmersion(e.MemberUin.ToString(), "group", ImmersionJsonBuilder("\\"));
                return Main(e, ImmersionJsonBuilder("\\"));
            }
            private static void SetStat(Konata.Core.Events.Model.GroupMessageEvent e, string type) {
                ImmersionMode.WriteImmersion(e.MemberUin.ToString(), "group", type);
            }
            public static MessageBuilder Main(Konata.Core.Events.Model.GroupMessageEvent e, string immersionStatus) {

                //return OldMode(e, immersionStatus);
                return NewMode(e, immersionStatus);
            }
            private static string ImmersionJsonBuilder(string text) {
                var jo=new JObject();
                jo.Add(new JProperty("key", "help"));
                jo.Add(new JProperty("val", text));
                return jo.ToString();
            }
            private static Dictionary<string,string> HelpReader(string path) {
                using (System.IO.StreamReader file = System.IO.File.OpenText(path))
                {
                    using (JsonTextReader reader = new JsonTextReader(file))
                    {
                        JObject jo = (JObject)JToken.ReadFrom(reader);
                        return new Dictionary<string, string> {
                    {"Title",jo["title"].ToString() },
                    { "Context",jo["context"].ToString()},
                    { "Bottom",jo["bottom"].ToString()},
                    { "Type",jo["type"].ToString()}
                    };
                    }

                }
                   
            }
            private static MessageBuilder NewMode(GroupMessageEvent e, string immersionStatus)
            {
                /*
                    {
                        "key":"help",
                        "val":"\"
                    }
                 */
                
                var cmd = e.Message.Chain.ToString();
                if (cmd == "exit")
                {
                    SetStat(e, "null");
                    return new MessageBuilder()
                        .Text("已退出帮助模式，Bot会正常响应指令了");
                }
                if (cmd == ".")
                {
                    SetStat(e, ImmersionJsonBuilder("\\"));
                    cmd = "/help";
                    immersionStatus = ImmersionJsonBuilder("\\");
                }
                string val = ((JObject)JsonConvert.DeserializeObject(immersionStatus))["val"].ToString();


                if (cmd=="/help")
                {
                    var content=HelpReader($"{GlobalScope.Path.Manual}{val}index.json");
                    return BuildHelpMessage(content["Title"],content["Context"],content["Bottom"]).Text("看完帮助记得输入exit不然bot不会响应任何指令");
                }
                else
                {
                    Dictionary<string, string> c = new Dictionary<string, string>();
                    string bpath = $"{GlobalScope.Path.Manual}{val}";
                    if (Directory.Exists($"{bpath}{cmd}"))
                    {
                         c = HelpReader($"{bpath}{cmd}\\index.json");
                        SetStat(e, ImmersionJsonBuilder($"{val}{cmd}\\"));
                    }
                    else if(File.Exists($"{bpath}{cmd}.json"))
                    {
                         c = HelpReader($"{bpath}{cmd}.json");
                    }
                    else
                    {
                        return null;
                    }
                    return BuildHelpMessage(c["Title"], c["Context"], c["Bottom"]);
                }
            }
            #region OldShitMountain
            private static MessageBuilder OldMode(GroupMessageEvent e, string immersionStatus)
            {
                var cmd = e.Message.Chain.ToString();
                if (cmd == "exit")
                {
                    SetStat(e, "null");
                    return new MessageBuilder()
                        .Text("已退出帮助模式，Bot会正常响应指令了");
                }
                if (cmd == ".")
                {
                    SetStat(e, "help");
                    cmd = "/help";
                    immersionStatus = "help";
                }
                if (immersionStatus == "help")
                {
                    if (cmd == "/help")
                    {
                        return BuildHelpMessage("Vanilla", "1.帮助\n2.词典\n3.关于", "直接输入选项前面的序号就可以查阅对应帮助哦\n输入exit可以退出帮助页面，退出前bot不会响应一切指令");
                    }

                    else if (cmd == "1")
                    {
                        //帮助文本
                        SetStat(e, "help.1");
                        return HelpPicker.Help_1;
                    }
                    else if (cmd == "2")
                    {
                        //词典
                        return new MessageBuilder()
                            .Text("还没写，敬请期待");
                    }
                    else if (cmd == "3")
                    {
                        //关于
                        return new MessageBuilder()
                            .Text("还没写，敬请期待");
                    }
                }
                else if (immersionStatus == "help.1")
                {
                    //帮助
                    if (cmd == "1")
                    {
                        SetStat(e, "help.1.1");
                        return HelpPicker.Help_1_1;
                    }
                    else if (cmd == "2")
                    {
                        SetStat(e, "help.1.2");
                        return HelpPicker.Help_1_2;
                    }
                    else if (cmd == "3")
                    {
                        SetStat(e, "help.1.3");
                        return HelpPicker.Help_1_3;
                    }
                }
                else if (immersionStatus == "help.2")
                {
                    //词典
                }
                else if (immersionStatus == "help.3")
                {
                    //关于
                }
                else if (immersionStatus == "help.1.1")
                {
                    //Help-帮助-System
                    if (cmd == "1")
                    {
                        return HelpPicker.Help_1_1_1;
                    }
                    else if (cmd == "2")
                    {
                        return HelpPicker.Help_1_1_2;
                    }
                    else if (cmd == "3")
                    {
                        return HelpPicker.Help_1_1_3;
                    }
                }
                else if (immersionStatus == "help.1.2")
                {
                    //Help-帮助-图
                    if (cmd == "1")
                    {
                        return HelpPicker.Help_1_2_1;
                    }
                }
                else if (immersionStatus == "help.1.3")
                {
                    //Help-帮助-小工具
                    if (cmd == "1")
                    {
                        return HelpPicker.Help_1_3_1;
                    }
                }
                return null;
            }
            #endregion
            public static MessageBuilder BuildHelpMessage(string title,string context,string tips) {
                byte[] res;
                using (PictureDrawer.Drawer drawer = new PictureDrawer.Drawer(PictureDrawer.Drawer.Themes.Ver1,PictureDrawer.Drawer.Direction.I)) {
                    drawer.DrawBack();
                    drawer.DrawTitle(title);
                    drawer.DrawContext(context);
                    drawer.DrawOtherText(tips);
                    res = drawer.Save();
                    drawer.Dispose();
                }
                return new MessageBuilder().Image(res);
            }
        }

        
    }
}
