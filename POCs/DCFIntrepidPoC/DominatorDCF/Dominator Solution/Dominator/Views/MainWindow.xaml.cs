using System;
using System.ComponentModel;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using Dominator.Classes;
using Dominator.UI.Classes.Helpers;

namespace Dominator.Views
{
    public partial class MainWindow
    {
        #region Constructor
        public MainWindow()
        {
            InitializeComponent();
            ResourceDictionaryLoader.LoadInto(Resources);
            Style = (Style)Resources["CustomChrome"];
        }
        #endregion

        #region Event Handlers
        private void loaded(object sender, RoutedEventArgs e)
        {
            var thread = new Thread(loadAsync) { Priority = ThreadPriority.Highest };
            thread.Start();
        }

        private void closing(object sender, CancelEventArgs e)
        {
            saveWindowStateAndLocation();
        }

        private void closed(object sender, System.EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void stateChanged(object sender, EventArgs e)
        {
            updateWindowButtonControlVisibility();
        }

        private void minimize(object sender, RoutedEventArgs e)
        {
            var mainWindow = getMainWindow(sender as FrameworkElement);
            if (mainWindow == null) return;

            mainWindow.WindowState = WindowState.Minimized;
        }

        private void restore(object sender, RoutedEventArgs e)
        {
            var mainWindow = getMainWindow(sender as FrameworkElement);
            if (mainWindow == null) return;

            mainWindow.WindowState = WindowState.Normal;
        }

        private void maximize(object sender, RoutedEventArgs e)
        {
            var mainWindow = getMainWindow(sender as FrameworkElement);
            if (mainWindow == null) return;

            mainWindow.WindowState = WindowState.Maximized;
        }

        private void close(object sender, RoutedEventArgs e)
        {
            var mainWindow = getMainWindow(sender as FrameworkElement);
            if (mainWindow == null) return;

            mainWindow.Close();
        }
        #endregion

        #region Methods
        private void updateWindowButtonControlVisibility()
        {
            var buttonMaximize = Template.FindName("buttonMaximize", this) as Button;
            var buttonRestore = Template.FindName("buttonRestore", this) as Button;
            if (buttonRestore == null || buttonMaximize == null) return;

            switch (WindowState)
            {
                case WindowState.Normal:
                    buttonMaximize.Visibility = Visibility.Visible;
                    buttonRestore.Visibility = Visibility.Collapsed;
                    break;
                case WindowState.Maximized:
                    buttonMaximize.Visibility = Visibility.Collapsed;
                    buttonRestore.Visibility = Visibility.Visible;
                    break;
            }
        }

        private MainWindow getMainWindow(FrameworkElement sender)
        {
            if (sender == null) return null;
            return sender.Tag as MainWindow;
        }

        private void loadAsync()
        {
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(restoreWindowStateAndLocation));
            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(updateWindowButtonControlVisibility));
        }

        private void restoreWindowStateAndLocation()
        {
            var ws = WindowState.Maximized;

            var windowControlHelper = new WindowControlHelper();
            try
            {
                var windowState = windowControlHelper.GetWindowState();
                ws = (WindowState)Enum.Parse(typeof(WindowState), windowState);
            }
            catch (Exception)
            {
            }

            WindowState = ws;

            try
            {
                int left, top, width, height;
                if (windowControlHelper.GetWindowLocation(out left, out top, out width, out height))
                {
                    Left = left;
                    Top = top;

                    if (WindowState == WindowState.Normal)
                    {
                        Width = width;
                        Height = height;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void saveWindowStateAndLocation()
        {
            var windowControlWriter = new WindowControlHelper();
            try
            {
                if (WindowState == WindowState.Normal || WindowState == WindowState.Maximized)
                    windowControlWriter.SetWindowState(WindowState.ToString());
            }
            catch (Exception)
            {
            }

            try
            {
                windowControlWriter.SetWindowLocation((int)Left, (int)Top, (int)Width, (int)Height);
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
