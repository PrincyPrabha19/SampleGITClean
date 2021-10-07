// <copyright file="Impersonator.cs" company="Dell Inc.">
//      Copyright (c) Dell Inc. All rights reserved.
// </copyright>

using System;
using System.Management;
using System.Runtime.CompilerServices;
using System.Security.Principal;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Tools.Classes
{
    /// <summary>
    /// Get windows use access rights.
    /// </summary>
    public static class Impersonator
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        /// <summary>
        /// process ID.
        /// </summary>
        private static uint processId;

        /// <summary>
        /// windows identity.
        /// </summary>
        private static WindowsIdentity identityForImpersonation;

        /// <summary>
        /// Gets or sets the process used for impersonation
        /// </summary>
        public static uint ProcessId
        {
            get { return processId; }
            set { processId = value; }
        }

        /// <summary>
        /// This method wraps an action in some impersonation
        /// </summary>
        /// <param name="action">Action what to be run under </param>
        /// <returns>Success or not.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static bool RunImpersonated(Action action)
        {
            try
            {
                using (getIdentityForImpersonation().Impersonate())
                {
                    action();
                }

                return true;
            }
            catch (InvalidOperationException)
            {
                action();
                return false;
            }
            catch (System.Security.SecurityException)
            {
                action();
                return false;
            }
        }

        /// <summary>
        /// The Security token used for impersonation
        /// </summary>
        /// <returns>Windows identity.</returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        private static WindowsIdentity getIdentityForImpersonation()
        {
            if (identityForImpersonation != null)
            {
                return identityForImpersonation;
            }

            logger?.WriteLine("Getting identity of desktop user.");

            // step 1: Do we have an existing process?
            if (processId != 0)
            {
                IntPtr securityToken = getSecurityToken(processId);
                if (securityToken != IntPtr.Zero)
                {
                    logger?.WriteLine($"Getting identity from process id {processId}.");
                    identityForImpersonation = new WindowsIdentity(securityToken);
                    NativeMethods.CloseHandle(securityToken);
                }
            }

            // step 2: no luck with an existing process ID.  Let's look for explorer.exe
            // NOTE: hard coded
            processId = getProcessIdFromExePath(Environment.ExpandEnvironmentVariables("%SystemDrive%\\WINDOWS\\EXPLORER.EXE"));

            if (processId != 0)
            {
                IntPtr securityToken = getSecurityToken(processId);
                logger?.WriteLine($"Getting identity from process id {processId}.");

                if (securityToken != IntPtr.Zero)
                {
                    identityForImpersonation = new WindowsIdentity(securityToken);
                    NativeMethods.CloseHandle(securityToken);
                }
            }

            // if we got here, we failed
            logger?.WriteLine(identityForImpersonation != null
                ? $"Retrieved identity of {identityForImpersonation.Name} for impersonation."
                : "Failed to get identity of the desktop user.");

            return identityForImpersonation;
        }

        /// <summary>
        /// Gets the id of a process given an executable path
        /// </summary>
        /// <param name="executablePath">Executable file path.</param>
        /// <returns>Process ID.</returns>
        private static uint getProcessIdFromExePath(string executablePath)
        {
            logger?.WriteLine($"Getting process ID for '{executablePath}'");

            uint id = 0;

            Tryblock.Run(() =>
            {
                using (ManagementObjectSearcher procs = new ManagementObjectSearcher("SELECT * FROM Win32_Process"))
                {
                    foreach (ManagementObject proc in procs.Get())
                    {
                        if (proc["ExecutablePath"] == null)
                        {
                            continue;
                        }

                        if (proc["ExecutablePath"].ToString().ToUpperInvariant() == executablePath.ToUpperInvariant())
                        {
                            id = (uint)proc["ProcessId"];
                            break;
                        }
                    }
                }
            });

            logger?.WriteLine($"Returning process ID {id} for '{executablePath}'");

            return id;
        }

        /// <summary>
        /// Returns a security token for a given process ID
        /// </summary>
        /// <param name="procId">process ID.</param>
        /// <returns>Windows pointer.</returns>
        private static IntPtr getSecurityToken(uint procId)
        {
            IntPtr token = IntPtr.Zero;
            const int PROCESS_ALL_ACCESS = -1;
            IntPtr uiProcess = NativeMethods.OpenProcess(PROCESS_ALL_ACCESS, false, procId);
            if (uiProcess == IntPtr.Zero)
            {
                int errorCode = NativeMethods.GetLastError();
                logger?.WriteLine($"Can't open process id {procId} (Error {errorCode})");
            }
            else
            {

                Tryblock.Run(() =>
                {
                    IntPtr tmpToken;
                    if (!NativeMethods.OpenProcessToken(uiProcess, (uint)TokenAccessLevels.MaximumAllowed, out tmpToken))
                    {
                        int errorCode = NativeMethods.GetLastError();
                        logger?.WriteLine($"Can't get security token for process id {procId} (Error {errorCode})");
                    }
                    else
                    {
                        token = tmpToken;
                    }
                }, ex => logger?.WriteLine($"Error getting token from process id {procId}: {ex.Message}"), () => NativeMethods.CloseHandle(uiProcess));
            }

            return token;
        }
    }
}
