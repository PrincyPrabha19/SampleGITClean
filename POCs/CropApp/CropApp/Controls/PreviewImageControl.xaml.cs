using System;
using System.Collections.Generic;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CropApp.Controls
{
    public sealed partial class PreviewImageControl : UserControl
    {
        public PreviewImageControl()
        {
            this.InitializeComponent();
        }

        private async void BrowseButton_Click(object sender, RoutedEventArgs e)
        {
            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.Thumbnail;
            openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".bmp");
            openPicker.FileTypeFilter.Add(".gif");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile imgFile = await openPicker.PickSingleFileAsync();
            if (imgFile != null)
            {
                var wb = new WriteableBitmap(1, 1);
                var result = await wb.LoadAsync(imgFile);
                if (result.Item1)
                {
                    if (sender is FrameworkElement element)
                    {
                        element.Visibility = Visibility.Collapsed;
                        imageCropper.SourceImage = result.Item2;
                    }
                }
            }
        }

        private async void ApplyButton_Click(object sender, RoutedEventArgs e)
        {
            FileSavePicker savePicker = new FileSavePicker();
            savePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            savePicker.FileTypeChoices.Add("Portable Network Graphics", new List<string>() { ".png" });
            var file = await savePicker.PickSaveFileAsync();
            if (file != null)
                await imageCropper.CroppedImage.SaveAsync(file);
        }

        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}
