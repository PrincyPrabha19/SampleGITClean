using Dominator.Domain.Classes.Models;

namespace Dominator.Domain.Classes.Factories
{
    public static class AdvancedOCFactory
    {
        private static IAdvancedOCModel advancedOCModel;
   
        public static IAdvancedOCModel NewAdvancedOCModel()
        {
            if (advancedOCModel != null) return advancedOCModel;

            //var cpuDataConfigService = new CPUDataConfigService{XTUService = ServiceRepository.XTUServiceInstance };
            //cpuDataConfigService.Initialize();

            advancedOCModel = new AdvancedOCModel
            {
                CPUDataConfigService = CPUDataConfigServiceFactory.NewCPUDataConfigService(),
                PlatformComponentManager = PlatformComponentFactory.CreatePlatformComponentManager(),
                TuningManager = TuningFactory.NewTuningManager(),
                MonitorManager = MonitorFactory.NewMonitorManager(),
                ProfileNameRepository = ProfileFactory.NewProfileNameRepository(),
                ProfileManager = ProfileFactory.NewProfileManager(),
                OCStatusManager = OverclockingFactory.NewOCStatusManager(),
                ValidationManager = ValidationFactory.NewValidationManager()
            };

            return advancedOCModel;
        }
    }
}
