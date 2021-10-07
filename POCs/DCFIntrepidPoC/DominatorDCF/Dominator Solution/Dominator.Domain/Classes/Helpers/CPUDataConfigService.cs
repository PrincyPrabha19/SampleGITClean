using System;
using System.Collections.Generic;
using System.Linq;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Classes.Models;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Helpers
{
    public class CPUDataConfigService : ICPUDataConfigService
    {
        public IXTUService XTUService { private get; set; }

        private IPlatformComponentManager platformComponentManager;
        private IDataConfigurationReader dataConfigurationReader;
        private List<IDataConfiguration> frequencyConfiguration;
        private List<IConfiguration> coreVoltageConfiguration;
        private List<IConfiguration> voltageOffsetConfiguration;

        public void Initialize()
        {
            platformComponentManager = PlatformComponentFactory.CreatePlatformComponentManager();
            dataConfigurationReader = DataConfigurationRepository.ConfigurationReader;
            frequencyConfiguration = dataConfigurationReader.FrequencyConfiguration;
            coreVoltageConfiguration = dataConfigurationReader.CoreVoltageConfiguration;
            voltageOffsetConfiguration = dataConfigurationReader.VoltageOffsetConfiguration;
        }

        public List<ProfileSetting> GetInitialSettingValues(string profileName)
        {
            List<ProfileSetting> settingsList;
            if (XTUService.LoadXTUProfileValues(profileName, out settingsList))
            {
                settingsList.AddRange(GetIntialMemorySettings());
                return settingsList;
            }
            return null;
        }

        public List<ProfileSetting> GetIntialMemorySettings()
        {
            var settingsList = new List<ProfileSetting>();
            settingsList.Add(new ProfileSetting
            {
                Id = (uint) XTUID.ExtremeMemoryProfile,
                Value = XTUService.GetDefaultValue((uint) XTUID.ExtremeMemoryProfile)
            });
            return settingsList;
        }

        public SettingsInfo GetFrequencySettings()
        {
            var numOfLevels = dataConfigurationReader.NumberOfFreqLevels;
            var minLevel = frequencyConfiguration.FirstOrDefault(level => level.LevelID == 1);
            var maxLevel = frequencyConfiguration.FirstOrDefault(level => level.LevelID == numOfLevels);
            if (minLevel == null || maxLevel == null) return null;
            return new SettingsInfo
            {
                Min = minLevel.MinFrequency,
                Max = maxLevel.MaxFrequency,
                NoOfDecimals = 1,
                SafeModeUpperLimit = Convert.ToDecimal(4.3),
                DangerModeLowerLimit = Convert.ToDecimal(4.8)
            };
        }

        public SettingsInfo GetCoreVoltageSettings()
        {
            var setting = platformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.ActualVoltage);
            if (setting?.AdvanceSetting == null) return null;
            
            return new SettingsInfo
            {
                Max = setting.AdvanceSetting.MaxValue,
                Min = setting.AdvanceSetting.MinValue,
                NoOfDecimals = setting.AdvanceSetting.NumOfDecimals,
                CurrentValue = setting.Value,
                SafeModeUpperLimit = Convert.ToDecimal(1.3),
                DangerModeLowerLimit = Convert.ToDecimal(1.8)
            };
        }

        public SettingsInfo GetOffsetVoltageSettings()
        {
            var setting = platformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.VoltageOffset);
            if (setting?.AdvanceSetting == null) return null;

            return new SettingsInfo
            {
                Max = setting.AdvanceSetting.MaxValue,
                Min = setting.AdvanceSetting.MinValue,
                NoOfDecimals = setting.AdvanceSetting.NumOfDecimals
            };
        }

        public SettingsInfo GetVoltageModeSettings()
        {
            var setting = platformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.VoltageMode);
            if (setting?.AdvanceSetting == null) return null;

            return new SettingsInfo
            {
                Max = setting.AdvanceSetting.MaxValue,
                Min = setting.AdvanceSetting.MinValue
            };
        }

        public ConfiguredSettings GetVoltageConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            if (configInfo == null) return null;
            return new ConfiguredSettings
            {
                Min = configInfo.MinVoltageOverride,
                Max = configInfo.MaxVoltageOverride,
                Default = configInfo.DefaultVoltageOverride
            };
        }

        public ConfiguredSettings GetVoltageOffsetConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            if (configInfo == null) return null;
            return new ConfiguredSettings
            {
                Min = configInfo.MinVoltageOffset,
                Max = configInfo.MaxVoltageOffset,
                Default = configInfo.DefaultVoltageOffset
            };
        }

        public decimal GetVoltageModeConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            if (configInfo == null) return 0;
            return configInfo.VoltageMode;
        }

        public decimal GetPowerConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.Power ?? 0;
        }

        public decimal GetICCMaxConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.ICCMax ?? 0;
        }

        public decimal GetDefaultVoltage(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.DefaultVoltageOverride ?? 0;
        }

        public decimal GetFanType()
        {
            return dataConfigurationReader.FanType;
        }

        public ConfiguredSettings GetFanRange()
        {
            return new ConfiguredSettings
            {
                Min = dataConfigurationReader.FanMinValue,
                Max = dataConfigurationReader.FanMaxValue
            };
        }

        public uint GetFreqRiskLevel(decimal currentFrequency)
        {
            var level = findFreqLevel(currentFrequency);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.RiskLevel ?? 1;
        }

        public uint GetCoreVoltageRiskLevel(decimal currentVoltage)
        {
            var level = findVoltageLevel(currentVoltage);
            var configInfo = coreVoltageConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.RiskLevel ?? 1;
        }

       

        public uint GetVoltageOffsetRiskLevel(decimal currentVoltageOffset)
        {
            var level = findVoltageOffsetLevel(currentVoltageOffset);
            var configInfo = voltageOffsetConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.RiskLevel ?? 1;
        }

        private uint findFreqLevel(decimal frequencyCurrentValue)
        {
            foreach (var level in frequencyConfiguration)
            {
                if (frequencyCurrentValue <= level.MaxFrequency)
                    return level.LevelID;
            }
            return (uint) frequencyConfiguration.Count;
        }

        private ulong findVoltageLevel(decimal currentVoltage)
        {
            foreach (var level in coreVoltageConfiguration)
            {
                if (currentVoltage <= level.Max)
                    return level.LevelID;
            }
            return (uint)coreVoltageConfiguration.Count;
        }

        private ulong findVoltageOffsetLevel(decimal currentVoltageOffset)
        {
            foreach (var level in voltageOffsetConfiguration)
            {
                if (currentVoltageOffset <= level.Max)
                    return level.LevelID;
            }
            return (uint)voltageOffsetConfiguration.Count;
        }

        public List<IConfiguration> FrequencySettings()
        {
            var freqConfig = new List<IConfiguration>();
            foreach (var level in frequencyConfiguration)
            {
                freqConfig.Add(new Configuration
                {
                    LevelID = level.LevelID,
                    RiskLevel = level.RiskLevel,
                    Max = level.MaxFrequency,
                    Min = level.MinFrequency
                });
            }
            return freqConfig;
        }

        public List<IConfiguration> CoreVoltageSettings()
        {
            return coreVoltageConfiguration;
        }

        public List<IConfiguration> VoltageOffsetSettings()
        {
            return voltageOffsetConfiguration;
        }

        public int CoreVoltageDecimals()
        {
            var setting = platformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.ActualVoltage);
            return setting?.AdvanceSetting.NumOfDecimals ?? 1;
        }

        public int VoltageOffsetDecimals()
        {
            var setting = platformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.VoltageOffset);
            return setting?.AdvanceSetting.NumOfDecimals ?? 0;
        }

        public decimal GetCacheICCMaxConfiguredValues(decimal frequencyCurrentValue)
        {
            var level = findFreqLevel(frequencyCurrentValue);
            var configInfo = frequencyConfiguration.FirstOrDefault(id => id.LevelID == level);
            return configInfo?.CacheICCMax ?? 0;
        }

        public bool IsCheckThermalThrottling()
        {
            return dataConfigurationReader?.ThermalThrottling ?? true;
        }

        public bool IsCheckPowerLimitThrottling()
        {
            return dataConfigurationReader?.PowerLimitThrottling ?? true;
        }

        public bool IsCheckCurrentLimitThrottling()
        {
            return dataConfigurationReader?.CurrentLimitThrottling ?? true;
        }

        public bool IsVoltageOffsetAlwaysOn()
        {
            return dataConfigurationReader?.VoltageOffsetAlwaysOn ?? false;
        }
    }
}