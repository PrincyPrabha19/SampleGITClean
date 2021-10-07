using System;

namespace NotificationLauncher
{
    public interface IWindowsNotification
    {
        void Show();
        event Action NotificationResolved;
    }
}
