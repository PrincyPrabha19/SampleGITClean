using System.IO;
using System.Threading;
using Dominator.Domain.Classes.Factories;
using Dominator.Tools.Classes;
using Dominator.Tools.Classes.Security;
using Dominator.Tools.Helpers;

namespace Dominator.Dell.Intrepid.Agent.Plugin
{
    public static class NotificationService
    {
        public static void Start()
        {
            var thread = new Thread(delegate ()
            {
                var ocStatusManager = OverclockingFactory.NewOCStatusManager();
                ocStatusManager.Initialize();
                if (!ocStatusManager.IsOCEnabled) return;

                var biosSupportProvider = BIOSSupportProviderFactory.NewBIOSSupportProvider();
                biosSupportProvider.Initialize();
                int overclockingReport;
                if (!biosSupportProvider.RefreshOverclockingReport(out overclockingReport, false)) return;
                if (!biosSupportProvider.IsOCFailsafeFlagStatusEnabled && !biosSupportProvider.IsOCUIBIOSControlStatusEnabled) return;
                var notifierPath = readNotifierInstallPathFromRegistry();
                if (File.Exists(notifierPath) && PathHelper.IsValid(notifierPath) && AuthenticationManager.Singleton.IsSigned(notifierPath))
                        new ProcessAPI().StartAsInteractiveUser(notifierPath);
            });

            thread.Start();
        }

        private static string readNotifierInstallPathFromRegistry()
        {
            return Path.Combine(PathProvider.InstallPath, "NotificationLauncher.exe"); 
        }
    }
}
