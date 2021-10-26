using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using AWCCPackageEligibility;
namespace PackageAssitant
{
    public class FirstPartyEligibility : IPackageEligibility
    {
        private  String[] SupportedManufacturers = { "Alienware", "Dell Inc." };
        // These Models are list of supported models
        // this is just a saftey net in case isELCPresent logic fails to detect
        private String[] SupportedModels = { "G55590", "G77590", "G77790", "G33590", "G55000", "G33500",
            "G55500", "G55505", "G77500", "G77700", "DellG155510", "G175710", "DELLG155511", "DELLG155515",
            "Alienware17R5", "Alienware15R4", "AlienwareArea51R5", "AlienwareArea51R6", "AlienwareArea51R7",
            "Alienwarem15", "Alienwarem17", "AlienwareAuroraR8", "AlienwareAuroraR9", "AlienwareAuroraRyzenEdition",
            "AlienwareArea51mR2", "AlienwareAuroraR11", "AlienwareAuroraR12", "AlienwareAuroraR13", "Alienwarem15R3",
            "Alienwarem15R6", "Alienwarem17R3", "Alienwarem15R4", "Alienwarem17R4", "AlienwareAuroraRyzenEditionR10",
            "Alienwarem15RyzenEd.R5", "Alienwarex15R1", "Alienwarex17R1", "AlienwareArea51m", "Alienwarem15R2", "Alienwarem17R2"};
        private string err = string.Empty;
        
        public string GetPackageName()
        {
            // This should be one among CSV for PACKAGE_NAMES
            return ("FIRSTPARTY"); 
        }
        // Desc: In order to find if its Alienware, after AWCC core is installed
        // Under following registry key Computer\HKEY_LOCAL_MACHINE\SOFTWARE\Alienware\System\AFXCapableDevices\
        // One of the PID value will be 0x0550
        // isPIDAvailable(@"SOFTWARE\Alienware\System\AFXCapableDevices", "0x0550", "PID")
        // *This function is no more used*
        private bool isPIDAvailable(string path, string valueToFind, string PID)
        {
            bool ret = false;
            // Since this function is launched for 32 bit process
            RegistryKey wowkey = RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, RegistryView.Registry64);
            RegistryKey AFXDevicesKey = wowkey.OpenSubKey(path);
            // 2.0, 2.1 versions
            var versions = AFXDevicesKey.GetSubKeyNames();
            foreach (var version in versions)
            {
                RegistryKey subkey = AFXDevicesKey.OpenSubKey(version);
                var keys = subkey.GetSubKeyNames();
                foreach (var key in keys)
                {
                    RegistryKey guid = subkey.OpenSubKey(key);
                    if (string.Equals(valueToFind, guid.GetValue(PID, string.Empty).ToString(), StringComparison.CurrentCulture))
                    {
                        ret = true;
                    }
                }
            }
            return ret;
        }
        private bool isElcPresent()
        {
            bool elcPresent = false;
            //List<Tuple<string, string>> devices = new List<Tuple<string, string>>();
            var regex = new System.Text.RegularExpressions.Regex(@"\\VID_([a-fA-F0-9]{4})&PID_([a-fA-F0-9]{4})[\\&]{1}", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            try
            {
                var searcher = new System.Management.ManagementObjectSearcher(@"SELECT * FROM Win32_USBControllerDevice");
                foreach (var device in searcher.Get())
                {
                    var dependent = Convert.ToString(device["dependent"]);
                    var match = regex.Match(dependent);
                    if (match.Success &&
                        match.Groups.Count > 2 &&
                        match.Groups[1].Success && match.Groups[2].Success)
                    {
                        var vid = "0x" + match.Groups[1].Value.ToUpper();
                        var pid = "0x" + match.Groups[2].Value.ToUpper();
                       // if (!devices.Exists(x => x.Item1 == vid && x.Item2 == pid))
                       //     devices.Add(Tuple.Create(vid, pid));
                        if (vid == "0x187C" && pid == "0x0550")
                            elcPresent = true;
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return elcPresent;
        }
        public bool IsPackageEligibleForSystem()
        {
            bool ret = false;
            bool flag_manu = false;
            bool flag_model = false;
            // If PID matches then no need to check for any other condition.
            // This PID value will be available only once AWCC core is installed
            if (isElcPresent()  == true)
            {
                return true;
            }
            // Match manufacturer, any one manufacturer matches then set the flag.
            foreach (string manufacturer in SupportedManufacturers)
            {
                if (SystemDetails.GetManufacturer().Equals(manufacturer, StringComparison.OrdinalIgnoreCase))
                {
                    flag_manu = true;
                }
            }
            // Match Model, any model matches then set the flag.
            foreach(string model in SupportedModels)
            {
                string temp = SystemDetails.GetModel();
                string spacetrimmedModel = String.Concat(temp.Where(c => !Char.IsWhiteSpace(c)));
                if (spacetrimmedModel.Equals(model, StringComparison.OrdinalIgnoreCase))
                {
                    flag_model = true;
                }
                
            }
            // Both the conditions should match
            if (flag_manu == true && flag_model == true)
            {
                ret = true;
            }
            return ret;
        }
        public string GetLastError()
        {
            return err;
        }
    }
}
