using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
namespace PackageAssitant
{
    class SystemDetails
    {
        public static string GetManufacturer()
        {
            string manuf = string.Empty;
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            //collection to store all management objects
            ManagementObjectCollection moc = mc.GetInstances();
            if (moc.Count != 0)
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    // display general system information
                    if (mo["Manufacturer"] != null)
                    {
                        manuf = mo["Manufacturer"].ToString();
                    }

                }

            }
            return manuf;
        }
        public static string GetModel()
        {
            string model = string.Empty;
            ManagementClass mc = new ManagementClass("Win32_ComputerSystem");
            //collection to store all management objects
            ManagementObjectCollection moc = mc.GetInstances();
            if (moc.Count != 0)
            {
                foreach (ManagementObject mo in mc.GetInstances())
                {
                    // display general system information
                    if (mo["Model"] != null)
                    {
                        model = mo["Model"].ToString();
                    }

                }

            }
            return model;
        }
    }
}
