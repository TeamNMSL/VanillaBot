using Konata.Core.Message;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Common.Profile;
using TencentCloud.Nlp.V20190408;
using TencentCloud.Nlp.V20190408.Models;

namespace VanillaForKonata.BotFunction
{
    static class AutoReply
    {
        static ClientProfile clientProfile = new ClientProfile();
        static HttpProfile httpProfile = new HttpProfile();
        static NlpClient client;
        static public void init() {
            string sid = File.ReadAllText($"{GlobalScope.Path.configs}\\tencent.cloud.sid").Replace("\n","").Replace("\r","");
            string skey = File.ReadAllText($"{GlobalScope.Path.configs}\\tencent.cloud.skey").Replace("\n", "").Replace("\r", "");
            Credential cred = new Credential
            {
                SecretId = sid,
                SecretKey = skey
            };

            client = new NlpClient(cred, "ap-guangzhou", clientProfile);
            httpProfile.Endpoint = ("nlp.tencentcloudapi.com");
            clientProfile.HttpProfile = httpProfile;
        }
        static public MessageBuilder autoReply(string text, Konata.Core.Bot bot) {
            if (text.StartsWith(GlobalScope.Cfgs.BotName))
            {
                text = text.Substring(GlobalScope.Cfgs.BotName.Length);
            }
            if (text.StartsWith($"[KQ:at,qq={bot.Uin}]"))
            {
                text = text.Replace($"[KQ:at,qq={bot.Uin}] ", "").Replace($"[KQ:at,qq={bot.Uin}]", "");
            }
            if (text.Contains("[")||text.Contains("]"))
            {
                return null;
            }
            
            ChatBotRequest req = new ChatBotRequest();
            req.Query = text;
            ChatBotResponse resp = client.ChatBotSync(req);
            try
            {
                var r=AbstractModel.ToJsonString(resp);
                JObject jo = (JObject)JsonConvert.DeserializeObject(r);
                return new MessageBuilder().Text(jo["Reply"].ToString().Replace("腾讯小龙女",GlobalScope.Cfgs.BotName));
            }
            catch (Exception)
            {

                return null;
            }
            
        }
       
    }
}
