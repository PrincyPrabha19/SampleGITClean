using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using AlienLabs.CommandCenter.Tools;
using AlienLabs.Tools;
using AlienLabs.Tools.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        //private Mutex _mutex;

        #region Constructors
        App()
        {
            DispatcherUnhandledException += onUnhandledException;
        }
        #endregion

        #region Private Event Handlers
        private void onUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
            if (e.Exception is System.UnauthorizedAccessException)
                MsgBox.Show(AlienAdrenaline.App.Properties.Resources.ErrorText, AlienAdrenaline.App.Properties.Resources.AccessErrorText);
            else MsgBox.Show(AlienAdrenaline.App.Properties.Resources.ErrorText, e.Exception.ToString());

            Shutdown();
        }

        private void appStartup(object sender, StartupEventArgs args)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                AlienAdrenaline.App.Properties.Resources.Culture = UICulture.Current;

                string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                bool instanceRunning = ApplicationInstances.IsApplicationInstanceRunning(appName);

                if (instanceRunning)
                {
                    Shutdown();
                    return;
                }

                var mainWindow = new Main();
                mainWindow.Show();
            }
            catch (Exception ex)
            {
                MsgBox.Show("Error", ex.ToString());
                Shutdown();
            }
        }
        #endregion
    }
}
