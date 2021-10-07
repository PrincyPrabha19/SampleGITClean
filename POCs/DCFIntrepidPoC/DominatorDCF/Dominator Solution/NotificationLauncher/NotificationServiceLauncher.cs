using System;
using System.IO;
using System.Threading;
using Dominator.Domain.Classes.Factories;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;
using Dominator.Tools.Helpers;
using Microsoft.Win32;
using NotificationLauncher.Classes;

namespace NotificationLauncher
{
    public static class NotificationServiceLauncher
    {
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;
        private static readonly ManualResetEvent manualResetEvent = new ManualResetEvent(false);
        private static Thread thread;

        public static void Start()
        {
            var biosSupportProvider = BIOSSupportProviderFactory.NewBIOSSupportProvider();
            if (!biosSupportProvider.IsBIOSSupportAPIWrapperInitialized || biosSupportProvider.IsBIOSInterfaceNotSupported) return;

            int overclockingReport;
            if (!biosSupportProvider.RefreshOverclockingReport(out overclockingReport, false)) return;

            string notificationDescription = string.Empty;
            if (!biosSupportProvider.IsOCUIBIOSControlStatusNotSupported && biosSupportProvider.IsOCUIBIOSControlStatusEnabled)
                notificationDescription = string.Format(Properties.Resources.LoadBIOSDefaultNotificationDescription, Properties.Resources.NotificationApplicationName);
            else if (biosSupportProvider.IsOCFailsafeFlagStatusEnabled)
                notificationDescription = Properties.Resources.FailSafeNotificationDescription;

            if (string.IsNullOrEmpty(notificationDescription)) return;

            thread = new Thread(delegate ()
            {
                var toastNotification = newWindowsNotification(notificationDescription);
                if (toastNotification == null) return;
                toastNotification.Show();
                manualResetEvent.WaitOne(25000);
            });

            thread.Start();
        }

        private static IWindowsNotification newWindowsNotification(string description)
        {
            string appPath, iconPath;
            if (!readPathsFromRegistry(out appPath, out iconPath))
            {
                logger.WriteError("Unable to locate app install path or icon path. Notification won't be launched.");
                return null;
            }         

            var notification = new WindowsNotification()
            {
                Caption = string.Format(Properties.Resources.NotificationCaption, Properties.Resources.NotificationApplicationName),
                Description = description,
                Attribution = Properties.Resources.NotificationAttribution,
                Action1Text = Properties.Resources.NotificationReviewNow,
                Action2Text = Properties.Resources.NotificationDismiss,
                ApplicationName = Properties.Resources.NotificationApplicationName,
                ApplicationPath = appPath,
                ImageSource = iconPath
            };

            notification.NotificationResolved += NotificationNotificationResolved;

            return notification;
        }

        private static void NotificationNotificationResolved()
        {
            manualResetEvent.Set();
        }

        private static bool readPathsFromRegistry(out string appPath, out string iconPath)
        {
            appPath = string.Empty;
            iconPath = string.Empty;

            try
            {
                iconPath = Path.Combine(PathProvider.InstallPath, "OCControls.png");
                appPath = Path.Combine(PathProvider.InstallPath, "OCControls.exe");

                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                    using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\Command Center", false))
                    {
                        var _installPath = key?.GetValue("S1_KEY");
                        if (_installPath != null)
                            appPath = _installPath + "|/L \"OC Controls\"";
                    }
            }
            catch (Exception e)
            {                
                return false;
            }

            return true;
        }
    }
}
