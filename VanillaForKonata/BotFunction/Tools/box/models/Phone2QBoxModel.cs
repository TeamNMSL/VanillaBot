using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Tools.box.models
{
    unsafe public class Phone2QBoxModel : IQQBox
    {
        string phone;
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

        public bool getQQ(ref string p0)
        {
            if (jo.ContainsKey("qq"))
            {
                var v0 = jo["qq"].ToString();
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
            p0 = phone;
            return true;
        }
        public Phone2QBoxModel(string p0)
        {
            phone = p0;
            query();
        }
        private void query()
        {
            var clinet = new HttpClient();
            clinet.BaseAddress = new Uri(BotFunction.Tools.box.QQBox.apiUrl);
            jo = (JObject)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(
                clinet.GetByteArrayAsync($"qqphone?phone={phone}").Result
                ));
        }
        public P2QBox getBox()
        {
            string qq = "没开到";
            string phone = "没开到";
            string area = "没开到";
            getQQ(ref qq);
            getArea(ref area);
            getPhone(ref phone);
            return new P2QBox()
            {
                QQ = qq,
                Phone = phone,
                Area = area
            };
        }
        public class P2QBox
        {
            public string QQ;
            public string Phone;
            public string Area;

        }
    }
}
