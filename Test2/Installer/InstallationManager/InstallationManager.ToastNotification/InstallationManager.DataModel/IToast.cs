namespace InstallationManager.DataModel
{
    public interface IToast
    {
        string ToastTitle { get; }

        void OnActivation();
    }
}
