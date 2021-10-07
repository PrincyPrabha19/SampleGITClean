using System;
using System.Runtime.InteropServices;

namespace Dominator.Tools.Classes {

    public static class ProcessAPIMap {

        public const uint INFINITE = 0xFFFFFFFF;
        public const uint NoUserSessionId = 0xFFFFFFFF;
        public const int GENERIC_ALL_ACCESS = 0x10000000;
        public const uint QUERY_INFORMATION = 0x00000400;
        public const uint STANDARD_RIGHTS_READ = 0x00020000;
        public const uint STANDARD_RIGHTS_WRITE = 0x00020000;
        public const uint TOKEN_ADJUST_PRIVILEGES = 0x0020;
        public const uint TOKEN_QUERY = 0x0008;
        public const uint TOKEN_DUPLICATE = 0x0002;
        public const uint TOKEN_ASSIGN_PRIMARY = 0x0001;
        public const uint TOKEN_ADJUST_SESSIONID = 0x0100;
        public const uint TOKEN_ADJUST_GROUPS = 0x0040;
        public const uint TOKEN_ADJUST_DEFAULT = 0x0080;
        public const uint TOKEN_READ = STANDARD_RIGHTS_READ | TOKEN_QUERY;
        public const uint TOKEN_WRITE = STANDARD_RIGHTS_WRITE | TOKEN_ADJUST_PRIVILEGES | TOKEN_ADJUST_GROUPS | TOKEN_ADJUST_DEFAULT;
        public const uint TOKEN_FOR_OPEN_PROCESS =
            TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY | TOKEN_DUPLICATE | TOKEN_ASSIGN_PRIMARY |
            TOKEN_ADJUST_SESSIONID | TOKEN_READ | TOKEN_WRITE;

        [DllImport("kernel32.dll", EntryPoint = "CloseHandle", SetLastError = true, CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        public static extern bool CreateProcessAsUser(
            IntPtr hToken, 
            string lpApplicationName, 
            string lpCommandLine, 
            ref SECURITY_ATTRIBUTES lpProcessAttributes,
            ref SECURITY_ATTRIBUTES lpThreadAttributes, 
            bool bInheritHandle, 
            Int32 dwCreationFlags, 
            IntPtr lpEnvrionment, 
            string lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo, 
            ref PROCESS_INFORMATION lpProcessInformation
        );

        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        public static extern bool DuplicateTokenEx(
            IntPtr hExistingToken, 
            Int32 dwDesiredAccess, 
            ref SECURITY_ATTRIBUTES lpThreadAttributes,
            Int32 ImpersonationLevel, 
            Int32 dwTokenType, 
            ref IntPtr phNewToken
        );

        [DllImport("kernel32.dll")]
        public static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("kernel32", SetLastError = true, ExactSpelling = true)]
        public static extern Int32 WaitForSingleObject(
            IntPtr handle, 
            Int32 milliseconds
        );

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(
            IntPtr ProcessHandle, 
            UInt32 DesiredAccess, 
            out IntPtr TokenHandle
        );

        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(
            uint dwDesiredAccess, 
            [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle, 
            int dwProcessId
        );

    }

    [StructLayout(LayoutKind.Sequential)]
    public struct STARTUPINFO {
        public Int32 cb;
        public string lpReserved;
        public string lpDesktop;
        public string lpTitle;
        public Int32 dwX;
        public Int32 dwY;
        public Int32 dwXSize;
        public Int32 dwXCountChars;
        public Int32 dwYCountChars;
        public Int32 dwFillAttribute;
        public Int32 dwFlags;
        public Int16 wShowWindow;
        public Int16 cbReserved2;
        public IntPtr lpReserved2;
        public IntPtr hStdInput;
        public IntPtr hStdOutput;
        public IntPtr hStdError;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct PROCESS_INFORMATION {
        public IntPtr hProcess;
        public IntPtr hThread;
        public Int32 dwProcessID;
        public Int32 dwThreadID;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SECURITY_ATTRIBUTES {
        public Int32 Length;
        public IntPtr lpSecurityDescriptor;
        public bool bInheritHandle;
    }

    public enum SECURITY_IMPERSONATION_LEVEL {
        SecurityAnonymous,
        SecurityIdentification,
        SecurityImpersonation,
        SecurityDelegation
    }

    public enum TOKEN_TYPE {
        TokenPrimary = 1,
        TokenImpersonation
    }

}