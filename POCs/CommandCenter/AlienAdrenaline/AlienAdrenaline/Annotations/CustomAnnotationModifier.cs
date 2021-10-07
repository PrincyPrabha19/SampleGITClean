using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Abt.Controls.SciChart;
using Abt.Controls.SciChart.ChartModifiers;
using Abt.Controls.SciChart.Visuals.Annotations;
using Abt.Controls.SciChart.Visuals.RenderableSeries;
using TinyMessenger;

namespace AlienLabs.AlienAdrenaline.App.Annotations
{
    public class CustomAnnotationModifier : ChartModifierBase
    {
        private IList<CustomAnnotation> _annotations = new List<CustomAnnotation>();
        private TinyMessageSubscriptionToken _eventSubscription;
        private bool _areAnnotationsVisible;

        // Occurs when the modifier is attached to the parent surface
        public override void OnAttached()
        {
            base.OnAttached();
            _eventSubscription = Services.GetService<IEventAggregator>().Subscribe<SciChartRenderedMessage>(OnScichartSurfaceRendered);
        }

        public override void OnDetached()
        {
            base.OnDetached();
            Services.GetService<IEventAggregator>().Unsubscribe<SciChartRenderedMessage>(_eventSubscription);
        }

        public override void OnModifierDoubleClick(ModifierMouseArgs e)
        {
            base.OnModifierDoubleClick(e);

            AddAnnotation(e.MousePoint);
        }

        // Called each time the SciChartSurface is rendered
        private void OnScichartSurfaceRendered(SciChartRenderedMessage obj)
        {
            UpdateAnnotations();
        }

        private void UpdateAnnotations()
        {
            foreach (var annotation in this._annotations)
            {
                double xCoord1 = XAxis.GetCoordinate(annotation.X1);
                double yCoord1 = YAxis.GetCoordinate(annotation.Y1);

                Canvas.SetLeft(annotation, xCoord1);
                Canvas.SetTop(annotation, yCoord1);

                annotation.InvalidateVisual();
            }
        }

        private void AddAnnotation(Point mousePoint)
        {
            var annotation = new CustomAnnotation();

            annotation.X1 = (double)XAxis.HitTest(mousePoint).DataValue;
            annotation.Y1 = (double)YAxis.HitTest(mousePoint).DataValue;

            _annotations.Add(annotation);
            ModifierSurface.Children.Add(annotation);
            UpdateAnnotations();
        }

        public void AddAnnotation(double xVal, double yVal, object dataContext, Style style)
        {
            var mousePoint = new Point(xVal, yVal);
            var annotation = new CustomAnnotation() { Visibility = Visibility.Collapsed };

            annotation.X1 = (double)XAxis.HitTest(mousePoint).DataValue;
            annotation.Y1 = (double)YAxis.HitTest(mousePoint).DataValue;            
            annotation.DataContext = dataContext;
            annotation.Style = style;

            _annotations.Add(annotation);
            ModifierSurface.Children.Add(annotation);
            UpdateAnnotations();
        }

        public void AddAnnotation(double xVal, double yVal, Style style)
        {
            var mousePoint = new Point(xVal, yVal);
            var annotation = new CustomAnnotation();

            annotation.X1 = (double)XAxis.HitTest(mousePoint).DataValue;
            annotation.Y1 = (double)YAxis.HitTest(mousePoint).DataValue;
            annotation.Content = Math.Round((double)annotation.X1);
            annotation.Style = style;

            _annotations.Add(annotation);
            ModifierSurface.Children.Add(annotation);
            UpdateAnnotations();
        }

        public void ClearAnnotations()
        {
            foreach (var annotation in _annotations)
            {
                ModifierSurface.Children.Remove(annotation);
            }
            _annotations.Clear();
            UpdateAnnotations();
        }

        public void ShowAnnotations(bool show)
        {
            if (_areAnnotationsVisible != show)
            {
                _areAnnotationsVisible = show;
                foreach (var annotation in this._annotations)
                    annotation.Visibility = show ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private IRenderableSeries GetRenderableSeries(string seriesName)
        {
            // Iterate over all RenderableSeries
            foreach (var renderableSeries in ParentSurface.RenderableSeries)
            {
                // Get associated data series (populated internally by SciChart)
                var dataSeries = renderableSeries.DataSeries;
                if (dataSeries == null)
                    continue;

                // Match the name
                if (dataSeries.SeriesName == seriesName)
                {
                    return renderableSeries;
                }
            }

            return null;
        }
    }
}

