using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Games.mai
{

    public class DxItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double achievements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double ds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dxScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int level_index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level_label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ra { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int song_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }

    public class SdItem
    {
        /// <summary>
        /// 
        /// </summary>
        public double achievements { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public double ds { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int dxScore { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fc { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string fs { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int level_index { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string level_label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int ra { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string rate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int song_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
    }

    public class Charts
    {
        /// <summary>
        /// 
        /// </summary>
        public List<DxItem> dx { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SdItem> sd { get; set; }
    }

    public class MaiObject
    {
        /// <summary>
        /// 
        /// </summary>
        public int additional_rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Charts charts { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string plate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int rating { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_data { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string user_id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string username { get; set; }
    }

}
