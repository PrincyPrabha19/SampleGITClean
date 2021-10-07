using Dominator.Domain.Enums;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain
{
    public interface ISetting
    {
        uint Id { get; set; }
        string Name { get; set; }
        SettingType Type { get; set; }
        decimal Value { get; set; }
        SettingMask Mask { get; set; }
        IAdvancedSetting<decimal> AdvanceSetting { get; set; }
    }
}