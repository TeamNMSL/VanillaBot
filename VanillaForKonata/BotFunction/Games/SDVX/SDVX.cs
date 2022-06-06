using Konata.Core.Events.Model;
using Konata.Core.Interfaces.Api;
using Konata.Core.Message;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace VanillaForKonata.BotFunction.Games
{
    static public class SDVX
    {
        
        static JArray SongDB;
        static Dictionary<string, int> SongIdIndex=new();
        static Dictionary<string, int> SongNameIndex=new();
        static Dictionary<string, int> SongAsciiIndex = new();
        static List<JObject> Songlist=new List<JObject>();


        public static MessageBuilder randSong(Konata.Core.Bot bot, GroupMessageEvent e) { 
            int v0=Songlist.Count;
            int v1 = Util.Rand.GetRandomNumber(0, v0 - 1);
            var v2 = getSongDetail(Songlist[v1]);
            string v3 = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff");
            string v4 = v2["FullSong"];
            string cpath = v2["CoverPath"];
            Console.WriteLine(cpath);
            var v5 = new MessageBuilder().Text(v2["songInfo"]);
            int index = 0;
            foreach (var item in cpath.Split("\n"))
            {
                if (File.Exists(item))
                {
                    File.Copy(item, $"{GlobalScope.Path.temp}\\{v3}-{index}-c.png", true);
                    v5.Image($"{GlobalScope.Path.temp}\\{v3}-{index}-c.png");
                    
                }
            }
            bot.SendGroupMessage(e.GroupUin, v5);

            if (File.Exists(v4))
            {
                //preSoundAMR(spath,fn);
                preSoundSilk(v4, v3);
                try
                {
                    return new MessageBuilder().Record($"{GlobalScope.Path.temp}\\{v3}-s.mp3");
                }
                catch (Exception)
                {
                    try
                    {
                        preSoundAMR(v4, v3);
                        return new MessageBuilder().Record($"{GlobalScope.Path.temp}\\{v3}-s.amr");
                    }
                    catch (Exception)
                    {

                        return new MessageBuilder().Text($"炸了别听了");
                    }

                }

            }
            else
            {
                return new MessageBuilder().Text($"出错了，找不到文件，请窒息");
            }
        }

        public static MessageBuilder SendSound(GroupMessageEvent originEventArgs, string commandString, Konata.Core.Bot bot)
        {
            commandString = commandString.Replace("/v sdvx song ", "");
            string[] cmds = commandString.Split(' ', 2);
            JObject songInfo = null;
            if (cmds.Length < 2)
            {
                return null;
            }
            if (cmds[1] == null)
            {
                return null;
            }
            if (cmds[0] == "name")
            {
                if (SongNameIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongNameIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }
            }
            else if (cmds[0] == "id")
            {
                if (SongIdIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongIdIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }

            }
            else if (cmds[0] == "ascii")
            {
                if (SongAsciiIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongAsciiIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }

            }
            else
            {
                return null;
            }
            Dictionary<string, string> res = getSongDetail(songInfo);
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff");
            string spath = res["FullSong"];

            if (File.Exists(spath))
            {
                //preSoundAMR(spath,fn);
                preSoundSilk(spath, fn);
                try
                {
                    return new MessageBuilder().Record($"{GlobalScope.Path.temp}\\{fn}-s.mp3");
                }
                catch (Exception)
                {
                    try
                    {
                        preSoundAMR(spath, fn);
                        return new MessageBuilder().Record($"{GlobalScope.Path.temp}\\{fn}-s.amr");
                    }
                    catch (Exception)
                    {

                        return new MessageBuilder().Text($"炸了别听了");
                    }

                }

            }
            else
            {
                return new MessageBuilder().Text($"出错了，找不到文件，请窒息");
            }

        }
        public static MessageBuilder? Main(GroupMessageEvent originEventArgs, string commandString, Konata.Core.Bot bot) {
            commandString = commandString.Replace("/v sdvx info ", "");
            string[] cmds = commandString.Split(' ', 2);
            JObject songInfo = null;
            if (cmds.Length < 2)
            {
                return null;
            }
            if (cmds[1] == null)
            {
                return null;
            }
            if (cmds[0] == "name")
            {
                if (SongNameIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongNameIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }
            }
            else if (cmds[0] == "id")
            {
                if (SongIdIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongIdIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }

            }
            else if (cmds[0] == "ascii")
            {
                if (SongAsciiIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongAsciiIndex[cmds[1]]];
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }

            }
            else if (cmds[0] == "json")
            {
                if (SongIdIndex.ContainsKey(cmds[1]))
                {
                    songInfo = Songlist[SongIdIndex[cmds[1]]];
                    return new MessageBuilder().Text(songInfo.ToString());
                }
                else
                {
                    return new MessageBuilder().Text("没这歌");
                }
            }
            else
            {
                return null;
            }
            Dictionary<string, string> res = getSongDetail(songInfo);
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff");
            string spath = res["SongFile"];
            string cpath = res["CoverPath"];
            Console.WriteLine(cpath);
            if (File.Exists(spath))
            {
                //preSoundAMR(spath,fn);
                preSoundSilk(spath, fn);
                bot.SendGroupMessage(originEventArgs.GroupUin, new MessageBuilder().Record($"{GlobalScope.Path.temp}\\{fn}-s.mp3"));
            }
            var mb = new MessageBuilder().Text(res["songInfo"]);
            int index = 0;
            foreach (var item in cpath.Split("\n"))
            {
                if (File.Exists(item))
                {
                    File.Copy(item, $"{GlobalScope.Path.temp}\\{fn}-{index}-c.png", true);
                    mb.Image($"{GlobalScope.Path.temp}\\{fn}-{index}-c.png");
                }
            }


            return mb;
        } 
        

        private static void preSoundSilk(string spath, string fn)
        {
            File.Copy(spath, $"{GlobalScope.Path.temp}\\{fn}-s.s3v", true);

            void SoundFormatConverter(string SoundFullPath, string SoundOutputPath)
            {

                if (File.Exists(SoundOutputPath))
                {
                    return;
                }
                string Arg = $"-i \"{SoundFullPath}\" -q:a 0 \"{SoundOutputPath}\"";
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = false,
                        FileName = $"{GlobalScope.Path.Bin}\\ffmpeg.exe",
                        Arguments = Arg,
                        RedirectStandardInput = true

                    }
                };
                proc.Start();
                while (true)
                {
                    if (proc.HasExited)
                    {

                        return;
                    }
                }


            }
            SoundFormatConverter($"{GlobalScope.Path.temp}\\{fn}-s.s3v", $"{GlobalScope.Path.temp}\\{fn}-s.mp3");
        }

        private static void preSoundAMR(string spath,string fn)
        {
            File.Copy(spath, $"{GlobalScope.Path.temp}\\{fn}-s.s3v", true);

            void SoundFormatConverter(string SoundFullPath, string SoundOutputPath)
            {

                if (File.Exists(SoundOutputPath))
                {
                    return;
                }
                string Arg = $"-i \"{SoundFullPath}\"  -f wav \"{SoundOutputPath}.wav\"";
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = false,
                        FileName = $"{GlobalScope.Path.Bin}\\ffmpeg.exe",
                        Arguments = Arg,
                        RedirectStandardInput = true

                    }
                };
                proc.Start();
                while (true)
                {
                    if (proc.HasExited)
                    {

                        return;
                    }
                }

            }
            void SoundFormatConverterAMR(string SoundFullPath, string SoundOutputPath)
            {

                if (File.Exists(SoundOutputPath))
                {
                    return;
                }
                string Arg = $"-i \"{SoundOutputPath}.wav\" -ab 12.2k -ac 1 -ar 8000  \"{SoundOutputPath}\"";
                var proc = new Process()
                {
                    StartInfo = new ProcessStartInfo()
                    {
                        RedirectStandardOutput = false,
                        UseShellExecute = false,
                        WindowStyle = ProcessWindowStyle.Hidden,
                        CreateNoWindow = false,
                        FileName = $"{GlobalScope.Path.Bin}\\ffmpeg.exe",
                        Arguments = Arg,
                        RedirectStandardInput = true

                    }
                };
                proc.Start();
                while (true)
                {
                    if (proc.HasExited)
                    {

                        return;
                    }
                }

            }
            SoundFormatConverter($"{GlobalScope.Path.temp}\\{fn}-s.s3v", $"{GlobalScope.Path.temp}\\{fn}-s.amr");
            SoundFormatConverterAMR($"{GlobalScope.Path.temp}\\{fn}-s.amr.wav", $"{GlobalScope.Path.temp}\\{fn}-s.amr");
            while (!File.Exists($"{GlobalScope.Path.temp}\\{fn}-s.amr"))
            {
                Thread.Sleep(100);
                Console.WriteLine("Couldnt find " + $"{GlobalScope.Path.temp}\\{fn}-s.amr");
            }
            while (Util.Files.IsFileInUse($"{GlobalScope.Path.temp}\\{fn}-s.amr"))
            {
                Thread.Sleep(100);
                Console.WriteLine("In use: " + $"{GlobalScope.Path.temp}\\{fn}-s.amr");
            }
        }

        private static Dictionary<string,string>getSongDetail(JObject songInfo)
        {
            string id = songInfo["@id"].ToString();
            while (id.Length!=4)
            {
                id = "0" + id;
            }
            string titleName = songInfo["info"]["title_name"].ToString();
            string artistName = songInfo["info"]["artist_name"].ToString();
            string bpm_max=(int.Parse(songInfo["info"]["bpm_max"]["#text"].ToString())/100).ToString();
            string bpm_min = (int.Parse(songInfo["info"]["bpm_min"]["#text"].ToString()) / 100).ToString();
            string title_yomigana = songInfo["info"]["title_yomigana"].ToString();
            string bpmString = "";
            string ascii = songInfo["info"]["ascii"].ToString();
            if (bpm_max==bpm_min)
            {
                bpmString = bpm_max;
            }
            else
            {
                bpmString = $"{bpm_min}~{bpm_max}";
            }
            string BasePath = $"{GlobalScope.Path.SDVX}\\data\\music\\{id}_{ascii}";
            string cover="";
            List<string> coverlist = new();
            foreach (var item in new[] { 0,1,2,3,4,5})
            {
                if (File.Exists($"{BasePath}\\jk_{id}_{item}_b.png"))
                {
                    coverlist.Add($"{BasePath}\\jk_{id}_{item}_b.png");
                }
            }
            cover = String.Join("\n", coverlist.ToArray());
            string SongFilePath = "";
            string FullSongFilePath = "";
            if (File.Exists($"{BasePath}\\{id}_{ascii}_pre.s3v"))
            {
                SongFilePath = $"{BasePath}\\{id}_{ascii}_pre.s3v";
            }
            if (File.Exists($"{BasePath}\\{id}_{ascii}.s3v"))
            {
                FullSongFilePath = $"{BasePath}\\{id}_{ascii}.s3v";
            }
            string DiffInfo = "";
            string Inf = "NoInf";
            bool haveInf = false;
            foreach (var item in (JObject)songInfo["info"])
            {
                if (item.Key == "inf_ver")
                {
                    haveInf = true;
                    break;
                }
            }
            if (haveInf) {
                switch (songInfo["info"]["inf_ver"]["#text"].ToString())
                {
                    case "2":
                        Inf = "Infinite";
                        break;
                    case "3":
                        Inf = "Gravity";
                        break;
                    case "4":
                        Inf = "Heavenly";
                        break;
                    case "5":
                        Inf = "Vivid";
                        break;
                    default:
                        Inf = "Maximum";
                        haveInf = false;
                        break;
                }
            }
            List<string> diffInfoStr=new List<string>();
            
            foreach (var item in (JObject)songInfo["difficulty"])
            {
                if (item.Value["difnum"]["#text"].ToString()!="0")
                {
                    if (item.Key=="infinite")
                    {
                        diffInfoStr.Add($"[{Inf.ToUpper()} {item.Value["difnum"]["#text"].ToString()}]\nIllustrator:{item.Value["illustrator"]}\nEffector:{item.Value["effected_by"]}");
                    }
                    else
                    {
                        diffInfoStr.Add($"[{item.Key.ToUpper()} {item.Value["difnum"]["#text"].ToString()}]\nIllustrator:{item.Value["illustrator"]}\nEffector:{item.Value["effected_by"]}");
                    }
                    
                }
                
            }
            DiffInfo=String.Join("\n\n", diffInfoStr.ToArray());
            return new Dictionary<string, string>() {
                { "songInfo",$"Title:{titleName}\nSongId:{id}\nASCII:{ascii}\nYomigana:{title_yomigana}\nArtist:{artistName}\nBPM:{bpmString}\n\n{DiffInfo}\n\nCovers:"},
                { "CoverPath",cover},
                { "SongFile",SongFilePath},
                { "FullSong",FullSongFilePath}
            };

        }

        static public void Init() {
            File.Copy($"{GlobalScope.Path.SDVX}\\data\\others\\music_db.xml", $"{GlobalScope.Path.temp}\\music_db.xml",true);
            byte[] f;
            using (var reader = new FileStream($"{GlobalScope.Path.temp}\\music_db.xml",FileMode.Open,FileAccess.Read))
            {
                f=new byte[reader.Length];
                reader.Read(f, 0, f.Length);
            }
            
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(GlobalScope.Encodings.shiftjis.GetString(f));
            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            SongDB = (JArray)(((JObject)Newtonsoft.Json.JsonConvert.DeserializeObject(json))["mdb"]["music"]);
            int i = 0;
            string asc = "";
            foreach (var item in SongDB)
            {
                asc = item["info"]["ascii"].ToString();
                SongIdIndex.Add(item["@id"].ToString(), i);
                SongNameIndex.Add(item["info"]["title_name"].ToString(), i);
                if (!SongAsciiIndex.ContainsKey(asc))
                {
                    SongAsciiIndex.Add(asc, i);
                }
                Songlist.Add((JObject)item);
                i += 1;
            }

        }
       
    }
}
