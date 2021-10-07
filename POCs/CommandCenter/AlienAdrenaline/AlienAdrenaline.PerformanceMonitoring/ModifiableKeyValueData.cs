using System.Collections.Generic;
using System.ComponentModel;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface ModifiableKeyValueData : INotifyPropertyChanged
	{
		string Key { get; }
		double Value { get; set; }
		int Index { get; set; }
		List<ProcessData> ExtraData { get; set; }
		MonitoringCategoryInfo CategoryInfo { get; }
	}
}