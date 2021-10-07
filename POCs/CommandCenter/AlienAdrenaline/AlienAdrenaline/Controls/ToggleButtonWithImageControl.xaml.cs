using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
	public partial class ToggleButtonWithImageControl
	{
		#region Properties
		private static readonly DependencyProperty textProperty = DependencyProperty.Register("Text", typeof(string), typeof(ToggleButtonWithImageControl), new UIPropertyMetadata(""));
		public string Text
		{
			get { return (string)GetValue(textProperty); }
			set { SetValue(textProperty, value); }
		}

		private static readonly DependencyProperty imageProperty = DependencyProperty.Register("Image", typeof(ImageSource), typeof(ToggleButtonWithImageControl), new UIPropertyMetadata(null));
		public ImageSource Image
		{
			get { return (ImageSource)GetValue(imageProperty); }
			set { SetValue(imageProperty, value); }
		}

		private static readonly DependencyProperty isCheckedProperty = DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleButtonWithImageControl), new UIPropertyMetadata(false));
		public bool IsChecked
		{
			get { return (bool)GetValue(isCheckedProperty); }
			set { SetValue(isCheckedProperty, value); }
		}

		private static readonly DependencyProperty category = DependencyProperty.Register("Category", typeof(MonitoringCategories), typeof(ToggleButtonWithImageControl), new UIPropertyMetadata(MonitoringCategories.CPU));
		public MonitoringCategories Category
		{
			get { return (MonitoringCategories)GetValue(category); }
			set { SetValue(category, value); }
		}

        private static readonly DependencyProperty buttonTemplate = DependencyProperty.Register("ButtonTemplate", typeof(ControlTemplate), typeof(ToggleButtonWithImageControl));
        public ControlTemplate ButtonTemplate
        {
            get { return (ControlTemplate)GetValue(buttonTemplate); }
            set { SetValue(buttonTemplate, value); }
        }

		private bool canCallEvents = true;
		#endregion

		#region Events
		public event Func<ToggleButtonWithImageControl, bool> ButtonChecked;
		public event Func<ToggleButtonWithImageControl, bool> ButtonUnchecked;
		#endregion

		#region Constructor
		public ToggleButtonWithImageControl()
		{
			InitializeComponent();
		}

		private bool onSelectionChanged(Func<ToggleButtonWithImageControl, bool> eventToCall)
		{
			return eventToCall == null || eventToCall(this);
		}
		#endregion

		#region Event Handlers
		private void buttonChecked(object sender, RoutedEventArgs e)
		{
			if (!canCallEvents)
				return;

			onSelectionChanged(ButtonChecked);
		}

		private void buttonUnchecked(object sender, RoutedEventArgs e)
		{
			if (onSelectionChanged(ButtonUnchecked))
				return;

			canCallEvents = false;
			toggleButton.IsChecked = true;
			canCallEvents = true;
		}
		#endregion
	}
}