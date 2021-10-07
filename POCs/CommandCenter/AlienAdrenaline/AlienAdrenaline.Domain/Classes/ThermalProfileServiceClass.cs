
using System;
using System.Collections.ObjectModel;
using System.IO;
using AlienAdrenaline.SupportService;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.CommandCenter.Tools.Classes;
using DynamicPluginSupport.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class ThermalProfileServiceClass : ThermalProfileService
    {
        #region Static Properties
        private const string fullyQualifiedThermalControlsSupportReaderServiceClass = "AlienLabs.ThermalControls.SupportService.Classes.ThermalControlsSupportReaderServiceClass";
        private const string fullyQualifiedThermalControlsSupportProcessorServiceClass = "AlienLabs.ThermalControls.SupportService.Classes.ThermalControlsSupportProcessorServiceClass";
        private static readonly string supportServiceDllPath;
        public static bool IsThermalProfileServicePresent { get; set; }
        #endregion

        #region Private Properties
        private ThermalControlsSupportReaderService thermalControlsSupportReaderService;
        private ThermalControlsSupportReaderService ThermalControlsSupportReaderService
        {
            get
            {
                try
                {
                    return thermalControlsSupportReaderService ??
                           (thermalControlsSupportReaderService = SupportClassesHelper<ThermalControlsSupportReaderService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedThermalControlsSupportReaderServiceClass));
                }
                catch { }

                return null;
            }
        }

        private ThermalControlsSupportProcessorService thermalControlsSupportProcessorService;
        private ThermalControlsSupportProcessorService ThermalControlsSupportProcessorService
        {
            get
            {
                try
                {
                    return thermalControlsSupportProcessorService ??
                        (thermalControlsSupportProcessorService = SupportClassesHelper<ThermalControlsSupportProcessorService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedThermalControlsSupportProcessorServiceClass));
                }
                catch { }

                return null;
            }
        }
        #endregion

        #region ThermalProfileService Members
        public bool IsThermalControlsSupportServiceActive { get { return thermalControlsSupportReaderService != null && thermalControlsSupportProcessorService != null; } }

        public ObservableCollection<ThermalProfile> GetAllThermalProfiles()
        {
            var thermalProfiles = new ObservableCollection<ThermalProfile>();

            if (ThermalControlsSupportReaderService != null)
                foreach (var profile in thermalControlsSupportReaderService.GetAllThermalProfiles())
                {
                    Guid guid;
                    if (Guid.TryParse(profile.Name, out guid)) continue;

                    thermalProfiles.Add(
                        new ThermalProfileClass()
                        {
                            Id = profile.Id,
                            Name = profile.Name,
                            TempName = profile.Name
                        });
                }

            return thermalProfiles;
        }

        public Guid GetActiveThermalProfile()
        {
            if (ThermalControlsSupportReaderService != null)
                return thermalControlsSupportReaderService.GetActiveThermalProfile();
            return Guid.Empty;
        }

        public bool ExistsThermalProfile(Guid id)
        {
            if (ThermalControlsSupportReaderService != null)
                return thermalControlsSupportReaderService.GetAllThermalProfiles().Exists(p => p.Id == id);
            return false;
        }

        public void  SetThermalProfile(Guid id)
        {
            if (ThermalControlsSupportProcessorService != null)
 	            thermalControlsSupportProcessorService.SetThermalProfile(id);
        }

        public void Dispose()
        {
            if (ThermalControlsSupportReaderService != null)
                thermalControlsSupportReaderService.Dispose();

            if (ThermalControlsSupportProcessorService != null)
                thermalControlsSupportProcessorService.Dispose();
        }
        #endregion

        #region Constructors
        public ThermalProfileServiceClass()
        {
        }

        static ThermalProfileServiceClass()
        {
            var dynamicPluginRegistryReader = new DynamicPluginRegistryReaderClass();
            var plugin = dynamicPluginRegistryReader.ReadPlugin("ThermalControls");
            if (plugin != null)
            {
                var installPath = Path.GetDirectoryName(plugin.InstallPath);
                if (!String.IsNullOrEmpty(installPath))
                {
                    supportServiceDllPath = Path.Combine(installPath, "ThermalControls.SupportService.dll");
                    IsThermalProfileServicePresent = File.Exists(supportServiceDllPath);
                }                    
            }
        }
        #endregion 
    }
}
