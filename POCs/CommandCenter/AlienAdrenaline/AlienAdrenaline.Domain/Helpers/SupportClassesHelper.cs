using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.Domain.Helpers
{
    public class SupportClassesHelper<T>
    {
        public static T GetSupportServiceInstance(string dllPath, string fullyQualifiedClassName)
        {
            var assembly = SupportDllAssemblyRepository.GetSupportDllAssembly(dllPath);
            var type = assembly.GetType(fullyQualifiedClassName);
            return (T)Activator.CreateInstance(type);                
        }
    }
}
