using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VanillaForKonata.BotFunction.Games.Arcaea.Arcaea;

namespace VanillaForKonata.BotFunction.Games.Arcaea.API
{
    public partial class Arcaea
    {
        public class ArcaeaChartPreview : ArcaeaNetworkQueryInfo
        {
            public ArcaeaChartPreview(string para)
            {
                APIRoutine = $"assets/preview?{para}";
                Init();
            }
            static public string ParaBuilder(string songname, string diff)
                => $"songid={songname}&difficulty={diff}";
            public void saveStream(string path) {
                var s=getByte();
                File.WriteAllBytes(path, s);
            }
        }
    }
}
