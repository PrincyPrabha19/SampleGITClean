using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Windows.Storage;

namespace ExternalDllLoading
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

             var files = await ApplicationData.Current.LocalFolder.GetFilesAsync();
            //var fileLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
             files = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFilesAsync();
            // StorageFolder sharedFolder = ApplicationData.Current.GetPublisherCacheFolder("Project");
            ////Read the first line of dataFile.txt in Publisher Cache folder Downloads
            //var fileo = await sharedFolder.GetFileAsync("ClassLibrary1.dll");
            //var newFile = fileo.CopyAsync(Windows.ApplicationModel.Package.Current.InstalledLocation);
            //var fold = await sharedFolder.GetFolderAsync("ClassLibrary1");
            //var boho = await Windows.ApplicationModel.Package.Current.InstalledLocation.CreateFolderAsync("doess");
            //var newFolder = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFolderAsync("ClassLibrary1");

            //var newPoo = await fold.GetFilesAsync();
            //foreach (var fl in newPoo)
            //{
            //    await fl.CopyAsync(newFolder);
            //}

            //var fileLocation = Windows.ApplicationModel.Package.Current.InstalledLocation;
            //var files = await fileLocation.GetFilesAsync();
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