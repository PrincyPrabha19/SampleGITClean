using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using AlienLabs.AlienAdrenaline.App.Controls;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
	public class SnapshotViewPresenter : MonitoringPresenter
	{
        #region Properties
        public PerformanceMonitoringView View { get; set; }
        public ViewRepository ViewRepository { get; set; }      
        public CommandContainer CommandContainer { get; set; }
        public EventTrigger EventTrigger { get; set; }

		public virtual bool IsDirty
        {
            get { return !CommandContainer.IsEmpty; }
        }
                
	    private RecordingReader recordingReader1;
	    private RecordingReader recordingReader2;
	    private PerformanceMonitoringBuffer monitorBuffer1;
        private PerformanceMonitoringBuffer monitorBuffer2;
        private bool recordingReader1HeaderRead;
        private bool recordingReader2HeaderRead;
	    private bool recordingReader1ReadCompleted;
        private bool recordingReader2ReadCompleted;
	    private bool chartsInitialized;
        #endregion

		#region Events
        public event Action EOFReached;
		public event Action IncompatibleFileDetected;
        public Action<ExtendedPoint, ExtendedPoint> CPUChartProcessDataSelectionChanged;
		#endregion

        #region Methods
        public bool ReadSnapshot(List<string> fileNames)
		{
			if (View == null)
				return false;

            recordingReader1HeaderRead = false;
            recordingReader2HeaderRead = false;
            recordingReader1ReadCompleted = false;
            recordingReader2ReadCompleted = false;
            chartsInitialized = false;

            monitorBuffer1 = null;
            recordingReader1 = null;
            if (fileNames.Count >= 1 && !String.IsNullOrEmpty(fileNames[0]))
                recordingReader1 = new RecordingReaderClass();

            monitorBuffer2 = null;
            recordingReader2 = null;
            if (fileNames.Count >= 2 && !String.IsNullOrEmpty(fileNames[1]))
                recordingReader2 = new RecordingReaderClass();

            if (fileNames.Count >= 1 && !String.IsNullOrEmpty(fileNames[0]))
            {                
                recordingReader1.HeaderWasRead -= header1WasRead;
                recordingReader1.HeaderWasRead += header1WasRead;
                recordingReader1.ReadCompleted -= read1Completed;
                recordingReader1.ReadCompleted += read1Completed;
                recordingReader1.IncompatibleFileDetected -= incompatibleFileDetected;
                recordingReader1.IncompatibleFileDetected += incompatibleFileDetected;

			    AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringStartPlaying, DateTime.Now);
                if (!recordingReader1.Load(fileNames[0]))
                    return false;
            }

            if (fileNames.Count >= 2 && !String.IsNullOrEmpty(fileNames[1]))
            {                
                recordingReader2.HeaderWasRead += header2WasRead;
                recordingReader2.ReadCompleted -= read2Completed;
                recordingReader2.ReadCompleted += read2Completed;
                recordingReader2.IncompatibleFileDetected -= incompatibleFileDetected;
                recordingReader2.IncompatibleFileDetected += incompatibleFileDetected;

                AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringStartPlaying, DateTime.Now);
                if (!recordingReader2.Load(fileNames[1]))
                    return false;
            }

            return true;
		}

	    public bool AreSnapshotsComparable()
	    {
	        return true;
	    }

	    public Visibility[] GetCategoriesToShow()
		{
			var enumValues = Enum.GetValues(typeof(MonitoringCategories));
			var result = new Visibility[enumValues.Length];

			result[(int)MonitoringCategories.CPU] = Visibility.Visible;
			result[(int)MonitoringCategories.Memory] = Visibility.Visible;

            var supportsNetwork = recordingReader1.NetworkBytesReceived.Count != 0 || (recordingReader2 != null && recordingReader2.NetworkBytesReceived.Count != 0);
            result[(int)MonitoringCategories.Network] = supportsNetwork ? Visibility.Visible : Visibility.Collapsed;

            var supportsVideoCard = recordingReader1.VideoGPUUsage.Count != 0 || (recordingReader2 != null && recordingReader2.VideoGPUUsage.Count != 0);
			result[(int)MonitoringCategories.GPU] = supportsVideoCard ? Visibility.Visible : Visibility.Collapsed;

			return result;
		}

        public void NetworkMoreInfoChartsOpened()
        {
            //var chartSeconds = Math.Max((int)recordingReader1.Records, recordingReader2 != null ? (int)recordingReader2.Records : 0);
            //if (monitorBuffer1.NetworkBytesSentBuffer.Count > 0)
            //{
            //    View.ShowNetworkBytesSentChart(recordingReader1.NetworkBytesSent, recordingReader2 != null ? recordingReader2.NetworkBytesSent : null, chartSeconds, false);               
            //    updateNetworkBytesSentChart();
            //}
        }

        public void VideoMoreInfoChartsOpened()
        {
            //var chartSeconds = Math.Max((int)recordingReader1.Records, recordingReader2 != null ? (int)recordingReader2.Records : 0);

            //if (monitorBuffer1.VideoFanSpeedBuffer.Count > 0 || 
            //    (monitorBuffer2 != null && monitorBuffer2.VideoFanSpeedBuffer.Count > 0))
            //{
            //    View.ShowVideoFanSpeedChart(recordingReader1.VideoFanSpeed, recordingReader2 != null ? recordingReader2.VideoFanSpeed : null, chartSeconds, false);
            //    updateVideoFanSpeedChart();
            //}

            //if (monitorBuffer1.VideoMemoryLoadBuffer.Count > 0 ||
            //    (monitorBuffer2 != null && monitorBuffer2.VideoMemoryLoadBuffer.Count > 0))
            //{
            //    View.ShowVideoMemoryLoadChart(recordingReader1.VideoMemoryLoad, recordingReader2 != null ? recordingReader2.VideoMemoryLoad : null, chartSeconds, false);
            //    updateVideoMemoryLoadChart();
            //}

            //if (monitorBuffer1.VideoTemperatureBuffer.Count > 0 ||
            //    (monitorBuffer2 != null && monitorBuffer2.VideoTemperatureBuffer.Count > 0))
            //{
            //    View.ShowVideoTemperatureChart(recordingReader1.VideoTemperature, recordingReader2 != null ? recordingReader2.VideoTemperature : null, chartSeconds, false);
            //    updateVideoTemperatureChart();
            //}
        }

        public void CPUChartProcessDataSelectionHasChanged(ExtendedPoint extendedPoint1, ExtendedPoint extendedPoint2)
        {
            if (CPUChartProcessDataSelectionChanged != null)
                CPUChartProcessDataSelectionChanged(extendedPoint1, extendedPoint2);
        }

		private void callMethodWithIntParameter(Action<int> method, int parameter)
		{
			if (method == null)
				return;

			var view = View as UserControl;
			if (view == null)
				return;

			if (view.Dispatcher.CheckAccess())
				method(parameter);
			else
				view.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action<int>(method), parameter);
		}

		private void callMethod(Action method)
		{
			if (method == null)
				return;

			var view = View as UserControl;
			if (view == null)
				return;

			if (view.Dispatcher.CheckAccess())
				method();
			else
				view.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(method));
		}

        private void initChartLayout()
		{                       
            View.ClearChartControl();
            if (recordingReader1 != null && recordingReader2 != null)
                View.UpdateChartControlLegend(recordingReader1.FileName, (int)recordingReader1.Records, recordingReader2.FileName, (int)recordingReader2.Records);

            var chartSeconds = Math.Max((int) recordingReader1.Records, recordingReader2 != null ? (int) recordingReader2.Records : 0);

            View.InitializeCPUChart(recordingReader1.CPUInfo, recordingReader2 != null ? recordingReader2.CPUInfo : null, chartSeconds, false);
            View.InitializeMemoryChart(recordingReader1.MemoryInfo, recordingReader2 != null ? recordingReader2.MemoryInfo : null, chartSeconds, false);
            
            if (recordingReader1.NetworkBytesReceived.Count > 0 ||
                (recordingReader2 != null && recordingReader2.NetworkBytesReceived.Count > 0))
                View.InitializeNetworkBytesReceivedChart(recordingReader1.NetworkBytesReceived, recordingReader2 != null ? recordingReader2.NetworkBytesReceived : null, chartSeconds, false);

            if (recordingReader1.NetworkBytesSent.Count > 0 ||
                (recordingReader2 != null && recordingReader2.NetworkBytesSent.Count > 0))
                View.InitializeNetworkBytesSentChart(recordingReader1.NetworkBytesSent, recordingReader2 != null ? recordingReader2.NetworkBytesSent : null, chartSeconds, false);

            if (recordingReader1.VideoGPUUsage.Count > 0 ||
                (recordingReader2 != null && recordingReader2.VideoGPUUsage.Count > 0))
                View.InitializeVideoGPUUsageChart(recordingReader1.VideoGPUUsage, recordingReader2 != null ? recordingReader2.VideoGPUUsage : null, chartSeconds, false);

            if (recordingReader1.VideoMemoryLoad.Count > 0 ||
                (recordingReader2 != null && recordingReader2.VideoMemoryLoad.Count > 0))
                View.InitializeVideoMemoryLoadChart(recordingReader1.VideoMemoryLoad, recordingReader2 != null ? recordingReader2.VideoMemoryLoad : null, chartSeconds, false);

            if (recordingReader1.VideoFanSpeed.Count > 0 ||
                (recordingReader2 != null && recordingReader2.VideoFanSpeed.Count > 0))
                View.InitializeVideoFanSpeedChart(recordingReader1.VideoFanSpeed, recordingReader2 != null ? recordingReader2.VideoFanSpeed : null, chartSeconds, false);

            if (recordingReader1.VideoTemperature.Count > 0 ||
                (recordingReader2 != null && recordingReader2.VideoTemperature.Count > 0))
                View.InitializeVideoTemperatureChart(recordingReader1.VideoTemperature, recordingReader2 != null ? recordingReader2.VideoTemperature : null, chartSeconds, false);
		}

		private void loadDataInView()
		{
            updateCPUChart();
            updateMemoryChart();
            updateNetworkBytesReceivedChart();
            updateNetworkBytesSentChart();            
            updateVideoGPUUsageChart();
            updateVideoMemoryLoadChart();
            updateVideoFanSpeedChart();
            updateVideoTemperatureChart();
		}

        private void updateCPUChart()
        {
            monitorBuffer1.UpdateCPUBuffer(recordingReader1.CPUInfo);

            if (monitorBuffer1.CPUInfoBuffer.Count > 0)
            {
                var chart = View.GetChart<CPUChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null)
                    {
                        monitorBuffer2.UpdateCPUBuffer(recordingReader2.CPUInfo);
                        View.SetChartValues(chart, monitorBuffer1.CPUInfoBuffer, monitorBuffer2.CPUInfoBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.CPUInfoBuffer);
                }
            }
        }

        private void updateMemoryChart()
        {
            monitorBuffer1.UpdateMemoryBuffer(recordingReader1.MemoryInfo);

            if (monitorBuffer1.MemoryInfoBuffer.Count > 0)
            {
                var chart = View.GetChart<MemoryChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null)
                    {
                        monitorBuffer2.UpdateMemoryBuffer(recordingReader2.MemoryInfo);
                        View.SetChartValues(chart, monitorBuffer1.MemoryInfoBuffer, monitorBuffer2.MemoryInfoBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.MemoryInfoBuffer);
                }
            }
        }

        private void updateNetworkBytesReceivedChart()
        {
            monitorBuffer1.UpdateNetworkBytesReceivedBuffer(recordingReader1.NetworkBytesReceived);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateNetworkBytesReceivedBuffer(recordingReader2.NetworkBytesReceived);

            if (monitorBuffer1.NetworkBytesReceivedBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.NetworkBytesReceivedBuffer.Count > 0))
            {
                var chart = View.GetChart<NetworkBytesReceivedChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.NetworkBytesReceivedBuffer, monitorBuffer2.NetworkBytesReceivedBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.NetworkBytesReceivedBuffer);
                }
            }
        }

        private void updateNetworkBytesSentChart()
        {
            monitorBuffer1.UpdateNetworkBytesSentBuffer(recordingReader1.NetworkBytesSent);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateNetworkBytesSentBuffer(recordingReader2.NetworkBytesSent);

            if (monitorBuffer1.NetworkBytesSentBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.NetworkBytesSentBuffer.Count > 0))
            {
                var chart = View.GetChart<NetworkBytesSentChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.NetworkBytesSentBuffer, monitorBuffer2.NetworkBytesSentBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.NetworkBytesSentBuffer);
                }
            }
        }

        private void updateVideoGPUUsageChart()
        {
            monitorBuffer1.UpdateVideoGPUUsageBuffer(recordingReader1.VideoGPUUsage);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateVideoGPUUsageBuffer(recordingReader2.VideoGPUUsage);

            if (monitorBuffer1.VideoGPUUsageBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.VideoGPUUsageBuffer.Count > 0))
            {
                var chart = View.GetChart<VideoGPUUsageChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.VideoGPUUsageBuffer, monitorBuffer2.VideoGPUUsageBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.VideoGPUUsageBuffer);
                }
            }
        }

        private void updateVideoTemperatureChart()
        {
			monitorBuffer1.UpdateVideoTemperatureBuffer(recordingReader1.VideoTemperature);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateVideoTemperatureBuffer(recordingReader2.VideoTemperature);

            if (monitorBuffer1.VideoTemperatureBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.VideoTemperatureBuffer.Count > 0))
            {
                var chart = View.GetChart<VideoTemperatureChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.VideoTemperatureBuffer, monitorBuffer2.VideoTemperatureBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.VideoTemperatureBuffer);
                }
            }
        }

        private void updateVideoMemoryLoadChart()
        {
            monitorBuffer1.UpdateVideoMemoryLoadBuffer(recordingReader1.VideoMemoryLoad);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateVideoMemoryLoadBuffer(recordingReader2.VideoMemoryLoad);

            if (monitorBuffer1.VideoMemoryLoadBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.VideoMemoryLoadBuffer.Count > 0))
            {
                var chart = View.GetChart<VideoMemoryLoadChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.VideoMemoryLoadBuffer, monitorBuffer2.VideoMemoryLoadBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.VideoMemoryLoadBuffer);
                }
            }
        }

        private void updateVideoFanSpeedChart()
        {
            monitorBuffer1.UpdateVideoFanSpeedBuffer(recordingReader1.VideoFanSpeed);
            if (recordingReader2 != null)
                monitorBuffer2.UpdateVideoFanSpeedBuffer(recordingReader2.VideoFanSpeed);

            if (monitorBuffer1.VideoFanSpeedBuffer.Count > 0 || (
                monitorBuffer2 != null && monitorBuffer2.VideoFanSpeedBuffer.Count > 0))
            {
                var chart = View.GetChart<VideoFanSpeedChart>();
                if (chart != null)
                {
                    if (recordingReader2 != null && monitorBuffer2 != null) {
                        View.SetChartValues(chart, monitorBuffer1.VideoFanSpeedBuffer, monitorBuffer2.VideoFanSpeedBuffer);
                        return;
                    }

                    View.SetChartValues(chart, monitorBuffer1.VideoFanSpeedBuffer);
                }
            }
        }
		#endregion

		#region Event Handlers
		private void header1WasRead()
		{
		    recordingReader1HeaderRead = true;
		    monitorBuffer1 = PerfMonBufferFactory.NewPerformanceMonitoringBuffer();
            monitorBuffer1.InitializeBuffer(recordingReader1);

            if (!chartsInitialized && 
                (recordingReader2 == null || recordingReader2HeaderRead))
            {
                callMethod(initChartLayout);
                chartsInitialized = true;
            }
		}

        private void header2WasRead()
        {
            recordingReader2HeaderRead = true;
            monitorBuffer2 = PerfMonBufferFactory.NewPerformanceMonitoringBuffer();
            monitorBuffer2.InitializeBuffer(recordingReader2);

            if (!chartsInitialized && 
                (recordingReader1 == null || recordingReader1HeaderRead))
            {
                callMethod(initChartLayout);
                chartsInitialized = true;
            }
        }

        private void read1Completed()
        {
            recordingReader1ReadCompleted = true;
            if (recordingReader2 == null || recordingReader2ReadCompleted)
                callMethod(loadDataInView);
        }

        private void read2Completed()
        {
            recordingReader2ReadCompleted = true;
            if (recordingReader1 == null || recordingReader2ReadCompleted)
                callMethod(loadDataInView);
        }

		private void incompatibleFileDetected()
		{
			if (IncompatibleFileDetected != null)
				IncompatibleFileDetected();
		}
		#endregion
	}
}