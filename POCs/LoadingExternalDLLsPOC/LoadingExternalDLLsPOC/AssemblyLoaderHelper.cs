using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Threading.Tasks;
using System.Linq;

namespace LoadingExternalDLLsPOC
{
    public static class AssemblyLoaderHelper
    {
        public static bool CheckTypeIsSubClassOf(Type sub, Type super)
        {
            return super == sub || super.GetTypeInfo().IsAssignableFrom(sub.GetTypeInfo());
        }

        public static async Task<List<Assembly>> GetAssemblyList()
        {
            List<Assembly> assemblies = new List<Assembly>();

            var files = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync();
            if (files == null)
                return assemblies;

            foreach (var file in files.Where(file => file.FileType == ".dll" || file.FileType == ".exe"))
            {
                try
                {
                    assemblies.Add(Assembly.Load(new AssemblyName(file.DisplayName)));
                }
                catch (Exception ex)
                {
                    Debug.Write(ex.Message);
                }

            }

            return assemblies;
        }
    }
}
