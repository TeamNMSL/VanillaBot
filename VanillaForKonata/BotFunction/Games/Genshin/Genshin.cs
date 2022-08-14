using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunkong.Hoyolab;
using PuppeteerSharp;
using Konata.Core.Events.Model;
using Konata.Core;
using Xunkong.Hoyolab.Avatar;

namespace VanillaForKonata.BotFunction.Games.Genshin
{
    static public class Genshin
    {
        static public string dbPath="";
        const string CONST_TableName = "GenshinBind";
        const string CONST_UserID = "uin";
        const string CONST_Cookie = "cookie";
        static string resPath="";
        static public void init() {
            dbPath = $"{GlobalScope.Path.DatabasePath}\\GenshinBind";
            if (!File.Exists(dbPath))
            {
                Util.Database db = new(dbPath);
                db.Create(CONST_TableName,CONST_UserID,CONST_Cookie);
            }
            resPath = $"{GlobalScope.Path.ImagesPath}\\Genshin\\";
        }
      
        public static async Task<string> login(string phone,string cap)
        {

            using (var browser = await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = false
            }))
            {
                using (var page = await browser.NewPageAsync())
                {
                     page.SetViewportAsync(new ViewPortOptions
                    {
                        Width = 1280,
                        Height = 720
                    });
                    try
                    {
                        await page.GoToAsync("https://bbs.mihoyo.com/ys/");
                        await page.WaitForSelectorAsync(".header__avatar");
                        page.ClickAsync(".header__avatar");
                        page.WaitForSelectorAsync(".mhy-login-button");
                        await page.WaitForSelectorAsync("div.mhy-login-form-input:nth-child(1) > div:nth-child(1) > input:nth-child(1)");
                        await page.FocusAsync("div.mhy-login-form-input:nth-child(1) > div:nth-child(1) > input:nth-child(1)");
                        await page.Keyboard.TypeAsync(phone);

                        await page.FocusAsync("div.mhy-login-form-input:nth-child(2) > div:nth-child(1) > input:nth-child(1)");
                        await page.Keyboard.TypeAsync(cap);
                        Thread.Sleep(1000);
                        await page.ClickAsync(".login-btn");
                        Thread.Sleep(9000);
                        var cookie = page.WaitForExpressionAsync("document.cookie").Result;
                        Console.WriteLine(cookie.RemoteObject.Value.ToString());
                        return cookie.RemoteObject.Value.ToString();
                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e.Message);
                    }
                    return null;
                }


            }




        }
        static public string loginner(string pn, string ca)
        {
            try
            {
                string token = login(pn, ca).Result;
                var client = new HoyolabClient();
                var roles = client.GetGenshinRoleInfosAsync(token).Result;

                return token;

            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.Message);
                return "csbnmsl";
            }
        }   
            
            
        static public MessageBuilder bind(Konata.Core.Events.Model.FriendMessageEvent e, string commandString) {
            return new MessageBuilder().Text("暂不支持绑定因为出现了些小问题");
            string[] cmds = commandString.Split(" ");
            if (cmds.Length!=5)
            {
                return new MessageBuilder().Text("参数错误");
            }
            string token = loginner(cmds[3], cmds[4]);
            if (token=="csbnmsl")
            {
                return new MessageBuilder().Text("绑定失败，可能是参数不正确");
            }
            Util.Database db = new(dbPath);
            if (db.Select(CONST_TableName,$"{CONST_UserID}={e.FriendUin}").Rows.Count!=0)
            {
                db.Delete(CONST_TableName, $"{CONST_UserID}={e.FriendUin}");
            }
            db.Insert(CONST_TableName,
                new[] {
                    new KeyValuePair<string,string>(CONST_UserID,e.FriendUin.ToString()),
                    new KeyValuePair<string,string>(CONST_Cookie,token)
                });
            var client = new HoyolabClient();
            var acc = client.GetHoyolabUserInfoAsync(token).Result;
            return new MessageBuilder().Text($"已将你的账号绑定到{acc.Nickname}({acc.Uid})");
        }

        static public (HoyolabClient,string) getUser(string uin) {
            try
            {
                Util.Database db = new(dbPath);
                var res = db.Select(CONST_TableName, $"{CONST_UserID}={uin}");
                string token = res.Rows[0][CONST_Cookie].ToString();
                return (new(), token);
            }
            catch (Exception)
            {

                return (null,"cnmnmsl");
            }
        }

        public static MessageBuilder bindGroup()
            => new MessageBuilder().Text("您无法在群内绑定米游社账号，请加bot为好友并打开米游社，输入手机号发送验证码，不要输入验证码并向bot发送以下内容\n/v genshin bind 你的手机号 你收到的6位验证码\n在Bot回复你的米游社昵称后则绑定成功");

        public static MessageBuilder queryChar(GroupMessageEvent e, Bot bot,string cmd)
        {
            // /v genshin query charname
            string[] cmds = cmd.Split(" ",5);
            if (cmds.Length<4)
            {
                return new MessageBuilder().Text("参数错误");
            }
            var usrdata = getUser(e.MemberUin.ToString());
            var client = usrdata.Item1;
            string token = usrdata.Item2;
            if (token=="cnmnmsl")
            {
                return new MessageBuilder().Text("你似乎没有绑定账号？请使用/v genshin bind进行绑定");
            }

            var role = client.GetGenshinRoleInfosAsync(token).Result;
            if (role.Count==0)
            {
                return new MessageBuilder().Text("没有找到你的原神账号（疑惑");
            }
            var player = role[0];
            string playName = player.Nickname;
            var Avatars=client.GetAvatarDetailsAsync(player).Result;
            bool isFind = false;
            AvatarDetail chara = null;
            foreach (var avt in Avatars)
            {
                if (avt.Name == cmds[3])
                {
                    chara = avt;
                    isFind = true;
                    break;
                }
            }
            if (!isFind)
            {
                return new MessageBuilder().Text("没有找到你要查询的角色（可能是没抽到？");
            }
            var model = new CharModel(chara);

            return picDrawer(model, player);
        }
        static public MessageBuilder picDrawer(CharModel model, Xunkong.Hoyolab.Account.GenshinRoleInfo player) {
            string tpl = File.ReadAllText($"{GlobalScope.Path.templates}\\genshinQuery.html");
            string tplP= File.ReadAllText($"{GlobalScope.Path.templates}\\genshinQuery_part.html");
            Util.PicHTMLBuilder h = new(tpl);
            string userinfo = $"用户名<br>&nbsp&nbsp&nbsp&nbsp{player.Nickname}<br>等级<br>&nbsp&nbsp&nbsp&nbsp{player.Level}";
            string charinfo = $"{model.name}<br>已激活命座数：{model.mingzuo}<br>等级：{model.LV}<br>好感：{model.haogan}";
            string charaurl = model.icon;
            List<string> htmls = new();
            var w = new Util.PicHTMLBuilder(tplP);
            w.setValue("itemurl", model.wuqi.url);
            w.setValue("raurl", pather(model.wuqi.xiyoudu));
            string descw = $"LV{model.wuqi.lv}<br>精{model.wuqi.affixLV}";
            w.setValue("desc", descw);
            htmls.Add(w.getHTML());
            w = null;
            foreach (var item in model.shengyiwu.reliquaries)
            {
                var p= new Util.PicHTMLBuilder(tplP);
                p.setValue("itemurl", item.url);
                p.setValue("raurl", pather(item.rat));
                string desc = $"LV{item.lv}<br>-";
                p.setValue("desc", desc);
                htmls.Add(p.getHTML());
                p = null;
            }
            string pa1=string.Join("\n", htmls.ToArray());
            htmls.Clear();
            h.setValue("charaurl", charaurl);
            h.setValue("userinfo",userinfo);
            h.setValue("charinfo",charinfo);
            h.putHTML("item",pa1);
            string full = h.getHTML();
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-FFFFFFF");
            File.WriteAllText(GlobalScope.Path.temp + "\\GenshinGeneralQuery" + fn + ".html", full);
            var a = Util.HTML2Pic.generate(GlobalScope.Path.temp + "\\GenshinGeneralQuery" + fn + ".html", GlobalScope.Path.temp + "\\GenshinGeneralQuery" + fn + ".png", 800, 500,5000);
            if (a.Result)
            {
                return new MessageBuilder().Image(GlobalScope.Path.temp + "\\GenshinGeneralQuery" + fn + ".png");
            }
            return null;
        }
        static public string pather(string r) {
            switch (r)
            {
                case "5":
                    return Util.PicHTMLBuilder.pathConverter($"{resPath}s-5.png");
                case "4":
                    return Util.PicHTMLBuilder.pathConverter($"{resPath}s-4.png");
                case "3":
                    return Util.PicHTMLBuilder.pathConverter($"{resPath}s-3.png");
                case "2":
                    return Util.PicHTMLBuilder.pathConverter($"{resPath}s-2.png");
                default:
                    return Util.PicHTMLBuilder.pathConverter($"{resPath}s-1.png");
            }
        }
        static public string NickName(string ori) {
            if (ori.StartsWith("原神查 "))
            {
                return ori.Replace("原神查 ", "/v genshin query ");
            }
            if (ori.StartsWith("原神查"))
            {
                return ori.Replace("原神查", "/v genshin query ");
            }
            return ori;
        }
        
    }
}
