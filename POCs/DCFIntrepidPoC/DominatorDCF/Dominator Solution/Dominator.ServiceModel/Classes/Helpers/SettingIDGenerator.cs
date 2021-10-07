using Dominator.ServiceModel.Enums;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public class SettingIDGenerator : ISettingIDGenerator
    {
        public uint GetID(HWComponentType hwComponentType, SettingType settingType, byte index)
        {
            return (uint)hwComponentType << 24 | (uint)settingType << 16 | (uint)index << 8;
        }
    }
}