// <copyright file="NativeMethods.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using System.Runtime.InteropServices;

namespace Dominator.Tools.Classes
{
    /// <summary>
    /// This class is shared between assemblies
    /// </summary>
    public static class NativeMethods
    {
        /// <summary>
        /// Get GEO identity of current user.
        /// </summary>
        /// <param name="geoId">GEO identity</param>
        /// <returns>current user GEO identity</returns>
        [DllImport("kernel32.dll")]
        public static extern int GetUserGeoID(int geoId);

        // Closes HANDLE object
        [DllImport("kernel32.dll")]
        public static extern bool CloseHandle(IntPtr handleObject);

        // Opens the access token associated with a process
        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool OpenProcessToken(IntPtr processHandle, uint desiredAccess, out IntPtr tokenHandle);

        [DllImport("Kernel32")]
        public static extern IntPtr OpenProcess(int desiredAccess, bool inheritClientHandle, uint processID);

        [DllImport("kernel32.dll")]
        public static extern int GetLastError();

        [Flags]
        internal enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            VirtualMemoryWrite = 0x00000020,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNamedPipeServerProcessId(IntPtr pipe, out uint serverProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNamedPipeClientProcessId(IntPtr pipe, out uint clientProcessId);
    }
}
