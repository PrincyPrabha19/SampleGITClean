using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class MemoryChart : ComponentChartView
    {
        #region Properties
        public event Action<double, double> VisibleRangeChanged;
        public event Action<ComponentChartView, bool> ChartMaximizedMinimized;
        public event Action<string> ChartStatusChanged;

        public bool IsMaximized { get; set; }
        public bool IsMainChart { get { return true; } }

        private static readonly DependencyProperty isChartTitleEnabledProperty = DependencyProperty.Register("IsChartTitleEnabled", typeof(bool), typeof(MemoryChart), new UIPropertyMetadata(true));
        public bool IsChartTitleEnabled
        {
            get { return (bool)GetValue(isChartTitleEnabledProperty); }
            set { SetValue(isChartTitleEnabledProperty, value); }
        }

        private static readonly DependencyProperty chartTitleProperty = DependencyProperty.Register("ChartTitle", typeof(string), typeof(MemoryChart));
        public string ChartTitle
        {
            get { return (string)GetValue(chartTitleProperty); }
            set { SetValue(chartTitleProperty, value); }
        }

        private static readonly DependencyProperty IsRealtimeChartProperty = DependencyProperty.Register("IsRealtimeChart", typeof(bool), typeof(MemoryChart));
        public bool IsRealtimeChart
        {
            get { return (bool)GetValue(IsRealtimeChartProperty); }
            set { SetValue(IsRealtimeChartProperty, value); }
        }

        private static readonly DependencyProperty minimumXTextProperty = DependencyProperty.Register("MinimumXText", typeof(string), typeof(MemoryChart));
        public string MinimumXText
        {
            get { return (string)GetValue(minimumXTextProperty); }
            set { SetValue(minimumXTextProperty, value); }
        }

        private static readonly DependencyProperty maximumXTextProperty = DependencyProperty.Register("MaximumXText", typeof(string), typeof(MemoryChart));
        public string MaximumXText
        {
            get { return (string)GetValue(maximumXTextProperty); }
            set { SetValue(maximumXTextProperty, value); }
        }

        private static readonly DependencyProperty maximumYTextProperty = DependencyProperty.Register("MaximumYText", typeof(string), typeof(MemoryChart));
        public string MaximumYText
        {
            get { return (string)GetValue(maximumYTextProperty); }
            set { SetValue(maximumYTextProperty, value); }
        }

        private int chartSeconds;
        public int ChartSeconds
        {
            get { return chartSeconds; }
            set { chartSeconds = value; lineChart.ChartSeconds = chartSeconds; }
        }

        private double intervalX;
        public double IntervalX
        {
            get { return intervalX; }
            set { intervalX = value; lineChart.IntervalX = intervalX; }
        }

        private double intervalY;
        public double IntervalY
        {
            get { return intervalY; }
            set { intervalY = value; lineChart.IntervalY = intervalY; }
        }

        private double minimumX;
        public double MinimumX
        {
            get { return minimumX; }
            set
            {
                minimumX = value;
                //lineChart.MinimumX = minimumX;

                if (IsRealtimeChart)
                    MaximumXText = String.Format("{0}s", minimumX);
                else
                    MinimumXText = TimeSpanHelper.ConvertTotalSecondsToHourMinutesSeconds((int)minimumX);
            }
        }

        private double maximumX;
        public double MaximumX
        {
            get { return maximumX; }
            set
            {
                maximumX = value;
                //lineChart.MaximumX = maximumX;

                if (IsRealtimeChart)
                    MinimumXText = String.Format("{0}s", maximumX);
                else
                    MaximumXText = TimeSpanHelper.ConvertTotalSecondsToHourMinutesSeconds((int)maximumX);
            }
        }

        private double minimumY;
        public double MinimumY
        {
            get { return minimumY; }
            set { minimumY = value; lineChart.MinimumY = minimumY; }
        }

        private double maximumY;
        public double MaximumY
        {
            get { return maximumY; }
            set { maximumY = value; MaximumYText = String.Format("{0} GB", maximumY); lineChart.MaximumY = maximumY; }
        }

        private SolidColorBrush lineSeries1Color;
        public SolidColorBrush LineSeries1Color
        {
            get { return lineSeries1Color; }
            set { lineSeries1Color = value; lineChart.LineSeries1Color = value; }
        }

        private SolidColorBrush lineSeries2Color;
        public SolidColorBrush LineSeries2Color
        {
            get { return lineSeries2Color; }
            set { lineSeries2Color = value; lineChart.LineSeries2Color = value; }
        }
        #endregion

        #region Constructor
        public MemoryChart(PerformanceMonitoringService monitorService, int chartSeconds, bool isRealtimeChart)
        {
            InitializeComponent();
            
            ChartSeconds = chartSeconds;
            IsRealtimeChart = isRealtimeChart;
            MaximumY = monitorService.MemoryMaximum;
            MinimumX = 0;
            MaximumX = chartSeconds;
            ChartTitle = monitorService.MemoryTitle;

            RefreshLayout();
        }

        public MemoryChart(List<ChartData> chartData1, List<ChartData> chartData2, int chartSeconds, bool isRealtimeChart)
        {
            InitializeComponent();

            ChartSeconds = chartSeconds;
            IsRealtimeChart = isRealtimeChart;
            MaximumY = Math.Max(chartData1[0].Maximum, chartData2 != null ? chartData2[0].Maximum : 0);
            MinimumX = 0;
            MaximumX = chartSeconds;
            ChartTitle = mergeTitles(chartData1, chartData2, 0);

            lineChart.VisibleRangeChanged += visibleRangeChanged;

            RefreshLayout();
        }
        #endregion

        #region Public Methods
        public void SetChartValues(List<ChartBufferData> values)
        {
            lineChart.SetDataContext(values[0].ChartPoints);
        }

        public void SetChartValues(List<ChartBufferData> values1, List<ChartBufferData> values2)
        {
            lineChart.SetDataContext(values1[0].ChartPoints, values2[0].ChartPoints);
        }

        public void RefreshLayout()
        {
            lineChart.IsRealtimeChart = IsRealtimeChart;
            lineChart.IntervalY = MaximumY / 10;
            lineChart.IntervalX = MaximumX / 10;
        }

        public void SetChartVisibleRange(double minimum, double maximum, double interval)
        {
            MinimumX = minimum;
            MaximumX = maximum;
            IntervalX = interval;

            lineChart.SetChartVisibleRange(minimum, maximum, interval);
        }
        #endregion

        #region Private Methods
        private string mergeTitles(List<ChartData> chartData1, List<ChartData> chartData2, int index)
        {
            string title1 = String.Empty;
            if (chartData1 != null && index < chartData1.Count)
                title1 = chartData1[index].ChartTitle;

            string title2 = String.Empty;
            if (chartData2 != null && index < chartData2.Count)
                title2 = chartData2[index].ChartTitle;

            if (String.Compare(title1, title2, StringComparison.InvariantCultureIgnoreCase) == 0)
                return title1;

            if (String.IsNullOrEmpty(title1))
                return title2;
            if (String.IsNullOrEmpty(title2))
                return title1;

            string category = Regex.Replace(title1, @"\s*-.+$", "");
            title1 = Regex.Replace(title1, @"^.+-\s*", "");
            title2 = Regex.Replace(title2, @"^.+-\s*", "");

            return String.Format("{0} - {1} vs {2}", category, title1, title2);
        }

        //protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        //{
        //    base.OnRenderSizeChanged(sizeInfo);

        //    if (sizeInfo.HeightChanged)
        //    {
        //        double ctrlHeight = sizeInfo.NewSize.Height;
        //        double chartHeight = ctrlHeight - 23; // Header Spacing
        //        double intervalY = Math.Round((MaximumY) / (chartHeight / 20), 2);
        //        lineChart.IntervalY = intervalY;
        //    }

        //    if (sizeInfo.WidthChanged)
        //    {
        //        double ctrlWidth = sizeInfo.NewSize.Width;
        //        double chartWidth = ctrlWidth - 20; // Both Side Spacing
        //        double intervalX = Math.Round((MaximumX - MinimumX) / (chartWidth / 20), 2);
        //        lineChart.IntervalX = intervalX;
        //    }
        //}   
        #endregion

        #region Event Handlers
        private void visibleRangeChanged(double minimum, double maximum)
        {
            if (VisibleRangeChanged != null)
                VisibleRangeChanged(minimum, maximum);
        }

        private void chartMaximizedMinimized(object sender, RoutedEventArgs e)
        {
            if (ChartMaximizedMinimized != null)
            {
                IsMaximized = !IsMaximized;
                ChartMaximizedMinimized(this, IsMaximized);
            }
        }

        private void chartTitleMouseEnter(object sender, MouseEventArgs e)
        {
            if (ChartStatusChanged != null)
                ChartStatusChanged(IsMaximized ? Properties.Resources.MinimizeChartText : Properties.Resources.MaximizeChartText);
        }

        private void chartTitleMouseLeave(object sender, MouseEventArgs e)
        {
            if (ChartStatusChanged != null)
                ChartStatusChanged(null);
        }
        #endregion
    }
}
