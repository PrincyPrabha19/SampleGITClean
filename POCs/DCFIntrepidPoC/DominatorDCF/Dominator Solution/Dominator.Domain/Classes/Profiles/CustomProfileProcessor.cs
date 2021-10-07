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
    public class CustomProfileProcessor : IProfileProcessor
    {
        public ITuningManager TuningManager { get; set; }
        private readonly ISettingIDGenerator idGenerator;
        private static readonly ISystemInfo systemInfo = SystemInfoRepository.Instance;
        private bool isNewProfile;
        private readonly List<ControlValue> cpuSettings;
        private readonly List<ControlValue> memorySettings;  
        public CustomProfileProcessor()
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
            RebootMask memoryRebootRequired=RebootMask.NoRebootRequired;
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
#if NON_XTUPROFILE
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMaxEnable, 0), 1);
#endif
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostPowerMax, 0), cpuComponent.Power);
            addCPUSettings(idGenerator.GetID(HWComponentType.CPU, SettingType.TurboBoostShortPowerMax, 0), cpuComponent.Power);

            bool iccRebootRequired;
            bool iccMaxRebootRequired;
            TuningManager.TuneControl(idGenerator.GetID(HWComponentType.CPU, SettingType.IccMaxCurrent, 0), cpuComponent.ICCMax, out iccRebootRequired);
            TuningManager.TuneControl(idGenerator.GetID(HWComponentType.CPU, SettingType.CacheIccMaxCurrent, 0), cpuComponent.CacheICCMax, out iccMaxRebootRequired);
            bool restartRequired = false;
            var result = TuningManager?.TuneCPUControls(cpuSettings, out restartRequired) ?? false;
            bool rebootRequired = iccRebootRequired || iccMaxRebootRequired || restartRequired;
            if(result && rebootRequired)
                cpuRebootRequired |= RebootMask.CPUOCRebootRequired;
            return result;
        }

        private bool applyMemoryProfile(MemoryDataComponent memoryComponent, out RebootMask memoryRebootRequired)
        {
            memorySettings.Clear();

            bool rebootRequired = false;
            bool result;
            memoryRebootRequired = RebootMask.NoRebootRequired;
            if (!memoryComponent.IsOCEnabled)
            {
                result = TuningManager?.ApplyDefaultMemoryValues(memoryComponent, out rebootRequired) ?? false;
                if (result && rebootRequired)
                    memoryRebootRequired |= RebootMask.MemoryOCRebootRequired;
                return result;
            }
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
    }
}