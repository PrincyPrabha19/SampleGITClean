using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml.Media.Imaging;

namespace CropApp.Controls
{
    public class CropBitmap
    {
        async public static Task<WriteableBitmap> GetCroppedBitmapAsync(StorageFile originalImageFile, Point startPoint, Size corpSize, double scale)
        {
            if (double.IsNaN(scale) || double.IsInfinity(scale))
            {
                scale = 1;
            }

            uint startPointX = (uint)Math.Floor(startPoint.X * scale);
            uint startPointY = (uint)Math.Floor(startPoint.Y * scale);
            uint height = (uint)Math.Floor(corpSize.Height * scale);
            uint width = (uint)Math.Floor(corpSize.Width * scale);

            using (IRandomAccessStream stream = await originalImageFile.OpenReadAsync())
            {
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(stream);

                uint scaledWidth = (uint)Math.Floor(decoder.PixelWidth * scale);
                uint scaledHeight = (uint)Math.Floor(decoder.PixelHeight * scale);

                if (startPointX + width > scaledWidth)
                    startPointX = scaledWidth - width;
                if (startPointY + height > scaledHeight)
                    startPointY = scaledHeight - height;

                byte[] pixels = await GetPixelData(decoder, startPointX, startPointY, width, height, scaledWidth, scaledHeight);
                WriteableBitmap cropBmp = new WriteableBitmap((int)width, (int)height);
                Stream pixStream = cropBmp.PixelBuffer.AsStream();
                pixStream.Write(pixels, 0, (int)(width * height * 4));

                return cropBmp;
            }
        }

        async static private Task<byte[]> GetPixelData(BitmapDecoder decoder, uint startPointX, uint startPointY, uint width, uint height)
        {
            return await GetPixelData(decoder, startPointX, startPointY, width, height,
                decoder.PixelWidth, decoder.PixelHeight);
        }

        async static private Task<byte[]> GetPixelData(BitmapDecoder decoder, uint startPointX, uint startPointY, uint width, uint height, uint scaledWidth, uint scaledHeight)
        {
            BitmapTransform transform = new BitmapTransform();
            BitmapBounds bounds = new BitmapBounds();
            bounds.X = startPointX;
            bounds.Y = startPointY;
            bounds.Height = height;
            bounds.Width = width;
            transform.Bounds = bounds;

            transform.ScaledWidth = scaledWidth;
            transform.ScaledHeight = scaledHeight;

            PixelDataProvider pix = await decoder.GetPixelDataAsync(
                BitmapPixelFormat.Bgra8,
                BitmapAlphaMode.Straight,
                transform,
                ExifOrientationMode.IgnoreExifOrientation,
                ColorManagementMode.ColorManageToSRgb);

            byte[] pixels = pix.DetachPixelData();
            return pixels;
        }
    }
}
