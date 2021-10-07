using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes
{
    public class TuningManager : ITuningManager
    {
        public ITuning CPUTuning {private get; set; }
        public ITuning MemoryTuning { private get; set; }
        public IPlatformComponentManager PlatformManager { get; set; }

        public bool TuneCPUControls(List<ControlValue> cpuSettings, out bool isRestartRequired)
        {
            isRestartRequired = false;
            var result = CPUTuning?.TuneControls(cpuSettings, out isRestartRequired) ?? false;
            return result;
        }

        public bool TuneMemoryControls(List<ControlValue> memorySettings, out bool isRestartRequired)
        {
            isRestartRequired = false;
            var result = MemoryTuning?.TuneControls(memorySettings, out isRestartRequired) ?? false;
            return result;
        }

        public bool TuneControl(uint controlID, decimal controlValue, out bool rebootNeeded)
        {
            rebootNeeded = false;
            if (getHardwareType(controlID) == HWComponentType.CPU)
            {
                var cpuResult = CPUTuning?.TuneControl(controlID, controlValue, out rebootNeeded) ?? false;
                return cpuResult;
            }
            var memResult = MemoryTuning?.TuneControl(controlID, controlValue, out rebootNeeded) ?? false;
            return memResult;
        }

        public void RestartSystem()
        {
            CPUTuning.RestartSystem();
        }

        public decimal GetDefaultValue(uint controlID)
        {
            if (getHardwareType(controlID) == HWComponentType.CPU)
                return CPUTuning?.GetDefaultValue(controlID) ?? -1;
            return MemoryTuning?.GetDefaultValue(controlID) ?? -1;
        }

        public IControlData GetHWControl(uint controlID)
        {
            if (getHardwareType(controlID) == HWComponentType.CPU)
                return CPUTuning?.GetHWControl(controlID);
            return MemoryTuning?.GetHWControl(controlID);
        }

        public bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired)
        {
            isRestartRequired =false;
            List<ControlValue> memoryTuningProposalResult = new List<ControlValue>();
            proposalResult = new List<ControlValue>();
            var result =(CPUTuning?.ProposeControls(out proposalResult, out isRestartRequired) ?? false) && (MemoryTuning?.ProposeControls(out memoryTuningProposalResult, out isRestartRequired) ?? false);
            proposalResult.AddRange(memoryTuningProposalResult);
            return result;
        }
        
        public bool ApplySystemDefaultValues(out bool rebootRequired)
        {
            //TODO: split the cpu and memory reboot
            rebootRequired = false;
            bool cpuRebootRequired = false;
            if (!(CPUTuning?.ApplyDefaultProfile(out cpuRebootRequired) ?? false)) return false; 
            bool memoryRebootRequired=false;
            if (!(MemoryTuning?.ApplyDefaultProfile( out memoryRebootRequired) ?? false)) return false;
            if (!ApplyThermalDefaultValues()) return false;
            rebootRequired = cpuRebootRequired || memoryRebootRequired;
            return true;
        }

        public bool ApplyCPUDefaultValues(IMemoryDataComponent memoryComponent)
        {
            bool rebootRequired;
            return CPUTuning?.ApplyDefaultValues(memoryComponent, out rebootRequired) ?? false;
        }

        public bool ApplyDefaultMemoryValues(MemoryDataComponent memoryComponent, out bool rebootRequired)
        {
           rebootRequired = false;
           return MemoryTuning?.ApplyDefaultValues(memoryComponent, out rebootRequired) ?? false;
        }

        public bool ApplyThermalDefaultValues()
        {
            return true;
        }

        private HWComponentType getHardwareType(uint settingID)
        {
            return (HWComponentType)((settingID >> 24) & 255);
        }
    }
}
