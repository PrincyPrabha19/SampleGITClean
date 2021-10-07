using System.Collections.Generic;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface HWInfo
	{
		List<ModifiableKeyValueData> Results { get; }
		string DataTitle { get; }
		double Maximum { get; }

		void Initialize();
		void Start();
		void Stop();
	}
}