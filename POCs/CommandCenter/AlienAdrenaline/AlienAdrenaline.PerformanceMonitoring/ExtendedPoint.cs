using System.Collections.Generic;
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
	public interface ExtendedPoint
	{
		double X { get; }
		double Y { get; }
		List<ProcessData> ExtraData { get; }
        SolidColorBrush Color { get; set; }
	}
}