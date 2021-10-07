using System;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class CategorySelectorPresenter
	{
		#region Properties
		public readonly CategorySelectorView View;
		#endregion

		#region Events
		public event Action<Visibility[], MonitoringCategories> SelectionChanged;
		#endregion

		#region Constructor
		public CategorySelectorPresenter(CategorySelectorView categorySelectorView)
		{
			View = categorySelectorView;
			View.SelectionChanged += selectionChanged;
		}
		#endregion

		#region Methods
		public void Show(Visibility[] categoriesToShow)
		{
			if (View == null)
				return;

			View.ShowOnly(categoriesToShow);
			View.Visibility = Visibility.Visible;
		}

		public void Hide()
		{
			if (View == null)
				return;

			View.Visibility = Visibility.Collapsed;
		}

		private void onSelectionChanged(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
		{
			if (SelectionChanged != null)
				SelectionChanged(categoriesToShow, selectedCategory);
		}

		public void UpdateInfo(string cpu, string memory, string network, string video)
		{
			View.UpdateInfo(cpu, memory, network, video);
		}
		#endregion

		#region Event Handlers
		private void selectionChanged(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
		{
			onSelectionChanged(categoriesToShow, selectedCategory);
		}
		#endregion
	}
}
