using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace RunCommands
{
    class Program
    {
        static StreamWriter sw = null;
        static void CreateLog()
        {
            try
            {
                string temppath = Path.GetTempPath();
                string filename = String.Format("{0}__{1}", DateTime.Now.ToString("yyyyMMddhhnnss"), "RunCommandsLog");
                string path = Path.Combine(temppath, filename);
                sw = new StreamWriter(path, true);
            }
            catch (Exception ex)
            {

            }
        }
        static void CloseLog()
        {
            sw.Close();
        }
        static void Log(string logtxt)
        {
            if (sw != null)
            {
                sw.WriteLine(logtxt);
                sw.Flush();
            }
        }
        /// @summary
        /// This function gives two strings in string[]
        /// string[0] will be uninstaller
        /// string[1] will be rest of command line
        /// Example input C:\ProgramData\Package Cache\{27f62ca6-f3b1-463e-a2ff-fdcb721925d6}\AW988HWControlInstaller.exe /s /uninstall
        /// string[0] = "C:\ProgramData\Package Cache\{27f62ca6-f3b1-463e-a2ff-fdcb721925d6}\AW988HWControlInstaller.exe"
        /// string[1] =  /s /uninstall
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
        /// @summary
        /// Slits the CSV
        static List<string> SplitCommands(string[] args)
        {
            List<string> cmds = new List<string>();
            string fullstring = string.Join(" ", args);
            cmds = fullstring.Split(',').ToList<string>();
            return cmds;

        }
        static void RunCommands(List<string> cmds)
        {
            foreach (string cmd in cmds)
            {
                var result = SplitExe(cmd);
                if (result.Count == 2)
                {
                    string exe = result[0].Trim();
                    string cmdline = result[1].Trim();
                    Log("Command= " + exe);
                    Log("Command line Arguments= " + cmdline);
                    try
                    {
                        using (Process exec = new Process())
                        {
                            exec.StartInfo.UseShellExecute = false;
                            exec.StartInfo.FileName = exe;
                            exec.StartInfo.Arguments = cmdline;
                            exec.StartInfo.CreateNoWindow = true;
                            exec.Start();
                            exec.WaitForExit();

                        }
                    }
                    catch (Exception ex)
                    {
                        Log("Exception:" + ex.Message);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            CreateLog();
            if (args.Length > 0)
            {
                var csvsplit = SplitCommands(args);

                RunCommands(csvsplit);
            }
            CloseLog();
        }
    }
}

