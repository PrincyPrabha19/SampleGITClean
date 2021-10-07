using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Abt.Controls.SciChart.Model.DataSeries;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes
{
	public class PerformanceMonitoringBufferClass : PerformanceMonitoringBuffer
    {
        #region PerformanceMonitoringBuffer Members
        public int NetworkCardsCount { get; private set; }
        public List<MonitoringCategoryInfo> NetworkCardsInfo { get; private set; }

        public int VideoCardsCount { get; private set; }
        public List<MonitoringCategoryInfo> VideoCardsInfo { get; private set; }

        private List<ChartBufferData> cpuInfoBuffer;
        public List<ChartBufferData> CPUInfoBuffer { get { return cpuInfoBuffer; } }

        private List<ChartBufferData> memoryInfoBuffer;
        public List<ChartBufferData> MemoryInfoBuffer { get { return memoryInfoBuffer; } }

        private List<ChartBufferData> networkBytesReceivedBuffer;
        public List<ChartBufferData> NetworkBytesReceivedBuffer { get { return networkBytesReceivedBuffer; } }

        private List<ChartBufferData> networkBytesSentBuffer;
        public List<ChartBufferData> NetworkBytesSentBuffer { get { return networkBytesSentBuffer; } }

        private List<ChartBufferData> videoGPUUsageBuffer;
        public List<ChartBufferData> VideoGPUUsageBuffer { get { return videoGPUUsageBuffer; } }

        private List<ChartBufferData> videoTemperatureBuffer;
		public List<ChartBufferData> VideoTemperatureBuffer { get { return videoTemperatureBuffer; } }

        private List<ChartBufferData> videoMemoryLoadBuffer;
        public List<ChartBufferData> VideoMemoryLoadBuffer { get { return videoMemoryLoadBuffer; } }

        private List<ChartBufferData> videoFanSpeedBuffer;
        public List<ChartBufferData> VideoFanSpeedBuffer { get { return videoFanSpeedBuffer; } }

	    public int BufferSize { get; set; }

        public void UpdateCPUBuffer(List<ModifiableKeyValueData> infoResults)
        {
            cpuInfoBuffer[0].UpdateChartPoints(infoResults[0].Value);
        }

        public void UpdateCPUBuffer(List<ChartData> cpuInfoResults)
        {
            cpuInfoBuffer[0].UpdateChartPoints(cpuInfoResults[0].ChartPoints);
        }

        public void UpdateMemoryBuffer(List<ModifiableKeyValueData> infoResults)
        {
            memoryInfoBuffer[0].UpdateChartPoints(infoResults[0].Value);
        }

        public void UpdateMemoryBuffer(List<ChartData> memoryInfoResults)
        {
            memoryInfoBuffer[0].UpdateChartPoints(memoryInfoResults[0].ChartPoints);
        }

        public void UpdateNetworkBuffer(List<ModifiableKeyValueData> infoResults)
        {
            int idx1=0, idx2=0;
            foreach (var info in infoResults)
            {
                switch (info.CategoryInfo)
                {
                    case MonitoringCategoryInfo.Network_BytesReceived:
                        networkBytesReceivedBuffer[idx1++].UpdateChartPoints(info.Value);
                        break;
                    case MonitoringCategoryInfo.Network_BytesSent:
                        networkBytesSentBuffer[idx2++].UpdateChartPoints(info.Value);
                        break;
                }
            }
        }

        public void UpdateNetworkBytesReceivedBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                networkBytesReceivedBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }

        public void UpdateNetworkBytesSentBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                networkBytesSentBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }

        public void UpdateVideoBuffer(List<ModifiableKeyValueData> infoResults)
        {
            int idx1=0, idx2=0, idx3=0, idx4=0;
            foreach (var info in infoResults)
            {
                switch (info.CategoryInfo)
                {
                    case MonitoringCategoryInfo.Video_GPUUsage:
                        videoGPUUsageBuffer[idx1++].UpdateChartPoints(info.Value);
                        break;
                    case MonitoringCategoryInfo.Video_Temperature:
                        videoTemperatureBuffer[idx2++].UpdateChartPoints(info.Value);
                        break;
                    case MonitoringCategoryInfo.Video_MemoryLoad:
                        videoMemoryLoadBuffer[idx3++].UpdateChartPoints(info.Value);
                        break;
                    case MonitoringCategoryInfo.Video_FanSpeed:
                        videoFanSpeedBuffer[idx4++].UpdateChartPoints(info.Value);
                        break;
                }
            }
        }

        public void UpdateVideoGPUUsageBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                videoGPUUsageBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }

		public void UpdateVideoTemperatureBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                videoTemperatureBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }

        public void UpdateVideoMemoryLoadBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                videoMemoryLoadBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }

        public void UpdateVideoFanSpeedBuffer(List<ChartData> infoResults)
        {
            int idx = 0;
            foreach (var info in infoResults)
                VideoFanSpeedBuffer[idx++].UpdateChartPoints(info.ChartPoints);
        }
	    #endregion

        #region Public Methods
        public void InitializeBuffer(PerformanceMonitoringService monitorService, int bufferSize)
	    {
            BufferSize = bufferSize;

            cpuInfoBuffer = new List<ChartBufferData>();
            memoryInfoBuffer = new List<ChartBufferData>();
            networkBytesReceivedBuffer = new List<ChartBufferData>();
            networkBytesSentBuffer = new List<ChartBufferData>();
            videoGPUUsageBuffer = new List<ChartBufferData>();
            videoTemperatureBuffer = new List<ChartBufferData>();
            videoMemoryLoadBuffer = new List<ChartBufferData>();
            videoFanSpeedBuffer = new List<ChartBufferData>();

            cpuInfoBuffer.Add(new ChartBufferData(bufferSize, true));
            memoryInfoBuffer.Add(new ChartBufferData(bufferSize, true));

            int count;
            NetworkCardsInfo = getCategoryInfo(monitorService.NetworkInfoResults, MonitoringCategoryInfo.Network_BytesReceived, out count);
            NetworkCardsCount = count;

            var networkInfoResults = monitorService.NetworkInfoResults;
            foreach (var info in networkInfoResults)
            {
                var charBufferData = new ChartBufferData(bufferSize, true) { ChartTitle = info.Key };
                switch (info.CategoryInfo)
                {
                    case MonitoringCategoryInfo.Network_BytesReceived:
                        networkBytesReceivedBuffer.Add(charBufferData);
                        break;
                    case MonitoringCategoryInfo.Network_BytesSent:
                        networkBytesSentBuffer.Add(charBufferData);
                        break;
                }
            }

            VideoCardsInfo = getCategoryInfo(monitorService.VideoInfoResults, MonitoringCategoryInfo.Video_GPUUsage, out count);
            VideoCardsCount = count;

            var videoInfoResults = monitorService.VideoInfoResults;
            foreach (var info in videoInfoResults)
            {
                var charBufferData = new ChartBufferData(bufferSize, true) { ChartTitle = info.Key };
                switch (info.CategoryInfo)
                {
                    case MonitoringCategoryInfo.Video_GPUUsage:
                        videoGPUUsageBuffer.Add(charBufferData);
                        break;
					case MonitoringCategoryInfo.Video_Temperature:
                        videoTemperatureBuffer.Add(charBufferData);
                        break;
                    case MonitoringCategoryInfo.Video_MemoryLoad:
                        videoMemoryLoadBuffer.Add(charBufferData);
                        break;
                    case MonitoringCategoryInfo.Video_FanSpeed:
                        videoFanSpeedBuffer.Add(charBufferData);
                        break;
                }
            } 
        }

        public void InitializeBuffer(RecordingReader recordingReader)
        {
            BufferSize = (int) recordingReader.Records;

            cpuInfoBuffer = new List<ChartBufferData>();
            memoryInfoBuffer = new List<ChartBufferData>();
            networkBytesReceivedBuffer = new List<ChartBufferData>();
            networkBytesSentBuffer = new List<ChartBufferData>();
            videoGPUUsageBuffer = new List<ChartBufferData>();
            videoTemperatureBuffer = new List<ChartBufferData>();
            videoMemoryLoadBuffer = new List<ChartBufferData>();
            videoFanSpeedBuffer = new List<ChartBufferData>();

            cpuInfoBuffer.Add(new ChartBufferData(BufferSize, false));
            memoryInfoBuffer.Add(new ChartBufferData(BufferSize, false));

            NetworkCardsCount = recordingReader.NetworkBytesReceived.Count;
            foreach (var info in recordingReader.NetworkBytesReceived)
                networkBytesReceivedBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });

            foreach (var info in recordingReader.NetworkBytesSent)
                networkBytesSentBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });

            VideoCardsCount = recordingReader.VideoGPUUsage.Count;
            foreach (var info in recordingReader.VideoGPUUsage)
                videoGPUUsageBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });

            foreach (var info in recordingReader.VideoTemperature)
                videoTemperatureBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });

            foreach (var info in recordingReader.VideoMemoryLoad)
                videoMemoryLoadBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });

            foreach (var info in recordingReader.VideoFanSpeed)
                videoFanSpeedBuffer.Add(new ChartBufferData(BufferSize, false) { ChartTitle = info.ChartTitle });
        }
	    #endregion

        #region Private Methods
        private List<MonitoringCategoryInfo> getCategoryInfo(List<ModifiableKeyValueData> infoResults, MonitoringCategoryInfo categoryInfo, out int count)
        {
            count = infoResults.Count(i => i.CategoryInfo == categoryInfo);
            var list = infoResults.GroupBy(i => i.CategoryInfo).Select(grp => grp.First().CategoryInfo).ToList();
            return list;
        }
	    #endregion
    }

    public class ChartBufferData
    {
        public string ChartTitle { get; set; }
        
        private ObservableCollection<ExtendedPoint> chartPoints;
        public ObservableCollection<ExtendedPoint> ChartPoints { get { return chartPoints; } }
        
        public int BufferSize { get; private set; }
        private int timeIndex = 0;

        public ChartBufferData(int bufferSize, bool initializeBuffer)
        {
            BufferSize = bufferSize;

            chartPoints = new ObservableCollection<ExtendedPoint>();
            if (initializeBuffer)
                for (; timeIndex < BufferSize + 1; timeIndex++)
                    chartPoints.Add(new ExtendedPointClass(timeIndex, -1000000, null));
        }
        
        public void UpdateChartPoints(double value)
        {
            chartPoints.Add(new ExtendedPointClass(timeIndex, value, null));
            if (chartPoints.Count >= BufferSize + 1)
                chartPoints.RemoveAt(0);

            timeIndex++;
        }

        public void UpdateChartPoints(ObservableCollection<ExtendedPoint> values)
        {
            chartPoints = values;
        }
    }
}
