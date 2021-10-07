using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Helpers;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class XTULibraryLoader
    {
        #region Properties
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;
        private const string XTU_ASSEMBLY_NAME = "IntelOverclockingSDK";
        private const string PROFILES_ASSEMBLY_NAME = "ProfileHelperModel";
        #endregion

        #region Methods
        public static void TryLoadAssemblies()
        {
            try
            {
                checkForDll(PROFILES_ASSEMBLY_NAME);
                checkForDll(XTU_ASSEMBLY_NAME);                
            }
            catch (Exception e)
            {
                logger?.WriteError("Loading XTU DLLs", null, e.ToString());
            }
        }

        private static void checkForDll(string libraryName)
        {
            var asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith(libraryName));
            if (asm == null)
                loadDllInTheDomain(libraryName);
        }

        private static void loadDllInTheDomain(string libraryName)
        {
            var location = PathProvider.InstallPath;
            if (location == null) return;

            var asmName = Path.Combine(location, $"{libraryName}.dll");
            Debug.WriteLine("InstallPATH:" + asmName);
            if (!File.Exists(asmName)) return;

            var asm = Assembly.LoadFrom(asmName);
            AppDomain.CurrentDomain.Load(asm.GetName());
        }
        #endregion
    }
}