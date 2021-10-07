#define TURBOMAX_SUPPORTED

using System;
using System.Collections.Generic;
using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Helpers
{
    public class CPUValidation : ICPUValidation
    {
        public ITuningManager TuningManager { get; set; }
        public IMonitorManager MonitorManager { get; set; }

        private readonly ILogger logger = LoggerFactory.LoggerInstance;
        private readonly ISettingIDGenerator idGenerator;

        public CPUValidation()
        {
            idGenerator = new SettingIDGenerator();
        }

        public bool ApplySettings(IProfile profile)
        {
            List<ControlValue> cpuSettings = new List<ControlValue>();
            var cpuComponent =
                profile.DataComponents.FirstOrDefault(component => component.Type == HWComponentType.CPU) as
                    CPUDataComponent;

            var mem =
                profile.DataComponents.FirstOrDefault(component => component.Type == HWComponentType.Memory) as
                    MemoryDataComponent;

            if (cpuComponent == null) return false;
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0),
                Value = cpuComponent.Multiplier
            });
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0),
                Value = cpuComponent.Voltage
            });
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0),
                Value = cpuComponent.VoltageMode
            });
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0),
                Value = cpuComponent.VoltageOffset
            });

#if !ALIENWARE15
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostPowerMax, 0),
                Value = cpuComponent.Power
            });
            cpuSettings.Add(new ControlValue
            {
                Id = idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMax, 0),
                Value = cpuComponent.Power
            });

             bool reboot;
             TuningManager.TuneControl(idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0), cpuComponent.ICCMax, out reboot);
             TuningManager.TuneControl(idGenerator.GetID(HWComponentType.CPU, SettingType.CacheIccMaxCurrent, 0), cpuComponent.CacheICCMax, out reboot);

#endif
            bool restartRequired;
            return TuningManager?.TuneCPUControls(cpuSettings, out restartRequired) ?? false;
        }
    }

    public class MemoryValidation : IMemoryValidation
    {
        public ITuningManager TuningManager { get; set; }
        private readonly ISettingIDGenerator idGenerator;
        private static readonly ISystemInfo systemInfo = SystemInfoRepository.Instance;
        public MemoryValidation()
        {
            idGenerator = new SettingIDGenerator();
        }
        public bool ApplySettings(IProfile profile)
        {
            if (TuningManager == null || profile?.DataComponents == null) return false;

            List<ControlValue> memorySettings = new List<ControlValue>();
            var memoryComponent = profile.DataComponents.FirstOrDefault(component => component.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (memoryComponent == null) return false;
            // if (systemInfo.MemoryInfoData.IsMemoryOCSupported != null && systemInfo.MemoryInfoData.IsMemoryOCSupported.Value)
            //     memorySettings.Add(new ControlValue { Id = idGenerator.GetID(HWComponentType.Memory, SettingType.ActualFrequency, 0), Value = memoryComponent.Multiplier });
            if (systemInfo.XMPInfoData.IsXMPSupported)
            {
                bool rebootRequired;
                return TuningManager.TuneControl(idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0), memoryComponent.ProfileID, out rebootRequired);
            }
            else
                return true;
        }
    }
}