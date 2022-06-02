using Konata.Core.Events.Model;
using VanillaForKonata.Util;

namespace VanillaForKonata.BotInternal
{
    public class ImmersionMode
    {
        public static void Initial()
        {
            Util.Database database = new Database($"{GlobalScope.Path.DatabasePath}\\ImmersionMode");
            database.Create("ImmersionModeSwitch","Uin", "Type", "status");
        }
        public static string ReadImmersion(uint uin,string type) {
            try
            {
                if (!UsersData.ImmersionMode.ContainsKey(uin.ToString()))
                {
                    UsersData.ImmersionMode.Add(uin.ToString(), ReadImmersionFromDatabase(uin.ToString(), type));
                    return ReadImmersion(uin, type);
                }
                return UsersData.ImmersionMode[uin.ToString()];
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                return "null";
            }
        }

        internal static bool IsImmersionMode(GroupMessageEvent e)
        {
            if (ReadImmersion(e.MemberUin,"group")=="null")
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private static string ReadImmersionFromDatabase(string uin, string type)
        {
            try
            {
                Database database = new Database($"{GlobalScope.Path.DatabasePath}\\ImmersionMode");
                var res = database.Select("ImmersionModeSwitch", $"type='{type}' and uin='{uin}'");
                if (res.Rows.Count <= 0)
                {
                    WriteImmersion(uin, type, "null");
                    return "null";
                }
                else
                {
                    
                    return res.Rows[0]["status"].ToString();
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                return "null";
            }
        }

        public static void WriteImmersion(string uin, string type, string status)
        {
            try
            {
                Database database = new Database($"{GlobalScope.Path.DatabasePath}\\ImmersionMode");
                var res = database.Select("ImmersionModeSwitch", $"type='{type}' and uin='{uin}'");
                if (res.Rows.Count > 0)
                {
                    database.Delete("ImmersionModeSwitch", $"type='{type}' and uin='{uin}'");
                }
                database.Insert("ImmersionModeSwitch", new KeyValuePair<string, string>[] {
                new KeyValuePair<string, string>("uin",uin),
                new KeyValuePair<string, string>("type",type),
                new KeyValuePair<string, string>("status",status)
            });
                if (UsersData.ImmersionMode.ContainsKey(uin.ToString()))
                {
                    UsersData.ImmersionMode.Remove(uin.ToString());
                    ReadImmersion(uint.Parse(uin), type);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                
            }
        }

        internal static void AddImmersionTimer(uint memberUin, CancellationTokenSource cts)
        {
            if (isImmersionTaskExists(memberUin))
            {
               DeleteImmersionTimer(memberUin);
            }
            UsersData.ImmersionTimer.Add(memberUin.ToString(), cts);
        }
        internal static void DeleteImmersionTimer(uint member) {
            if (isImmersionTaskExists(member))
            {
                UsersData.ImmersionTimer[member.ToString()].Cancel();
                UsersData.ImmersionTimer.Remove(member.ToString());
            }
        }
        private static bool isImmersionTaskExists(uint uin) {
            if (UsersData.ImmersionTimer.ContainsKey(uin.ToString()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}