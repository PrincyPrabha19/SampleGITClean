using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public class ProcessHelper
    {
        [DllImport("kernel32.dll")]
        private static extern bool QueryFullProcessImageName(IntPtr hprocess, int dwFlags, StringBuilder lpExeName, out int size);

        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        [Flags]
        enum ProcessAccessFlags : uint
        {
            PROCESS_QUERY_LIMITED_INFORMATION = 0x1000
        }

        public static Process GetApplicationProcess(string applicationPath)
        {
            foreach (var p in Process.GetProcesses())
            {
                try
                {
                    var fileName = getExecutablePath(p);
                    if (String.Compare(fileName, applicationPath, StringComparison.OrdinalIgnoreCase) == 0)
                        return p;
                }
                catch
                {
                }
            }

            return null;
        }

        public static Process GetSteamGameProcess(int steamPID, string gamePath)
        {
            var childProcesses = getChildrenProcesses(steamPID);

            Process steamGameProcess = (from p in childProcesses
                                        where
                                            p.MainModule != null &&
                                            p.MainModule.FileName.StartsWith(gamePath, StringComparison.InvariantCultureIgnoreCase)
                                        select p).FirstOrDefault();

            return steamGameProcess;
        }

        private static Process[] getChildrenProcesses(int parentPID)
        {
            var childProcesses = new List<Process>();
            
            foreach (var process in Process.GetProcesses())
            {
                if (String.Compare(process.ProcessName, "gameoverlayui", true) != 0 && getParentProcess(process.Id) == parentPID)
                    childProcesses.Add(process);
            }

            return childProcesses.ToArray();
        }

        private static int getParentProcess(int pID)
        {
            int parentPID = 0;
            
            using (var mo = new ManagementObject("win32_process.handle='" + pID + "'"))
            {
                mo.Get();
                parentPID = Convert.ToInt32(mo["ParentProcessId"]);
            }

            return parentPID;
        }

        public static string getExecutablePath(Process process) 
        {      
            if (Environment.OSVersion.Version.Major >= 6)
                return getExecutablePathAboveVista(process.Id);

            return process.MainModule.FileName; 
        }   
        
        private static string getExecutablePathAboveVista(int processId)
        {
            var buffer = new StringBuilder(1024);
            IntPtr hprocess = OpenProcess(ProcessAccessFlags.PROCESS_QUERY_LIMITED_INFORMATION, false, processId);    
            if (hprocess != IntPtr.Zero)
            {
                try
                {
                    int size = buffer.Capacity;
                    if (QueryFullProcessImageName(hprocess, 0, buffer, out size))
                    {
                        return buffer.ToString();
                    }
                }
                finally
                {
                    CloseHandle(hprocess);
                }
            }     
            
            throw new Win32Exception(Marshal.GetLastWin32Error());
        }          
    }
}
