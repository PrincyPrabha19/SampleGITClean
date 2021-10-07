using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;

namespace Dominator.Domain
{
    public interface ICPUDataConfigService
    {
        IXTUService XTUService { set; }
        void Initialize();
        List<ProfileSetting> GetInitialSettingValues(string profileName);
        List<ProfileSetting> GetIntialMemorySettings();
        SettingsInfo GetFrequencySettings();
        SettingsInfo GetCoreVoltageSettings();
        ConfiguredSettings GetVoltageConfiguredValues(decimal frequencyCurrentValue);
        SettingsInfo GetOffsetVoltageSettings();
        SettingsInfo GetVoltageModeSettings();
        ConfiguredSettings GetVoltageOffsetConfiguredValues(decimal frequencyCurrentValue);
        decimal GetVoltageModeConfiguredValues(decimal frequencyCurrentValue);
        decimal GetPowerConfiguredValues(decimal frequencyCurrentValue);
        decimal GetICCMaxConfiguredValues(decimal frequencyCurrentValue);
        decimal GetFanType();
        decimal GetDefaultVoltage(decimal frequencyCurrentValue);
        ConfiguredSettings GetFanRange();
        uint GetFreqRiskLevel(decimal currentFrequency);
        uint GetCoreVoltageRiskLevel(decimal currentVoltage);
        uint GetVoltageOffsetRiskLevel(decimal currentVoltageOffset);
        List<IConfiguration> FrequencySettings();
        List<IConfiguration> CoreVoltageSettings();
        List<IConfiguration> VoltageOffsetSettings();
        int CoreVoltageDecimals();
        int VoltageOffsetDecimals();
        decimal GetCacheICCMaxConfiguredValues(decimal value);
        bool IsCheckThermalThrottling();
        bool IsCheckPowerLimitThrottling();
        bool IsCheckCurrentLimitThrottling();
        bool IsVoltageOffsetAlwaysOn();
    }

}
