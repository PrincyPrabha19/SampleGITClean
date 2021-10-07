using System;
using System.Threading;
using AlienLabs.Tools.Classes;
namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers
{
    public class ConfigurableUIEventManagerClass : ConfigurableUIEventManager
    {
        #region Members
		private NamedEvent uiEvent;
		private readonly string uiNameEvent;

        private bool listening;
        private Thread eventsListener;
        private IntPtr[] events;
        #endregion

        #region Enums
        enum EventHandlerPosition : uint
        {
			RefreshUI
        }
        #endregion

		#region Events
    	public event Action RefreshUIHasArrived;
		#endregion

        #region Constructor
		public ConfigurableUIEventManagerClass(string eventName)
		{
			uiNameEvent = eventName;
        }
    	#endregion

        #region Methods
		public void StartMonitoring()
		{
			if (listening)
				return;

			listening = true;
			setupListenersForEvents();
		}

		public void StopMonitoring()
		{
			if (!listening)
				return;

			listening = false;
		}
		
		private void setupListenersForEvents()
        {
            events = new IntPtr[Enum.GetNames(typeof(EventHandlerPosition)).Length];

			if (uiEvent == null)
			{
				uiEvent = new NamedEvent(uiNameEvent, false, true);
				events[(uint)EventHandlerPosition.RefreshUI] = uiEvent.EventHnd;
			}

           eventsListener = new Thread(startListeningForEvents) { Name = "Recieving Events", IsBackground = true };
           eventsListener.Start();
        }

        private void startListeningForEvents()
        {
            while (listening)
            {
                uint obj = NamedEvent.WaitForMultipleObjects(events, 10, false);
				switch (obj)
                {
                	case (uint) EventHandlerPosition.RefreshUI:
						if (RefreshUIHasArrived != null)
							RefreshUIHasArrived();
                		break;
                }
            }
        }
        #endregion
    }
}