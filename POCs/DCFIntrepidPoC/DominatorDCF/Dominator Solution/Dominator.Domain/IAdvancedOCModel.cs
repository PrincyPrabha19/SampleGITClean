using System;
using System.Collections.Generic;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain
{
    public interface IAdvancedOCModel
    {
        ITuningManager TuningManager { get; set; }
        IPlatformComponentManager PlatformComponentManager { get; set; }
        IMonitorManager MonitorManager { get; set; }
        ICPUDataConfigService CPUDataConfigService { set; }
        IProfileNameRepository ProfileNameRepository { get; set; }
        IProfileManager ProfileManager { get; set; }
        void Initialize();
        bool IsXMPSelected { get; set;}
        bool IsNewProfile { get; set; }
        List<ControlValue> CPUSettings { get; set; }
        List<ControlValue> MemorySettings { get; set; }
        ConfiguredSettings SetCPUVoltage(decimal freqValue);
        bool SetMemoryFrequency(decimal memoryValue);        
        SettingsInfo GetCPUFrequencySettings();
        SettingsInfo GetCPUVoltageSettings();
        decimal GetDefaultVoltage(decimal frequency);
        bool SaveProfile();
        void SetCurrentProfileValues();
        void ResetValues();
        string GetNewCustomProfileName(string profileNameFormat);
        string GetExistingProfileName();
        decimal GetInitialFrequency();
        decimal GetOC2Frequency();
        decimal GetInitialVoltage();
        decimal GetInitialVoltageOffset();
        decimal GetInitialVoltageMode();
        bool DeleteCurrentProfile();
        decimal GetCurrentProfileFrequency();
        decimal GetCurrentProfileVoltage();
        decimal GetCurrentProfileVoltageOffset();
        decimal GetCurrentProfileVoltageMode();
        int GetCurrentProfileXMPProfileID();
        string GetCurrentProfileThermalMode();
        SettingsInfo GetMemoryFrequencySettings();
        SettingsInfo GetCPUVoltageOffsetSettings();
        SettingsInfo GetCPUVoltageModeSettings();
        bool IsVoltageOffsetAlwaysOn();

        ConfiguredSettings SetVoltageOffset(decimal freqValue);
        decimal SetVoltageMode(decimal freqValue);
        bool SetXMPValue(int profileID);
        void AddControl(HWComponentType hwComponentType, SettingType settingType, decimal value);
        void UpdateProfileName(string profileName);
        ValidationStatus ValidateSettings(bool isValidationRequired);
        int GetInitialMemoryProfileID();
        void CancelCurrrentProfile();
        List<IConfiguration> OverallOCSettings();
        List<IConfiguration> FrequencySettings();
        List<IConfiguration> CoreVoltageSettings();
        List<IConfiguration> VoltageOffsetSettings();
        int CoreVoltageDecimals();
        int VoltageOffsetDecimals();

        event Action<int, bool> ValidationSettingsProgressChanged;
    }

}
