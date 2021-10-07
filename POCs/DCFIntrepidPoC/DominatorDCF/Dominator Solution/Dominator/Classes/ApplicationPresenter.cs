using System.ComponentModel;
using System.Windows;
using Dominator.UI.Classes.Enums;
using Dominator.UI.ViewModels;
using Dominator.UI.Views;
using Dominator.Views;

namespace Dominator.Classes
{
    public static class ApplicationPresenter
    {
        #region Properties
        private static ShellViewModel shellViewModel;
        private static MainWindow mainWindow;
        #endregion

        #region Methods
        public static void ShowMainWindow()
        {
            shellViewModel = new ShellViewModel { View = new ShellView() };            
            shellViewModel.HostStartupView();
            
            mainWindow = new MainWindow { Content = (UIElement) shellViewModel.View };
            mainWindow.Closing += mainWindowsClosing;
            mainWindow.Show();

            shellViewModel.Initialize();
            shellViewModel.HostView(ViewTypes.Main);
        }
        #endregion

        #region Event Handlers
        private static void mainWindowsClosing(object sender, CancelEventArgs e)
        {
            shellViewModel.UnloadViews();
        } 
        #endregion
    }
}
