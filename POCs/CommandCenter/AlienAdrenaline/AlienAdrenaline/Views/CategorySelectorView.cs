using System;
using System.Windows;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Views
{
	public interface CategorySelectorView : View
	{
		Visibility Visibility { get; set;}

		event Action<Visibility[], MonitoringCategories> SelectionChanged;

		void ShowOnly(Visibility[] categoriesToShow);
		void UpdateInfo(string cpu, string memory, string network, string video);
	}
}
