using System;
using System.IO;
using System.Text.Json;
using Konata.Core;
using Konata.Core.Events.Model;
using Konata.Core.Common;
using Konata.Core.Interfaces.Api;
using PuppeteerSharp;

// ReSharper disable ArrangeTypeModifiers

namespace VanillaForKonata
{
    static class Program
    {
        private static Bot _bot;

        public static void Main(string[] args)
        {
            try
            {
                GlobalScope.Initialization();
                
                 new BrowserFetcher().DownloadAsync(BrowserFetcher.DefaultChromiumRevision);
               
                
                _bot = new Bot(GetConfig(),
                    GetDevice(), GetKeyStore());
                {
                    // Print the log
                    _bot.OnLog += (s, e) => { Console.WriteLine(e.EventMessage); };

                    // Handle the captcha
                    _bot.OnCaptcha += (s, e) =>
                    {
                        switch (e.Type)
                        {
                            case CaptchaEvent.CaptchaType.Sms:
                                Console.WriteLine(e.Phone);
                                ((Bot)s)!.SubmitSmsCode(Console.ReadLine());
                                break;

                            case CaptchaEvent.CaptchaType.Slider:
                                Console.WriteLine(e.SliderUrl);
                                ((Bot)s)!.SubmitSliderTicket(Console.ReadLine());
                                break;

                            default:
                            case CaptchaEvent.CaptchaType.Unknown:
                                break;
                        }
                    };

                    // Handle  messages
                    _bot.OnGroupMessage += _bot_OnGroupMessage;
                    _bot.OnGroupInvite += _bot_OnGroupInvite;
                    _bot.OnFriendRequest += _bot_OnFriendRequest;
                  

                }

                // Login the bot
                var result = _bot.Login().Result;
                {
                    // Update the keystore
                    if (result) UpdateKeystore(_bot.KeyStore);
                }

                // cli
                while (true)
                {
                    switch (Console.ReadLine())
                    {
                        case "/stop":
                            _bot.Logout().Wait();
                            return;
                    }
                }
            }
            catch (Exception e)
            {

                Console .WriteLine(e.ToString());
            }
        }

        private static void _bot_OnFriendRequest(Bot sender, FriendRequestEvent args)
        {
            Task.Run(() => {
                foreach (var item in GlobalScope.Cfgs.BotAdmins)
                {
                    sender.SendFriendMessage((uint)item,
                        new Konata.Core.Message.MessageBuilder()
                        .Text(
                            $"被申请好友了\n" +
                            $"申请人:{args.ReqNick}({args.ReqUin})\n" +
                            $"Token:{args.Token}"
                            ));
                }
                sender.ApproveFriendRequest(args.ReqUin,args.Token);
            });
        }

        private static void _bot_OnGroupInvite(Bot sender, GroupInviteEvent args)
        {
            Task.Run(() => {
                foreach (var item in GlobalScope.Cfgs.BotAdmins)
                {
                    sender.SendFriendMessage((uint)item,
                        new Konata.Core.Message.MessageBuilder()
                        .Text(
                            $"被拉群了\n" +
                            $"邀请人:{args.InviterNick}({args.InviterUin})\n" +
                            $"目标群:{args.GroupName}({args.GroupUin})\n" +
                            $"管理员状态:{args.InviterIsAdmin}\n" +
                            $"Token:{args.Token}"
                            ));
                }
                sender.ApproveGroupInvitation(args.GroupUin,args.InviterUin,args.Token);
                if (args.InviterIsAdmin)
                {
                    Thread.Sleep(5000);
                    sender.SendGroupMessage(args.GroupUin, new Konata.Core.Message.MessageBuilder().Text("Bot已被拉入本群，使用方法请发送/help或到该页面查看帮助http://blog.nijikuu.com/BotHelp/"));
                }
                else
                {
                    sender.SendFriendMessage(args.InviterUin, new Konata.Core.Message.MessageBuilder().Text("已同意群邀请，请在bot入群后告知群友bot使用方法，可以在群内使用/help查看帮助,亦可进入以下页面查看帮助http://blog.nijikuu.com/BotHelp/"));
                }
            });
        }

        private static void _bot_OnGroupMessage(object? sender, GroupMessageEvent e)
        {
            Task.Run(() => {
                GroupMessage.Main(sender, e, _bot);
                
            });
        }

        /// <summary>
        /// Get bot config
        /// </summary>
        /// <returns></returns>
        private static BotConfig GetConfig()
        {
            return new BotConfig
            {
                EnableAudio = true,
                TryReconnect = true,
                HighwayChunkSize = 8192,
            };
        }

        /// <summary>
        /// Load or create device 
        /// </summary>
        /// <returns></returns>
        private static BotDevice GetDevice()
        {
            // Read the device from config
            if (File.Exists("device.json"))
            {
                return JsonSerializer.Deserialize
                    <BotDevice>(File.ReadAllText("device.json"));
            }

            // Create new one
            var device = BotDevice.Default();
            {
                var deviceJson = JsonSerializer.Serialize(device,
                    new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText("device.json", deviceJson);
            }

            return device;
        }

        /// <summary>
        /// Load or create keystore
        /// </summary>
        /// <returns></returns>
        private static BotKeyStore GetKeyStore()
        {
            // Read the device from config
            if (File.Exists("keystore.json"))
            {
                return JsonSerializer.Deserialize
                    <BotKeyStore>(File.ReadAllText("keystore.json"));
            }

            Console.WriteLine("For first running, please " +
                              "type your account and password.");

            Console.Write("Account: ");
            var account = Console.ReadLine();

            Console.Write("Password: ");
            var password = Console.ReadLine();

            // Create new one
            Console.WriteLine("Bot created.");
            return UpdateKeystore(new BotKeyStore(account, password));
        }

        /// <summary>
        /// Update keystore
        /// </summary>
        /// <param name="keystore"></param>
        /// <returns></returns>
        private static BotKeyStore UpdateKeystore(BotKeyStore keystore)
        {
            var deviceJson = JsonSerializer.Serialize(keystore,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText("keystore.json", deviceJson);
            return keystore;
        }
    }
}