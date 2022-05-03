using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata
{
    public static class UsersData
    {
        static public Dictionary<string,Dictionary<string,string>> Switches=new();
        static public Dictionary<string,string> ImmersionMode=new();
        static public Dictionary<string,int> AntiAbuseCounter=new();
        static public Dictionary<string, string> Abuser = new();
    }
}
