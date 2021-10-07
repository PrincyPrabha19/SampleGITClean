using System.Collections.Generic;
using System.Linq;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
    public class MonitoringComponentInfo
    {
        public int CardCount { get; set; }
        public List<MonitoringComponentCategoryInfo> ComponentCategoryInfos { get; set; }

        public MonitoringComponentInfo(List<ModifiableKeyValueData> infoResults, MonitoringCategoryInfo categoryInfo)
        {
            int count;
            ComponentCategoryInfos = getCategoryInfo(infoResults, categoryInfo, out count);
            CardCount = count;
        }

        private List<MonitoringComponentCategoryInfo> getCategoryInfo(List<ModifiableKeyValueData> infoResults, MonitoringCategoryInfo categoryInfo, out int count)
        {
            count = infoResults.Count(i => i.CategoryInfo == categoryInfo);
            var list = infoResults.Select( res =>
                    new MonitoringComponentCategoryInfo()
                        {
                            MonitoringCategoryInfo = res.CategoryInfo,
                            Title = res.Key
                        }
                ).ToList();
            return list;

            //count = infoResults.Count(i => i.CategoryInfo == categoryInfo);
            //var list = infoResults.GroupBy(i => i.CategoryInfo).Select(grp =>
            //        new MonitoringComponentCategoryInfo()
            //        {
            //            MonitoringCategoryInfo = grp.First().CategoryInfo,
            //            Title = grp.First().Key
            //        }
            //    ).ToList();
            //return list;
        }
    }

    public class MonitoringComponentCategoryInfo
    {
        public MonitoringCategoryInfo MonitoringCategoryInfo { get; set; }
        public string Title { get; set; }
    }
}
