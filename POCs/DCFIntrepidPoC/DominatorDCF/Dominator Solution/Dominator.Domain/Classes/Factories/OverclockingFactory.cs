using Dominator.Domain.Classes.Models;

namespace Dominator.Domain.Classes.Factories
{
    public static class OverclockingFactory
    {
        private static IOverclockingModel overclockingModel;

        public static IOverclockingModel NewOverclockingModel()
        {
            if (overclockingModel == null)
            {
                //var cpuDataConfigService = new CPUDataConfigService {
                //    XTUService = ServiceRepository.XTUServiceInstance
                //};
                //cpuDataConfigService.Initialize();

                overclockingModel = new OverclockingModel
                {
                    PlatformComponentManager = PlatformComponentFactory.CreatePlatformComponentManager(),
                    ProfileManager = ProfileFactory.NewProfileManager(),
                    TuningManager = TuningFactory.NewTuningManager(),
                    MonitorManager = MonitorFactory.NewMonitorManager(),
                    OCStatusManager = NewOCStatusManager(),
                    BIOSSupportProvider = BIOSSupportProviderFactory.NewBIOSSupportProvider(),
                    CPUDataConfigService = CPUDataConfigServiceFactory.NewCPUDataConfigService()
                };
                overclockingModel.Initialize();
            }

            return overclockingModel;
        }

        private static IOCStatusManager ocStatusManager;

        public static IOCStatusManager NewOCStatusManager()
        {
            if (ocStatusManager == null)
            {
                ocStatusManager = new OCStatusManager() { CryptoManager = EncryptionFactory.NewCryptoManager() };
                ocStatusManager.Initialize();
            }
                
            return ocStatusManager;
        }

        //private static IBIOSSupportProvider biosProvider;
        //public static IBIOSSupportProvider NewBIOSSupportProvider()
        //{
        //    if (biosProvider == null)
        //    {
        //        biosProvider = new BIOSSupportProviderTest();
        //        biosProvider.Initialize();
        //    }

        //    return biosProvider;
        //}
    }
}
