using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Classes.SystemInfo;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Intel.Overclocking.SDK.Tuning;

namespace Dominator.ServiceModel.Classes
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single/*, AddressFilterMode = AddressFilterMode.Any*/)]
    public class XTUService : IXTUService
    {
        private XTUSDKLibrary xtuSDKLibrary;
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public bool Initialize()
        {
            if (xtuSDKLibrary != null) return true;

            try
            {
                xtuSDKLibrary = new XTUSDKLibrary();
                xtuSDKLibrary.Initialize();
                return true;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to Initialize XTU SDK", null, e.Message);                
            }

            return false;
        }

        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public bool LoadXTUProfile(string path)
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                var result = xtuSDKLibrary.Load(path);
                if (!result)
                    logger?.WriteWarning($"XTUService Failed to load profile: {path}");
                return result;
            }
            catch (Exception e)
            {
                logger?.WriteError($"Unable to Load XTU Profile: {path}", $"Check the profile exists in {path} and is valid for this CPU type", e.Message);
            }

            return false;
        }

        public bool LoadXTUProfileValues(string name, out List<ProfileSetting> profileSettings)
        {
            profileSettings = null;
            if (xtuSDKLibrary == null) return false;
            try
            {
                return xtuSDKLibrary.LoadXTUProfileValues(name, out profileSettings);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public decimal GetProfileSettings(string name, uint controlID)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetProfileSettings(name ,controlID);
            }
            catch (Exception e)
            {
                
            }

            return -1;
        }

        public bool ApplyXTUProfile(string name, out bool rebootRequired, bool forceRestart = false)
        {
            rebootRequired = false;
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.Apply(name, out rebootRequired ,forceRestart);
            }
            catch (Exception e)
            {
                logger?.WriteError($"Unable to Apply XTU Profile: {name}", $"Check the profile {name} is valid for this CPU type", e.Message);
            }

            return false;
        }

        public string GetProcessorBrand()
        {
            if (xtuSDKLibrary == null) return null;

            try
            {
                return xtuSDKLibrary.GetProcessorBrand();
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to GetProcessorBrand", null, e.Message);
            }

            return null;
        }

        public bool ApplyDefaultProfile(out bool rebootRequired)
        {
            rebootRequired = false;
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.ApplyDefaultProfile(out rebootRequired);
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to ApplyDefaultProfile", null, e.Message);
            }

            return false;
        }

        public bool IsSystemOverclocked()
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.IsSystemOverClocked();
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to IsSystemOverClocked", null, e.Message);
            }

            return false;
        }

        public bool IsOverclockingSupported()
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.IsOverclockingSupported();
            }
            catch (Exception e)
            {
            }

            return false;
        }

        public decimal GetValueOfControl(uint controlId)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetValueOfControl(controlId);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public decimal[] GetAllControlValue(uint[] controlIDs)
        {
            if (controlIDs == null || controlIDs.Length < 0)
                return null;

            var res = new decimal[controlIDs.Length];
            for (int i = 0; i < controlIDs.Length; i++)
            {
                try
                {
                    res[i] = xtuSDKLibrary.GetValueOfControl(controlIDs[i]);
                }
                catch (Exception)
                {
                    res[i] = -1;
                }
            }

            return res;
        }

        public bool TuneControl(uint controlID, decimal controlValue, out bool isRestartRequired)
        {
            isRestartRequired = false;
            if (xtuSDKLibrary == null) return false;
            
            try
            {
                return xtuSDKLibrary.TuneControl(controlID, controlValue, out isRestartRequired);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool TuneListOfControls(List<ControlValue> proposals, out bool isRestartRequired)
        {
            isRestartRequired = false;
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.TuneListOfControls(proposals, out isRestartRequired);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public decimal GetControlValue(uint controlID)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetControlValue(controlID);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public bool ProposeChanges(List<ControlValue> proposals, out List<ControlValue> proposalResult, out bool requiresReboot)
        {
            proposalResult = new List<ControlValue>();
            requiresReboot = false;
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.ProposeChanges(proposals, out proposalResult, out requiresReboot);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool ApplyChanges(bool forceRestart)
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.ApplyChanges(forceRestart);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public List<ClientTuningProposalResult> DiscardChanges()
        {
            if (xtuSDKLibrary == null) return null;

            try
            {
                return xtuSDKLibrary.DiscardChanges();
            }
            catch (Exception e)
            {
            }
            return null;
        }

        public decimal GetMaxControlValue(uint controlID)
        {

            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetMaxControlValue(controlID);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public decimal GetMinControlValue(uint controlID)
        {

            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetMinControlValue(controlID);
            }
            catch (Exception e)
            {
            }
            return -1;
        }


        public CPUInfoData GetCPUInfoData()
        {
            CPUInfoData cpuInfoData;

            try
            {
                cpuInfoData = new CPUInfoData
                {
                    ProcessorBrand = xtuSDKLibrary.GetProcessorBrand(),
                    LogicalCpuCores = xtuSDKLibrary.GetLogicalCpuCores(),
                    PhysicalCpuCores = xtuSDKLibrary.GetPhysicalCpuCores(),
                    FeatureFlags = xtuSDKLibrary.GetFeatureFlags(),
                    IsOverclockSupported = xtuSDKLibrary.IsOverclockingSupported(),
                    IsTurboBoostTechnologyEnabled = xtuSDKLibrary.IsTurboOverclockable()
                };
            }
            catch (Exception e)
            {
                cpuInfoData = null;
                logger?.WriteError("Unable to GetCPUInfoData", null, e.Message);
            }

            return cpuInfoData;
        }

        public WatchdogTimerInfo GetWatchdogTimerInfo()
        {
            WatchdogTimerInfo watchdogTimerInfo;

            try
            {
                watchdogTimerInfo = new WatchdogTimerInfo
                {
                    IsWatchdogPresent = xtuSDKLibrary.IsWatchdogPresent(),
                    IsWatchdogRunningAtBoot = xtuSDKLibrary.IsWatchdogRunning(),
                    HasWatchdogFailed = xtuSDKLibrary.HasWatchdogFailed()
                };
            }
            catch (Exception e)
            {
                watchdogTimerInfo = null;
                logger?.WriteError("Unable to GetWatchdogTimerInfo", null, e.Message);
            }

            return watchdogTimerInfo;
        }

        public decimal ControlStepValue(string name, uint controlID)
        {
            throw new NotImplementedException();
        }

        public decimal SettingsStepValue(uint controlID)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.SettingsStepValue(controlID);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public XMPInfoData GetXMPInfoData()
        {
            try
            {
                var xmpInfo = new XMPInfoData
                {
                    IsXMPSupported = xtuSDKLibrary.XmpSupported(),
                    NumberOfXMPProfiles = 0
                };
                if (xmpInfo.IsXMPSupported)
                    xmpInfo.NumberOfXMPProfiles = xtuSDKLibrary.NumberOfXMPProfiles();
                return xmpInfo;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to GetXMPInfoData", null, e.Message);
                return null;
            }
        }

        public bool IsMemoryOCSupported()
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.IsMemoryOCSupported();
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public decimal GetDefaultValue(uint controlID)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.GetDefaultValue(controlID);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public void RestartSystem()
        {
          xtuSDKLibrary.RestartSystem();
        }

        public bool GetHWControl(uint id, out IControlData control)
        {
            control = new ControlData();
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.GetHWControl(id, out control);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public bool ControlRequiresReboot(uint id)
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.ControlRequiresReboot(id);
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public decimal ControlBootValue(uint id)
        {
            if (xtuSDKLibrary == null) return -1;

            try
            {
                return xtuSDKLibrary.ControlBootValue(id);
            }
            catch (Exception e)
            {
            }
            return -1;
        }

        public bool SetCurrentAuto()
        {
            if (xtuSDKLibrary == null) return false;

            try
            {
                return xtuSDKLibrary.SetCurrentAuto();
            }
            catch (Exception e)
            {
            }
            return false;
        }

        public void StartMonitor()
        {
            if (xtuSDKLibrary == null) return;

            try
            {
                 xtuSDKLibrary.StartMonitor();
            }
            catch (Exception e)
            {
            }
        }

        public void StopMonitor()
        {
            if (xtuSDKLibrary == null) return;

            try
            {
                 xtuSDKLibrary.StopMonitor();
            }
            catch (Exception e)
            {
            }
        }
    }
}