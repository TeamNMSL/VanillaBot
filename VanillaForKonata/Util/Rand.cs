using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.Util
{
    public static class Rand
    {
        /// <summary>
        /// 取随机数
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static int GetRandomNumber(int min, int max)
        {

            try
            {
                byte[] buffer = Guid.NewGuid().ToByteArray();
                int iSeed = BitConverter.ToInt32(buffer, 0);
                Random r = new Random(iSeed);
                int rtn = r.Next(min, max + 1);
                return rtn;
            }
            catch (Exception e) { return -1; }
        }
        public static int GetRandomNumber(int seed,int min, int max)
        {

            try
            {
                
                Random r = new Random(seed);
                int rtn = r.Next(min, max + 1);
                return rtn;
            }
            catch (Exception e) { return -1; }
        }
        /// <summary>
        /// 随机文件
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Random_File(string path)
        {

            try
            {
                DirectoryInfo folder = new DirectoryInfo(path);
                FileInfo[] file = folder.GetFiles();
                int i = 0;
                foreach (FileInfo info in file)
                {
                    i++;
                }
                int random = GetRandomNumber(0, i - 1);
                return file[random].FullName;
            }
            catch (Exception e) { return "RandomFile.Dll-CsError:" + e.Message; }

        }
        /// <summary>
        /// 随机文件夹
        /// </summary>
        /// <param name="dirPath"></param>
        /// <returns></returns>
        public static string Random_Folders(string dirPath)
        {
            try
            {
                ArrayList list = new ArrayList();
                List<string> dirs = new List<string>(Directory.GetDirectories(dirPath, "*", System.IO.SearchOption.AllDirectories));
                foreach (var dir in dirs)
                {
                    //Console.WriteLine("{0}", dir);
                    list.Add(dir);

                }
                int random_s = GetRandomNumber(0, dirs.Count - 1);
                return list[random_s].ToString();
            }
            catch (Exception e)
            {
                return "err";
            }
        }
        public static bool CanIDo(double factor = 0.5f)
            => new Random().NextDouble() >= (1 - factor);
    }
}
