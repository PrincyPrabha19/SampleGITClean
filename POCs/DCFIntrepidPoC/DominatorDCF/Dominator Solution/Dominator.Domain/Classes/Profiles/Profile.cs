using System.Collections.Generic;
using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Profiles
{
    public class Profile : IProfile
    {
        public string Name { get; set; }
        public string Path { get; set; }
        public bool IsPredefinedProfile { get; set; }
        public IDataHeader DataHeader { get; set; }
        public List<IDataComponent> DataComponents { get; set; }
        public IProfileReader Reader { private get; set; }
        public IProfileWriter Writer { private get; set; }
        public IProfileProcessor Processor { get; set; }
        public bool IsReadOnly { get; }

        public bool Load(out bool loadProfileValuesFailed)
        {
            loadProfileValuesFailed = false;
            if (Reader == null || !Reader.Load(Path, out loadProfileValuesFailed)) return false;
            DataHeader = Reader.DataHeader;
            DataComponents = Reader.DataComponents;
            return true;
        }

        public bool Apply(bool newProfile, out RebootMask rebootRequired, bool forceRestart = false)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            var result = Processor?.Apply(DataComponents, forceRestart, newProfile, out rebootRequired) ?? false;
            return result;
        }

        public bool Save()
        {
            return Writer?.Save(Path, DataHeader, DataComponents) ?? false;
        }


        //TODO: set a reboot out
        public bool SetCurrentCPUProfile()
        {
            RebootMask rebootRequired;
            return Processor?.ApplyCPUProfile(DataComponents, out rebootRequired) ?? false;
        }

        //TODO: set a reboot out
        public bool SetCurrentMemoryProfile(out RebootMask rebootRequired)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            return Processor?.ApplyMemoryProfile(DataComponents, out rebootRequired) ?? false;
        }

        public bool IsCPUOCEnabled()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.IsOCEnabled ?? false;
        }

        public void SetCPUOCStatus(bool enabled)
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (dataComponent == null) return;
            dataComponent.IsOCEnabled = enabled;
        }

        public bool IsMemoryOCEnabled()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            return dataComponent?.IsOCEnabled ?? false;
        }

        public void SetMemoryOCStatus(bool enabled)
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (dataComponent == null) return;
            dataComponent.IsOCEnabled = enabled;
        }

        public decimal GetCPUFrequency()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.Frequency ?? 0;
        }

        public decimal GetCPUMultiplier()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.Multiplier ?? 0;
        }

        public decimal GetCPUVoltage()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.Voltage ?? 0;
        }

        public decimal GetCPUVoltageOffset()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.VoltageOffset ?? 0;
            //return 0;
        }

        public decimal GetCPUVoltageMode()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            return dataComponent?.VoltageMode ?? 0;
        }

        public int GetXMPProfileID()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            return dataComponent?.ProfileID ?? 0;
        }

        public decimal GetMemoryMultiplier()
        {
            var dataComponent = DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            return dataComponent?.Multiplier ?? 0;
        }

        public string GetThermalMode()
        {
            return null;
        }
    }
}
