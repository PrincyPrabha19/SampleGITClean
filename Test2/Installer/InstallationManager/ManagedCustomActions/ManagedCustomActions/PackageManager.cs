using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using PackageAssitant;
using AWCCSecurity;
using AWCCPackageEligibility;
namespace ManagedCustomActions
{
   
    public class PackageManager
    {
        Dictionary<string, IPackageEligibility> m_packages = null;
        List<Assembly> m_plugInAssemblyList = null;
        
        private  int LoadPlugInAssemblies(string path)
        {
            if (Directory.Exists(path))
            {
                DirectoryInfo dInfo = new DirectoryInfo(path);
                FileInfo[] files = dInfo.GetFiles("PackageEligibility*.dll");
                
                m_plugInAssemblyList = new List<Assembly>();

                if (null != files)
                {
                    foreach (FileInfo file in files)
                    {
                        var assembly = Assembly.LoadFile(file.FullName);
                        Module module = assembly.GetModules().First();
                        
                        if (DellDigitalSignatureVerifier.Singleton.IsDellSigned(file.FullName))
                        {
                            Console.Out.WriteLine("Certificate valid for:" + file);
                            m_plugInAssemblyList.Add(assembly);
                        }
                    }
                }
            }
            return 0;

        }

        public static Type GetDerivedType(Assembly assembly)
        {

            Type derived = null;
            var types = assembly.GetTypes();
            foreach(Type t in types)
            {
                
                if (t.IsInterface == false && typeof(IPackageEligibility).IsAssignableFrom(t))
                {
                    derived = t;
                    break;
                }
            }
            return derived;
        }
        
        private  List<IPackageEligibility> GetPackageImplemenation()
        {
            List<Type> availableTypes = new List<Type>();
            List<IPackageEligibility> packages = new List<IPackageEligibility>();
            foreach (Assembly currentAssembly in m_plugInAssemblyList)
            {
                
                var derived = GetDerivedType(currentAssembly);
                if (derived != null)
                {
                    System.Runtime.Remoting.ObjectHandle oh = Activator.CreateInstanceFrom(currentAssembly.CodeBase, derived.FullName);
                    var package = oh.Unwrap() as IPackageEligibility;
                    packages.Add(package);
                }

            }
            return packages;
        }
        public int LoadPackages(string path)
        {
            LoadPlugInAssemblies(path);
            List<IPackageEligibility> packages = GetPackageImplemenation();
            if (packages.Count > 0)
            {
                m_packages = new Dictionary<string, IPackageEligibility>();

                foreach (var package in packages)
                {
                    m_packages.Add(package.GetPackageName(), package);
                }
            }

            return 0;

        }
        public bool IsPackageEligibleForSystem(string package_name)
        {
            bool ret = false;
            if (m_packages != null && m_packages.ContainsKey(package_name))
            {
                ret = m_packages[package_name].IsPackageEligibleForSystem();
            }
            return ret;
        }
        public List<Assembly> GetLoadedAssemblies()
        {
            return m_plugInAssemblyList;
        }
    }
}
