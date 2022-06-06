using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        static unsafe public MessageBuilder main(string p1) {
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
    }
}
