using System;
using System.Linq;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Controls;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class CategorySelectorControl : CategorySelectorView
	{
		#region Properties
		public ViewType Type { get { return ViewType.CategorySelector; } }
		private bool canCallEvents;
		private int checkedButtons;
		#endregion

		#region Events
		public event Action<Visibility[], MonitoringCategories> SelectionChanged;
		#endregion

		#region Constructor
		public CategorySelectorControl()
		{
			InitializeComponent();
		} 
		#endregion

		#region Methods
		public void ShowOnly(Visibility[] categoriesToShow)
		{
			buttonCPU.Visibility = categoriesToShow[(int)MonitoringCategories.CPU];
			buttonMemory.Visibility = categoriesToShow[(int)MonitoringCategories.Memory];
			buttonNetwork.Visibility = categoriesToShow[(int)MonitoringCategories.Network];
			buttonGPU.Visibility = categoriesToShow[(int)MonitoringCategories.GPU];

			canCallEvents = false;
			buttonCPU.IsChecked = buttonCPU.Visibility == Visibility.Visible;
			buttonMemory.IsChecked = buttonMemory.Visibility == Visibility.Visible;
			buttonNetwork.IsChecked = buttonNetwork.Visibility == Visibility.Visible;
			buttonGPU.IsChecked = buttonGPU.Visibility == Visibility.Visible;
			canCallEvents = true;

			checkedButtons = categoriesToShow.Count(x => x == Visibility.Visible);
		}

		public void UpdateInfo(string cpu, string memory, string network, string video)
		{
			buttonCPU.Tag = cpu;
			buttonMemory.Tag = memory;
			buttonNetwork.Tag = network;
			buttonGPU.Tag = video;
		}

		private void selectionWasChanged(MonitoringCategories selectedCategory)
		{
			var categoriesToShow = new[]
				{
					(buttonCPU.IsChecked) ? Visibility.Visible : Visibility.Collapsed,
					(buttonMemory.IsChecked) ? Visibility.Visible : Visibility.Collapsed,
					(buttonNetwork.IsChecked) ? Visibility.Visible : Visibility.Collapsed,
					(buttonGPU.IsChecked) ? Visibility.Visible : Visibility.Collapsed
				};

			onSelectionChanged(categoriesToShow, selectedCategory);
		}

		private void onSelectionChanged(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
		{
			if (SelectionChanged != null)
				SelectionChanged(categoriesToShow, selectedCategory);
		}
		#endregion

		#region Event Handlers
		private bool buttonChecked(ToggleButtonWithImageControl sender)
		{
			if (!canCallEvents)
				return true;

			checkedButtons++;
			selectionWasChanged(sender.Category);
			return true;
		}

		private bool buttonUnchecked(ToggleButtonWithImageControl sender)
		{
			if (!canCallEvents)
				return true;

            if (checkedButtons == 1)
                return false;

			checkedButtons--;
			selectionWasChanged(sender.Category);
			return true;
		}
		#endregion
	}
}
