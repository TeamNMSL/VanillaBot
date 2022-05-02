namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public partial class Arcaea
    {
        public class ArcaeaRecentInfo:ArcaeaNetworkQueryInfo {
            Models.ArcaeaRecentInfo.ArcaeaRecent recentInfo;
            public ArcaeaRecentInfo(string para)
            {
                APIRoutine = $"user/info?{para}";
                Init();
            }
            static public string buildPara(RecentParaType type,string userPara,int recentCount=1) {
                string userParaString = "";
                if (type==RecentParaType.user)
                {
                    userParaString = $"user={userPara}";
                }
                else if (type==RecentParaType.code)
                {
                    userParaString = $"usercode={userPara}";
                }
                return $"{userParaString}&recent={recentCount}&withsonginfo=true";
            }
            public Models.ArcaeaRecentInfo.ArcaeaRecent? getRecentInfo() {
                if (IsReturned)
                {
                    try
                    {
                        return Result.ToObject<Models.ArcaeaRecentInfo.ArcaeaRecent>();
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            public enum RecentParaType
            {
                user,code
            }

        }
    }
}
