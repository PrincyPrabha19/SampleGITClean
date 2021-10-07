using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using Microsoft.Win32;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class SupportAssistServiceHelper
    {
        private const string SERVICE_NAME = "SupportAssistAgent";

        [StructLayout(LayoutKind.Sequential)]
        internal sealed class SERVICE_STATUS_PROCESS
        {
            [MarshalAs(UnmanagedType.U4)]
            public uint dwServiceType;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwCurrentState;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwControlsAccepted;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwWin32ExitCode;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwServiceSpecificExitCode;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwCheckPoint;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwWaitHint;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwProcessId;
            [MarshalAs(UnmanagedType.U4)]
            public uint dwServiceFlags;
        }

        internal const int ERROR_INSUFFICIENT_BUFFER = 0x7a;
        internal const int SC_STATUS_PROCESS_INFO = 0;

        [DllImport("advapi32.dll", SetLastError = true)]
        internal static extern bool QueryServiceStatusEx(SafeHandle hService, int infoLevel, IntPtr lpBuffer, uint cbBufSize, out uint pcbBytesNeeded);

        public static int GetServiceProcessId(this ServiceController sc)
        {
            if (sc == null) return -1;

            IntPtr zero = IntPtr.Zero;

            try
            {
                UInt32 dwBytesNeeded;
                // Call once to figure the size of the output buffer.
                QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, zero, 0, out dwBytesNeeded);
                if (Marshal.GetLastWin32Error() == ERROR_INSUFFICIENT_BUFFER)
                {
                    // Allocate required buffer and call again.
                    zero = Marshal.AllocHGlobal((int)dwBytesNeeded);

                    if (QueryServiceStatusEx(sc.ServiceHandle, SC_STATUS_PROCESS_INFO, zero, dwBytesNeeded, out dwBytesNeeded))
                    {
                        var ssp = new SERVICE_STATUS_PROCESS();
                        Marshal.PtrToStructure(zero, ssp);
                        return (int)ssp.dwProcessId;
                    }
                }
            }
            catch (Exception e) { }
            finally
            {
                if (zero != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(zero);
                }
            }

            return -1;
        }

        public static bool IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == SERVICE_NAME);
        }

        public static bool IsServiceInstalled(out int processID)
        {
            processID = -1;
            var serviceController = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == SERVICE_NAME);
            if (serviceController == null) return false;
            processID = serviceController.GetServiceProcessId();
            return true;
        }

        public static bool GetSupportAssistPath(int processID, out string appPath)
        {
            appPath = string.Empty;

            var process = Process.GetProcesses().FirstOrDefault(p => p.Id == processID);
            string dirName = Path.GetDirectoryName(process?.MainModule.FileName);
            if (string.IsNullOrEmpty(dirName)) return false;
            appPath = Path.Combine(dirName, @"..\");
            return true;
        }

        public static bool GetSupportAssistPath(out string appPath)
        {
            appPath = string.Empty;
                              
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Dell Support Center", false))
                {
                    var installLocation = key?.GetValue("InstallLocation");
                    if (installLocation != null)
                    {
                        appPath = Path.Combine(installLocation.ToString(), "pcdrcui.exe");
                        return true;
                    }
                }
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
