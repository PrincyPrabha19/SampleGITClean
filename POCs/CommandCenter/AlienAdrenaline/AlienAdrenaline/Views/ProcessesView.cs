using System;
using System.Collections.Generic;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Views
{
	public interface ProcessesView
	{
        SolidColorBrush Label1Foreground { get; set; }
        SolidColorBrush Label2Foreground { get; set; }
        int Label1ProcessCount { get; set; }
        int Label2ProcessCount { get; set; }
        int Label1CPUUsage { get; set; }
        int Label2CPUUsage { get; set; }

		event Action CopyDataToClipboard;

		void Close();
	    void SetData(List<ProcessData> data);
		void SetData(List<ProcessDataCompare> data);
	    bool? ShowDialog();
	}
}
