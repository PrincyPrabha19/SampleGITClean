using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.Tools;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public class VideoChartUserControl : UserControl, ComponentVideoChartView
    {
        #region Properties
        public event Action<double, double> VisibleRangeChanged;
        public event Action<ComponentChartView, bool> ChartMaximizedMinimized;
        public event Action<string> ChartStatusChanged;

        public bool IsMaximized { get; set; }
        public virtual bool IsMainChart { get { return false; } }
        public int CardsCount { get; private set; }
        public Panel ChartsPanel { get; set; }
        public event Action<ComponentChartView> MoreInfoChartsOpened;
        public event Action<ComponentChartView> MoreInfoChartsClosed;
        public event Action<ComponentChartView> ChartClosed;

        private static readonly DependencyProperty isChartTitleEnabledProperty = DependencyProperty.Register("IsChartTitleEnabled", typeof(bool), typeof(VideoChartUserControl), new UIPropertyMetadata(true));
        public bool IsChartTitleEnabled
        {
            get { return (bool)GetValue(isChartTitleEnabledProperty); }
            set { SetValue(isChartTitleEnabledProperty, value); }
        }

        private static readonly DependencyProperty chartTitleProperty = DependencyProperty.Register("ChartTitle", typeof(string), typeof(VideoChartUserControl), new UIPropertyMetadata(null));
        public string ChartTitle
        {
            get { return (string)GetValue(chartTitleProperty); }
            set { SetValue(chartTitleProperty, value); }
        }

        private static readonly DependencyProperty IsRealtimeChartProperty = DependencyProperty.Register("IsRealtimeChart", typeof(bool), typeof(VideoChartUserControl));
        public bool IsRealtimeChart
        {
            get { return (bool)GetValue(IsRealtimeChartProperty); }
            set { SetValue(IsRealtimeChartProperty, value); }
        }

        private static readonly DependencyProperty minimumXTextProperty = DependencyProperty.Register("MinimumXText", typeof(string), typeof(VideoChartUserControl));
        public string MinimumXText
        {
            get { return (string)GetValue(minimumXTextProperty); }
            set { SetValue(minimumXTextProperty, value); }
        }

        private static readonly DependencyProperty maximumXTextProperty = DependencyProperty.Register("MaximumXText", typeof(string), typeof(VideoChartUserControl));
        public string MaximumXText
        {
            get { return (string)GetValue(maximumXTextProperty); }
            set { SetValue(maximumXTextProperty, value); }
        }

        private static readonly DependencyProperty maximumYTextProperty = DependencyProperty.Register("MaximumYText", typeof(string), typeof(VideoChartUserControl));
        public string MaximumYText
        {
            get { return (string)GetValue(maximumYTextProperty); }
            set { SetValue(maximumYTextProperty, value); }
        }

        public int ChartSeconds { get; set; }
        public double IntervalX { get; set; }
        public double IntervalY { get; set; }

        private double minimumX;
        public double MinimumX
        {
            get { return minimumX; }
            set
            {
                minimumX = value;

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

                if (IsRealtimeChart)
                    MinimumXText = String.Format("{0}s", maximumX);
                else
                    MaximumXText = TimeSpanHelper.ConvertTotalSecondsToHourMinutesSeconds((int)maximumX);
            }
        }

        public double MinimumY { get; set; }

        private double maximumY;
        public double MaximumY
        {
            get { return maximumY; }
            set { maximumY = value; MaximumYText = String.Format("{0}%", maximumY); }
        }

        private static readonly DependencyProperty areMoreInfoChartsVisibleProperty = DependencyProperty.Register("AreMoreInfoChartsVisible", typeof(bool), typeof(VideoChartUserControl), new UIPropertyMetadata(false));
        public bool AreMoreInfoChartsVisible
        {
            get { return (bool)GetValue(areMoreInfoChartsVisibleProperty); }
            set { SetValue(areMoreInfoChartsVisibleProperty, value); }
        }

        private SolidColorBrush lineSeries1Color;
        public SolidColorBrush LineSeries1Color
        {
            get { return lineSeries1Color; }
            set
            {
                lineSeries1Color = value;
                foreach (var ctrl in ChartsPanel.Children.OfType<LineChartControl>())
                    ctrl.LineSeries1Color = value;
            }
        }

        private SolidColorBrush lineSeries2Color;
        public SolidColorBrush LineSeries2Color
        {
            get { return lineSeries2Color; }
            set 
            { 
                lineSeries2Color = value;
                foreach (var ctrl in ChartsPanel.Children.OfType<LineChartControl>())
                    ctrl.LineSeries2Color = value;
            }
        }
        #endregion

        #region Constructor
        public VideoChartUserControl()
        {
        }
        #endregion

        #region Public Methods
        public void SetChartValues(List<ChartBufferData> values)
        {
            int idx = 0;
            foreach (var ctrl in ChartsPanel.Children.OfType<LineChartControl>())
                ctrl.SetDataContext(values[idx++].ChartPoints);
        }

        public void SetChartValues(List<ChartBufferData> values1, List<ChartBufferData> values2)
        {
            int idx1 = 0, idx2 = 0;
            foreach (var ctrl in ChartsPanel.Children.OfType<LineChartControl>())
            {
                var chartPoints1 = idx1 < values1.Count ? values1[idx1++].ChartPoints : null;
                var chartPoints2 = idx2 < values2.Count ? values2[idx2++].ChartPoints : null;

                if (chartPoints1 != null && chartPoints2 != null)
                    ctrl.SetDataContext(chartPoints1, chartPoints2);
                else
                if (chartPoints1 != null)
                    ctrl.SetDataContext(chartPoints1);
                else
                    if (chartPoints2 != null)
                        ctrl.SetDataContext(chartPoints2);
            } 
        }

        public void RefreshLayout()
        {
        }

        public void RefreshLayout(List<ModifiableKeyValueData> infoResults, Panel chartPanel)
        {
            ChartsPanel = chartPanel;
            CardsCount = infoResults.Count;

            var foreground = (SolidColorBrush)FindResource("AlienLightGray");
            var fontFamily = new FontFamily("Pill Gothic 600mg Semibd");
            var style = (Style)FindResource("ChartCaptionLinkButtonStyle");

            for (var i = 0; i < CardsCount; i++)
            {
                string content = String.Format("#{0} {1}", i + 1, infoResults[i].Key);
                var titleLabel = new Button() { Content = content, FontFamily = fontFamily, FontSize = 13, FontWeight = FontWeights.Bold, Foreground = foreground, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 10, 20, 0), Padding = new Thickness(0), Style = style };
                titleLabel.Click += chartMaximizedMinimized;
                titleLabel.MouseEnter += chartTitleMouseEnter;
                titleLabel.MouseLeave += chartTitleMouseLeave;

                var binding = new Binding { Source = this, Path = new PropertyPath("IsChartTitleEnabled"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                titleLabel.SetBinding(IsEnabledProperty, binding);

                var lineChart = new LineChartControl();
                lineChart.ChartSeconds = ChartSeconds;
                lineChart.IsRealtimeChart = IsRealtimeChart;
                lineChart.MinimumX = MinimumX;
                lineChart.MaximumX = MaximumX;
                lineChart.MaximumY = MaximumY;
                lineChart.IntervalY = MaximumY / 10;
                lineChart.IntervalX = MaximumX / 5;
                
                ChartsPanel.Children.Add(titleLabel);
                ChartsPanel.Children.Add(lineChart);
            }
        }

        public void RefreshLayout(List<ChartData> chartData1, List<ChartData> chartData2, Panel chartPanel)
        {
            ChartsPanel = chartPanel;
            CardsCount = Math.Max(chartData1.Count, chartData2 != null ? chartData2.Count : 0);

            var foreground = (SolidColorBrush)FindResource("AlienLightGray");
            var fontFamily = new FontFamily("Pill Gothic 600mg Semibd");
            var style = (Style)FindResource("ChartCaptionLinkButtonStyle");

            for (var i = 0; i < CardsCount; i++)
            {
                string content = mergeTitles(chartData1, chartData2, i);
                var titleLabel = new Button() { Content = content, FontFamily = fontFamily, FontSize = 13, FontWeight = FontWeights.Bold, Foreground = foreground, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 10, 20, 0), Padding = new Thickness(0), Style = style };
                titleLabel.Click += chartMaximizedMinimized;
                titleLabel.MouseEnter += chartTitleMouseEnter;
                titleLabel.MouseLeave += chartTitleMouseLeave;

                var binding = new Binding { Source = this, Path = new PropertyPath("IsChartTitleEnabled"), UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged };
                titleLabel.SetBinding(IsEnabledProperty, binding);

                var lineChart = new LineChartControl();
                lineChart.ChartSeconds = ChartSeconds;
                lineChart.IsRealtimeChart = IsRealtimeChart;
                lineChart.MinimumX = MinimumX;
                lineChart.MaximumX = MaximumX;
                lineChart.MaximumY = MaximumY;
                lineChart.IntervalY = MaximumY / 10;
                lineChart.IntervalX = MaximumX / 10;
                lineChart.VisibleRangeChanged += visibleRangeChanged;

                ChartsPanel.Children.Add(titleLabel);
                ChartsPanel.Children.Add(lineChart);
            }
        }

        private string mergeTitles(List<ChartData> chartData1, List<ChartData> chartData2, int index)
        {
            string prefix = String.Format("#{0} ", index + 1);

            string title1 = String.Empty;
            if (chartData1 != null && index < chartData1.Count)
                title1 = chartData1[index].ChartTitle;

            string title2 = String.Empty;
            if (chartData2 != null && index < chartData2.Count)
                title2 = chartData2[index].ChartTitle;

            if (String.Compare(title1, title2, StringComparison.InvariantCultureIgnoreCase) == 0)
                return prefix + title1;

            if (String.IsNullOrEmpty(title1))
                return prefix + title2;
            if (String.IsNullOrEmpty(title2))
                return prefix + title1;

            string category = Regex.Replace(title1, @"^.+\s*-\s*", "");
            title1 = Regex.Replace(title1, @"\s*-\s*.+$", "");
            title2 = Regex.Replace(title2, @"\s*-\s*.+$", "");

            return String.Format("{0}{1} vs {2} - {3}", prefix, title1, title2, category);
        }

        public void SetChartVisibleRange(double minimum, double maximum, double interval)
        {
            MinimumX = minimum;
            MaximumX = maximum;
            IntervalX = interval;

            var lineCharts = ChartsPanel.Children.OfType<LineChartControl>().ToList();
            foreach (var chart in lineCharts)
                chart.SetChartVisibleRange(minimum, maximum, interval);
        }
        #endregion

        #region Private Methods
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (GeneralSettings.IsInDesignMode)
                return;

            if (!sizeInfo.HeightChanged && !sizeInfo.WidthChanged)
                return;

            var lineCharts = ChartsPanel.Children.OfType<LineChartControl>().ToList();

            if (sizeInfo.HeightChanged)
            {
                double ctrlHeight = (sizeInfo.NewSize.Height - 20 /* Footer */) / CardsCount;
                double chartHeight = ctrlHeight - 23; // Header Spacing
                //double intervalY = Math.Round((MaximumY) / (ctrlHeight / 20), 2);

                foreach (var lineChartControl in lineCharts)
                {
                    lineChartControl.Height = chartHeight;
                    //lineChartControl.IntervalY = intervalY;
                    lineChartControl.IntervalY = MaximumY / 10;
                }
            }

            if (sizeInfo.WidthChanged)
            {
                double ctrlWidth = sizeInfo.NewSize.Width;
                double chartWidth = ctrlWidth - 20 - 10; // Both Side Spacing + Right Space
                //double intervalX = Math.Round((MaximumX - MinimumX) / (chartWidth / 20), 2);

                foreach (var lineChartControl in lineCharts)
                {
                    lineChartControl.Width = chartWidth;
                    //lineChartControl.IntervalX = intervalX;
                    lineChartControl.IntervalX = (MaximumX - MinimumX) / 10;
                }
            }
        }
        #endregion

        #region Event Handlers
        public void buttonMoreInfo_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var button = sender as ToggleButton;
            if (button == null || !button.IsChecked.HasValue) return;

            if (button.IsChecked.Value && MoreInfoChartsOpened != null)
            {
                MoreInfoChartsOpened(this);
                return;
            }

            if (!button.IsChecked.Value && MoreInfoChartsClosed != null)
                MoreInfoChartsClosed(this);
        }

        public void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            if (ChartClosed != null)
                ChartClosed(this);
        }

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
