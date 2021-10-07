using System;
using System.Linq.Expressions;
using Windows.UI;
using Windows.UI.Xaml;
using Server;

namespace Client
{
    public sealed partial class MainPage
    {
        #region Properties
        private AlienFXComponent afx;
        private readonly uint[] colors = { 0x0F0F0000, 0x0F000F00, 0x0F00000F };
        private int colorIndex;
        #endregion

        #region Constructor
        public MainPage()
        {
            InitializeComponent();
        }
        #endregion

        #region Event Handlers
        private void inititalize(object sender, RoutedEventArgs e)
        {
            if (afx != null)
                return;

            afx = new AlienFXComponent();
            if (afx.InitializeAPI())
            {
                btnInit.IsEnabled = false;
                btnColor.IsEnabled = true;
                btnRelease.IsEnabled = true;
                colorIndex = 0;
            }
            else
            {
                textBlockStatus.Text = "Initialization failed ...";
                afx = null;
            }
        }

        private void setColor(object sender, RoutedEventArgs e)
        {
            if (afx == null)
                return;

            var result = afx.SetLightColor(0x07, Convert.ToUInt32(colors[colorIndex]));
            textBlockStatus.Text = result ? $"Color set to: {colors[colorIndex]:X8}" : "Failed to set color ...";

            if (++colorIndex == 3)
                colorIndex = 0;
        }

        private void release(object sender, RoutedEventArgs e)
        {
            if (afx == null)
                return;

            var result = afx.ReleaseAPI();
            if (result)
            {
                btnInit.IsEnabled = true;
                btnColor.IsEnabled = false;
                btnRelease.IsEnabled = false;
                colorIndex = 0;
                afx = null;
            }
            else
                textBlockStatus.Text = "Release failed ...";
        }
        #endregion
    }
}
