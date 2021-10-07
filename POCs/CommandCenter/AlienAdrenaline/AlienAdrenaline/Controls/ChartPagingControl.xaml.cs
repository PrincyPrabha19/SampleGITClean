#define DEBUG
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class ChartPagingControl
    {
        #region Public Properties
        private static readonly DependencyProperty chartStatusProperty = DependencyProperty.Register("ChartStatus", typeof(string), typeof(ChartPagingControl));
        public string ChartStatus
        {
            get { return (string)GetValue(chartStatusProperty); }
            set { SetValue(chartStatusProperty, value); }
        }

        private static readonly DependencyProperty nextTabButtonVisibleProperty = DependencyProperty.Register("NextTabButtonVisible", typeof(bool), typeof(ChartPagingControl), new UIPropertyMetadata(true));
        public bool NextTabButtonVisible
        {
            get { return (bool)GetValue(nextTabButtonVisibleProperty); }
            set { SetValue(nextTabButtonVisibleProperty, value); }
        }

        private static readonly DependencyProperty previousTabButtonVisibleProperty = DependencyProperty.Register("PreviousTabButtonVisible", typeof(bool), typeof(ChartPagingControl), new UIPropertyMetadata(false));
        public bool PreviousTabButtonVisible
        {
            get { return (bool)GetValue(previousTabButtonVisibleProperty); }
            set { SetValue(previousTabButtonVisibleProperty, value); }
        }

        private static readonly DependencyProperty chartPageTitleProperty = DependencyProperty.Register("ChartPageTitle", typeof(string), typeof(ChartPagingControl));
        public string ChartPageTitle
        {
            get { return (string)GetValue(chartPageTitleProperty); }
            set { SetValue(chartPageTitleProperty, value); }
        }

        private static readonly DependencyProperty lineSeries1ColorProperty = DependencyProperty.Register("LineSeries1Color", typeof(SolidColorBrush), typeof(ChartPagingControl), new UIPropertyMetadata(Brushes.White, onColor1Changed));
        public SolidColorBrush LineSeries1Color
        {
            get { return (SolidColorBrush)GetValue(lineSeries1ColorProperty); }
            set { SetValue(lineSeries1ColorProperty, value); }
        }

        private static readonly DependencyProperty lineSeries2ColorProperty = DependencyProperty.Register("LineSeries2Color", typeof(SolidColorBrush), typeof(ChartPagingControl), new UIPropertyMetadata(Brushes.White, onColor2Changed));
        public SolidColorBrush LineSeries2Color
        {
            get { return (SolidColorBrush)GetValue(lineSeries2ColorProperty); }
            set { SetValue(lineSeries2ColorProperty, value); }
        }

        private static readonly DependencyProperty lineSeries1TooltipProperty = DependencyProperty.Register("LineSeries1Tooltip", typeof(string), typeof(ChartPagingControl));
        public string LineSeries1Tooltip
        {
            get { return (string)GetValue(lineSeries1TooltipProperty); }
            set { SetValue(lineSeries1TooltipProperty, value); }
        }

        private static readonly DependencyProperty lineSeries2TooltipProperty = DependencyProperty.Register("LineSeries2Tooltip", typeof(string), typeof(ChartPagingControl));
        public string LineSeries2Tooltip
        {
            get { return (string)GetValue(lineSeries2TooltipProperty); }
            set { SetValue(lineSeries2TooltipProperty, value); }
        }

        public double MinimumX { get; private set; }
        public double MaximumX { get; private set; }
        public double IntervalX { get; private set; }
        public bool VisibleRangeChanged { get; private set; }

        public Action<ExtendedPoint, ExtendedPoint> CPUChartProcessDataSelectionChanged;
        #endregion

        #region Private Properties
        //private Size controlSize = new Size(758, 500);
        private Size controlSize = new Size(718, 500);
        //private Size chartSize = new Size(379, 262);
        private Size chartSize = new Size(359, 250);
        private WrapPanel homeWrapPanel;
        private WrapPanel networkWrapPanel;
        private WrapPanel videoWrapPanel;
        private bool selectionUpdated;
        private double selectionMinimum, selectionMaximum;
        #endregion

        #region Constructor
        public ChartPagingControl()
        {
            InitializeComponent();

            LineSeries1Color = Brushes.Aquamarine;
            LineSeries2Color = Brushes.Crimson;
        }
        #endregion

        #region Public Methods
        public void AddChart(ComponentChartView chartView)
        {
            if (chartView is CPUChart)
                ((CPUChart) chartView).ProcessDataSelectionChanged += cpuChart_ProcessDataSelectionChanged;
            chartView.VisibleRangeChanged += chartView_VisibleRangeChanged;

            if (tabControl.Items.Count <= 0)
            {
                homeWrapPanel = new WrapPanel() { Width = controlSize.Width, Background = Brushes.Black };
                tabControl.Items.Add(homeWrapPanel);                

                networkWrapPanel = new WrapPanel() { Width = controlSize.Width, Background = Brushes.Black };
                tabControl.Items.Add(networkWrapPanel);

                videoWrapPanel = new WrapPanel() { Width = controlSize.Width, Background = Brushes.Black };
                tabControl.Items.Add(videoWrapPanel);

                tabControl.SelectedIndex = 0;
            }

            chartView.Width = chartSize.Width;
            chartView.Height = chartSize.Height;
            chartView.LineSeries1Color = LineSeries1Color;
            chartView.LineSeries2Color = LineSeries2Color;
            chartView.ChartMaximizedMinimized -= chartView_ChartMaximizedMinimized;
            chartView.ChartMaximizedMinimized += chartView_ChartMaximizedMinimized;
            chartView.ChartStatusChanged -= chartView_ChartStatusChanged;
            chartView.ChartStatusChanged += chartView_ChartStatusChanged;

            int index;
            if (chartView is NetworkChartUserControl &&
                (index = indexOfChart<VideoGPUUsageChart>(homeWrapPanel)) != -1)
                homeWrapPanel.Children.Insert(index, (UIElement) chartView);
            else
                homeWrapPanel.Children.Add((UIElement) chartView);

            refreshButtons();
        }

        public void ClearChart()
        {
            tabControl.Items.Clear();
            expandableWrapPanel.Children.Clear();
            panelLegend.Visibility = Visibility.Collapsed;
            buttonZoomOut.Visibility = Visibility.Collapsed;
            expandableGrid.Background = null;
        }

        public void UpdateChartControlLegend(string fileName1, int records1, string fileName2, int records2)
        {
            string tooltip1 = String.Format(Properties.Resources.ChartLegendTooltip, Path.GetFileNameWithoutExtension(fileName1), records1);
            string tooltip2 = String.Format(Properties.Resources.ChartLegendTooltip, Path.GetFileNameWithoutExtension(fileName2), records2);

            LineSeries1Tooltip = records1 >= records2 ? tooltip1 : tooltip2;
            LineSeries2Tooltip = records1 >= records2 ? tooltip2 : tooltip1;
        }

        public void RefreshChartsVisibility(Visibility[] categoriesToShow, MonitoringCategories selectedCategory)
        {
            chartZoomOut();

            for (var i = 0; i < categoriesToShow.Length; i++)
            {
                var visibility = categoriesToShow[i];

                switch (i)
                {
                    case (int)MonitoringCategories.CPU:
                        refreshCPUChartVisibility(visibility, selectedCategory);
                        break;
                    case (int)MonitoringCategories.Memory:
                        refreshMemoryChartVisibility(visibility, selectedCategory);
                        break;
                    case (int)MonitoringCategories.Network:
                        refreshNetworkChartVisibility(visibility, selectedCategory);
                        break;
                    case (int)MonitoringCategories.GPU:
                        refreshVideoChartVisibility(visibility, selectedCategory);
                        break;
                }
            }          
  
            refreshButtons();
        }
        #endregion

        #region Private Methods
        private void refreshCPUChartVisibility(Visibility visibility, MonitoringCategories selectedCategory)
        {
            if (selectedCategory != MonitoringCategories.CPU)
                return;

            setChartVisibility<CPUChart>(visibility);
            
            if (visibility == Visibility.Visible)
                tabControl.SelectedItem = homeWrapPanel;
            else
            {
                if (tabControl.SelectedItem.Equals(homeWrapPanel))
                    if (homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) <= 0)
                        tabControl.SelectedItem = (networkWrapPanel.Children.Count > 0) ? networkWrapPanel : videoWrapPanel;
            }

            RefreshChartLayout(homeWrapPanel);
        }

        private void refreshMemoryChartVisibility(Visibility visibility, MonitoringCategories selectedCategory)
        {
            if (selectedCategory != MonitoringCategories.Memory)
                return;

            setChartVisibility<MemoryChart>(visibility);

            if (visibility == Visibility.Visible)
                tabControl.SelectedItem = homeWrapPanel;
            else
            {
                if (tabControl.SelectedItem.Equals(homeWrapPanel))
                    if (homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) <= 0)
                        tabControl.SelectedItem = (networkWrapPanel.Children.Count > 0) ? networkWrapPanel : videoWrapPanel;
            }

            RefreshChartLayout(homeWrapPanel);
        }

        private void refreshNetworkChartVisibility(Visibility visibility, MonitoringCategories selectedCategory)
        {
            if (selectedCategory != MonitoringCategories.Network)
                return;

            var networkCharts = GetCharts<ComponentNetworkChartView>(networkWrapPanel);
            var mainNetworkChart = networkCharts.FirstOrDefault(c => c.IsMainChart);

            if (visibility == Visibility.Collapsed)
            {
                if (mainNetworkChart != null)
                    mainNetworkChart.AreMoreInfoChartsVisible = false;

                UpdateAllNetworkChartsVisibility(visibility);
                if (networkWrapPanel.Children.Count > 0)
                {                    
                    foreach (var chart in networkCharts)
                    {
                        networkWrapPanel.Children.Remove((UIElement)chart);
                        //homeWrapPanel.Children.Add((UIElement)chart);
                        AddChart(chart);
                    }
                }

                if (tabControl.SelectedItem.Equals(homeWrapPanel) && 
                    homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) <= 0)
                    tabControl.SelectedItem = (networkWrapPanel.Children.Count > 0) ? networkWrapPanel : videoWrapPanel;
                else
                if (tabControl.SelectedItem.Equals(networkWrapPanel))
                    tabControl.SelectedItem = (homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) > 0) ? homeWrapPanel : videoWrapPanel;

                RefreshChartLayout((WrapPanel) tabControl.SelectedItem);
                refreshButtons();
                return;
            }

            tabControl.SelectedItem = homeWrapPanel;
            UpdateAllNetworkChartsVisibility(visibility, false, false);
            RefreshChartLayout(homeWrapPanel);
            refreshButtons();
        }

        private void refreshVideoChartVisibility(Visibility visibility, MonitoringCategories selectedCategory)
        {
            if (selectedCategory != MonitoringCategories.GPU)
                return;

            var videoCharts = GetCharts<ComponentVideoChartView>(videoWrapPanel);
            var mainVideoChart = videoCharts.FirstOrDefault(c => c.IsMainChart);

            if (visibility == Visibility.Collapsed)
            {
                if (mainVideoChart != null)
                    mainVideoChart.AreMoreInfoChartsVisible = false;

                UpdateAllVideoChartsVisibility(visibility);
                if (videoWrapPanel.Children.Count > 0)
                {
                    foreach (var chart in videoCharts)
                    {
                        videoWrapPanel.Children.Remove((UIElement)chart);
                        //homeWrapPanel.Children.Add((UIElement)chart);
                        AddChart(chart);
                    }
                }

                if (tabControl.SelectedItem.Equals(homeWrapPanel) &&
                    homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) <= 0)
                    tabControl.SelectedItem = (networkWrapPanel.Children.Count > 0) ? networkWrapPanel : videoWrapPanel;
                else
                if (tabControl.SelectedItem.Equals(videoWrapPanel))
                    tabControl.SelectedItem = (homeWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible) > 0) ? homeWrapPanel : networkWrapPanel;

                RefreshChartLayout((WrapPanel)tabControl.SelectedItem);
                refreshButtons();
                return;
            }

            tabControl.SelectedItem = homeWrapPanel;
            UpdateAllVideoChartsVisibility(visibility, false, false);
            RefreshChartLayout(homeWrapPanel);
            refreshButtons();
        }

        public void SetChartValues(ComponentChartView chart, List<ChartBufferData> values)
        {
            if (chart != null)
                chart.SetChartValues(values);
        }

        public void SetChartValues(ComponentChartView chart, List<ChartBufferData> values1, List<ChartBufferData> values2)
        {
            if (chart != null)
            {
                if (values1.Count > 0 && values2.Count > 0)
                {
                    panelLegend.Visibility = Visibility.Visible;

                    if (values1[0].ChartPoints.Count >= values2[0].ChartPoints.Count)
                        chart.SetChartValues(values1, values2);
                    else
                        chart.SetChartValues(values2, values1);

                    return;
                }

                chart.SetChartValues(values1.Count > 0 ? values1 : values2);
            }            
        }

        private ComponentChartView findChart<T>()
        {
            foreach (WrapPanel panel in tabControl.Items)
                foreach (var ctrl in panel.Children)
                    if (ctrl.GetType() == typeof(T))
                        return ctrl as ComponentChartView;

            return findChart<T>(expandableWrapPanel);
        }

        private ComponentChartView findChart<T>(WrapPanel panel)
        {
            foreach (var ctrl in panel.Children)
                if (ctrl.GetType() == typeof(T))
                    return ctrl as ComponentChartView;

            return null;
        }

        private int indexOfChart<T>(WrapPanel panel)
        {
            var chart = findChart<T>(panel);
            if (chart == null)
                return -1;
            return panel.Children.IndexOf(chart as UIElement);
        }

        public List<T> GetCharts<T>()
        {
            var list = new List<T>();

            foreach (WrapPanel panel in tabControl.Items)
                list.AddRange(panel.Children.OfType<T>());            
            list.AddRange(expandableWrapPanel.Children.OfType<T>());            
            
            return list;
        }

        public List<T> GetCharts<T>(WrapPanel panel)
        {
            return panel.Children.OfType<T>().ToList();
        }

        public bool ExistsChart<T>()
        {
            return findChart<T>() != null;
        }

        private void setChartVisibility<T>(Visibility visibility)
        {
            var chart = findChart<T>();
            if (chart != null)
                chart.Visibility = visibility;
        }

        public ComponentChartView GetChart<T>()
        {
            var chart = findChart<T>();
            return chart ?? null;
        }

        public void RefreshChartLayout(WrapPanel panel)
        {
            double width = chartSize.Width;
            double height = chartSize.Height;

            int visibleCount = panel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible);
            if (visibleCount <= 1)
            {
                width = 2 * chartSize.Width;
                height = 2 * chartSize.Height;
            }
            else
            if (visibleCount <= 2)
                width = 2 * chartSize.Width;

            foreach (var ctrl in panel.Children)
            {
                var chart = ctrl as ComponentChartView;
                if (chart != null)
                {
                    if (chart.Visibility == Visibility.Visible)
                    {
                        chart.Width = width;
                        chart.Height = height;                        
                    }

                    chart.IsChartTitleEnabled = panel == expandableWrapPanel || visibleCount > 1;
                }              
            }
        }

        public void UpdateAllNetworkChartsVisibility(Visibility visibility, bool excludeMainChart = false, bool changeMoreInfoCharts = true)
        {
            var networkCharts = GetCharts<ComponentNetworkChartView>();
            foreach (var chartView in networkCharts)
                if ((!chartView.IsMainChart && changeMoreInfoCharts) || (chartView.IsMainChart && !excludeMainChart))
                    chartView.Visibility = visibility;
        }

        public void UpdateAllVideoChartsVisibility(Visibility visibility, bool excludeMainChart = false, bool changeMoreInfoCharts = true)
        {
            var videoCharts = GetCharts<ComponentVideoChartView>();
            foreach (var chartView in videoCharts)
                if ((!chartView.IsMainChart && changeMoreInfoCharts) || (chartView.IsMainChart && !excludeMainChart))
                    chartView.Visibility = visibility;
        }
        #endregion

        private int forwardWrapPanelIndexWithVisibleCharts()
        {
            for (var i = tabControl.SelectedIndex + 1; i < tabControl.Items.Count; i++)
            {
                var visibleCount = ((WrapPanel) tabControl.Items[i]).Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible);
                if (visibleCount > 0)
                    return i;
            }

            return -1;
        }

        private int backwardWrapPanelIndexWithVisibleCharts()
        {
            for (var i = tabControl.SelectedIndex - 1; i >= 0; i--)
            {
                var visibleCount = ((WrapPanel)tabControl.Items[i]).Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible);
                if (visibleCount > 0)
                    return i;
            }

            return -1;
        }

        private void refreshButtons()
        {
            NextTabButtonVisible = forwardWrapPanelIndexWithVisibleCharts() != - 1;
            PreviousTabButtonVisible = backwardWrapPanelIndexWithVisibleCharts() != -1;
        }

        private void nextTagPanelButton_OnClick(object sender, RoutedEventArgs e)
        {
            chartZoomOut();

            var index = forwardWrapPanelIndexWithVisibleCharts();
            if (index != -1)
            {
                tabControl.SelectedIndex = index;
                refreshButtons();
            }            
        }

        private void previousTagPanelButton_OnClick(object sender, RoutedEventArgs e)
        {
            chartZoomOut();

            var index = backwardWrapPanelIndexWithVisibleCharts();
            if (index != -1)
            {
                tabControl.SelectedIndex = index;
                refreshButtons();
            } 
        }

        private void chartView_ChartStatusChanged(string message)
        {
            ChartStatus = message;
        }

        private WrapPanel sourceWrapPanel;
        private int chartExpandedIndexOf;
        private void chartView_ChartMaximizedMinimized(ComponentChartView chartView, bool maximize)
        {
            if (!maximize)
            {
                chartZoomOut();
                return;
            }

            sourceWrapPanel = ((UIElement)chartView).TryFindParent<WrapPanel>();
            if (sourceWrapPanel != null)
            {
                var visibleCount = sourceWrapPanel.Children.Cast<UIElement>().Count(ctrl => ctrl.Visibility == Visibility.Visible);
                if (visibleCount <= 1)
                    return;

                chartExpandedIndexOf = sourceWrapPanel.Children.IndexOf((UIElement)chartView);
                sourceWrapPanel.Children.RemoveAt(chartExpandedIndexOf);

                expandableWrapPanel.Children.Add((UIElement)chartView);
                RefreshChartLayout(expandableWrapPanel);
                showExpandableGrid();
            }
        }

        private void buttonZoomOut_OnClick(object sender, RoutedEventArgs e)
        {
            chartZoomOut();
        }

        private bool chartZoomingOut;
        private void chartZoomOut()
        {
            if (chartZoomingOut) return;

            try
            {
                chartZoomingOut = true;                

                if (expandableWrapPanel.Children.Count > 0)
                {
                    var chartView = expandableWrapPanel.Children[0] as ComponentChartView;
                    if (chartView != null)
                    {
                        chartView.IsMaximized = false;
                        expandableWrapPanel.Children.Remove((UIElement)chartView);
                        sourceWrapPanel.Children.Insert(chartExpandedIndexOf, (UIElement)chartView);
                        RefreshChartLayout(sourceWrapPanel);
                        sourceWrapPanel = null;
                        chartExpandedIndexOf = -1;

                        hideExpandableGrid();
                    }
                }  
            }
            finally
            {
                chartZoomingOut = false;
            }
        }

        public void NetworkChartMoreInfoChartsOpened(ComponentChartView chartView)
        {
            UpdateAllNetworkChartsVisibility(Visibility.Visible);

            var networkCharts = GetCharts<ComponentNetworkChartView>(homeWrapPanel);
            foreach (var chart in networkCharts)
            {
                homeWrapPanel.Children.Remove((UIElement)chart);
                networkWrapPanel.Children.Add((UIElement)chart);
            }            

            RefreshChartLayout(homeWrapPanel);
            RefreshChartLayout(networkWrapPanel);

            tabControl.SelectedItem = networkWrapPanel;

            refreshButtons();

            //UpdateAllNetworkChartsVisibility(Visibility.Visible);
        }

        public void NetworkChartMoreInfoChartsClosed(ComponentChartView chartView)
        {            
            UpdateAllNetworkChartsVisibility(Visibility.Collapsed, true);

            var currentPanel = (WrapPanel) tabControl.SelectedItem;
            var networkCharts = GetCharts<ComponentNetworkChartView>();
            foreach (var chart in networkCharts)
            {
                currentPanel.Children.Remove((UIElement)chart);
                //homeWrapPanel.Children.Add((UIElement)chart);
                AddChart(chart);
            }

            RefreshChartLayout(homeWrapPanel);
            tabControl.SelectedItem = homeWrapPanel;
                       
            refreshButtons();

            //UpdateAllNetworkChartsVisibility(Visibility.Collapsed, true);
        }

        public void NetworkChartChartClosed(ComponentChartView chartView)
        {
            chartView.Visibility = Visibility.Collapsed;

            var currentPanel = (WrapPanel)tabControl.SelectedItem;

            var networkCharts = GetCharts<ComponentNetworkChartView>();
            if (networkCharts.Count(c => c.Visibility == Visibility.Visible) > 1)
            {
                RefreshChartLayout(currentPanel);
                return;
            }

            var networkChartMainCtrl = networkCharts.FirstOrDefault(c => c.IsMainChart);
            if (networkChartMainCtrl != null)
                networkChartMainCtrl.AreMoreInfoChartsVisible = false;

            foreach (var chart in networkCharts)
            {
                currentPanel.Children.Remove((UIElement) chart);
                //homeWrapPanel.Children.Add((UIElement) chart);
                AddChart(chart);
            }

            RefreshChartLayout(homeWrapPanel);
            tabControl.SelectedItem = homeWrapPanel;

            refreshButtons();
        }

        public void VideoChartMoreInfoChartsOpened(ComponentChartView chartView)
        {
            UpdateAllVideoChartsVisibility(Visibility.Visible);

            var videoCharts = GetCharts<ComponentVideoChartView>(homeWrapPanel);
            foreach (var chart in videoCharts)
            {
                homeWrapPanel.Children.Remove((UIElement)chart);
                videoWrapPanel.Children.Add((UIElement)chart);
            }

            RefreshChartLayout(homeWrapPanel);
            RefreshChartLayout(videoWrapPanel);

            tabControl.SelectedItem = videoWrapPanel;

            refreshButtons();

            //UpdateAllNetworkChartsVisibility(Visibility.Visible);
        }

        public void VideoChartMoreInfoChartsClosed(ComponentChartView chartView)
        {
            UpdateAllVideoChartsVisibility(Visibility.Collapsed, true);

            var currentPanel = (WrapPanel)tabControl.SelectedItem;
            var videoCharts = GetCharts<ComponentVideoChartView>();
            foreach (var chart in videoCharts)
            {
                currentPanel.Children.Remove((UIElement)chart);
                //homeWrapPanel.Children.Add((UIElement)chart);
                AddChart(chart);
            }

            RefreshChartLayout(homeWrapPanel);

            tabControl.SelectedItem = homeWrapPanel;

            refreshButtons();

            //UpdateAllNetworkChartsVisibility(Visibility.Collapsed, true);
        }

        public void VideoChartChartClosed(ComponentChartView chartView)
        {
            chartView.Visibility = Visibility.Collapsed;

            var currentPanel = (WrapPanel)tabControl.SelectedItem;
            var videoCharts = GetCharts<ComponentVideoChartView>();
            if (videoCharts.Count(c => c.Visibility == Visibility.Visible) > 1)
            {
                RefreshChartLayout(currentPanel);
                return;
            }

            var videoChartMainCtrl = videoCharts.FirstOrDefault(c => c.IsMainChart);
            if (videoChartMainCtrl != null)
                videoChartMainCtrl.AreMoreInfoChartsVisible = false;

            foreach (var chart in videoCharts)
            {
                currentPanel.Children.Remove((UIElement) chart);
                //homeWrapPanel.Children.Add((UIElement) chart);
                AddChart(chart);
            }

            RefreshChartLayout(homeWrapPanel);
            tabControl.SelectedItem = homeWrapPanel;

            refreshButtons();
        }

        #region Event Handlers
        private void cpuChart_ProcessDataSelectionChanged(ExtendedPoint extendedPoint1, ExtendedPoint extendedPoint2)
        {
            if (CPUChartProcessDataSelectionChanged != null)
                CPUChartProcessDataSelectionChanged(extendedPoint1, extendedPoint2);
        }

        private void tabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
			 if (tabControl.SelectedItem == null)
				 return;

            if (tabControl.SelectedItem.Equals(homeWrapPanel))
                ChartPageTitle = Properties.Resources.ChartPagingHomePage;
            else
            if (tabControl.SelectedItem.Equals(networkWrapPanel))
                ChartPageTitle = Properties.Resources.ChartPagingNetworkPageTitle;
            else
            if (tabControl.SelectedItem.Equals(videoWrapPanel))
                ChartPageTitle = Properties.Resources.ChartPagingVideoPageTitle;
        }

        private void chartView_VisibleRangeChanged(double minimum, double maximum)
        {
            if (minimum > maximum)
                return;
            
            double interval = (maximum - minimum) / 10;

            VisibleRangeChanged = true;
            MinimumX = minimum;
            MaximumX = maximum;
            IntervalX = interval;

            var charts = GetCharts<ComponentChartView>();
            foreach (var chart in charts)
                chart.SetChartVisibleRange(minimum, maximum, interval);
        }

        private static void onColor1Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ChartPagingControl;
            if (control != null && control.infoBox1.Content is Border)
                ((Border) control.infoBox1.Content).Background = (SolidColorBrush) e.NewValue;
        }

        private static void onColor2Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ChartPagingControl;
            if (control != null && control.infoBox2.Content is Border)
                ((Border)control.infoBox2.Content).Background = (SolidColorBrush) e.NewValue;
        }

        private void showExpandableGrid()
        {
            expandableGrid.Background = Brushes.Black;
            buttonZoomOut.Visibility = Visibility.Visible;
            var animation = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(350));
            //animation.Completed += (s, _) => buttonZoomOut.Visibility = Visibility.Visible;
            expandableWrapPanel.BeginAnimation(OpacityProperty, animation);
        }

        private void hideExpandableGrid()
        {
            buttonZoomOut.Visibility = Visibility.Collapsed;
            expandableGrid.Background = null;
            var animation = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(350));
            expandableWrapPanel.BeginAnimation(OpacityProperty, animation);
        }
        #endregion
    }
}
