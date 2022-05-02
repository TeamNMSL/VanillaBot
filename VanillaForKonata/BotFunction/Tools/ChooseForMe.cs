using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Tools
{
    internal class ChooseForMe
    {
        static public MessageBuilder Choose(string commandString) { 
            string[] c=commandString.Replace("/v choose ","").Split(' ');
            int co = c.Length;
            string res = c[new Random().Next(0,c.Length)];
            
            return new MessageBuilder().Text($"那我建议你选择{res}");
        }
    }
}
