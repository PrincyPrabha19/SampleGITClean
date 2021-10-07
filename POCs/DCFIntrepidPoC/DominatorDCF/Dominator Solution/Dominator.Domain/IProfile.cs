
using System.Collections.Generic;
using Dominator.Domain.Enums;

namespace Dominator.Domain
{
    public interface IProfile
    {
        string Name { get; set; }
        string Path { get; set; }
        bool IsPredefinedProfile { get; set; }
        IDataHeader DataHeader { get; set; }
        List<IDataComponent> DataComponents { get; set; }
        IProfileReader Reader { set; }
        IProfileWriter Writer { set; }
        IProfileProcessor Processor { get; set; }
        bool IsReadOnly { get; }
        bool Load(out bool loadProfileValuesFailed);
        bool Apply(bool newProfile, out RebootMask rebootRequired, bool forceRestart = false);
        bool IsCPUOCEnabled();
        void SetCPUOCStatus(bool enabled);
        bool IsMemoryOCEnabled();
        void SetMemoryOCStatus(bool enabled);
        decimal GetCPUFrequency();
        decimal GetCPUVoltage();
        decimal GetCPUVoltageOffset();
        decimal GetCPUVoltageMode();
        int GetXMPProfileID();
        decimal GetMemoryMultiplier();
        bool Save();
        bool SetCurrentCPUProfile();
        bool SetCurrentMemoryProfile(out RebootMask rebootRequired);
        string GetThermalMode();
        decimal GetCPUMultiplier();
    }
}
