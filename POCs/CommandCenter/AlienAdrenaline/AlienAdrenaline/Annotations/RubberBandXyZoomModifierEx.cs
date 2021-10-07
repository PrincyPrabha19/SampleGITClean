using System;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;

namespace AlienLabs.AlienAdrenaline.App.Annotations
{
    public class RubberBandXyZoomModifierEx : RubberBandXyZoomModifier
    {
        public event Action<double, double> VisibleRangeChanged;

        private static readonly DependencyProperty annotationStyleProperty = DependencyProperty.Register("AnnotationStyle", typeof(Style), typeof(RubberBandXyZoomModifierEx));
        public Style AnnotationStyle
        {
            get { return (Style)GetValue(annotationStyleProperty); }
            set { SetValue(annotationStyleProperty, value); }
        }

        private CustomAnnotation annotation1;
        private CustomAnnotation annotation2;

        private double minMousePointX;
        private double maxMousePointX;

        public RubberBandXyZoomModifierEx()
        {
            annotation1 = new CustomAnnotation();
            annotation2 = new CustomAnnotation();
        }

        private bool initialized;
        public override void OnModifierMouseDown(ModifierMouseArgs e)
        {
            if (!initialized)
            {
                initialized = true;
                annotation1.Style = AnnotationStyle;
                annotation2.Style = AnnotationStyle;
                ModifierSurface.Children.Add(annotation1);
                ModifierSurface.Children.Add(annotation2);
            }

            base.OnModifierMouseDown(e);
            minMousePointX = e.MousePoint.X;
            
            annotation1.Content = Math.Round((double) XAxis.HitTest(e.MousePoint).DataValue);
            Canvas.SetLeft(annotation1, minMousePointX);
            Canvas.SetTop(annotation1, 0);
            annotation1.InvalidateVisual();
        }

        public override void OnModifierMouseMove(ModifierMouseArgs e)
        {
            base.OnModifierMouseMove(e);

            if (IsDragging && Math.Abs(minMousePointX - e.MousePoint.X) > 1)
            {
                annotation1.Visibility = Visibility.Visible;
                annotation2.Visibility = Visibility.Visible;
                annotation2.Content = Math.Round((double)XAxis.HitTest(e.MousePoint).DataValue);
                Canvas.SetLeft(annotation2, e.MousePoint.X);
                Canvas.SetBottom(annotation2, 0);
                annotation2.InvalidateVisual();
            }
        }

        public override void OnModifierMouseUp(ModifierMouseArgs e)
        {
            base.OnModifierMouseUp(e);

            annotation1.Visibility = Visibility.Hidden;
            annotation2.Visibility = Visibility.Hidden;

            maxMousePointX = e.MousePoint.X;
            if (Math.Abs(minMousePointX - maxMousePointX) < 0.0001)
                return;

            if (maxMousePointX < minMousePointX)
            {
                maxMousePointX = minMousePointX;
                minMousePointX = e.MousePoint.X;
            }

            if (VisibleRangeChanged != null)
                VisibleRangeChanged(minMousePointX, maxMousePointX);
        }
    }
}
