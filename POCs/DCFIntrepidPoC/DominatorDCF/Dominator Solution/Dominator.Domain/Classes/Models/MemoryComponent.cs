using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class MemoryComponent : HWComponent
    {
        public override HWComponentType Type => HWComponentType.Memory;

        public override bool RefreshSettings()
        {
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.XMP == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.ActualFrequency == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.BClockFrequency == ctrl.Type));
            RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.ActualVoltage == ctrl.Type));
            return true;
        }

        public override void Initialize(ISystemInfo systemInfo)
        {
            CPUDataService.Initialize();
            createSettingList();
        }

        private void createSettingList()
        {
            var settingInfo = GetAdvancedSettingInfo(SettingType.XMP, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0);
            addSetting(generateSetting(SettingType.XMP, SettingMask.Read | SettingMask.Save | SettingMask.Write, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0);
            addSetting(generateSetting(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.ActualVoltage, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0);
            addSetting(generateSetting(SettingType.ActualVoltage, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            settingInfo = GetAdvancedSettingInfo(SettingType.BClockFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0);
            addSetting(generateSetting(SettingType.BClockFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Reboot, 0, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            addSetting(generateSetting(SettingType.EffectiveFrequency, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.EffectiveVoltage, SettingMask.Read, 0));
            addSetting(generateSetting(SettingType.Utilization, SettingMask.Read, 0));
        }
    }
}