using System.Collections.Generic;
using Dominator.Domain.Enums;
using Dominator.Tools;

namespace Dominator.Domain
{
    public interface IProfileManager
    {
        IProfileNameRepository ProfileNameRepository { get; set; }
        IProfileValidator ProfileValidator { get; set; }
        List<IProfile> PredefinedProfiles { get; set; }
        IProfile ActiveProfile { get; set; }
        IProfile CurrentProfile { get; set; }
        ISignatureVerifier SignatureVerifier { get; set; }
        bool LoadingPredefinedProfileValuesFailed { get; }

        void Initialize();
        bool IsSystemOverclocked();
        List<string> GetCustomProfileNameList();
        bool IsValidProfileName(string profileName);
        bool ApplyProfile(string profileName, out RebootMask rebootRequired, bool forceRestart = false, bool isNewProfile = false);
        decimal GetCPUFrequency();
        decimal GetVoltageMode();
        int GetXMPProfileID();
        bool IsPredefinedProfileSelected();
        bool IsCPUOCEnabled();
        bool IsMemoryOCEnabled();
        bool IsPredefinedProfile(string profileName);
        void SetCPUOCStatus(bool enabled);
        void SetMemoryOCStatus(bool enabled, out RebootMask memoryRebootRequired);
        IMemoryDataComponent GetMemoryComponent();
        IProfile LoadProfile(string profileName, bool isPredefinedProfile, out bool loadProfileValuesFailed);
        IProfile CreateNewProfile(string profileName);
        bool SaveProfile(IProfile profile);
        IProfile GetPredefinedProfile();
        IProfile GetOC2Profile();
        bool ApplyProfile(IProfile profile, out RebootMask rebootRequired, bool isNewProfile = false);
        bool DeleteCurrentProfile();
        int GetCurrentProfileXMPProfileID();
        decimal GetCurrentProfileFrequency();
        decimal GetCurrentProfileVoltage();
        decimal GetCurrentProfileVoltageOffset();
        decimal GetCurrentProfileVoltageMode();
        string GetCurrentProfileThermalMode();
        ValidationStatus GetProfileValidationStatus(string profileName);
        bool RestoreDefaultProfile();
        List<string> GetPredefinedProfileNameList();
        List<IDataComponent> GetPredefinedProfileDataComponents();
        bool IsXMPSelected();
    }
}
