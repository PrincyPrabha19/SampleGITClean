using System;
using Dominator.UI.Classes.Enums;
using Microsoft.Win32;

namespace Dominator.UI.Classes.Helpers
{
    public static class DisclaimerHelper
    {
        #region Methods
        public static bool IsDisclaimerAccepted()
        {
            bool acceptedAndSaved = readRegistryKeyValue("DisclaimerWasAccepted");
            if (acceptedAndSaved) return true;

            bool doNotShowAgain;
            var btnArr = new[] { Properties.Resources.Accept.ToUpper(), Properties.Resources.Cancel.ToUpper() };
            var disclaimer = string.Format(Properties.Resources.OCDisclaimer, Properties.Resources.ApplicationName);
            var result = new Notifier().Show(Properties.Resources.ApplicationName.ToUpper(), disclaimer, NotifierIcon.Question, btnArr, NotifierDefaultButton.FirstButton, true, out doNotShowAgain);

            if (result == 1) return false; // Cancel

            // Continue
            if (doNotShowAgain)
                writeRegistryKeyValue("DisclaimerWasAccepted", true);

            return true;
        }

        private static bool readRegistryKeyValue(string registryKey)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.OpenSubKey(@"SOFTWARE\Alienware\OC Controls", false))
                {
                    return key != null && Convert.ToBoolean(key.GetValue(registryKey, false));
                }
            }
            catch (Exception)
            {
                // ignored
            }

            return false;
        }

        private static void writeRegistryKeyValue(string registryKey, bool value)
        {
            try
            {
                using (var hklm = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64))
                using (var key = hklm.CreateSubKey(@"SOFTWARE\Alienware\OC Controls"))
                    key?.SetValue(registryKey, value);
            }
            catch (Exception)
            {
                // ignored
            }
        }
       #endregion
    }
}
