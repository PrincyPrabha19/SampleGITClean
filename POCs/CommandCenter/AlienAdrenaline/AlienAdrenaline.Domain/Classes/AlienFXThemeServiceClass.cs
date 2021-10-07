
using System;
using System.Collections.ObjectModel;
using System.IO;
using AlienAdrenaline.SupportService;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.CommandCenter.Tools.Classes;
using DynamicPluginSupport.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class AlienFXThemeServiceClass : AlienFXThemeService
    {
        #region Static Properties
        private const string fullyQualifiedAlienFXSupportReaderServiceClass = "AlienLabs.AlienFX.SupportService.Classes.AlienFXSupportReaderServiceClass";
        private const string fullyQualifiedAlienFXSupportProcessorServiceClass = "AlienLabs.AlienFX.SupportService.Classes.AlienFXSupportProcessorServiceClass";
        private static readonly string supportServiceDllPath;
        public static bool IsAlienFXThemeServicePresent { get; set; }
        #endregion

        #region Private Properties
        private AlienFXSupportReaderService alienFXSupportReaderService;
        private AlienFXSupportReaderService AlienFXSupportReaderService
        {
            get
            {
                try
                {
                    return alienFXSupportReaderService ??
                           (alienFXSupportReaderService = SupportClassesHelper<AlienFXSupportReaderService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedAlienFXSupportReaderServiceClass));
                }
                catch { }

                return null;
            }
        }

        private AlienFXSupportProcessorService alienFXSupportProcessorService;
        private AlienFXSupportProcessorService AlienFXSupportProcessorService
        {
            get
            {
                try
                {
                    return alienFXSupportProcessorService ??
                        (alienFXSupportProcessorService = SupportClassesHelper<AlienFXSupportProcessorService>.GetSupportServiceInstance(supportServiceDllPath, fullyQualifiedAlienFXSupportProcessorServiceClass));
                }
                catch { }

                return null;
            }
        }
        #endregion

        #region AlienFXThemeService Members
        public ObservableCollection<AlienFXTheme> GetAllThemes()
        {
            var themes = new ObservableCollection<AlienFXTheme>();

            if (AlienFXSupportReaderService != null)
                 foreach (var theme in alienFXSupportReaderService.GetAllThemes())
                {
                    themes.Add(
                        new AlienFXThemeClass()
                        {
                            Name = theme.Name,
                            TempName = theme.Name,
                            Path = theme.Path
                        });
                }

            return themes;
        }

        public string GetActiveTheme()
        {
            if (AlienFXSupportReaderService != null)
                return alienFXSupportReaderService.GetActiveTheme();
            return String.Empty;
        }

        public bool IsGoDark()
        {
            if (AlienFXSupportReaderService != null)
                return alienFXSupportReaderService.IsGoDark();
            return false;
        }

        public bool IsAlienFXAPIEnabled()
        {
            if (AlienFXSupportReaderService != null)
                return alienFXSupportReaderService.IsAlienFXAPIEnabled();
            return false;
        }

        public bool ExistsAlienFXTheme(string themePath)
        {
            if (AlienFXSupportReaderService != null)
                return alienFXSupportReaderService.GetAllThemes().Exists(t => String.Compare(t.Path, themePath, true) == 0);
            return false;
        }

        public void SetActiveTheme(string path)
        {
            if (AlienFXSupportProcessorService != null)
 	            alienFXSupportProcessorService.SetActiveTheme(path);
        }

        public void GoDark()
        {
            if (AlienFXSupportProcessorService != null)
                alienFXSupportProcessorService.GoDark();
        }

        public void GoLight()
        {
            if (AlienFXSupportProcessorService != null)
                alienFXSupportProcessorService.GoLight();
        }

        public void EnableAlienFXAPI()
        {
            if (AlienFXSupportProcessorService != null)
                alienFXSupportProcessorService.EnableAlienFXAPI();
        }

        public void DisableAlienFXAPI()
        {
            if (AlienFXSupportProcessorService != null)
                alienFXSupportProcessorService.DisableAlienFXAPI();
        }
        #endregion

        #region Constructors
        public AlienFXThemeServiceClass()
        {
        }

        static AlienFXThemeServiceClass()
        {
            var dynamicPluginRegistryReader = new DynamicPluginRegistryReaderClass();
            var plugin = dynamicPluginRegistryReader.ReadPlugin("AlienFX");
            if (plugin != null)
            {
                var installPath = Path.GetDirectoryName(plugin.InstallPath);
                if (!String.IsNullOrEmpty(installPath))
                {
                    supportServiceDllPath = Path.Combine(installPath, "AlienwareAlienFXSupportService.dll");
                    IsAlienFXThemeServicePresent = File.Exists(supportServiceDllPath);
                }                    
            }
        }
        #endregion    
    }
}
