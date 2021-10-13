using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ManagedCustomActions;
using PackageAssitant;
using System.Reflection;

namespace UnitTest
{
    class Program
    {
        static void UnitTestPackageManager(string[] args)
        {
            PackageManager m_pm = new PackageManager();
            m_pm.LoadPackages(args[0]);
            
            List<Assembly> la = m_pm.GetLoadedAssemblies();
            foreach (var assembly in la)
            {
                Console.Out.WriteLine("Loaded assembly=" + assembly.GetName().FullName);

            }
            if (false == m_pm.IsPackageEligibleForSystem("PackageMK"))
            {
                Console.Out.WriteLine("Not eligible for this system");

            } else
            {
                Console.Out.WriteLine("Eligible for this system");

            }

            if (false == m_pm.IsPackageEligibleForSystem("FIRSTPARTY"))
            {
                Console.Out.WriteLine("Not eligible for this system");

            }
            else
            {
                Console.Out.WriteLine("Eligible for this system");
            }
        }
        static void UnitTestAuthticodeTools(string folderpath)
        {
            //CustomAction.FilesinFolderSigned(folderpath);
        }
        static void UnitTestApplicationHide()
        {
            CustomAction ca = new CustomAction();
            ca.AddSystemComponentRegKey(@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\InstallShield_{9F4193F3-EBA4-489D-9003-D3820B65656A}");
        }
        static void Main(string[] args)
        {
            UnitTestPackageManager(args);
            
            // UnitTestApplicationHide();
            //CustomAction ca = new CustomAction();
            //if (ca.isVersionGreaterThan("5.3"))
            //{
            //    Console.WriteLine("Version is greater than 5.3");

            //}
            //else
            //{
            //    Console.WriteLine("Version is not greater than 5.3");
            //}


            return;


        }
    }
}
