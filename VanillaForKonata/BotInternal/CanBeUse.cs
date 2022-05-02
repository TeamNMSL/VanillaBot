using VanillaForKonata.Util;
using Konata.Core.Events.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotInternal
{
    public static class CanBeUse
    {
        
        public static bool test(string functionname, GroupMessageEvent e)
        {
            if (!UsersData.Switches.ContainsKey(e.GroupUin.ToString()))
            {
                var Switches = ReadSwitches(e.GroupUin, "Group");
                UsersData.Switches.Add(e.GroupUin.ToString(), Switches);
                
                return test(functionname,e);
            }
            if (!UsersData.Switches[e.GroupUin.ToString()].ContainsKey(functionname))
            {
                UsersData.Switches[e.GroupUin.ToString()].Add(functionname, "on");
                return true;
            }
            if (UsersData.Switches[e.GroupUin.ToString()][functionname]=="on"|| UsersData.Switches[e.GroupUin.ToString()][functionname] == "lon")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public static bool SaveSwitchStatus(Dictionary<string,string> switches,ulong uin,string type) {
            try
            {
                var functionlist = GlobalScope.Cfgs.FunctionList;
                foreach (var function in functionlist)
                {
                    if (!switches.ContainsKey(function))
                    {
                        switches.Add(function, "on");
                    }
                }
                Util.Database database = new Database($"{GlobalScope.Path.DatabasePath}\\Switches");
                if (database.Select("Switches", $"Uin='{uin}' and Type='{type}'").Rows.Count!=0)
                {
                    database.Delete("Switches", $"Uin='{uin}' and Type='{type}'");
                }
                database.Insert("Switches", new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("Uin",uin.ToString()),
                new KeyValuePair<string, string>("Type",type),
                new KeyValuePair<string, string>("Data",Base64.EncodeBase64(Encoding.ASCII,
                JsonConvert.SerializeObject(switches,new JsonSerializerSettings(){ StringEscapeHandling=StringEscapeHandling.EscapeNonAscii})))
            });
                ReadSwitches(uin,type);
                return true;
            }
            catch (Exception e)
            {

                Console.WriteLine(e.ToString());
                return false;
            }


        }
        public static Dictionary<string, string> ReadSwitches(ulong uin,string type) {
            Database database = new Database($"{GlobalScope.Path.DatabasePath}\\Switches");
            DataTable table = database.Select("Switches",$"Uin='{uin}' and Type='{type}'");
            if (table.Rows.Count==0)
            {
                var functionlist = GlobalScope.Cfgs.FunctionList;
                Dictionary<string, string> switches = new ();
                foreach (var function in functionlist)
                {
                    switches.Add(function,"on");
                }
                SaveSwitchStatus(switches, uin, type);
                return switches;
            }
            //return Dictionary<string, string>.Deserialize(Base64.DecodeBase64(Encoding.ASCII,table.Rows[0]["Data"].ToString()));
            return JsonConvert.DeserializeObject <Dictionary<string, string>>(Base64
                .DecodeBase64(Encoding.ASCII, table
                .Rows[0]["Data"]
                .ToString()));
        }

        internal static void Initial()
        {
            Util.Database database = new Database($"{GlobalScope.Path.DatabasePath}\\Switches");
            database.Create("Switches","Uin","Type","Data");

        }
    }
}
