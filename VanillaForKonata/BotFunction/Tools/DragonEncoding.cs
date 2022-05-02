using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction.Tools
{
    public class DragonEncoding
    {
        private static class DragonByteConverter {
            
            static private string Str2Hex(string str)
                => BitConverter.ToString(Encoding.UTF8.GetBytes(str)).Replace("-", "");
            static private Dictionary<string, string> char2hex=new()
            {
                {  "你" ,"0"},
                {  "妈","1" },
                {  "死" ,"2"},
                {  "了" ,"3"},
                {  "我" ,"4"},
                {  "杀" ,"5"},
                {  "马" ,"6"},
                {  "龙" ,"7"},
                {  "舍" ,"8"},
                {  "得" ,"9"},
                {  "打" ,"A"},
                {  "破" ,"B"},
                {  "这" ,"C"},
                {  "片" ,"D"},
                {  "宁" ,"E"},
                {  "静" ,"F"}

            };
            static private Dictionary<string, string> hex2char= new()
            {
                { "0", "你" },
                { "1", "妈" },
                { "2", "死" },
                { "3", "了" },
                { "4", "我" },
                { "5", "杀" },
                { "6", "马" },
                { "7", "龙" },
                { "8", "舍" },
                { "9", "得" },
                { "A", "打" },
                { "B", "破" },
                { "C", "这" },
                { "D", "片" },
                { "E", "宁" },
                { "F", "静" }

            };

            static private string Hex2Str(string hex, string separator = null)
            {
                byte[] bytes = new byte[hex.Length / 2];
                for (int i = 0; i < bytes.Length; i++)
                {
                    bytes[i] = byte.Parse(hex.Substring(i * 2, 2),
                       System.Globalization.NumberStyles.HexNumber);
                }
                return System.Text.Encoding.UTF8.GetString(bytes);
            }
            static public string Text2Dragon(string str)
            {
                string r = "";
                foreach (var item in Str2Hex(str).ToCharArray())
                {
                    r += hex2char[item.ToString()];
                }
                return r;
            }
            static public string Dragon2Text(string DragonChar)
            {
                string r = "";
                foreach (var item in DragonChar.ToCharArray())
                {
                    r += char2hex[item.ToString()];
                }
                return Hex2Str(r);

            }
           
        }
        public static MessageBuilder Encode(string oritext)
               => new MessageBuilder().Text($"龙曰:{DragonByteConverter.Text2Dragon(oritext.Substring(16))}");
        public static MessageBuilder Decode(string dratext)
               => new MessageBuilder().Text($"解读:{DragonByteConverter.Dragon2Text(dratext.Substring(19))}");
    }
}
