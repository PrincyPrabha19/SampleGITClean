using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace CropApp.Controls
{
    public sealed class ImageCropper : Control, INotifyPropertyChanged
    {
        private const int CORNER_SIZE = 30;

        private SelectedRegion selectedRegion;
        private Path selectRegion;
        private ContentControl topLeftCorner;
        private ContentControl topRightCorner;
        private ContentControl bottomLeftCorner;
        private ContentControl bottomRightCorner;
        private Canvas imageCanvas;
        private Image sourceImage;
        private Grid layoutRoot;

        private uint sourceImagePixelWidth;
        private uint sourceImagePixelHeight;

        private readonly Dictionary<uint, Point?> pointerPositionHistory = new Dictionary<uint, Point?>();
        private StorageFile sourceImageFile = null;

        private static readonly DependencyProperty aspectRatioXProperty = DependencyProperty.Register("AspectRatioX", typeof(double), typeof(ImageCropper), new PropertyMetadata(1));
        public double AspectRatioX
        {
            get { return (double)GetValue(aspectRatioXProperty); }
            set { SetValue(aspectRatioXProperty, value); }
        }

        private static readonly DependencyProperty aspectRatioYProperty = DependencyProperty.Register("AspectRatioY", typeof(double), typeof(ImageCropper), new PropertyMetadata(1));
        public double AspectRatioY
        {
            get { return (double)GetValue(aspectRatioYProperty); }
            set { SetValue(aspectRatioYProperty, value); }
        }

        private static readonly DependencyProperty sourceImageProperty =
            DependencyProperty.Register("SourceImage", typeof(WriteableBitmap), typeof(ImageCropper), new PropertyMetadata(null, sourceImageChanged));
        public WriteableBitmap SourceImage
        {
            get { return (WriteableBitmap)GetValue(sourceImageProperty); }
            set { SetValue(sourceImageProperty, value); }
        }

        private static async void sourceImageChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var imageCropper = d as ImageCropper;
            var writeableBitmap = e.NewValue as WriteableBitmap;

            StorageFolder temp = ApplicationData.Current.LocalCacheFolder;
            StorageFile file = await temp.CreateFileAsync("temp_image.png", CreationCollisionOption.ReplaceExisting);
            if (await writeableBitmap.SaveAsync(file))
                await imageCropper.loadImage(file);
        }

        private WriteableBitmap croppedImage;
        public WriteableBitmap CroppedImage
        {
            get { return croppedImage; }
            private set
            {
                croppedImage = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CroppedImage"));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public ImageCropper()
        {
            DefaultStyleKey = typeof(ImageCropper);
            DataContext = selectedRegion;
        }

        protected override void OnApplyTemplate()
        {
            layoutRoot = GetTemplateChild("LayoutRootGrid") as Grid;

            selectRegion = GetTemplateChild("selectRegionPath") as Path;
            selectRegion.ManipulationMode = ManipulationModes.Scale | ManipulationModes.TranslateX | ManipulationModes.TranslateY;
            selectedRegion = new SelectedRegion { MinSelectRegionWidth = 2 * CORNER_SIZE, MinSelectRegionHeight = AspectRatioY * 2 * CORNER_SIZE / AspectRatioX };
            DataContext = selectedRegion;

            topLeftCorner = GetTemplateChild("topLeftCornerContentControl") as ContentControl;
            topRightCorner = GetTemplateChild("topRightCornerContentControl") as ContentControl;
            bottomLeftCorner = GetTemplateChild("bottomLeftCornerContentControl") as ContentControl;
            bottomRightCorner = GetTemplateChild("bottomRightCornerContentControl") as ContentControl;

            imageCanvas = GetTemplateChild("imageCanvas") as Canvas;
            sourceImage = GetTemplateChild("sourceImage") as Image;

            addCornerEvents(topLeftCorner);
            addCornerEvents(topRightCorner);
            addCornerEvents(bottomLeftCorner);
            addCornerEvents(bottomRightCorner);

            selectRegion.ManipulationDelta += selectRegion_ManipulationDelta;
            selectRegion.ManipulationCompleted += selectRegion_ManipulationCompleted;

            sourceImage.SizeChanged += sourceImage_SizeChanged;
        }

        private async Task loadImage(StorageFile imageFile)
        {
            using (IRandomAccessStream fileStream = await imageFile.OpenAsync(FileAccessMode.Read))
            {
                sourceImageFile = imageFile;
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(fileStream);
                sourceImagePixelHeight = decoder.PixelHeight;
                sourceImagePixelWidth = decoder.PixelWidth;
            }

            if (sourceImagePixelHeight < 2 * CORNER_SIZE || sourceImagePixelWidth < 2 * CORNER_SIZE)
                return;
            else
            {
                double sourceImageScale = 1;
                //if (sourceImagePixelHeight > layoutRoot.ActualHeight || sourceImagePixelWidth > layoutRoot.ActualWidth)
                //    sourceImageScale = Math.Min(layoutRoot.ActualWidth / sourceImagePixelWidth, layoutRoot.ActualHeight / sourceImagePixelHeight);
                //if (sourceImageScale == 0) return;

                sourceImage.Source = await CropBitmap.GetCroppedBitmapAsync(sourceImageFile, new Point(0, 0), new Size(sourceImagePixelWidth, sourceImagePixelHeight), sourceImageScale);
                CroppedImage = null;
            }
        }

        private void addCornerEvents(Control corner)
        {
            corner.PointerEntered += corner_PointerEntered;
            corner.PointerPressed += corner_PointerPressed;
            corner.PointerMoved += corner_PointerMoved;
            corner.PointerReleased += corner_PointerReleased;
            corner.PointerExited += corner_PointerExited;
        }

        private void corner_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            //Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Cross, 1);
        }

        private void corner_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            //Windows.UI.Xaml.Window.Current.CoreWindow.PointerCursor = new Windows.UI.Core.CoreCursor(Windows.UI.Core.CoreCursorType.Arrow, 1);
        }

        private void corner_PointerPressed(object sender, PointerRoutedEventArgs e)
        {
            (sender as UIElement).CapturePointer(e.Pointer);
            Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(this);
            pointerPositionHistory[pt.PointerId] = pt.Position;
            e.Handled = true;
        }

        private void corner_PointerMoved(object sender, PointerRoutedEventArgs e)
        {
            if (!(sender is ContentControl contentControl) || !(contentControl.Tag is string cornerName))
            {
                e.Handled = false;
                return;
            }

            try
            {
                Windows.UI.Input.PointerPoint pt = e.GetCurrentPoint(this);
                uint ptrId = pt.PointerId;

                if (pointerPositionHistory.ContainsKey(ptrId) && pointerPositionHistory[ptrId].HasValue)
                {
                    Point currentPosition = pt.Position;
                    Point previousPosition = pointerPositionHistory[ptrId].Value;

                    double xUpdate = currentPosition.X - previousPosition.X;
                    double xUpdateSign = Math.Abs(xUpdate) < Double.Epsilon ? 1 : Math.Sign(xUpdate);
                    double x = 0;
                    double y = 0;

                    switch (cornerName)
                    {
                        case SelectedRegion.TopLeftCornerName:
                        case SelectedRegion.BottomRightCornerName:
                            if (Math.Abs(xUpdate) > 0)
                            {
                                x = xUpdate;
                                y = AspectRatioY * xUpdate / AspectRatioX;
                            }
                            break;

                        case SelectedRegion.TopRightCornerName:
                        case SelectedRegion.BottomLeftCornerName:
                            if (Math.Abs(xUpdate) > 0)
                            {
                                x = xUpdate;
                                y = AspectRatioY * Math.Abs(xUpdate) / AspectRatioX * xUpdateSign * -1;
                            }
                            break;
                    }

                    selectedRegion.UpdateCorner(cornerName, x, y);

                    pointerPositionHistory[ptrId] = currentPosition;
                }

                e.Handled = true;
            }
            catch (Exception ex)
            {                
            }
        }

        private void corner_PointerReleased(object sender, PointerRoutedEventArgs e)
        {
            uint ptrId = e.GetCurrentPoint(this).PointerId;
            if (pointerPositionHistory.ContainsKey(ptrId))
                pointerPositionHistory.Remove(ptrId);

            if (sender is UIElement element)
                element.ReleasePointerCapture(e.Pointer);

            updatePreviewImage();
            e.Handled = true;
        }

        private async void updatePreviewImage()
        {
            double sourceImageWidthScale = imageCanvas.Width / sourceImagePixelWidth;
            double sourceImageHeightScale = imageCanvas.Height / sourceImagePixelHeight;

            Size previewImageSize = new Size(selectedRegion.SelectedRect.Width / sourceImageWidthScale, selectedRegion.SelectedRect.Height / sourceImageHeightScale);

            double previewImageScale = 1;
            //if (previewImageSize.Width <= imageCanvas.Width && previewImageSize.Height <= imageCanvas.Height)
            //    previewImageScale = Math.Max(imageCanvas.Width / previewImageSize.Width, imageCanvas.Height / previewImageSize.Height);

            CroppedImage = await CropBitmap.GetCroppedBitmapAsync(
                   sourceImageFile,
                   new Point(selectedRegion.SelectedRect.X / sourceImageWidthScale, selectedRegion.SelectedRect.Y / sourceImageHeightScale),
                   previewImageSize,
                   previewImageScale);
        }

        private void selectRegion_ManipulationDelta(object sender, ManipulationDeltaRoutedEventArgs e)
        {
            selectedRegion.UpdateSelectedRect(e.Delta.Scale, e.Delta.Translation.X, e.Delta.Translation.Y);
            e.Handled = true;
        }

        private void selectRegion_ManipulationCompleted(object sender, ManipulationCompletedRoutedEventArgs e)
        {
            updatePreviewImage();
        }

        private void sourceImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.IsEmpty || double.IsNaN(e.NewSize.Height) || e.NewSize.Height <= 0)
            {
                imageCanvas.Visibility = Visibility.Collapsed;
                selectedRegion.OuterRect = Rect.Empty;
                selectedRegion.ResetCorner(0, 0, 0, 0);
            }
            else
            {
                imageCanvas.Visibility = Visibility.Visible;
                imageCanvas.Width = e.NewSize.Width;
                imageCanvas.Height = e.NewSize.Height;                
                selectedRegion.OuterRect = new Rect(0, 0, e.NewSize.Width, e.NewSize.Height);

                var sourceAspectRatio = e.NewSize.Width / e.NewSize.Height;
                var targetAspectRatio = AspectRatioX / AspectRatioY;

                if (sourceAspectRatio <= targetAspectRatio)
                    selectedRegion.ResetCorner(0, 0, e.NewSize.Width, AspectRatioY * e.NewSize.Width / AspectRatioX);
                else
                    selectedRegion.ResetCorner(0, 0, AspectRatioX * e.NewSize.Height / AspectRatioY, e.NewSize.Height);

                updatePreviewImage();
            }
        }
    }
}
