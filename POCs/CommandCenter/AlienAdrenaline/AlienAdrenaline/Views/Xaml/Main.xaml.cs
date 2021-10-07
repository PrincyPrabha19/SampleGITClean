using System;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using Application = System.Windows.Application;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
    public partial class Main
    {
        #region Properties
        private bool isControlRequest;        
        #endregion

        #region Constructors
        public Main()
        {
            InitializeComponent();
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;   
        }
        #endregion

        #region Event Handlers
        private void closeClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void unloadingAlienAdrenaline(CC_PlugIn.CommandCenter_UserControl uc)
        {
            isControlRequest = true;
            Close();
        }

        private void windowLoaded(object sender, RoutedEventArgs e)
        {
			AlienLabs.Tools.Classes.UsageTacking.Initialize(Properties.Resources.ProcessIdentifier);
        }

        private void windowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!isControlRequest)
                e.Cancel = !mainControl.Close(false);

            if (e.Cancel)
                return;

			AlienLabs.Tools.Classes.UsageTacking.Dispose(Properties.Resources.ProcessIdentifier);
            Application.Current.Shutdown();
        }

        private void Window_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!(e.Source is AlienAdrenalinePluginCtrl)) 
                DragMove();
        }
        #endregion
    }
}
