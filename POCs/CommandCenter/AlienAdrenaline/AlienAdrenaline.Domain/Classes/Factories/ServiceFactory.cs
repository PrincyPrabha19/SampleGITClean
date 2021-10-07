
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.Factories;
using AlienLabs.GameDiscoveryHelper.Providers.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Factories
{
    public class ServiceFactory
    {
        private static GameModeService gameModeService;
        public static GameModeService GameModeService
        {
            get
            {
                return gameModeService ??
                       (gameModeService = new GameModeServiceClass()
                        {
                            ProfileService = ProfileService,
                            GameServiceProvider = GameObjectFactory.NewGameServiceProvider()
                        });
            }
        }

        public static WebBrowserService NewWebBrowserService()
        {
            return new WebBrowserServiceClass();
        }

        private static ProfileService profileService;
        public static ProfileService ProfileService
        {
            get
            {
                if (profileService == null)
                {
                    profileService = new ProfileServiceClass();
                    profileService.Initialize();
                }
                return profileService;
            }
        }

        private static GameModeActionSequenceService gameModeActionSequenceService;
        public static GameModeActionSequenceService GameModeActionSequenceService
        {
            get
            {
                return gameModeActionSequenceService ??
                       (gameModeActionSequenceService = new GameModeActionSequenceServiceClass()
                        {
                            ProfileService = ProfileService,
                            GameModeProfileActionImageRepository = RepositoryFactory.NewGameModeProfileActionImageRepository()
                        });
            }
        }

        private static PowerPlanService powerPlanService;
        public static PowerPlanService PowerPlanService
        {
            get
            {
                return powerPlanService ??
                       (powerPlanService = new PowerPlanServiceClass()
                       {
                       });
            }
        }

        private static ThermalProfileService thermalProfileService;
        public static ThermalProfileService ThermalProfileService
        {
            get
            {
                return thermalProfileService ??
                       (thermalProfileService = new ThermalProfileServiceClass()
                       {
                       });
            }
        }

        public static bool IsThermalProfileServiceInitialized
        {
            get { return thermalProfileService != null; }
        }

        private static AlienFXThemeService alienFXThemeService;
        public static AlienFXThemeService AlienFXThemeService
        {
            get
            {
                return alienFXThemeService ??
                       (alienFXThemeService = new AlienFXThemeServiceClass()
                       {
                       });
            }
        }

        private static GameModeActionSummaryService gameModeActionSummaryService;
        public static GameModeActionSummaryService GameModeActionSummaryService
        {
            get
            {
                return gameModeActionSummaryService ??
                       (gameModeActionSummaryService = new GameModeActionSummaryServiceClass()
                       {
                           GameModeActionSequenceService = GameModeActionSequenceService,
                           GameModeProfileActionImageRepository = RepositoryFactory.NewGameModeProfileActionImageRepository(),
                           ProfileActionFormatter = new ProfileActionFormatterClass()
                       });
            }
        }

        private static GameModeCreateService gameModeCreateService;
        public static GameModeCreateService GameModeCreateService
        {
            get
            {
                return gameModeCreateService ??
                       (gameModeCreateService = new GameModeCreateServiceClass()
                       {
                            ProfileService = ProfileService,
                            GameServiceProvider = GameObjectFactory.NewGameServiceProvider()
                       });
            }
        }

        private static GameModeProcessorService gameModeProcessorService;
        public static GameModeProcessorService GameModeProcessorService
        {
            get
            {
                return gameModeProcessorService ??
                       (gameModeProcessorService = new GameModeProcessorServiceClass()
                       {                           
                           ProfileRepository = ProfileRepositoryFactory.ProfileRepository
                       });
            }
        }

        private static EnergyBoosterService energyBoosterService;
        public static EnergyBoosterService EnergyBoosterService
        {
            get
            {
                return energyBoosterService ??
                       (energyBoosterService = new EnergyBoosterServiceClass()
                       {
                           //EnergyBooster = new EnergyBooster()
                       });
            }
        }

        private static PerformanceMonitoringService performanceMonitoringService;
        public static PerformanceMonitoringService PerformanceMonitoringService
        {
            get
            {
                return performanceMonitoringService ??
                       (performanceMonitoringService = new PerformanceMonitoringServiceClass()
                       {
                       });
            }
        }
    }
}