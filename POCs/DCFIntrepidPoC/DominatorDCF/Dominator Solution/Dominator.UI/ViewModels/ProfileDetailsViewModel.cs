using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dominator.Domain;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Enums;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Helpers;
using ICommand = System.Windows.Input.ICommand;

namespace Dominator.UI.ViewModels
{
    public class ProfileDetailsViewModel : ViewModelBase
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

        private bool isOverclockingEnabled;
        public bool IsOverclockingEnabled
        {
            get { return isOverclockingEnabled; }
            set { SetProperty(ref isOverclockingEnabled, value, "IsOverclockingEnabled"); }
        }

        private bool isCPUOverclockingEnabled;
        public bool IsCPUOverclockingEnabled
        {
            get { return isCPUOverclockingEnabled; }
            set { SetProperty(ref isCPUOverclockingEnabled, value, "IsCPUOverclockingEnabled"); }
        }

        private bool isMemoryOverclockingEnabled;
        public bool IsMemoryOverclockingEnabled
        {
            get { return isMemoryOverclockingEnabled; }
            set { SetProperty(ref isMemoryOverclockingEnabled, value, "IsMemoryOverclockingEnabled"); }
        }

        private bool isToggleOnOffVisible;
        public bool IsToggleOnOffVisible
        {
            get { return isToggleOnOffVisible; }
            set { SetProperty(ref isToggleOnOffVisible, value, "IsToggleOnOffVisible"); }
        }

        private bool isCPUToggleOnOffVisible;
        public bool IsCPUToggleOnOffVisible
        {
            get { return isCPUToggleOnOffVisible; }
            set { SetProperty(ref isCPUToggleOnOffVisible, value, "IsCPUToggleOnOffVisible"); }
        }

        private bool isMemoryToggleOnOffVisible;
        public bool IsMemoryToggleOnOffVisible
        {
            get { return isMemoryToggleOnOffVisible; }
            set { SetProperty(ref isMemoryToggleOnOffVisible, value, "IsMemoryToggleOnOffVisible"); }
        }

        private bool isXMPSupported;
        public bool IsXMPSupported
        {
            get { return isXMPSupported; }
            set { SetProperty(ref isXMPSupported, value, "IsXMPSupported"); }
        }

        private bool cpuOCRebootRequired;
        public bool CPUOCRebootRequired
        {
            get { return cpuOCRebootRequired; }
            set { SetProperty(ref cpuOCRebootRequired, value, "CPUOCRebootRequired"); }
        }

        private bool memoryOCRebootRequired;
        public bool MemoryOCRebootRequired
        {
            get { return memoryOCRebootRequired; }
            set { SetProperty(ref memoryOCRebootRequired, value, "MemoryOCRebootRequired"); }
        }

        private bool isMaximumFrequencyVisible;
        public bool IsMaximumFrequencyVisible
        {
            get { return isMaximumFrequencyVisible; }
            set { SetProperty(ref isMaximumFrequencyVisible, value, "IsMaximumFrequencyVisible"); }
        }

        private bool isVoltageModeVisible;
        public bool IsVoltageModeVisible
        {
            get { return isVoltageModeVisible; }
            set { SetProperty(ref isVoltageModeVisible, value, "IsVoltageModeVisible"); }
        }

        private CPUCategoryInfo cpuCategoryInfo;
        public CPUCategoryInfo CPUCategoryInfo
        {
            get { return cpuCategoryInfo; }
            set { SetProperty(ref cpuCategoryInfo, value, "CPUCategoryInfo"); }
        }

        private MemoryCategoryInfo memoryCategoryInfo;
        public MemoryCategoryInfo MemoryCategoryInfo
        {
            get { return memoryCategoryInfo; }
            set { SetProperty(ref memoryCategoryInfo, value, "MemoryCategoryInfo"); }
        }

        private bool isCelsiusSelected;
        public bool IsCelsiusSelected
        {
            get { return isCelsiusSelected; }
            set
            {
                SetProperty(ref isCelsiusSelected, value, "IsCelsiusSelected");
                Model.IsTempUnitCelsius = value;
            }
        }

        public ICommand ToggleCPUCategoryStatusCommand { get; set; }
        public ICommand ToggleMemoryCategoryStatusCommand { get; set; }

        private IMonitorManager monitorManager;
        private Timer timer;
        private ISetting[] settingToMonitor;
        private decimal fanMin;
        private decimal fanMax;       
        #endregion

        #region Constructor
        public ProfileDetailsViewModel()
        {
            initializeCommands();            
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            CPUCategoryInfo = new CPUCategoryInfo();
            CPUCategoryInfo.FanType = Model.GetFanType();
            if (Model.GetFanType() > 0)
            {
                fanMin = Model.GetFanRange().Min;
                fanMax = Model.GetFanRange().Max;
            }
            CPUOCRebootRequired = false;
            MemoryOCRebootRequired = false;
            MemoryCategoryInfo = new MemoryCategoryInfo();
            var xmpInfoData = SystemInfoRepository.Instance.XMPInfoData;
            if (xmpInfoData == null) return;
            IsXMPSupported = xmpInfoData.IsXMPSupported;
            MemoryCategoryInfo.XMPProfileID = Model.XMPProfileID;
        }

        public override void Load()
        {
            IsOverclockingEnabled = Model.IsOverclockingEnabled;
            IsCPUOverclockingEnabled = Model.IsCPUOCEnabled;
            IsMemoryOverclockingEnabled = Model.IsMemoryOCEnabled;
            IsCPUToggleOnOffVisible = IsOverclockingEnabled && !Model.IsPredefinedProfileSelected;
            IsMemoryToggleOnOffVisible = IsOverclockingEnabled && !Model.IsPredefinedProfileSelected && Model.IsXMPSelected;
            //IsToggleOnOffVisible = IsOverclockingEnabled && !Model.IsPredefinedProfileSelected; 
            IsMaximumFrequencyVisible = Model.IsMaximumFrequencyVisible();
            IsVoltageModeVisible = Model.IsVoltageModeVisible();
            startMonitor();
        }

        public override void Unload()
        {
            stopMonitor();
        }

        private void initializeCommands()
        {
            ToggleCPUCategoryStatusCommand = new RelayCommand<object>(executeToggleCPUCategoryStatus, canExecuteToggleCPUCategoryStatus);
            ToggleMemoryCategoryStatusCommand = new RelayCommand<object>(executeToggleMemoryCategoryStatus, canExecuteToggleMemoryCategoryStatus);
        }

        private bool canExecuteToggleCPUCategoryStatus(object obj)
        {
            return true;
        }

        private void executeToggleCPUCategoryStatus(object obj)
        {
            Model.IsCPUOCEnabled = (bool) obj;
            IsMaximumFrequencyVisible = Model.IsMaximumFrequencyVisible();
            IsVoltageModeVisible = Model.IsVoltageModeVisible();
            if (IsMaximumFrequencyVisible)
                CPUCategoryInfo.MaximumFrequency = Math.Round(Model.CPUFrequency, 1);
            if (IsVoltageModeVisible)
                CPUCategoryInfo.VoltageMode = Model.CPUVoltageMode;
        }

        private bool canExecuteToggleMemoryCategoryStatus(object obj)
        {  
            return true;

        }

        private void executeToggleMemoryCategoryStatus(object obj)
        {
            Model.IsMemoryOCEnabled = (bool)obj;
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
                SettingType.FanSpeed,
                SettingType.EffectiveVoltage,
                SettingType.EffectiveFrequency
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
            for(int i = 0; settingIDs != null && i < settingIDs.Length; ++i)
                settingIDs[i] = settingToMonitor[i].Id;
            var retValues = monitorManager.GetAllElementValues(settingIDs);
            for(int i = 0; settingIDs != null && retValues != null && i < retValues.Length; ++i)
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
                            CPUCategoryInfo.Temperature = Convert.ToInt32(temp);
                        else
                            CPUCategoryInfo.Temperature =Convert.ToInt32(Convert.ToDouble(temp) * 9.0 / 5.0 + 32.0);
                    }
                    else
                    if (st == SettingType.Utilization)
                        CPUCategoryInfo.Utilization = Convert.ToInt32(retValues[i]);
                    else if (st == SettingType.FanSpeed)
                    {
                        CPUCategoryInfo.FanType = Model.GetFanType();
                        CPUCategoryInfo.FanSpeed = 0;

                        if (Model.GetFanType() == 0) // pump
                            CPUCategoryInfo.FanSpeed = Convert.ToInt32(retValues[i]);
                        else if (Model.GetFanType() == 1) // fan
                        {
                            fanMin = Model.GetFanRange().Min;
                            fanMax = Model.GetFanRange().Max;
                            if (fanMax > fanMin)
                                CPUCategoryInfo.FanSpeed = (Convert.ToInt32(retValues[i]) - fanMin) / (fanMax - fanMin) * 100;
                        }
                    }                   
                    else
                    if (st == SettingType.EffectiveVoltage)
                        CPUCategoryInfo.Voltage = Math.Round(Convert.ToDecimal(retValues[i]), 3);
                    else
                    if (st == SettingType.EffectiveFrequency)
                        CPUCategoryInfo.Frequency = Math.Round(Convert.ToDecimal(retValues[i]), 1);
                }
                else
                if (hw == HWComponentType.Memory)
                {
                    if (st == SettingType.EffectiveFrequency)
                        MemoryCategoryInfo.Frequency = Math.Round(Convert.ToDecimal(retValues[i]), 1);
                    else
                    if (st == SettingType.EffectiveVoltage)
                        MemoryCategoryInfo.Voltage = Math.Round(Convert.ToDecimal(retValues[i]), 1);
                    else
                    if (st == SettingType.Utilization)
                        MemoryCategoryInfo.Utilization = Math.Round(Convert.ToDecimal(retValues[i]), 0);
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