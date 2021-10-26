using InstallationManager.DataModel;
using InstallationManager.MessengerLib;
using System;
using System.Threading;

namespace InstallationManager.InstallerHost
{
    class Program
    {
        static void Main(string[] args)
        {

            //if (args.Length != 0)
            //{
            //    ToastManager.LaunchProcess(args[0]);
            //}
            //else
            //{
            //    ToastManager.LaunchProcess(@"C:\Users\vishal\Desktop\bins\InstallationManager.ToastNotifier.exe");
            //}

            ToastManager.LaunchProcess(@"C:\Users\vishal\Desktop\bins\AWCC.ToastNotifier.exe", string.Empty);

            PublishMessages();
        }

        static Random rnd = new Random();

        public static void PublishMessages()
        {
            int _counter = 0;
            while (_counter <= 100)
            {
                ToastManager.InitializeToastSession();

                
                if (_counter == 1)
                {
                    ToastManager.SendMessage(new MessageFormat { InstallationStatus = $"Status {_counter}", OInstallStatus = OverallInstallationStatus.Launch, ProgressValue = (double)_counter / 100, ProgressValueStringOverride = $"{_counter}% Completed" });
                }
                else if (_counter == 100)
                {
                    ToastManager.SendMessage(new MessageFormat { InstallationStatus = $"Status {_counter}", OInstallStatus = OverallInstallationStatus.Completed, ProgressValue = (double)_counter / 100, ProgressValueStringOverride = $"{_counter}% Completed" });
                    break;
                }
                else
                {
                    ToastManager.SendMessage(new MessageFormat { InstallationStatus = $"Status {_counter}", OInstallStatus = OverallInstallationStatus.Updating, ProgressValue = (double)_counter / 100, ProgressValueStringOverride = $"{_counter}% Completed" });
                }

                //ToastManager.CloseToastSession();
                _counter++;

                Thread.Sleep(300);

            }
        }
    }

}
