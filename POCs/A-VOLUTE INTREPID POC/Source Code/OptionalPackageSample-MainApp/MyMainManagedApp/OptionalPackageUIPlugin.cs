using System;
using Windows.UI.Xaml.Controls;
using IInspectableParser;
using System.Runtime.InteropServices;
using System.Collections.Generic;

namespace MyMainManagedApp
{
    internal class OptionalPackageUiPlugin : IDisposable
    {
        private const string LoadPluginsCFunctionName = "LoadPlugins";

        private delegate void LoadPluginsDelegate(IntPtr inspectableGridPtr);
        private LoadPluginsDelegate _loadPluginsDelegate;

        //[return: MarshalAs(UnmanagedType.BStr)]
        //private delegate string GetPluginNameDelegate();
        //private GetPluginNameDelegate _getPluginNameDelegate;


        #region IDisposable Support
        private bool _disposedValue = false; // To detect redundant calls
        private UnmanagedLibrary _dllHost;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _dllHost.Dispose();
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~OptionalPackageUiPlugin() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion

        public static OptionalPackageUiPlugin CreateFromPackagePath(string packagePath)
        {
            var dllPath = packagePath + @"\NativeUIPlugin.dll";
            var dllHost = new UnmanagedLibrary(dllPath);
            var newOptionalPackage = new OptionalPackageUiPlugin
            {
                _dllHost = dllHost,
                _loadPluginsDelegate = dllHost.GetUnmanagedFunction<LoadPluginsDelegate>(LoadPluginsCFunctionName)
            };
            return newOptionalPackage;
        }

        //public string GetPluginName()
        //{
        //    return _getPluginNameDelegate.Invoke();
        //}

        //public void InsertUiPlugin(Grid gridHost)
        //{
        //    var inspectable = (IntPtr) CParser.GetInspectableFromObject(gridHost);
        //    _insertUiPluginDelegate.Invoke(inspectable);
        //}

        public void LoadPlugins(Object container)
        {
            var inspectable = (IntPtr)CParser.GetInspectableFromObject(container);
            _loadPluginsDelegate.Invoke(inspectable);
        }
    }
}
