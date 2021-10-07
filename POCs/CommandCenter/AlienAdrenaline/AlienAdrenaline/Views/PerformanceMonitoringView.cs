using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface PerformanceMonitoringView : ContentView, INotifyPropertyChanged
    {
        MonitoringPresenter Presenter { get; set; }        

        void InitializeCPUChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);        
        void InitializeMemoryChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeNetworkBytesReceivedChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeNetworkBytesSentChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoGPUUsageChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoMemoryLoadChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoFanSpeedChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoTemperatureChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart);

        void InitializeCPUChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);
        void InitializeMemoryChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart); 
        void InitializeNetworkBytesReceivedChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);
        void InitializeNetworkBytesSentChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart); 
        void InitializeVideoGPUUsageChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoMemoryLoadChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoFanSpeedChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);
        void InitializeVideoTemperatureChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart);

        void RefreshChartsVisibility(Visibility[] categoriesToShow, MonitoringCategories selectedCategory);
        ComponentChartView GetChart<T>();
        void SetChartValues(ComponentChartView chart, List<ChartBufferData> values);
        void SetChartValues(ComponentChartView chart, List<ChartBufferData> values1, List<ChartBufferData> values2);
        void ClearChartControl();
        void UpdateChartControlLegend(string fileName1, int records1, string fileName2, int records2);
        void UpdateRecordingStatus(bool recording);
        void HideRecordingTimeElapsed();
    }
}
