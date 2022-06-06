using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Tools.box.models
{
    unsafe public class Q2PhoneBoxModel : IQQBox
    {
        string qq;
        JObject jo;
        public bool getArea(ref string p0)
        {
            if (jo.ContainsKey("phonediqu"))
            {
                var v0 = jo["phonediqu"].ToString();
                if (v0 != null)
                {
                    p0 = v0;
                    return true;
                }
            }
            return false;
        }

        public bool getPhone(ref string p0)
        {
            if (jo.ContainsKey("phone"))
            {
                var v0 = jo["phone"].ToString();
                if (v0!=null)
                {
                    p0 = v0;
                    return true;
                }
            }
            return false;
        }

        public bool getQQ(ref string p0)
        {
            p0 = qq;
            return true;
        }
        public Q2PhoneBoxModel(string p0) { 
            qq = p0;
            query();
        }
        private void query() {
            var clinet = new HttpClient();
            clinet.BaseAddress = new Uri(BotFunction.Tools.box.QQBox.apiUrl);
            jo = (JObject)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(
                clinet.GetByteArrayAsync($"qqapi?qq={qq}").Result
                ));
        }
        public Q2PBox getBox() {
            string qq = "没开到";
            string phone = "没开到";
            string area = "没开到";
            getQQ(ref qq);
            getArea(ref area);
            getPhone(ref phone);
            return new Q2PBox()
            {
                QQ = qq,
                Phone = phone,
                Area = area
            };
        }
        public class Q2PBox {
            public string QQ;
            public string Phone;
            public string Area;
        
        }
    }
}
