using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PuppeteerSharp;
using Konata.Core.Events.Model;
using Konata.Core;
using Xunkong.Hoyolab.Avatar;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace VanillaForKonata.BotFunction.Games.Genshin
{
    static public class Genshin
    {
        static public string bindDbPath = "";
         
         static public async Task<bool> queryGenshinResult(string uid,string targetChar,string savename){
            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true
            }))
            {
                using (var page = await browser.NewPageAsync())
                {
                    try
                    {
                        await page.SetJavaScriptEnabledAsync(true);
                        await page.SetViewportAsync(new ViewPortOptions
                        {
                            Width = 1920,
                            Height = 1080
                        });
                        await page.GoToAsync($"https://enka.network/u/{uid}");
                        //   
                        await page.WaitForXPathAsync("/html/body/main/content/div[1]/div/figure/img");

                        var lang = page.MainFrame.XPathAsync("/html/body/main/header/menu/div[2]").Result;
                        if (lang.Length < 1)
                        {
                            return false;
                        }
                        if (targetChar == "#bind")
                        {
                            return true;
                        }
                        await lang[0].ClickAsync();
                        await page.WaitForXPathAsync("/html/body/main/header/menu/div[2]/div[2]/i[14]");
                        var zhcn = page.MainFrame.XPathAsync("/html/body/main/header/menu/div[2]/div[2]/i[14]").Result;
                        if (zhcn.Length < 1)
                        {
                            return false;
                        }
                        await zhcn[0].ClickAsync();

                        //var clst = page.MainFrame.QuerySelectorAllAsync("html body main.svelte-v08ffs.dark content.svelte-fi8pxd div.CharacterList.fix.svelte-1kjx8ue div.avatar.svelte-1kjx8ue");
                        var charlist = page.MainFrame.XPathAsync("/html/body/main/content/div[2]/div").Result;
                        bool isFind = false;
                        foreach (var it in charlist)
                        {
                            var a = it.GetPropertiesAsync().Result;

                            if (a.Count != 1)
                            {
                                await it.ClickAsync();
                                var cn = page.MainFrame.XPathAsync("/html/body/main/content/div[3]/div[1]/div/div/div[2]/div[2]/b").Result;
                                if (cn.Length >= 1)
                                {
                                    var n = cn[0].GetPropertyAsync("innerHTML").Result.JsonValueAsync().Result.ToString();
                                    if (n == targetChar)
                                    {
                                        isFind = true;
                                        break;
                                    }
                                }
                            }



                        }
                        if (!isFind)
                        {
                            return false;
                        }
                        var xp = page.MainFrame.XPathAsync("/html/body/main/content/div[3]/div[1]/div").Result;
                        if (xp.Length < 1)
                        {
                            return false;
                        }
                        Thread.Sleep(10000);
                        await xp[0].ScreenshotAsync($"{GlobalScope.Path.temp}\\Genshin{savename}.png", new ScreenshotOptions() { FullPage = false, Type = ScreenshotType.Png });
                        await browser.CloseAsync();
                        return true;
                    }
                    catch (Exception)
                    {

                        return false;
                    }
                }


            }

        }

        internal static MessageBuilder bind(string commandString, GroupMessageEvent e)
        {
            string uid = commandString.Replace("/v genshin bind", "").Replace(" ", "");
            if (queryGenshinResult(uid, "#bind", null).Result)
            {
                Util.Database sb = new(bindDbPath);
                if (sb.Select("bdt", $"quin={e.MemberUin}").Rows.Count != 0)
                {
                    sb.Delete("bdt", $"quin={e.MemberUin}");
                }
                sb.Insert("bdt", new KeyValuePair<string, string>[]{
                new KeyValuePair<string, string>("quin",e.MemberUin.ToString()),
                new KeyValuePair<string, string>("guid",uid)});
                return new MessageBuilder().Text("绑定完毕");
            }
            return new MessageBuilder().Text("绑定失败了，请检查是否为正确的uid");
        }

        internal static void init()
        {
            bindDbPath = $"{GlobalScope.Path.DatabasePath}\\GenshinBind";
            if (!File.Exists(bindDbPath))
            {
                var sb=new Util.Database(bindDbPath);
                sb.Create("bdt","quin","guid");
            }
        }

        internal static MessageBuilder query(string commandString, GroupMessageEvent e)
        {
            Util.Database sb = new(bindDbPath);
            var a = sb.Select("bdt", $"quin={e.MemberUin}").Rows;
            if (a.Count==0)
            {
                return new MessageBuilder().Text("查询失败 您还没有绑定，请使用/v genshin bind uid进行绑定");
            }
            string uid = a[0]["guid"].ToString();
            string tarc = commandString.Replace("/v genshin query", "").Replace(" ", "");
            var dt = DateTime.Now;
            string svn = $"{dt.ToString("yyyy")}{dt.ToString("MM")}{dt.ToString("dd")}{dt.ToString("HH")}{dt.ToString("mm")}{dt.ToString("ss")}{dt.ToString("fffff")}";
            if (queryGenshinResult(uid,tarc,svn).Result)
            {
                return new MessageBuilder().Image($"{GlobalScope.Path.temp}\\Genshin{svn}.png");
            }
            else
            {
                return new MessageBuilder().Text("查询失败了，查询不到这个角色的详细数据，可能是你没有在角色展柜里面展示这个角色，也可能是你没有开启显示角色详细信息");
            }
        }
        public static string nickName(string oc) {
            if (oc.ToLower().StartsWith("/v genshin query"))
            {
                return oc.ToLower();
            }
            if (oc.ToLower().StartsWith("/v genshin bind"))
            {
                return oc.ToLower();
            }
            if (oc.StartsWith("原神绑定"))
            {
                return oc.Replace("原神绑定", "/v genshin bind ");
            }
            if (oc.StartsWith("原神查"))
            {
                return oc.Replace("原神查", "/v genshin query ");
            }
            return oc;
        }
    }
}
