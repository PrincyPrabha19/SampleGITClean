using System.Collections.Generic;
using Dominator.Domain.Enums;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public abstract class HWComponent : IHWComponent
    {
        public IDataService CPUDataService { get; set; }
        public uint Id { get; set; }
        public string Name { get; set; }
        public abstract HWComponentType Type { get; }
        public List<IHWComponent> HWComponentList { get; }
        public List<ISetting> SettingList { get; }
        public ISettingIDGenerator IDGenerator { get; }
        protected HWComponent()
        {
            SettingList = new List<ISetting>();
            HWComponentList = new List<IHWComponent>();
            IDGenerator = new SettingIDGenerator();        
        }

        protected void addSetting(ISetting setting)
        {
            if (setting == null) return;
            SettingList?.Add(setting);
        }

        protected void addHWComponent(IHWComponent hwComponent)
        {
            if (hwComponent == null) return;
            HWComponentList?.Add(hwComponent);
        }

        protected ISetting generateSetting(SettingType settingType, SettingMask settingMask, byte index)
        {
            return new Setting
            {
                Id = IDGenerator.GetID(Type, settingType, index),
                Type = settingType,
                Mask = settingMask
           };
        }

        protected ISetting generateSetting(SettingType settingType, SettingMask settingMask, byte index, decimal maxValue, decimal minValue, int decimals)
        {
            var setting = generateSetting(settingType, settingMask, index);
            setting.Value = GetSettingValue(setting.Id, settingMask);
            setting.AdvanceSetting = new AdvancedSetting<decimal> {MaxValue = maxValue, MinValue = minValue, NumOfDecimals = decimals};
            return setting;
        }
        protected ControlInfo GetAdvancedSettingInfo(SettingType settingType, SettingMask settingMask, byte index)
        {
            return CPUDataService.ReadSettingInfo(IDGenerator.GetID(Type, settingType, index));
        }

        protected decimal GetSettingValue(uint settingID, SettingMask settingMask)
        {
            return CPUDataService.GetControlValue(settingID, settingMask);
        }

        protected bool RefreshSettingValue(ISetting setting)
        {
            if (setting == null) return false;
            setting.Value = GetSettingValue(setting.Id, setting.Mask);
            return true;
        }
        public abstract bool RefreshSettings();
        public abstract void Initialize(ISystemInfo systemInfo);
    }
}
