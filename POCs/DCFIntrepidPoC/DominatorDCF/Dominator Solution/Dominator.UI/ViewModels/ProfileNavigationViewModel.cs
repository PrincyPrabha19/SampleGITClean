using System;
using System.Collections.Generic;
using System.Linq;
using Dominator.Domain;
using Dominator.Domain.Classes.Helpers;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.ViewModels
{
    public class ProfileNavigationViewModel : ViewModelBase
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

        private List<IProfileInfo> predefinedProfileList;
        public List<IProfileInfo> PredefinedProfileList
        {
            get { return predefinedProfileList; }
            set { SetProperty(ref predefinedProfileList, value, "PredefinedProfileList"); }
        }

        private List<IProfileInfo> customProfileList;
        public List<IProfileInfo> CustomProfileList
        {
            get { return customProfileList; }
            set { SetProperty(ref customProfileList, value, "CustomProfileList"); }
        }

        private bool isOverclockingEnabled;        
        public bool IsOverclockingEnabled
        {
            get { return isOverclockingEnabled; }
            set { SetProperty(ref isOverclockingEnabled, value, "IsOverclockingEnabled"); }
        }
        
        private bool isSystemOverclockable;
        public bool IsSystemOverclockable
        {
            get { return isSystemOverclockable; }
            set { SetProperty(ref isSystemOverclockable, value, "IsSystemOverclockable"); }
        }

        private bool areMaxProfilesCreated;
        public bool AreMaxProfilesCreated
        {
            get { return areMaxProfilesCreated; }
            set { SetProperty(ref areMaxProfilesCreated, value, "AreMaxProfilesCreated"); }
        }

        public IAdvancedOCModel AdvancedOCModel { get; set; }

        public RelayCommand<object> ToggleOCStatusCommand { get; set; }
        public RelayCommand<object> AddNewCustomProfileCommand { get; set; }
        public RelayCommand<object> ActivateProfileCommand { get; set; }
        public RelayCommand<object> EditCustomProfileCommand { get; set; }

        public event Action<bool> OCStatusChanged;
        public event Action RefreshViewRequested;
        public event Action RefreshRebootStatus;
        #endregion

        #region Constructor
        public ProfileNavigationViewModel()
        {
            initializeCommands();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            Model.InvalidateProfile -= invalidateProfile;
            Model.InvalidateProfile += invalidateProfile;
            Model.ReportRebootRequired -= reportRebootRequired;
            Model.ReportRebootRequired += reportRebootRequired;
            Model.LoadCustomProfilesFailed -= loadCustomProfilesFailed;
            Model.LoadCustomProfilesFailed += loadCustomProfilesFailed;
            Model.MemoryRebootRequired -= memoryRebootRequired;
            Model.MemoryRebootRequired += memoryRebootRequired;
            IsSystemOverclockable = Model.IsSystemOverclockable;
            IsOverclockingEnabled = Model.IsOverclockingEnabled;
        }

        private void memoryRebootRequired(RebootMask rebootRequired)
        {
            displayRebootRequiredMessage(rebootRequired);
            RefreshRebootStatus?.Invoke();
        }

        public override void Load()
        {            
            if (PredefinedProfileList == null)
                checkConfigurationFiles();

            PredefinedProfileList = Model?.GetPredefinedProfileList();
            CustomProfileList = Model?.GetCustomProfileList();
            Model?.CheckProfileValidation();

            OCStatusChanged?.Invoke(IsOverclockingEnabled);
            AreMaxProfilesCreated = CustomProfileList?.Count >= 5;

            updateSelectedProfile(Model?.ActiveProfileName);
            AddNewCustomProfileCommand.RaiseCanExecuteChanged();
            if (Model != null)
                Model.IsActiveProfileDeleted = false;
            RefreshViewRequested?.Invoke();
        }

        private void reportRebootRequired(RebootMask rebootRequired)
        {
            displayRebootRequiredMessage(rebootRequired);
        }

        private void displayRebootRequiredMessage(RebootMask rebootRequired)
        {
            if (rebootRequired == RebootMask.NoRebootRequired)
            {
                RefreshRebootStatus?.Invoke();
                return;
            }

            if ((rebootRequired & RebootMask.CPUOCRebootRequired) == RebootMask.CPUOCRebootRequired || 
                ((rebootRequired & RebootMask.MemoryOCRebootRequired) == RebootMask.MemoryOCRebootRequired && FastStartupRegistryHelper.IsFastStartupEnabled()))
            { 
                ThreadLauncher.Start(
                    () =>
                    {
                        var btnArr = new[] {Properties.Resources.Now.ToUpper(), Properties.Resources.Later.ToUpper()};
                        var result = new Notifier().Show(Properties.Resources.RebootRequiredError.ToUpper(),
                            Properties.Resources.RebootRequiredAtStart, NotifierIcon.Question, btnArr,
                            NotifierDefaultButton.FirstButton);
                        if (result == 1)
                        {
                            RefreshRebootStatus?.Invoke();
                            return;
                        }
                        RebootHelper.RestartSystem();
                    });
                return;
            }

            RefreshRebootStatus?.Invoke();
        }

        private void checkConfigurationFiles()
        {                        
            ThreadLauncher.Start(() =>
            {
                //if (Model.LoadingPredefinedProfileValuesFailed())
                //    new Notifier().Show(Properties.Resources.ApplicationName.ToUpper(),
                //        string.Format(Properties.Resources.LoadingPredefinedProfileValuesFailedMessage, Properties.Resources.ApplicationName), NotifierIcon.Error, NotifierButtons.Ok);

                if (Model.AreAllPredefinedProfilesMissing())
                    new Notifier().Show(Properties.Resources.AllPredefinedProfilesMissing.ToUpper(),
                        Properties.Resources.AllPredefinedProfilesMissingMessage, NotifierIcon.Error, NotifierButtons.Ok);
                else
                if (Model.IsAnyPredefinedProfileMissing())
                    new Notifier().Show(Properties.Resources.PredefinedProfileMissing.ToUpper(),
                        Properties.Resources.PredefinedProfileMissingMessage, NotifierIcon.Error, NotifierButtons.Ok);

                if (!Model?.IsDataConfigAvailable() ?? false)
                    new Notifier().Show(Properties.Resources.DataConfigError.ToUpper(), 
                        Properties.Resources.DataConfigMissingErrorMessage, NotifierIcon.Error, NotifierButtons.Ok);
            });
        }

        private void invalidateProfile(ProfileType type)
        {
            switch (type)
            {
                case ProfileType.ActiveProfileInvalid:
                {
                    invalidate(Model?.CurrentProfileName);
                    IsOverclockingEnabled = false;
                    OCStatusChanged?.Invoke(false);
                    ThreadLauncher.Start(() => { new Notifier().Show(Properties.Resources.InvalidProfileError.ToUpper(), String.Format(Properties.Resources.InvalidProfileErrorMessage, Properties.Resources.ApplicationName), NotifierIcon.Error, NotifierButtons.Ok); });
                    break;
                }
                case ProfileType.PredefinedProfileInvalid:
                {
                    isOverclockingEnabled = false;
                    OCStatusChanged?.Invoke(false);
                    if (Model?.IsActiveProfileDeleted ?? false)
                        break;
                    ThreadLauncher.Start(() => { new Notifier().Show(Properties.Resources.InvalidProfileError.ToUpper(), Properties.Resources.InvalidPredefinedProfileErrorMessage, NotifierIcon.Error, NotifierButtons.Ok); });
                    break;
                }
                case ProfileType.CurrentProfileUnderValidation:
                {
                    if (cpuocRebootRequired())
                        break;
                    AdvancedOCModel.IsNewProfile = false;
                    NavigationViewModel?.HostView(ViewTypes.Advanced);
                    break;
                }
            }
        }

        private void invalidate(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return;
            var profileInfo = customProfileList.FirstOrDefault(p => p.ProfileName == profileName);
            if (profileInfo != null)
            {
                profileInfo.IsValid = false;
            }
                
        }

        private void loadCustomProfilesFailed()
        {
            ThreadLauncher.Start(() => { new Notifier().Show(Properties.Resources.InvalidProfileError.ToUpper(), Properties.Resources.LoadingCustomProfilesFailedMessage, NotifierIcon.Error, NotifierButtons.Ok); });
        }

        private void initializeCommands()
        {
            ToggleOCStatusCommand = new RelayCommand<object>(executeToggleOCStatus, canExecuteToggleOCStatus);
            AddNewCustomProfileCommand = new RelayCommand<object>(executeAddNewCustomProfile, canExecuteAddNewCustomProfile);
            ActivateProfileCommand = new RelayCommand<object>(executeActivateProfile, canExecuteActivateProfile);
            EditCustomProfileCommand = new RelayCommand<object>(executeEditCustomProfile, canExecuteEditCustomProfile);
        }

        private bool canExecuteToggleOCStatus(object obj)
        {
            return true;
        }

        private void executeToggleOCStatus(object obj)
        {
            RebootMask rebootRequired;
            if (IsOverclockingEnabled)
            {
                Model.TurnOnOverclockingControl(out rebootRequired);
                displayRebootRequiredMessage(rebootRequired);
            }
            else
                Model.TurnOffOverclockingControl(out rebootRequired);
            
            OCStatusChanged?.Invoke(IsOverclockingEnabled);
            updateSelectedProfile(Model.ActiveProfileName);            
        }

        private bool canExecuteAddNewCustomProfile(object obj)
        {
            return Model.IsDataConfigAvailable() && CustomProfileList?.Count < 5;
        }

        private void executeAddNewCustomProfile(object obj)
        {
            if (cpuocRebootRequired())
                return;

            if (!DisclaimerHelper.IsDisclaimerAccepted())
                return;
           
            Model.ClearCurrentProfile();
            AdvancedOCModel.IsNewProfile = true;
            NavigationViewModel?.HostView(ViewTypes.Advanced);
        }

        private bool cpuocRebootRequired()
        {
            if ((Model.RebootRequired & RebootMask.CPUOCRebootRequired) == RebootMask.CPUOCRebootRequired)
            {
                ThreadLauncher.Start(
                    () =>
                    {
                        var btnArr = new[] { Properties.Resources.Now.ToUpper(), Properties.Resources.Cancel.ToUpper() };
                        var result = new Notifier().Show(Properties.Resources.RebootRequiredError.ToUpper(), Properties.Resources.RebootRequiredAtStart, NotifierIcon.Question, btnArr, NotifierDefaultButton.FirstButton);
                        if (result == 1)
                        {
                            RefreshRebootStatus?.Invoke();
                            return;
                        }
                        RebootHelper.RestartSystem();
                    });
                return true;
            }
            return false;
        }

        private bool canExecuteActivateProfile(object obj)
        {
            return true;
        }

        private void executeActivateProfile(object obj)
        {
            var prevProfileName = Model.ActiveProfileName;
            var profileName = obj.ToString();
            RebootMask isRebootRequired;
            if (Model.SetOCProfile(profileName, out isRebootRequired))
            {
                displayRebootRequiredMessage(isRebootRequired);
                RefreshViewRequested?.Invoke();
            }
            else
            {
                Model?.InvalidateSelectedProfile(profileName);
                updateSelectedProfile(prevProfileName);
                Model.SetOCProfile(prevProfileName, out isRebootRequired);
                ThreadLauncher.Start(() =>
                {
                    displayRebootRequiredMessage(isRebootRequired);
                    new Notifier().Show(Properties.Resources.InvalidProfileError.ToUpper(), Properties.Resources.ProfileSelectionError, NotifierIcon.Error, NotifierButtons.Ok);
                });

                RefreshViewRequested?.Invoke();
            }
        }

        private bool canExecuteEditCustomProfile(object obj)
        {            
            return Model.IsDataConfigAvailable();
        }

        private void executeEditCustomProfile(object obj)
        {   
            if (cpuocRebootRequired())
                return;

            var profileName = obj.ToString();
            if (Model.LoadCurrentProfile(profileName))
            {
                AdvancedOCModel.IsNewProfile = false;
                NavigationViewModel?.HostView(ViewTypes.Advanced);
            }
        }

        private void updateSelectedProfile(string profileName)
        {
            if (string.IsNullOrEmpty(profileName)) return;

            var profileInfo = PredefinedProfileList.FirstOrDefault(p => p.ProfileName == profileName);
            if (profileInfo != null && !profileInfo.IsSelected)
            {
                profileInfo.IsSelected = true;
                return;
            }

            profileInfo = CustomProfileList.FirstOrDefault(p => p.ProfileName == profileName);
            if (profileInfo != null && !profileInfo.IsSelected)
            {
                if (profileInfo.IsValid)
                    profileInfo.IsSelected = true;
                else
                {
                    Model.RestorePredefinedProfile();
                    updateSelectedProfile(Model.ActiveProfileName);
                }
            }
        }
        #endregion
    }
}