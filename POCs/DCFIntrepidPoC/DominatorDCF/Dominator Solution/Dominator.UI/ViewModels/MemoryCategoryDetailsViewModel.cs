using System;
using System.Collections.Generic;
using System.Windows;
using Dominator.Domain;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.UI.Classes;
using Dominator.UI.Classes.Helpers;

namespace Dominator.UI.ViewModels
{
    public class MemoryCategoryDetailsViewModel : ViewModelBase
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

        private decimal currentFrequency;
        public decimal CurrentFrequency
        {
            get { return currentFrequency; }
            set { SetProperty(ref currentFrequency, value, "CurrentFrequency"); }
        }

        private decimal minimumFrequency;
        public decimal MinimumFrequency
        {
            get { return minimumFrequency; }
            set { SetProperty(ref minimumFrequency, value, "MinimumFrequency"); }
        }

        private decimal maximumFrequency;
        public decimal MaximumFrequency
        {
            get { return maximumFrequency; }
            set { SetProperty(ref maximumFrequency, value, "MaximumFrequency"); }
        }

        private IList<XMPData> xmpList;
        public IList<XMPData> XMPList
        {
            get { return xmpList; }
            set { SetProperty(ref xmpList, value, "XMPList"); }
        }

        private int xmpSelected;
        public int XMPSelected
        {
            get { return xmpSelected; }
            set
            {
                SetProperty(ref xmpSelected, value, "XMPSelected");
                IsManualModeEnabled = !IsXMPSupported || value != 0;
                executeXMPChanged(value);
            }
        }

        private IList<ThermalModeData> thermalModeList;
        public IList<ThermalModeData> ThermalModeList
        {
            get { return thermalModeList; }
            set { SetProperty(ref thermalModeList, value, "ThermalModeList"); }
        }

        private string thermalModeSelected;
        public string ThermalModeSelected
        {
            get { return thermalModeSelected; }
            set { SetProperty(ref thermalModeSelected, value, "ThermalModeSelected"); }
        }

        private bool isManualModeEnabled;
        public bool IsManualModeEnabled
        {
            get { return isManualModeEnabled; }
            set { SetProperty(ref isManualModeEnabled, value, "IsManualModeEnabled"); }
        }

        private bool isXMPSupported;
        public bool IsXMPSupported
        {
            get { return isXMPSupported; }
            set { SetProperty(ref isXMPSupported, value, "IsXMPSupported"); }
        }

        private Visibility isMemoryOCSupported;
        public Visibility IsMemoryOCSupported
        {
            get { return isMemoryOCSupported; }
            set { SetProperty(ref isMemoryOCSupported, value, "IsMemoryOCSupported"); }
        }

        public RelayCommand<object> FrequencyEditedCommand { get; set; }
        public RelayCommand<object> FrequencyChangedCommand { get; set; }
        public RelayCommand<IEquatableCommand> ViewActionCommand { get; set; }

        private ISystemInfo systemInfo;
        #endregion

        #region Constructor
        public MemoryCategoryDetailsViewModel()
        {
            initializeCommands();            
        }
        #endregion

        #region Methods
        public override void Initialize()
        {
            systemInfo = SystemInfoRepository.Instance;

            IsXMPSupported = systemInfo.XMPInfoData.IsXMPSupported;
           
            if (IsXMPSupported)
            {
                initializeXMPProfileList();
                XMPSelected = AdvancedOCModel.GetCurrentProfileXMPProfileID();
            }
            else
                IsManualModeEnabled = true;

            IsMemoryOCSupported = Visibility.Collapsed;
            //if (systemInfo.MemoryInfoData.IsMemoryOCSupported != null && !systemInfo.MemoryInfoData.IsMemoryOCSupported.Value)
            //{
            //    IsMemoryOCSupported = Visibility.Collapsed;
            //    return;
            //}

            AdvancedOCModel.IsXMPSelected = IsXMPSupported && XMPSelected > 0;

            var frequencySettings = AdvancedOCModel.GetMemoryFrequencySettings();
            MaximumFrequency = frequencySettings.Max;
            MinimumFrequency = frequencySettings.Min;
            CurrentFrequency = frequencySettings.CurrentValue;
            executeFrequencyChanged(CurrentFrequency);
            ViewActionCommand?.Execute(CommandFactory.NewAdvancedMEMOCCommand(xmpSelected));
        }

        public override void Load()
        {
        }

        public override void Unload()
        {           
        }

        private void initializeXMPProfileList()
        {
            var xmpList = new List<XMPData>();

            xmpList.Add(new XMPData { ProfileID = 0, ProfileName = string.Format(Properties.Resources.XMPFormat, Properties.Resources.Disabled) });
            for (var i=1; i <= systemInfo?.XMPInfoData.NumberOfXMPProfiles; i++)
                xmpList.Add(new XMPData { ProfileID = i + 1, ProfileName = string.Format(Properties.Resources.XMPFormat, i) });

            XMPList = xmpList;
            //XMPSelected = XMPList[0].ProfileID;
        }

        private void initializeThermalStateList()
        {
            var modeList = new List<ThermalModeData>();
            foreach (string mode in Enum.GetNames(typeof(ThermalModes)))
            {
                var name = Properties.Resources.ResourceManager.GetString($"Thermal{mode}");
                modeList.Add(new ThermalModeData { Mode = mode, Name = name});
            }

            ThermalModeList = modeList;
            //ThermalModeSelected = ThermalModeList[0].Mode;
        }

        private void initializeCommands()
        {
            FrequencyEditedCommand = new RelayCommand<object>(executeFrequencyEdited, canExecuteFrequencyEdited);
            FrequencyChangedCommand = new RelayCommand<object>(executeFrequencyChanged, canExecuteFrequencyChanged);
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
        }

        private bool canExecuteFrequencyChanged(object obj)
        {
            return true;
        }

        private void executeFrequencyChanged(object obj)
        {
            var frequency = Convert.ToDecimal(obj);
            AdvancedOCModel.SetMemoryFrequency(frequency);
        }

        private void executeXMPChanged(int profileID)
        {
            AdvancedOCModel.SetXMPValue(profileID);

            ViewActionCommand?.Execute(CommandFactory.NewAdvancedMEMOCCommand(profileID));
        }
        #endregion
    }
}