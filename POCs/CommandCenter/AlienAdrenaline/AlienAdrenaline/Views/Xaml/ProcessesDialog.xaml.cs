using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AlienLabs.AlienAdrenaline.App.Views.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class ProcessesDialog : ProcessesView
    {
        #region Properties
        private static readonly DependencyProperty label1ForegroundProperty = DependencyProperty.Register("Label1Foreground", typeof(SolidColorBrush), typeof(ProcessesDialog), new UIPropertyMetadata(Brushes.White));
        public SolidColorBrush Label1Foreground
        {
            get { return (SolidColorBrush)GetValue(label1ForegroundProperty); }
            set { SetValue(label1ForegroundProperty, value); }
        }

        private static readonly DependencyProperty label2ForegroundProperty = DependencyProperty.Register("Label2Foreground", typeof(SolidColorBrush), typeof(ProcessesDialog), new UIPropertyMetadata(Brushes.White));
        public SolidColorBrush Label2Foreground
        {
            get { return (SolidColorBrush)GetValue(label2ForegroundProperty); }
            set { SetValue(label2ForegroundProperty, value); }
        }

        private static readonly DependencyProperty label1ProcessCountProperty = DependencyProperty.Register("Label1ProcessCount", typeof(int), typeof(ProcessesDialog), new UIPropertyMetadata(0));
        public int Label1ProcessCount
        {
            get { return (int)GetValue(label1ProcessCountProperty); }
            set { SetValue(label1ProcessCountProperty, value); }
        }

        private static readonly DependencyProperty label2ProcessCountProperty = DependencyProperty.Register("Label2ProcessCount", typeof(int), typeof(ProcessesDialog), new UIPropertyMetadata(0));
        public int Label2ProcessCount
        {
            get { return (int)GetValue(label2ProcessCountProperty); }
            set { SetValue(label2ProcessCountProperty, value); }
        }

        private static readonly DependencyProperty label1CPUUsageProperty = DependencyProperty.Register("Label1CPUUsage", typeof(int), typeof(ProcessesDialog), new UIPropertyMetadata(0));
        public int Label1CPUUsage
        {
            get { return (int)GetValue(label1CPUUsageProperty); }
            set { SetValue(label1CPUUsageProperty, value); }
        }

        private static readonly DependencyProperty label2CPUUsageProperty = DependencyProperty.Register("Label2CPUUsage", typeof(int), typeof(ProcessesDialog), new UIPropertyMetadata(0));
        public int Label2CPUUsage
        {
            get { return (int)GetValue(label2CPUUsageProperty); }
            set { SetValue(label2CPUUsageProperty, value); }
        }

        private Size originalSize = new Size(550, 363);
        #endregion

		#region Events
		public event Action CopyDataToClipboard;
		#endregion

		#region Constructor
		public ProcessesDialog()
		{
			InitializeComponent();
		} 
		#endregion

		#region Methods
		public void ClearData()
		{
			listboxProcesses.ItemsSource = null;
		}

        public void SetData(List<ProcessData> data)
        {
            panelProcessCompareList.Visibility = Visibility.Collapsed;
            panelProcessList.Visibility = Visibility.Visible;

            listboxProcesses.ItemsSource = data;
            listboxProcesses.Visibility = Visibility.Visible;
        }

        public void SetData(List<ProcessDataCompare> data)
		{
            panelProcessList.Visibility = Visibility.Collapsed;
            panelProcessCompareList.Visibility = Visibility.Visible;          

            listboxProcessesCompare.ItemsSource = data;
            listboxProcessesCompare.Visibility = Visibility.Visible;
		}

        private void buttonClose_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

        private void window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
			if (!(e.OriginalSource is Image))
		       	DragMove();
        }

        private void window_Closing(object sender, CancelEventArgs e)
        {
            Closing -= window_Closing;
            e.Cancel = true;

            var animation = new DoubleAnimation(0, TimeSpan.FromMilliseconds(350));
            animation.Completed += (s, _) => Close();
            BeginAnimation(OpacityProperty, animation);
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
			if (CopyDataToClipboard != null)
				CopyDataToClipboard();
		}
		#endregion
    }
}
