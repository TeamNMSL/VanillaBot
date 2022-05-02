using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.Util
{
    public static class Base64
    {
        /// <summary>
        /// Base64 编码
        /// </summary>
        /// <param name="encode">编码方式</param>
        /// <param name="source">要编码的字符串</param>
        /// <returns>返回编码后的字符串</returns>
        public static string EncodeBase64(Encoding encode, string source)
        {
            string result = "";
            byte[] bytes = encode.GetBytes(source);
            try
            {
                result = Convert.ToBase64String(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        }


        /// <summary>
        /// Base64 解码
        /// </summary>
        /// <param name="encode">解码方式</param>
        /// <param name="source">要解码的字符串</param>
        /// <returns>返回解码后的字符串</returns>
        public static string DecodeBase64(Encoding encode, string source)
        {
            string result = "";
            byte[] bytes = Convert.FromBase64String(source);
            try
            {
                result = encode.GetString(bytes);
            }
            catch
            {
                result = source;
            }
            return result;
        } 

    }
}
