using Konata.Core;
using Konata.Core.Events.Model;
using Konata.Core.Interfaces.Api;
using Konata.Core.Message;
using Konata.Core.Message.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaForKonata.BotInternal;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net;
using PuppeteerSharp;

namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public static class AutoUpdate
    {
        public static async Task<string> downloadLink()
        {
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
                
            }))
            {
                
                using (var page = await browser.NewPageAsync())
                {
                    var a = page.GoToAsync((@"https://arcaea.lowiro.com/zh")).Result;
                    var div = page.XPathAsync("/html/body/div/div/div/div[2]/div/a").Result[0];
                    
                    return div.GetPropertyAsync("href").Result.JsonValueAsync<string>().Result;
                }
                

            }
        }
        public static MessageBuilder UpdateArcaea(Konata.Core.Bot bot, Konata.Core.Events.Model.GroupMessageEvent e) {
            bot.SendGroupMessage(e.GroupUin,
                new MessageBuilder()
                .Text("[Arcaea Update]Info:Get download link"));
            string dl = downloadLink().Result;
            if (dl==null)
            {
                bot.SendGroupMessage(e.GroupUin,
                new MessageBuilder()
                .Text("[Arcaea Update]Error:Get download link failed\nProcess have been stopped"));
                return null;
            }
            bot.SendGroupMessage(e.GroupUin,
                new MessageBuilder()
                .Text("[Arcaea Update]Info:Setting proxy"));
            using (WebClient client = new())
            {
                try
                {
                    try
                    {
                        var p = new WebProxy();
                        p.Address = new Uri("http://127.0.0.1:10809");
                        client.Proxy = p;
                    }
                    catch (Exception)
                    {
                        bot.SendGroupMessage(e.GroupUin,
                    new MessageBuilder()
                    .Text("[Arcaea Update]Warn:Setting proxy failed"));

                    }
                    finally
                    {
                        bot.SendGroupMessage(e.GroupUin,
                            new MessageBuilder()
                            .Text("[Arcaea Update]Info:Downloading started"));
                        var res = client.DownloadData(new Uri(dl));
                        File.WriteAllBytes($"{GlobalScope.Path.Arcaea}\\Arcaea.zip", res);
                        res = null;
                        bot.SendGroupMessage(e.GroupUin,
                            new MessageBuilder()
                            .Text("[Arcaea Update]Done,Please unpack manually"));
                    }



                }
                catch (Exception ex)
                {
                    bot.SendGroupMessage(e.GroupUin,
                    new MessageBuilder()
                    .Text($"[Arcaea Update]Error:{ex.Message}"));
                    return null;

                }

                return null;
            }
            
            
        }
    }
}
