using System;
using Dominator.Domain.Enums;

namespace Dominator.Domain
{
    public interface IValidationManager
    {
        ICPUValidation CPUValidation { set; }
        IMemoryValidation MemoryValidation {set; }
        IMonitorManager MonitorManager { get; set; }
        bool IsValidationRunning { get; }

        ValidationStatus StartValidation(IProfile currentProfile);
        void StopValidation();
        event Action<int, bool> ProgressChanged;
    }

    public interface IMemoryValidation
    {
        ITuningManager TuningManager { get; set; }
        bool ApplySettings(IProfile profile);
    }

    public interface ICPUValidation
    {
        ITuningManager TuningManager { get; set; }
        IMonitorManager MonitorManager { get; set; }

        bool ApplySettings(IProfile profile);
    }
}
