using System;
using System.Threading;

namespace RealTimePerformanceMonitorRecorder.Classes
{
    public class RecordingEventManager
    {
        #region Members
		private NamedEvent stopRecordingEvent;

        private bool listening;
        private Thread eventsListener;
        private IntPtr[] events;

		private readonly string profilePrefix;
		private readonly string profileName;
		private readonly PerformanceMonitoringRecordingWrapper pmRecordingWrapper =  new PerformanceMonitoringRecordingWrapperClass();

		private readonly RecordingTimer timer = new RecordingTimerClass();
        #endregion

        #region Enums
        enum EventHandlerPosition : uint
        {
			StopRecording
        }
        #endregion

        #region Constructor
		public RecordingEventManager(string filename, string prefix)
		{
			profileName = filename;
			profilePrefix = prefix;
            timer.TimeOut += timer_TimeOut;

			startRecordingAndMonitoringForStop();
        }
    	#endregion

        #region Methods
        private void setupListenersForEvents()
        {
            events = new IntPtr[Enum.GetNames(typeof(EventHandlerPosition)).Length];

			if (stopRecordingEvent == null)
			{
				stopRecordingEvent = new NamedEvent("STOP_RECORDING", false, true);
				events[(uint)EventHandlerPosition.StopRecording] = stopRecordingEvent.EventHnd;
			}

           eventsListener = new Thread(startListeningForEvents) { Name = "Recieving Events" };
           eventsListener.Start();
        }

        private void startListeningForEvents()
        {
            while (listening)
            {
                uint uiEvent = NamedEvent.WaitForMultipleObjects(events, 10, false);

                switch (uiEvent)
                {
					case (uint)EventHandlerPosition.StopRecording:
                        {
                            processStopRecordingEvent();
                            break;
                        }
                }
            }
        }

        private void processStopRecordingEvent()
        {
            if (stopMonitoring())
            {
                pmRecordingWrapper.StopRecording();
                sendSignalToUIProcesses();
            }
        }

        private void startRecordingAndMonitoringForStop()
        {
            if (listening)
                return;

			pmRecordingWrapper.StartRecording(profileName, profilePrefix);
			sendSignalToUIProcesses();
			timer.Start();

			listening = true;
			setupListenersForEvents();
        }

		private bool stopMonitoring()
		{
			if (!listening)
				return false;

			timer.Stop();
			listening = false;
			return true;
		}

		private void sendSignalToUIProcesses()
		{
			var refreshGadget = new NamedEvent("REFRESH_GADGET", false, true);
			refreshGadget.Set();

			var refreshPMABP = new NamedEvent("REFRESH_PM_ABP", false, true);
			refreshPMABP.Set();

			var refreshPMPMP = new NamedEvent("REFRESH_PM_PMP", false, true);
			refreshPMPMP.Set();
		}
        #endregion

        #region Event Handlers
        private void timer_TimeOut()
        {
            processStopRecordingEvent();
        }
        #endregion
    }
}