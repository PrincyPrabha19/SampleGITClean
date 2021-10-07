using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Dominator.UI.Controls
{
    public partial class SliderNotches
    {
        #region Properties
        private static readonly DependencyProperty rangesProperty = DependencyProperty.Register("Ranges", typeof(List<ColourSliderRange>), typeof(SliderNotches), new UIPropertyMetadata(new List<ColourSliderRange>(), renderRanges));
        public List<ColourSliderRange> Ranges
        {
            get { return (List<ColourSliderRange>)GetValue(rangesProperty); }
            set { SetValue(rangesProperty, value); }
        }

        private static readonly DependencyProperty selectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(KeyValuePair<int, decimal>), typeof(SliderNotches), new UIPropertyMetadata(new KeyValuePair<int, decimal>(), selectedValueChanged));

        public KeyValuePair<int, decimal> SelectedValue
        {
            get { return (KeyValuePair<int, decimal>)GetValue(selectedValueProperty); }
            set { SetValue(selectedValueProperty, value); }
        }

        private static readonly DependencyProperty notchWithProperty = DependencyProperty.Register("NotchWith", typeof(int), typeof(SliderNotches), new UIPropertyMetadata(20));
        public int NotchWith
        {
            get { return (int)GetValue(notchWithProperty); }
            set { SetValue(notchWithProperty, value); }
        }

        private static readonly DependencyProperty rangeRenderWithProperty = DependencyProperty.Register("RangeRenderWith", typeof(int), typeof(SliderNotches), new UIPropertyMetadata(100));
        public int RangeRenderWith
        {
            get { return (int)GetValue(rangeRenderWithProperty); }
            set { SetValue(rangeRenderWithProperty, value); }
        }

        private Dominator.UI.Controls.Notch selectedNotch;
        #endregion

        #region Constructor
        public SliderNotches()
        {
            InitializeComponent();
            renderControl();
        }
        #endregion

        #region Methods
        private void renderControl()
        {
            string styleNameDefault = "NotchesDefault";
            if (Ranges == null)
                return;

            stackLayout.Children.Clear();
            selectedNotch = null;
            Color? prevRangeColor = null;
            foreach (var range in Ranges)
            {
                var stackPanel = new StackPanel
                {
                    Orientation = Orientation.Horizontal,
                    Width = RangeRenderWith,
                    Height = Height
                };

                var items = 100/NotchWith;
                var notchRenderWith = RangeRenderWith * ((decimal)NotchWith/100);
                

                for (var i = 0; i < items; i++)
                {
                    Notch notch;
                    if (i == 0 && prevRangeColor !=null)
                    {
                        LinearGradientBrush myLinearGradientBrush = new LinearGradientBrush
                        {
                            StartPoint = new Point(0, 0),
                            EndPoint = new Point(1, 0)
                        };
                        myLinearGradientBrush.GradientStops.Add(
                            new GradientStop(prevRangeColor.Value, 0));
                        myLinearGradientBrush.GradientStops.Add(
                            new GradientStop(range.Color, 1));
                        notch = new Dominator.UI.Controls.Notch
                        {
                            Height = Height,
                            Width = (double)notchRenderWith,
                            Foreground = myLinearGradientBrush
                        };

                        stackPanel.Children.Add(notch);
                        continue;
                    }
                    
                    notch = new Dominator.UI.Controls.Notch
                    {
                        Height = Height,
                        Width = (double)notchRenderWith,
                        Foreground = new SolidColorBrush(range.Color)
                    };

                    stackPanel.Children.Add(notch);
                }

                stackLayout.Children.Add(stackPanel);
                prevRangeColor = range.Color;
            }
        }

        private void selectNotch(KeyValuePair<int, decimal> newValue)
        {
            if (Ranges == null || Ranges.Count == 0)
                return;

            if (newValue.Key < 0  || newValue.Key > Ranges.Count -1)
                return;

            if (selectedNotch != null)
                selectedNotch.Selected = false;

            var notchWith = (Ranges[newValue.Key].Max - Ranges[newValue.Key].Min) * ((decimal)NotchWith / 100);
            int notchIndexToSelect = (int)(newValue.Value/notchWith);
            if (notchIndexToSelect != 0)
                notchIndexToSelect -= 1;
            var panel = (stackLayout.Children[newValue.Key] as StackPanel);
            if (panel == null)
                return;

            var notch = (panel.Children[notchIndexToSelect] as Dominator.UI.Controls.Notch);
            selectedNotch = notch;
            if (selectedNotch != null)
                selectedNotch.Selected = true;
        }
        #endregion

        #region Event Handlers
        private static void renderRanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SliderNotches)?.renderControl();
        }

        private static void selectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SliderNotches)?.selectNotch((KeyValuePair<int, decimal>)e.NewValue);
        }
        #endregion
    }
}
