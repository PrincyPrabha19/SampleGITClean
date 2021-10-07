using System.Collections.Generic;
using System.Linq;
using Abt.Controls.SciChart.Rendering.Common;
using Abt.Controls.SciChart.Visuals.PointMarkers;

namespace AlienLabs.AlienAdrenaline.App.Annotations
{
    public class EllipsePointMarkerExtended : EllipsePointMarker
    {
        protected override void DrawInternal(IRenderContext2D context, IEnumerable<PointF> centers, IPen2D pen, IBrush2D brush)
        {
            var viewportRect = new RectF(new PointF(0, 0), context.ViewportSize);

            var _centers = centers.ToList();
            for (var i = 0; i < _centers.Count; i++)
            {
                if (i % 30 == 0 && IsInBounds(_centers[i], (float)Width, (float)Height, viewportRect))
                    context.DrawEllipse(Width, Height, _centers[i], brush, pen);
            }
        }

        private bool IsInBounds(PointF centre, float width, float height, RectF viewportRect)
        {
            var markerRect = new RectF(centre.X - width / 2, centre.Y - height / 2, width, height);
            viewportRect.Intersect(markerRect);
            return !viewportRect.IsEmpty;
        }
    }
}
