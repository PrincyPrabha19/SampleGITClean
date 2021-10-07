using Dominator.ServiceModel.Enums;

namespace Dominator.ServiceModel
{
    public interface ISettingIDGenerator
    {
        uint GetID(HWComponentType hwComponentType, SettingType settingType, byte index);
    }
}