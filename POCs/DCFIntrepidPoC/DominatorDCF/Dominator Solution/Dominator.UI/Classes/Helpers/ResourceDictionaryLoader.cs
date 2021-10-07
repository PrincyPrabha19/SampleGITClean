using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Dominator.Tools.Helpers;

namespace Dominator.UI.Classes.Helpers
{
    public static class ResourceDictionaryLoader
    {        
        public const string ASSEMBLY_NAME = "OCControls.Resources";

        public static Assembly TryLoadResourceAssembly()
        {
            Assembly asm;
            try
            {
                asm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName.StartsWith(ASSEMBLY_NAME));
                if (asm != null)
                    return asm;

                var location = PathProvider.InstallPath;
                if (location == null)
                    return null;

                var dictFileName = Path.Combine(location, $"{ASSEMBLY_NAME}.dll");
                if (!File.Exists(dictFileName)) return null;

                asm = Assembly.LoadFrom(dictFileName);
            }
            catch (Exception e)
            {
                asm = null;
            }

            return asm;
        }

        public static void LoadInto(ResourceDictionary resources)
        {
            try
            {
                if (TryLoadResourceAssembly() == null) 
                    return;

                var uri = new Uri($"/{ASSEMBLY_NAME};component/Dictionaries/CommonDictionary.xaml", UriKind.RelativeOrAbsolute);
                ResourceDictionary dict = new ResourceDictionary { Source = uri };

                resources.MergedDictionaries.Clear();
                resources.MergedDictionaries.Add(dict);
            }
            catch (Exception e)
            {
            }
        }

        public static void LoadInto(ResourceDictionary resources, string viewPath)
        {
            try
            {
                var uri = new Uri($"/OCControls.UI;component{viewPath}", UriKind.RelativeOrAbsolute);
                ResourceDictionary dict = new ResourceDictionary { Source = uri };
                resources.MergedDictionaries.Add(dict);
            }
            catch (Exception e)
            {
            }
        }
    }
}