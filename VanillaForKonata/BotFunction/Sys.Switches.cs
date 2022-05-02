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
                            if (BotInternal.CanBeUse.test(item, e))
                            {
                                list.Add($"{item}  On");
                            }
                            else
                            {
                                list.Add($"{item}  Off");
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
                    if (!GlobalScope.Cfgs.FunctionList.Contains(vs[1].ToLower()))
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
                        if (UsersData.Switches[e.GroupUin.ToString()][vs[1]] == "lon" && UsersData.Switches[e.GroupUin.ToString()][vs[1]] == "loff")
                        {
                            return new MessageBuilder()
                            .Text("修改失败，此功能开关被Bot管理团队锁定");
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

        }
    }
}
