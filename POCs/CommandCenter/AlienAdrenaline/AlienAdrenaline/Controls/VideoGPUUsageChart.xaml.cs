﻿using System;
using System.Collections.Generic;
using System.Linq;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class VideoGPUUsageChart
    {
        public override bool IsMainChart { get { return true; } }

        #region Constructor
        public VideoGPUUsageChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            InitializeComponent();

            ChartSeconds = chartSeconds;
            IsRealtimeChart = isRealtimeChart;
            MaximumY = monitorService.VideoMaximum;
            MinimumX = 0;
            MaximumX = chartSeconds;

            var infoResults = monitorService.VideoInfoResults.Where(x => x.CategoryInfo == MonitoringCategoryInfo.Video_GPUUsage).ToList();
            RefreshLayout(infoResults, panelCharts);
        }

        public VideoGPUUsageChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
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