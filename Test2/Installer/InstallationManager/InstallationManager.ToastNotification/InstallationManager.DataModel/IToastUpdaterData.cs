namespace InstallationManager.DataModel
{
    public enum OverallInstallationStatus
    {
        Launch,
        Staging,
        Downloading,
        Extracting,
        Installing,
        Configuring,
        Cleanup,
        Rollback,
        Repairing,
        Removing,
        Updating,
        Modifying,
        Completed
    }

    public interface IToastUpdaterData
    {
        string InstallationStatus { get; }
        double ProgressValue { get; }
        double ProgressValueNext { get; }
        string ProgressValueStringOverride { get; }
        OverallInstallationStatus OInstallStatus { get; }
    }
}
