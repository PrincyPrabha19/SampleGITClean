using AlienLabs.AlienAdrenaline.App.Controls;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes.Helpers;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;
using UsageTacking.Domain;
using PerformanceMonitoringService = AlienLabs.AlienAdrenaline.PerformanceMonitoring.PerformanceMonitoringService;
using UserControl = System.Windows.Controls.UserControl;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class PerformanceMonitoringPresenter : MonitoringPresenter
    {
        #region Properties
        public PerformanceMonitoringView View { get; set; }
        public ViewRepository ViewRepository { get; set; }
        public CommandContainer CommandContainer { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private readonly PerformanceMonitoringService monitorService = PerfMonServiceFactory.Instance;
        private readonly PerformanceMonitoringBuffer monitorBuffer = PerfMonBufferFactory.NewPerformanceMonitoringBuffer();
        private ConfigurableUIEventManager refreshUIEventManager;
        private bool recording;

        public virtual bool IsDirty
        {
            get { return !CommandContainer.IsEmpty; }
        }
        #endregion

		#region Events
		public event Action<string, string, string, string> DataWasUpdated;
		#endregion

        #region Constructor
        public PerformanceMonitoringPresenter(PerformanceMonitoringView view)
        {
            View = view;
            initRefreshUIEventManager();
        }
        #endregion

        #region Public Methods
        public void Refresh()
        {
			if (View is UserControl)
				(View as UserControl).Dispatcher.Invoke(DispatcherPriority.Normal, new Action(initChartLayout));

	        recording = RTPMRecorderWrapper.IsRecording();
        }

        public void StartMonitoring()
        {
            if (monitorService == null || monitorService.Started)
                return;
            
            monitorService.DataWasUpdated += dataWasUpdated;
            monitorService.Start();
        }

        public void StopMonitoring()
        {
            if (recording || monitorService == null || !monitorService.Started)
                return;

            monitorService.DataWasUpdated -= dataWasUpdated;
            monitorService.Stop();			
        }

        public void StartRecording(string recordingName)
        {
            AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "PerformanceMonitoring", Features.PerformanceMonitoringGameModeStartRecording, DateTime.Now);
            if (monitorService == null || !monitorService.Started)
                return;

            RTPMRecorderWrapper.StartRecording(recordingName, "");
        }

        public void StopRecording()
        {
            RTPMRecorderWrapper.StopRecording();
        }
        #endregion

        #region Private Methods
        private void initRefreshUIEventManager()
        {
            refreshUIEventManager = new ConfigurableUIEventManagerClass("REFRESH_PM_PMP");
            refreshUIEventManager.RefreshUIHasArrived += refreshUIHasArrived;
            refreshUIEventManager.StartMonitoring();
        }

        private void initChartLayout()
        {            
            monitorBuffer.InitializeBuffer(monitorService, PerfMonBufferFactory.BUFFER_SIZE);

            View.ClearChartControl();

            var isRecording = RTPMRecorderWrapper.IsRecording();
            View.UpdateRecordingStatus(isRecording);
            if (!isRecording)
                View.HideRecordingTimeElapsed();

            View.InitializeCPUChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);
            View.InitializeMemoryChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);

            if (monitorService.NetworkInfoResults.Count > 0)
            {
                View.InitializeNetworkBytesReceivedChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);

                if (monitorBuffer.NetworkBytesSentBuffer.Count > 0)
                    View.InitializeNetworkBytesSentChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);
            }

            if (monitorService.VideoInfoResults.Count > 0)
            {
                View.InitializeVideoGPUUsageChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);

                if (monitorBuffer.VideoMemoryLoadBuffer.Count > 0)
                    View.InitializeVideoMemoryLoadChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);
                if (monitorBuffer.VideoFanSpeedBuffer.Count > 0)
                    View.InitializeVideoFanSpeedChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);
                if (monitorBuffer.VideoTemperatureBuffer.Count > 0)
                    View.InitializeVideoTemperatureChart(monitorService, PerfMonBufferFactory.BUFFER_SIZE, true);
            }
        }

        private void refreshData()
        {
            var view = View as UserControl;
            if (view == null)
                return;

            if (view.Dispatcher.CheckAccess())
                updateChart();
            else
                view.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(refreshData));
        }

	    private string getStringData(List<ModifiableKeyValueData> dataCollection, MonitoringCategoryInfo category, string format, string separator)
	    {
			if (dataCollection == null || dataCollection.Count <= 0)
			    return "";

		    string value = "";
			value = dataCollection.Where(data => data.CategoryInfo == category).Aggregate(value, (current, data) => current + string.Format(format + separator, Math.Round(data.Value, 0)));
		    if (!string.IsNullOrEmpty(separator) && value.EndsWith(separator))
				value = value.Substring(0, value.LastIndexOf(separator, StringComparison.InvariantCultureIgnoreCase));

		    return value;
	    }

	    private void updateChart()
	    {
		    const string separator = " | ";
			var cpuValue = "";
			var memoryValue = "";
			var networkValue = "";
			var videoValue = "";

			if (monitorService.CPUInfoResults != null && monitorService.CPUInfoResults.Count > 0)
				cpuValue = $"{Math.Round(monitorService.CPUInfoResults[0].Value)} %";
			if (monitorService.MemoryInfoResults != null && monitorService.MemoryInfoResults.Count > 0)
				memoryValue = string.Format(Properties.Resources.GBStr, monitorService.MemoryInfoResults[0].Value);
		    if (monitorService.NetworkInfoResults != null && monitorService.NetworkInfoResults.Count > 0)
				networkValue = getStringData(monitorService.NetworkInfoResults, MonitoringCategoryInfo.Network_BytesReceived, "{0} Kbps", separator);
			if (monitorService.VideoInfoResults != null && monitorService.VideoInfoResults.Count > 0)
				videoValue = getStringData(monitorService.VideoInfoResults, MonitoringCategoryInfo.Video_GPUUsage, "{0} %", separator);

			updateData(DataWasUpdated, cpuValue, memoryValue, networkValue, videoValue);

			monitorBuffer.UpdateCPUBuffer(monitorService.CPUInfoResults);
			monitorBuffer.UpdateMemoryBuffer(monitorService.MemoryInfoResults);
			monitorBuffer.UpdateNetworkBuffer(monitorService.NetworkInfoResults);
			monitorBuffer.UpdateVideoBuffer(monitorService.VideoInfoResults);

			ComponentChartView cpuChart =  null;
			ComponentChartView memoryChart = null;
			ComponentChartView bytesReceivedChart = null;
			ComponentChartView bytesSentChart = null;
			ComponentChartView gpuUsageChart = null;
			ComponentChartView videoTemperatureChart = null;
			ComponentChartView videoMemoryLoadChart = null;
			ComponentChartView videoFanSpeedChart = null;

	        if (monitorBuffer.CPUInfoBuffer.Count > 0)
		        cpuChart = View.GetChart<CPUChart>();

	        if (monitorBuffer.MemoryInfoBuffer.Count > 0)
		        memoryChart = View.GetChart<MemoryChart>();

	        if (monitorBuffer.NetworkBytesReceivedBuffer.Count > 0)
		        bytesReceivedChart = View.GetChart<NetworkBytesReceivedChart>();

	        if (monitorBuffer.NetworkBytesSentBuffer.Count > 0)
		        bytesSentChart = View.GetChart<NetworkBytesSentChart>();

	        if (monitorBuffer.VideoGPUUsageBuffer.Count > 0)
		        gpuUsageChart = View.GetChart<VideoGPUUsageChart>();

			if (monitorBuffer.VideoTemperatureBuffer.Count > 0)
		        videoTemperatureChart = View.GetChart<VideoTemperatureChart>();

	        if (monitorBuffer.VideoMemoryLoadBuffer.Count > 0)
		        videoMemoryLoadChart = View.GetChart<VideoMemoryLoadChart>();

	        if (monitorBuffer.VideoFanSpeedBuffer.Count > 0)
		        videoFanSpeedChart = View.GetChart<VideoFanSpeedChart>();

	        if (cpuChart != null)
				View.SetChartValues(cpuChart, monitorBuffer.CPUInfoBuffer);

			if (memoryChart != null)
				View.SetChartValues(memoryChart, monitorBuffer.MemoryInfoBuffer);

			if (bytesReceivedChart != null)
				View.SetChartValues(bytesReceivedChart, monitorBuffer.NetworkBytesReceivedBuffer);

			if (bytesSentChart != null)
				View.SetChartValues(bytesSentChart, monitorBuffer.NetworkBytesSentBuffer);

			if (gpuUsageChart != null)
				View.SetChartValues(gpuUsageChart, monitorBuffer.VideoGPUUsageBuffer);

			if (videoTemperatureChart != null)
				View.SetChartValues(videoTemperatureChart, monitorBuffer.VideoTemperatureBuffer);

			if (videoMemoryLoadChart != null)
				View.SetChartValues(videoMemoryLoadChart, monitorBuffer.VideoMemoryLoadBuffer);

            if (videoFanSpeedChart != null)
                View.SetChartValues(videoFanSpeedChart, monitorBuffer.VideoFanSpeedBuffer);
        }

        private void updateRecordingStatus()
        {
            recording = !recording;
            View.UpdateRecordingStatus(recording);
        }

	    private void updateData(Action<string, string, string, string> eventToCall, string cpu, string memory, string network, string videoValue)
	    {
		    if (eventToCall != null)
				eventToCall(cpu, memory, network, videoValue);
	    }
	    #endregion

        #region Event Handlers
        private void dataWasUpdated()
        {
            refreshData();
        }

        private void refreshUIHasArrived()
        {
            var tmpView = View as UserControl;
            if (tmpView == null)
                return;

            if (tmpView.Dispatcher.CheckAccess())
                updateRecordingStatus();
            else
                tmpView.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(updateRecordingStatus));
        }
        #endregion
    }
}