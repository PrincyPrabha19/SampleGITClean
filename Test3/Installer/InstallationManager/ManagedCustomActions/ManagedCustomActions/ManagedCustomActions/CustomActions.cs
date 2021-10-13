using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Reflection;
using System.Diagnostics;
using System.Threading;
using Microsoft.Win32;
using InstallationManager.MessengerLib;
using InstallationManager.DataModel;
using System.IO;
using AWCCSecurity;

namespace ManagedCustomActions
{
    [Guid("BAFAEAED-08C6-4679-B94E-487A4D89DE63")]

    [TypeLibType(4288)]

    public interface ISuiteExtension

    {

        [DispId(1)]

        string get_Attribute(string bstrName);

        [DispId(2)]

        void LogInfo(string bstr);

        [DispId(3)]

        string get_Property(string bstrName);

        [DispId(3)]

        void set_Property(string bstrName, string bstrValue);

        [DispId(4)]

        string FormatProperty(string bstrValue);

        [DispId(5)]

        string ResolveString(string bstrStringId);

    }
    public class CustomAction
    {
        

        const UInt32 ERROR_INSTALL_FAILURE = 1603;

        const UInt32 ERROR_SUCCESS = 0;
        private const string PROPERTY_OVERALLPROGRESS = "ISInstallProgress";
        private const string CURRENT_PACKAGEPROGRESS = "ISParcelProgress";
        private const string INSTALL_STATUS = "ISInstallStatus";
        private const string PACKAGE_STATUS = "ISParcelStatus";
        private const string PROPERTY_SUPPORTDIR = "SETUPSUPPORTDIR";
        private const string PROPERTY_PACKAGE_NAMES = "PACKAGE_NAMES_CSV";
        private const string PROPERTY_SILENT_INSTALL = "ISSilentInstall";
        private const string PROPERTY_IS_DRIVER_FLOW = "IS_DRIVER_FLOW";
        private const string NOTIFICATIONAPP = "InstallationManager.ToastNotifier.exe";
        private const string AWCC_INSTALL_KEY = @"SOFTWARE\Alienware\Alienware Command Center\";
        private const string IS_TOAST_DISABLE = "IS_TOAST_DISABLE";
        private const string PROGRAMDATA = "PROGRAMDATA";

        List<string> packages = null;
        
       
        ISuiteExtension GetExtensionFromDispatch(object pDispatch)

        {

            if (Marshal.IsComObject(pDispatch) == false)

                throw new System.ContextMarshalException("Invalid dispatch object passed to CLR method");
            return (ISuiteExtension)pDispatch;

        }
        // This function is for diagnostics and logging
        private void Dump_LoadedLibraries(ISuiteExtension suiteExtension, PackageManager pm)
        {
            if (pm != null && suiteExtension != null)
            {
                List<Assembly> la = pm.GetLoadedAssemblies();
                foreach (var assembly in la)
                {
                    suiteExtension.LogInfo("Loaded assembly=" + assembly.GetName().FullName);

                }
            }
        }
        private void GetInstallerWindowHandle()
        {
            
            // Get the handle to a dialog
            
        }
        private void SendMessage(ISuiteExtension suiteExtension, OverallInstallationStatus status, int progress, int progress_next, string message)
        {
            if (suiteExtension != null)
            {
                string isToastDisable = suiteExtension.get_Property(IS_TOAST_DISABLE);
                if (isToastDisable.Equals("NO", StringComparison.OrdinalIgnoreCase))
                {
                    MessageFormat msg = new MessageFormat();
                    msg.ProgressValue = (double)progress / 100;
                    msg.ProgressValueNext = (double)progress_next / 100;
                    msg.InstallationStatus = message;
                    msg.OInstallStatus = status;
                    msg.ProgressValueStringOverride = $"{progress}% Completed";
                    ToastManager.InitializeToastSession();
                    ToastManager.SendMessage(msg);
                }
            }

        }
        // progress value for OnBegin will be inbetween 0 and 10
        public UInt32 OnBegin(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 0;
                int progress_next = 10;
                
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                // Launch process only for the first time
                string path = suiteExtension.get_Property(PROPERTY_SUPPORTDIR);
                path = System.IO.Path.Combine(path, NOTIFICATIONAPP);
                suiteExtension.LogInfo("Application path=" + path);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_ON_BEGIN");
                suiteExtension.LogInfo("ID_ON_BEGIN message=" + msg);
                string issilent = suiteExtension.get_Property(PROPERTY_SILENT_INSTALL);
                // set program data folder so that it will be used during uninstall
                suiteExtension.set_Property(PROGRAMDATA, Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
                suiteExtension.LogInfo("issilent=" + issilent);
                string isToastDisable = suiteExtension.get_Property(IS_TOAST_DISABLE);
                suiteExtension.LogInfo("is toast disable:" + isToastDisable);
                if (isToastDisable.Equals("NO", StringComparison.OrdinalIgnoreCase) ) {

                    if (issilent.Equals("True", StringComparison.OrdinalIgnoreCase))
                    {
                        suiteExtension.LogInfo("Calling LaunchProcess with Silent");
                        ToastManager.LaunchProcess(path, "/SILENT");
                    }
                    else
                    {
                        suiteExtension.LogInfo("Calling LaunchProcess without Silent");
                        ToastManager.LaunchProcess(path, String.Empty);
                    }
                    SendMessage(suiteExtension, OverallInstallationStatus.Launch, progress, progress_next, msg);
                }

            }
            catch (System.ContextMarshalException)
            { 
                return ERROR_INSTALL_FAILURE;
                
            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        public UInt32 OnStaging(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                int progress = 10;
                int progress_next = 30;
                suiteExtension.LogInfo("Percentage = " + progress.ToString());

                string msg = suiteExtension.ResolveString("ID_DOWNLOAD_START");
                suiteExtension.LogInfo("ID_DOWNLOAD_START message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);
            }
            catch (System.ContextMarshalException)
            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        // Test code
        
        private bool IsPackageSigned(ISuiteExtension suiteExtension)
        {
            bool ret = false;
            string localappfolder = suiteExtension.get_Property("LocalAppDataFolder");
            string downloadpath = System.IO.Path.Combine(localappfolder, @"Downloaded Installations\AWCC");
            suiteExtension.LogInfo("Download Location=" + downloadpath);
            if (false == DellDigitalSignatureVerifier.Singleton.isFolderContentSigned(downloadpath))
            {
                suiteExtension.LogInfo("Packages not signed");
                ret = false;

            }
            else
            {
                suiteExtension.LogInfo("Packages signed");
                ret = true;
            }
               
            return ret;
        }
        // check security of package if not met fail the entire installer.
        public UInt32 OnStaged(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                
                int progress = 30;
                int progress_next = 35;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                if (false == IsPackageSigned(suiteExtension))
                {
                    suiteExtension.set_Property("IS_PACKAGES_SIGNED", "NO");
                    string ms = suiteExtension.ResolveString("ID_SECURITY_ISSUE");
                    SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, ms);
                    return ERROR_INSTALL_FAILURE;
                }
                suiteExtension.LogInfo("Percentage = " + progress.ToString());

                string msg = suiteExtension.ResolveString("ID_DOWNLOAD_COMPLETE");
                suiteExtension.LogInfo("ID_DOWNLOAD_COMPLETE message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        public UInt32 OnAWCC_Package_Configure(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 35;
                int progress_next = 70;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());

                string msg = suiteExtension.ResolveString("ID_AWCC_INSTALL");
                suiteExtension.LogInfo("ID_AWCC_INSTALL message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);
            }
            catch (System.ContextMarshalException)
            {
                return ERROR_INSTALL_FAILURE;
            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }
        public UInt32 OnAWCC_Package_Configured(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {

                int progress = 70;
                int progress_next = 70;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
               
                
                string msg = suiteExtension.ResolveString("ID_AWCC_INSTALLED");
                suiteExtension.LogInfo("ID_AWCC_INSTALLED message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

                // Copy awcc_uninstall.iss to 
                // "Program Files (x86)\InstallShield Installation Information\{9F4193F3-EBA4-489D-9003-D3820B65656A}"
                // This value can be stored in AWCC_SETUP_FOLDER
                string path = suiteExtension.get_Property(PROPERTY_SUPPORTDIR);
                string support_uninstall_path = System.IO.Path.Combine(path, "awcc_uninstall.iss");
                suiteExtension.LogInfo("uninstall support path=" + support_uninstall_path);
                string setup_path = suiteExtension.get_Property("AWCC_SETUP_FOLDER");
                string installshieldpath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), 
                    setup_path);
                string uninstallpath = installshieldpath  + "awcc_uninstall.iss";
                
                suiteExtension.LogInfo("uninstall installshieldpath =" + uninstallpath);
                
                System.IO.File.Copy(support_uninstall_path, uninstallpath, true);

                string support_repair_path = System.IO.Path.Combine(path, "awcc_repair.iss");
                suiteExtension.LogInfo("support_repair_path=" + support_uninstall_path);
                string repair_path = installshieldpath + "awcc_repair.iss";
                suiteExtension.LogInfo("repair installshieldpath =" + repair_path);

                System.IO.File.Copy(support_repair_path, repair_path, true);

                // Copy UninstallHelper.exe to Programfilesx86\InstallShield Installation Information\ 
                string uninstallHelperpath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86),
                    "InstallShield Installation Information");
                uninstallHelperpath = System.IO.Path.Combine(uninstallHelperpath, "UninstallHelper.exe");
                suiteExtension.LogInfo("UninstallerHelper path =" + uninstallHelperpath);
                string support_uinstallerhelper_path = System.IO.Path.Combine(path, "UninstallHelper.exe");
                System.IO.File.Copy(support_uinstallerhelper_path, uninstallHelperpath, true);
            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        public UInt32 OnSmartInstaller_Configure(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 70;
                int progress_next = 75;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_SMART_INSTALL");
                suiteExtension.LogInfo("ID_SMART_INSTALL message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }
        public UInt32 OnSmartInstaller_Configured(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 75;
                int progress_next = 80;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_SMART_INSTALLED");
                suiteExtension.LogInfo("ID_SMART_INSTALLED message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }
        public UInt32 OnAWFXSmart_Configure(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 80;
                int progress_next = 85;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_AWFXSMART_INSTALL");
                suiteExtension.LogInfo("ID_AWFXSMART_INSTALL message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }
        public UInt32 OnAWFXSmart_Configured(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 85;
                int progress_next = 90;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_AWFXSMART_INSTALLED");
                suiteExtension.LogInfo("ID_AWFXSMART_INSTALLED message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }

        public UInt32 OnOC_Package_Configure(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 90;
                int progress_next = 95;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_OC_INSTALL");
                suiteExtension.LogInfo("ID_AWCC_INSTALLED message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }
        public UInt32 OnOC_Package_Configured(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 95;
                int progress_next = 100;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_OC_INSTALLED");
                suiteExtension.LogInfo("ID_AWCC_INSTALLED message=" + msg);
                
                SendMessage(suiteExtension, OverallInstallationStatus.Updating, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;
        }

        public UInt32 OnEnd(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                int progress = 100;
                int progress_next = 100;
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Percentage = " + progress.ToString());
                string msg = suiteExtension.ResolveString("ID_ON_END");
                suiteExtension.LogInfo("ID_ON_END message=" + msg);
                SendMessage(suiteExtension, OverallInstallationStatus.Completed, progress, progress_next, msg);

            }
            catch (System.ContextMarshalException)
            {
                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        public bool AddSystemComponentRegKey(string key)
        {
            bool ret = false;
            RegistryKey uninstallkey = Registry.LocalMachine.OpenSubKey(key, true);
            uninstallkey.SetValue("SystemComponent", 1);
            return ret;
        }
        public UInt32 HideApps(object pDispatch)
        {
            ISuiteExtension suiteExtension = null;
            try
            {
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                string keycsv = suiteExtension.get_Property("APPS_TO_HIDE_KEYS_CSV");
                string[] keys = keycsv.Split(',');
                foreach(string k in keys)
                {
                    string key = k.Trim();
                    suiteExtension.LogInfo("Hiding App with key" + key);
                    AddSystemComponentRegKey(key);
                }

            }
            catch (System.ContextMarshalException)
            {
                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }

            return ERROR_SUCCESS;
        }
        public UInt32 ConditionalInstall(object pDispatch)

        {
            ISuiteExtension suiteExtension = null;
            string supportdir = string.Empty;
            try
            {
                // Add programmatic conditional check and then set the property
                packages = new List<string>();
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                supportdir = suiteExtension.get_Property(PROPERTY_SUPPORTDIR);
                string package_csv = suiteExtension.get_Property(PROPERTY_PACKAGE_NAMES);
                packages = package_csv.Split(',').ToList<string>();
                PackageManager pm = null;
                pm = new PackageManager();
                pm.LoadPackages(supportdir);
                // debug only
                Dump_LoadedLibraries(suiteExtension, pm);
                // debug only
                // Execute condition for each package
                foreach (string pack in packages)
                {
                    string package = pack.Trim();
                    if (pm.IsPackageEligibleForSystem(package) == false)
                    {
                        // Set corresponding <PACKAGENAME>_INSTALL
                        suiteExtension.set_Property(package + "_INSTALL", "NO");
                        suiteExtension.LogInfo("This Package is not eligibale:" + package);
                    }
                    else
                    {
                        suiteExtension.set_Property(package + "_INSTALL", "YES");
                        suiteExtension.LogInfo("This Package is eligibale:" + package);
                    }
                }

            }
            catch (System.ContextMarshalException)

            {

                return ERROR_INSTALL_FAILURE;

            }
            catch (Exception ex)
            {
                if (suiteExtension != null)
                {
                    suiteExtension.LogInfo("Exception:" + ex.Message);
                }
            }
            return ERROR_SUCCESS;


        }
        public bool isVersionGreaterThan(string targetversion)
        {
            bool ret = false;
            using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
            using (var key = hklm.OpenSubKey(AWCC_INSTALL_KEY))
            {
                var version = key.GetValue("InstallVersion");
                if (null != version)
                {
                    Version target = new Version(targetversion);
                    Version current = new Version(version.ToString().Trim());
                    int result = current.CompareTo(target);
                    if (result > 0)
                    {
                        // this will abort the installation and rollsback
                        ret = true;
                    }
                }

            }
            return ret;
        }
        public UInt32 DriverFlowCheck(object pDispatch)
        {
            uint ret = ERROR_SUCCESS;
            ISuiteExtension suiteExtension = null;
            try
            {
                suiteExtension = GetExtensionFromDispatch(pDispatch);
                suiteExtension.LogInfo("Checking driver flow...");
                string isDriverFlow = suiteExtension.get_Property(PROPERTY_IS_DRIVER_FLOW);
                if(isDriverFlow.Equals("YES", StringComparison.OrdinalIgnoreCase))
                {

                    suiteExtension.LogInfo("IS_DRIVER_FLOW is set to YES, checking if AWCC version is >= 5.3");
                    if (isVersionGreaterThan("5.3"))
                    {
                        ret = ERROR_INSTALL_FAILURE;
                        suiteExtension.LogInfo("AWCC version is >= 5.3, Aborting Installation");
                    }

                }
            }
            catch (System.ContextMarshalException)
            {

                ret = ERROR_INSTALL_FAILURE;

            }

            return (ret);
        }


    }
}
