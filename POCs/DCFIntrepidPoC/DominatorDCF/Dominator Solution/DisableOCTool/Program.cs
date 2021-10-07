using System;
using System.IO;
using Dominator.Domain.Classes.Factories;
using Microsoft.Win32;

namespace DisableOCTool
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0 ||
                string.Compare(args[0], "/start", StringComparison.InvariantCultureIgnoreCase) != 0) return 0;

            bool appliedDefaults = false;
            bool enableOCBIOSSupport = false;
            bool removedOCData = false;
            bool updateDefaultModule = false;

            //Apply Default OC Profile
            try
            {
                var model = OverclockingFactory.NewOverclockingModel();
                appliedDefaults = model?.ApplyDefaultProfile() ?? false;
            }
            catch (Exception e)
            {
                appliedDefaults = false;
            }

            //Enable OC UI in BIOS
            try
            {
                var biosFactory = BIOSSupportProviderFactory.NewBIOSSupportProvider();
                enableOCBIOSSupport = biosFactory.SetOCUIBIOSControl(true);
            }
            catch (Exception e)
            {
                enableOCBIOSSupport = false;
            }            

            //Delete OC Status file
            try
            {
                string ocStatusPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls\OCStatus.xml");
                if (File.Exists(ocStatusPath))
                {
                    File.Delete(ocStatusPath);
                    removedOCData = true;
                }
            }
            catch (Exception e)
            {
                removedOCData = false;
            }

            //Clear Default Module in AWCC (if OC Controls was selected)
            try
            {
                updateDefaultModule = updateAWCCDefaultModule();
            }
            catch (Exception e)
            {
            }

            try
            {
                string outputLog = Path.Combine(Path.GetTempPath(), "DisableOCTool.log");
                using (var sw = new StreamWriter(outputLog, false))
                {
                    sw.Write($"{DateTime.Now} Default OC Applied [{appliedDefaults}] Removed OC Status [{removedOCData}] Enable OCBIOSControlFlag [{enableOCBIOSSupport}] Update Default Module [{updateDefaultModule}]");
                    sw.Close();
                }
            }
            catch { }

            return 0;
        }

        static bool updateAWCCDefaultModule()
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\Command Center", true))
                {
                    var o = key?.GetValue("DefaultModule", null);
                    if (o != null)
                    {
                        var defaultModule = o.ToString();
                        if (string.Compare(defaultModule, "OC Controls", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            key.SetValue("DefaultModule", "");
                            return true;
                        }
                    }
                }                
            }
            catch (Exception)
            {
            }

            return false;
        }
    }
}
