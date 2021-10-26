using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AWCCPackageEligibility;

namespace BaseEligibilityComponent
{
    public abstract class EligibilityComponentBase_USBController  : IPackageEligibility
    {
        protected List<string> eligibleDevices;
        protected string err = string.Empty;
        protected string packageName = string.Empty;

        public string GetPackageName()
        {
            return packageName;
        }
        public bool CheckConnectedDeviceForEligibility()
        {
            bool isDeviceEligible = false;
            var regex = new Regex(@"\\VID_([a-fA-F0-9]{4})&PID_([a-fA-F0-9]{4})[\\&]{1}", RegexOptions.IgnoreCase);
            try
            {
                var searcher = new ManagementObjectSearcher(@"SELECT * FROM Win32_USBControllerDevice");
                foreach (var device in searcher.Get())
                {
                    var dependent = Convert.ToString(device["dependent"]);
                    var match = regex.Match(dependent);
                    if (match.Success && match.Groups.Count > 2 && match.Groups[1].Success && match.Groups[2].Success)
                    {
                        var pid = "0x" + match.Groups[2].Value;
                        var result = eligibleDevices.Find(x => string.Compare(x, pid, true) == 0);
                        if (result != null)
                        {
                            //Connected USB device is eligible for package install.
                            isDeviceEligible = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception e)
            {
            }
            return isDeviceEligible;
        }

        public bool IsPackageEligibleForSystem()
        {
            bool isPackageEligible = false;
            try
            {
                isPackageEligible = CheckConnectedDeviceForEligibility();
            }
            catch (Exception e)
            { }
            return isPackageEligible;
        }
        public string GetLastError()
        {
            return err;
        }
    }
}

