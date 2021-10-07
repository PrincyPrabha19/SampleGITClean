using System;
using System.Windows;
using System.Windows.Input;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class ApplicationButtonsControl : ApplicationButtonsView
	{
		#region Properties
		private static readonly DependencyProperty isSnapshotSelectedProperty = DependencyProperty.Register("IsSnapshotSelected", typeof(bool), typeof(ApplicationButtonsControl));
		public bool IsSnapshotSelected
		{
			get { return (bool)GetValue(isSnapshotSelectedProperty); }
			set
			{
			    SetValue(isSnapshotSelectedProperty, value);
			}
		}

        private static readonly DependencyProperty areMultipleSnapshotsSelectedProperty = DependencyProperty.Register("AreMultipleSnapshotsSelected", typeof(bool), typeof(ApplicationButtonsControl));
        public bool AreMultipleSnapshotsSelected
        {
            get { return (bool)GetValue(areMultipleSnapshotsSelectedProperty); }
            set 
            {
                SetValue(areMultipleSnapshotsSelectedProperty, value);
                btnViewSnapshot.Content = value ? Properties.Resources.CompareText.ToUpper() : Properties.Resources.ViewText.ToUpper();
            }
        }

        private static readonly DependencyProperty isOneSnapshotSelectedProperty = DependencyProperty.Register("IsOneSnapshotSelected", typeof(bool), typeof(ApplicationButtonsControl));
        public bool IsOneSnapshotSelected
        {
            get { return (bool)GetValue(isOneSnapshotSelectedProperty); }
            set { SetValue(isOneSnapshotSelectedProperty, value); }
        }

        private static readonly DependencyProperty isProcessDataSelectedProperty = DependencyProperty.Register("IsProcessDataSelected", typeof(bool), typeof(ApplicationButtonsControl));
        public bool IsProcessDataSelected
        {
            get { return (bool)GetValue(isProcessDataSelectedProperty); }
            set { SetValue(isProcessDataSelectedProperty, value); }
        }

        private static readonly DependencyProperty areMultipleProcessDataSelectedProperty = DependencyProperty.Register("AreMultipleProcessDataSelected", typeof(bool), typeof(ApplicationButtonsControl));
        public bool AreMultipleProcessDataSelected
        {
            get { return (bool)GetValue(areMultipleProcessDataSelectedProperty); }
            set
            {
                SetValue(areMultipleProcessDataSelectedProperty, value);
                btnViewProcessData.Content = value ? Properties.Resources.CompareText.ToUpper() : Properties.Resources.ViewText.ToUpper();
            }
        }

		private static readonly DependencyProperty isDirtyProperty = DependencyProperty.Register("IsDirty", typeof(bool), typeof(ApplicationButtonsControl));
		public bool IsDirty
		{
			get { return (bool)GetValue(isDirtyProperty); }
			set { SetValue(isDirtyProperty, value); }
		}

		private static readonly DependencyProperty isDeleteEnabledProperty = DependencyProperty.Register("IsDeleteEnabled", typeof(bool), typeof(ApplicationButtonsControl));
		public bool IsDeleteEnabled
		{
			get { return (bool)GetValue(isDeleteEnabledProperty); }
			set { SetValue(isDeleteEnabledProperty, value); }
		}

		private static readonly DependencyProperty isPlayEnabledProperty = DependencyProperty.Register("IsPlayEnabled", typeof(bool), typeof(ApplicationButtonsControl));
		public bool IsPlayEnabled
		{
			get { return (bool)GetValue(isPlayEnabledProperty); }
			set { SetValue(isPlayEnabledProperty, value); }
		}

		private static readonly DependencyProperty isDeleteSnapshotEnabledProperty = DependencyProperty.Register("IsDeleteSnapshotEnabled", typeof(bool), typeof(ApplicationButtonsControl));
		public bool IsDeleteSnapshotEnabled
		{
			get { return (bool)GetValue(isDeleteSnapshotEnabledProperty); }
			set { SetValue(isDeleteSnapshotEnabledProperty, value); }
		}

        private static readonly DependencyProperty startStopRecordingTooltipProperty = DependencyProperty.Register("StartStopRecordingTooltip", typeof(string), typeof(ApplicationButtonsControl));
        public string StartStopRecordingTooltip
        {
            get { return (string)GetValue(startStopRecordingTooltipProperty); }
            set { SetValue(startStopRecordingTooltipProperty, value); }
        }
		#endregion

		#region Events
		// Game Mode section
		public event Action SaveCurrentGMProfile;
		public event Action SaveAsCurrentGMProfile;
		public event Action CancelCurrentGMProfile;
		public event Action DeleteCurrentGMProfile;

		// Performance Monitoring List section
		public event Action DeleteCurrentSnapshot;
		public event Action ViewCurrentSnapshot;
		public event Action SaveAsCurrentSnapshot;
		public event Action RecordCurrentSnapshot;

		// Recording Performance Monitoring section
		public event Action StartStopRecordingSnapshot;

		// Playing Snapshot section
		public event Action DeleteSnapshotBeingPlayed;
        public event Action ViewProcessData;
		#endregion

		#region Constructor
		public ApplicationButtonsControl()
		{
			InitializeComponent();
		}
		#endregion

		#region Methods
		public void HideApplicationButtons()
		{
			LayoutRoot.Visibility = Visibility.Hidden;
		}

		public void ShowGMProfilesButtons()
		{
			LayoutRoot.Visibility = Visibility.Visible;
			gridGameModeProfiles.Visibility = Visibility.Visible;
			gridPerformanceSnapshotList.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotView.Visibility = Visibility.Hidden;
			gridRecordingPerformanceSnapshot.Visibility = Visibility.Hidden;
		}

		public void ShowSnapshotListButtons()
		{
			LayoutRoot.Visibility = Visibility.Visible;
			gridGameModeProfiles.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotList.Visibility = Visibility.Visible;
			gridPerformanceSnapshotView.Visibility = Visibility.Hidden;
			gridRecordingPerformanceSnapshot.Visibility = Visibility.Hidden;
		}

		public void ShowSnapshotRecordingButtons()
		{
			LayoutRoot.Visibility = Visibility.Visible;
			gridGameModeProfiles.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotList.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotView.Visibility = Visibility.Hidden;
			gridRecordingPerformanceSnapshot.Visibility = Visibility.Visible;

		    var recordingKeystrokes = Properties.Resources.CtrlShiftAltF10;
//            if (AlienLabs.Tools.SystemInfo.Platform.ToString().StartsWith("Mobile", StringComparison.InvariantCultureIgnoreCase))
//                recordingKeystrokes = Properties.Resources.FnF10;
		    StartStopRecordingTooltip = String.Format(Properties.Resources.StartStopRecordingKeystrokes, recordingKeystrokes);
		}

		public void ShowPerformanceSnapshotView()
		{
			LayoutRoot.Visibility = Visibility.Visible;
			gridGameModeProfiles.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotList.Visibility = Visibility.Hidden;
			gridPerformanceSnapshotView.Visibility = Visibility.Visible;
			gridRecordingPerformanceSnapshot.Visibility = Visibility.Hidden;

		}

		public void SetPlayingVisualState(bool playing)
		{
            //var state = (playing) ? "Playing" : "Paused";
            //VisualStateManager.GoToState(btnPlayPauseCurrentSnapshot, state, true);
		}

		public void SetRecordingVisualState(bool recording)
		{
			var state = (recording) ? "Recording" : "Stopped";
			VisualStateManager.GoToState(btnStartStopRecording, state, true);
		}

		public void SetIsDeleteSnapshotEnabledStatus(bool enabled)
		{
			IsDeleteSnapshotEnabled = enabled;
		}
        
		public void SetIsPlaySnapshotEnabledStatus(bool enabled)
		{
			IsPlayEnabled = enabled;
		}

		private void onEventCall(Action eventToCall)
		{
			if (eventToCall != null)
				eventToCall();
		}
		#endregion

		#region Event Handlers
		private void btnSaveGMProfileClick(object sender, RoutedEventArgs e)
		{
			var currentCursor = btnSaveGMProfile.Cursor;
			try
			{
				btnSaveGMProfile.Cursor = Cursors.Wait;
				onEventCall(SaveCurrentGMProfile);
			}
			finally
			{
				btnSaveGMProfile.Cursor = currentCursor;
			}
		}

		private void btnSaveAsGMProfileClick(object sender, RoutedEventArgs e)
		{
			onEventCall(SaveAsCurrentGMProfile);
		}

		private void btnCancelGMProfileClick(object sender, RoutedEventArgs e)
		{
			onEventCall(CancelCurrentGMProfile);
		}

		private void btnDeleteGMProfileClick(object sender, RoutedEventArgs e)
		{
			onEventCall(DeleteCurrentGMProfile);
		}

		private void btnDeleteSnapshotClick(object sender, RoutedEventArgs e)
		{
			onEventCall(DeleteCurrentSnapshot);
		}

		private void btnViewSnapshotClick(object sender, RoutedEventArgs e)
		{
			onEventCall(ViewCurrentSnapshot);
		}

		private void btnSaveAsSnapshotClick(object sender, RoutedEventArgs e)
		{
			onEventCall(SaveAsCurrentSnapshot);
		}

		private void btnRecordNewSnapshotClick(object sender, RoutedEventArgs e)
		{
			onEventCall(RecordCurrentSnapshot);
		}

		private void btnStartStopRecordingClick(object sender, RoutedEventArgs e)
		{
			onEventCall(StartStopRecordingSnapshot);
		}

		private void btnDeleteCurrentSnapshotClick(object sender, RoutedEventArgs e)
		{
			onEventCall(DeleteSnapshotBeingPlayed);
		}

        private void btnViewProcessDataClick(object sender, RoutedEventArgs e)
        {
            onEventCall(ViewProcessData);
        }
		#endregion
	}
}