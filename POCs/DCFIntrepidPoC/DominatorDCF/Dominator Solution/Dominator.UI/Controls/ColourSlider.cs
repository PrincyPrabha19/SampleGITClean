using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace Dominator.UI.Controls
{
    public class ColourSlider : Slider
    {
        #region Properties
        public static readonly DependencyProperty yellowStartValueProperty = DependencyProperty.Register("YellowStartValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)0, onCalculateGradientIsNeeded));
        public decimal YellowStartValue
        {
            get { return (decimal)GetValue(yellowStartValueProperty); }
            set { SetValue(yellowStartValueProperty, value); }
        }

        public static readonly DependencyProperty redStartValueProperty = DependencyProperty.Register("RedStartValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)0, onCalculateGradientIsNeeded));
        public decimal RedStartValue
        {
            get { return (decimal)GetValue(redStartValueProperty); }
            set { SetValue(redStartValueProperty, value); }
        }

        public static readonly DependencyProperty minimumValueProperty = DependencyProperty.Register("MinimumValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)int.MinValue, onCalculateGradientIsNeeded));
        public decimal MinimumValue
        {
            get { return (decimal)GetValue(minimumValueProperty); }
            set { SetValue(minimumValueProperty, value); }
        }

        public static readonly DependencyProperty maximumValueProperty = DependencyProperty.Register("MaximumValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)int.MaxValue, onCalculateGradientIsNeeded));
        public decimal MaximumValue
        {
            get { return (decimal)GetValue(maximumValueProperty); }
            set { SetValue(maximumValueProperty, value); }
        }

        public static readonly DependencyProperty minimumLimitValueProperty = DependencyProperty.Register("MinimumLimitValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)-1, onCalculateLimits));
        public decimal MinimumLimitValue
        {
            get { return (decimal)GetValue(minimumLimitValueProperty); }
            set { SetValue(minimumLimitValueProperty, value); }
        }

        public static readonly DependencyProperty maximumLimitValueProperty = DependencyProperty.Register("MaximumLimitValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)-1, onCalculateLimits));
        public decimal MaximumLimitValue
        {
            get { return (decimal)GetValue(maximumLimitValueProperty); }
            set { SetValue(maximumLimitValueProperty, value); }
        }

        public static readonly DependencyProperty selectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(decimal), typeof(ColourSlider),
            new UIPropertyMetadata((decimal)int.MinValue, onSelectedValueChanged));
        public decimal SelectedValue
        {
            get { return (decimal)GetValue(selectedValueProperty); }
            set { SetValue(selectedValueProperty, value); }
        }

        public static readonly DependencyProperty dragCompletedCommandProperty = DependencyProperty.Register("DragCompletedCommand", typeof(ICommand), typeof(ColourSlider));
        public ICommand DragCompletedCommand
        {
            get { return (ICommand)GetValue(dragCompletedCommandProperty); }
            set { SetValue(dragCompletedCommandProperty, value); }
        }

        public static readonly DependencyProperty decimalsProperty = DependencyProperty.Register("Decimals", typeof(int), typeof(ColourSlider));
        public int Decimals
        {
            get { return (int)GetValue(decimalsProperty); }
            set { SetValue(decimalsProperty, value); }
        }

        public decimal StepRatio { get; private set; }

        private decimal minimumLimitValueToDraw = -1;
        private decimal maximumLimitValueToDraw = -1;
        #endregion

        #region Events
        public event Action<decimal> SelectedValueChanged;
        #endregion

        #region Constructors
        static ColourSlider()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ColourSlider), new FrameworkPropertyMetadata(typeof(ColourSlider)));
        }

        public ColourSlider()
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
            (dependencyObject as ColourSlider)?.calculateGradient();
        }

        private static void onCalculateLimits(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            (dependencyObject as ColourSlider)?.calculateRatioAndLimits();
        }

        private static void onSelectedValueChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var slider = dependencyObject as ColourSlider;
            if (slider == null)
                return;

            var roundedValue = Math.Round(slider.SelectedValue, slider.Decimals);
            if (slider.SelectedValue != roundedValue)
                slider.SelectedValue = roundedValue;

            slider.Value = (double)((roundedValue - slider.MinimumValue) * slider.StepRatio);
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
            if (MinimumLimitValue >= 0 && MaximumLimitValue >= 0 && MinimumLimitValue < MaximumLimitValue)
            {
                if (newValue > oldValue && newValue >= maximumLimitValueToDraw)
                    newValue = maximumLimitValueToDraw;
                else
                if (newValue < oldValue && newValue <= minimumLimitValueToDraw)
                    newValue = minimumLimitValueToDraw;

                Value = (double)newValue;
            }

            SelectedValue = Math.Round((newValue / StepRatio) + MinimumValue, Decimals);
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
            calculateRatioAndLimits();

            var yellowStartValue = ((YellowStartValue - MinimumValue) * StepRatio) / (decimal)(Maximum - Minimum);
            var redStartValue = ((RedStartValue - MinimumValue) * StepRatio) / (decimal)(Maximum - Minimum);

            var brush = new LinearGradientBrush(new GradientStopCollection
            {
                new GradientStop(Color.FromRgb(0x67, 0x98, 0x1a), 0.0),
                new GradientStop(Color.FromRgb(0x67, 0x98, 0x1a), (double)yellowStartValue),
                new GradientStop(Color.FromRgb(0xcb, 0x98, 0x22), (double)redStartValue),
                new GradientStop(Color.FromRgb(0xca, 0x08, 0x14), 1)
            });

            Background = brush;
        }

        private void calculateRatioAndLimits()
        {
            StepRatio = (decimal)(Maximum - Minimum) / (MaximumValue - MinimumValue);

            minimumLimitValueToDraw = -1;
            maximumLimitValueToDraw = -1;
            if (MinimumLimitValue >= 0 && MaximumLimitValue >= 0 && MinimumLimitValue < MaximumLimitValue)
            {
                minimumLimitValueToDraw = (MinimumLimitValue - MinimumValue) * StepRatio;
                maximumLimitValueToDraw = (MaximumLimitValue - MinimumValue) * StepRatio;
            }

            InvalidateVisual();
        }

        #endregion
    }
}
