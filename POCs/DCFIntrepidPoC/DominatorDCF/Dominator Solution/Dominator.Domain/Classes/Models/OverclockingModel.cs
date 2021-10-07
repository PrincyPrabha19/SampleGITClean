using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Classes.Profiles;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Models
{
    public class OverclockingModel : IOverclockingModel
    {
        public IProfileManager ProfileManager { get; set; }
        public ITuningManager TuningManager { get; set; }
        public ICPUDataConfigService CPUDataConfigService { get; set; }
        public IPlatformComponentManager PlatformComponentManager { get; set; }
        public IMonitorManager MonitorManager { get; set; }
        public IOCStatusManager OCStatusManager { get; set; }
        public IBIOSSupportProvider BIOSSupportProvider { get; set; }
        public bool IsOverclockingEnabled => OCStatusManager.IsOCEnabled;
        public bool IsSystemOverclockable { get; set; }
        public string ActiveProfileName => OCStatusManager.ActiveProfileName;
        public string CurrentProfileName => OCStatusManager.CurrentProfileName;
        public decimal CPUFrequency => ProfileManager.GetCPUFrequency();
        public decimal CPUVoltageMode => ProfileManager.GetVoltageMode();
        public int XMPProfileID => ProfileManager.GetXMPProfileID();
        public bool IsTempUnitCelsius { get; set; }
        public RebootMask RebootRequired { get; set; }
        public bool IsActiveProfileDeleted { get; set; }
        public bool IsPredefinedProfileSelected => ProfileManager.IsPredefinedProfileSelected();
        public bool IsXMPSelected => ProfileManager.IsXMPSelected();
        public bool IsBIOSInterfaceSupported => !BIOSSupportProvider.IsBIOSInterfaceNotSupported;
        public bool IsSupportAssistInstalled => SupportAssistServiceHelper.IsServiceInstalled();        

        public event Action<ProfileType> InvalidateProfile;
        public event Action<RebootMask> ReportRebootRequired;
        public event Action LoadCustomProfilesFailed;
        public event Action<RebootMask> MemoryRebootRequired;

        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        public void Initialize()
        {
            IsSystemOverclockable = SystemInfoRepository.Instance.CPUInfoData.IsOverclockSupported && IsBIOSInterfaceSupported && !AreAllPredefinedProfilesMissing();
            var overclockingEnabled =  OCStatusManager.IsOCEnabled && IsSystemOverclockable && !IsBIOSOCUIControlStatusEnabled();
            if (overclockingEnabled != OCStatusManager.IsOCEnabled)
                OCStatusManager.SaveOCStatus(overclockingEnabled);

            //aligning OC Controls and BIOS
            if (!overclockingEnabled && !IsBIOSOCUIControlStatusEnabled())
                BIOSSupportProviderFactory.NewBIOSSupportProvider().SetOCUIBIOSControl(true);
        }

        public void RefreshMetadata()
        {
            CPUDataConfigService?.Initialize();
            ProfileManager?.Initialize();
            Initialize();
        }

        public bool InvalidateSelectedProfile(string profileName)
        {
            bool loadProfileValuesFailed;
            var profile = ProfileManager.LoadProfile(profileName, false, out loadProfileValuesFailed);
            RebootMask rebootRequired;
            if (profile == null || profile.IsPredefinedProfile)
               return true;
            if (profile.Apply(false, out rebootRequired) || profile.DataHeader.Status == ValidationStatus.Invalidated.ToString())
                return true;
            profile.DataHeader.Status = ValidationStatus.Invalidated.ToString();
            return ProfileManager.SaveProfile(profile);
        }

        //public bool LoadingPredefinedProfileValuesFailed()
        //{
        //    return ProfileManager?.LoadingPredefinedProfileValuesFailed ?? false;
        //}

        public void EmptyProfile()
        {
            OCStatusManager?.SaveOCActiveAndCurrentProfile("");
        }

        public uint GetFreqRiskLevel(decimal currentFrequency)
        {
            return CPUDataConfigService.GetFreqRiskLevel(currentFrequency);
        }

        public uint GetCoreVoltageRiskLevel(decimal currentVoltage)
        {
            return CPUDataConfigService.GetCoreVoltageRiskLevel(currentVoltage);
        }

        public uint GetVoltageOffsetRiskLevel(decimal currentVoltageOffset)
        {
            return CPUDataConfigService.GetVoltageOffsetRiskLevel(currentVoltageOffset);
        }

        public bool IsAnyPredefinedProfileMissing()
        {
            return ProfileManager.PredefinedProfiles.Count > 0 && ProfileManager.PredefinedProfiles.Count < 2;
        }

        public bool AreAllPredefinedProfilesMissing()
        {
            return ProfileManager.PredefinedProfiles.Count == 0;
        }

        public bool IsDataConfigAvailable()
        {
            var signatureVerifier = SignatureVerifierFactory.NewSignatureVerifier();
            var dataConfigPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls\DataConfig.occ");
            if (!File.Exists(dataConfigPath) || !signatureVerifier.VerifySignature(dataConfigPath)) return false;
            return true;
        }

        public bool IsBIOSFailSafeFlagEnabled()
        {
            int overclockingReport;
            BIOSSupportProvider.RefreshOverclockingReport(out overclockingReport, false);
            return BIOSSupportProvider.IsOCFailsafeFlagStatusEnabled;
        }

        public bool IsBIOSOCUIControlStatusEnabled()
        {
            int overclockingReport;
            BIOSSupportProvider.RefreshOverclockingReport(out overclockingReport, false);
            return BIOSSupportProvider.IsOCUIBIOSControlStatusEnabled;
        }
        
        public bool IsCurrentProfileFailure(string profileName)
        {
            if (profileName == ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME || profileName == ProfilesPathProvider.STAGE2_PREDEFINED_PROFILENAME)
                return false;

            if (!LoadCurrentProfile(profileName)) return true;
            var profileStatus = ProfileManager.CurrentProfile.DataHeader.Status;
            if (ActiveProfileName == CurrentProfileName && profileStatus == ValidationStatus.Validated.ToString())
            {
                ProfileManager.CurrentProfile.DataHeader.Status = ValidationStatus.Invalidated.ToString();
                ProfileManager.SaveProfile(ProfileManager.CurrentProfile);
                return false;
            }

            return true;
        }

        public bool IsCPUOCEnabled
        {
            get { return ProfileManager?.IsCPUOCEnabled() ?? false; }
            set { ProfileManager?.SetCPUOCStatus(value); }
        }

        public bool IsMemoryOCEnabled
        {
            get { return ProfileManager?.IsMemoryOCEnabled() ?? false; }
            set {
                RebootMask memoryRebootRequired = RebootMask.NoRebootRequired;
                ProfileManager?.SetMemoryOCStatus(value, out memoryRebootRequired);
                var t = RebootRequired & RebootMask.CPUOCRebootRequired;
                RebootRequired = t | memoryRebootRequired;
                MemoryRebootRequired?.Invoke(memoryRebootRequired);
            }
        }

        public bool TurnOffOverclockingControl(out RebootMask rebootRequired)
        {
            OCStatusManager?.SaveOCStatus(false);
            rebootRequired = RebootMask.NoRebootRequired;
            var result = ApplyDefaultProfile(); //SetOCStatus(false, out rebootRequired);
            BIOSSupportProviderFactory.NewBIOSSupportProvider().SetOCUIBIOSControl(true);
            return result;
        }

        public bool TurnOnOverclockingControl(out RebootMask rebootRequired)
        {
            OCStatusManager?.SaveOCStatus(true);
            rebootRequired = RebootMask.NoRebootRequired;
            if (string.IsNullOrEmpty(OCStatusManager?.ActiveProfileName))
                OCStatusManager?.SaveOCActiveAndCurrentProfile(ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME);

            var activeProfileName = OCStatusManager?.ActiveProfileName ?? ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME;
            var result = SetOCProfile(activeProfileName, out rebootRequired);
            BIOSSupportProviderFactory.NewBIOSSupportProvider().SetOCUIBIOSControl(false);
            return result;
        }

        public bool SetOCStatus(bool isOverclockingEnabled, out RebootMask rebootRequired)
        {
            bool result = false;
            rebootRequired = RebootMask.NoRebootRequired;
            OCStatusManager?.SaveOCStatus(isOverclockingEnabled);
            //if (!isOverclockingEnabled)
            //    result = ApplyDefaultProfile();
            //else
            //{}
            //TODO: need to handle ISOverclocking disabled
            if(isOverclockingEnabled)
            { 
                if (string.IsNullOrEmpty(OCStatusManager?.ActiveProfileName))
                    OCStatusManager?.SaveOCActiveAndCurrentProfile(ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME);

                var activeProfileName = OCStatusManager?.ActiveProfileName ?? ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME;
                result = SetOCProfile(activeProfileName, out rebootRequired);
            }

          //  BIOSSupportProviderFactory.NewBIOSSupportProvider().SetOCUIBIOSControl(!isOverclockingEnabled);

            return result;
        }

        public bool SetOCProfile(string profileName, out RebootMask rebootRequired)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            var predefinedProfileNameList = ProfileManager?.GetPredefinedProfileNameList();
            if (predefinedProfileNameList?.Contains(profileName) ?? false)
                return ApplyProfile(profileName, out rebootRequired) && (OCStatusManager?.SaveOCActiveAndCurrentProfile(profileName) ?? false);

            var customProfileNameList = ProfileManager?.GetCustomProfileNameList();
            if (customProfileNameList?.Contains(profileName) ?? false)
                return ApplyProfile(profileName, out rebootRequired) && (OCStatusManager?.SaveOCActiveAndCurrentProfile(profileName) ?? false);
            
            if (ProfileManager?.RestoreDefaultProfile() ?? false)
                return OCStatusManager?.SaveOCActiveAndCurrentProfile(ProfileManager?.ActiveProfile?.Name) ?? false;

            return false;
        }

        public bool RestorePredefinedProfile()
        {
            if (ProfileManager?.RestoreDefaultProfile() ?? false)
                return OCStatusManager?.SaveOCActiveAndCurrentProfile(ProfileManager?.ActiveProfile?.Name) ?? false;
            return false;
        }

        public bool ApplyProfile(string profileName, out RebootMask rebootRequired)
        {
            rebootRequired=RebootMask.NoRebootRequired;
            var result = ProfileManager?.ApplyProfile(profileName, out rebootRequired) ?? false;
            RebootRequired = rebootRequired;
            return result;
        }

        public List<IProfileInfo> GetPredefinedProfileList()
        {
            return ProfileManager?.PredefinedProfiles.Select(p => new ProfileInfo { ProfileName = p.Name, IsPredefinedProfile = true, IsValid = true }).Cast<IProfileInfo>().ToList();
        }

        public List<IProfileInfo> GetCustomProfileList()
        {
            List<IProfileInfo> list = new List<IProfileInfo>();
            var customProfileNameList = ProfileManager?.GetCustomProfileNameList();
            if (customProfileNameList == null) return list;

            var loadingFailed = false;
            foreach (var profileName in customProfileNameList)
            {
                var validationStatus = ProfileManager.GetProfileValidationStatus(profileName);
                if (validationStatus == ValidationStatus.LoadingFailed)
                {
                    loadingFailed = true;
                    if (OCStatusManager.ActiveProfileName == profileName)
                        IsActiveProfileDeleted = true;
                    continue;
                }

                IProfileInfo info = new ProfileInfo {ProfileName = profileName, IsPredefinedProfile = false, IsValid = validationStatus == ValidationStatus.Validated};
                list.Add(info);
            }

            if (loadingFailed)
                LoadCustomProfilesFailed?.Invoke();

            return list;
        }

        public uint GetProcessorCores()
        {
            return SystemInfoRepository.Instance?.CPUInfoData.PhysicalCpuCores ?? 0;
        }

        public ISetting[] GetSettingArray(List<SettingType> settingTypes)
        {
            return PlatformComponentManager?.GetSettingArray(settingTypes);
        }
        public ISetting GetSetting(HWComponentType hwComponentType, SettingType settingType)
        {
            return PlatformComponentManager?.GetSetting(hwComponentType, settingType);
        }

        public ISetting GetSetting(SettingType settingType, uint coreIndex)
        {
            var setting = PlatformComponentManager?.GetSetting(settingType, coreIndex);
            return setting;
        }
        public bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired)
        {
            isRestartRequired = false;
            proposalResult = new List<ControlValue>();
            var result = TuningManager?.ProposeControls(out proposalResult, out isRestartRequired) ?? false;
            foreach (var proposal in proposalResult)
                if (getSettingType(proposal.Id) == SettingType.ActualFrequency)
                    proposal.Value *= GetSetting(HWComponentType.CPU, SettingType.BClockFrequency).Value;
            return result;
        }
        public bool IsMaximumFrequencyVisible()
        {
            return IsOverclockingEnabled && IsCPUOCEnabled;
        }

        public bool IsVoltageModeVisible()
        {
            return IsOverclockingEnabled && IsCPUOCEnabled;
        }

        public bool LoadCurrentProfile(string profileName)
        {
            bool loadProfileValuesFailed;
            var profile = ProfileManager.LoadProfile(profileName, false, out loadProfileValuesFailed);

            logger?.WriteError($"LoadCurrentProfile: {profileName} -> " + (profile != null ? "Success Load" : "Failed Load"));

            if (profile != null)
            {
                ProfileManager.CurrentProfile = profile;
                return true;
            }

            return false;
        }

        public void ClearCurrentProfile()
        {
            ProfileManager.CurrentProfile = null;
        }

        public bool ApplyDefaultProfile()
        {
            bool rebootRequired;
            return TuningManager.ApplySystemDefaultValues(out rebootRequired);
        }

        public decimal GetFanType()
        {
            return CPUDataConfigService.GetFanType();
        }
        public ConfiguredSettings GetFanRange()
        {
            return CPUDataConfigService.GetFanRange();
        }

        public void CheckProfileValidation()
        {
            if (IsOverclockingEnabled)
            {
                RebootMask rebootRequired;
                if (!IsBIOSFailSafeFlagEnabled())
                {
                    SetOCProfile(ActiveProfileName, out rebootRequired);
                    ReportRebootRequired?.Invoke(rebootRequired);
                    return;
                }

                if (IsCurrentProfileFailure(CurrentProfileName))
                {
                    LoadCurrentProfile(CurrentProfileName);
                    if (string.Compare(ActiveProfileName, CurrentProfileName, StringComparison.InvariantCultureIgnoreCase) != 0)
                        SetOCProfile(ActiveProfileName, out rebootRequired);
                    else
                    if (ProfileManager?.RestoreDefaultProfile() ?? false)
                        OCStatusManager?.SaveOCActiveAndCurrentProfile(ProfileManager?.ActiveProfile?.Name);
                       
                    InvalidateProfile?.Invoke(ProfileType.CurrentProfileUnderValidation);
                }
                else
                {
                    if (ActiveProfileName == ProfilesPathProvider.STAGE1_PREDEFINED_PROFILENAME ||
                        ActiveProfileName == ProfilesPathProvider.STAGE2_PREDEFINED_PROFILENAME)
                    {
                        InvalidateProfile?.Invoke(ProfileType.PredefinedProfileInvalid);
                        TurnOffOverclockingControl(out rebootRequired);
                        ReportRebootRequired?.Invoke(rebootRequired);
                    }
                    else
                    {
                        InvalidateProfile?.Invoke(ProfileType.ActiveProfileInvalid);
                        TurnOffOverclockingControl(out rebootRequired);
                        //SetOCStatus(false, out rebootRequired);
                        //ApplyDefaultProfile();
                        EmptyProfile();
                        ReportRebootRequired?.Invoke(rebootRequired);
                    }
                }
            }

            BIOSSupportProvider.ClearOCFailSafeFlag();
        }

        public void RestartSystem()
        {
            TuningManager.RestartSystem();
        }

        private IMemoryDataComponent getMemoryComponent()
        {
            return ProfileManager?.GetMemoryComponent();
        }
        private SettingType getSettingType(uint settingID)
        {
            return (SettingType)((settingID >> 16) & 255);
        }
    }
}
