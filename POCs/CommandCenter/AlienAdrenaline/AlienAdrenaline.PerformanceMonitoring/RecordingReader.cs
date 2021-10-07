using System;
using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface RecordingReader
	{
		List<ChartData> CPUInfo { get; }
		List<ChartData> MemoryInfo { get; }
		List<ChartData> NetworkBytesReceived { get; }
		List<ChartData> NetworkBytesSent { get; }
		List<ChartData> VideoGPUUsage { get; }
		List<ChartData> VideoTemperature { get; }
		List<ChartData> VideoMemoryLoad { get; }
		List<ChartData> VideoFanSpeed { get; }

		DateTime Started { get; }
		DateTime Ended { get; }
		long Records { get; }
        string FileName { get; }

		event Action HeaderWasRead;
		event Action ReadCompleted;
		event Action<int> ProgressChanged;
		event Action IncompatibleFileDetected;

		bool Load(string fileName);
	}
}