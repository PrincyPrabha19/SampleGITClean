using InstallationManager.DataModel;
using InstallationManager.MessengerLib;
using InstallationManager.ToastNotificationLib;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace InstallationManager.ToastNotifier
{
    class Program
    {
        public static bool isParentAlive = false;
        private static string _SelfProcess = "AWCC.ToastNotifier";
        private static string _ParentProcess = "AWCCInstallationManager";
        private static string _pipeName = "AWCC Comm Pipe";
        private static string _toastTitle = "AWCC Installation Manager";
        private static string _logoPath = @"\AWCCLogo.PNG";

        static void Main(string[] args)
        {
            //Thread.Sleep(100);

            #region Pre-requisite Validation
            try
            {
                bool isSelfRunning = Process.GetProcessesByName(_SelfProcess).Length > 1;
                if (isSelfRunning)
                {
                    Environment.Exit(0);
                }
            }
            catch (Exception ex)
            {
                ToastManager.WriteLog($"Failed to Identify if another instance is running: {ex.Message}");
            }

            //Spawn a thread which checks if the parent process is running, if the parent is not present, kill the current process. Provide an interval of 500 ms.

            isParentAlive = Process.GetProcessesByName(_ParentProcess).Length > 0;

            if (!isParentAlive)
            {
                ToastManager.WriteLog("Terminating process as parent is killed or exited!!");
                AWCCToast.ClearToasts();
                Environment.Exit(0);
            }
            Task.Factory.StartNew(() =>
                {
                    while (Process.GetProcessesByName(_ParentProcess).Length > 0)
                    {
                        Thread.Sleep(500);
                    }
                    ToastManager.WriteLog("Terminating process as parent is killed or exited!!");
                    AWCCToast.ClearToasts();
                    Environment.Exit(0);
                });
            #endregion

            ToastManager.WriteLog($"Testing. Args.Length = {args.Length}");

            var receiver = new MsgReceiverPublisher(_pipeName);

            IToast toast = new AWCCToast(null, _toastTitle, _logoPath);

            if (args.Length > 0 && args[0].ToUpper() == "/SILENT")
                AWCCToast.IsLaunchedWithSilent = true;

            ToastManager.WriteLog($"Toast is launched with Silent Status = {AWCCToast.IsLaunchedWithSilent}");

            receiver.MsgReceived += (toast as AWCCToast).UpdateToast;

            toast.OnActivation();

            receiver.ListenForMessages();
        }
    }
}
