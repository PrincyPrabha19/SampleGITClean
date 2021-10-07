using System;
using System.ComponentModel;
using Windows.Foundation;

namespace CropApp.Controls
{
    public class SelectedRegion : INotifyPropertyChanged
    {
        public const string TopLeftCornerName = "TopLeftCorner";
        public const string TopRightCornerName = "TopRightCorner";
        public const string BottomLeftCornerName = "BottomLeftCorner";
        public const string BottomRightCornerName = "BottomRightCorner";

        public event PropertyChangedEventHandler PropertyChanged;

        public double MinSelectRegionWidth { get; set; }
        public double MinSelectRegionHeight { get; set; }

        private double topLeftCornerCanvasLeft;
        public double TopLeftCornerCanvasLeft
        {
            get { return topLeftCornerCanvasLeft; }
            protected set
            {
                if (topLeftCornerCanvasLeft != value)
                {
                    topLeftCornerCanvasLeft = value;
                    OnPropertyChanged(nameof(TopLeftCornerCanvasLeft));                    
                }
            }
        }

        private double topLeftCornerCanvasTop;
        public double TopLeftCornerCanvasTop
        {
            get { return topLeftCornerCanvasTop; }
            protected set
            {
                if (topLeftCornerCanvasTop != value)
                {
                    topLeftCornerCanvasTop = value;
                    OnPropertyChanged(nameof(TopLeftCornerCanvasTop));
                }
            }
        }

        private double bottomRightCornerCanvasLeft;
        public double BottomRightCornerCanvasLeft
        {
            get { return bottomRightCornerCanvasLeft; }
            protected set
            {
                if (bottomRightCornerCanvasLeft != value)
                {
                    bottomRightCornerCanvasLeft = value;
                    OnPropertyChanged(nameof(BottomRightCornerCanvasLeft));
                }
            }
        }

        private double bottomRightCornerCanvasTop;
        public double BottomRightCornerCanvasTop
        {
            get { return bottomRightCornerCanvasTop; }
            protected set
            {
                if (bottomRightCornerCanvasTop != value)
                {
                    bottomRightCornerCanvasTop = value;
                    OnPropertyChanged(nameof(BottomRightCornerCanvasTop));
                }
            }
        }

        private Rect outerRect;
        public Rect OuterRect
        {
            get { return outerRect; }
            set
            {
                if (outerRect != value)
                {
                    outerRect = value;
                    OnPropertyChanged(nameof(OuterRect));
                }
            }
        }

        private Rect selectedRect;
        public Rect SelectedRect
        {
            get { return selectedRect; }
            protected set
            {
                if (selectedRect != value)
                {
                    selectedRect = value;
                    OnPropertyChanged(nameof(SelectedRect));
                }
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (propertyName == nameof(TopLeftCornerCanvasLeft) || propertyName == nameof(TopLeftCornerCanvasTop) ||
                propertyName == nameof(BottomRightCornerCanvasLeft) || propertyName == nameof(BottomRightCornerCanvasTop))
            {
                if (BottomRightCornerCanvasLeft - TopLeftCornerCanvasLeft > 0 && BottomRightCornerCanvasTop - topLeftCornerCanvasTop > 0)
                    SelectedRect = new Rect(TopLeftCornerCanvasLeft, TopLeftCornerCanvasTop, BottomRightCornerCanvasLeft - TopLeftCornerCanvasLeft, BottomRightCornerCanvasTop - topLeftCornerCanvasTop);
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void ResetCorner(double _topLeftCornerCanvasLeft, double _topLeftCornerCanvasTop, double _bottomRightCornerCanvasLeft, double _bottomRightCornerCanvasTop)
        {
            TopLeftCornerCanvasLeft = _topLeftCornerCanvasLeft;
            TopLeftCornerCanvasTop = _topLeftCornerCanvasTop;
            BottomRightCornerCanvasLeft = _bottomRightCornerCanvasLeft;
            BottomRightCornerCanvasTop = _bottomRightCornerCanvasTop;
        }

        public void UpdateCorner(string cornerName, double leftUpdate, double topUpdate)
        {
            UpdateCorner(cornerName, leftUpdate, topUpdate, MinSelectRegionWidth, MinSelectRegionHeight);
        }

        public void UpdateCorner(string cornerName, double leftUpdate, double topUpdate, double minWidthSize, double minHeightSize)
        {
            switch (cornerName)
            {
                case SelectedRegion.TopLeftCornerName:
                    if (topLeftCornerCanvasLeft + leftUpdate < 0 || topLeftCornerCanvasTop + topUpdate < 0) return;                   
                    TopLeftCornerCanvasLeft = validateValue(topLeftCornerCanvasLeft + leftUpdate, 0, bottomRightCornerCanvasLeft - minWidthSize);
                    TopLeftCornerCanvasTop = validateValue(topLeftCornerCanvasTop + topUpdate, 0, bottomRightCornerCanvasTop - minHeightSize);
                    break;

                case SelectedRegion.TopRightCornerName:
                    if (bottomRightCornerCanvasLeft + leftUpdate > outerRect.Width || topLeftCornerCanvasTop + topUpdate < 0) return;
                    BottomRightCornerCanvasLeft = validateValue(bottomRightCornerCanvasLeft + leftUpdate, topLeftCornerCanvasLeft + minWidthSize, outerRect.Width);
                    TopLeftCornerCanvasTop = validateValue(topLeftCornerCanvasTop + topUpdate, 0, bottomRightCornerCanvasTop - minHeightSize);
                    break;

                case SelectedRegion.BottomLeftCornerName:
                    if (topLeftCornerCanvasLeft + leftUpdate < 0 || bottomRightCornerCanvasTop + topUpdate > outerRect.Height) return;
                    TopLeftCornerCanvasLeft = validateValue(topLeftCornerCanvasLeft + leftUpdate, 0, bottomRightCornerCanvasLeft - minWidthSize);
                    BottomRightCornerCanvasTop = validateValue(bottomRightCornerCanvasTop + topUpdate, topLeftCornerCanvasTop + minHeightSize, outerRect.Height);
                    break;

                case SelectedRegion.BottomRightCornerName:
                    if (bottomRightCornerCanvasLeft + leftUpdate > outerRect.Width || bottomRightCornerCanvasTop + topUpdate > outerRect.Height) return;
                    BottomRightCornerCanvasLeft = validateValue(bottomRightCornerCanvasLeft + leftUpdate, topLeftCornerCanvasLeft + minWidthSize, outerRect.Width);
                    BottomRightCornerCanvasTop = validateValue(bottomRightCornerCanvasTop + topUpdate, topLeftCornerCanvasTop + minHeightSize, outerRect.Height);
                    break;
            }
        }

        public void UpdateSelectedRect(double scale, double leftUpdate, double topUpdate)
        {
            double width = bottomRightCornerCanvasLeft - topLeftCornerCanvasLeft;
            double height = bottomRightCornerCanvasTop - topLeftCornerCanvasTop;

            //if (scale != 1)
            //{
            //    double scaledLeftUpdate = width * (scale - 1) / 2;
            //    double scaledTopUpdate = height * (scale - 1) / 2;

            //    if (scale > 1)
            //    {
            //        UpdateCorner(SelectedRegion.BottomRightCornerName, scaledLeftUpdate, scaledTopUpdate);
            //        UpdateCorner(SelectedRegion.TopLeftCornerName, -scaledLeftUpdate, -scaledTopUpdate);
            //    }
            //    else
            //    {
            //        UpdateCorner(SelectedRegion.TopLeftCornerName, -scaledLeftUpdate, -scaledTopUpdate);
            //        UpdateCorner(SelectedRegion.BottomRightCornerName, scaledLeftUpdate, scaledTopUpdate);
            //    }

            //    return;
            //}

            double minWidth = Math.Max(MinSelectRegionWidth, width * scale);
            double minHeight = Math.Max(MinSelectRegionHeight, height * scale);

            if (leftUpdate >= 0 && topUpdate >= 0)
            {
                UpdateCorner(SelectedRegion.BottomRightCornerName, leftUpdate, topUpdate, minWidth, minHeight);
                UpdateCorner(SelectedRegion.TopLeftCornerName, leftUpdate, topUpdate, minWidth, minHeight);
            }
            else 
            if (leftUpdate >= 0 && topUpdate < 0)
            {
                UpdateCorner(SelectedRegion.TopRightCornerName, leftUpdate, topUpdate, minWidth, minHeight);
                UpdateCorner(SelectedRegion.BottomLeftCornerName, leftUpdate, topUpdate, minWidth, minHeight);
            }
            else 
            if (leftUpdate < 0 && topUpdate >= 0)
            {
                UpdateCorner(SelectedRegion.BottomLeftCornerName, leftUpdate, topUpdate, minWidth, minHeight);
                UpdateCorner(SelectedRegion.TopRightCornerName, leftUpdate, topUpdate, minWidth, minHeight);
            }
            else 
            if (leftUpdate < 0 && topUpdate < 0)
            {
                UpdateCorner(SelectedRegion.TopLeftCornerName, leftUpdate, topUpdate, minWidth, minHeight);
                UpdateCorner(SelectedRegion.BottomRightCornerName, leftUpdate, topUpdate, minWidth, minHeight);
            }
        }

        private double validateValue(double tempValue, double from, double to)
        {
            if (tempValue < from)
                tempValue = from;

            if (tempValue > to)
                tempValue = to;

            return tempValue;
        }
    }
}
