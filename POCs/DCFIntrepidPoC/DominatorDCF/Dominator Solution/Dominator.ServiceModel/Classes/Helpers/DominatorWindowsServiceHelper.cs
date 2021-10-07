namespace Dominator.ServiceModel.Classes.Helpers
{
    public static class DominatorWindowsServiceHelper
    {
        #region Properties
        private const string SERVICE_NAME = "OCControlsWindowsService";
        private const string SERVICE_LAUNCHER_NAME = "ServiceLauncher";
        private const string PROCESS_NAME = "OCControlsWindowsService";

        private static readonly WindowsServiceHelper serviceHelper =  new WindowsServiceHelper(SERVICE_NAME, PROCESS_NAME, SERVICE_LAUNCHER_NAME);
        #endregion

        #region Methods
        public static bool IsRunning()
        {
            return serviceHelper.IsRunning();
        }

        public static bool Start()
        {
          return serviceHelper.Start(ServiceDetails.DominatorStart);
        }

        public static void Stop()
        {
           serviceHelper.Stop(ServiceDetails.DominatorStop);
        }

        public static bool IsServiceInstalled()
        {
            return serviceHelper.IsServiceInstalled();
        }
        #endregion
    }
}