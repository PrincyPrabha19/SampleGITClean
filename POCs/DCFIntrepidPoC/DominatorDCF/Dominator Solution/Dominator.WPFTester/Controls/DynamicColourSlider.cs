using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace DominatorTester.Controls
{
    public class DynamicColourSlider : Slider
    {
        #region Properties
        public static readonly DependencyProperty stopsProperty = DependencyProperty.Register("Ranges", typeof(List<ColourSliderRange>), typeof(DynamicColourSlider),
            new UIPropertyMetadata(new List<ColourSliderRange>(), onCalculateGradientIsNeeded));
        public List<ColourSliderRange> Ranges
        {
            get { return (List<ColourSliderRange>)GetValue(stopsProperty); }
            set { SetValue(stopsProperty, value); }
        }

        public static readonly DependencyProperty minimumLimitValueProperty = DependencyProperty.Register("MinimumLimitValue", typeof(decimal), typeof(DynamicColourSlider),
            new UIPropertyMetadata((decimal)-1, onCalculateLimits));
        public decimal MinimumLimitValue
        {
            get { return (decimal)GetValue(minimumLimitValueProperty); }
            set { SetValue(minimumLimitValueProperty, value); }
        }

        public static readonly DependencyProperty maximumLimitValueProperty = DependencyProperty.Register("MaximumLimitValue", typeof(decimal), typeof(DynamicColourSlider),
            new UIPropertyMetadata((decimal)-1, onCalculateLimits));
        public decimal MaximumLimitValue
        {
            get { return (decimal)GetValue(maximumLimitValueProperty); }
            set { SetValue(maximumLimitValueProperty, value); }
        }

        public static readonly DependencyProperty selectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(decimal), typeof(DynamicColourSlider),
            new UIPropertyMetadata((decimal)int.MinValue, onSelectedValueChanged));
        public decimal SelectedValue
        {
            get { return (decimal)GetValue(selectedValueProperty); }
            set { SetValue(selectedValueProperty, value); }
        }

        public static readonly DependencyProperty dragCompletedCommandProperty = DependencyProperty.Register("DragCompletedCommand", typeof(ICommand), typeof(DynamicColourSlider));
        public ICommand DragCompletedCommand
        {
            get { return (ICommand)GetValue(dragCompletedCommandProperty); }
            set { SetValue(dragCompletedCommandProperty, value); }
        }

        public static readonly DependencyProperty decimalsProperty = DependencyProperty.Register("Decimals", typeof(int), typeof(DynamicColourSlider));
        public int Decimals
        {
            get { return (int)GetValue(decimalsProperty); }
            set { SetValue(decimalsProperty, value); }
        }

        private decimal realTransitionPercent;
        public static readonly DependencyProperty transitionPercentProperty = DependencyProperty.Register("TransitionPercent", typeof(int), typeof(DynamicColourSlider), new PropertyMetadata(10));
        public int TransitionPercent
        {
            get { return (int)GetValue(transitionPercentProperty); }
            set
            {
                if (value < 0)
                    value = 0;
                else if (value > 100)
                    value = 100;

                SetValue(transitionPercentProperty, value);
                realTransitionPercent = value/100M;
            }
        }

        public decimal StepRatio { get; private set; }

        private decimal minimumLimitValueToDraw = -1;
        private decimal maximumLimitValueToDraw = -1;
        #endregion

        #region Events
        public event Action<decimal> SelectedValueChanged;
        #endregion

        #region Constructors
        static DynamicColourSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DynamicColourSlider), new FrameworkPropertyMetadata(typeof(DynamicColourSlider)));
        }

        public DynamicColourSlider()
        {
            Minimum = 0;
            Maximum = 1000;
            LargeChange = 50;
            SmallChange = 1;
        }

        private bool dragStarted;

        protected override void OnThumbDragStarted(DragStartedEventArgs e)
        {
            dragStarted = true;
        }

        protected override void OnThumbDragCompleted(DragCompletedEventArgs e)
        {
            dragStarted = false;
            calculateNewSelectedValue((decimal)Value, (decimal)Value);
        }
        #endregion

        #region Event Handlers
        private static void onCalculateGradientIsNeeded(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as DynamicColourSlider)?.calculateGradient();
        }

        private static void onCalculateLimits(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as DynamicColourSlider)?.calculateRatioAndLimits();
        }

        private static void onSelectedValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var slider = dependencyObject as DynamicColourSlider;
            if (slider?.Ranges == null || slider.Ranges.Count < 2)
                return;

            var minimumValue = slider.Ranges[0].Min;
            var roundedValue = Math.Round(slider.SelectedValue, slider.Decimals);
            if (slider.SelectedValue != roundedValue)
                slider.SelectedValue = roundedValue;

            slider.Value = (double)((roundedValue - minimumValue) * slider.StepRatio);
        }
        #endregion

        #region Methods
        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            calculateGradient();
        }

        protected override void OnValueChanged(double oldValue, double newValue)
        {
            calculateNewSelectedValue((decimal)oldValue, (decimal)newValue);
            base.OnValueChanged(oldValue, newValue);
        }

        private void calculateNewSelectedValue(decimal oldValue, decimal newValue)
        {
            if (Ranges == null || Ranges.Count < 2)
                return;

            if (MinimumLimitValue >= 0 && MaximumLimitValue >= 0 && MinimumLimitValue < MaximumLimitValue)
            {
                if (newValue > oldValue && newValue >= maximumLimitValueToDraw)
                    newValue = maximumLimitValueToDraw;
                else
                if (newValue < oldValue && newValue <= minimumLimitValueToDraw)
                    newValue = minimumLimitValueToDraw;

                Value = (double)newValue;
            }

            var minimumValue = Ranges[0].Min;
            SelectedValue = Math.Round((newValue / StepRatio) + minimumValue, Decimals);
            SelectedValueChanged?.Invoke(SelectedValue);

            if (!dragStarted)
                DragCompletedCommand?.Execute(SelectedValue);
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            base.OnRender(drawingContext);

            if (minimumLimitValueToDraw >= 0 && maximumLimitValueToDraw >= 0)
                drawingContext.DrawRectangle(null, new Pen(Brushes.Aqua, 1.0), new Rect((double)minimumLimitValueToDraw * ActualWidth / Maximum, 0, ((double)(maximumLimitValueToDraw - minimumLimitValueToDraw) * ActualWidth / Maximum) - 5, ActualHeight));
        }

        private void calculateGradient()
        {
            if (Ranges == null || Ranges.Count < 2)
                return;

            calculateRatioAndLimits();

            var minimumValue = Ranges[0].Min;
            var gradientStops = new GradientStopCollection();

            for (int index = 0; index < Ranges.Count; index++)
            {
                var range = Ranges[index];
                double minStopValue;
                double maxStopValue;

                if (index == 0)
                    minStopValue = 0.0;
                else
                {
                    minStopValue = (double)((range.Min - minimumValue) * StepRatio)/(Maximum - Minimum);
                    minStopValue += (minStopValue * (double)realTransitionPercent);
                }

                if (index == Ranges.Count - 1)
                    maxStopValue = 1.0;
                else
                {
                    maxStopValue = (double)((range.Max - minimumValue) * StepRatio) / (Maximum - Minimum);
                    maxStopValue -= (maxStopValue * (double)realTransitionPercent);
                }

                gradientStops.Add(new GradientStop(range.Color, minStopValue));
                gradientStops.Add(new GradientStop(range.Color, maxStopValue));

                if (index < Ranges.Count - 1)
                {
                    gradientStops.Add(new GradientStop(range.Color, maxStopValue));

                    minStopValue = (double)((Ranges[index + 1].Min - minimumValue) * StepRatio) / (Maximum - Minimum);
                    minStopValue += (minStopValue * (double)realTransitionPercent);
                    gradientStops.Add(new GradientStop(Ranges[index + 1].Color, minStopValue));
                }

            }

            var brush = new LinearGradientBrush(gradientStops);
            Background = brush;
        }

        private void calculateRatioAndLimits()
        {
            if (Ranges == null || Ranges.Count < 2)
                return;

            var minimumValue = Ranges[0].Min;
            var maximumValue = Ranges[Ranges.Count - 1].Max;
            StepRatio = (decimal)(Maximum - Minimum) / (maximumValue - minimumValue);

            minimumLimitValueToDraw = -1;
            maximumLimitValueToDraw = -1;
            if (MinimumLimitValue >= 0 && MaximumLimitValue >= 0 && MinimumLimitValue < MaximumLimitValue)
            {
                minimumLimitValueToDraw = (MinimumLimitValue - minimumValue) * StepRatio;
                maximumLimitValueToDraw = (MaximumLimitValue - minimumValue) * StepRatio;
            }

            InvalidateVisual();
        }
        #endregion
    }
}
