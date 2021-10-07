using System.Collections.Generic;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Enums;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class SettingsIDRepository
    {
        public const uint WINDOWS_BASEID = 4000000000;
        public const uint WINDOWS_MEMORY_BASEID = 4100000000;

        public static Dictionary<uint, uint> ControlIDs { get; private set; }
        private static readonly ISystemInfo systemInfo = SystemInfoRepository.Instance;

        static SettingsIDRepository()
        {
            initialize();
        }

        static void initialize()
        {
            var idGenerator = new SettingIDGenerator();
            ControlIDs = new Dictionary<uint, uint>
            {
                {idGenerator.GetID(HWComponentType.CPU, SettingType.BClockFrequency, 0), (uint)XTUID.BaseClockFrequency},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0), (uint)XTUID.CPUActualFrequency },
                {idGenerator.GetID(HWComponentType.CPU, SettingType.EffectiveVoltage, 0), (uint)XTUID.CPUEffectiveVoltage },
                {idGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0), (uint)XTUID.CPUActualVoltage},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.EffectiveFrequency, 0), (uint)XTUID.CPUEffectiveFrequency},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.Temperature, 0), (uint)XTUID.CPUTemperature},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.Utilization, 0), WINDOWS_BASEID},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.FanSpeed, 0), (uint)XTUID.CPUFanSpeed},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0), (uint)XTUID.CPUVoltageOffset},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0), (uint)XTUID.CPUVoltageMode},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.ThermalThrottling, 0), (uint)XTUID.ThermalThrottling},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.PowerLimitThrottling, 0), (uint)XTUID.PowerLimitThrottling},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.CurrentLimitThrottling, 0), (uint)XTUID.CurrentLimitThrottling},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0), (uint)XTUID.IccMaxCurrent},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.CacheIccMaxCurrent, 0), (uint)XTUID.CacheIccMaxCurrent},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostPowerMax, 0), (uint)XTUID.TurboBoostPowerMax},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMax, 0), (uint)XTUID.TurboBoostShortPowerMax},
                {idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMaxEnable, 0), (uint)XTUID.TurboBoostShortPowerMaxEnable},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.Utilization, 0), WINDOWS_MEMORY_BASEID},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0), (uint)XTUID.ExtremeMemoryProfile},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.ActualFrequency, 0), (uint)XTUID.MemoryActualFrequency},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.EffectiveFrequency, 0), (uint)XTUID.MemoryEffectiveFrequency},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.ActualVoltage, 0), (uint)XTUID.MemoryActualVoltage},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.EffectiveVoltage, 0), (uint)XTUID.MemoryEffectiveVoltage},
                {idGenerator.GetID(HWComponentType.Memory, SettingType.BClockFrequency, 0), (uint)XTUID.MemoryClockMultiplier}
            };

            for (uint coreIndex = 0; coreIndex < systemInfo.CPUInfoData.PhysicalCpuCores; coreIndex++)
            {
                ControlIDs.Add(idGenerator.GetID(HWComponentType.Core, SettingType.ActualFrequency, (byte)coreIndex), (uint)XTUID.CoreActualFrequencyBase + coreIndex);
                ControlIDs.Add(idGenerator.GetID(HWComponentType.Core, SettingType.Temperature, (byte)coreIndex), (uint)XTUID.CoreTempuratureBase + coreIndex);
                ControlIDs.Add(idGenerator.GetID(HWComponentType.Core, SettingType.Utilization, (byte)coreIndex), WINDOWS_BASEID + 1 + coreIndex);
            }
        }
    }
}
