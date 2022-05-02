namespace VanillaForKonata.BotFunction.Games.Arcaea
{
    public partial class Arcaea
    {
        public class ArcaeaSongBestInfo : ArcaeaNetworkQueryInfo
        {
            Models.ArcaeaSongBestInfo.SongBestInfo songBestInfo;
            public ArcaeaSongBestInfo(string para)
            {
                APIRoutine = $"user/best?{para}";
                Init();
            }
            static public string buildPara(UserIdType utype,SongIdType stype, string userPara,string songname,string diff,bool withRecent=false)
            {
                string userParaString = "";
                string songParaString = "";
                if (utype == UserIdType.user)
                {
                    userParaString = $"user={userPara}";
                }
                else if (utype == UserIdType.code)
                {
                    userParaString = $"usercode={userPara}";
                }
                if (stype == SongIdType.name)
                {
                    songParaString = $"songname={songname}";
                }
                else if (stype == SongIdType.id) {
                    songParaString = $"songid={songname}";
                }
                return $"{userParaString}&{songParaString}&difficulty={diff}&withrecent={withRecent.ToString().ToLower()}&withsonginfo=true";
            }
            public Models.ArcaeaSongBestInfo.SongBestInfo? getSongBestInfo()
            {
                if (IsReturned)
                {
                    try
                    {
                        return Result.ToObject<Models.ArcaeaSongBestInfo.SongBestInfo>();
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
            public enum SongIdType { 
                id,name
            }

        }
    }
}
