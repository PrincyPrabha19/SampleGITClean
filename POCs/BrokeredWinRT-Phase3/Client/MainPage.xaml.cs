using System;
using System.Diagnostics;
using System.IO;
using Windows.UI.Xaml;
using Server;

namespace Client
{
    public sealed partial class MainPage
    {
        #region Properties
        private AlienFXComponent afx;
        #endregion

        #region Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void discover(object sender, RoutedEventArgs e)
        {
            if (afx != null)
                return;

            afx = new AlienFXComponent();
            try
            {
                var devices = afx.DiscoveDevices();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
        #endregion
    }
}
