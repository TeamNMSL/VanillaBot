using Konata.Core;
using Konata.Core.Events.Model;
using Konata.Core.Message;
using Konata.Core.Interfaces.Api;

namespace VanillaForKonata
{
    static public class FriendMessage
    {

        internal static void Main(FriendMessageEvent e, Bot bot)
        {
            try
            {
                string commandString = e.Message.Chain.ToString();
                MessageBuilder Reply=null;
                if (e.FriendUin == e.SelfUin)
                    return;
                if (commandString.StartsWith("//v genshin bind "))
                    Reply = BotFunction.Games.Genshin.Genshin.bind(e,commandString);

                if (Reply != null)
                    bot.SendFriendMessage(e.FriendUin,Reply);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}