using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public partial class Arcaea
    {
        
        public class ArcaeaNetworkQueryInfo {
            public static bool IsReturned=false;
            public byte[] QueryResult;
            public JObject Result;
            protected HttpClient client;
            protected static string APIRoutine;
            protected static string UA;
            public  byte[] getByte()
                => client.GetByteArrayAsync(APIRoutine).Result;
            public async Task<bool> GetInfoAsync()
            {
                
                byte[]? result = await client.GetByteArrayAsync(APIRoutine);
                
                try
                {
                    if (result == null)
                    {
                        IsReturned = false;
                        return false;
                    }
                    QueryResult = result;
                    Console.WriteLine(Encoding.UTF8.GetString(result));
                    Result = (JObject)JsonConvert.DeserializeObject(Encoding.UTF8.GetString(result));
                    IsReturned = true;
                    return true;
                }
                catch (Exception)
                {
                    IsReturned = false;
                    return false;
                }
            }
            protected void Init() {
                UA = GlobalScope.Arc.UserAgent;
                client = new();
                client.BaseAddress = new Uri("https://" + GlobalScope.Arc.Api + "/");
                client.DefaultRequestHeaders.Add("Authorization", UA);
            }
            public Stream GetStream() {
                return client.GetStreamAsync(client.BaseAddress+APIRoutine).Result;
            
            }
        }

        
    }
}
