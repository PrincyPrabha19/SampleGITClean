using InstallationManager.DataModel;
using InstallationManager.ToastNotificationLib;
using System;

namespace InstallationManager.ToastNotifier
{
    internal class MsgNotifier
    {
        public void DisplayObject(object sender, char[] receivedBytes)
        {
            string receivedObj = new string(receivedBytes);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            var receivedObject = new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom };

            Console.WriteLine($"Received Object:\r\nInstallation Status = {receivedObject.InstallationStatus}, Progress Value = {receivedObject.ProgressValue}, Overall Installation Status: {receivedObject.OInstallStatus}");

        }

        internal void DisplayToast(object sender, char[] receivedBytes)
        {
            string receivedObj = new string(receivedBytes);
            string[] properties = receivedObj.Split(',');

            OverallInstallationStatus enumCustom;
            Enum.TryParse<OverallInstallationStatus>(properties[3], out enumCustom);

            var receivedObject = new ReceivedObj { InstallationStatus = properties[0], ProgressValue = Double.Parse(properties[1]), ProgressValueStringOverride = properties[2], OInstallStatus = enumCustom };
        }
    }
}
