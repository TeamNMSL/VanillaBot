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
        private static object 用户Id;

        static public MessageBuilder Choose(string commandString) { 
            string[] c=commandString.Replace("/v choose ","").Split(' ');
            int co = c.Length;
            string res = c[new Random().Next(0,c.Length)];
            
            return new MessageBuilder().Text($"那我建议你选择{res}");

            return 从数据库中获取的用户Id为(用户Id).的答案;
        }

        class 数据组 { 
            public MessageBuilder 的答案=new MessageBuilder();
        }
        private static 数据组 从数据库中获取的用户Id为(object 用户)
        {
            throw new NotImplementedException();
        }
    }
}
