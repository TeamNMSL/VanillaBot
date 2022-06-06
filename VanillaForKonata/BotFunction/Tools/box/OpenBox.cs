using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konata.Core.Interfaces.Api;

namespace VanillaForKonata.BotFunction.Tools.box
{
    public  interface IQQBox
    {
        public bool getQQ(ref string p0);
        public bool getPhone(ref string p0);
        public bool getArea(ref string p0);

    }
    static class QQBox {
        static public bool enable;
        static public string apiUrl = "";
        static public void init() {
            string v0 = $"{GlobalScope.Path.configs}\\openbox";
            if (File.Exists(v0))
            {
                apiUrl = File.ReadAllText(v0).Replace("\r", "").Replace("\n", "");
                enable = true;
            }
            else
            {
                File.Create(v0);
                enable = false;
            }
        
        }

        static public MessageBuilder main(string p1) {
            // /v openbox q||p <q|p>
            if (!enable)
            {
                return null;
            }
            string[] v0 = p1.Split(' ',4);
            if (v0.Length!=4)
            {
                return null;
            }
            if (v0[2] == "q")
            {
                var v1 =new models.Q2PhoneBoxModel(v0[3]).getBox();
                return new MessageBuilder().Text($"QQ:{v1.QQ}\n手机:{v1.Phone}\n归属地:{v1.Area}");
            }
            else if (v0[2] == "p")
            {
                var v1 = new models.Phone2QBoxModel(v0[3]).getBox();
                return new MessageBuilder().Text($"QQ:{v1.QQ}\n手机:{v1.Phone}\n归属地:{v1.Area}");
            }
            else {
                return null;
            }
        }
        static public (bool, MessageBuilder) randBox(Konata.Core.Events.Model.GroupMessageEvent p1, Konata.Core.Bot p2) {
            try
            {
                string sub_1145141919810(string p0) 
                    {
                        if (p0 == "没开到")
                        {
                            return p0;
                        }
                        else
                        {
                            var v6 = p0;
                            v6=v6.Substring(3, 4);
                            return p0.Replace(v6, "****");
                        }
                    }
                
                var v0 = p2.GetGroupMemberList(p1.GroupUin).Result;
                int v1 = v0.Count;
                int v2 = Util.Rand.GetRandomNumber(0, v1 - 1);
                var v3 = v0[v2];
                var v4 = new models.Q2PhoneBoxModel(v3.Uin.ToString()).getBox();
                var v5 = new MessageBuilder().Text($"恭喜这位群友即将出道！\nQQ:{v3.NickName}({v3.Uin})\n手机:{sub_1145141919810(v4.Phone)}\n归属地:{v4.Area}");
                p2.SendGroupMessage(p1.GroupUin,v5);
                return (true, null);
            }
            catch (Exception)
            {

                return (false, null);
            }

        }
    }
}
