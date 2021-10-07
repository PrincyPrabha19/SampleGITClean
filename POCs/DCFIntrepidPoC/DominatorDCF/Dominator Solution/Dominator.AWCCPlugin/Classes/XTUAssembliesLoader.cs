using System;
using System.IO;
using System.Reflection;

namespace Dominator.AWCCPlugin.Classes
{
    public static class XTUAssembliesLoader
    {
        public const string ASSEMBLY_NAME1 = "IntelOverclockingSDK.dll";
        public const string ASSEMBLY_NAME2 = "ProfileHelperModel.dll";
        

        public static void TryLoadResourceAssembly()
        {
            Assembly asm1, asm2;
            try
            {
                var location = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
                log($"location: {location}");
                if (location == null)
                    return;

                
                asm1 = Assembly.LoadFrom(Path.Combine(location, $"{ASSEMBLY_NAME1}"));
                asm2 = Assembly.LoadFrom(Path.Combine(location, $"{ASSEMBLY_NAME2}"));
            }
            catch (Exception e)
            {
                log(e.ToString());
            }
        }

        static void log(string msg)
        {
            using (var sw = new StreamWriter(@"c:\temp\t.txt", true))
            {
                sw.WriteLine(msg);
            }
        }
    }
}