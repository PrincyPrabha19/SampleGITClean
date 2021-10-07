using System.Collections.Generic;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain
{
    public interface IPlatformComponentManager
    {
        IHWComponent CPUComponent { get; }
        IHWComponent MemoryComponent { get; }
        void Initialize();
        ISetting[] GetSettingArray(List<SettingType> settingTypes);
        ISetting GetSetting(HWComponentType hwComponentType, SettingType settingType);
        ISetting GetSetting(SettingType settingType, uint coreIndex);
        bool RefreshSettings();
    }
}
