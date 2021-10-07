
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Factories;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class NetworkBytesReceivedChart : ComponentNetworkChartView
    {
        #region Properties
        public bool IsMainChart { get { return true; } }
        public int NetworkInterfacesCount { get; private set; }
        public event Action<ComponentNetworkChartView> MoreInfoChartsOpened;
        public event Action<ComponentNetworkChartView> MoreInfoChartsClosed;
        public event Action<ComponentNetworkChartView> ChartClosed;        

        private static readonly DependencyProperty areMoreInfoChartsVisibleProperty = DependencyProperty.Register("AreMoreInfoChartsVisible", typeof(bool), typeof(NetworkBytesReceivedChart), new UIPropertyMetadata(false));
        public bool AreMoreInfoChartsVisible
        {
            get { return (bool)GetValue(areMoreInfoChartsVisibleProperty); }
            set { SetValue(areMoreInfoChartsVisibleProperty, value); }
        }

        private static readonly DependencyProperty chartSecondsProperty = DependencyProperty.Register("ChartSeconds", typeof(int), typeof(NetworkBytesReceivedChart), new UIPropertyMetadata(0));
        public int ChartSeconds
        {
            get { return (int)GetValue(chartSecondsProperty); }
            set { SetValue(chartSecondsProperty, value); }
        }

        private static readonly DependencyProperty chartTitleProperty = DependencyProperty.Register("ChartTitle", typeof(string), typeof(NetworkBytesReceivedChart), new UIPropertyMetadata(null));
        public string ChartTitle
        {
            get { return (string)GetValue(chartTitleProperty); }
            set { SetValue(chartTitleProperty, value); }
        }

        private static readonly DependencyProperty intervalXProperty = DependencyProperty.Register("IntervalX", typeof(int), typeof(NetworkBytesReceivedChart), new UIPropertyMetadata(3));
        public int IntervalX
        {
            get { return (int)GetValue(intervalXProperty); }
            set { SetValue(intervalXProperty, value); }
        }

        private static readonly DependencyProperty intervalYProperty = DependencyProperty.Register("IntervalY", typeof(int), typeof(NetworkBytesReceivedChart), new UIPropertyMetadata(8));
        public int IntervalY
        {
            get { return (int)GetValue(intervalYProperty); }
            set { SetValue(intervalYProperty, value); }
        }
        #endregion

        #region Constructor
        public NetworkBytesReceivedChart()
        {
            InitializeComponent();
            RefreshLayout();
        }
        #endregion

        #region Public Methods
        public void SetChartTitle(string title)
        {
            ChartTitle = title;
        }

        public void SetChartValues(ObservableCollection<Point> values)
        {
        }

        public void SetChartValues(List<ChartBufferData> values)
        {
            int idx = 0;
            foreach (var ctrl in panelNetworkCharts.Children)
            {
                if (ctrl.GetType() == typeof (LineChartControl))
                    ((LineChartControl) ctrl).SetDataContext(values[idx++].ChartPoints);
            }
        }

        private readonly int[] intervals = new[] { 8, 19, 34 };
        public void RefreshLayout()
        {
            var monitorService = PerfMonServiceFactory.Instance;
            NetworkInterfacesCount = monitorService.NetworkInfoResults.Count / 2;

            var foreground = (SolidColorBrush) FindResource("AlienLightGray");
            var fontFamily = new FontFamily("Pill Gothic 600mg Semibd");

            for (var i = 0; i < monitorService.NetworkInfoResults.Count; i++)
            {
                if (i%2 != 0) continue;
                var keyValue = monitorService.NetworkInfoResults[i];
                var titleTextBlock = new TextBlock() { Text = keyValue.Key, TextWrapping = TextWrapping.Wrap, FontFamily = fontFamily, FontSize = 13, FontWeight = FontWeights.Bold, Foreground = foreground, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(0, 10, 0, 0)};
                var lineChart = new LineChartControl() { ChartSeconds = 60 };
                panelNetworkCharts.Children.Add(titleTextBlock);
                panelNetworkCharts.Children.Add(lineChart);
            } 
        }
        #endregion

        #region Private Methods
        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            if (sizeInfo.HeightChanged)
            {
                int ctrlHeight = (int)sizeInfo.NewSize.Height / NetworkInterfacesCount;
                int chartHeight = ctrlHeight - 23; // Header Spacing
                int intervalY = NetworkInterfacesCount <= 2 ? intervals[NetworkInterfacesCount - 1] : intervals[2];

                var lineCharts = panelNetworkCharts.Children.OfType<LineChartControl>().ToList();
                foreach (var lineChartControl in lineCharts)
                {
                    lineChartControl.Height = chartHeight;
                    lineChartControl.IntervalY = intervalY;
                }
            }
        }
        #endregion

        #region Event Handlers
        private void buttonMoreInfo_Click(object sender, System.Windows.RoutedEventArgs e)
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
        #endregion
    }
}
