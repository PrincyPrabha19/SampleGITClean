using Dominator.Domain.Classes.Helpers;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.Domain.Classes.Factories
{
    public static class CPUDataConfigServiceFactory
    {
        private static ICPUDataConfigService cpuDataConfigService;
        public static ICPUDataConfigService NewCPUDataConfigService()
        {
            if (cpuDataConfigService == null)
            {
                cpuDataConfigService = new CPUDataConfigService() {XTUService = ServiceRepository.XTUServiceInstance};
                cpuDataConfigService.Initialize();
            }

            return cpuDataConfigService;
        }
    }
}
