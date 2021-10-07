using System;
using System.Globalization;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using AlienLabs.Tools;
using AlienLabs.Tools.Classes;
using ObjectFactory = AlienLabs.AlienAdrenaline.GameModeProcessor.Classes.Factories.ObjectFactory;

namespace AlienLabs.AlienAdrenaline.GameModeProcessor.Views.Xaml
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
                MsgBox.Show(GameModeProcessor.Properties.Resources.ErrorText, GameModeProcessor.Properties.Resources.AccessErrorText);
            else MsgBox.Show(GameModeProcessor.Properties.Resources.ErrorText, e.Exception.ToString());

            Shutdown();
        }

        private void appStartup(object sender, StartupEventArgs args)
        {
            try
            {
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                GameModeProcessor.Properties.Resources.Culture = CultureInfo.CurrentUICulture;

                if (args.Args.Length < 1)
                {
                    MsgBox.Show(GameModeProcessor.Properties.Resources.ErrorText, GameModeProcessor.Properties.Resources.ErrorGameModeNameExpectedText);
                    Shutdown();
                    return;
                }

                string appName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
                bool instanceRunning = ApplicationInstances.IsApplicationInstanceRunning(appName);

                if (instanceRunning)
                {
                    MsgBox.Show(GameModeProcessor.Properties.Resources.ErrorText, GameModeProcessor.Properties.Resources.GameModeInstanceRunningText);
                    Shutdown();
                    return;
                }

                var mainWindow = new Main() { GameModeName = args.Args[0] };
                var presenter = ObjectFactory.NewMainPresenter(mainWindow);
                mainWindow.Presenter = presenter;
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
