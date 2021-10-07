using System;
using System.Diagnostics;
using System.IO;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;
using Dominator.Tools.Classes;
using Dominator.Tools.Classes.Security;
using NotificationsExtensions;
using NotificationsExtensions.Toasts;

namespace NotificationLauncher.Classes
{
    public class WindowsNotification : IWindowsNotification
    {
        public string Caption { get; set; }
        public string Description { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationPath { get; set; }
        public string Attribution { get; set; }
        public string ImageSource { get; set; }
        public string Action1Text { get; set; }
        public string Action2Text { get; set; }

        public event Action NotificationResolved;

        private const string APP_ID = "E33D6C1F-69F1-4726-8127-4D0881CA74EF";

        public void Show()
        {
            var toastContent = new ToastContent()
            {
                Launch = APP_ID,

                Visual = new ToastVisual()
                {
                    BindingGeneric = new ToastBindingGeneric
                    {
                        AppLogoOverride = new ToastGenericAppLogo
                        {
                            HintCrop = ToastGenericAppLogoCrop.None,
                            Source = ImageSource
                        },
                        Children =
                        {
                            new AdaptiveText {Text = Caption},
                            new AdaptiveText
                            {
                                Text = Description
                            }
                        },
                        //Attribution = new ToastGenericAttributionText
                        //{
                        //    Text = Attribution
                        //},
                    }
                },

                Scenario = ToastScenario.Default,

                Actions = new ToastActionsCustom
                {
                    Buttons =
                    {
                        new ToastButton(Action1Text, ApplicationPath),
                        new ToastButtonDismiss(Action2Text)
                    }
                },

                Duration = ToastDuration.Long
            };

            showNotification(toastContent);
        }

        private void showNotification(ToastContent toastContent)
        {
            var xmlStr = toastContent.GetContent();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlStr);

            ToastNotifier toastNotifier = ToastNotificationManager.CreateToastNotifier(ApplicationName);
            ToastNotification toastNotification = new ToastNotification(xmlDoc);
            toastNotification.Activated += ToastNotification_Activated;
            toastNotification.Dismissed += ToastNotification_Dismissed;
            toastNotification.Failed += ToastNotification_Failed;
            toastNotifier.Show(toastNotification);
        }

        private void ToastNotification_Failed(ToastNotification sender, ToastFailedEventArgs args)
        {
            NotificationResolved?.Invoke();
        }

        private void ToastNotification_Dismissed(ToastNotification sender, ToastDismissedEventArgs args)
        {
            NotificationResolved?.Invoke();
        }

        private void ToastNotification_Activated(ToastNotification sender, object args)
        {
            var value = sender.Content.SelectSingleNode("//toast/@launch");
            var id = (value == null || value.NodeValue == null) ? string.Empty : value.NodeValue.ToString();
            if (string.IsNullOrEmpty(id) || string.Compare(id, APP_ID, StringComparison.InvariantCultureIgnoreCase) != 0)
                return;

            var activatedEventArgs = args as ToastActivatedEventArgs;
            if (activatedEventArgs == null) return;

            var eventArgs = activatedEventArgs;
            if (string.Compare(eventArgs.Arguments, APP_ID, StringComparison.InvariantCultureIgnoreCase) == 0 ||
                string.Compare(eventArgs.Arguments, "dismiss", StringComparison.InvariantCultureIgnoreCase) == 0)
                return;

            string[] appStrings = eventArgs.Arguments.Split('|');
            string appPath = appStrings[0];
            string appArgs = string.Empty;
            if (appStrings.Length > 1) appArgs = appStrings[1];
            
            if (File.Exists(appPath) && PathHelper.IsValid(appPath) && AuthenticationManager.Singleton.IsSigned(appPath))
                Process.Start(appPath, appArgs);

            NotificationResolved?.Invoke();
        }
    }
}
