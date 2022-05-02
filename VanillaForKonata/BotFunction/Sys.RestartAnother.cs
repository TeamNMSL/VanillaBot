using Konata.Core.Message;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VanillaForKonata.BotFunction
{
    public static partial class Sys
    {
        internal static MessageBuilder? RestartAnotherBot()
        {
            System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();
            
             foreach (System.Diagnostics.Process myProcess in myProcesses)
              {
                if (myProcess.ProcessName == "ChocolateForKonata")
                    myProcess.Kill();
              }
            var proc = new Process()
            {
                StartInfo = new ProcessStartInfo()
                {
                    RedirectStandardOutput = false,
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Minimized,
                    CreateNoWindow = false,
                    FileName = @".\anotherbot.bat",
                    Arguments = "",
                    RedirectStandardInput = false
                }
            };
            proc.Start();

            return new MessageBuilder().Text("执行完毕");
        }
    }
}
