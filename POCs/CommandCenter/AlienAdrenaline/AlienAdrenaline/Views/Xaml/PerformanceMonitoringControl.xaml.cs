using AlienLabs.AlienAdrenaline.App.Controls;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Threading;

namespace AlienLabs.AlienAdrenaline.App.Views.Xaml
{
	public partial class PerformanceMonitoringControl : PerformanceMonitoringView
	{
		#region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Properties
		private MonitoringPresenter presenter;
		public MonitoringPresenter Presenter 
		{ 
			get
			{
				if (presenter is PerformanceMonitoringPresenter)
					return realTimeMonitoringPresenter;
				
				return snapshotViewPresenter;
			}

			set
			{
				presenter = value;
				if (presenter is PerformanceMonitoringPresenter)
					realTimeMonitoringPresenter = (PerformanceMonitoringPresenter) value;
				else
					snapshotViewPresenter = (SnapshotViewPresenter) value;
			}
		}

		public ViewType Type { get; private set; }

        private static readonly DependencyProperty timeElapsedSecondsProperty = DependencyProperty.Register("TimeElapsedSeconds", typeof(int), typeof(PerformanceMonitoringControl), new UIPropertyMetadata(0));
        public int TimeElapsedSeconds
        {
            get { return (int)GetValue(timeElapsedSecondsProperty); }
            set { SetValue(timeElapsedSecondsProperty, value); }
        }

		private PerformanceMonitoringPresenter realTimeMonitoringPresenter;
		private SnapshotViewPresenter snapshotViewPresenter;
        private readonly Timer recordingTimer = new Timer() { Interval = 300 };
        private readonly TimeRecordingHelper timeRecordingHelper = new TimeRecordingHelper();
        #endregion

		#region Constructor
		public PerformanceMonitoringControl()
		{
			InitializeComponent();
		}

		public PerformanceMonitoringControl(ViewType type) : this()
		{
			Type = type;
		    chartPagingControl.CPUChartProcessDataSelectionChanged += cpuChartProcessDataSelectionChanged;
            recordingTimer.Elapsed += recordingTimer_Elapsed;
		}
		#endregion

        #region Methods
        public void Refresh()
        {
        }

        #region Initialize Recording Charts
        public void InitializeCPUChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
	    {
            var cpuChart = new CPUChart(chartData1, chartData2, chartSeconds, isRealtimeChart);
            chartPagingControl.AddChart(cpuChart);
        }

        public void InitializeMemoryChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
	    {
            var memoryChart = new MemoryChart(chartData1, chartData2, chartSeconds, isRealtimeChart);
            chartPagingControl.AddChart(memoryChart);
	    }

        public void InitializeNetworkBytesReceivedChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var networkBytesReceivedChart = new NetworkBytesReceivedChart(chartData1, chartData2, chartSeconds, isRealtimeChart);
            networkBytesReceivedChart.MoreInfoChartsOpened += networkBytesReceivedChart_MoreInfoChartsOpened;
            networkBytesReceivedChart.MoreInfoChartsClosed += networkBytesReceivedChart_MoreInfoChartsClosed;
            chartPagingControl.AddChart(networkBytesReceivedChart);
        }

        public void InitializeNetworkBytesSentChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var networkBytesSentChart = new NetworkBytesSentChart(chartData1, chartData2, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            networkBytesSentChart.ChartClosed += networkChart_ChartClosed;
            chartPagingControl.AddChart(networkBytesSentChart);
        }        

        public void InitializeVideoGPUUsageChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var videoGPUUsageChart = new VideoGPUUsageChart(chartData1, chartData2, chartSeconds, isRealtimeChart);
            videoGPUUsageChart.MoreInfoChartsOpened += videoGPUUsageChart_MoreInfoChartsOpened;
            videoGPUUsageChart.MoreInfoChartsClosed += videoGPUUsageChart_MoreInfoChartsClosed;
            chartPagingControl.AddChart(videoGPUUsageChart);
        }

        public void InitializeVideoMemoryLoadChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var videoMemoryLoadChart = new VideoMemoryLoadChart(chartData1, chartData2, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoMemoryLoadChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoMemoryLoadChart);
        }

        public void InitializeVideoFanSpeedChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var videoFanSpeedChart = new VideoFanSpeedChart(chartData1, chartData2, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoFanSpeedChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoFanSpeedChart);
        }

        public void InitializeVideoTemperatureChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            var videoTemperatureChart = new VideoTemperatureChart(chartData1, chartData2, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoTemperatureChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoTemperatureChart);
        }
        #endregion

        #region Initialize Real-time Charts
        public void InitializeCPUChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
	    {
            var cpuChart = new CPUChart(monitorService, chartSeconds, isRealtimeChart);
            chartPagingControl.AddChart(cpuChart);
	    }

	    public void InitializeMemoryChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
	    {
            var memoryChart = new MemoryChart(monitorService, chartSeconds, isRealtimeChart);
            chartPagingControl.AddChart(memoryChart); 
	    }

        public void InitializeNetworkBytesReceivedChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var networkBytesReceivedChart = new NetworkBytesReceivedChart(monitorService, chartSeconds, isRealtimeChart);
            networkBytesReceivedChart.MoreInfoChartsOpened += networkBytesReceivedChart_MoreInfoChartsOpened;
            networkBytesReceivedChart.MoreInfoChartsClosed += networkBytesReceivedChart_MoreInfoChartsClosed;
            chartPagingControl.AddChart(networkBytesReceivedChart);
        }

        public void InitializeNetworkBytesSentChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var networkBytesSentChart = new NetworkBytesSentChart(monitorService, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            networkBytesSentChart.ChartClosed += networkChart_ChartClosed;
            chartPagingControl.AddChart(networkBytesSentChart);
        }

        public void InitializeVideoGPUUsageChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var videoGPUUsageChart = new VideoGPUUsageChart(monitorService, chartSeconds, isRealtimeChart);
            videoGPUUsageChart.MoreInfoChartsOpened += videoGPUUsageChart_MoreInfoChartsOpened;
            videoGPUUsageChart.MoreInfoChartsClosed += videoGPUUsageChart_MoreInfoChartsClosed;
            chartPagingControl.AddChart(videoGPUUsageChart);
        }

        public void InitializeVideoMemoryLoadChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var videoMemoryLoadChart = new VideoMemoryLoadChart(monitorService, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoMemoryLoadChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoMemoryLoadChart);
        }

        public void InitializeVideoFanSpeedChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var videoFanSpeedChart = new VideoFanSpeedChart(monitorService, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoFanSpeedChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoFanSpeedChart);
        }

        public void InitializeVideoTemperatureChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            var videoTemperatureChart = new VideoTemperatureChart(monitorService, chartSeconds, isRealtimeChart) { Visibility = Visibility.Collapsed };
            videoTemperatureChart.ChartClosed += videoChart_ChartClosed;
            chartPagingControl.AddChart(videoTemperatureChart);
        }
        #endregion

        public void RefreshChartsVisibility(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
        {
            chartPagingControl.RefreshChartsVisibility(categoriesToShow, selectedCategory);
        }

        public ComponentChartView GetChart<T>()
        {
            return chartPagingControl.GetChart<T>();
        }

        public void SetChartValues(ComponentChartView chart, List<ChartBufferData> values)
        {
            chartPagingControl.SetChartValues(chart, values);
        }

        public void SetChartValues(ComponentChartView chart, List<ChartBufferData> values1, List<ChartBufferData> values2)
        {
            chartPagingControl.SetChartValues(chart, values1, values2);
        }

	    public void UpdateChartControlLegend(string fileName1, int records1, string fileName2, int records2)
	    {
	        chartPagingControl.UpdateChartControlLegend(fileName1, records1, fileName2, records2);
	    }

	    public void ClearChartControl()
        {
            chartPagingControl.ClearChart();
        }

        public void UpdateRecordingStatus(bool recording)
        {
            if (!Dispatcher.CheckAccess())
        {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<bool>(UpdateRecordingStatus), recording);
                return;
            }

            if (recording)
                startRecordingTimer();
            else
                stopRecordingTimer();    
        }

        public void HideRecordingTimeElapsed()
        {
            textBoxTimeElapsed.Visibility = Visibility.Hidden;
        }

        private void updateTimeElapsed()
        {
            TimeElapsedSeconds = timeRecordingHelper.GetStartedSeconds();
        }

        private void startRecordingTimer()
        {
            textBoxTimeElapsed.Visibility = Visibility.Visible;
            timeRecordingHelper.Start();
            recordingTimer.Start();            
	    }

        private void stopRecordingTimer()
        {
            recordingTimer.Stop();
            updateTimeElapsed();
        }
	    #endregion

        #region Event Handlers
        private void networkBytesReceivedChart_MoreInfoChartsOpened(ComponentChartView chartView)
        {
            chartPagingControl.NetworkChartMoreInfoChartsOpened(chartView);
        }

        private void networkBytesReceivedChart_MoreInfoChartsClosed(ComponentChartView chartView)
        {
            chartPagingControl.NetworkChartMoreInfoChartsClosed(chartView);
        }

        private void networkChart_ChartClosed(ComponentChartView chartView)
        {
            chartPagingControl.NetworkChartChartClosed(chartView);
        }

        private void videoGPUUsageChart_MoreInfoChartsOpened(ComponentChartView chartView)
        {
            chartPagingControl.VideoChartMoreInfoChartsOpened(chartView);
        }

        private void videoGPUUsageChart_MoreInfoChartsClosed(ComponentChartView chartView)
        {
            chartPagingControl.VideoChartMoreInfoChartsClosed(chartView);
        }

        private void videoChart_ChartClosed(ComponentChartView chartView)
        {
            chartPagingControl.VideoChartChartClosed(chartView);
        }

        private void cpuChartProcessDataSelectionChanged(ExtendedPoint extendedPoint1, ExtendedPoint extendedPoint2)
        {
            snapshotViewPresenter.CPUChartProcessDataSelectionHasChanged(extendedPoint1, extendedPoint2);
        }

        private void recordingTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (Dispatcher.CheckAccess())
                updateTimeElapsed();
            else
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(updateTimeElapsed));
        }
        #endregion
    }
}








