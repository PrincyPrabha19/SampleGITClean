using InstallationManager.DataModel;
using System;

namespace InstallationManager.MessengerLib
{
    public class MessagePublisher
    {
        public event EventHandler<IToastUpdaterData> MessageEvent;

        public void OnMessageReceived(IToastUpdaterData toastData)
        {
            MessageEvent?.Invoke(this, toastData);
        }
    }
}
