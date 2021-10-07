using System.Collections.Generic;
using System.Linq;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Monitoring;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Models
{
    public class PlatformComponentManager : IPlatformComponentManager
    {
        public IHWComponent CPUComponent { get; private set; }
        public IHWComponent MemoryComponent { get; private set; }

        public void Initialize()
        {
            CPUComponent = new CPUComponent { CPUDataService = new DataService() };
            CPUComponent.Initialize(SystemInfoRepository.Instance);

            MemoryComponent = new MemoryComponent { CPUDataService = new DataService() };
            MemoryComponent.Initialize(SystemInfoRepository.Instance);
        }

        public ISetting[] GetSettingArray(List<SettingType> settingTypes)
        {
            var resultList = getSettingArray(CPUComponent, settingTypes).ToList();
            var memoryList = getSettingArray(MemoryComponent, settingTypes).ToList();
            resultList.AddRange(memoryList);
            return resultList.ToArray();
        }

        private ISetting[] getSettingArray(IHWComponent hwComponent, List<SettingType> settingTypes)
        {
            var results = hwComponent.SettingList.Where(x => settingTypes.Contains(x.Type)).ToList();
            if (hwComponent.HWComponentList == null || hwComponent.HWComponentList.Count == 0)
                return results.ToArray();

            foreach (var component in hwComponent.HWComponentList)
                results.AddRange(getSettingArray(component, settingTypes));

            return results.ToArray();
        }

        public ISetting GetSetting(HWComponentType hwComponentType, SettingType settingType)
        {
            if(hwComponentType == HWComponentType.CPU)
              return CPUComponent.SettingList.FirstOrDefault(ctrl => settingType == ctrl.Type);
            return MemoryComponent.SettingList.FirstOrDefault(ctrl => settingType == ctrl.Type);
        }

        public ISetting GetSetting(SettingType settingType, uint coreIndex)
        {
            var core = CPUComponent?.HWComponentList?.FirstOrDefault(hw => coreIndex == hw.Id);
            return core?.SettingList.FirstOrDefault(ctrl => settingType == ctrl.Type);
        }

        public bool RefreshSettings()
        {
            var a = CPUComponent.RefreshSettings();
            var b = MemoryComponent.RefreshSettings();
        //    if (a && b)
                return true;
           // return false;
        }
    }
}