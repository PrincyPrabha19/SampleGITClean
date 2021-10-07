//#define NON_XTUPROFILE

using System.Collections.Generic;
using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Profiles
{
    public class PredefinedProfileProcessor : IProfileProcessor
    {
#if NON_XTUPROFILE
        public ITuningManager TuningManager { get; set; }
        private readonly ISettingIDGenerator idGenerator;
        private static readonly ISystemInfo systemInfo = SystemInfoRepository.Instance;
        private bool isNewProfile;
        private readonly List<ControlValue> cpuSettings;
        private readonly List<ControlValue> memorySettings;
        public PredefinedProfileProcessor()
        {
            idGenerator = new SettingIDGenerator();
            cpuSettings = new List<ControlValue>();
            memorySettings = new List<ControlValue>();
        }
        public bool Apply(List<IDataComponent> dataComponents, bool forceRestart, bool NewProfile, out RebootMask rebootMask)
        {
            rebootMask = RebootMask.NoRebootRequired;
            bool rebootRequired = false;
            isNewProfile = NewProfile;
            var cpuComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (cpuComponent == null) return false;
            var memoryComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (memoryComponent == null) return false;
            if (!cpuComponent.IsOCEnabled && !memoryComponent.IsOCEnabled)
                return TuningManager?.ApplySystemDefaultValues(out rebootRequired) ?? false;

            RebootMask cpuRebootRequired;
            RebootMask memoryRebootRequired = RebootMask.NoRebootRequired;
            var result = applyCPUProfile(cpuComponent, memoryComponent, out cpuRebootRequired) && applyMemoryProfile(memoryComponent, out memoryRebootRequired);
            rebootMask = cpuRebootRequired | memoryRebootRequired;
            return result;
        }

        public bool ApplyMemoryProfile(List<IDataComponent> dataComponents, out RebootMask rebootRequired)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            var memoryComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (memoryComponent == null) return false;
            return applyMemoryProfile(memoryComponent, out rebootRequired);
        }

        public bool ApplyCPUProfile(List<IDataComponent> dataComponents, out RebootMask rebootRequired)
        {
            rebootRequired = RebootMask.NoRebootRequired;
            var cpuComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (cpuComponent == null) return false;
            var memoryComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (memoryComponent == null) return false;
            return applyCPUProfile(cpuComponent, memoryComponent, out rebootRequired);
        }

        private bool applyCPUProfile(CPUDataComponent cpuComponent, MemoryDataComponent memoryComponent, out RebootMask cpuRebootRequired)
        {
            cpuSettings.Clear();
            cpuRebootRequired = RebootMask.NoRebootRequired;
            if (!cpuComponent.IsOCEnabled) return TuningManager?.ApplyCPUDefaultValues(memoryComponent) ?? false;
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0), cpuComponent.Multiplier);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0), cpuComponent.Voltage);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0), cpuComponent.VoltageMode);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0), cpuComponent.VoltageOffset);

            //TODO: to make profile non xtu
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMaxEnable, 0),1);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostPowerMax, 0), cpuComponent.Power);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMax, 0), cpuComponent.Power);

            bool iccRebootRequired;
            TuningManager.TuneControl(idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0), cpuComponent.ICCMax, out iccRebootRequired);

            bool restartRequired = false;
            var result = TuningManager?.TuneCPUControls(cpuSettings, out restartRequired) ?? false;
            bool rebootRequired = iccRebootRequired || restartRequired;
            if (result && rebootRequired)
                cpuRebootRequired |= RebootMask.CPUOCRebootRequired;
            return result;
        }

        private bool applyMemoryProfile(MemoryDataComponent memoryComponent, out RebootMask memoryRebootRequired)
        {
            memorySettings.Clear();
            memoryRebootRequired = RebootMask.NoRebootRequired;
            if (!memoryComponent.IsOCEnabled) return TuningManager?.ApplyDefaultMemoryValues(memoryComponent) ?? false;

            bool rebootRequired = false;
            bool result;
            if (systemInfo.XMPInfoData.IsXMPSupported)
            {
                result = TuningManager.TuneControl(idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0),
                   memoryComponent.ProfileID, out rebootRequired);
                if (result && rebootRequired)
                    memoryRebootRequired |= RebootMask.MemoryOCRebootRequired;
                return result;
            }
            return true;
        }

        private void addCPUSettings(uint controlID, decimal controlValue)
        {
            var control = cpuSettings.FirstOrDefault(ctrl => controlID == ctrl.Id);
            if (control == null)
            {
                cpuSettings.Add(new ControlValue
                {
                    Id = controlID,
                    Value = controlValue
                });
            }
            else
                control.Value = controlValue;
        }

        private void addMemorySettings(uint controlID, decimal controlValue)
        {
            var control = memorySettings.FirstOrDefault(ctrl => controlID == ctrl.Id);
            if (control == null)
            {
                memorySettings.Add(new ControlValue
                {
                    Id = controlID,
                    Value = controlValue
                });
            }
            else
                control.Value = controlValue;
        }

#else
        public IXTUService XTUService { private get; set; }
        public ITuningManager TuningManager { get; set; }
        private static readonly ISystemInfo systemInfo = SystemInfoRepository.Instance;
        private readonly ISettingIDGenerator idGenerator;
        public PredefinedProfileProcessor()
        {
            idGenerator = new SettingIDGenerator();
        }
        public bool Apply(List<IDataComponent> dataComponents, bool forceRestart, bool isNewProfile, out RebootMask rebootRequired)
        {
            RebootMask cpuRebootRequired;
            RebootMask memoryRebootRequired = RebootMask.NoRebootRequired;
            var result = ApplyCPUProfile(dataComponents, out cpuRebootRequired) && ApplyMemoryProfile(dataComponents, out memoryRebootRequired);
            rebootRequired = cpuRebootRequired | memoryRebootRequired;
            return result;
        }

        public bool ApplyMemoryProfile(List<IDataComponent> dataComponents, out RebootMask memoryRebootRequired)
        {
            memoryRebootRequired = RebootMask.NoRebootRequired;
            if (TuningManager == null || dataComponents == null)
                return true;

            var memoryComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (systemInfo.XMPInfoData.IsXMPSupported && memoryComponent != null)
            {
                bool rebootRequired;
                var result = TuningManager.TuneControl(idGenerator.GetID(HWComponentType.Memory, SettingType.XMP, 0), memoryComponent.ProfileID, out rebootRequired);
                if (result && rebootRequired)
                    memoryRebootRequired = RebootMask.MemoryOCRebootRequired;
                return result;
            }
            return true;
        }

        public bool ApplyCPUProfile(List<IDataComponent> dataComponents, out RebootMask cpuRebootRequired)
        {
            cpuRebootRequired = RebootMask.NoRebootRequired;
            var cpuComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            var memoryComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (string.IsNullOrEmpty(cpuComponent?.XtuPath)) return false;
            var xtuProfileName = System.IO.Path.GetFileNameWithoutExtension(cpuComponent.XtuPath);
            if (string.IsNullOrEmpty(xtuProfileName)) return false;
            bool rebootRequired;
            var result = XTUService.ApplyXTUProfile(xtuProfileName, out rebootRequired, false);
            if (result && rebootRequired)
                cpuRebootRequired = RebootMask.CPUOCRebootRequired;
            return result;
        }
#endif
    }
}