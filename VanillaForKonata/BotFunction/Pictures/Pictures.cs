using Konata.Core.Message;

namespace VanillaForKonata.BotFunction
{
    static public class Pictures
    {
        static private byte[] GetRandomPicture(string foldPath) { 
            FileStream fs=new FileStream(Util.Rand.Random_File(foldPath), FileMode.Open, FileAccess.Read);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, bytes.Length);
            fs.Close();
            fs.Dispose();
            return bytes;
        }
        static public MessageBuilder DragonPicture()
            => new MessageBuilder().Image(GetRandomPicture($"{GlobalScope.Path.ImagesPath}\\龙图"));
    }
}