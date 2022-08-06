using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Games.mai
{
    internal class maimaidx
    {
        MaiObject getMaiObject(string type, string postdata, bool b50)
        {
            var a = FetchJson("qq", "1981131648", false).Result;
            JObject jo = (JObject)JsonConvert.DeserializeObject(a);
            var res = jo.ToObject<MaiObject>();
            return res;

        }
        private static async Task<string> FetchJson(string type, string postdata, bool b50)
        {
            HttpClient Client = new();
            var submitData = new Dictionary<string, object>();
            if (type == "qq")
            {
                submitData = new Dictionary<string, object>
                {
                    {"qq", int.Parse(postdata)},
                    { "b50",b50}
                // {"qq", uin}
                };
            }
            else
            {
                submitData = new Dictionary<string, object>
                {
                    {"username", postdata},
                    { "b50",b50}
                // {"qq", uin}
                };
            }

            var data = JsonConvert.SerializeObject(submitData);
            var url = $"https://www.diving-fish.com/api/maimaidxprober/query/player";
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            request.Content = new StringContent(data);
            request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var response = (await Client.SendAsync(request)).EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            return responseString;
        }

    }
}
