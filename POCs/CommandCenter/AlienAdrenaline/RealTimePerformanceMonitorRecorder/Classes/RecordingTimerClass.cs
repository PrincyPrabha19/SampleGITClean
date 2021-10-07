using System;
using System.Threading;

namespace RealTimePerformanceMonitorRecorder.Classes
{
	public class RecordingTimerClass : RecordingTimer
	{
		#region Properties
		private readonly AutoResetEvent wait = new AutoResetEvent(false);
	    private const int maximumTimeMilliseconds = 14400000; // 4hrs * 60mins * 60secs * 1000 milliseconds

		private bool started;
		private bool callEvent;
		#endregion

	    #region Events
	    public event Action TimeOut;
	    #endregion

		#region Methods
		public void Start()
		{
			if (started)
				return;

			started = true;
			callEvent = true;
			var t = new Thread(sleep) {IsBackground = true};
			t.Start(maximumTimeMilliseconds);
		}

		public void Stop()
		{
			if (!started)
				return;

			callEvent = false;
			wait.Set();
			started = false;
		}

		private void sleep(object time)
		{
			var milliseconds = (int)time;

			wait.Reset();
			wait.WaitOne(milliseconds);
			if (callEvent && TimeOut != null)
				TimeOut();
		}
		#endregion
	}
}
