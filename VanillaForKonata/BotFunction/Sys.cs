using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction
{
    public static partial class Sys
    {
        public static MessageBuilder Ping()
            => new MessageBuilder().Text("Pong!");
        public static MessageBuilder Repeat(MessageChain baseChains)
            => new MessageBuilder().Add(baseChains);
    }
}
