using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Classes.Helpers;
using Dominator.Domain.Classes.Profiles;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes
{
    public class ProfileManager : ProfilesPathProvider, IProfileManager
    {
        public IProfileNameRepository ProfileNameRepository { get; set; }
        public IProfileValidator ProfileValidator { get; set; }
        public ISignatureVerifier SignatureVerifier { get; set; }
        public List<IProfile> PredefinedProfiles { get; set; }
        public IProfile ActiveProfile { get; set; }
        public IProfile CurrentProfile { get; set; }
        public bool LoadingPredefinedProfileValuesFailed { get; private set; }

        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        public void Initialize()
        {
            ProfileNameRepository.RefreshPredefinedProfileNameList();

            PredefinedProfiles = new List<IProfile>();
            foreach (var p in ProfileNameRepository.PredefinedProfileNameList)
            {
                bool loadProfileValuesFailed;
                var profile = LoadProfile(p.Key, true, out loadProfileValuesFailed);
                //LoadingPredefinedProfileValuesFailed |= loadProfileValuesFailed;
                //if (profile != null && !loadProfileValuesFailed)
                if (profile != null)
                {
                    var validProfile = ProfileValidator.IsValidProfileForSystem(profile);
                    var verificationPassed = SignatureVerifier.VerifySignature(p.Value);
                    if (validProfile && verificationPassed)
                        PredefinedProfiles.Add(profile);
                }
            }
        }

        public IProfile CreateNewProfile(string profileName)
        {
            var profile = ProfileFactory.NewCustomProfile(profileName);
            return profile;
        }

        public bool SaveProfile(IProfile profile)
        {
            return profile?.Save() ?? false;
        }

        public IProfile LoadProfile(string profileName, bool isPredefinedProfile, out bool loadProfileValuesFailed)
        {
            loadProfileValuesFailed = false;
            var profilePath = isPredefinedProfile ? ProfileNameRepository.GetPredefinedProfilePath(profileName)
                : ProfileNameRepository.GetCustomProfilePath(profileName);
            if (string.IsNullOrEmpty(profilePath)) return null;

            var profile = isPredefinedProfile ? ProfileFactory.NewPredefinedProfile(profileName, profilePath) 
                : ProfileFactory.NewCustomProfile(profileName, profilePath);
            return profile.Load(out loadProfileValuesFailed) ? profile : null;
        }

        public bool ApplyProfile(string profileName, out RebootMask rebootRequired, bool forceRestart = false, bool isNewProfile = false)
        {
            bool loadProfileValuesFailed;
            rebootRequired = RebootMask.NoRebootRequired;
            var profile = PredefinedProfiles.FirstOrDefault(p => string.Compare(p.Name, profileName, StringComparison.InvariantCultureIgnoreCase) == 0);
            if (profile == null)
                profile = LoadProfile(profileName, false, out loadProfileValuesFailed);

            if (profile == null) return false;
            if (profile.Apply(isNewProfile, out rebootRequired, false))
            {
                ActiveProfile = profile;
                return true;
            }

            return false;
        }

        public bool ApplyProfile(IProfile profile, out RebootMask rebootRequired, bool isNewProfile = true)
        {
            if (profile.Apply(isNewProfile, out rebootRequired))
            {
                ActiveProfile = profile;
                return true;
            }

            return false;
        }

        public bool DeleteCurrentProfile()
        {
            if (CurrentProfile == null) return false;
            var profilePath = CurrentProfile.Path;

            try
            {                
                if (File.Exists(profilePath))
                    File.Delete(profilePath);
            }
            catch (Exception e)
            {
                logger?.WriteError($"DeleteCurrentProfile({profilePath})", "Review file permissions", e.ToString());
            }

            return true;
        }

        public bool DeleteProfile(string profileName)
        {
            var profilePath = ProfileNameRepository.GetCustomProfilePath(profileName);
            if (string.IsNullOrEmpty(profilePath)) return false;

            try
            {
                if (File.Exists(profilePath))
                    File.Delete(profilePath);
            }
            catch (Exception e)
            {
                logger?.WriteError($"DeleteCurrentProfile({profilePath})", "Review file permissions", e.ToString());
            }                

            return true;
        }

        public int GetCurrentProfileXMPProfileID()
        {
            return CurrentProfile?.GetXMPProfileID() ?? 0;
        }

        public decimal GetCurrentProfileFrequency()
        {
            return CurrentProfile?.GetCPUFrequency() ?? 0;
        }

        public decimal GetCurrentProfileVoltage()
        {
            return CurrentProfile?.GetCPUVoltage() ?? 0;
        }

        public decimal GetCurrentProfileVoltageOffset()
        {
            return CurrentProfile?.GetCPUVoltageOffset() ?? 0;
        }

        public decimal GetCurrentProfileVoltageMode()
        {
            return CurrentProfile?.GetCPUVoltageMode() ?? 0;
        }

        public string GetCurrentProfileThermalMode()
        {
            return CurrentProfile?.GetThermalMode();
        }

        public ValidationStatus GetProfileValidationStatus(string profileName)
        {
            bool loadProfileValuesFailed;
            var profile = LoadProfile(profileName, false, out loadProfileValuesFailed);
            if (profile == null)
            {
                DeleteProfile(profileName);          
                return ValidationStatus.LoadingFailed;
            }

            ValidationStatus validationStatus;
            if (Enum.TryParse(profile.DataHeader.Status, out validationStatus))
                return validationStatus;
            return ValidationStatus.Invalidated;
        }

        /// <summary>
        /// if OC1 & OC2 are not present OC is disabled, otherwise either OC1 or OC2 are present
        /// </summary>
        /// <returns></returns>
        public bool RestoreDefaultProfile()
        {
            RebootMask rebootRequired;
            var profilePath = ProfileNameRepository.GetPredefinedProfilePath(STAGE1_PREDEFINED_PROFILENAME);
            if (!string.IsNullOrEmpty(profilePath))
                return ApplyProfile(STAGE1_PREDEFINED_PROFILENAME, out rebootRequired);

            return ApplyProfile(STAGE2_PREDEFINED_PROFILENAME, out rebootRequired);
        }

        public bool IsSystemOverclocked()
        {
            var systemState = new SystemState();
            return systemState.IsSystemOverclocked();
        }

        public decimal GetCPUFrequency()
        {
            if (ActiveProfile != null)
                return ActiveProfile.GetCPUFrequency();
            return 0;
        }

        public decimal GetVoltageMode()
        {
            if (ActiveProfile != null)
                return ActiveProfile.GetCPUVoltageMode();
            return 0;
        }

        public bool IsPredefinedProfileSelected()
        {
            return ActiveProfile != null && ActiveProfile.IsPredefinedProfile;
        }

        public bool IsPredefinedProfile(string profileName)
        {
            var profile = PredefinedProfiles.FirstOrDefault(p => string.Compare(p.Name, profileName, StringComparison.InvariantCultureIgnoreCase) == 0);
            return profile != null;
        }

        public bool IsCPUOCEnabled()
        {
            return ActiveProfile?.IsCPUOCEnabled() ?? false;
        }

        public void SetCPUOCStatus(bool enabled)
        {
            ActiveProfile?.SetCPUOCStatus(enabled);
            ActiveProfile?.Save();
            ActiveProfile?.SetCurrentCPUProfile();
        }

        public bool IsMemoryOCEnabled()
        {
            return ActiveProfile?.IsMemoryOCEnabled() ?? false;
        }

        public void SetMemoryOCStatus(bool enabled, out RebootMask rebootRequired)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            ActiveProfile?.SetMemoryOCStatus(enabled);
            ActiveProfile?.Save();
            ActiveProfile?.SetCurrentMemoryProfile(out rebootRequired);
        }

        public int GetXMPProfileID()
        {
            if (ActiveProfile != null)
                return ActiveProfile.GetXMPProfileID();
            return 0;
        }

        public List<string> GetCustomProfileNameList()
        {
            ProfileNameRepository?.RefreshCustomProfileNameList();
            return ProfileNameRepository?.CustomProfileNameList.Keys.ToList();
        }

        public List<string> GetPredefinedProfileNameList()
        {
            ProfileNameRepository?.RefreshPredefinedProfileNameList();
            return ProfileNameRepository?.PredefinedProfileNameList.Keys.ToList();
        }

        public List<IDataComponent> GetPredefinedProfileDataComponents()
        {
            if (PredefinedProfiles.FirstOrDefault(profile => profile.Name == STAGE1_PREDEFINED_PROFILENAME) != null)
                return PredefinedProfiles.FirstOrDefault(profile => profile.Name == STAGE1_PREDEFINED_PROFILENAME)?.DataComponents;
            return PredefinedProfiles.FirstOrDefault(profile => profile.Name == STAGE2_PREDEFINED_PROFILENAME)?.DataComponents;
        }

        public bool IsXMPSelected()
        {
            return ActiveProfile != null && ActiveProfile.GetXMPProfileID()!=0;
        }

        public bool IsValidProfileName(string profileName)
        {
            var profileNames = GetCustomProfileNameList();
            return !profileNames.Exists(x => string.Compare(x, profileName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public IMemoryDataComponent GetMemoryComponent()
        {
            var profileComponents = ActiveProfile?.DataComponents;
            return profileComponents?.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
        }

        public IProfile GetPredefinedProfile()
        {
            if(PredefinedProfiles.FirstOrDefault(p=>p.Name == STAGE1_PREDEFINED_PROFILENAME)!=null)
              return PredefinedProfiles.FirstOrDefault(p => string.Compare(p.Name, STAGE1_PREDEFINED_PROFILENAME, StringComparison.InvariantCultureIgnoreCase) == 0);
            return PredefinedProfiles.FirstOrDefault(p => string.Compare(p.Name, STAGE2_PREDEFINED_PROFILENAME, StringComparison.InvariantCultureIgnoreCase) == 0);
        }

        public IProfile GetOC2Profile()
        {
            return PredefinedProfiles.FirstOrDefault(p => string.Compare(p.Name, "OC2", StringComparison.InvariantCultureIgnoreCase) == 0);
        }
    }
}