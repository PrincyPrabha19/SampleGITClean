using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface ChartData
	{
		string ChartTitle { get; }
		double Maximum { get; }
		MonitoringCategoryInfo MonitoringCategory { get; }
        ObservableCollection<ExtendedPoint> ChartPoints { get; }

		void Add(int value, List<ProcessData> extraData);
	}
}