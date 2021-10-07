using System;
using System.Collections.Generic;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain
{
    public interface IOverclockingModel
    {
        IProfileManager ProfileManager { get; set; }
        ITuningManager TuningManager { get; set; }
        ICPUDataConfigService CPUDataConfigService { get; set; }
        IPlatformComponentManager PlatformComponentManager { get; set; }
        IMonitorManager MonitorManager { get; set; }
        IOCStatusManager OCStatusManager { get; set; }
        IBIOSSupportProvider BIOSSupportProvider { get; set; }
        bool IsOverclockingEnabled { get;}
        bool IsSystemOverclockable { get; set; }
        string ActiveProfileName { get; }
        string CurrentProfileName { get; }
        decimal CPUFrequency { get; }
        decimal CPUVoltageMode { get; }
        bool IsPredefinedProfileSelected { get; }
        bool IsXMPSelected { get; }
        bool IsCPUOCEnabled { get; set; }
        bool IsMemoryOCEnabled { get; set; }
        int XMPProfileID { get; }
        bool IsBIOSInterfaceSupported { get; }
        bool IsSupportAssistInstalled { get; }
        bool IsTempUnitCelsius { get; set; }
        RebootMask RebootRequired { get; set; }
        bool IsActiveProfileDeleted { get; set; }
        void Initialize();
        decimal GetFanType();
        ConfiguredSettings GetFanRange();
        bool IsDataConfigAvailable();
        bool IsBIOSFailSafeFlagEnabled();
        bool IsCurrentProfileFailure(string profileName);
        bool SetOCStatus(bool isOverclockingEnabled, out RebootMask rebootRequired);
        bool SetOCProfile(string profileName, out RebootMask rebootRequired);

        event Action<ProfileType> InvalidateProfile;
        event Action<RebootMask> ReportRebootRequired;
        event Action LoadCustomProfilesFailed;
        event Action<RebootMask> MemoryRebootRequired;

        List<IProfileInfo> GetPredefinedProfileList();
        List<IProfileInfo> GetCustomProfileList();
        ISetting[] GetSettingArray(List<SettingType> settingTypes);
        ISetting GetSetting(HWComponentType hwComponentType, SettingType settingType);
        ISetting GetSetting(SettingType settingType, uint coreIndex);
        uint GetProcessorCores();
        bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired);
        void RestartSystem();
        bool ApplyProfile(string profileName, out RebootMask rebootRequired);
        bool IsMaximumFrequencyVisible();
        bool IsVoltageModeVisible();
        bool LoadCurrentProfile(string profileName);
        void ClearCurrentProfile();
        bool ApplyDefaultProfile();
        void CheckProfileValidation();
        void EmptyProfile();
        uint GetFreqRiskLevel(decimal currentFrequency);
        uint GetCoreVoltageRiskLevel(decimal currentVoltage);
        uint GetVoltageOffsetRiskLevel(decimal currentVoltageOffset);
        bool IsAnyPredefinedProfileMissing();
        bool AreAllPredefinedProfilesMissing();
        bool IsBIOSOCUIControlStatusEnabled();
        bool RestorePredefinedProfile();
        void RefreshMetadata();
        bool InvalidateSelectedProfile(string profileName);
     //   bool LoadingPredefinedProfileValuesFailed();
        bool TurnOnOverclockingControl(out RebootMask rebootRequired);
        bool TurnOffOverclockingControl(out RebootMask rebootRequired);
    }
}
