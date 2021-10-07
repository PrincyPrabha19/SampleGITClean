
#define DEBUG

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace AlienLabs.AlienAdrenaline.App.Controls
{
    public partial class TimeSpanControl
    {
        public event Action<double, double> TimeSpanSelectionCompleted;
        public event Action<double, double> TimeSpanSelectionStarted;
        public event Action<double, double> TimeSpanSelectionChanged;

        #region Properties
        private static readonly DependencyProperty chartSecondsProperty = DependencyProperty.Register("ChartSeconds", typeof(int), typeof(TimeSpanControl), new UIPropertyMetadata(60));
        public int ChartSeconds
        {
            get { return (int)GetValue(chartSecondsProperty); }
            set { SetValue(chartSecondsProperty, value); }
        }

        private static readonly DependencyProperty minimumProperty = DependencyProperty.Register("Minimum", typeof(double), typeof(TimeSpanControl), new UIPropertyMetadata(0d));
        public double Minimum
        {
            get { return (double)GetValue(minimumProperty); }
            set { SetValue(minimumProperty, value); }
        }

        private static readonly DependencyProperty maximumProperty = DependencyProperty.Register("Maximum", typeof(double), typeof(TimeSpanControl), new UIPropertyMetadata(60d));
        public double Maximum
        {
            get { return (double)GetValue(maximumProperty); }
            set { SetValue(maximumProperty, value); }
        }
        #endregion

        #region Private Properties
        private const int SLIDERS_GAP = 60;
        private bool dragStarted;
        private bool dragMiddleStarted;
        private bool dragMaximumStarted;
        private bool dragMinimumStarted;
        #endregion

        #region Public Methods
        public void ResetControl()
        {
            refreshBorderTimespanSelection(Minimum, Maximum);
        }
        #endregion

        #region Constructor
        public TimeSpanControl()
        {
            InitializeComponent();

            sliderMinimum.ValueChanged += sliderMinimum_OnValueChanged;
            sliderMaximum.ValueChanged += sliderMaximum_OnValueChanged; 
        }
        #endregion

        #region Private Methods
        private void refreshBorderTimespanSelection(double minimum, double maximum)
        {
            const int margin = 10;
            const double actualWidth = 698;
            double perc = actualWidth / ChartSeconds;
            double minNor = (minimum) * perc;
            double maxNor = (maximum) * perc;

            borderSelection.Margin = new Thickness(minNor >= 0 ? minNor + margin : margin, 0, 0, 0);
            borderSelection.Width = maxNor >= 0 ? maxNor - minNor - margin : actualWidth - minNor - margin;

            sliderMiddle.Visibility = minimum > 0 || maximum < ChartSeconds ? Visibility.Visible : Visibility.Collapsed;
        }
        #endregion

        #region Event Handlers
        private void sliderMinimum_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue > sliderMaximum.Value - SLIDERS_GAP)
            {
                sliderMinimum.Value = e.OldValue;
                return;
            }
           
            if (dragStarted)
            {
                if (dragMinimumStarted)
                {
                    dragMinimumStarted = false;
                    if (TimeSpanSelectionCompleted != null && e.NewValue < e.OldValue)
                        TimeSpanSelectionCompleted(0, sliderMaximum.Maximum);
                }

                refreshBorderTimespanSelection(sliderMinimum.Value, sliderMaximum.Value);
                if (TimeSpanSelectionChanged != null)
                    TimeSpanSelectionChanged((int) Minimum, (int) Maximum);
            }
        }

        private void sliderMaximum_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (e.NewValue < sliderMinimum.Value + SLIDERS_GAP)
            {
                sliderMaximum.Value = e.OldValue;
                return;
            }            

            if (dragStarted)
            {
                if (dragMaximumStarted)
                {
                    dragMaximumStarted = false;
                    if (TimeSpanSelectionCompleted != null && e.NewValue > e.OldValue)
                        TimeSpanSelectionCompleted(0, sliderMaximum.Maximum);
                }

                refreshBorderTimespanSelection(sliderMinimum.Value, sliderMaximum.Value);
                if (TimeSpanSelectionChanged != null)
                    TimeSpanSelectionChanged((int) Minimum, (int) Maximum);
            }
        }

        private void sliderMinimum_ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            dragStarted = true;
            dragMinimumStarted = true;
            textBlockMinValue.Visibility = Visibility.Visible;
            if (TimeSpanSelectionStarted != null)
                TimeSpanSelectionStarted((int)Minimum, (int)Maximum);
        }

        private void sliderMinimum_ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragStarted = false;
            dragMinimumStarted = false;
            textBlockMinValue.Visibility = Visibility.Collapsed;
            if (TimeSpanSelectionCompleted != null)
                TimeSpanSelectionCompleted((int)Minimum, (int)Maximum);
        }

        private void sliderMaximum_ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            dragStarted = true;
            dragMaximumStarted = true;
            textBlockMaxValue.Visibility = Visibility.Visible;
            if (TimeSpanSelectionStarted != null)
                TimeSpanSelectionStarted((int)Minimum, (int)Maximum);
        }

        private void sliderMaximum_ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            dragStarted = false;
            dragMaximumStarted = false;
            textBlockMaxValue.Visibility = Visibility.Collapsed;
            if (TimeSpanSelectionCompleted != null)
                TimeSpanSelectionCompleted((int)Minimum, (int)Maximum);
        }

        private void sliderMiddle_ThumbDragStarted(object sender, DragStartedEventArgs e)
        {
            dragMiddleStarted = true;
        }

        private void sliderMiddle_ThumbDragCompleted(object sender, DragCompletedEventArgs e)
        {
            textBlockMinValue.Visibility = Visibility.Collapsed;
            textBlockMaxValue.Visibility = Visibility.Collapsed;

            if (TimeSpanSelectionCompleted != null && Minimum < Maximum)
                TimeSpanSelectionCompleted((int)Minimum, (int)Maximum);
        }

        private bool refreshingBorderTimespanSelection;
        private void sliderMiddle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (refreshingBorderTimespanSelection)
                return;

            if (dragMiddleStarted)
            {
                dragMiddleStarted = false;
                textBlockMinValue.Visibility = Visibility.Visible;
                textBlockMaxValue.Visibility = Visibility.Visible;
                if (TimeSpanSelectionCompleted != null)
                    TimeSpanSelectionCompleted(0, sliderMaximum.Maximum);
            }

            try
            {
                refreshingBorderTimespanSelection = true;

                var minimum = (int)Minimum;
                var maximum = (int)Maximum;
                if (e.NewValue > e.OldValue)
                {
                    if (maximum + 1 <= ChartSeconds)
                    {
                        Maximum = maximum + 1;
                        Minimum = minimum + 1;
                        refreshBorderTimespanSelection(sliderMinimum.Value, sliderMaximum.Value);
                        if (TimeSpanSelectionChanged != null)
                            TimeSpanSelectionChanged((int)sliderMinimum.Value, (int)sliderMaximum.Value);
                    }
                }
                else
                if (e.NewValue < e.OldValue)
                {
                    if (minimum - 1 >= 0)
                    {
                        Minimum = minimum - 1;
                        Maximum = maximum - 1;
                        refreshBorderTimespanSelection(sliderMinimum.Value, sliderMaximum.Value);
                        if (TimeSpanSelectionChanged != null)
                            TimeSpanSelectionChanged((int)sliderMinimum.Value, (int)sliderMaximum.Value);
                    }
                }

                sliderMiddle.Value = 0;
            }
            finally
            {
                refreshingBorderTimespanSelection = false;
            }
        }

        private void sliderMiddle_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            sliderMinimum.Value = 0;
            sliderMaximum.Value = sliderMaximum.Maximum;

            refreshBorderTimespanSelection(sliderMinimum.Value, sliderMaximum.Value);
            if (TimeSpanSelectionCompleted != null)
                TimeSpanSelectionCompleted(0, sliderMaximum.Maximum);
        }
        #endregion
    }
}
