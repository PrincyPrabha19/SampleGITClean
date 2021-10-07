using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dominator.Domain.Classes.Helpers;
using Dominator.Domain.Classes.Profiles;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class AdvancedOCModel : IAdvancedOCModel
    {
        public ITuningManager TuningManager { get; set; }
        public IPlatformComponentManager PlatformComponentManager { get; set; }
        public IMonitorManager MonitorManager { get; set; }
        public ICPUDataConfigService CPUDataConfigService { private get; set; }
        public IProfileNameRepository ProfileNameRepository { get; set; }
        public IProfileManager ProfileManager { get; set; }
        public IOCStatusManager OCStatusManager { get; set; }
        public IValidationManager ValidationManager { get; set; }
        public bool IsNewProfile { get; set; }
        public List<ControlValue> CPUSettings { get; set; }
        public List<ControlValue> MemorySettings { get; set; }
        public bool IsXMPSelected { get; set; }

        public event Action<int, bool> ValidationSettingsProgressChanged;

        private bool xmpSupported => SystemInfoRepository.Instance.XMPInfoData.IsXMPSupported;
        private string presentProfileName { get; set; }
        public void Initialize()
        {
            CPUSettings = new List<ControlValue>();
            MemorySettings = new List<ControlValue>();

            //var profileInfo = ProfileManager.GetPredefinedProfileDataComponents(); 
            //var cpuDataComponent = profileInfo?.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            //List<ProfileSetting> settingsList =
            //    CPUDataConfigService.GetInitialSettingValues(Path.GetFileNameWithoutExtension(cpuDataComponent?.XtuPath));

            //updateInitialValues(settingsList);
        }

        public ConfiguredSettings SetCPUVoltage(decimal freqValue)
        {
            var freqSetting = getSetting(HWComponentType.CPU, SettingType.ActualFrequency);
            if (freqSetting == null) return null;
            var voltageValues = getConfiguredVoltageValues(freqValue);
            var voltageSetting = getSetting(HWComponentType.CPU, SettingType.ActualVoltage);
            if (voltageSetting == null) return null;
            return voltageValues;
        }

        public ConfiguredSettings SetVoltageOffset(decimal freqValue)
        {
            var voltageOffsetValues = getConfiguredVoltageOffsetValues(freqValue);
            var voltageOffsetSetting = getSetting(HWComponentType.CPU, SettingType.VoltageOffset);
            if (voltageOffsetSetting == null) return null;
            return voltageOffsetValues;
        }

        public decimal SetVoltageMode(decimal freqValue)
        {
            var voltageModeValue = getConfiguredVoltageModeValue(freqValue);
            var voltageModeSetting = getSetting(HWComponentType.CPU, SettingType.VoltageMode);
            if (voltageModeSetting == null) return 0;
            return voltageModeValue;
        }

        public bool SetMemoryFrequency(decimal memoryFreqValue)
        {
            var freqSetting = getSetting(HWComponentType.Memory, SettingType.ActualFrequency);
            var clockMultiplier = getSetting(HWComponentType.Memory, SettingType.BClockFrequency);
            if (freqSetting == null) return false;
            return addMultiplierControl(freqSetting.Id, clockMultiplier.Value, memoryFreqValue);
            //TODO: Add memory voltage section once its avaialble
        }

        public bool SetXMPValue(int profileID)
        {
            var xmpSetting = getSetting(HWComponentType.Memory, SettingType.XMP);
            var freqSetting = getSetting(HWComponentType.Memory, SettingType.ActualFrequency);
            if (profileID == 0) //remove the multiplier if defaul profile is chosen
                removeSetting(freqSetting.Id);
            if (xmpSetting == null) return false;
            return addControl(xmpSetting.Id, profileID);
        }

        public ValidationStatus ValidateSettings(bool isValidationRequired)
        {
            if (!SaveProfile() || !updateCurrentProfile(ProfileManager.CurrentProfile.Name)) return ValidationStatus.Invalidated;
            if (!isValidationRequired)
            {
                ProfileManager.ActiveProfile = ProfileManager.CurrentProfile;
                PlatformComponentManager?.RefreshSettings();
                updateActiveProfile(ProfileManager.CurrentProfile.Name);
                return ValidationStatus.Validated;
            }

            ProfileManager.CurrentProfile.DataHeader.Status = ValidationStatus.UnderValidation.ToString();
            SaveProfile();
            ValidationManager.ProgressChanged -= validationManager_ProgressChanged;
            ValidationManager.ProgressChanged += validationManager_ProgressChanged;

            var validationStatus = ValidationManager.StartValidation(ProfileManager.CurrentProfile);
            if (validationStatus == ValidationStatus.ValidationCancelled) return validationStatus;

            if (validationStatus == ValidationStatus.Validated)
            {
                ProfileManager.CurrentProfile.DataHeader.Status = validationStatus.ToString();
                SaveProfile();

                ProfileManager.ActiveProfile = ProfileManager.CurrentProfile;
                PlatformComponentManager?.RefreshSettings();
                updateActiveProfile(ProfileManager.CurrentProfile.Name);
            }
            return validationStatus;
        }

        private void validationManager_ProgressChanged(int percentage, bool showPercentage)
        {
            ValidationSettingsProgressChanged?.Invoke(percentage, showPercentage);
        }

        public int GetInitialMemoryProfileID()
        {
            return !IsNewProfile
                ? ProfileManager.CurrentProfile.GetXMPProfileID()
                : ProfileManager.GetPredefinedProfile().GetXMPProfileID();
        }

        public void CancelCurrrentProfile()
        {
            if (IsNewProfile)
                DeleteCurrentProfile();
            ResetValues();

            if (ValidationManager.IsValidationRunning)
                ValidationManager.StopValidation();
        }

        public List<IConfiguration> OverallOCSettings()
        {
            var overallOCSettings = new List<IConfiguration>();
            overallOCSettings.Add(new Configuration
            {
                LevelID = 1,
                RiskLevel = 1,
                Min = 0,
                Max = 100
            });
            overallOCSettings.Add(new Configuration
            {
                LevelID = 2,
                RiskLevel = 2,
                Min = 0,
                Max = 100
            });
            overallOCSettings.Add(new Configuration
            {
                LevelID = 3,
                RiskLevel = 3,
                Min = 0,
                Max = 100
            });
            return overallOCSettings;
        }

        public List<IConfiguration> FrequencySettings()
        {
            return CPUDataConfigService.FrequencySettings();
        }

        public List<IConfiguration> CoreVoltageSettings()
        {
            return CPUDataConfigService.CoreVoltageSettings();
        }

        public List<IConfiguration> VoltageOffsetSettings()
        {
            return CPUDataConfigService.VoltageOffsetSettings();
        }

        public int CoreVoltageDecimals()
        {
            return CPUDataConfigService.CoreVoltageDecimals();
        }

        public int VoltageOffsetDecimals()
        {
            return CPUDataConfigService.VoltageOffsetDecimals();
        }

        public SettingsInfo GetCPUFrequencySettings()
        {
            return CPUDataConfigService.GetFrequencySettings();
        }

        public SettingsInfo GetCPUVoltageSettings()
        {
            return CPUDataConfigService.GetCoreVoltageSettings();
        }

        public decimal GetDefaultVoltage(decimal frequency)
        {
            return CPUDataConfigService.GetDefaultVoltage(frequency);
        }

        public SettingsInfo GetCPUVoltageOffsetSettings()
        {
            return CPUDataConfigService.GetOffsetVoltageSettings();
        }

        public SettingsInfo GetCPUVoltageModeSettings()
        {
            return CPUDataConfigService.GetVoltageModeSettings();
        }

        public bool IsVoltageOffsetAlwaysOn()
        {
            return CPUDataConfigService.IsVoltageOffsetAlwaysOn();
        }

        public bool SaveProfile()
        {
            return saveProfile();
        }

        public void SetCurrentProfileValues()
        {
            if (CPUSettings.Count > 0 || MemorySettings.Count > 0)
                updateProfileValues(ProfileManager.CurrentProfile);
        }

        public void ResetValues()
        {
            CPUSettings.Clear();
            MemorySettings.Clear();
        }

        public string GetNewCustomProfileName(string profileNameFormat)
        {
            return ProfileNameRepository.GenerateNewCustomProfileName(profileNameFormat);
        }

        public string GetExistingProfileName()
        {
            return ProfileManager.CurrentProfile?.Name;
        }

        public decimal GetInitialFrequency()
        {
            if (IsNewProfile)
                return ProfileManager.GetPredefinedProfile().GetCPUFrequency();
            var freq = ProfileManager.CurrentProfile.GetCPUFrequency();
            if (freq == 0)
                return ProfileManager.GetPredefinedProfile().GetCPUFrequency();
            return freq;
            //return !IsNewProfile
            //    ? ProfileManager.CurrentProfile.GetCPUFrequency()
            //    : ProfileManager.GetPredefinedProfile().GetCPUFrequency();
        }

        public decimal GetOC2Frequency()
        {
            return ProfileManager.GetOC2Profile().GetCPUFrequency();
        }

        public decimal GetInitialVoltage()
        {
            return !IsNewProfile
                ? ProfileManager.CurrentProfile.GetCPUVoltage()
                : ProfileManager.GetPredefinedProfile().GetCPUVoltage();
        }

        public decimal GetInitialVoltageOffset()
        {
            return !IsNewProfile
                ? ProfileManager.CurrentProfile.GetCPUVoltageOffset()
                : ProfileManager.GetPredefinedProfile().GetCPUVoltageOffset();
        }

        public decimal GetInitialVoltageMode()
        {
            return !IsNewProfile
                ? ProfileManager.CurrentProfile.GetCPUVoltageMode()
                : ProfileManager.GetPredefinedProfile().GetCPUVoltageMode();
        }

        public bool DeleteCurrentProfile()
        {
            if (string.Compare(ProfileManager.CurrentProfile?.Name, ProfileManager.ActiveProfile?.Name, StringComparison.CurrentCultureIgnoreCase) == 0)
                ProfileManager.RestoreDefaultProfile();

            if (ProfileManager.DeleteCurrentProfile())
                return OCStatusManager?.SaveOCActiveProfile(ProfileManager.ActiveProfile?.Name) ?? false;

            return false;
        }

        public decimal GetCurrentProfileFrequency()
        {
            return ProfileManager.GetCurrentProfileFrequency();
        }

        public decimal GetCurrentProfileVoltage()
        {
            return ProfileManager.GetCurrentProfileVoltage();
        }

        public decimal GetCurrentProfileVoltageOffset()
        {
            return ProfileManager.GetCurrentProfileVoltageOffset();
        }

        public decimal GetCurrentProfileVoltageMode()
        {
            return ProfileManager.GetCurrentProfileVoltageMode();
        }

        public int GetCurrentProfileXMPProfileID()
        {
            return ProfileManager.GetCurrentProfileXMPProfileID();
        }

        public string GetCurrentProfileThermalMode()
        {
            return ProfileManager.GetCurrentProfileThermalMode();
        }

        public SettingsInfo GetMemoryFrequencySettings()
        {
            var setting = PlatformComponentManager?.GetSetting(HWComponentType.Memory, SettingType.ActualFrequency);
            var clockMultiplier =
                PlatformComponentManager?.GetSetting(HWComponentType.Memory, SettingType.BClockFrequency).Value;
            var bclk = PlatformComponentManager?.GetSetting(HWComponentType.CPU, SettingType.BClockFrequency).Value;
            if (setting?.AdvanceSetting == null) return null;
            return new SettingsInfo
            {
                Max = setting.AdvanceSetting.MaxValue * bclk.Value * clockMultiplier.Value / 1000,
                Min = setting.AdvanceSetting.MinValue * bclk.Value * clockMultiplier.Value / 1000,
                StepValue = setting.AdvanceSetting.NumOfDecimals * bclk.Value * clockMultiplier.Value / 1000,
                CurrentValue = setting.Value * bclk.Value * clockMultiplier.Value / 1000
            };
        }

        public decimal GetInitialMemoryMultiplier()
        {
            return !IsNewProfile
                ? ProfileManager.CurrentProfile.GetMemoryMultiplier()
                : ProfileManager.GetPredefinedProfile().GetMemoryMultiplier();
        }

        public void AddControl(HWComponentType hwComponentType, SettingType settingType, decimal value)
        {
            var idGenerator = new SettingIDGenerator();
            addControl(idGenerator.GetID(hwComponentType, settingType, 0), value);
            if (hwComponentType == HWComponentType.CPU && settingType == SettingType.ActualFrequency)
            {
                var power = CPUDataConfigService.GetPowerConfiguredValues(value);
                var iccMax = CPUDataConfigService.GetICCMaxConfiguredValues(value);
                var cacheICCMax = CPUDataConfigService.GetCacheICCMaxConfiguredValues(value);
                addControl(idGenerator.GetID(hwComponentType, SettingType.TurboBoostPowerMax, 0), power);
                addControl(idGenerator.GetID(hwComponentType, SettingType.TurboBoostShortPowerMax, 0), power);
                addControl(idGenerator.GetID(hwComponentType, SettingType.IccMaxCurrent, 0), iccMax);
                addControl(idGenerator.GetID(hwComponentType, SettingType.CacheIccMaxCurrent, 0), cacheICCMax);
            }
        }

        public void UpdateProfileName(string profileName)
        {
            updateProfileName(profileName);
        }

        private bool saveProfile()
        {
            if (ProfileManager.SaveProfile(ProfileManager.CurrentProfile))
                return true;
            return false;
        }

        private IProfile createNewProfile(string profileName)
        {
            var systemInfo = SystemInfoRepository.Instance;

            var dataHeader = new DataHeader();
            dataHeader.ProfileName = profileName;
            dataHeader.Model = systemInfo.PlatformInfoData.Platform;
            dataHeader.Status = ValidationStatus.UnderValidation.ToString();

            var cpuDataComponent = new CPUDataComponent();
            cpuDataComponent.IsOCEnabled = true;
            cpuDataComponent.BaseClock = getSetting(HWComponentType.CPU, SettingType.BClockFrequency).Value;
            cpuDataComponent.Brand = ProcessorHelper.GetProcessorCode(systemInfo.ProcessorBrand);

            cpuDataComponent.CoreDataList = new List<ICoreData>();
            for (uint i = 0; i < systemInfo.CPUInfoData.PhysicalCpuCores; i++)
            {
                cpuDataComponent.CoreDataList.Add(
                    new CoreData(cpuDataComponent.BaseClock, getSetting(SettingType.ActualFrequency, i).Value));
            }

            var xmpDataComponent = new MemoryDataComponent();
            xmpDataComponent.IsOCEnabled = true;
            xmpDataComponent.ClockMultiplier = getSetting(HWComponentType.Memory, SettingType.BClockFrequency).Value;

            var profile = ProfileManager.CreateNewProfile(profileName);
            profile.DataHeader = dataHeader;
            profile.DataComponents = new List<IDataComponent>();
            profile.DataComponents.Add(cpuDataComponent);
            profile.DataComponents.Add(xmpDataComponent);
            return profile;
        }

        private void updateProfileValues(IProfile profile)
        {
            var idGenerator = new SettingIDGenerator();
            var cpuDataComponent =
                profile.DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (cpuDataComponent != null && CPUSettings.Count != 0)
            {
                cpuDataComponent.Frequency = CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0))?.Value ?? 0;
                cpuDataComponent.Voltage = CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0))?.Value ?? 0;
                cpuDataComponent.VoltageOffset = CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0))?.Value ?? 0;
                cpuDataComponent.VoltageMode = Convert.ToInt32(CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0))?.Value ?? 0);
                cpuDataComponent.Power = Convert.ToDecimal(CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostPowerMax, 0))?.Value ?? 0);
                cpuDataComponent.ICCMax = Convert.ToDecimal(CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0))?.Value ?? 0);
                cpuDataComponent.CacheICCMax = Convert.ToDecimal(CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.CPU, SettingType.CacheIccMaxCurrent, 0))?.Value ?? 0);
            }

            var xmpDataComponent =
                profile.DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (xmpDataComponent != null)
            {
                xmpDataComponent.ProfileID = Convert.ToInt32(MemorySettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0))?.Value ?? 0);
                //xmpSupported ? Convert.ToInt32(CPUSettings.FirstOrDefault(setting => setting.Id == idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0))?.Value ?? 0) : 0;
                //var multiplier = getSettingValue(idGenerator.GetID(HWComponentType.Memory, SettingType.ActualFrequency, 0));
                //if (multiplier == 0)
                //    multiplier = GetInitialMemoryMultiplier();
                //xmpDataComponent.Multiplier = multiplier;
            }
        }

        private bool updateActiveProfile(string profileName)
        {
            return OCStatusManager?.SaveOCActiveProfile(profileName) ?? false;
        }

        private bool updateCurrentProfile(string profileName)
        {
            return OCStatusManager?.SaveOCCurrentProfile(profileName) ?? false;
        }
        private void updateProfileName(string profileName)
        {
            if (ProfileManager.CurrentProfile?.Name != profileName)
                profileName = ProfileNameRepository.CheckIfProfileExists(profileName);
            presentProfileName = profileName;

            if (ProfileManager.CurrentProfile != null && profileName != GetExistingProfileName())
            {
                var profilePath = ProfileNameRepository.UpdateProfileName(GetExistingProfileName(), profileName);
                ProfileManager.CurrentProfile.Name = profileName;
                ProfileManager.CurrentProfile.DataHeader.ProfileName = profileName;
                ProfileManager.CurrentProfile.DataHeader.Path = profilePath;
                ProfileManager.CurrentProfile.Path = profilePath;
                return;
            }

            var profile = createNewProfile(presentProfileName);
            ProfileManager.CurrentProfile = profile;
        }

        private void updateInitialValues(List<ProfileSetting> settingsList)
        {
            var settingIDGenerator = new SettingIDGenerator();
            var voltage =
                settingsList.FirstOrDefault(
                    s =>
                        s.Id ==
                        SettingsIDRepository.ControlIDs[
                            settingIDGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0)])?.Value ?? 0;
            var voltageOffset =
                settingsList.FirstOrDefault(
                    s =>
                        s.Id ==
                        SettingsIDRepository.ControlIDs[
                            settingIDGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0)])?.Value ?? 0;
            var voltageMode =
                settingsList.FirstOrDefault(
                    s =>
                        s.Id ==
                        SettingsIDRepository.ControlIDs[
                            settingIDGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0)])?.Value ?? 0;
            var multiplier =
                settingsList.FirstOrDefault(
                    s =>
                        s.Id ==
                        SettingsIDRepository.ControlIDs[
                            settingIDGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0)])?.Value ?? 0;
            if (multiplier == 0)
                multiplier =
                    settingsList.FirstOrDefault(
                        s =>
                            s.Id ==
                            SettingsIDRepository.ControlIDs[
                                settingIDGenerator.GetID(HWComponentType.Core, SettingType.ActualFrequency, 0)])?.Value ??
                    0;

            if (xmpSupported)
            {
                var memoryProfile =
                    settingsList.FirstOrDefault(
                        s =>
                            s.Id ==
                            SettingsIDRepository.ControlIDs[
                                settingIDGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0)])?
                        .Value ?? 0;

            }
        }

        private ConfiguredSettings getConfiguredVoltageValues(decimal freqValue)
        {
            return CPUDataConfigService.GetVoltageConfiguredValues(freqValue);
        }

        private ConfiguredSettings getConfiguredVoltageOffsetValues(decimal freqValue)
        {
            return CPUDataConfigService.GetVoltageOffsetConfiguredValues(freqValue);
        }

        private decimal getConfiguredVoltageModeValue(decimal freqValue)
        {
            return CPUDataConfigService.GetVoltageModeConfiguredValues(freqValue);
        }

        private bool addControl(uint controlID, decimal controlValue)
        {
            if (getHardwareType(controlID) == HWComponentType.CPU)
                return addCPUSettings(controlID, controlValue);
            return addMemorySettings(controlID, controlValue);
        }

        private bool addCPUSettings(uint controlID, decimal controlValue)
        {
            var control = CPUSettings.FirstOrDefault(ctrl => controlID == ctrl.Id);
            if (control == null)
            {
                CPUSettings.Add(new ControlValue
                {
                    Id = controlID,
                    Value = controlValue
                });
            }
            else
                control.Value = controlValue;
            return true;
        }

        private bool addMemorySettings(uint controlID, decimal controlValue)
        {
            var control = MemorySettings.FirstOrDefault(ctrl => controlID == ctrl.Id);
            if (control == null)
            {
                MemorySettings.Add(new ControlValue
                {
                    Id = controlID,
                    Value = controlValue
                });
            }
            else
                control.Value = controlValue;
            return true;
        }

        private decimal getSettingValue(uint controlID)
        {
            if (getHardwareType(controlID) == HWComponentType.CPU)
                return CPUSettings?.FirstOrDefault(setting => setting.Id == controlID).Value ?? 0;
            return MemorySettings?.FirstOrDefault(setting => setting.Id == controlID).Value ?? 0;
        }

        private void removeSetting(uint controlID)
        {
            if (getHardwareType(controlID) == HWComponentType.CPU)
                CPUSettings.Remove(CPUSettings?.FirstOrDefault(setting => setting.Id == controlID));
            MemorySettings.Remove(MemorySettings?.FirstOrDefault(setting => setting.Id == controlID));
        }

        private bool addMultiplierControl(uint controlID, decimal clockMultiplier, decimal controlValue)
        {
            var bclk = getSetting(HWComponentType.CPU, SettingType.BClockFrequency).Value;
            controlValue = Math.Round((controlValue * 1000) / (bclk * clockMultiplier));
            addMemorySettings(controlID, controlValue);
            return true;
        }

        private bool addMultiplierControl(uint controlID, decimal controlValue)
        {
            var bclk = getSetting(HWComponentType.CPU, SettingType.BClockFrequency).Value;
            controlValue = Math.Round(controlValue * 1000 / bclk);
            addCPUSettings(controlID, controlValue);
            return true;
        }

        private ISetting getSetting(HWComponentType hwComponentType, SettingType settingType)
        {
            return PlatformComponentManager?.GetSetting(hwComponentType, settingType);
        }

        private ISetting getSetting(SettingType settingType, uint coreIndex)
        {
            return PlatformComponentManager?.GetSetting(settingType, coreIndex);
        }

        private SettingType getSettingType(uint settingID)
        {
            return (SettingType)((settingID >> 16) & 255);
        }

        private HWComponentType getHardwareType(uint settingID)
        {
            return (HWComponentType)((settingID >> 24) & 255);
        }
    }
}
