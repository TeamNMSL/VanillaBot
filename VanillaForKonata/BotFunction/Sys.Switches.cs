using Konata.Core;
using Konata.Core.Events.Model;
using Konata.Core.Interfaces.Api;
using Konata.Core.Message;

namespace VanillaForKonata.BotFunction
{
    public static partial class Sys
    {

        public static class Switches
        {
            public static MessageBuilder SwitchMain(Konata.Core.Events.Model.GroupMessageEvent e, Konata.Core.Bot bot)
            {
                try
                {
                    BotInternal.CanBeUse.test("复读", e);
                    string cmd = e.Message.Chain.ToString().ToLower();
                    //   0            1
                    //  on|off|list [复读]
                    cmd = cmd.Replace("/v module ", "");
                    string[] vs = cmd.Split(' ');
                    if (vs.Length == 1 && vs[0] == "list")
                    {
                        string rpl = "[MODULE LIST]\n";
                        List<string> list = new List<string>();
                        foreach (var item in GlobalScope.Cfgs.FunctionList)
                        {
                            if (BotInternal.CanBeUse.test(item.Key, e))
                            {
                                list.Add($"{item.Key}  On");
                            }
                            else
                            {
                                list.Add($"{item.Key}  Off");
                            }

                        }
                        rpl += String.Join("\n", list.ToArray());
                        return new MessageBuilder()
                            .Text(rpl);
                    }
                    if (vs.Length < 2)
                    {
                        return new MessageBuilder()
                            .Text("修改失败，因为缺少参数\n/v module <on|off> <moduleName>");
                    }
                    if (vs[0].ToLower() != "on" && vs[0].ToLower() != "off")
                    {
                        if ((vs[0].ToLower() != "lon" && vs[0].ToLower() != "loff" && vs[0].ToLower() != "admin") || !GlobalScope.Cfgs.BotAdmins.Contains(e.MemberUin))
                        {
                            return new MessageBuilder()
                            .Text("修改失败，参数错误\n/v module <on|off> <moduleName>");
                        }
                        if (vs[0].ToLower() == "admin")
                        {
                            return AdminCmd(e, vs, bot);
                        }

                    }
                    if (!GlobalScope.Cfgs.FunctionList.ContainsKey(vs[1].ToLower()))
                    {

                        return new MessageBuilder()
                            .Text("修改失败，没有这个功能，请查看模块列表后进行修改\n/v module list");
                    }
                    
                    Dictionary<string, string> edit(string stat)
                    {
                        if (UsersData.Switches[e.GroupUin.ToString()].ContainsKey(vs[1]))
                        {
                            UsersData.Switches[e.GroupUin.ToString()][vs[1]] = stat;
                        }
                        else
                        {
                            UsersData.Switches[e.GroupUin.ToString()].Add(vs[1], stat);
                        }
                        return UsersData.Switches[e.GroupUin.ToString()];
                    }
                    if (UsersData.Switches[e.GroupUin.ToString()].ContainsKey(vs[1]))
                    {
                        if (UsersData.Switches[e.GroupUin.ToString()][vs[1]] == "lon" || UsersData.Switches[e.GroupUin.ToString()][vs[1]] == "loff")
                        {
                            if (!GlobalScope.Cfgs.BotAdmins.Contains(e.MemberUin))
                            {
                                return new MessageBuilder()
                            .Text("修改失败，此功能开关被Bot管理团队锁定");
                            }
                            
                        }
                    }
                    if (bot.GetGroupMemberInfo(e.GroupUin, e.MemberUin).Result.Role == Konata.Core.Common.RoleType.Member && !GlobalScope.Cfgs.BotAdmins.Contains(e.MemberUin))
                    {
                        return new MessageBuilder()
                           .Text("修改失败，开关仅该群群主或管理员能修改");
                    }
                    BotInternal.CanBeUse.SaveSwitchStatus(edit(vs[0].ToLower()), e.GroupUin, "Group");
                    return new MessageBuilder()
                        .Text("修改完毕");
                }
                catch (Exception ex)
                {
                    return new MessageBuilder()
                        .Text($"修改失败\n{ex.ToString()}");

                }
            }

            

            private static MessageBuilder AdminCmd(GroupMessageEvent e, string[] vs, Bot bot)
            {
                //admin setdefault||setall||lockthisall||unlockthisall
                
                switch (vs[1].ToLower())
                {                   
                    case "setall":
                        return setall(vs,bot);
                        
                    case "lockthisall":
                    case "unlockthisall":
                        return lockthisall(vs,e);
                    default:
                        return new MessageBuilder().Text("Unknown command");
                }

            }



            private static MessageBuilder setall(string[] vs,Bot bot)
            {
                //admin setall stat fn
                try
                {
                    Dictionary<string, string> edit(uint groupuin, string stat, string fname)
                    {
                        BotInternal.CanBeUse.test("复读",groupuin);
                        if (UsersData.Switches[groupuin.ToString()].ContainsKey(fname))
                        {
                            UsersData.Switches[groupuin.ToString()][fname] = stat;
                        }
                        else
                        {

                            UsersData.Switches[groupuin.ToString()].Add(fname, stat);
                        }
                        return UsersData.Switches[groupuin.ToString()];
                    }
                    var a = bot.GetGroupList();
                    Dictionary<string, string> b = new();
                    foreach (var item in a.Result)
                    {
                        BotInternal.CanBeUse.SaveSwitchStatus(edit(item.Uin, vs[2], vs[3]), item.Uin, "Group");
                    }
                    return new MessageBuilder().Text("Done");
                }
                catch (Exception ex)
                {

                    return new MessageBuilder().Text(ex.Message);
                }
            }

            private static MessageBuilder lockthisall(string[] vs, GroupMessageEvent e)
            {
                //admin lockthisall fn
                try
                {
                    
                    Dictionary<string, string> edit(uint groupuin, string stat,string fname)
                    {
                        BotInternal.CanBeUse.test("复读", groupuin);
                        if (UsersData.Switches[groupuin.ToString()].ContainsKey(fname))
                        {
                            UsersData.Switches[groupuin.ToString()][fname] = stat;
                        }
                        else
                        {

                            UsersData.Switches[groupuin.ToString()].Add(fname, stat);
                        }
                        return UsersData.Switches[groupuin.ToString()];
                    }
                    Dictionary<string, string> a = new();
                    if (vs[1].ToLower()=="lockthisall")
                    {
                        foreach (var item in GlobalScope.Cfgs.FunctionList)
                        {
                            if (BotInternal.CanBeUse.test(item.Key, e))
                            {
                                a = edit(e.GroupUin, "lon", item.Key);
                            }
                            else
                            {
                                a = edit(e.GroupUin, "loff", item.Key);
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in GlobalScope.Cfgs.FunctionList)
                        {
                            if (BotInternal.CanBeUse.test(item.Key, e))
                            {
                                a = edit(e.GroupUin, "on", item.Key);
                            }
                            else
                            {
                                a = edit(e.GroupUin, "off", item.Key);
                            }
                        }
                    }
                    
                    BotInternal.CanBeUse.SaveSwitchStatus(a, e.GroupUin, "Group");
                    return new MessageBuilder().Text("Success");
                }
                catch (Exception ex)
                {

                    return new MessageBuilder().Text($"Fail:\n{ex.Message}");
                }
                

            }
        }
    }
}
