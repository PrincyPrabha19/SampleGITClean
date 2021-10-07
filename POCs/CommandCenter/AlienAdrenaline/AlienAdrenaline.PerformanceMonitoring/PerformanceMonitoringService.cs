using System;
using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface PerformanceMonitoringService
	{
		List<ModifiableKeyValueData> CPUInfoResults { get; }
		List<ModifiableKeyValueData> MemoryInfoResults { get; }
		List<ModifiableKeyValueData> VideoInfoResults { get; }
		List<ModifiableKeyValueData> NetworkInfoResults { get; }

		string CPUDataTitle { get; }
		string MemoryTitle { get; }
		string VideoDataTitle { get; }
		string NetworkDataTitle { get; }

		double CPUMaximum { get; }
		double MemoryMaximum { get; }
		double VideoMaximum { get; }
		double NetworkMaximum { get; }

		bool Started { get; }

		event Action DataWasUpdated;

		void Start();
		void Stop();
	}
}