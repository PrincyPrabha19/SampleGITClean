using System;
using System.Threading.Tasks;
using Windows.Networking.Vpn;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;
using Server;

namespace Client
{
    public sealed partial class MainPage
    {
        #region Properties
        private AlienFXComponent afx;
        private readonly uint[] colors = { 0x0F0F0000, 0x0F000F00, 0x0F00000F };
        private int colorIndex;
        private bool continuePolling;

        private static readonly DependencyProperty interval = DependencyProperty.Register("Interval", typeof(int), typeof(MainPage), new PropertyMetadata(10));
        public int Interval
        {
            get { return (int)GetValue(interval); }
            set { SetValue(interval, value); }
        }
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

            continuePolling = true;
            btnColor.IsEnabled = false;
            processColors();
        }

        private async void processColors()
        {
            const int ITERATIONS = 500;
            var startTime = DateTime.Now;
            var i = 0;
            for(i = 0; i < ITERATIONS && continuePolling; i++) 
            {
                var result = afx?.SetLightColor(0x07, Convert.ToUInt32(colors[colorIndex])) ?? false;
                textBlockStatus.Text = result ? $"Iteration {i}/{ITERATIONS}. Color set to: {colors[colorIndex]:X8}" : "Failed to set color ...";
            
                if (++colorIndex == 3)
                    colorIndex = 0;
            
                await Task.Delay(TimeSpan.FromMilliseconds(Interval));
            }
            textBlockStatus.Text = $"Time spent (ms) in {i}/{ITERATIONS} iterations: {(DateTime.Now - startTime)}";
            release();
        }

        private void release(object sender, RoutedEventArgs e)
        {
            release();
        }

        private void release()
        {
            if (afx == null)
                return;

            continuePolling = false;
            btnInit.IsEnabled = true;
            btnColor.IsEnabled = false;
            btnRelease.IsEnabled = false;
            colorIndex = 0;
            afx = null;
        }
        #endregion
    }
}
