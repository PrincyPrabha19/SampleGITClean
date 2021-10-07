
using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.PerformanceMonitoring
{
    public interface PerformanceMonitoringBuffer
    {
        int BufferSize { get; set; }
        int NetworkCardsCount { get; }
        List<MonitoringCategoryInfo> NetworkCardsInfo { get; }
        int VideoCardsCount { get; }
        List<MonitoringCategoryInfo> VideoCardsInfo { get; }

        List<ChartBufferData> CPUInfoBuffer { get; }
        List<ChartBufferData> MemoryInfoBuffer { get; }
        List<ChartBufferData> NetworkBytesReceivedBuffer { get; }
        List<ChartBufferData> NetworkBytesSentBuffer { get; }
        List<ChartBufferData> VideoGPUUsageBuffer { get; }
        List<ChartBufferData> VideoTemperatureBuffer { get; }
        List<ChartBufferData> VideoMemoryLoadBuffer { get; }
        List<ChartBufferData> VideoFanSpeedBuffer { get; }

        void UpdateCPUBuffer(List<ModifiableKeyValueData> infoResults);
        void UpdateCPUBuffer(List<ChartData> infoResults);

        void UpdateMemoryBuffer(List<ModifiableKeyValueData> infoResults);
        void UpdateMemoryBuffer(List<ChartData> infoResults);

        void UpdateNetworkBuffer(List<ModifiableKeyValueData> infoResults);
        void UpdateNetworkBytesReceivedBuffer(List<ChartData> infoResults);
        void UpdateNetworkBytesSentBuffer(List<ChartData> infoResults);

        void UpdateVideoBuffer(List<ModifiableKeyValueData> infoResults);
        void UpdateVideoGPUUsageBuffer(List<ChartData> infoResults);
        void UpdateVideoTemperatureBuffer(List<ChartData> infoResults);
        void UpdateVideoMemoryLoadBuffer(List<ChartData> infoResults);
        void UpdateVideoFanSpeedBuffer(List<ChartData> infoResults);

        void InitializeBuffer(PerformanceMonitoringService monitorService, int bufferSize);
        void InitializeBuffer(RecordingReader recordingReader);
    }
}
