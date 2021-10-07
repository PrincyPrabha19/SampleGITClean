
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using AlienLabs.AlienAdrenaline.PerformanceMonitoring.Classes;

namespace AlienLabs.AlienAdrenaline.App.Views
{
    public interface ComponentChartView
    {
        event Action<double, double> VisibleRangeChanged;
        event Action<ComponentChartView, bool> ChartMaximizedMinimized;
        event Action<string> ChartStatusChanged;

        Visibility Visibility { get; set; }
        double Width { get; set; }
        double Height { get; set; }

        bool IsChartTitleEnabled { get; set; }
        bool IsMaximized { get; set; }
        bool IsMainChart { get; }
        double IntervalX { get; set; }
        double IntervalY { get; set; }
        double MaximumX { get; set; }
        double MaximumY { get; set; }
        double MinimumX { get; set; }
        SolidColorBrush LineSeries1Color { get; set; }
        SolidColorBrush LineSeries2Color { get; set; }

        int ChartSeconds { get; set; }
        void SetChartValues(List<ChartBufferData> values);
        void SetChartValues(List<ChartBufferData> values1, List<ChartBufferData> values2);
        void RefreshLayout();

        void SetChartVisibleRange(double minimum, double maximum, double interval);
    }
}
