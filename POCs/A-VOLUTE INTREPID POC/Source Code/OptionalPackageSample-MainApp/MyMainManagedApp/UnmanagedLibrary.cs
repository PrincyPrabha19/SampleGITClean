﻿using System;
using System.Runtime.InteropServices;

// ADAPTED CODE FROM https://blogs.msdn.microsoft.com/jmstall/2007/01/06/type-safe-managed-wrappers-for-kernel32getprocaddress/

namespace MyMainManagedApp
{

    /// <summary>
    /// Utility class to wrap an unmanaged DLL and be responsible for freeing it.
    /// Allow the caller to call DLLs that are unknown at build time (without DllImport)
    /// </summary>
    /// <remarks>This is a managed wrapper over the native LoadLibrary, GetProcAddress, and
    /// FreeLibrary calls.</remarks>
    public sealed class UnmanagedLibrary : IDisposable
    {
        #region Native imports

        static class NativeMethods
        {
            [DllImport("kernel32", CharSet = CharSet.Unicode, BestFitMapping = false, SetLastError = true)]
            public static extern SafeLibraryHandle LoadLibrary(string fileName);

            [DllImport("kernel32", SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool FreeLibrary(IntPtr hModule);

            [DllImport("kernel32")]
            public static extern IntPtr GetProcAddress(SafeLibraryHandle hModule, String procname);

        }

        public sealed class SafeLibraryHandle : SafeHandle
        {
            private SafeLibraryHandle() : base(new IntPtr(-1), true)
            {
            }

            public SafeLibraryHandle(IntPtr invalidHandleValue, bool ownsHandle) : base(invalidHandleValue, ownsHandle)
            {
            }

            public override bool IsInvalid => (int) handle == -1 || handle == IntPtr.Zero;

            protected override bool ReleaseHandle()
            {
                return NativeMethods.FreeLibrary(handle);
            }

        }
        #endregion // Safe Handles and Native imports

        /// <summary>
        /// Constructor to load a dll and be responible for freeing it.
        /// </summary>
        /// <param name="fileName">full path name of dll to load</param>
        /// <exception cref="System.IO.FileNotFoundException">if fileName can’t be found</exception>
        /// <remarks>Throws exceptions on failure. Most common failure would be file-not-found, or
        /// that the file is not a  loadable image.</remarks>
        public UnmanagedLibrary(string fileName)
        {
            _hLibrary = NativeMethods.LoadLibrary(fileName);
            if (_hLibrary.IsInvalid)
            {
                var hr = Marshal.GetHRForLastWin32Error();
                Marshal.ThrowExceptionForHR(hr);
            }

        }

        /// <summary>
        /// Dynamically lookup a function in the dll via kernel32!GetProcAddress.
        /// </summary>
        /// <param name="functionName">raw name of the function in the export table.</param>
        /// <returns>null if function is not found. Else a delegate to the unmanaged function.
        /// </returns>
        /// <remarks>GetProcAddress results are valid as long as the dll is not yet unloaded. This
        /// is very very dangerous to use since you need to ensure that the dll is not unloaded
        /// until after you’re done with any objects implemented by the dll. For example, if you
        /// get a delegate that then gets an IUnknown implemented by this dll,
        /// you can not dispose this library until that IUnknown is collected. Else, you may free
        /// the library and then the CLR may call release on that IUnknown and it will crash.</remarks>
        public TDelegate GetUnmanagedFunction<TDelegate>(string functionName) where TDelegate : class
        {
            var procAddress = NativeMethods.GetProcAddress(_hLibrary, functionName);
            // Failure is a common case, especially for adaptive code.
            if (procAddress == IntPtr.Zero)
            {
                return null;
            }
            return Marshal.GetDelegateForFunctionPointer<TDelegate>(procAddress);
        }

        #region IDisposable Members
        /// <summary>
        /// Call FreeLibrary on the unmanaged dll. All function pointers
        /// handed out from this class become invalid after this.
        /// </summary>
        /// <remarks>This is very dangerous because it suddenly invalidate
        /// everything retrieved from this dll. This includes any functions
        /// handed out via GetProcAddress, and potentially any objects returned
        /// from those functions (which may have an implemention in the
        /// dll).
        /// </remarks>
        public void Dispose()
        {
            _hLibrary.Dispose();
        }

        // Unmanaged resource. CLR will ensure SafeHandles get freed, without requiring a finalizer on this class.
        private readonly SafeLibraryHandle _hLibrary;
        #endregion

    } // UnmanagedLibrary
}
