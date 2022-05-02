using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VanillaForKonata.BotFunction.Games.Arcaea.Models;

namespace VanillaForKonata.BotFunction.Games.Arcaea.Pictures
{
    public class BestImage
    {
        private Models.ArcaeaSongBestInfo.SongBestInfo best;
        private string html = "";
        public BestImage(ArcaeaSongBestInfo.SongBestInfo best)
        {
            if (best.status != 0)
            {
                throw new Exception("Error:" + best.status);
            }
            this.best = best;
            html = @"<!DOCTYPE html>
<style>
    p{
        text-shadow: 0.1px 0.1px 0.1px black;
        font-family:""consolas"";
    }
    .Player{
        color: aliceblue;
        position: absolute;
        z-index: 2;
        font-size: 13px;
        z-index: 2;
        left: 200px;
        top: 93px;
    }
    .Detail{
        color: aliceblue;
        position: absolute;
        z-index: 2;
        font-size: 15px;
        z-index: 2;
        left: 20px;
        top: 120px;
    }
    .SongName{
        font-size: 20px;
        position: absolute;
        z-index: 2;
        left: 13px;
        top: 65px;
        color: aliceblue;
        width:300px; overflow:hidden; 
        white-space:nowrap; 
        text-overflow:ellipsis;
    }
    .Score{
        color: aliceblue;
        position: absolute;
        z-index: 2;
        font-size: 20px;
        z-index: 2;
        left: 18px;
        top: 85px;
    }
    .content{
        position: absolute;
        left: 0px;
        top: 0px;
        width: 320px;
        height: 180px;
        overflow: hidden;

    }
    img{

        filter: blur(3px);
        left: 0px;
        top: 0px;
        height: 320px;
        width: 320px;
        object-fit: cover;
        position: relative;
    }
    .mask{
        
        opacity: 1;
        position: absolute;
        z-index: 1;
        left:0px;
        top: 0px;
        width: 360px;
        height: 250px;

    }
</style>";
        }
        private void addBody() {
            html += @"<body>
    <div class=""content"">
        <img src = """ + $"{getSongCoverPath_HTML()}" + @""" >
 
         <div class=""Score"">
            <p>" + $"{buildScore()}" + @"</p>
        </div>
        <div class=""Detail"">
            <p>" + $"{buildDetail()}" + @"</p>
        </div>
        <div class=""Player"">
            <p>" + $"{buildPlayer()}" + @"</p>
        </div>
        <div class=""SongName"">
            <p>" + $"{getSongName()}" + @"</p>
        </div>
        <div class=""mask"" style=""background: linear-gradient(rgba(0,0,0,0) ,rgba(" + $"{getColor()}" + @",1) 90%);""/>
    </div>
</body>";
        }

        private string getColor()
        {
            int diff = (int)best.content.record.difficulty;
            switch (diff)
            {
                case 0:
                    return "13,71,146";
                case 1:
                    return "35,146,73";
                case 2:
                    return "142,13,146";
                default:
                    return "146,13,13";
            }
        }

        private string getSongName()
            => best.content.songinfo[0].name_en.Replace(" ","&nbsp;");
        private string buildPlayer()
        {
            return $"{best.content.account_info.name}({(float)((int)(best.content.account_info.rating)) / 100})<br>{best.content.account_info.code}";
        }

        private string buildDetail()
        {
            return $"P:{best.content.record.perfect_count}(+{best.content.record.shiny_perfect_count})&nbsp;F:{best.content.record.near_count}&nbsp;L:{best.content.record.miss_count}<br>SongRating:{((float)best.content.songinfo[0].rating/10).ToString("0.0")} &nbsp;PlayRating:{best.content.record.rating.ToString("0.00")}";
        }

        private string buildScore()
            => best.content.record.score.ToString();

        private string getSongCoverPath_HTML()
        {
            string songbasepath = best.content.record.song_id;
            if (best.content.songinfo[0].remote_download.ToString().ToLower() == "true")
            {
                songbasepath = "dl_" + songbasepath;
            }
            return ("file:///" + $"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.Assets}{GlobalScope.Arc.PartPath.SongsFold}\\{songbasepath}\\base.jpg").Replace("\\", "/");
        }

        public async Task<MessageBuilder?> getBest() {
            addBody();
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff");
            File.WriteAllText($"{GlobalScope.Path.temp}\\ArcBest-{fn}.html", html);
            await Util.HTML2Pic.generate($"{GlobalScope.Path.temp}\\ArcBest-{fn}.html", $"{GlobalScope.Path.temp}\\ArcBest-{fn}.png", 320, 180);
            return new MessageBuilder().Image($"{GlobalScope.Path.temp}\\ArcBest-{fn}.png");
        }
    }
}
