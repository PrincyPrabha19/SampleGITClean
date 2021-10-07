using System.Collections.Generic;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;

namespace Dominator.Domain
{
    public interface ITuningManager
    {
        ITuning CPUTuning { set; }
        ITuning MemoryTuning { set; }
        IPlatformComponentManager PlatformManager { get; set; }
        bool ProposeControls(out List<ControlValue> proposalResult, out bool isRestartRequired);
        bool TuneCPUControls(List<ControlValue> cpuSettings, out bool isRestartRequired);
        bool TuneMemoryControls(List<ControlValue> memorySettings, out bool isRestartRequired);
        bool TuneControl(uint controlID, decimal controlValue, out bool rebootNeeded);
        bool ApplySystemDefaultValues(out bool rebootRequired);
        bool ApplyCPUDefaultValues(IMemoryDataComponent memoryComponent);
        bool ApplyThermalDefaultValues();
        bool ApplyDefaultMemoryValues(MemoryDataComponent memoryComponent, out bool rebootRequired);
        void RestartSystem();
        decimal GetDefaultValue(uint controlID);
        IControlData GetHWControl(uint controlID);
    }
}
