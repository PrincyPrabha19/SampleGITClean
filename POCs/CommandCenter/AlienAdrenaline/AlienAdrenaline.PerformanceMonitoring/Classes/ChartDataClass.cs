using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class ChartDataClass : ChartData
	{
		#region Properties
		public string ChartTitle { get; private set; }
		public double Maximum { get; private set; }
		public MonitoringCategoryInfo MonitoringCategory { get; private set; }
        public ObservableCollection<ExtendedPoint> ChartPoints { get; private set; }
		#endregion

		#region Constructor
		public ChartDataClass(string title, double maximum, MonitoringCategoryInfo categoryInfo)
		{
			ChartTitle = title;
			Maximum = maximum;
			MonitoringCategory = categoryInfo;
            ChartPoints = new ObservableCollection<ExtendedPoint>();
		}
		#endregion

		#region Methods
		public void Add(int value, List<ProcessData> extraData)
		{
			ChartPoints.Add(new ExtendedPointClass(ChartPoints.Count, value, extraData));
		}
		#endregion	
	}
}