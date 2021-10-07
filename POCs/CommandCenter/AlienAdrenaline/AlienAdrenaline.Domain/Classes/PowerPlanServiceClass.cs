
using AlienAdrenaline.SupportService;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using DynamicPluginSupport.Classes;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class PowerPlanServiceClass : PowerPlanService
    {
        #region Static Properties
        private const string fullyQualifiedPowerPlanSupportReaderServiceName = "AlienLabs.AlienFusion.SupportService.Classes.PowerPlanSupportReaderServiceClass";
        private const string fullyQualifiedPowerPlanSupportProcessorServiceName = "AlienLabs.AlienFusion.SupportService.Classes.PowerPlanSupportProcessorServiceClass";
        private static readonly string supportServiceDllPath;
        public static bool IsPowerPlanServicePresent { get; set; }
        #endregion

        #region Private Properties
        private PowerPlanSupportReaderService powerPlanSupportReaderService;
        private PowerPlanSupportReaderService PowerPlanSupportReaderService
        {
            get
            {
                try
                {
                    return powerPlanSupportReaderService ??
                           (powerPlanSupportReaderService = SupportClassesHelper<PowerPlanSupportReaderService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedPowerPlanSupportReaderServiceName));
                }
                catch { }

                return null;
            }
        }

        private PowerPlanSupportProcessorService powerPlanSupportProcessorService;
        private PowerPlanSupportProcessorService PowerPlanSupportProcessorService
        {
            get
            {
                try
                {
                    return powerPlanSupportProcessorService ??
                        (powerPlanSupportProcessorService = SupportClassesHelper<PowerPlanSupportProcessorService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedPowerPlanSupportProcessorServiceName));
                }
                catch { }

                return null;
            }
        }
        #endregion

        #region PowerPlanService Members
        public ObservableCollection<PowerPlan> GetAllPowerPlans()
        {
            var powerPlans = new ObservableCollection<PowerPlan>();

            if (PowerPlanSupportReaderService != null)
                foreach (var powerPlan in powerPlanSupportReaderService.GetAllPowerPlans())
                {
                    powerPlans.Add(
                        new PowerPlanClass()
                        {
                            Id = powerPlan.Id,
                            Name = powerPlan.Name,
                            TempName = powerPlan.Name,
                            Description = powerPlan.Description
                        });
                }

            return powerPlans;
        }

        public Guid GetActivePowerPlan()
        {
            if (PowerPlanSupportReaderService != null)
                return powerPlanSupportReaderService.GetActivePowerPlan();
            return Guid.Empty;
        }

        public bool ExistsPowerPlan(Guid id)
        {
            if (PowerPlanSupportReaderService != null)
                return powerPlanSupportReaderService.GetAllPowerPlans().Exists(p => p.Id == id);
            return false;
        }

        public void SetPowerPlan(Guid id)
        {
            if (PowerPlanSupportProcessorService != null)
                powerPlanSupportProcessorService.SetActivePowerPlan(id);
        }
        #endregion

        #region Constructors
        public PowerPlanServiceClass()
        {
        }

        static PowerPlanServiceClass()
        {
            var dynamicPluginRegistryReader = new DynamicPluginRegistryReaderClass();
            var plugin = dynamicPluginRegistryReader.ReadPlugin("AlienFusion");
            if (plugin != null)
            {
                var installPath = Path.GetDirectoryName(plugin.InstallPath);
                if (!String.IsNullOrEmpty(installPath))
                {
                    supportServiceDllPath = Path.Combine(installPath, "AlienFusionSupportService.dll");
                    IsPowerPlanServicePresent = File.Exists(supportServiceDllPath);
                }                    
            }
        }
        #endregion
    }
}
