using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using Dominator.Classes;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Helpers;
using Microsoft.Win32;

namespace Dominator
{
    public partial class App
    {
        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        App()
        {
            DispatcherUnhandledException += onUnhandledException;
        }

        private void onUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            logger?.WriteError("App.OnUnhandledException", null, e.Exception.ToString());
            MessageBox.Show(e.Exception.ToString(), Dominator.Properties.Resources.ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            Shutdown();
        }

        private void startup(object sender, StartupEventArgs args)
        {
            if (SingleApplicationDetector.IsRunning())
            {
                Shutdown();
                return;
            }

#if !DEBUG
            if (!args.Args.Contains("/standalone"))
            {
                string appPath, appArgs;
                if (readPathsFromRegistry(out appPath, out appArgs))
                {
                    Shutdown();
                    Process.Start(appPath, appArgs);
                    return;
                }                
            }
#endif

            try
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                Dominator.Properties.Resources.Culture = UICulture.Current;
                ApplicationPresenter.ShowMainWindow();
            }
            catch (Exception e)
            {
                logger?.WriteError("App.Startup", null, e.ToString());
                Shutdown();
            }
        }

        private static bool readPathsFromRegistry(out string appPath, out string appArgs)
        {
            appPath = string.Empty;
            appArgs = string.Empty;

            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\Command Center", false))
                {
                    var _installPath = key?.GetValue("S1_KEY");
                    if (_installPath != null)
                    {
                        appPath = _installPath.ToString();
                        if (!string.IsNullOrEmpty(appPath) && File.Exists(appPath))
                        {
                            appArgs = "/L \"OC Controls\"";
                            return true;
                        }
                    }
                }
            }
            catch (Exception e)
            {                
            }

            return false;
        }
    }
}
