#define DEBUG
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.Model.DataSeries;
using Abt.Controls.SciChart.Visuals.Annotations;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Presenters;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class LineChartControl : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        #endregion

        #region Properties
        public event Action<double, double> VisibleRangeChanged;

        private static readonly DependencyProperty chartSecondsProperty = DependencyProperty.Register("ChartSeconds", typeof(int), typeof(LineChartControl), new UIPropertyMetadata(0));
        public int ChartSeconds
        {
            get { return (int)GetValue(chartSecondsProperty); }
            set
            {
                SetValue(chartSecondsProperty, value);
                zoomExtentsModifier.ChartSeconds = value;
            }
        }

        private static readonly DependencyProperty IsRealtimeChartProperty = DependencyProperty.Register("IsRealtimeChart", typeof(bool), typeof(LineChartControl));
        public bool IsRealtimeChart
        {
            get { return (bool)GetValue(IsRealtimeChartProperty); }
            set
            {
                SetValue(IsRealtimeChartProperty, value);

                dataPointsAnnotationModifier.IsEnabled = !value;
                zoomExtentsModifier.IsEnabled = !value;
                rubberBandXyZoomModifier.IsEnabled = !value;
                
                xyDataSeries1.FifoCapacity = null;
                if (value)
                    xyDataSeries1.FifoCapacity = ChartSeconds + 1;                
            }
        }

        private double intervalX;
        public double IntervalX
        {
            set
            {
                intervalX = value;
                //sciLineChart.XAxis.MajorDelta = value;
            }
            get { return intervalX; }

        }

        private double intervalY;
        public double IntervalY
        {
            set
            {
                intervalY = value;
                //sciLineChart.YAxis.MajorDelta = value;
            }
            get { return intervalY; }
        }

        public double MinimumX { get; set; }
        public double MaximumX { get; set; }

        public double MinimumY { get; set; }
        //{
        //    set
        //    {
        //        sciLineChart.YAxis.VisibleRange.Min = value;
        //    }
        //}

        public double MaximumY { get; set; }
        //{
        //    set
        //    {
        //        sciLineChart.YAxis.VisibleRange.Max = value + value*0.002d;
        //    }
        //}

        private static readonly DependencyProperty showLineDataPointsProperty = DependencyProperty.Register("ShowLineDataPoints", typeof(bool), typeof(LineChartControl), new UIPropertyMetadata(false));
        public bool ShowLineDataPoints
        {
            get { return (bool)GetValue(showLineDataPointsProperty); }
            set
            {
                SetValue(showLineDataPointsProperty, value);
            }
        }

        private static readonly DependencyProperty lineSeries1ColorProperty = DependencyProperty.Register("LineSeries1Color1", typeof(SolidColorBrush), typeof(LineChartControl), new UIPropertyMetadata(Brushes.White));
        public SolidColorBrush LineSeries1Color
        {
            get { return (SolidColorBrush)GetValue(lineSeries1ColorProperty); }
            set
            {
                SetValue(lineSeries1ColorProperty, value);
                sciLineSeries1.SeriesColor = value.Color;
            }
        }

        private static readonly DependencyProperty lineSeries2ColorProperty = DependencyProperty.Register("LineSeries2Color2", typeof(SolidColorBrush), typeof(LineChartControl), new UIPropertyMetadata(Brushes.White));
        public SolidColorBrush LineSeries2Color
        {
            get { return (SolidColorBrush)GetValue(lineSeries2ColorProperty); }
            set
            {
                SetValue(lineSeries2ColorProperty, value);
                sciLineSeries2.SeriesColor = value.Color;
            }
        }

        private static readonly DependencyProperty lineSeries1DataPointStyleProperty = DependencyProperty.Register("LineSeries1DataPointStyle", typeof(Style), typeof(LineChartControl));
        public Style LineSeries1DataPointStyle
        {
            get { return (Style)GetValue(lineSeries1DataPointStyleProperty); }
            set { SetValue(lineSeries1DataPointStyleProperty, value); }
        }

        private static readonly DependencyProperty lineSeries2DataPointStyleProperty = DependencyProperty.Register("LineSeries2DataPointStyle", typeof(Style), typeof(LineChartControl));
        public Style LineSeries2DataPointStyle
        {
            get { return (Style)GetValue(lineSeries2DataPointStyleProperty); }
            set { SetValue(lineSeries2DataPointStyleProperty, value); }
        }

        public Action<ExtendedPoint, ExtendedPoint> ProcessDataSelectionChanged; 
        #endregion

        #region Private Properties
        private bool onPreviewDoubleClick;
        private readonly List<CheckBox> dataPointSelected = new List<CheckBox>();
        private readonly Style dataPointsAnnotationModifier1Style;
        private readonly Style dataPointsAnnotationModifier2Style;
        private const int SHOW_DATAPOINTS_SECONDS = 1800;
        #endregion

        #region Constructor
        public LineChartControl()
        {
            InitializeComponent();            

            dataPointsAnnotationModifier1Style = (Style)FindResource("DataPointsAnnotationModifier1Style");
            dataPointsAnnotationModifier2Style = (Style)FindResource("DataPointsAnnotationModifier2Style");

            sciLineChart.XAxis.VisibleRangeChanged += XAxis_VisibleRangeChanged;
            sciLineChart.YAxis.VisibleRangeChanged += YAxis_VisibleRangeChanged;
        }

        private void XAxis_VisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            sciLineChart.XAxis.VisibleRangeChanged -= XAxis_VisibleRangeChanged;
            sciLineChart.XAxis.MajorDelta = (MaximumX - MinimumX) / 10;                        
        }

        private void YAxis_VisibleRangeChanged(object sender, VisibleRangeChangedEventArgs e)
        {
            sciLineChart.YAxis.VisibleRangeChanged -= YAxis_VisibleRangeChanged;
            sciLineChart.YAxis.MajorDelta = (MaximumY - MinimumY) / 10;            
        }
        #endregion

        #region Public Methods
        private ObservableCollection<ExtendedPoint> extendedPoints1, extendedPoints2;
        private readonly IXyDataSeries<double, double> xyDataSeries1 = new XyDataSeries<double, double>();
        private readonly IXyDataSeries<double, double> xyDataSeries2 = new XyDataSeries<double, double>();

        public void SetDataContext(ObservableCollection<ExtendedPoint> chartValues)
        {
            if (IsRealtimeChart && sciLineSeries1.DataSeries != null)
            {
                xyDataSeries1.Append(chartValues[chartValues.Count - 1].X, chartValues[chartValues.Count - 1].Y);
                MaximumX++;
                MinimumX++;
                sciLineChart.XAxis.VisibleRange = new DoubleRange(MinimumX, MaximumX);
                return;
            }

            MaximumX = chartValues[chartValues.Count - 1].X;
            MinimumX = chartValues[0].X;
            //IntervalX = MaximumX/10;
            //IntervalY = MaximumY/10;
            sciLineChart.XAxis.VisibleRange = new DoubleRange(MinimumX, MaximumX);
            sciLineChart.YAxis.VisibleRange = new DoubleRange(MinimumY - MaximumY * 0.002d, MaximumY + MaximumY * 0.002d);

            xyDataSeries1.Clear();
            xyDataSeries1.Append(chartValues.Select(i => i.X), chartValues.Select(i => i.Y));
            sciLineSeries1.DataSeries = xyDataSeries1;
            
            if (ShowLineDataPoints)
            {
                extendedPoints1 = chartValues;
                extendedPoints2 = null;

                sciLineChart.Rendered -= sciLineChart_Rendered;
                sciLineChart.Rendered += sciLineChart_Rendered;                
            }
        }
        
        public void SetDataContext(ObservableCollection<ExtendedPoint> chartValues1, ObservableCollection<ExtendedPoint> chartValues2)
        {
            MaximumX = chartValues1[chartValues1.Count - 1].X;
            MinimumX = chartValues1[0].X;
            sciLineChart.XAxis.VisibleRange = new DoubleRange(MinimumX, MaximumX);
            sciLineChart.YAxis.VisibleRange = new DoubleRange(MinimumY, MaximumY + MaximumY * 0.002d);

            xyDataSeries1.Clear();
            xyDataSeries1.Append(chartValues1.Select(i => i.X), chartValues1.Select(i => i.Y));
            sciLineSeries1.DataSeries = xyDataSeries1;

            if (chartValues2 != null)
            {
                xyDataSeries2.Clear();
                xyDataSeries2.Append(chartValues2.Select(i => i.X), chartValues2.Select(i => i.Y));
                sciLineSeries2.DataSeries = xyDataSeries2;
            }

            if (ShowLineDataPoints)
            {
                extendedPoints1 = chartValues1;
                extendedPoints2 = chartValues2;

                sciLineChart.Rendered -= sciLineChart_Rendered;
                sciLineChart.Rendered += sciLineChart_Rendered;
            }
        }

        public void SetChartVisibleRange(double minimum, double maximum, double interval)
        {           
            MinimumX = minimum;
            MaximumX = maximum;
            IntervalX = interval;

            sciLineChart.XAxis.VisibleRange = new DoubleRange(minimum, maximum);
            //sciLineChart.XAxis.MajorDelta = (maximum - minimum) / 10;
        }
        #endregion

        #region Event Handlers
        private void dataPointCheckbox_OnClick(object sender, RoutedEventArgs e)
        {
            var checkbox = (CheckBox)sender;
            var isChecked = checkbox.IsChecked.HasValue && checkbox.IsChecked.Value;
            if (!isChecked)
                dataPointSelected.Remove(checkbox);
            else
            {
                dataPointSelected.Add(checkbox);
                if (dataPointSelected.Count > 2)
                {
                    dataPointSelected[0].IsChecked = false;
                    dataPointSelected.RemoveAt(0);
                }
            }

            if (ProcessDataSelectionChanged != null)
            {
                ExtendedPoint extendedPoint1 = null;
                if (dataPointSelected.Count > 0)
                {
                    var annotation1 = dataPointSelected[0].TryFindParent<CustomAnnotation>();
                    if (annotation1 != null)
                    {
                        extendedPoint1 = annotation1.DataContext as ExtendedPoint;
                        if (extendedPoint1 != null)
                        {
                            var c1 = ((SolidColorBrush)dataPointSelected[0].Background).Color;
                            extendedPoint1.Color = new SolidColorBrush(Color.FromArgb(0x4f, c1.R, c1.G, c1.B));
                        }
                    }
                }

                ExtendedPoint extendedPoint2 = null;
                if (dataPointSelected.Count > 1)
                {
                    var annotation2 = dataPointSelected[1].TryFindParent<CustomAnnotation>();
                    if (annotation2 != null)
                    {
                        extendedPoint2 = annotation2.DataContext as ExtendedPoint;
                        if (extendedPoint2 != null)
                        {
                            var c1 = ((SolidColorBrush)dataPointSelected[1].Background).Color;
                            extendedPoint2.Color = new SolidColorBrush(Color.FromArgb(0x4f, c1.R, c1.G, c1.B));
                        }
                    }
                }

                ProcessDataSelectionChanged(extendedPoint1, extendedPoint2);
            }
        }

        private void dataPointCheckbox_OnPreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (onPreviewDoubleClick)
                return;

            try
            {
                onPreviewDoubleClick = true;

                var annotation = (sender as CheckBox).TryFindParent<CustomAnnotation>();
                if (annotation != null)
                {
                    var extendedPoint = annotation.DataContext as ExtendedPoint;
                    if (extendedPoint != null)
                    {
                        var processPresenter = new ProcessesPresenter();
                        processPresenter.ShowData(extendedPoint);
                    }
                }
            }
            finally
            {
                onPreviewDoubleClick = false;
            }
        }

        private void sciLineChart_Rendered(object sender, EventArgs e)
        {
            try
            {
                var xCalc = sciLineChart.XAxis.GetCurrentCoordinateCalculator();
                var yCalc = sciLineChart.YAxis.GetCurrentCoordinateCalculator();

                for (var i = 0; i < sciLineSeries1.DataSeries.Count; i++)
                {
                    if (extendedPoints1 == null || extendedPoints1[i].ExtraData == null)
                        continue;

                    var xCoord = xCalc.GetCoordinate((double)sciLineSeries1.DataSeries.XValues[i]);
                    var yCoord = yCalc.GetCoordinate((double)sciLineSeries1.DataSeries.YValues[i]);
                    dataPointsAnnotationModifier.AddAnnotation(xCoord, yCoord, extendedPoints1[i], dataPointsAnnotationModifier1Style);
                }

                if (sciLineSeries2.DataSeries != null)
                {
                    for (var i = 0; i < sciLineSeries2.DataSeries.Count; i++)
                    {
                        if (extendedPoints2 == null || extendedPoints2[i].ExtraData == null)
                            continue;

                        var xCoord = xCalc.GetCoordinate((double)sciLineSeries2.DataSeries.XValues[i]);
                        var yCoord = yCalc.GetCoordinate((double)sciLineSeries2.DataSeries.YValues[i]);
                        dataPointsAnnotationModifier.AddAnnotation(xCoord, yCoord, extendedPoints2[i], dataPointsAnnotationModifier2Style);
                    }
                }
            }
            finally
            {
                sciLineChart.Rendered -= sciLineChart_Rendered;
            }

            dataPointsAnnotationModifier.ShowAnnotations(ChartSeconds <= SHOW_DATAPOINTS_SECONDS);
        }

        private void rubberBandXyZoomModifier_OnVisibleRangeChanged(double minMousePointX, double maxMousePointX)
        {
            var xCalc = sciLineChart.XAxis.GetCurrentCoordinateCalculator();
            var minValue = Math.Round(xCalc.GetDataValue(minMousePointX));
            var maxValue = Math.Round(xCalc.GetDataValue(maxMousePointX));

            if (minValue < 0)
                minValue = 0;
            if (maxValue > ChartSeconds)
                maxValue = ChartSeconds;

            dataPointsAnnotationModifier.ShowAnnotations((maxValue - minValue) <= SHOW_DATAPOINTS_SECONDS);
            //if ((maxValue - minValue) < 1)
            //{
            //    if (minValue > 0) minValue--;
            //    else 
            //    if (maxValue < ChartSeconds) maxValue++;
            //}

            if (VisibleRangeChanged != null)
                VisibleRangeChanged(minValue, maxValue);
        }

        private void zoomExtentsModifier_OnVisibleRangeChanged(double minValue, double maxValue)
        {
            dataPointsAnnotationModifier.ShowAnnotations(Math.Abs(maxValue - minValue) <= SHOW_DATAPOINTS_SECONDS);

            if (VisibleRangeChanged != null)
                VisibleRangeChanged(minValue, maxValue);
        }
        #endregion
    }
}
