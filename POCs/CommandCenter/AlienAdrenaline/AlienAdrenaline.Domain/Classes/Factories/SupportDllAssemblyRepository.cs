using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Factories
{
    public class SupportDllAssemblyRepository
    {
        private static readonly Dictionary<string, Assembly> assemblies = new Dictionary<string, Assembly>();

        public static Assembly GetSupportDllAssembly(string dllPath)
        {
            var dllName = Path.GetFileName(dllPath);
            if (String.IsNullOrEmpty(dllName))
                return null;

            if (assemblies.ContainsKey(dllName))
                return assemblies[dllName];

            Assembly assembly = null;
            try
            {
                assembly = Assembly.LoadFile(dllPath);
                assemblies.Add(dllName, assembly);
            }
            catch
            {
            }

            return assembly;
        }
    }
}
