using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Games.Arcaea.Models
{
    public class ArcaeaBest30Info
    {
        public class Account_info
        {
            /// <summary>
            /// 
            /// </summary>
            public string code { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long user_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string is_mutual { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string is_char_uncapped_override { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string is_char_uncapped { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string is_skill_sealed { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long rating { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long join_date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long character { get; set; }
        }

        public class Best30_listItem
        {
            /// <summary>
            /// 
            /// </summary>
            public long score { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long health { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double rating { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string song_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long modifier { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long difficulty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long clear_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long best_clear_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long time_played { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long near_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long miss_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long perfect_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long shiny_perfect_count { get; set; }
        }

        public class Best30_overflowItem
        {
            /// <summary>
            /// 
            /// </summary>
            public long score { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long health { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double rating { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string song_id { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long modifier { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long difficulty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long clear_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long best_clear_type { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long time_played { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long near_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long miss_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long perfect_count { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long shiny_perfect_count { get; set; }
        }

        public class Best30_songinfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name_en { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name_jp { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string artist { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bpm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long bpm_base { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string @set { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string set_friendly { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long side { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string world_unlock { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remote_download { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long difficulty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long rating { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long note { get; set; }
            /// <summary>
            /// 迷路深層
            /// </summary>
            public string chart_designer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string jacket_designer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string jacket_override { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string audio_override { get; set; }
        }

        public class Best30_overflow_songinfoItem
        {
            /// <summary>
            /// 
            /// </summary>
            public string name_en { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string name_jp { get; set; }
            /// <summary>
            /// 翡乃イスカ
            /// </summary>
            public string artist { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bpm { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long bpm_base { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string @set { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string set_friendly { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long time { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long side { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string world_unlock { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string remote_download { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string bg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long date { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string version { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long difficulty { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long rating { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public long note { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string chart_designer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string jacket_designer { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string jacket_override { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public string audio_override { get; set; }
        }

        public class Content
        {
            /// <summary>
            /// 
            /// </summary>
            public double best30_avg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public double recent10_avg { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Account_info account_info { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Best30_listItem> best30_list { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Best30_overflowItem> best30_overflow { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Best30_songinfoItem> best30_songinfo { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public List<Best30_overflow_songinfoItem> best30_overflow_songinfo { get; set; }
        }

        public class Best30Info
        {
            /// <summary>
            /// 
            /// </summary>
            public long status { get; set; }
            /// <summary>
            /// 
            /// </summary>
            public Content content { get; set; }
        }


        

    }
}
