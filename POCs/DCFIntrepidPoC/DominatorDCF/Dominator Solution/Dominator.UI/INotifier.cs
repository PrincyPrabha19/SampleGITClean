using Dominator.UI.Classes.Enums;

namespace Dominator.UI
{
    public interface INotifier
    {
        NotifierResult Show(string title, string message);
        NotifierResult Show(string title, string message, NotifierIcon icon);
        NotifierResult Show(string title, string message, NotifierIcon icon, NotifierButtons buttons);
        NotifierResult Show(string title, string message, NotifierIcon icon, NotifierButtons buttons, NotifierDefaultButton defaultButton);
        int Show(string title, string message, NotifierIcon icon, string[] buttonText, NotifierDefaultButton defaultButton);
        int Show(string title, string message, NotifierIcon icon, string[] buttonText, NotifierDefaultButton defaultButton, bool isDoNotShowAgainVisible, out bool doNotShowAgain);
    }
}