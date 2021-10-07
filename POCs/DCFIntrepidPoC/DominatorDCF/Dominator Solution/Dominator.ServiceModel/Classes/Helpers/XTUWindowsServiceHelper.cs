namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class XTUWindowsServiceHelper
    {
        #region Properties
        private const string SERVICE_NAME = "XTU3SERVICE";
        private const string SERVICE_LAUNCHER_NAME = "ServiceLauncher";
        private const string PROCESS_NAME = "XtuService";

        private static readonly WindowsServiceHelper serviceHelper = new WindowsServiceHelper(SERVICE_NAME, PROCESS_NAME, SERVICE_LAUNCHER_NAME);
        #endregion

        #region Methods
        public static bool IsRunning()
        {
            return serviceHelper.IsRunning();
        }

        public static bool Start()
        {
            return serviceHelper.Start(ServiceDetails.XTUStart);
        }

        public static bool IsServiceInstalled()
        {
            return serviceHelper.IsServiceInstalled();
        }
        #endregion
    }
}