using Konata.Core.Interfaces.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction
{
    partial class Sys
    {
        static public class AntiAbuse
        {
            internal static bool IsAbused(uint MemberUin)
            {
                if (UsersData.Abuser.ContainsKey(MemberUin.ToString()))
                {
                    return true;
                }
                if (!UsersData.AntiAbuseCounter.ContainsKey(MemberUin.ToString()))
                {
                    return false;
                }
                if (UsersData.AntiAbuseCounter[MemberUin.ToString()]>GlobalScope.Cfgs.AbuseLimit)
                {
                    return true;
                }
                return false;
            }

            internal static void Counter(Konata.Core.Events.Model.GroupMessageEvent e, Konata.Core.Bot bot)
            {
                if (UsersData.AntiAbuseCounter.ContainsKey(e.MemberUin.ToString()))
                {
                    UsersData.AntiAbuseCounter[e.MemberUin.ToString()] += 1;
                }
                else
                {
                    UsersData.AntiAbuseCounter.Add(e.MemberUin.ToString(), 1);
                    Task.Run(() => {
                        Thread.Sleep((int)GlobalScope.Cfgs.AbuseClearTime);
                        
                        if (UsersData.AntiAbuseCounter.ContainsKey(e.MemberUin.ToString()))
                        {
                            UsersData.AntiAbuseCounter.Remove(e.MemberUin.ToString());
                        }

                    });
                }
                if (UsersData.Abuser.ContainsKey(e.MemberUin.ToString()))
                {
                    return;
                }
                if (UsersData.AntiAbuseCounter[e.MemberUin.ToString()] > GlobalScope.Cfgs.AbuseLimit)
                {
                    UsersData.Abuser.Add(e.MemberUin.ToString(), "sb");
                    UsersData.AntiAbuseCounter[e.MemberUin.ToString()] = 0;
                    Task.Run(() => {
                        Thread.Sleep((int)GlobalScope.Cfgs.AbuseClearTime);
                        if (UsersData.Abuser.ContainsKey(e.MemberUin.ToString()))
                        {
                            UsersData.Abuser.Remove(e.MemberUin.ToString());
                        }
                        if (UsersData.AntiAbuseCounter.ContainsKey(e.MemberUin.ToString()))
                        {
                            UsersData.AntiAbuseCounter.Remove(e.MemberUin.ToString());
                        }

                    });
                    bot.SendGroupMessage(e.GroupUin,new Konata.Core.Message.MessageBuilder().Text("触发防滥用了，待会再来罢"));
              
                }

            }
        }
    }
}
