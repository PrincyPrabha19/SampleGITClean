using InstallationManager.DataModel;
using InstallationManager.MessengerLib;
using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Notifications;

namespace InstallationManager.ToastNotificationLib
{
    public class AWCCToast : IToast, IToastUpdater, IToastUpdaterData
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        static extern int IsZoomed(IntPtr hWnd);
        [DllImport("user32")]
        private static extern int IsIconic(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        private const int SW_SHOWMAXIMIZED = 3;
        private const int SW_RESTORE = 9;

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetWindowPlacement(IntPtr hWnd,
        [In] ref WINDOWPLACEMENT lpwndpl);

        [StructLayout(LayoutKind.Sequential)]
        private struct WINDOWPLACEMENT
        {
            public int length;
            public int flags;
            public int showCmd;
        }


        public string ToastTag { get; private set; } = $"AWCC Toast - {DateTime.Now.GetHashCode()}";
        public string ToastGroup { get; private set; }
        public string ToastTitle { get; private set; } = string.Empty;
        public string InstallationStatus { get; set; } = "Installation Started";
        public double ProgressValue { get; set; } = 0.0;
        public double ProgressValueNext { get; set; } = 0.0;
        private double _DisplayProgress = 0.0;
        public string ProgressValueStringOverride { get; set; } = "0 %";
        public OverallInstallationStatus OInstallStatus { get; set; }
        public static bool IsLaunchedWithSilent { get; set; } = false;
        private static ToastNotifierCompat NOTIFIER = ToastNotificationManagerCompat.CreateToastNotifier();

        private string WindowName = "Alienware Command Center Package Manager";
        private string procName = "AWCCInstallationManager";

        private bool _ProgressInterpolationStatus = false;
        private bool _isWaiting = false;
        //private bool _showCountertimedout = false;
        CancellationTokenSource cts = null;

        private int _timeoutToShowToast = 5000;
        private bool _doneWaiting = false;

        private object _lockObj = new object();

        private string _toastLogoImgPath;
        private uint _version = 0;
        private bool _launchCounter = false;

        public void UpdateToast(object sender, char[] receivedChars)
        {
            //Convert receivedChars to toast Object and update the values.
            string receivedObj = new string(receivedChars);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            var _toastData = new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom, ProgressValueNext = properties.Length > 4 ? Double.Parse(properties[4]) : 1.0 };

            ToastManager.WriteLog($"ProgressValue = {ProgressValue} & ProgressValueNext = {ProgressValueNext}");

            if (_toastData.OInstallStatus == OverallInstallationStatus.Completed)
            {
                CompletedToast();
            }
            else
            {
                lock (_lockObj)
                {
                    InstallationStatus = _toastData.InstallationStatus;
                    ProgressValue = _toastData.ProgressValue;
                    ProgressValueNext = _toastData.ProgressValueNext;
                    ProgressValueStringOverride = _toastData.ProgressValueStringOverride;
                    _DisplayProgress = ProgressValue;
                }

                CheckUpdateAndHandleProgressInterpolation();
                //ManageToastDisplay();
            }
        }

        private void ManageToastDisplay()
        {
            if (IsLaunchedWithSilent)
            {
                if (!_launchCounter)
                {
                    LaunchToast();
                    _launchCounter = true;
                }

                else
                    UpdateToast();

            }
            else
            {
                if (IsInstallerWindowMinimized(WindowName))
                {
                    if (!_isSessionOneToastAlive)
                    {
                        if (!_doneWaiting)
                        {
                            //ToastManager.WriteLog("To begin waiting");
                            if (!_isWaiting)
                            {
                                Task.Factory.StartNew(() =>
                                {
                                    _isWaiting = true;
                                    Task.Delay(_timeoutToShowToast).Wait();
                                    _doneWaiting = true;
                                });
                            }

                        }
                        else
                        {
                            LaunchToast();
                            _isSessionOneToastAlive = true;
                        }
                    }
                    else
                    {
                        _isWaiting = false;
                        _doneWaiting = false;
                        UpdateToast();
                    }

                }
                else
                {
                    if (_isSessionOneToastAlive)
                    {
                        ToastNotificationManagerCompat.History.Clear();
                        _isSessionOneToastAlive = false;
                    }
                }
            }

        }

        private void CheckUpdateAndHandleProgressInterpolation()
        {
            if (!_ProgressInterpolationStatus)
            {
                _ProgressInterpolationStatus = true;
                Task.Factory.StartNew(() =>
                {
                    while (OInstallStatus != OverallInstallationStatus.Completed)
                    {
                        //Perform Interpolation and update values once every second.
                        lock (_lockObj)
                        {
                            ManageToastDisplay();
                            _DisplayProgress = (ProgressValueNext - _DisplayProgress) / 5 + _DisplayProgress;
                            ToastManager.WriteLog($"Display Progress = {_DisplayProgress}");
                        }

                        Task.Delay(1000).Wait();
                    }

                });
            }
        }

        private bool _isCompleted = false;
        private bool _isSessionOneToastAlive = false;
        

        public void MonitorToast(object sender, char[] receivedChars)
        {
            string receivedObj = new string(receivedChars);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            var _toastData = new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom };

            if (_toastData.OInstallStatus == OverallInstallationStatus.Completed)
            {
                CompletedToast();
            }
            else
            {
                lock (_lockObj)
                {
                    InstallationStatus = _toastData.InstallationStatus;
                    ProgressValue = _toastData.ProgressValue;
                    ProgressValueStringOverride = _toastData.ProgressValueStringOverride;
                }
            }
        }

        public void MonitorSvc()
        {
            while (!IsInstallerWindowMinimized(WindowName))
            {
                ToastNotificationManagerCompat.History.Clear();
                _isSessionOneToastAlive = false;
                Task.Delay(_timeoutToShowToast).Wait();
            }

            ActSvc();
        }

        public void ActSvc()
        {
            while (IsInstallerWindowMinimized(WindowName))
            {
                if (!_isSessionOneToastAlive)
                {
                    Task.Delay(_timeoutToShowToast).Wait();
                    lock (_lockObj)
                    {
                        LaunchToast();
                    }
                    _isSessionOneToastAlive = true;
                }
                else
                {
                    lock (_lockObj)
                    {
                        UpdateToast();
                    }
                }

            }

            MonitorSvc();
        }

        public AWCCToast(string toastGroup = null, string toastTitle = "Default AWCC Title", string toastLogoImgPath = "")
        {
            //OnActivation();

            if (toastTitle == null)
            {
                toastTitle = "Default AWCC Title";
            }

            ToastGroup = toastGroup;

            string ProcessPath = Process.GetCurrentProcess().MainModule.FileName;

            _toastLogoImgPath = ProcessPath.Substring(0, ProcessPath.LastIndexOf('\\')) + toastLogoImgPath;
            ToastTitle = toastTitle;
        }

        public static void ClearToasts()
        {
            ToastNotificationManagerCompat.History.Clear();
        }

        private void LaunchToast()
        {
            #region Toast Construction
            /*
             * Core Components of toast are:
             * 1. Launch: Defines arguments which would be passed to the process being launched while the user clicks on the toast. 
             * 2. Visual: The visual portion of the toast, including the generic binding that contains text and images.
             * 3. Actions: The interactive portion of the toast, including inputs and actions.
             * 4. Audio: Controls the audio played when the toast is shown to the user.
             * Ref: https://docs.microsoft.com/en-us/windows/uwp/design/shell/tiles-and-notifications/adaptive-interactive-toasts?tabs=xml
             * 
             * Only Visual property is to be used in the current scope.
             * Important !! : As long as the host process is not alive onactivated toast would re-launch the hosting process in a background thread recursively.
             */

            //ToastManager.WriteLog("Launch toast called");

            ToastTag = $"AWCC Toast - {DateTime.Now.GetHashCode()}";

            ToastContent CustomToastContent = new ToastContent
            {
                Visual = new ToastVisual
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        Children =
                        {
                            new AdaptiveText { Text = ToastTitle },
                            new AdaptiveProgressBar
                            {
                                Title = "Installation Manager",
                                Value = new BindableProgressBarValue("progressValue"),
                                ValueStringOverride = new BindableString("progressValueStringOverride"),
                                Status = new BindableString("installationStatus")
                            }

                        },
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            Source = _toastLogoImgPath,
                            HintCrop = ToastGenericAppLogoCrop.Circle
                        }
                    }
                }

            };

            #endregion

            #region Toast Variable Binding
            var data = new Dictionary<string, string>
                    {
                        { "progressValue", _DisplayProgress.ToString() },
                        { "progressValueStringOverride", $"{Convert.ToInt32(_DisplayProgress*100)} %"},
                        {"installationStatus", InstallationStatus}
                    };
            #endregion

            #region Display Toast
            ToastNotification AWCCCustomToast = new ToastNotification(CustomToastContent.GetXml())
            {
                Tag = ToastTag,
                Data = new NotificationData(data),
                SuppressPopup = _launchCounter
            };

            ToastNotificationManagerCompat.History.Clear();

            //ToastManager.WriteLog("About to launch toast");
            ToastNotificationManagerCompat.CreateToastNotifier().Show(AWCCCustomToast);
            #endregion


        }

        public static bool ShowToastEval(string processName, string WindowName)
        {
            bool toShowToast = false;

            var procArr = Process.GetProcessesByName(processName);

            foreach (var process in procArr)
            {
                if (process.SessionId > 0)
                {
                    if (IsLaunchedWithSilent)
                    {
                        toShowToast = true;
                    }
                    else
                    {
                        toShowToast = IsInstallerWindowMinimized(WindowName);
                    }
                }
                else
                    toShowToast = true;
            }

            //toShowToast = true;

            return toShowToast;
        }

        public static bool IsInstallerWindowMinimized(string WindowTitle)
        {
            bool isMinimized = false;
            IntPtr handle = IntPtr.Zero;
            handle = FindWindow(null, WindowTitle);
            if (handle != IntPtr.Zero)
            {
                if (IsIconic(handle) != 0)
                {
                    isMinimized = true;
                }
            }
            ToastManager.WriteLog($"{WindowTitle} minimized status: {isMinimized}");
            return isMinimized;
        }

        public void OnActivation()
        {
            ToastNotificationManagerCompat.OnActivated += e =>
            {
                //ToastManager.WriteLog("If inside the activated method");

                if (!_isCompleted)
                {

                    //Check whether it is session 1 or session 0. If session 0, then LaunchToast only.
                    //If Session 1, check if Window minimized, maximize the window, if not then LaunchToast only.

                    if (IsParentSessionZero(procName) || IsLaunchedWithSilent)
                    {
                        LaunchToast();
                    }
                    else
                    {
                        if (IsInstallerWindowMinimized(WindowName))
                        {
                            //Maximize Installer Window
                            //ToastManager.WriteLog("Maximize Installer Window Called");
                            MaximizeWindow();
                            _isSessionOneToastAlive = false;
                            _doneWaiting = false;
                            _isWaiting = false;
                        }
                        else
                        {
                            ToastNotificationManagerCompat.History.Clear();
                        }
                    }
                    //ToastNotificationManagerCompat.History.Clear();
                    //LaunchToast();
                }

                else
                {
                    ToastNotificationManagerCompat.History.Clear();

                    foreach (var item in Process.GetProcessesByName("AWCC.ToastNotifier"))
                    {
                        item.Kill();
                    }
                }


            };
        }

        public static bool IsParentSessionZero(string parentProcName)
        {
            bool _isParentSessionZero = false;

            var procArr = Process.GetProcessesByName(parentProcName);

            foreach (var process in procArr)
            {
                if (process.SessionId > 0)
                {
                    _isParentSessionZero = false;
                }
                else
                    _isParentSessionZero = true;
            }

            return _isParentSessionZero;
        }

        private void MaximizeWindow()
        {
            bool isMaximized = true;
            IntPtr handle = IntPtr.Zero;
            handle = FindWindow(null, WindowName);
            if (handle != IntPtr.Zero)
            {
                if (IsIconic(handle) != 0)
                {
                    isMaximized = false;
                }
            }

            if (!isMaximized)
            {
                //Maximize Window
                //ToastManager.WriteLog("Window can be maximized here");
                ShowWindow(handle, SW_RESTORE);
            }
        }

        public void ManageToast(object sender, char[] receivedChars)
        {
            string receivedObj = new string(receivedChars);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            var toastData = new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom };

            switch (toastData.OInstallStatus)
            {
                case OverallInstallationStatus.Launch:

                    if (!_launchCounter)
                    {
                        InstallationStatus = toastData.InstallationStatus;
                        ProgressValue = toastData.ProgressValue;
                        ProgressValueStringOverride = toastData.ProgressValueStringOverride;

                        if (ShowToastEval(procName, WindowName))
                        {
                            LaunchToast();
                            _launchCounter = true;
                        }

                    }
                    break;
                case OverallInstallationStatus.Staging:
                    break;
                case OverallInstallationStatus.Downloading:
                    break;
                case OverallInstallationStatus.Extracting:
                    break;
                case OverallInstallationStatus.Installing:
                    break;
                case OverallInstallationStatus.Configuring:
                    break;
                case OverallInstallationStatus.Cleanup:
                    break;
                case OverallInstallationStatus.Rollback:
                    break;
                case OverallInstallationStatus.Repairing:
                    break;
                case OverallInstallationStatus.Removing:
                    break;
                case OverallInstallationStatus.Updating:
                    InstallationStatus = toastData.InstallationStatus;
                    ProgressValue = toastData.ProgressValue;
                    ProgressValueStringOverride = toastData.ProgressValueStringOverride;
                    if (ShowToastEval(procName, WindowName) && !_isSessionOneToastAlive)
                    {
                        LaunchToast();
                        _isSessionOneToastAlive = true;
                    }
                    UpdateToast();
                    break;
                case OverallInstallationStatus.Modifying:
                    break;
                case OverallInstallationStatus.Completed:
                    InstallationStatus = toastData.InstallationStatus;
                    ProgressValue = toastData.ProgressValue;
                    ProgressValueStringOverride = toastData.ProgressValueStringOverride;
                    CompletedToast();
                    break;
            }
        }

        private void UpdateToast()
        {
            var data = new Dictionary<string, string>
                        {
                            { "progressValue", _DisplayProgress.ToString() },
                            { "progressValueStringOverride",  $"{Convert.ToInt32(_DisplayProgress*100)} %"},
                            {"installationStatus", InstallationStatus}
                        };
            try
            {
                _version++;
                //ToastManager.WriteLog("About to update toast");
                NOTIFIER.Update(new NotificationData(data, _version), ToastTag);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }

        }

        private void CompletedToast()
        {

            if (IsLaunchedWithSilent)
            {
                ToastContent toastContent = new ToastContent()
                {
                    Visual = new ToastVisual()
                    {
                        BindingGeneric = new ToastBindingGeneric()
                        {
                            Children =
                            {
                                new AdaptiveText()
                                {
                                    Text = "Installation completed!"
                                },

                                new AdaptiveText()
                                {
                                    Text = "AWCC Advanced Installer Message"
                                }
                            },
                            AppLogoOverride = new ToastGenericAppLogo()
                            {
                                Source = _toastLogoImgPath,
                                HintCrop = ToastGenericAppLogoCrop.Circle
                            }
                        }
                    }
                };

                ToastNotification notification = new ToastNotification(toastContent.GetXml())
                {
                    Tag = "AWCC Completed Toast"
                };
                ToastNotificationManagerCompat.History.Clear();
                NOTIFIER.Show(notification);
            }

            
            _isCompleted = true;
        }
    }
}
