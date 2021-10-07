using System.Collections.Generic;
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class ExtendedPointClass : ExtendedPoint
	{
		public double X { get; private set; }
		public double Y { get; private set; }
		public List<ProcessData> ExtraData { get; private set; }
        public SolidColorBrush Color { get; set; }

		public ExtendedPointClass(double x, double y, List<ProcessData> extraData)
		{
			X = x;
			Y = y;
			ExtraData = extraData;
		}
	}
}