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