using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dominator.Tools.Classes
{
    public class ProcessAPI
    {
        public void StartAsInteractiveUser(string fullPath)
        {
            attributes = newAttributes();
            STARTUPINFO startupInfo = newStartupInfo();
            PROCESS_INFORMATION processInfo = new PROCESS_INFORMATION();
            IntPtr duplicatedUserToken = IntPtr.Zero;

            try {

                duplicatedUserToken = getDuplicatedUserToken();

                ProcessAPIMap.CreateProcessAsUser(
                    duplicatedUserToken,
                    fullPath,
                    String.Empty,
                    ref attributes,
                    ref attributes,
                    false, 0, IntPtr.Zero, @"C:\",
                    ref startupInfo,
                    ref processInfo);

            } finally {

                closeHandle(processInfo.hProcess);
                closeHandle(processInfo.hThread);
                closeHandle(duplicatedUserToken);

            }
        }

        //public bool existInteractiveUser() {
        //    return ProcessAPIMap.WTSGetActiveConsoleSessionId() !=
        //           ProcessAPIMap.NoUserSessionId;
        //}

        private void closeHandle(IntPtr handle) {
            if (handle != IntPtr.Zero) ProcessAPIMap.CloseHandle(handle);
        }

        private SECURITY_ATTRIBUTES attributes;

        private STARTUPINFO newStartupInfo() {
            STARTUPINFO startupInfo = new STARTUPINFO();
            startupInfo.cb = Marshal.SizeOf(startupInfo);
            startupInfo.lpDesktop = String.Empty;
            return startupInfo;
        }

        private SECURITY_ATTRIBUTES newAttributes() {
            SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
            sa.Length = Marshal.SizeOf(sa);
            return sa;
        }

        private IntPtr getDuplicatedUserToken() {

            IntPtr result = IntPtr.Zero;
            IntPtr userToken = getUserToken();

            ProcessAPIMap.DuplicateTokenEx(
                userToken,
                ProcessAPIMap.GENERIC_ALL_ACCESS,
                ref attributes,
                (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, 
                (int)TOKEN_TYPE.TokenPrimary, 
                ref result
            );

            return result;
        }

        private IntPtr getUserToken() {

            IntPtr result;
            IntPtr explorerProcess = getExplorerProcess();

            ProcessAPIMap.OpenProcessToken(
                explorerProcess, 
                ProcessAPIMap.TOKEN_FOR_OPEN_PROCESS, 
                out result
            );

            return result;
        }

        private IntPtr getExplorerProcess()
        {
            int interactiveUserSessionId = (int)ProcessAPIMap.WTSGetActiveConsoleSessionId();

            Process[] explorers = Process.GetProcessesByName("explorer");
            foreach (Process process in explorers)
                if (process.SessionId == interactiveUserSessionId)
                    return ProcessAPIMap.OpenProcess(
                        ProcessAPIMap.QUERY_INFORMATION,
                        true,
                        process.Id
                    );

            return IntPtr.Zero;
        }
    }
}
