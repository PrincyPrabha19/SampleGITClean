using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.App.Views.Xaml;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;
using AlienLabs.Tools;
using AlienLabs.Tools.Classes;
using AlienLabs.Tools.Classes.Presenters;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class AlienAdrenalinePresenter
	{
		#region Properties
		private AlienAdrenalineView view;
		public AlienAdrenalineView View
		{
			get { return view; }
			set
			{
				if (view == value)
					return;

				view = value;
				initApplicationButtonsPresenter();
			}
		}

		public ViewRepository ViewRepository { get; set; }
		public ProfileRepository ProfileRepository { get; set; }
		public CommandContainer CommandContainer { get; set; }
		public EventTrigger EventTrigger { get; set; }

		private CategorySelectorPresenter categorySelectorPresenter;

		public virtual bool IsDirty
		{
			get { return !CommandContainer.IsEmpty; }
		}

		public bool IsDeleteEnabled
		{
			get { return ProfileRepository.CurrentProfile != null; }
		}

		private readonly BackgroundWorker backgroundWorkerLoadingGameProfiles = new BackgroundWorker();
		private readonly BackgroundWorker backgroundWorkerLoadingPerformanceMonitoring = new BackgroundWorker();

		private ProgressBarPresenter progressBarPresenter;
		private ApplicationButtonsPresenter appButtons;
		private PerformanceSnapshotListPresenter snapshotListPresenter;
		private SnapshotViewPresenter snapshotViewPresenter;
        private PerformanceMonitoringPresenter performanceMonitoringPresenter;
		//private string snapshotToReproduce = "";
        private List<string> snapshotToReproduceList = new List<string>();
		private bool canPlaySnapshot;
        private ExtendedPoint cpuExtendedPoint1, cpuExtendedPoint2;
		#endregion

		#region Constructors
		public AlienAdrenalinePresenter()
		{
			backgroundWorkerLoadingGameProfiles.DoWork += backgroundWorkerLoadingGameProfilesDoWork;
			backgroundWorkerLoadingGameProfiles.RunWorkerCompleted += backgroundWorkerLoadingGameProfilesCompleted;
			backgroundWorkerLoadingGameProfiles.WorkerReportsProgress = true;

			backgroundWorkerLoadingPerformanceMonitoring.DoWork += backgroundWorkerLoadingPerformanceMonitoringDoWork;
			backgroundWorkerLoadingPerformanceMonitoring.RunWorkerCompleted += backgroundWorkerLoadingPerformanceMonitoringCompleted;
			backgroundWorkerLoadingPerformanceMonitoring.WorkerReportsProgress = true;
		}
		#endregion

		#region Public Methods
		public void Load()
		{
			View.NavigationView = ViewRepository.GetByType(ViewType.Navigation) as NavigationView;
			View.CategorySelectorView = ViewRepository.GetByType(ViewType.CategorySelector) as CategorySelectorView;
			runeGameServiceProvider();
		}

		public void Activate(ViewType type)
		{
            if (View.ActiveView != null && View.ActiveView.Type == type)
                return;

			stopPerformanceMonitoringIfNeeded();
			hideCategorySelector();

			View.ActiveView = ViewRepository.GetByType(type) as ContentView;

			switch (type)
			{
				case ViewType.GameMode:
					appButtons.ShowGMProfilesButtons();
					break;
				case ViewType.GameModeProfileList:
					appButtons.HideApplicationButtons();
					break;
				case ViewType.RealTimePerformanceMonitoring:
					loadPerformanceMonitoringClasses();
					appButtons.ShowSnapshotRecordingButtons();
					break;
				case ViewType.PerformanceSnapshots:
					initSnapshotPresenterIfNeeded(View.ActiveView as PerformanceSnapshotsView);
					appButtons.ShowSnapshotListButtons();
					break;
				case ViewType.SnapshopView:
                    if (initSnapshotViewPresenterIfNeeded(View.ActiveView as PerformanceMonitoringView, snapshotToReproduceList))
                    {
                        if (canPlaySnapshot)
                            appButtons.ShowPerformanceSnapshotView(snapshotToReproduceList.Count);
                        else
                            appButtons.ShowPerformanceSnapshotViewForIncompatibleFile(snapshotToReproduceList.Count);
                    }
                    else
                        Activate(ViewType.PerformanceSnapshots); 

                    break;
			}
		}

        public void ActivatePlugin(UserControl plugin)
        {
            hideCategorySelector();
            View.ActivePluginView = plugin;
            appButtons.HideApplicationButtons();
            ProfileRepository.SetCurrentProfile(null);
        }

		public void Receive(EquatableCommand equatableCommand)
		{
			CommandContainer.Add(equatableCommand);
			View.IsDirty = IsDirty;
		}

		public void Save()
		{
			foreach (var command in CommandContainer.Commands)
				command.Execute();

			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeSaveProfile, DateTime.Now);
			ProfileRepository.SaveProfile();
			clearModelAndView();
		}

		public void SaveAs()
		{
			Save();
			RefreshAllContentViews();
		}

		public void Cancel()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeCancelChanges, DateTime.Now);
			clearModelAndView();
			RefreshAllContentViews();
		}

		public void Delete()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeDeleteProfile, DateTime.Now);
			ProfileRepository.DeleteCurrentProfile();
			ProfileRepository.SaveProfile();
			ProfileRepository.RefreshProfiles();

			clearModelAndView();
			Activate(ViewType.GameModeProfileList);
			RefreshAllContentViews();

			View.NavigationView.Refresh();
		}

		public void ShutdownServices()
		{
			if (ServiceFactory.IsThermalProfileServiceInitialized)
				ServiceFactory.ThermalProfileService.Dispose();
		}

		public void Close(bool activateGameModeProfileList = true)
		{
			if (IsDirty)
			{
				var result = MsgBox.Show(
					Properties.Resources.CloseGameModeProfileText,
						String.Format(Properties.Resources.WantToSaveGameModeChangesToText, GetCurrentProfileName()), MsgBoxIcon.Question, MsgBoxButtons.YesNo);
				if (result == MsgBoxResult.Yes)
					Save();
				else
					Cancel();
			}

			clearModelAndView();
			if (activateGameModeProfileList)
			{
				Activate(ViewType.GameModeProfileList);
				RefreshAllContentViews();
			}

			//AlienLabs.Tools.Classes.UsageTacking.LogModuleUsage(Properties.Resources.ProcessIdentifier, lastPlugin, lastPluginLaunchingTime, DateTime.Now);
			ProfileRepository.SetCurrentProfile(null);
			View.NavigationView.Refresh();
			View.IsDeleteEnabled = false;
		}

		public virtual void RefreshAllContentViews()
		{
			foreach (var contentView in ViewRepository.GetAllContentViews())
				contentView.Refresh();
		}

		public virtual void RefreshNavigation()
		{
			if (View.NavigationView != null)
				View.NavigationView.Refresh();
		}

		public string GetCurrentProfileName()
		{
			if (ProfileRepository != null &&
				ProfileRepository.CurrentProfile != null)
				return ProfileRepository.CurrentProfile.Name;
			return String.Empty;
		}

		public void ProfileViewAction(ProfileActionType action)
		{
			switch (action)
			{
				case ProfileActionType.RefreshAllView:
					RefreshAllContentViews();
					break;
				case ProfileActionType.RefreshNavigation:
					RefreshNavigation();
					break;
				case ProfileActionType.EnableDelete:
					View.IsDeleteEnabled = true;
					break;
				case ProfileActionType.EnableSave:
					View.IsDirty = true;
					break;
				case ProfileActionType.Save:
					Save();
					break;
				case ProfileActionType.Cancel:
					Cancel();
					break;
				case ProfileActionType.Close:
					Close();
					break;
				case ProfileActionType.CloseActivatePerformance:
					Close(false);
					break;
                case ProfileActionType.ActivateGameModeProfileListView:
			        if (View.ActiveView == null ||
			            (View.ActiveView.Type != ViewType.GameMode &&
			             View.ActiveView.Type != ViewType.GameModeProfileList))
			            Activate(ViewType.GameModeProfileList);
			        break;
			}
		}

		public void ShowHelp()
		{
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeHelp, DateTime.Now);
			HelpFile.Show();
		}
		#endregion

		#region Private Methods
		private void clearModelAndView()
		{
			CommandContainer.Clear();
			View.IsDirty = false;
		}

		private void initApplicationButtonsPresenter()
		{
			appButtons = Factories.ObjectFactory.NewApplicationButtonsPresenter(view.ApplicationButtonsView);

			// Game Mode Section
			appButtons.SaveCurrentGMProfile += appButtonsSaveCurrentGMProfile;
			appButtons.SaveAsCurrentGMProfile += appButtonsSaveAsCurrentGMProfile;
			appButtons.CancelCurrentGMProfile += appButtonsCancelCurrentGMProfile;
			appButtons.DeleteCurrentGMProfile += appButtonsDeleteCurrentGMProfile;

			// Snapshot List section
			appButtons.DeleteCurrentSnapshot += appButtonsDeleteCurrentSnapshot;
			appButtons.ViewCurrentSnapshot += appButtonsViewCurrentSnapshot;
			appButtons.SaveAsCurrentSnapshot += appButtonsSaveAsCurrentSnapshot;
			appButtons.RecordNewSnapshot += appButtonsRecordNewSnapshot;

			// Recording Performance Monitoring section
			appButtons.StartRecordingSnapshot += appButtonsStartRecordingSnapshot;
			appButtons.StopRecordingSnapshot += appButtonsStopRecordingSnapshot;

			// Playing Snapshot section
			appButtons.DeleteSnapshotBeingPlayed += appButtonsDeleteSnapshotBeingPlayed;
		    appButtons.ViewProcessData += appButtonViewProcessData;
		}

		private void runeGameServiceProvider()
		{
			showProgressBar();
			backgroundWorkerLoadingGameProfiles.RunWorkerAsync();
		}

		private void loadPerformanceMonitoringClasses()
		{
			showProgressBar();
			backgroundWorkerLoadingPerformanceMonitoring.RunWorkerAsync();
		}

		private void showProgressBar()
		{
			if (progressBarPresenter == null)
			{
				var progressBar = ProgressBarFinderClass.FindProgressBar(View as AlienAdrenalinePluginCtrl);
				progressBarPresenter = new ProgressBarPresenterClass(progressBar);
			}

			progressBarPresenter.ShowProgressBar(Properties.Resources.ProgressBarLoadingText, 0, 0);
			progressBarPresenter.SetIsIndeterminate(true);
		}

		private void initSnapshotPresenterIfNeeded(PerformanceSnapshotsView snapshotView)
		{
			if (snapshotView == null)
				return;

			if (snapshotListPresenter != null)
			{
				snapshotListPresenter.Initialize();
				snapshotListPresenter.Refresh();
			}
			else
			{
				snapshotListPresenter = snapshotView.Presenter;
				snapshotListPresenter.SelectionChanged -= snapshotListSelectionChanged;
				snapshotListPresenter.SelectionChanged += snapshotListSelectionChanged;
				snapshotListPresenter.ViewSelectionRequested -= snapshotListViewSelectionRequested;
				snapshotListPresenter.ViewSelectionRequested += snapshotListViewSelectionRequested;			    
			}
		}

        private bool initSnapshotViewPresenterIfNeeded(PerformanceMonitoringView performanceMonitoringView, List<string> fileNames)
        {
            canPlaySnapshot = true;
            if (performanceMonitoringView == null)
                return false;

            snapshotViewPresenter = (SnapshotViewPresenter)performanceMonitoringView.Presenter;
            if (snapshotViewPresenter == null)
                return false;

            snapshotViewPresenter.EOFReached -= eofReached;
            snapshotViewPresenter.EOFReached += eofReached;
            snapshotViewPresenter.IncompatibleFileDetected -= incompatibleFileDetected;
            snapshotViewPresenter.IncompatibleFileDetected += incompatibleFileDetected;
            snapshotViewPresenter.CPUChartProcessDataSelectionChanged += snapshotViewCPUChartProcessDataSelectionChanged;
            if (!snapshotViewPresenter.ReadSnapshot(fileNames))
                return false;

            showCategorySelector(snapshotViewPresenter.GetCategoriesToShow());
            return true;
        }

		private void stopPerformanceMonitoringIfNeeded()
		{
			if (View == null || View.ActiveView == null || View.ActiveView.Type != ViewType.RealTimePerformanceMonitoring)
				return;
			
			var performanceView = View.ActiveView as PerformanceMonitoringView;
			if (performanceView == null)
				return;

            if (performanceMonitoringPresenter == null)
                performanceMonitoringPresenter = (PerformanceMonitoringPresenter)performanceView.Presenter;
            if (performanceMonitoringPresenter == null)
                return;

            performanceMonitoringPresenter.StopMonitoring();
		}

		private void hideCategorySelector()
		{
			if (categorySelectorPresenter == null)
				return;

			categorySelectorPresenter.Hide();
		    View.NavigationView.ShowDynamicLightingControl();
		}

		private void showCategorySelector(Visibility[] categories)
		{
			if (categorySelectorPresenter == null)
			{
				categorySelectorPresenter = new CategorySelectorPresenter(view.CategorySelectorView);
				categorySelectorPresenter.SelectionChanged += categorySelectorPresenterSelectionChanged;

				if (performanceMonitoringPresenter != null)
					performanceMonitoringPresenter.DataWasUpdated += dataWasUpdated;
			}

            View.NavigationView.HideDynamicLightingControl();

			categorySelectorPresenter.UpdateInfo("", "", "", "");
			categorySelectorPresenter.Show(categories);
		}

		private Visibility[] getRealTimeCategoriesToShow()
		{
			var enumValues = Enum.GetValues(typeof (MonitoringCategories));
			var result = new Visibility[enumValues.Length];

			result[(int)MonitoringCategories.CPU] = Visibility.Visible;
			result[(int)MonitoringCategories.Memory] = Visibility.Visible;
            var supportsNetworkCard = PerfMonServiceFactory.Instance.NetworkInfoResults != null && PerfMonServiceFactory.Instance.NetworkInfoResults.Count > 0;
            result[(int)MonitoringCategories.Network] = supportsNetworkCard ? Visibility.Visible : Visibility.Collapsed;
			var supportsVideoCard = PerfMonServiceFactory.Instance.VideoInfoResults != null && PerfMonServiceFactory.Instance.VideoInfoResults.Count > 0;
			result[(int)MonitoringCategories.GPU] = supportsVideoCard ? Visibility.Visible : Visibility.Collapsed;

			return result;
		}

        private bool initPerformanceMonitoringPresenter(PerformanceMonitoringView performanceMonitoringView)
        {
            if (performanceMonitoringView == null)
                return false;
            
            if (performanceMonitoringPresenter == null)
                performanceMonitoringPresenter = (PerformanceMonitoringPresenter)performanceMonitoringView.Presenter;

            return performanceMonitoringPresenter != null;
	    }
	    #endregion

		#region Event Handlers
		private void backgroundWorkerLoadingGameProfilesCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			progressBarPresenter.CloseProgressBar();
			Activate(ViewType.GameModeProfileList);
		}

		private void backgroundWorkerLoadingGameProfilesDoWork(object sender, DoWorkEventArgs e)
		{
			GameObjectFactory.NewGameServiceProvider().Initialize();
		}

		private void backgroundWorkerLoadingPerformanceMonitoringCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			progressBarPresenter.CloseProgressBar();
			showCategorySelector(getRealTimeCategoriesToShow());
		}

		private void backgroundWorkerLoadingPerformanceMonitoringDoWork(object sender, DoWorkEventArgs e)
		{
			if (View.ActiveView.Type != ViewType.RealTimePerformanceMonitoring)
				return;

			var performanceView = View.ActiveView as PerformanceMonitoringView;
			if (performanceView == null)
				return;

            if (initPerformanceMonitoringPresenter(performanceView))
            {
                performanceMonitoringPresenter.Refresh();
                performanceMonitoringPresenter.StartMonitoring();
            }            
		}

		private void appButtonsSaveCurrentGMProfile()
		{
			Save();
		}

		private void appButtonsSaveAsCurrentGMProfile()
		{
			SaveAs();
			RefreshNavigation();
		}

		private void appButtonsCancelCurrentGMProfile()
		{
			Cancel();
		}

		private void appButtonsDeleteCurrentGMProfile()
		{
			var result = MsgBox.Show(Properties.Resources.DeleteGameModeTitleText,
				String.Format(Properties.Resources.WantToDeleteGameModeText, GetCurrentProfileName()), MsgBoxIcon.Question, MsgBoxButtons.YesNo);

			if (result != MsgBoxResult.Yes)
				return;

			Delete();
			View.IsDirty = false;
			View.IsDeleteEnabled = false;
		}

		private void appButtonsDeleteCurrentSnapshot()
		{
			snapshotListPresenter.DeleteCurrentSnapshot();
		}

		private void appButtonsViewCurrentSnapshot()
		{
			//snapshotToReproduce = snapshotListPresenter.GetSelectedSnapshotFile();
		    snapshotToReproduceList = snapshotListPresenter.GetSelectedSnapshotFiles();
			Activate(ViewType.SnapshopView);
			//snapshotToReproduce = "";
		}

		private void appButtonsSaveAsCurrentSnapshot()
		{
			snapshotListPresenter.SaveAsCurrentSnapshot();
		}

		private void appButtonsRecordNewSnapshot()
		{
			Activate(ViewType.RealTimePerformanceMonitoring);
		}

		private void appButtonsStartRecordingSnapshot(string recordingName)
		{
			if (View.ActiveView.Type != ViewType.RealTimePerformanceMonitoring)
				return;

			var performanceView = View.ActiveView as PerformanceMonitoringView;
			if (performanceView == null)
				return;

            if (initPerformanceMonitoringPresenter(performanceView))            
                performanceMonitoringPresenter.StartRecording(recordingName);
		}

		private void appButtonsStopRecordingSnapshot()
		{
			if (View.ActiveView.Type != ViewType.RealTimePerformanceMonitoring)
				return;

			var performanceView = View.ActiveView as PerformanceMonitoringView;
			if (performanceView == null)
				return;

            if (initPerformanceMonitoringPresenter(performanceView))      
                performanceMonitoringPresenter.StopRecording();
		}

		private void snapshotListSelectionChanged(int snapshotsSelected)
		{
            appButtons.IsSnapshotSelected = snapshotsSelected == 1 || snapshotsSelected == 2;
		    appButtons.AreMultipleSnapshotsSelected = snapshotsSelected == 2;
		}

		private void snapshotListViewSelectionRequested()
		{
			appButtonsViewCurrentSnapshot();
		}

        public void snapshotViewCPUChartProcessDataSelectionChanged(ExtendedPoint extendedPoint1, ExtendedPoint extendedPoint2)
        {
            cpuExtendedPoint1 = extendedPoint1;
            cpuExtendedPoint2 = extendedPoint2;
            appButtons.IsProcessDataSelected = extendedPoint1 != null || extendedPoint2 != null;
            appButtons.AreMultipleProcessDataSelected = extendedPoint1 != null && extendedPoint2 != null;
        }

		private void appButtonsDeleteSnapshotBeingPlayed()
		{
			if (snapshotListPresenter.DeleteCurrentSnapshot())
				Activate(ViewType.PerformanceSnapshots);
		}

	    private void appButtonViewProcessData()
	    {
            var processPresenter = new ProcessesPresenter();
	        if (cpuExtendedPoint1 != null && cpuExtendedPoint2 != null)
	        {
	            processPresenter.ShowData(cpuExtendedPoint1, cpuExtendedPoint2);
	            return;
	        }

	        if (cpuExtendedPoint1 != null)
	        {
                processPresenter.ShowData(cpuExtendedPoint1);
                return;
	        }

            processPresenter.ShowData(cpuExtendedPoint2);
	    }

	    private void eofReached()
		{
            appButtons.ShowPerformanceSnapshotView(snapshotToReproduceList.Count);
		}

		private void incompatibleFileDetected()
		{
			canPlaySnapshot = false;
            Activate(ViewType.PerformanceSnapshots);
            MsgBox.Show(Properties.Resources.ErrorText, Properties.Resources.InvalidFile);
		}

		private void categorySelectorPresenterSelectionChanged(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
		{
			if (View.ActiveView.Type != ViewType.RealTimePerformanceMonitoring && View.ActiveView.Type != ViewType.SnapshopView)
				return;
			
			var activeView = View.ActiveView as PerformanceMonitoringView;
			if (activeView != null)
				activeView.RefreshChartsVisibility(categoriesToShow, selectedCategory);
		}

		private void dataWasUpdated(string cpu, string memory, string network, string video)
		{
			categorySelectorPresenter.UpdateInfo(cpu, memory, network, video);
		}
		#endregion
	}
}
