using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace UninstallHelper
{
    class Program
    {
        static StreamWriter sw = null;
        static void CreateLog()
        {
            try
            {
                string temppath = Path.GetTempPath();
                string filename = String.Format("{0:yyyy-MM-dd}__{1}", DateTime.Now, "UninstallHelperLog");
                string path = Path.Combine(temppath, filename);
                sw = new StreamWriter(path, true);
            }
            catch (Exception ex)
            {

            }
        }
        static void Log(string logtxt)
        {
            if (sw != null)
            {
                sw.WriteLine(logtxt);
                sw.Flush();
            }
        }
        // This function gives two strings in string[]
        // string[0] will be uninstaller
        // string[1] will be rest of command line
        // Example input C:\ProgramData\Package Cache\{27f62ca6-f3b1-463e-a2ff-fdcb721925d6}\AW988HWControlInstaller.exe /s /uninstall
        // string[0] = "C:\ProgramData\Package Cache\{27f62ca6-f3b1-463e-a2ff-fdcb721925d6}\AW988HWControlInstaller.exe"
        // string[1] =  /s /uninstall
        static List<string> SplitExe(string fullstring)
        {
            List<string> result = new List<string>();
            fullstring = fullstring.Trim();
            // split at .exe
            string[] stringSeparator = new string[] { ".exe", ".EXE", ".Exe" };

            string[] splitexe = fullstring.Split(stringSeparator, StringSplitOptions.None);
            if (splitexe.Length == 2)
            {
                result.Add("\"" + splitexe[0] + ".exe\"");
                string remainging = splitexe[1].Trim();
                result.Add(remainging);
            }
            return result;

        }

        static void Main(string[] args)
        {
            CreateLog();
            Log("Number of arguments =" + args.Length.ToString());
            if (args.Length > 0)
            {
                string fullstring = string.Join(" ", args);
                var result = SplitExe(fullstring);
                if (result.Count == 2)
                {
                    Log("Uninstaller=" + result[0]);
                    Log("Command line to uninstaller=" + result[1]);
                    using (Process uninstaller = new Process())
                    {
                        uninstaller.StartInfo.UseShellExecute = false;
                        // You can start any process, HelloWorld is a do-nothing example.
                        uninstaller.StartInfo.FileName = result[0];
                        uninstaller.StartInfo.Arguments = result[1];
                        uninstaller.StartInfo.CreateNoWindow = true;
                        Log("Starting Uninstaller:" + result[0] + " Date Time:" + DateTime.Now.ToString());
                        uninstaller.Start();
                        uninstaller.WaitForExit();
                        Log("Uninstaller stopped, now waiting" + result[0] + "Date Time:" + DateTime.Now.ToString());
                        System.Threading.Thread.Sleep(10000);
                        Log("Waiting completed..." + "Date Time:" + DateTime.Now.ToString());
                    }
                }
            }
            sw.Close();
        }
    }
}
