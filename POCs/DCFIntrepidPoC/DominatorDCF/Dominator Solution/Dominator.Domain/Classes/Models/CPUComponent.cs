using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class CPUComponent : HWComponent 
    {
        public override HWComponentType Type => HWComponentType.CPU;
        public override void Initialize(ISystemInfo systemInfo)
        {
            Name = systemInfo.ProcessorBrand;
            CPUDataService.Initialize();
            createCoreComponentList(systemInfo);
            createSettingList();
        }

        public override bool RefreshSettings()
        {
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.ActualFrequency == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.ActualVoltage == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.VoltageOffset == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.VoltageMode == ctrl.Type));
            foreach (var component in HWComponentList)
                component.RefreshSettings();
            return true;
        }

        private uint numOfCores;
        private void createCoreComponentList(ISystemInfo systemInfo)
        {
            numOfCores = systemInfo.CPUInfoData.PhysicalCpuCores;
            for (uint i = 0; i < numOfCores; i++)
            {
                var coreComponent = new CoreComponent {Id = i, CPUDataService = CPUDataService}; 
                coreComponent.Initialize(systemInfo);
                addHWComponent(coreComponent);
            }
        }

        private void createSettingList()
        {
            var settingInfo = GetAdvancedSettingInfo(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0);
            addSetting(generateSetting(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.ActualVoltage, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0);
            addSetting(generateSetting(SettingType.ActualVoltage, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.VoltageOffset, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0);
            addSetting(generateSetting(SettingType.VoltageOffset, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.VoltageMode, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0);
            addSetting(generateSetting(SettingType.VoltageMode, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.BClockFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0);
            addSetting(generateSetting(SettingType.BClockFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            addSetting(generateSetting(SettingType.EffectiveFrequency, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.EffectiveVoltage, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.Temperature, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.Utilization, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.FanSpeed, SettingMask.Read, 0));
        }
    }
}