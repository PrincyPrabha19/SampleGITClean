using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;
using Dominator.UI.Views;

namespace Dominator.UI.ViewModels
{
    public class MainOCViewModel : ViewModelBase
    {
        #region Properties
        private IViewWithDataContextAndVisibility view;

        public IViewWithDataContextAndVisibility View
        {
            get { return view; }
            set
            {
                if (view == value) return;

                view = value;
                if (!DesignerProperties.IsInDesignMode)
                    view.DataContext = this;
            }
        }
       
        private ProfileNavigationViewModel profileNavigationViewModel;
        public ProfileNavigationViewModel ProfileNavigationViewModel
        {
            get { return profileNavigationViewModel; }
            set { SetProperty(ref profileNavigationViewModel, value, "ProfileNavigationViewModel"); }
        }

        private ProfileDetailsViewModel profileDetailsViewModel;
        public ProfileDetailsViewModel ProfileDetailsViewModel
        {
            get { return profileDetailsViewModel; }
            set { SetProperty(ref profileDetailsViewModel, value, "ProfileDetailsViewModel"); }
        }
        #endregion

        #region Constructor
        public MainOCViewModel()
        {
            initializeCommands();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            profileNavigationViewModel.OCStatusChanged += ocStatusChanged;
            profileNavigationViewModel.RefreshViewRequested += refreshViewRequested;
            profileNavigationViewModel.RefreshRebootStatus += refreshRebootStatus;

            ProfileDetailsViewModel?.Initialize();
            ProfileNavigationViewModel?.Initialize();
            int biosControlStatusFlag;
            BIOSSupportRegistryHelper.ReadOCUIBIOSControlStatus(out biosControlStatusFlag);
        }

        private void refreshRebootStatus()
        {
            if (!profileNavigationViewModel.IsOverclockingEnabled)
            {
                ProfileDetailsViewModel.CPUOCRebootRequired = false;
                ProfileDetailsViewModel.MemoryOCRebootRequired = false;
                return;
            }
            ProfileDetailsViewModel.CPUOCRebootRequired = (Model.RebootRequired & RebootMask.CPUOCRebootRequired) == RebootMask.CPUOCRebootRequired;
            ProfileDetailsViewModel.MemoryOCRebootRequired = (Model.RebootRequired & RebootMask.MemoryOCRebootRequired) == RebootMask.MemoryOCRebootRequired;
        }

        public override void Load()
        {
            verifyMetadata();

            ProfileNavigationViewModel?.Load();
            ProfileDetailsViewModel?.Load();
        }

        public override void Unload()
        {
            ProfileNavigationViewModel?.Unload();
            ProfileDetailsViewModel?.Unload();
        }

        private void initializeCommands()
        {
        }

        private void ocStatusChanged(bool enabled)
        {
            profileDetailsViewModel.IsOverclockingEnabled = enabled;
            refreshViewRequested();
        }

        private void refreshViewRequested()
        {
            refreshToggleOnOffVisisbility();
            refreshToggleOnOffStatus();
            refreshMaximumFrequency();
            refreshVoltageMode();
            refreshXmpProfile();
        }

        private void refreshVoltageMode()
        {
            profileDetailsViewModel.IsVoltageModeVisible = Model.IsVoltageModeVisible();
            if (profileDetailsViewModel.IsVoltageModeVisible)
                profileDetailsViewModel.CPUCategoryInfo.VoltageMode = Model.CPUVoltageMode;
        }

        private void refreshMaximumFrequency()
        {
            profileDetailsViewModel.IsMaximumFrequencyVisible =  Model.IsMaximumFrequencyVisible();
            if (profileDetailsViewModel.IsMaximumFrequencyVisible)  
                profileDetailsViewModel.CPUCategoryInfo.MaximumFrequency = Math.Round(Model.CPUFrequency, 1);
        }

        private void refreshXmpProfile()
        {
            profileDetailsViewModel.MemoryCategoryInfo.XMPProfileID = Model.XMPProfileID;
        }

        private void refreshToggleOnOffStatus()
        {
            profileDetailsViewModel.IsCPUOverclockingEnabled = Model.IsOverclockingEnabled && Model.IsCPUOCEnabled; 
            profileDetailsViewModel.IsMemoryOverclockingEnabled = Model.IsOverclockingEnabled && Model.IsMemoryOCEnabled; 
        }

        private void refreshToggleOnOffVisisbility()
        {
            profileDetailsViewModel.IsCPUToggleOnOffVisible = Model.IsOverclockingEnabled && !Model.IsPredefinedProfileSelected;
            profileDetailsViewModel.IsMemoryToggleOnOffVisible = Model.IsOverclockingEnabled && !Model.IsPredefinedProfileSelected && Model.IsXMPSelected;
            //profileDetailsViewModel.IsToggleOnOffVisible = Model.IsOverclockingEnabled && !Model.IsPredefinedProfileSelected;
        }

        private void verifyMetadata()
        {
            if (!Model.AreAllPredefinedProfilesMissing() && !Model.IsAnyPredefinedProfileMissing() && Model.IsDataConfigAvailable()) return;

            var viewModel = new MetadataDownloadViewModel {
                View = new MetadataDownloadView(),
                HostViewModel = this
            };

            viewModel.Initialize();
            viewModel.Load();
            if (viewModel.ShowView())
            {                
                Model.RefreshMetadata();
                ProfileNavigationViewModel?.Initialize();
            }
        }
        #endregion
    }
}