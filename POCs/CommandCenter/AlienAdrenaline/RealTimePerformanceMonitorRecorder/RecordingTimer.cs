using System;

namespace RealTimePerformanceMonitorRecorder
{
	public interface RecordingTimer
	{
		void Start();
		void Stop();
	    event Action TimeOut;
	}
}