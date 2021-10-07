using System.ComponentModel;
using System.Threading;
using System.Windows;
using Dominator.Domain;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;
using DesignerProperties = Dominator.UI.Classes.Helpers.DesignerProperties;

namespace Dominator.UI.ViewModels
{
    public class AdvancedOCSummaryViewModel : ViewModelBase
    {
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

        public IAdvancedOCModel AdvancedOCModel { get; set; }

        private string profileName;
        public string ProfileName
        {
            get { return profileName;}
            set { SetProperty(ref profileName, value, "ProfileName");}
        }

        private decimal currentFrequency;
        public decimal CurrentFrequency
        {
            get { return currentFrequency; }
            set { SetProperty(ref currentFrequency, value, "CurrentFrequency"); }
        }

        private decimal currentVoltage;
        public decimal CurrentVoltage
        {
            get { return currentVoltage; }
            set { SetProperty(ref currentVoltage, value, "CurrentVoltage"); }
        }

        private decimal currentVoltageOffset;
        public decimal CurrentVoltageOffset
        {
            get { return currentVoltageOffset; }
            set { SetProperty(ref currentVoltageOffset, value, "CurrentVoltageOffset"); }
        }

        private decimal currentVoltageMode;
        public decimal CurrentVoltageMode
        {
            get { return currentVoltageMode; }
            set { SetProperty(ref currentVoltageMode, value, "CurrentVoltageMode"); }
        }

        private bool isStaticMode;
        public bool IsStaticMode
        {
            get { return isStaticMode; }
            set { SetProperty(ref isStaticMode, value, "IsStaticMode"); }
        }

        private int xmpSelected;
        public int XMPSelected
        {
            get { return xmpSelected; }
            set
            {
                SetProperty(ref xmpSelected, value, "XMPSelected");
                //IsManualModeEnabled = !IsXMPSupported || value != 0;
                //executeXMPChanged(value);
            }
        }

        private bool isXMPSupported;
        public bool IsXMPSupported
        {
            get { return isXMPSupported; }
            set { SetProperty(ref isXMPSupported, value, "IsXMPSupported"); }
        }

        private bool isValidationReviewed;
        public bool IsValidationReviewed
        {
            get { return isValidationReviewed; }
            set { SetProperty(ref isValidationReviewed, value, "IsValidationReviewed"); }
        }

        private bool isValidationInProgress;
        public bool IsValidationInProgress
        {
            get { return isValidationInProgress; }
            set { SetProperty(ref isValidationInProgress, value, "IsValidationInProgress"); }
        }

        private bool isValidationPassed;
        public bool IsValidationPassed
        {
            get { return isValidationPassed; }
            set { SetProperty(ref isValidationPassed, value, "IsValidationPassed"); }
        }

        private bool isValidationFailed;
        public bool IsValidationFailed
        {
            get { return isValidationFailed; }
            set { SetProperty(ref isValidationFailed, value, "IsValidationFailed"); }
        }

        private ValidationControlStatus validationStatus;
        public ValidationControlStatus ValidationStatus
        {
            get { return validationStatus; }
            set { SetProperty(ref validationStatus, value, "ValidationStatus"); }
        }

        private int validationPercentage;
        public int ValidationPercentage
        {
            get { return validationPercentage; }
            set { SetProperty(ref validationPercentage, value, "ValidationPercentage"); }
        }

        private bool showPercentageValue;
        public bool ShowPercentageValue
        {
            get { return showPercentageValue; }
            set { SetProperty(ref showPercentageValue, value, "ShowPercentageValue"); }
        }

        public SummaryResult SummaryResult { get; private set; }

        public RelayCommand<object> SaveCommand { get; set; }
        public RelayCommand<object> CancelCommand { get; set; }
        public RelayCommand<object> CloseCommand { get; set; }

        private bool isValidationRunning;
        private bool isValidationRequired;

        public AdvancedOCSummaryViewModel()
        {
            initializeCommands();            
        }

        public override void Initialize()
        {
            IsValidationReviewed = true;

            ProfileName = AdvancedOCModel.GetExistingProfileName();
            CurrentFrequency = AdvancedOCModel.GetCurrentProfileFrequency();
            CurrentVoltage = AdvancedOCModel.GetCurrentProfileVoltage();
            CurrentVoltageOffset = AdvancedOCModel.GetCurrentProfileVoltageOffset();
            CurrentVoltageMode = AdvancedOCModel.GetCurrentProfileVoltageMode();
            IsStaticMode = CurrentVoltageMode != 0;

            var systemInfo = SystemInfoRepository.Instance;
            IsXMPSupported = systemInfo?.XMPInfoData.IsXMPSupported ?? false;
            if (IsXMPSupported)
                XMPSelected = AdvancedOCModel.GetCurrentProfileXMPProfileID();
        }

        public bool ShowView(bool validate)
        {
            isValidationRequired = validate;
            var result = (View as Window)?.ShowDialog();
            return result.HasValue && result.Value;
        }

        private void initializeCommands()
        {
            SaveCommand = new RelayCommand<object>(executeSave, canExecuteSave);
            CancelCommand = new RelayCommand<object>(executeCancel, canExecuteCancel);
            CloseCommand = new RelayCommand<object>(executeClose, canExecuteClose);
        }

        private bool canExecuteSave(object obj)
        {
            return IsValidationReviewed;
        }

        private void executeSave(object obj)
        {
            isValidationRunning = true;

            BackgroundWorker validationWorker = new BackgroundWorker();
            validationWorker.DoWork += validationWorker_DoWork;
            validationWorker.RunWorkerCompleted += validationWorker_RunWorkerCompleted;
            validationWorker.RunWorkerAsync();            
        }

        private bool canExecuteCancel(object obj)
        {
            return true;
        }

        private void executeCancel(object obj)
        {
            SummaryResult = SummaryResult.ValidationCancel;
            ((Window)View).DialogResult = true;
        }

        private bool canExecuteClose(object obj)
        {
            return true;
        }

        private void executeClose(object obj)
        {
            ((Window)View).DialogResult = true;
        }

        private void validationWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            IsValidationReviewed = false;
            IsValidationInProgress = true;
            ValidationStatus = ValidationControlStatus.InProgress;

            AdvancedOCModel.ValidationSettingsProgressChanged -= advancedOCModel_ValidationSettingsProgressChanged;
            AdvancedOCModel.ValidationSettingsProgressChanged += advancedOCModel_ValidationSettingsProgressChanged;
            
            e.Result = AdvancedOCModel.ValidateSettings(isValidationRequired);
        }

        private void validationWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //isValidationRunning = false;

            if (!isValidationRequired)
            {
                SummaryResult = SummaryResult.ValidationPassed;
                IsValidationReviewed = false;
                IsValidationInProgress = false;
                ((Window)View).DialogResult = true; 
                return;
            }
            var validationResult = (ValidationStatus)e.Result;
            switch (validationResult)
            {
                case Domain.Enums.ValidationStatus.Validated:
                    SummaryResult = SummaryResult.ValidationPassed;
                    IsValidationReviewed = false;
                    IsValidationInProgress = false;
                    IsValidationPassed = true;

                    var waitCloseWorker_RunWorkerCompleted = new BackgroundWorker();
                    waitCloseWorker_RunWorkerCompleted.DoWork += delegate { Thread.Sleep(3000); };
                    waitCloseWorker_RunWorkerCompleted.RunWorkerCompleted += delegate { ((Window)View).DialogResult = true; };
                    waitCloseWorker_RunWorkerCompleted.RunWorkerAsync();
                    break;
                case Domain.Enums.ValidationStatus.Invalidated:
                    SummaryResult = SummaryResult.ValidationFailed;
                    IsValidationReviewed = false;
                    IsValidationInProgress = false;
                    IsValidationFailed = true;
                    break;
            }
        }

        private void advancedOCModel_ValidationSettingsProgressChanged(int percentage, bool showPercentage)
        {
            ValidationPercentage = percentage;
            ShowPercentageValue = showPercentage;
        }
    }
}
