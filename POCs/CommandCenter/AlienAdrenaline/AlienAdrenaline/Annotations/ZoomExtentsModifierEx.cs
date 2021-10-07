using System;
using System.Windows;
using Abt.Controls.SciChart.ChartModifiers;

namespace AlienLabs.AlienAdrenaline.App.Annotations
{
    public class ZoomExtentsModifierEx : ChartModifierBase
    {
        public event Action<double, double> VisibleRangeChanged;
        public static readonly DependencyProperty IsAnimatedProperty = DependencyProperty.Register("IsAnimated", typeof(bool), typeof(ZoomExtentsModifierEx), new PropertyMetadata(false));

        private static readonly DependencyProperty chartSecondsProperty = DependencyProperty.Register("ChartSeconds", typeof(int), typeof(ZoomExtentsModifierEx), new UIPropertyMetadata(0));
        public int ChartSeconds
        {
            get { return (int)GetValue(chartSecondsProperty); }
            set { SetValue(chartSecondsProperty, value); }
        }

        public ZoomExtentsModifierEx()
        {
            ReceiveHandledEvents = true;
        }

        public bool IsAnimated
        {
            get { return (bool)GetValue(IsAnimatedProperty); }
            set { SetValue(IsAnimatedProperty, value); }
        }

        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);
            e.Handled = true;

            if (ParentSurface == null) return;

            //if (IsAnimated)
            //    XAxis.AnimateVisibleRangeTo(XAxis.GetMaximumRange(), TimeSpan.FromMilliseconds(500));
            //else
            //    XAxis.VisibleRange = XAxis.GetMaximumRange();

            if (VisibleRangeChanged != null)
                VisibleRangeChanged(0, ChartSeconds);
        }
    }

}

