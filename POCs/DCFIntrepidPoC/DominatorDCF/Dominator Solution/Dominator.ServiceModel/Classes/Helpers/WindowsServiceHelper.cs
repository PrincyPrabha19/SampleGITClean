using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Classes.Security;
using Dominator.Tools.Helpers;

namespace Dominator.ServiceModel.Classes.Helpers
{
    public class WindowsServiceHelper
    {
        #region Properties
        private readonly string serviceName;
        private readonly string serviceLauncherName;
        private readonly string processName;

        private readonly ServiceController service;
        private readonly ILogger logger = LoggerFactory.LoggerInstance;
        #endregion

        #region Constructor
        public WindowsServiceHelper(string serviceName, string processName, string serviceLauncherName)
        {
            this.serviceName = serviceName;
            this.serviceLauncherName = serviceLauncherName;
            this.processName = processName;

            service = new ServiceController(serviceName);
        }
        #endregion

        #region Methods
        public bool IsRunning()
        {
            var running = false;

            try
            {
                service.Refresh();
                running = service.Status == ServiceControllerStatus.Running;
            }
            catch (Exception e)
            {
                logger?.WriteError("WindowsServiceHelper.IsRunning", null, e.ToString());
            }

            return running;
        }

        public bool Start(ServiceDetails serviceDetails)
        {
            try
            {
                if (!IsRunning())
                {
                    service.Start();
                    service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(5));
                }
            }
            catch
            {
                try
                {
                    if (launchServiceWithAdminRight(serviceDetails))
                        service.WaitForStatus(ServiceControllerStatus.Running, TimeSpan.FromSeconds(5));
                }
                catch (Exception)
                {
                }
            }

            return IsRunning();
        }

        public void Stop(ServiceDetails serviceDetails)
        {
            try
            {
                if (IsRunning())
                {
                    service.Stop();
                    service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
            }
            catch
            {
                try
                {
                    if (launchServiceWithAdminRight(serviceDetails))
                      service.WaitForStatus(ServiceControllerStatus.Stopped, TimeSpan.FromSeconds(10));
                }
                catch (Exception)
                {
                }
            }          
        }

        public bool IsServiceInstalled()
        {
            return ServiceController.GetServices().Any(s => s.ServiceName == serviceName);
        }

        private bool launchServiceWithAdminRight(ServiceDetails serviceDetail)
        {
            try
            {
                var process = Process.GetProcesses().FirstOrDefault(pr => string.Compare(pr.ProcessName, processName, StringComparison.InvariantCultureIgnoreCase) == 0);
                if (process != null && serviceDetail != ServiceDetails.DominatorStop) return true;

                var location = PathProvider.InstallPath;
                var launcherName = Path.Combine(location, $"{serviceLauncherName}.exe");
                if (!File.Exists(launcherName) || !AuthenticationManager.Singleton.IsSigned(launcherName))
                    return false;
                ProcessStartInfo psi;
                if(serviceDetail == ServiceDetails.DominatorStop)
                   psi = new ProcessStartInfo { FileName = launcherName, Arguments = "dstop"};
                else if(serviceDetail == ServiceDetails.DominatorStart)
                   psi = new ProcessStartInfo { FileName = launcherName, Arguments = "dstar"};
                else if (serviceDetail == ServiceDetails.XTUStop)
                    psi = new ProcessStartInfo { FileName = launcherName, Arguments = "xstop" };
                else 
                   psi = new ProcessStartInfo { FileName = launcherName, Arguments = "xstar" };
                var p = new Process { StartInfo = psi };
                p.Start();
                p.WaitForExit();
            }
            catch (Exception e)
            {
                logger?.WriteError("WindowsServiceHelper.launchServiceWithAdminRight", null, e.ToString());
                return false;
            }

            return true;
        }
        #endregion
    }

    public enum ServiceDetails
    {
        XTUStart = 1,
        XTUStop = 2,
        DominatorStart = 3,
        DominatorStop = 4
    }
}

