using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Intel.Overclocking.SDK.Monitoring;
using Intel.Overclocking.SDK.Profile;
using Intel.Overclocking.SDK.ServiceInfo;
using Intel.Overclocking.SDK.Tuning;

namespace Dominator.ServiceModel.Classes
{
    public class XTUSDKLibrary
    {        
        public States State { get; private set; }

        private IProfileLibrary profileLibrary;
        private ITuningLibrary tuningLibrary;
        private IServiceInfoLibrary serviceInfoLibrary;
        private IMonitoringLibrary monitoringLibrary;

        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        public void Initialize()
        {
            //XTULibraryLoader.TryLoadAssemblies();

            profileLibrary = ProfileLibrary.Instance;
            profileLibrary.Initialize();
            tuningLibrary = TuningLibrary.Instance;
            tuningLibrary.Initialize();
            serviceInfoLibrary = ServiceInfoLibrary.Instance;
            serviceInfoLibrary.Initialize();
            monitoringLibrary = MonitoringLibrary.Instance;
            monitoringLibrary.Initialize();
        }

        public bool Load(string path)
        {
            ImportState importState = profileLibrary.AddProfile(path);

            switch (importState)
            {
                case ImportState.ImportSucceeded:
                case ImportState.ProfileAlreadyExist:
                    State = (States)importState;
                    if (isRestartRequired(Path.GetFileNameWithoutExtension(path)))
                        State = States.RestartRequired;
                    return true;
                   
                case ImportState.OverclockingLocked:
                case ImportState.EistDisabled:
                case ImportState.TurboDisabled:
                case ImportState.ImportInvalidPlatform:
                case ImportState.ImportCanceled:
                case ImportState.InvalidFile:
                case ImportState.ProcessorMismatch:
                    State = (States)importState;
                    return false;
                    
                default:
                    State = States.Unknown;
                    return false;
            }
        }

        public bool Apply(string name, out bool rebootRequired, bool forceRestart = false)
        {
            rebootRequired = false;
            var proposeResult = profileLibrary.ProposeProfile(name);
            logger?.WriteLine($"profileLibrary.ProposeProfile({name}) {proposeResult}");
            if (proposeResult)
            {
                var controls = profileLibrary.GetProfile(name).TuningProfile;
                var iccMax = controls.ProposedValues.FirstOrDefault(
                    ctrl => ctrl.ControlId == ((decimal) 0x00000066).ToString(CultureInfo.InvariantCulture))
                    .ProposedValue.Value;
                var cacheIccMax = controls.ProposedValues.FirstOrDefault(
                    ctrl => ctrl.ControlId == ((decimal) 0x0000006A).ToString(CultureInfo.InvariantCulture))
                    .ProposedValue.Value;

                var iccMaxBootValue = ControlBootValue(0x00000066);
                var cacheIccMaxBootValue = ControlBootValue(0x0000006A);
                rebootRequired = (iccMax!=0 && iccMax != iccMaxBootValue) || (cacheIccMax!=0 && cacheIccMax != cacheIccMaxBootValue);

                logger?.WriteLine($"{(rebootRequired ? "REBOOT REQUIRED" : "NOT REBOOT REQUIRED")}: iccMax[0x66]:{iccMax} iccMaxBoot[0x66]:{iccMaxBootValue} cacheIccMax[0x6A]:{cacheIccMax} cacheIccMaxBoot[0x6A]:{cacheIccMaxBootValue} ");

                var applyResult = profileLibrary.ApplyProfile(forceRestart);
                logger?.WriteLine($"profileLibrary.ApplyProfile({name}) {applyResult}");
                return applyResult;
            }

            return false;
        }

        private bool isRestartRequired(string name)
        {               
            var currentProfile = profileLibrary.GetProfile(name);
            return currentProfile.TuningProfile.RebootRequired;
        }

        public decimal GetProfileSettings(string name, uint controlID)
        {
            var tunes = name == "Default" ? profileLibrary.GetDefaultProfile().TuningProfile.ProposedValues : profileLibrary.GetProfile(name).TuningProfile.ProposedValues;
            var value = tunes.FirstOrDefault(ctrl => ctrl.ControlId == controlID.ToString()).ProposedValue.Value;
            return value;
        }
        public bool IsSystemOverClocked()
        {
            int modifiedControls = tuningLibrary.GetAvailableControls().Where(control => control.Id != 80).Count(control => control.ActiveValue != control.DefaultValue);
            return modifiedControls > 0;
        }

        public bool ApplyDefaultProfile(out bool isRestartRequired)
        {
            isRestartRequired = false;
            List<ClientTuningProposal> settings = new List<ClientTuningProposal>();
            var profileControls = profileLibrary.GetDefaultProfile().TuningProfile.ProposedValues;
            logger?.WriteLine($"ApplyDefaultProfile: profileControls: {profileControls?.Count ?? -1}");
            if (profileControls == null) return false;

            var cpuActualFrequencyControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUActualFrequency).ToString());
            //if (!string.IsNullOrEmpty(cpuActualFrequencyControl.ControlId))
            settings.Add(new ClientTuningProposal
            {
                Id = (uint)XTUID.CPUActualFrequency,
                Value = cpuActualFrequencyControl.ProposedValue.Value
            });

            var cpuActualVoltageControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUActualVoltage).ToString());            
            //if (!string.IsNullOrEmpty(cpuActualVoltageControl.ControlId))
                settings.Add(new ClientTuningProposal
                {
                    Id = (uint)XTUID.CPUActualVoltage,
                    Value = cpuActualVoltageControl.ProposedValue.Value
                });

            var cpuVoltageModeControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUVoltageMode).ToString());
            //if (!string.IsNullOrEmpty(cpuVoltageModeControl.ControlId))
                settings.Add(new ClientTuningProposal
                {
                    Id = (uint)XTUID.CPUVoltageMode,
                    Value = cpuVoltageModeControl.ProposedValue.Value
                });

            var cpuVoltageOffsetControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUVoltageOffset).ToString());
            //if (!string.IsNullOrEmpty(cpuVoltageOffsetControl.ControlId))
                settings.Add(new ClientTuningProposal
                {
                    Id = (uint)XTUID.CPUVoltageOffset,
                    Value = cpuVoltageOffsetControl.ProposedValue.Value
                });

            var turboBoostPowerMaxControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.TurboBoostPowerMax).ToString());
            //if (!string.IsNullOrEmpty(turboBoostPowerMaxControl.ControlId))
                settings.Add(new ClientTuningProposal
                {
                    Id = (uint)XTUID.TurboBoostPowerMax,
                    Value = turboBoostPowerMaxControl.ProposedValue.Value
                });

            var turboBoostShortPowerMaxControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.TurboBoostShortPowerMax).ToString());
            //if (!string.IsNullOrEmpty(turboBoostShortPowerMaxControl.ControlId))
                settings.Add(new ClientTuningProposal
                {
                    Id = (uint)XTUID.TurboBoostShortPowerMax,
                    Value = turboBoostShortPowerMaxControl.ProposedValue.Value
                });

            var logStr = string.Empty;
            for (uint i = 0; i < GetPhysicalCpuCores(); i++)
            {
                var coreActualFrequencyBaseControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)(XTUID.CoreActualFrequencyBase + i)).ToString());
                //if (!string.IsNullOrEmpty(coreActualFrequencyBaseControl.ControlId))
                    settings.Add(new ClientTuningProposal
                    {
                        Id = (uint)XTUID.CoreActualFrequencyBase + i,
                        Value = coreActualFrequencyBaseControl.ProposedValue.Value
                    });
                logStr += $"{Environment.NewLine}coreActualFrequencyBase{i}:{coreActualFrequencyBaseControl.ControlId}={coreActualFrequencyBaseControl.ProposedValue.Value}";
            }

            logStr += $"{Environment.NewLine}cpuActualFrequency:{cpuActualFrequencyControl.ControlId}={cpuActualFrequencyControl.ProposedValue.Value}{Environment.NewLine}cpuActualVoltage:{cpuActualVoltageControl.ControlId}={cpuActualVoltageControl.ProposedValue.Value}{Environment.NewLine}cpuVoltageMode:{cpuVoltageModeControl.ControlId}={cpuVoltageModeControl.ProposedValue.Value}{Environment.NewLine}cpuVoltageOffset:{cpuVoltageOffsetControl.ControlId}={cpuVoltageOffsetControl.ProposedValue.Value}{Environment.NewLine}turboBoostPowerMax:{turboBoostPowerMaxControl.ControlId}={turboBoostPowerMaxControl.ProposedValue.Value}{Environment.NewLine}turboBoostShortPowerMax:{turboBoostShortPowerMaxControl.ControlId}={turboBoostShortPowerMaxControl.ProposedValue.Value}";
            logger?.WriteLine("ApplyDefaultProfile:" + logStr);

            bool result = tuningLibrary.Tune(settings, out isRestartRequired);
            return result;
            
            //if (profileLibrary.GetDefaultProfile() != null)
            //   return Apply("Default");
            //return false;
        }

        public string GetProcessorBrand()
        {
            return serviceInfoLibrary.Processor.GetBrandString();
        }

        public uint GetPhysicalCpuCores()
        {
            return serviceInfoLibrary.Processor.GetPhysicalCpuCoreCount();
        }

        public uint GetLogicalCpuCores()
        {
            return serviceInfoLibrary.Processor.GetLogicalCpuCoreCount();
        }

        public string GetFeatureFlags()
        {
            return serviceInfoLibrary.Processor.GetCpuFeatureFlags();
        }

        public bool IsTurboOverclockable()
        {
            return serviceInfoLibrary.Processor.IsTurboBoostTechnologyEnabled();
        }

        public bool IsOverclockingSupported()
        {
         return serviceInfoLibrary.Processor.IsOverclockSupported();
        }

        public decimal GetValueOfControl(uint controlId)
        {
            return monitoringLibrary.GetValue(controlId);
        }
        
        public bool TuneControl(uint controlID, decimal controlValue, out bool isRestartRequired)
        {
            bool result = tuningLibrary.Tune(controlID, controlValue, out isRestartRequired);

            return result;
        }

        public bool TuneListOfControls(List<ControlValue> proposals, out bool isRestartRequired)
        {
            List<ClientTuningProposal> tunings = new List<ClientTuningProposal>();
            foreach (var tune in proposals)
            {
                ClientTuningProposal ctrl = new ClientTuningProposal
                {
                    Id = tune.Id,
                    Value = tune.Value
                };
                tunings.Add(ctrl);
            }
            
            bool result = tuningLibrary.Tune(tunings, out isRestartRequired);
            return result;
        }

        public decimal GetControlValue(uint controlID)
        {
            return tuningLibrary.GetControl(controlID).ActiveValue;
        }

        public decimal GetMaxControlValue(uint controlID)
        {
            return Convert.ToDecimal(tuningLibrary.GetControl(controlID).GetMaxPossibleValue());
        }

        public decimal GetMinControlValue(uint controlID)
        {
            return Convert.ToDecimal(tuningLibrary.GetControl(controlID).GetMinPossibleValue());
        }

        public decimal SettingsStepValue(uint controlID)
        {
            List<decimal> values = tuningLibrary.GetControl(controlID).SupportedValues;
            //TODO:create custom progress bar in view that displays the supported values
            if (values.Count < 2)          
               return 0;
            if (values.Count == 2)
                return values[1] - values[0];

            var diff = values[2] - values[1];
            decimal dif;
            for (var i = 3; i < values.Count; i++)
            {
                dif = values[i] - values[i - 1];
                if (dif != diff)
                    return -1;
            }
            return diff;
            //var ctrl = tuningLibrary.GetControl(controlID);
            //return ctrl.SupportedValues[0];
        }
        public bool ProposeChanges(List<ControlValue> proposals, out List<ControlValue> proposalResult, out bool requiresReboot)
        {
            List<ClientTuningProposal> tunings = new List<ClientTuningProposal>();
            List<ClientTuningProposalResult> proposalDelta;
            proposalResult = new List<ControlValue>();

            foreach (var tune in proposals)
            {
                ClientTuningProposal ctrl = new ClientTuningProposal
                {
                    Id = tune.Id,
                    Value = tune.Value
                };
                tunings.Add(ctrl);
            }
            bool result = tuningLibrary.ProposeChange(tunings, out proposalDelta, out requiresReboot);
            foreach (var control in proposalDelta)
            {
                ControlValue ctrl = new ControlValue
                {
                    Id = control.Id,
                    Value = control.Value
                };
                proposalResult.Add(ctrl);
            }
            return result;
        }

        public bool ApplyChanges(bool forceRestart)
        {
            return tuningLibrary.ApplyChanges(forceRestart);
        }
        
        public List<ClientTuningProposalResult> DiscardChanges()
        {
            return tuningLibrary.DiscardChanges();
        }

        //public bool LoadXTUProfileValues(string name, out List<ProfileSetting> profileSettings)
        //{
        //    var profileSettingsAvailable = true;
        //    profileSettings = new List<ProfileSetting>();
        //    var profileControls = name == "Default" ? profileLibrary.GetDefaultProfile().TuningProfile.ProposedValues : profileLibrary.GetProfile(name).TuningProfile.ProposedValues;
        //    logger?.WriteLine($"LoadXTUProfileValues: profileControls: {profileControls?.Count ?? -1}");
        //    if (profileControls == null) return false;

        //    var baseClockFrequencyControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.BaseClockFrequency).ToString());
        //    if (string.IsNullOrEmpty(baseClockFrequencyControl.ControlId))
        //        profileSettingsAvailable = false;
        //    profileSettings.Add(new ProfileSetting
        //    {
        //        Id = (uint)XTUID.BaseClockFrequency,
        //        Value = baseClockFrequencyControl.ProposedValue.Value
        //    });

        //    var cpuActualVoltageControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUActualVoltage).ToString());
        //    if (string.IsNullOrEmpty(cpuActualVoltageControl.ControlId))
        //        profileSettingsAvailable = false;
        //    profileSettings.Add(new ProfileSetting
        //    {
        //        Id = (uint)XTUID.CPUActualVoltage,
        //        Value = cpuActualVoltageControl.ProposedValue.Value
        //    });

        //    var cpuActualFrequencyControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUActualFrequency).ToString());
        //    //if (string.IsNullOrEmpty(cpuActualFrequencyControl.ControlId))
        //    //    profileSettingsAvailable = false;
        //    profileSettings.Add(new ProfileSetting
        //    {
        //        Id = (uint)XTUID.CPUActualFrequency,
        //        Value = cpuActualFrequencyControl.ProposedValue.Value
        //    });

        //    var cpuVoltageModeControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUVoltageMode).ToString());
        //    if (string.IsNullOrEmpty(cpuVoltageModeControl.ControlId))
        //        profileSettingsAvailable = false;
        //    profileSettings.Add(new ProfileSetting
        //    {
        //        Id = (uint)XTUID.CPUVoltageMode,
        //        Value = cpuVoltageModeControl.ProposedValue.Value
        //    });

        //    var cpuVoltageOffsetControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)XTUID.CPUVoltageOffset).ToString());
        //    if (string.IsNullOrEmpty(cpuVoltageOffsetControl.ControlId))
        //        profileSettingsAvailable = false;
        //    profileSettings.Add(new ProfileSetting
        //    {
        //        Id = (uint)XTUID.CPUVoltageOffset,
        //        Value = cpuVoltageOffsetControl.ProposedValue.Value
        //    });

        //    var logStr = string.Empty;
        //    for (uint i = 0; i < GetPhysicalCpuCores(); i++)
        //    {
        //        var coreActualFrequencyBaseControl = profileControls.FirstOrDefault(ctrl => ctrl.ControlId == ((uint)(XTUID.CoreActualFrequencyBase + i)).ToString());
        //        if (string.IsNullOrEmpty(coreActualFrequencyBaseControl.ControlId))
        //            profileSettingsAvailable = false;
        //        profileSettings.Add(new ProfileSetting
        //        {
        //            Id = (uint)XTUID.CoreActualFrequencyBase + i,
        //            Value = coreActualFrequencyBaseControl.ProposedValue.Value
        //        });

        //        logStr += $"{Environment.NewLine}coreActualFrequencyBase{i}:{coreActualFrequencyBaseControl.ControlId}={coreActualFrequencyBaseControl.ProposedValue.Value}";
        //    }

        //    logStr += $"{Environment.NewLine}cpuActualVoltage:{cpuActualVoltageControl.ControlId}={cpuActualVoltageControl.ProposedValue.Value}{Environment.NewLine}cpuVoltageMode:{cpuVoltageModeControl.ControlId}={cpuVoltageModeControl.ProposedValue.Value}{Environment.NewLine}cpuVoltageOffset:{cpuVoltageOffsetControl.ControlId}={cpuVoltageOffsetControl.ProposedValue.Value}{Environment.NewLine}baseClockFrequency:{baseClockFrequencyControl.ControlId}={baseClockFrequencyControl.ProposedValue.Value}{Environment.NewLine}cpuActualFrequency:{cpuActualFrequencyControl.ControlId}={cpuActualFrequencyControl.ProposedValue.Value}";
        //    logger?.WriteLine("LoadXTUProfileValues:" + logStr);

        //    List<ProfileSetting> memorySettings;
        //    if (loadMemoryProfileValues(out memorySettings))
        //        profileSettings.AddRange(memorySettings);
        //    return profileSettingsAvailable;
        //}

        public bool LoadXTUProfileValues(string name, out List<ProfileSetting> profileSettings)
        {
            var logStr = string.Empty;
            profileSettings = new List<ProfileSetting>();

            bool isDefaultProfile = name == "Default";
            var tuningProfile = isDefaultProfile ? profileLibrary.GetDefaultProfile() : profileLibrary.GetProfile(name);
            if (tuningProfile == null) return false;

            profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.BaseClockFrequency,
                Value = findControlValue(tuningProfile, (uint)XTUID.BaseClockFrequency) ?? 0
            });
            logStr += $"{Environment.NewLine}BaseClockFrequency: {profileSettings[profileSettings.Count - 1].Value}";

            profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.CPUActualVoltage,
                Value = findControlValue(tuningProfile, (uint)XTUID.CPUActualVoltage) ?? 0
            });
            logStr += $"{Environment.NewLine}CPUActualVoltage: {profileSettings[profileSettings.Count - 1].Value}";

            if (isDefaultProfile)
            {
                profileSettings.Add(new ProfileSetting
                {
                    Id = (uint) XTUID.CPUActualFrequency,
                    Value = findControlValue(tuningProfile, (uint) XTUID.CPUActualFrequency) ?? 0
                });
                logStr += $"{Environment.NewLine}CPUActualFrequency: {profileSettings[profileSettings.Count - 1].Value}";
            }

            profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.CPUVoltageMode,
                Value = findControlValue(tuningProfile, (uint)XTUID.CPUVoltageMode) ?? 0
            });
            logStr += $"{Environment.NewLine}CPUVoltageMode: {profileSettings[profileSettings.Count - 1].Value}";

            profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.CPUVoltageOffset,
                Value = findControlValue(tuningProfile, (uint)XTUID.CPUVoltageOffset) ?? 0
            });
            logStr += $"{Environment.NewLine}CPUVoltageOffset: {profileSettings[profileSettings.Count - 1].Value}";

            for (uint i = 0; i < GetPhysicalCpuCores(); i++)
            {
                if (i == 0 && !isDefaultProfile)
                {
                    profileSettings.Add(new ProfileSetting
                    {
                        Id = (uint)XTUID.CPUActualFrequency,
                        Value = findControlValue(tuningProfile, (uint)XTUID.CoreActualFrequencyBase + i) ?? 0
                    });
                    logStr += $"{Environment.NewLine}CPUActualFrequency: {profileSettings[profileSettings.Count - 1].Value}";
                }

                profileSettings.Add(new ProfileSetting
                {
                    Id = (uint)XTUID.CoreActualFrequencyBase + i,
                    Value = findControlValue(tuningProfile, (uint)XTUID.CoreActualFrequencyBase + i) ?? 0
                });
                logStr += $"{Environment.NewLine}CoreActualFrequencyBase{i}: {profileSettings[profileSettings.Count - 1].Value}";
            }

            List<ProfileSetting> memorySettings;
            if (loadMemoryProfileValues(out memorySettings))
                profileSettings.AddRange(memorySettings);

            logger?.WriteLine($"LoadXTUProfileValues: {name} {logStr}");

            return true;
        }

        private static decimal? findControlValue(XtuTuningProfile tuningProfile, uint controlID)
        {
            var tuningItem = tuningProfile.TuningProfile.ProposedValues.FirstOrDefault(ctrl => ctrl.ControlId == controlID.ToString());
            if (!string.IsNullOrEmpty(tuningItem.ControlId))
                return tuningItem.ProposedValue.Value;

            var tuningItemControl = tuningProfile.Controls.FirstOrDefault(ctrl => ctrl.Id == controlID.ToString());
            if (!string.IsNullOrEmpty(tuningItemControl.Id))
                return tuningItemControl.ProposedValue.Value;

            return null;
        }

        public bool XmpSupported()
        {
            var xmpProfiles = tuningLibrary.GetXmpProfiles();
            return xmpProfiles.Any(profile => profile.Any(control => control.Value != 0));
        }

        public int NumberOfXMPProfiles()
        {
            int profiles = 0;
            var xmpProfiles = tuningLibrary.GetXmpProfiles();
            foreach (var profile in xmpProfiles)
            {
                if (validProfile(profile))
                    profiles++;
            }
            return profiles;
        }

        public void RestartSystem()
        {
            tuningLibrary.Restart();
        }

        private bool validProfile(List<ClientTuningProposal> profile)
        {
            return profile.Any(ctrl => ctrl.Value != 0);
        }

        public decimal GetDefaultValue(uint controlID)
        {
            return tuningLibrary.GetControl(controlID)?.DefaultValue ?? -1;
        }

        private bool loadMemoryProfileValues(out List<ProfileSetting> profileSettings)
        {
           profileSettings = new List<ProfileSetting>();

           profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.MemoryActualFrequency,
                Value = GetDefaultValue((uint)XTUID.MemoryActualFrequency)
            });

            profileSettings.Add(new ProfileSetting
            {
                Id = (uint)XTUID.MemoryClockMultiplier,
                Value = GetDefaultValue((uint)XTUID.MemoryClockMultiplier)
            });
            return true;
        }

        public bool GetHWControl(uint controlID, out IControlData controlData)
        {
          var control = tuningLibrary.GetControl(controlID);
          controlData = new ControlData
            {
                ActiveValue = control.ActiveValue,
                BootValue = control.BootValue,
                RequiresReboot = control.RequiresReboot
            };
            return true;
        }

        public bool IsMemoryOCSupported()
        {
            return tuningLibrary.IsControlTunable((uint)XTUID.MemoryActualFrequency) &&
                   tuningLibrary.IsControlTunable((uint)XTUID.MemoryActualVoltage);
            // return tuningLibrary.IsMemoryTunable();
        }

        public bool SetCurrentAuto()
        {
            var list = tuningLibrary.GetControl(102).SupportedValues;
            bool reboot;
            return tuningLibrary.Tune(102, list[0], out reboot);
        }

        public bool IsWatchdogPresent()
        {
            return serviceInfoLibrary.WatchdogTimer.IsWatchdogTimerPresent();
        }

        public bool IsWatchdogRunning()
        {
            return serviceInfoLibrary.WatchdogTimer.IsWatchdogTimerRunning();
        }

        public bool HasWatchdogFailed()
        {
            return serviceInfoLibrary.WatchdogTimer.HasWatchdogTimerFailed();
        }

        public bool ControlRequiresReboot(uint id)
        {
            return tuningLibrary.GetControl(id).RequiresReboot;
        }

        public decimal ControlBootValue(uint id)
        {
            return tuningLibrary.GetControl(id).BootValue;
        }
    }
}

