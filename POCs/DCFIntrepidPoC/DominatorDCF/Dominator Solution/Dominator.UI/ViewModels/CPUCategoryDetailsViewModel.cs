using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using Dominator.Domain;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Classes.Helpers;
using Dominator.UI.Controls;

namespace Dominator.UI.ViewModels
{
    public class CPUCategoryDetailsViewModel : ViewModelBase
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

        public IAdvancedOCModel AdvancedOCModel { get; set; }

        // Frequency Properties
        private decimal currentFrequency;
        public decimal CurrentFrequency
        {
            get { return currentFrequency; }
            set { SetProperty(ref currentFrequency, value, "CurrentFrequency"); }
        }

        private int decimalsInFrequency;
        public int DecimalsInFrequency
        {
            get { return decimalsInFrequency; }
            set { SetProperty(ref decimalsInFrequency, value, "DecimalsInFrequency"); }
        }

        private List<ColourSliderRange> frequencyRanges;
        public List<ColourSliderRange> FrequencyRanges
        {
            get { return frequencyRanges; }
            set { SetProperty(ref frequencyRanges, value, "frequencyRanges"); }
        }

        private decimal maximumFrequency;
        public decimal MaximumFrequency
        {
            get { return maximumFrequency; }
            set { SetProperty(ref maximumFrequency, value, "MaximumFrequency"); }
        }

        private decimal minimumFrequency;
        public decimal MinimumFrequency
        {
            get { return minimumFrequency; }
            set { SetProperty(ref minimumFrequency, value, "MinimumFrequency"); }
        }
        // Voltages Properties
        private decimal currentVoltage;
        public decimal CurrentVoltage
        {
            get { return currentVoltage; }
            set { SetProperty(ref currentVoltage, value, "CurrentVoltage"); }
        }

        private int decimalsInVoltage;
        public int DecimalsInVoltage
        {
            get { return decimalsInVoltage; }
            set { SetProperty(ref decimalsInVoltage, value, "DecimalsInVoltage"); }
        }

        private List<ColourSliderRange> voltageRanges;
        public List<ColourSliderRange> VoltageRanges
        {
            get { return voltageRanges; }
            set { SetProperty(ref voltageRanges, value, "VoltageRanges"); }
        }

        private decimal voltageUpperLimit;
        public decimal VoltageUpperLimit
        {
            get { return voltageUpperLimit; }
            set { SetProperty(ref voltageUpperLimit, value, "VoltageUpperLimit"); }
        }

        private decimal voltageLowerLimit;
        public decimal VoltageLowerLimit
        {
            get { return voltageLowerLimit; }
            set { SetProperty(ref voltageLowerLimit, value, "VoltageLowerLimit"); }
        }

        private decimal maximumVoltage;
        public decimal MaximumVoltage
        {
            get { return maximumVoltage; }
            set { SetProperty(ref maximumVoltage, value, "MaximumVoltage"); }
        }

        private decimal minimumVoltage;
        public decimal MinimumVoltage
        {
            get { return minimumVoltage; }
            set { SetProperty(ref minimumVoltage, value, "MinimumVoltage"); }
        }

        // VolgateOffset Properties
        private decimal currentVoltageOffset;
        public decimal CurrentVoltageOffset
        {
            get { return currentVoltageOffset; }
            set { SetProperty(ref currentVoltageOffset, value, "CurrentVoltageOffset"); }
        }

        private int decimalsInVoltageOffset;
        public int DecimalsInVoltageOffset
        {
            get { return decimalsInVoltageOffset; }
            set { SetProperty(ref decimalsInVoltageOffset, value, "DecimalsInVoltageOffset"); }
        }

        private List<ColourSliderRange> voltageOffsetRanges;
        public List<ColourSliderRange> VoltageOffsetRanges
        {
            get { return voltageOffsetRanges; }
            set { SetProperty(ref voltageOffsetRanges, value, "VoltageOffsetRanges"); }
        }

        private decimal voltageOffsetUpperLimit;
        public decimal VoltageOffsetUpperLimit
        {
            get { return voltageOffsetUpperLimit; }
            set { SetProperty(ref voltageOffsetUpperLimit, value, "VoltageOffsetUpperLimit"); }
        }

        private decimal voltageOffsetLowerLimit;
        public decimal VoltageOffsetLowerLimit
        {
            get { return voltageOffsetLowerLimit; }
            set { SetProperty(ref voltageOffsetLowerLimit, value, "VoltageOffsetLowerLimit"); }
        }

        private decimal maximumVoltageOffset;
        public decimal MaximumVoltageOffset
        {
            get { return maximumVoltageOffset; }
            set { SetProperty(ref maximumVoltageOffset, value, "MaximumVoltageOffset"); }
        }

        private decimal minimumVoltageOffset;
        public decimal MinimumVoltageOffset
        {
            get { return minimumVoltageOffset; }
            set { SetProperty(ref minimumVoltageOffset, value, "MinimumVoltageOffset"); }
        }

        private bool isVoltageEnabled;
        public bool IsVoltageEnabled
        {
            get { return isVoltageEnabled; }
            set { SetProperty(ref isVoltageEnabled, value, "IsVoltageEnabled"); }
        }

        private bool isVoltageOffsetEnabled;
        public bool IsVoltageOffsetEnabled
        {
            get { return isVoltageOffsetEnabled; }
            set { SetProperty(ref isVoltageOffsetEnabled, value, "IsVoltageOffsetEnabled"); }
        }

        private bool isVoltageModeEnabled;
        public bool IsVoltageModeEnabled
        {
            get { return isVoltageModeEnabled; }
            set { SetProperty(ref isVoltageModeEnabled, value, "IsVoltageModeEnabled"); }
        }

        private bool isVoltageModeStaticSelected;
        public bool IsVoltageModeStaticSelected
        {
            get { return isVoltageModeStaticSelected; }
            set
            {
                SetProperty(ref isVoltageModeStaticSelected, value, "IsVoltageModeStaticSelected");
                ViewActionCommand?.Execute(CommandFactory.NewAdvancedCPUOCCommand(CurrentFrequency, CurrentVoltage, CurrentVoltageOffset, Convert.ToDecimal(IsVoltageModeStaticSelected)));
            }
        }

        private decimal currentTemperature;
        public decimal CurrentTemperature
        {
            get { return currentTemperature; }
            set { SetProperty(ref currentTemperature, value, "CurrentTemperature"); }
        }

        private decimal currentUtilization;
        public decimal CurrentUtilization
        {
            get { return currentUtilization; }
            set { SetProperty(ref currentUtilization, value, "CurrentUtilization"); }
        }

        private decimal currentFanSpeed;
        public decimal CurrentFanSpeed
        {
            get { return currentFanSpeed; }
            set { SetProperty(ref currentFanSpeed, value, "CurrentFanSpeed"); }
        }

        private decimal fanType;
        public decimal FanType
        {
            get { return fanType; }
            set { SetProperty(ref fanType, value, "FanType"); }
        }

        private ValidationControlStatus cpuValidationStatus;
        public ValidationControlStatus CPUValidationStatus
        {
            get { return cpuValidationStatus; }
            set { SetProperty(ref cpuValidationStatus, value, "CPUValidationStatus"); }
        }

        private int cpuValidationPercentage;
        public int CPUValidationPercentage
        {
            get { return cpuValidationPercentage; }
            set { SetProperty(ref cpuValidationPercentage, value, "CPUValidationPercentage"); }
        }

        public RelayCommand<object> FrequencyEditedCommand { get; set; }
        public RelayCommand<object> FrequencyChangedCommand { get; set; }
        public RelayCommand<object> VoltageChangedCommand { get; set; }
        public RelayCommand<object> VoltageOffsetChangedCommand { get; set; }
        public RelayCommand<object> ModeChangedCommand { get; set; }
        public RelayCommand<IEquatableCommand> ViewActionCommand { get; set; }

        private IMonitorManager monitorManager;
        private Timer timer;
        private ISetting[] settingToMonitor;
        public override event Action RiskChanged;
        private decimal fanMin;
        private decimal fanMax;
        #endregion

        #region Constructor
        public CPUCategoryDetailsViewModel()
        {
            initializeCommands();
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            AdvancedOCModel.Initialize();

            FanType = Model.GetFanType();
            if (Model.GetFanType() > 0)
            {
                fanMin = Model.GetFanRange().Min;
                fanMax = Model.GetFanRange().Max;
            }

            var frequencySettings = AdvancedOCModel.FrequencySettings();
            DecimalsInFrequency = 1;
            FrequencyRanges = createRanges(frequencySettings);
            if (frequencySettings != null && frequencySettings.Count > 1)
            {
                MinimumFrequency = frequencySettings[0].Min;
                MaximumFrequency = frequencySettings[frequencySettings.Count-1].Max;
            }

            var voltageSettings = AdvancedOCModel.CoreVoltageSettings();
            DecimalsInVoltage = AdvancedOCModel.CoreVoltageDecimals();
            VoltageRanges = createRanges(voltageSettings);
            if (voltageSettings != null && voltageSettings.Count > 1)
            {
                MinimumVoltage = voltageSettings[0].Min;
                MaximumVoltage = voltageSettings[voltageSettings.Count - 1].Max;
            }

            var voltageOffsetSettings = AdvancedOCModel.VoltageOffsetSettings();
            DecimalsInVoltageOffset = AdvancedOCModel.VoltageOffsetDecimals();
            VoltageOffsetRanges = createRanges(voltageOffsetSettings);
            if (voltageOffsetSettings != null && voltageOffsetSettings.Count > 1)
            {
                MinimumVoltageOffset = voltageOffsetSettings[0].Min;
                MaximumVoltageOffset = voltageOffsetSettings[voltageOffsetSettings.Count - 1].Max;
            }

            CurrentFrequency = AdvancedOCModel.GetInitialFrequency();
            CurrentVoltage = AdvancedOCModel.GetInitialVoltage();
            CurrentVoltageOffset = AdvancedOCModel.GetInitialVoltageOffset();

            IsVoltageModeEnabled = true;
            IsVoltageModeStaticSelected = AdvancedOCModel.GetInitialVoltageMode() > 0;
            IsVoltageEnabled = IsVoltageModeStaticSelected;
            isVoltageOffsetEnabled = IsVoltageModeStaticSelected || AdvancedOCModel.IsVoltageOffsetAlwaysOn();

            if (IsVoltageEnabled)
            {
                var configuredSettings = AdvancedOCModel.SetCPUVoltage(CurrentFrequency);
                if (configuredSettings != null && configuredSettings.Min <= configuredSettings.Max)
                {
                    VoltageLowerLimit = configuredSettings.Min;
                    VoltageUpperLimit = configuredSettings.Max;
                    if (CurrentVoltage < VoltageLowerLimit)
                        CurrentVoltage = VoltageLowerLimit;
                }
            }

            if (IsVoltageOffsetEnabled)
            {
                var configuredSettings = AdvancedOCModel.SetVoltageOffset(CurrentFrequency);
                if (configuredSettings != null && configuredSettings.Min <= configuredSettings.Max)
                {
                    VoltageOffsetLowerLimit = configuredSettings.Min;
                    VoltageOffsetUpperLimit = configuredSettings.Max;
                    if (CurrentVoltageOffset < VoltageOffsetLowerLimit)
                        CurrentVoltageOffset = VoltageOffsetLowerLimit;
                }
            }

            ViewActionCommand?.Execute(CommandFactory.NewAdvancedCPUOCCommand(CurrentFrequency, CurrentVoltage, CurrentVoltageOffset, Convert.ToDecimal(IsVoltageModeStaticSelected)));
            computeRiskLevel();
        }

        private List<ColourSliderRange> createRanges(List<IConfiguration> settings)
        {
            var sliderRange = new List<ColourSliderRange>();
            foreach (var setting in settings)
            {
                sliderRange.Add(new ColourSliderRange
                {
                    Min = setting.Min,
                    Max = setting.Max,
                    Color = (setting.RiskLevel == 1) ? Color.FromRgb(0x66, 0x99, 0x00): 
                            (setting.RiskLevel == 2) ? Color.FromRgb(0xCC, 0x99, 0x00): 
                                                       Color.FromRgb(0xCC, 0x00, 0x00)
                });
            }
            return sliderRange;
        }

        private void computeRiskLevel()
        {
            uint[] riskLevels = new uint[3]; 
            riskLevels[0] = Model.GetFreqRiskLevel(CurrentFrequency);
            if (IsVoltageModeStaticSelected)
            {
                riskLevels[1] = Model.GetCoreVoltageRiskLevel(CurrentVoltage);
                riskLevels[2] = Model.GetVoltageOffsetRiskLevel(CurrentVoltageOffset);
            }
            RiskInfo = new RiskLevelCalculator().ComputeRiskLevel(riskLevels);
            RiskChanged?.Invoke();
        }

        public override void Load()
        {
            startMonitor();           
        }

        public override void Unload()
        {
            stopMonitor();
        }

        private void initializeCommands()
        {
            FrequencyEditedCommand = new RelayCommand<object>(executeFrequencyEdited, canExecuteFrequencyEdited);
            FrequencyChangedCommand = new RelayCommand<object>(executeFrequencyChanged, canExecuteFrequencyChanged);
            VoltageChangedCommand = new RelayCommand<object>(executeVoltageChanged, canExecuteVoltageChanged);
            VoltageOffsetChangedCommand = new RelayCommand<object>(executeVoltageOffsetChanged, canExecuteVoltageOffsetChanged);
            ModeChangedCommand = new RelayCommand<object>(executeVoltageModeChanged, canExecuteVoltageModeChanged);
        }

        private bool canExecuteFrequencyEdited(object obj)
        {
            return true;
        }

        private void executeFrequencyEdited(object obj)
        {
            decimal value;
            if (decimal.TryParse(obj?.ToString(), out value))
                CurrentFrequency = value;

            CPUValidationStatus = ValidationControlStatus.None;
        }

        private bool canExecuteFrequencyChanged(object obj)
        {
            return true;
        }

        private void executeFrequencyChanged(object obj)
        {            
            var frequency = Convert.ToDecimal(obj);
            updateVoltageModeByFrequency(frequency);

            CPUValidationStatus = ValidationControlStatus.None;
            ViewActionCommand?.Execute(CommandFactory.NewAdvancedCPUOCCommand(CurrentFrequency, CurrentVoltage, CurrentVoltageOffset, Convert.ToDecimal(IsVoltageModeStaticSelected)));
            computeRiskLevel();
        }

        private void voltageSettingsInAutoMode()
        {
            VoltageUpperLimit = -1;
            VoltageLowerLimit = -1;
            CurrentVoltage = 0;

            var configuredSettings = AdvancedOCModel.SetVoltageOffset(CurrentFrequency);
            CurrentVoltageOffset = configuredSettings.Default;
  
            if (!(IsVoltageOffsetEnabled = AdvancedOCModel.IsVoltageOffsetAlwaysOn()))
            {
                VoltageOffsetUpperLimit = -1;
                VoltageOffsetLowerLimit = -1;
            }
            IsVoltageEnabled = false;
        }

        private void updateVoltage(decimal frequency)
        {
            var configuredSettings = AdvancedOCModel.SetCPUVoltage(frequency);
            if (configuredSettings == null) return;

            if (configuredSettings.Min <= configuredSettings.Max)
            {
                VoltageUpperLimit = configuredSettings.Max;
                VoltageLowerLimit = configuredSettings.Min;
                if (configuredSettings.Default >= configuredSettings.Min && configuredSettings.Default <= configuredSettings.Max)
                    CurrentVoltage = configuredSettings.Default;
                else
                    CurrentVoltage = configuredSettings.Min;
                //IsVoltageEnabled = true;
            }
            else
            {
                VoltageUpperLimit = -1;
                VoltageLowerLimit = -1;
                //IsVoltageEnabled = true;
            }
        }

        private void updateVoltageOffset(decimal frequency)
        {
            var configuredSettings = AdvancedOCModel.SetVoltageOffset(frequency);
            if (configuredSettings == null) return;

            if (configuredSettings.Min <= configuredSettings.Max)
            {
                VoltageOffsetUpperLimit = configuredSettings.Max;
                VoltageOffsetLowerLimit = configuredSettings.Min;
                if (configuredSettings.Default >= configuredSettings.Min && configuredSettings.Default <= configuredSettings.Max)
                    CurrentVoltageOffset = configuredSettings.Default;
                else
                    CurrentVoltageOffset = configuredSettings.Min;
                //IsVoltageEnabled = true;
            }
            else
            {
                VoltageOffsetUpperLimit = -1;
                VoltageOffsetLowerLimit = -1;
                //IsVoltageEnabled = true;
            }
        }

        private void updateVoltageModeByFrequency(decimal frequency)
        {
            if (AdvancedOCModel.GetDefaultVoltage(frequency) == 0)
            {
                IsVoltageModeStaticSelected = false;
                IsVoltageModeEnabled = false;
            }
            else
            {
                IsVoltageModeEnabled= true;
                IsVoltageModeStaticSelected = (int)AdvancedOCModel.SetVoltageMode(frequency) > 0;
            }
            executeVoltageModeChanged(IsVoltageModeStaticSelected);
        }

        private bool canExecuteVoltageModeChanged(object obj)
        {
            return true;
        }

        private void executeVoltageModeChanged(object obj)
        {
            updateVoltageOffset(CurrentFrequency);
            if (IsVoltageModeStaticSelected)
            {
                IsVoltageEnabled = true;
                IsVoltageOffsetEnabled = true;
                updateVoltage(CurrentFrequency);
            }
            else
            {
                voltageSettingsInAutoMode();
                IsVoltageEnabled = false;
            }
        }

        private bool canExecuteVoltageChanged(object obj)
        {
            return true;
        }

        private void executeVoltageChanged(object obj)
        {
            //AdvancedOCModel.UpdateCPUVoltage(CurrentVoltage);
            CPUValidationStatus = ValidationControlStatus.None;
            ViewActionCommand?.Execute(CommandFactory.NewAdvancedCPUOCCommand(CurrentFrequency, CurrentVoltage, CurrentVoltageOffset, Convert.ToDecimal(IsVoltageModeStaticSelected)));
            computeRiskLevel();
        }

        private bool canExecuteVoltageOffsetChanged(object obj)
        {
            return true;
        }

        private void executeVoltageOffsetChanged(object obj)
        {
            //AdvancedOCModel.UpdateCPUVoltageOffset(CurrentVoltageOffset);
            CPUValidationStatus = ValidationControlStatus.None;
            ViewActionCommand?.Execute(CommandFactory.NewAdvancedCPUOCCommand(CurrentFrequency, CurrentVoltage, CurrentVoltageOffset, Convert.ToDecimal(IsVoltageModeStaticSelected)));
            computeRiskLevel();
        }
        
        private void startMonitor()
        {
            if (monitorManager == null)
                try
                {
                    monitorManager = Model?.MonitorManager;
                }
                catch { return; }

            if (monitorManager != null)
                new Thread(startMonitoringAsync).Start();
        }

        private void stopMonitor()
        {
            if (monitorManager == null)
                return;

            disposeTimer();
            monitorManager.Stop();
            monitorManager = null;
        }

        private void startMonitoringAsync()
        {
            createSettingListToMonitor();
            monitorManager.Start();
            monitorManager.AddElements(settingToMonitor.Select(x => x.Id).ToArray());
            createTimer(1000);
        }

        private void createSettingListToMonitor()
        {
            settingToMonitor = Model.GetSettingArray(new List<SettingType>
            {
                SettingType.Temperature,
                SettingType.Utilization,
                SettingType.FanSpeed
            });
        }

        private void createTimer(int period)
        {
            timer = new Timer(timerCallBack, null, 0, period);
        }

        private void disposeTimer()
        {
            if (timer == null) return;

            timer.Dispose();
            timer = null;
        }

        private void refreshValues()
        {
            if (settingToMonitor == null || settingToMonitor.Length < 0 || monitorManager == null)
                return;

            var settingIDs = new uint[settingToMonitor.Length];
            for (int i = 0; settingIDs != null && i < settingIDs.Length; ++i)
                settingIDs[i] = settingToMonitor[i].Id;
            var retValues = monitorManager.GetAllElementValues(settingIDs);
            for (int i = 0; settingIDs != null && retValues != null && i < retValues.Length; ++i)
            {
                var settingID = settingIDs[i];
                var hw = (HWComponentType)(settingID >> 24);
                var st = (SettingType)((settingID >> 16) & 0xFF);
                //var index = (settingID >> 8) & 0xFF;

                if (hw == HWComponentType.CPU)
                {
                    if (st == SettingType.Temperature)
                    {
                        var temp = retValues[i];
                        if (Model.IsTempUnitCelsius)
                            CurrentTemperature = Convert.ToInt32(temp);
                        else
                            CurrentTemperature = Convert.ToInt32(Convert.ToDouble(temp) * 9.0 / 5.0 + 32.0);
                    }
                    if (st == SettingType.Utilization)
                        CurrentUtilization = Convert.ToInt32(retValues[i]);
                    else if (st == SettingType.FanSpeed)
                    {
                        CurrentFanSpeed = 0;
                        if (Model.GetFanType() == 0)
                            CurrentFanSpeed = Convert.ToInt32(retValues[i]);
                        else if(fanMax > fanMin)
                            CurrentFanSpeed = (Convert.ToInt32(retValues[i]) - fanMin) / (fanMax - fanMin) * 100;
                    }
                }
            }
        }

        private void timerCallBack(object stateObject)
        {
            refreshValues();
        }
        #endregion
    }
}