using System;
using System.Windows;
using Dominator.UI.Classes.Enums;
using Dominator.UI.Views;

namespace Dominator.UI.Classes
{
    public class Notifier : INotifier
    {
        #region Methods
        public NotifierResult Show(string title, string message)
        {
            var notifier = new NotifierView(title, message) { ShowInTaskbar = false };
            notifier.ShowDialog();

            return notifier.ButtonClicked;
        }

        public NotifierResult Show(string title, string message, NotifierIcon icon)
        {
            var notifier = new NotifierView(title, message, icon) { ShowInTaskbar = false };
            notifier.ShowDialog();

            return notifier.ButtonClicked;
        }

        public NotifierResult Show(string title, string message, NotifierIcon icon, NotifierButtons buttons)
        {
            var notifier = new NotifierView(title, message,icon, buttons) { ShowInTaskbar = false };
            notifier.ShowDialog();

            return notifier.ButtonClicked;
        }

        public NotifierResult Show(string title, string message, NotifierIcon icon, NotifierButtons buttons, NotifierDefaultButton defaultButton)
        {
            var notifier = new NotifierView(title, message, icon, buttons, defaultButton) { ShowInTaskbar = false };
            notifier.ShowDialog();

            return notifier.ButtonClicked;
        }

        public int Show(string title, string message, NotifierIcon icon, string[] buttonText, NotifierDefaultButton defaultButton)
        {
            var notifier = new NotifierView(title, message, icon, buttonText, defaultButton) { ShowInTaskbar = false};
            notifier.ShowDialog();
            return notifier.CustomButtonClicked;
        }

        public int Show(string title, string message, NotifierIcon icon, string[] buttonText, NotifierDefaultButton defaultButton, bool isDoNotShowAgainVisible, out bool doNotShowAgain)
        {
            var notifier = new NotifierView(title, message, icon, buttonText, defaultButton) { ShowInTaskbar = false, DoNotShowAgainVisibility = isDoNotShowAgainVisible ? Visibility.Visible : Visibility.Collapsed };
            notifier.ShowDialog();
            doNotShowAgain = Convert.ToBoolean(notifier.checkBoxDoNotShowAgain.IsChecked);
            return notifier.CustomButtonClicked;
        }
        #endregion
    }
}
