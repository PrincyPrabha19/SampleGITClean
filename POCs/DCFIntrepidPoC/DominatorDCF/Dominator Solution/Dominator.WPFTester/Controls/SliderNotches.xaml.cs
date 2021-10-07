using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace DominatorTester.Controls
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

        private static readonly DependencyProperty selectedValueProperty = DependencyProperty.Register("SelectedValue", typeof(KeyValuePair<int, decimal>), typeof(SliderNotches), new UIPropertyMetadata(new KeyValuePair<int, decimal>()));
        public KeyValuePair<int, decimal> SelectedValue
        {
            get { return (KeyValuePair<int, decimal>)GetValue(selectedValueProperty); }
            set { SetValue(selectedValueProperty, value); }
        }

        private static readonly DependencyProperty notchWithProperty = DependencyProperty.Register("NotchWith", typeof(int), typeof(SliderNotches), new UIPropertyMetadata(5));
        public int NotchWith
        {
            get { return (int)GetValue(notchWithProperty); }
            set { SetValue(notchWithProperty, value); }
        }

        private static readonly DependencyProperty rangeRenderWithProperty = DependencyProperty.Register("RangeRenderWith", typeof(int), typeof(SliderNotches), new UIPropertyMetadata(1000));
        public int RangeRenderWith
        {
            get { return (int)GetValue(rangeRenderWithProperty); }
            set { SetValue(rangeRenderWithProperty, value); }
        }
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
            if (Ranges == null)
                return;

            foreach (var range in Ranges)
            {
                var stackPanel = new StackPanel { Orientation = Orientation.Horizontal, Width = RangeRenderWith };
                var notchPercentWith = (range.Max - range.Min) * ((decimal)NotchWith/100);
                var notchRenderWith = RangeRenderWith * notchPercentWith;

                
            }
        }
        #endregion

        #region Event Handlers
        private static void renderRanges(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            (d as SliderNotches)?.renderControl();
        }
        #endregion
    }
}
