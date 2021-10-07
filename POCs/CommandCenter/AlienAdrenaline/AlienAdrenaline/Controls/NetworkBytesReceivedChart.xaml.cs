using System;
using System.Collections.Generic;
using System.Linq;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class NetworkBytesReceivedChart
    {
        public override bool IsMainChart { get { return true; } }

        #region Constructor
        public NetworkBytesReceivedChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            InitializeComponent();

            ChartSeconds = chartSeconds;
            IsRealtimeChart = isRealtimeChart;
            MaximumY = monitorService.NetworkMaximum;
            MinimumX = 0;
            MaximumX = chartSeconds;

            var infoResults = monitorService.NetworkInfoResults.Where(x => x.CategoryInfo == MonitoringCategoryInfo.Network_BytesReceived).ToList();
            RefreshLayout(infoResults, panelCharts);
        }

        public NetworkBytesReceivedChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            InitializeComponent();

            ChartSeconds = chartSeconds;
            IsRealtimeChart = isRealtimeChart;
            MaximumY = Math.Max(chartData1 != null && chartData1.Count > 0 ? chartData1[0].Maximum : 0, chartData2 != null && chartData2.Count > 0 ? chartData2[0].Maximum : 0);
            MinimumX = 0;
            MaximumX = chartSeconds;

            RefreshLayout(chartData1, chartData2, panelCharts);
        }
        #endregion
    }
}
