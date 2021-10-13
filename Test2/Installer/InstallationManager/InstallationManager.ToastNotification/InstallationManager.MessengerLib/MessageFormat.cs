using InstallationManager.DataModel;

namespace InstallationManager.MessengerLib
{
    public class MessageFormat : IToastUpdaterData
    {
        public string InstallationStatus { get; set; }

        public double ProgressValue { get; set; }

        public string ProgressValueStringOverride { get; set; }

        public OverallInstallationStatus OInstallStatus { get; set; }

        public double ProgressValueNext { get; set; }
    }
}
