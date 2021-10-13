using InstallationManager.DataModel;

namespace InstallationManager.ToastNotificationLib
{
    public class ToastUpdate : IToastUpdaterData
    {
        public string InstallationStatus { get; private set; }
        public double ProgressValue { get; private set; }
        public string ProgressValueStringOverride { get; private set; }
        public OverallInstallationStatus OInstallStatus { get; private set; }

        public double ProgressValueNext { get; private set; }

        public ToastUpdate(string InstallationStatus = "Installation Started", double ProgressValue = 0.00d, string ProgressValueStringOverride = "0 %", OverallInstallationStatus OInstallStatus = OverallInstallationStatus.Launch)
        {
            this.InstallationStatus = InstallationStatus;
            this.ProgressValue = ProgressValue;
            this.ProgressValueStringOverride = ProgressValueStringOverride;
            this.OInstallStatus = OInstallStatus;
        }
    }
}
