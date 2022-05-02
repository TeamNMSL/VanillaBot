namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public partial class Arcaea
    {
        public class ArcaeaBest30Info : ArcaeaNetworkQueryInfo
        {
            Models.ArcaeaBest30Info.Best30Info songBestInfo;
            public ArcaeaBest30Info(string para)
            {
                APIRoutine = $"user/best30?{para}";
                Init();
            }
            static public string buildPara(UserIdType utype, string userPara, int overflow=10,bool withRecent = false)
            {
                string userParaString = "";
                if (utype == UserIdType.user)
                {
                    userParaString = $"user={userPara}";
                }
                else if (utype == UserIdType.code)
                {
                    userParaString = $"usercode={userPara}";
                }
                return $"{userParaString}&overflow={overflow}&withrecent={withRecent.ToString().ToLower()}&withsonginfo=true";
            }
            public Models.ArcaeaBest30Info.Best30Info? getBest30Info()
            {
                if (IsReturned)
                {
                    try
                    {
                        return Result.ToObject<Models.ArcaeaBest30Info.Best30Info>();
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
            public enum UserIdType
            {
                user, code
            }
            

        }
    }
}
