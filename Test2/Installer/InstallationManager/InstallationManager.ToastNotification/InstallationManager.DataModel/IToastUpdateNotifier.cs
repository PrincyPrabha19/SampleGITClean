using System;

namespace InstallationManager.DataModel
{
    public interface IToastUpdateNotifier
    {
        event EventHandler<IToastUpdaterData> ToastUpdateReceived;

        void OnToastUpdateReceived();

        void UpdateListener();
    }
}
