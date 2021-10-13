using InstallationManager.DataModel;

namespace InstallationManager.ToastNotificationLib
{
    public class ReceivedObj : IToastUpdaterData
    {
        public string InstallationStatus { get; set; }

        public double ProgressValue { get; set; }

        public string ProgressValueStringOverride { get; set; }

        public OverallInstallationStatus OInstallStatus { get; set; }

        public double ProgressValueNext { get; set; }
    }
}
