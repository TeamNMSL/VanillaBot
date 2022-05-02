using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Konata.Core.Message;
using VanillaForKonata.BotFunction.Games.Arcaea.Models;

namespace VanillaForKonata.BotFunction.Games.Arcaea.Pictures
{
    public class Best30Image {
        Models.ArcaeaBest30Info.Best30Info b30info;
        public Best30Image(ArcaeaBest30Info.Best30Info t)
        {
            b30info = t;
            if (t.status!=0)
            {
                throw new Exception("Error:"+t.status);
            }
        }

        private class B30HTMLBuilder {
            private string html;
            private Models.ArcaeaBest30Info.Best30Info best30;

            public B30HTMLBuilder( ArcaeaBest30Info.Best30Info best30)
            {
                this.html = @"<!DOCTYPE html><head></head><body>";
                this.best30 = best30;
            }

            private string getSongCoverPath_HTML_30(int i)
            {
                string songbasepath = best30.content.best30_list[i].song_id;
                if (best30.content.best30_songinfo[i].remote_download.ToString().ToLower() == "true")
                {
                    songbasepath = "dl_" + songbasepath;
                }
                return ("file:///" + $"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.Assets}{GlobalScope.Arc.PartPath.SongsFold}\\{songbasepath}\\base.jpg").Replace("\\", "/");
            }
            private string getSongCoverPath_HTML_ov(int i)
            {
                string songbasepath = best30.content.best30_overflow[i].song_id;
                if (best30.content.best30_overflow_songinfo[i].remote_download.ToString().ToLower() == "true")
                {
                    songbasepath = "dl_" + songbasepath;
                }
                return ("file:///" + $"{GlobalScope.Path.Arcaea}{GlobalScope.Arc.PartPath.Assets}{GlobalScope.Arc.PartPath.SongsFold}\\{songbasepath}\\base.jpg").Replace("\\", "/");
            }
            private void addB3Item() {
                html += @"<div id=""Best30Content"">";
                int i = 1;
                foreach (var item in best30.content.best30_list)
                {
                    html += @"
                   
         <div class=""B30Item"">";
                    html += @" <div class=""InfoContent"">
                 <div class=""songTitle"">
                    <p style = ""width:100px; overflow:hidden; white-space:nowrap; text-overflow:ellipsis;"" > " + str2htmlstr(best30.content.best30_songinfo[i - 1].name_en) + @"</p>
                </div>";
                    html += @"<div class=""songScore"">
                     <p> " + item.score + @"</p>
 
                 </div> ";
                    html += @"<div class=""songPos"">
                     <p>" + $"#{i}" + @"</p>
                </div>
 
                 <div class=""songPtt"">
                    <p>" + item.rating.ToString("0.00") + @"</p>
                </div>
                <div class=""songDetail"">
                    <p>" + $"P:{item.perfect_count} F:{item.near_count} L:{item.miss_count}"
                    + @"</p>
                </div>
                <div class=""songDiff"">
                    <p>" + $"{((float)((int)best30.content.best30_songinfo[i-1].rating)/10).ToString()}" + @"</p>
                </div>
            </div>
            <div class=""imgmask"" style=""background-color: rgb("+ getColor((int)item.difficulty) + @");""></div>
            <img src = """+getSongCoverPath_HTML_30(i-1) + @""" class=""best30SongCover""/>
</div>";
                    i += 1;
                }
                i = 1;
                if (best30.content.best30_overflow != null)
                {
                    if (best30.content.best30_overflow.Count != 0)
                    {
                        foreach (var item in best30.content.best30_overflow)
                        {
                            html += @"
                   
         <div class=""B30Item"">";
                            html += @" <div class=""InfoContent"">
                 <div class=""songTitle"">
                    <p style = ""width:100px; overflow:hidden; white-space:nowrap; text-overflow:ellipsis;"" > " + str2htmlstr(best30.content.best30_overflow_songinfo[i - 1].name_en) + @"</p>
                </div>";
                            html += @"<div class=""songScore"">
                     <p> " + item.score + @"</p>
 
                 </div> ";
                            html += @"<div class=""songPos"">
                     <p>" + $"#{i + 30}" + @"</p>
                </div>
 
                 <div class=""songPtt"">
                    <p>" + item.rating.ToString("0.00") + @"</p>
                </div>
                <div class=""songDetail"">
                    <p>" + $"P:{item.perfect_count} F:{item.near_count} L:{item.miss_count}"
                            + @"</p>
                </div>
                <div class=""songDiff"">
                    <p>" + $"{((float)((int)best30.content.best30_overflow_songinfo[i - 1].rating) / 10).ToString()}" + @"</p>
                </div>
            </div>
            <div class=""imgmask"" style=""background-color: rgb(" + getColor((int)item.difficulty) + @");""></div>
            <img src = """ + getSongCoverPath_HTML_ov(i - 1) + @""" class=""best30SongCover""/>
</div>";
                            i += 1;
                        }
                    }
                }
                
                
                html += " </div>";
            }
            private string getColor(int diff) {
                switch (diff) {
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
            private string str2htmlstr(string str) {
                return str.Replace(" ", "&nbsp");
            }
            public string getHTML() {
                    addBg();
                    addUserInfo();
                    addB3Item();

                html += @"</body>";
                html += @"<style>
    
    #UserInfo{
        position: absolute;
        z-index: 5; 

    }
    .UserName{
        position: absolute;
        color: aliceblue;
        font-size: 23px;
        top: -20px;
        left: 200px;
        width:160px; 
        overflow:hidden; 
        white-space:nowrap; 
        text-overflow:ellipsis;
    }
    .FriendCode{
        color: aliceblue;
        position: absolute;
        font-size: 15px;
        top: 20px;
        left: 220px;
    }
    .UserPtt{
        color: aliceblue;
        position: absolute;
        font-size: 17px;
        top: 30px;
        left: 380px;
    }
    .B30Average{
        color: aliceblue;
        position: absolute;
        font-size: 17px;
        top: 10px;
        left: 380px;
    }
    .R10Average{
        color: aliceblue;
        position: absolute;
        font-size: 17px;
        top: -10px;
        left: 380px;
    }
    #bg{
        left: 0px;
        top: 0px;

        position: absolute;
        margin-left: -244px;
        overflow: hidden;
    }
    #bg img{
        overflow: hidden;
        object-fit:cover;
    }
    #Best30Content{
        left: 0px;
        top: 0px;
        margin-left: 10px;
        margin-top: 80px;
        width: 770px;
        height: 385px;
        display: inline-block;
        position: absolute;
    }
    div.B30Item{
        float: left;
        width: 150px;
        height: 75px;
        overflow: hidden;
        border-radius: 10px;
        margin-right:2px;
        margin-left:2px;
        margin-top:1px;
        margin-bottom: 1px;
        
    }
    div.B30Item img{
        position:relative;
        width:100%;
        height: 100%;
        object-fit:cover;
        filter: blur(1px);
        
    }

    .imgmask{
        
        opacity: 0.4;
        position:absolute;
        width: 150px;
        height: 75px;
        border-radius: 10px;
        z-index: 1;

    }

    div.InfoContent{
        position: absolute;
        height: 75px;
        font-size:xx-large;
        background-color:rgba(0,0,0,0);
        
    }
    .InfoContent p{
        position: absolute;
        z-index: 2;
        
    }
    .songTitle{
        position:relative;
        color: aliceblue;
        font-size: 18px;
        top: -15px;
        left: 5px;
    }
    .songScore{
        position:relative;
        color: aliceblue;
        font-size: 18px;
        top:10px;
        left: 5px;
    }
    .songPos{
        position:relative;
        color: aliceblue;
        font-size: 23px;
        top:23px;
        left: 5px;
    }
    .songDetail{
        position:relative;
        color: aliceblue;
        font-size: 12px;
        left: 105px;
        top:15px;
    }
    .songPtt{
        position:relative;
        color: aliceblue;
        font-size: 15px;
        top:40px;
        left: 48px;
    }
    .songDiff{
        position:relative;
        font-size: 15px;
        top:-10px;
        left: 110px;
        font-weight:bolder;
        color: aliceblue;
        
    }
    p{
        font-family:""consolas"";
    }
</style>";
                return html;
            }
            private void addBg() { 
                html+= @"<div id=""bg"">
         <img src =""file:///"+$"{$"{GlobalScope.Path.Arcaea}{ GlobalScope.Arc.PartPath.Assets}\\img\\bg\\observer_light.jpg"}".Replace("\\","/") +@""">
      </div>";
  
      
            }
            private void addUserInfo()
            {
                html += @"<div id=""UserInfo"">
         <p class=""UserName"">" + best30.content.account_info.name + @"</p>
        <p class=""UserPtt"">Potential:&nbsp;" + ((float)((int)(best30.content.account_info.rating))/100).ToString() + @"</p>
        <p class=""FriendCode"">" + best30.content.account_info.code + @"</p>
        <p class=""B30Average"">Best30&nbsp;Average:&nbsp;" + best30.content.best30_avg.ToString("0.0000") + @"</p>
        <p class=""R10Average"">Recent10&nbsp;Average:&nbsp;" + best30.content.recent10_avg.ToString("0.0000") + @"</p>
    </div>";
            }
        }

        public async Task<MessageBuilder?> B30()
        {
            string html=new B30HTMLBuilder(b30info).getHTML();
            string fn = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fffff");
            File.WriteAllText($"{GlobalScope.Path.temp}\\ArcBest30-{fn}.html", html);
            await Util.HTML2Pic.generate($"{GlobalScope.Path.temp}\\ArcBest30-{fn}.html", $"{GlobalScope.Path.temp}\\ArcBest30-{fn}.png",790,720);
            return new MessageBuilder().Image($"{GlobalScope.Path.temp}\\ArcBest30-{fn}.png");
        }
    }


  
}
