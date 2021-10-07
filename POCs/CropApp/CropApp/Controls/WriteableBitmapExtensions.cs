using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.UI.Xaml.Media.Imaging;

namespace CropApp.Controls
{
    public static class WriteableBitmapExtensions
    {
        public static async Task<bool> SaveAsync(this WriteableBitmap writeableBitmap, StorageFile outputFile)
        {
            try
            {
                Stream stream = writeableBitmap.PixelBuffer.AsStream();
                byte[] pixels = new byte[(uint)stream.Length];
                await stream.ReadAsync(pixels, 0, pixels.Length);

                using (var writeStream = await outputFile.OpenAsync(FileAccessMode.ReadWrite))
                {
                    var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.PngEncoderId, writeStream);
                    encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)writeableBitmap.PixelWidth,
                        (uint)writeableBitmap.PixelHeight,
                        72,
                        72,
                        pixels);

                    await encoder.FlushAsync();

                    using (var outputStream = writeStream.GetOutputStreamAt(0))
                    {
                        await outputStream.FlushAsync();
                    }
                }

                return true;
            }
            catch (Exception e)
            {                
            }

            return false;
        }

        public static async Task<Tuple<bool, WriteableBitmap>> LoadAsync(this WriteableBitmap writeableBitmap, StorageFile storageFile)
        {
            var wb = writeableBitmap;

            try
            {               
                using (var stream = await storageFile.OpenReadAsync())
                {
                    await wb.SetSourceAsync(stream);
                }

                return Tuple.Create(true, wb);
            }
            catch (Exception e)
            {
            }

            return Tuple.Create<bool, WriteableBitmap>(false, null);
        }

        public static async Task<Tuple<bool, WriteableBitmap>> ResizeAsync(this WriteableBitmap writeableBitmap, int maxWidth, int maxHeight)
        {
            var wb = writeableBitmap;

            try
            {


                return Tuple.Create(true, wb);
            }
            catch (Exception e)
            {
            }

            return Tuple.Create<bool, WriteableBitmap>(false, null);

            WriteableBitmap newImage;


            using (var fileStream = await file.OpenReadAsync())
            {
                var bitmap = new BitmapImage();
                bitmap.SetSource(fileStream);
                var origHeight = bitmap.PixelHeight;
                var origWidth = bitmap.PixelWidth;
                var ratioX = maxWidth / (float)origWidth;
                var ratioY = maxHeight / (float)origHeight;
                var ratio = Math.Min(ratioX, ratioY);
                var newHeight = (int)(origHeight * ratio);
                var newWidth = (int)(origWidth * ratio);

                if (ratio > 1)
                {
                    newHeight = origHeight;
                    newWidth = origWidth;
                }

                newImage = new WriteableBitmap(newWidth, newHeight);
                fileStream.Seek(0);
                var decoder = await BitmapDecoder.CreateAsync(fileStream);

                // Scale image to appropriate size 
                var transform = new BitmapTransform()
                {
                    ScaledWidth = Convert.ToUInt32(newImage.PixelWidth),
                    ScaledHeight = Convert.ToUInt32(newImage.PixelHeight)
                };
                var pixelData = await decoder.GetPixelDataAsync(
                    BitmapPixelFormat.Bgra8, // WriteableBitmap uses BGRA format 
                    BitmapAlphaMode.Straight,
                    transform,
                    ExifOrientationMode.IgnoreExifOrientation, // This sample ignores Exif orientation 
                    ColorManagementMode.DoNotColorManage
                );

                // An array containing the decoded image data, which could be modified before being displayed 
                var sourcePixels = pixelData.DetachPixelData();

                // Open a stream to copy the image contents to the WriteableBitmap's pixel buffer 
                using (var stream = newImage.PixelBuffer.AsStream())
                {
                    await stream.WriteAsync(sourcePixels, 0, sourcePixels.Length);
                }
            }

            return newImage;
        }
    }
}