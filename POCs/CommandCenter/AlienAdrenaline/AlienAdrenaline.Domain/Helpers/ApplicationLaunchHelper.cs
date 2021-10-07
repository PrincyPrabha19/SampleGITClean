using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using AlienLabs.AlienAdrenaline.Tools;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public class ApplicationLaunchHelper
    {
        #region Public Methods
        public static void Execute(string path, out Process process)
        {
            Execute(path, null, out process);
        }

        public static void Execute(string path, string arguments, out Process process)
        {
            string filename, _arguments;
            if (!FilePathHelper.IsValidPath(path, out filename, out _arguments))
                throw new FileNotFoundException();

            launchApplication(filename, arguments ?? _arguments, out process);
        }

        public static void Execute(string path, bool launchIfNotOpen, out Process process)
        {
            process = null;
            
            string filename, arguments;            
            if (!FilePathHelper.IsValidPath(path, out filename, out arguments))
                throw new FileNotFoundException();

            if (launchIfNotOpen)
            {
                if (!isApplicationAlreadyRunning(filename))
                    launchApplication(filename, arguments, out process);
            }
            else
            {
                if (isApplicationAlreadyRunning(filename))
                    closeApplication(filename);
            }
        }
        #endregion

        #region Private Methods
        private static void launchApplication(string filename, string arguments, out Process process)
        {
            process = new Process
            {
                StartInfo = { WorkingDirectory = Path.GetDirectoryName(filename), FileName = filename, Arguments = arguments }
            };

            process.Start();
        }

        private static void closeApplication(string filename)
        {
            Process[] processes = getProcessFromFilename(filename);
            if (processes != null)
                foreach (var process in processes)
                    process.Kill();
        }

        private static bool isApplicationAlreadyRunning(string filename)
        {
            var processes = getProcessFromFilename(filename);
            return processes != null && processes.Length > 0;
        }

        private static Process[] getProcessFromFilename(string filename)
        {
            Process[] processes = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(filename));
            return (from p in processes
                    where p.MainModule != null && String.Compare(p.MainModule.FileName, filename, true) == 0
                    select p).ToArray();
        }
        #endregion
    }
}
