using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class Setting : ISetting
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public SettingType Type { get; set; }
        public decimal Value { get; set; }
        public SettingMask Mask { get; set; }
        public IAdvancedSetting<decimal> AdvanceSetting { get; set; }

    }
}