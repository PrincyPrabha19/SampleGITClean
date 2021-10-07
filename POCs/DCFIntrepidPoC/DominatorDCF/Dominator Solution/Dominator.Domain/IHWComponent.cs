using System.Collections.Generic;
using Dominator.ServiceModel;

namespace Dominator.Domain
{
    public interface IHWComponent
    {
        IDataService CPUDataService { get; set; }
        uint Id { get; set; }
        string Name { get; set; }
        List<IHWComponent> HWComponentList { get; }
        List<ISetting> SettingList { get; }
        ISettingIDGenerator IDGenerator { get; }
        void Initialize(ISystemInfo systemInfo);
        bool RefreshSettings();
    }
}