using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using Microsoft.Win32;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class ApplicationButtonsPresenterClass : ApplicationButtonsPresenter
	{
		#region Properties
		private ApplicationButtonsView view;
		public ApplicationButtonsView View
		{
			get { return view; }
			set
			{
				if (view == value)
					return;

				view = value;
				initEventHandlers();
			}
		}

		public bool IsSnapshotSelected
		{
			get
			{
				return view != null && view.IsSnapshotSelected;
			}
			set
			{
				if (view == null)
					return;

				view.IsSnapshotSelected = value;
			}
		}

        public bool AreMultipleSnapshotsSelected
        {
            get
            {
                return view != null && view.AreMultipleSnapshotsSelected;
            }
            set
            {
                if (view == null)
                    return;

                view.AreMultipleSnapshotsSelected = value;
            }
        }

        public bool IsProcessDataSelected
        {
            get
            {
                return view != null && view.IsProcessDataSelected;
            }
            set
            {
                if (view == null)
                    return;

                view.IsProcessDataSelected = value;
            }
        }

        public bool AreMultipleProcessDataSelected
        {
            get
            {
                return view != null && view.AreMultipleProcessDataSelected;
            }
            set
            {
                if (view == null)
                    return;

                view.AreMultipleProcessDataSelected = value;
            }
        }

		private bool playing;
		private bool recording;

		private ConfigurableUIEventManager refreshUIEventManager;
		#endregion

		#region Constructor
		public ApplicationButtonsPresenterClass()
		{
			initRefreshUIEventManager();
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
	    public event Action CompareSelectedSnapshots;
		public event Action SaveAsCurrentSnapshot;
		public event Action RecordNewSnapshot;

		// Recording Performance Monitoring section
		public event Action<string> StartRecordingSnapshot;
		public event Action StopRecordingSnapshot;

		// Playing Snapshot section
		public event Action DeleteSnapshotBeingPlayed;
	    public event Action ViewProcessData;
		#endregion

		#region Methods
		private void onEventCall(Action eventToCall)
		{
			if (eventToCall != null)
				eventToCall();
		}

		private void initRefreshUIEventManager()
		{
			refreshUIEventManager = new ConfigurableUIEventManagerClass("REFRESH_PM_ABP");
			refreshUIEventManager.RefreshUIHasArrived += refreshUIHasArrived;
			refreshUIEventManager.StartMonitoring();
		}

		public void HideApplicationButtons()
		{
			view.HideApplicationButtons();
		}

		public void ShowGMProfilesButtons()
		{
			view.ShowGMProfilesButtons();
			playing = false;
		}

		public void ShowSnapshotListButtons()
		{
			view.ShowSnapshotListButtons();
			playing = false;
		}

		public void ShowSnapshotRecordingButtons()
		{
			recording = RTPMRecorderWrapper.IsRecording();
			View.SetRecordingVisualState(recording);
			view.ShowSnapshotRecordingButtons();
			playing = false;
		}

        public void ShowPerformanceSnapshotView(int recordingCount)
		{
			view.ShowPerformanceSnapshotView();
			playing = false;
			view.SetIsDeleteSnapshotEnabledStatus(recordingCount == 1);
			view.SetPlayingVisualState(playing);
            view.IsProcessDataSelected = false;
            view.AreMultipleProcessDataSelected = false;
            //view.SetIsPlaySnapshotEnabledStatus(true);
		}

        public void ShowPerformanceSnapshotViewForIncompatibleFile(int recordingCount)
		{
			view.ShowPerformanceSnapshotView();
			playing = false;
            view.SetIsDeleteSnapshotEnabledStatus(recordingCount == 1);
			view.SetPlayingVisualState(playing);
            view.IsProcessDataSelected = false;
            view.AreMultipleProcessDataSelected = false;
            //view.SetIsPlaySnapshotEnabledStatus(false);
		}

		private string getDefaultFileName()
		{
			var index = 0;
			var fileNameFormat = string.Format("{0}{1}.{2}", SnapshotsFolderProvider.SnapshotsFolder, PerformanceMonitoring.Properties.Resources.SnapshotDefaultFilename, SnapshotsFolderProvider.EXTENSION);
			var fileName = string.Format(fileNameFormat, (++index).ToString("D4"));
			bool ok = false;
			const int maximun = 9999;

			do
			{
				if (!File.Exists(fileName))
					ok = true;
				else
				{
					if (index <= maximun)
						fileName = string.Format(fileNameFormat, (++index).ToString("D4"));
					else
					{
						fileName = string.Format(fileNameFormat, Guid.NewGuid().ToString());
						ok = true;
					}

				}

			} while (!ok);

			return fileName;
		}
		#endregion

		#region Event Handlers
		private void initEventHandlers()
		{
			view.SaveCurrentGMProfile += saveCurrentGMProfile;
			view.SaveAsCurrentGMProfile += saveAsCurrentGMProfile;
			view.CancelCurrentGMProfile += cancelCurrentGMProfile;
			view.DeleteCurrentGMProfile += deleteCurrentGMProfile;

			view.DeleteCurrentSnapshot += deleteCurrentSnapshot;
			view.ViewCurrentSnapshot += viewCurrentSnapshot;
			view.SaveAsCurrentSnapshot += saveAsCurrentSnapshot;
			view.RecordCurrentSnapshot += recordNewSnapshot;

			view.StartStopRecordingSnapshot += startStopRecordingSnapshot;

			view.DeleteSnapshotBeingPlayed += deleteSnapshotBeingPlayed;
		    view.ViewProcessData += viewProcessData;
		}

		private void saveCurrentGMProfile()
		{
			onEventCall(SaveCurrentGMProfile);
		}

		private void saveAsCurrentGMProfile()
		{
			ViewFactory viewFactory = new ViewFactoryClass();
			var appWindow = viewFactory.NewView(ViewType.GameModeSaveAs) as Window;
			if (appWindow == null)
				return;

			var result = appWindow.ShowDialog();
			if (result == null || !result.Value)
				return;

			onEventCall(SaveAsCurrentGMProfile);
		}

		private void cancelCurrentGMProfile()
		{
			onEventCall(CancelCurrentGMProfile);
		}

		private void deleteCurrentGMProfile()
		{
			onEventCall(DeleteCurrentGMProfile);
		}

		private void deleteCurrentSnapshot()
		{
			onEventCall(DeleteCurrentSnapshot);
		}

		private void viewCurrentSnapshot()
		{
			onEventCall(ViewCurrentSnapshot);
		}      

		private void saveAsCurrentSnapshot()
		{
			onEventCall(SaveAsCurrentSnapshot);
		}

		private void recordNewSnapshot()
		{
			onEventCall(RecordNewSnapshot);
		}

		private void deleteSnapshotBeingPlayed()
		{
			onEventCall(DeleteSnapshotBeingPlayed);
		}

        private void viewProcessData()
        {
            onEventCall(ViewProcessData);
        }

		private void startStopRecordingSnapshot()
		{
			if (!recording)
			{
				if (!Directory.Exists(SnapshotsFolderProvider.SnapshotsFolder))
					try
					{
						Directory.CreateDirectory(SnapshotsFolderProvider.SnapshotsFolder);
					}
					catch (Exception) { }

				var saveDialog = new SaveFileDialog
				{
					DefaultExt = SnapshotsFolderProvider.EXTENSION,
					Filter = string.Format("{1} (.{0})|*.{0}", SnapshotsFolderProvider.EXTENSION, PerformanceMonitoring.Properties.Resources.PerformanceMonitoringSnapshotsText),
					InitialDirectory = SnapshotsFolderProvider.SnapshotsFolder,
					FileName = Path.GetFileName(getDefaultFileName())
				};

				bool ok = saveDialog.ShowDialog() == true;
				if (!ok)
					return;

				if (StartRecordingSnapshot != null)
					StartRecordingSnapshot(saveDialog.FileName);
			}
			else
				onEventCall(StopRecordingSnapshot);
		}

		private void refreshUIHasArrived()
		{
			var tmpView = View as UserControl;
			if (tmpView == null)
				return;

			if (tmpView.Dispatcher.CheckAccess())
				updateUI();
			else
				tmpView.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(updateUI));
		}

		private void updateUI()
		{
			recording = !recording;
			View.SetRecordingVisualState(recording);
		}
		#endregion
	}
}