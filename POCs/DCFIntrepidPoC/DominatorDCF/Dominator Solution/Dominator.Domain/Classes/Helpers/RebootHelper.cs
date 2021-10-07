using System;
using System.Diagnostics;

namespace Dominator.Domain.Classes.Helpers
{
    public static class RebootHelper
    {
        public static void RestartSystem(bool restart = true, int delay = 0)
        {
            try
            {
                string value = Environment.GetEnvironmentVariable("windir");
                string RebootPath = System.IO.Path.Combine(value, "SYSTEM32", "shutdown.exe");

                var process = new Process
                {
                    StartInfo =
                    {
                        UseShellExecute = false,
                        CreateNoWindow = true,
                        FileName = RebootPath,
                        Arguments = restart ? "/r /t 0" : "/s /t" + delay
                    }
                };
                process.Start();
            }
            catch (Exception)
            {
            }
        }
    }
}
