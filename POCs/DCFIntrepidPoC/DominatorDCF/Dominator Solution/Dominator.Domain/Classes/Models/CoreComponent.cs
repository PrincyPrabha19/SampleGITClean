using System.Linq;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class CoreComponent : HWComponent
    {
        public override HWComponentType Type => HWComponentType.Core;

        public override bool RefreshSettings()
        {
            return RefreshSettingValue(SettingList.FirstOrDefault(ctrl => SettingType.ActualFrequency == ctrl.Type));
        }

        public override void Initialize(ISystemInfo systemInfo)
        {
            CPUDataService.Initialize();
            createSettingList();
        }

        private void createSettingList()
        {
            var settingInfo = GetAdvancedSettingInfo(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Write, (byte)Id);
            addSetting(generateSetting(SettingType.ActualFrequency, SettingMask.Read | SettingMask.Save | SettingMask.Write, (byte)Id, settingInfo.MaxValue, settingInfo.MinValue, settingInfo.NoOfDecimals));
            addSetting(generateSetting(SettingType.ActualVoltage, SettingMask.Read | SettingMask.Save | SettingMask.Write, (byte)Id));
            addSetting(generateSetting(SettingType.EffectiveFrequency, SettingMask.Read, (byte)Id));
            addSetting(generateSetting(SettingType.EffectiveVoltage, SettingMask.Read, (byte)Id));
            addSetting(generateSetting(SettingType.Temperature, SettingMask.Read, (byte)Id));
            addSetting(generateSetting(SettingType.Utilization, SettingMask.Read, (byte)Id));
        }
    }
}
