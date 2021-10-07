using System;
using System.Timers;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;

namespace RTPMGadget
{
	public partial class MainWindow
	{
		#region Properties	    
	    private ConfigurableUIEventManager refreshUIEventManager;
		private bool refreshingUIBecauseOfExternalProcess;
		private bool recording;
        private readonly Timer recordingTimer = new Timer { Interval = 300 };
        private readonly TimeRecordingHelper timeRecordingHelper = new TimeRecordingHelper();

        private static readonly DependencyProperty timeElapsedSecondsProperty = DependencyProperty.Register("TimeElapsedSeconds", typeof(int), typeof(MainWindow), new UIPropertyMetadata(0));
        public int TimeElapsedSeconds
        {
            get { return (int)GetValue(timeElapsedSecondsProperty); }
            set { SetValue(timeElapsedSecondsProperty, value); }
        }

        private static readonly DependencyProperty startStopRecordingTooltipProperty = DependencyProperty.Register("StartStopRecordingTooltip", typeof(string), typeof(MainWindow));
        public string StartStopRecordingTooltip
        {
            get { return (string)GetValue(startStopRecordingTooltipProperty); }
            set { SetValue(startStopRecordingTooltipProperty, value); }
        }
		#endregion

		#region Constructor
		public MainWindow()
		{
			InitializeComponent();
			initRefreshUIEventManager();

		    recordingTimer.Elapsed += recordingTimer_Elapsed;

            var recordingKeystrokes = Properties.Resources.CtrlShiftAltF10;
            if (AlienLabs.Tools.SystemInfo.Platform.ToString().StartsWith("Mobile", StringComparison.InvariantCultureIgnoreCase))
                recordingKeystrokes = Properties.Resources.FnF10;
            StartStopRecordingTooltip = String.Format(Properties.Resources.StartStopRecordingKeystrokes, recordingKeystrokes);
		}
		#endregion

		#region Overriding Methods
		protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
		{
			base.OnMouseLeftButtonDown(e);
			DragMove();
		}
		#endregion

		#region Methods

	    private void initRefreshUIEventManager()
		{
			refreshUIEventManager = new ConfigurableUIEventManagerClass("REFRESH_GADGET");
			refreshUIEventManager.RefreshUIHasArrived += refreshUIHasArrived;
			refreshUIEventManager.StartMonitoring();

			if (RTPMRecorderWrapper.IsRecording())
				updateUI();
		}            

	    private void updateUI()
		{
	        recording = !recording;

			refreshingUIBecauseOfExternalProcess = true;
			checkboxRecording.IsChecked = recording;

	        if (!recording)
                stopRecordingTimer();
            else
	            startRecordingTimer();

	        refreshingUIBecauseOfExternalProcess = false;
		}

        private void updateTimeElapsed()
        {
            TimeElapsedSeconds = timeRecordingHelper.GetStartedSeconds();
        }

	    private void startRecordingTimer()
	    {
            textBoxTimeElapsed.Visibility = Visibility.Visible;
            timeRecordingHelper.Start();
            recordingTimer.Start();            
	    }

	    private void stopRecordingTimer()
	    {
            recordingTimer.Stop();
	        updateTimeElapsed();
	    }
	    #endregion

		#region Event Handlers
		private void closeClick(object sender, System.Windows.RoutedEventArgs e)
		{
			Close();
		}

		private void startRecording(object sender, System.Windows.RoutedEventArgs e)
		{
			if (refreshingUIBecauseOfExternalProcess)
				return;

			RTPMRecorderWrapper.StartRecording();            
		}

		private void stopRecording(object sender, System.Windows.RoutedEventArgs e)
		{
			if (refreshingUIBecauseOfExternalProcess)
				return;
            
			RTPMRecorderWrapper.StopRecording();
            stopRecordingTimer();
		}

        private void recordingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Dispatcher.CheckAccess())
                updateTimeElapsed();
            else
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(updateTimeElapsed));
        }

		private void refreshUIHasArrived()
		{
			if (Dispatcher.CheckAccess())
				updateUI();
			else
				Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(updateUI));
		}
		#endregion
	}
}
