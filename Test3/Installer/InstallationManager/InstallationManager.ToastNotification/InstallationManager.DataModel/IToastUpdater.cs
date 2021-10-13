namespace InstallationManager.DataModel
{
    public interface IToastUpdater
    {
        string ToastTag { get; }
        string ToastGroup { get; }
    }
}
