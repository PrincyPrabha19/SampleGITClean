using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace CropApp.Controls
{
    public enum DialogSelectedCommand
    {
        No,
        Yes,
        Cancel,
        Save
    }

    public class PreviewImageChangeDialogResult
    {
        public string ImageName { get; set; }
        public DialogSelectedCommand Result { get; set; }
    }

    public sealed partial class PreviewImageChangeDialog : ContentDialog, INotifyPropertyChanged
    {
        private Guid profileID;
        public Guid ProfileID
        {
            get => profileID;
            set => SetProperty(ref profileID, value, nameof(ProfileID));
        }

        private string dialogTitle;
        public string DialogTitle
        {
            get => dialogTitle;
            set => SetProperty(ref dialogTitle, value, nameof(DialogTitle));
        }

        private string instruction;
        public string Instruction
        {
            get => instruction;
            set => SetProperty(ref instruction, value, nameof(Instruction));
        }

        private string browseButtonText;
        public string BrowseButtonText
        {
            get => browseButtonText;
            set => SetProperty(ref browseButtonText, value, nameof(BrowseButtonText));
        }

        private string saveButtonText;
        public string SaveButtonText
        {
            get => saveButtonText;
            set => SetProperty(ref saveButtonText, value, nameof(SaveButtonText));
        }

        private string cancelButtonText;
        public string CancelButtonText
        {
            get => cancelButtonText;
            set => SetProperty(ref cancelButtonText, value, nameof(CancelButtonText));
        }

        public PreviewImageChangeDialogResult DialogResult { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PreviewImageChangeDialog(Guid profileID, string title, string instruction, string browseText, string saveText, string cancelText)
        {
            this.InitializeComponent();

            DataContext = this;

            ProfileID = profileID;
            DialogTitle = title;
            Instruction = instruction;
            BrowseButtonText = browseText;
            SaveButtonText = saveText;
            CancelButtonText = cancelText;
        }

        public async Task LoadImageAsync(StorageFile imageFile)
        {
            if (imageFile != null)
            {
                var wb = new WriteableBitmap(1, 1);
                await wb.LoadAsync(imageFile);
                imageCropper.SourceImage = wb;
            }
        }

        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {            
            StorageFolder fxImagesLocalFolder = await ApplicationData.Current.LocalFolder.CreateFolderAsync(@"FX\Images", CreationCollisionOption.OpenIfExists);
            StorageFile file = null;

            var imageName = $"{ProfileID}.png";            

            try
            {
                file = await fxImagesLocalFolder.CreateFileAsync(imageName, CreationCollisionOption.ReplaceExisting);
                if (await imageCropper.CroppedImage.SaveAsync(file))
                {
                    DialogResult = new PreviewImageChangeDialogResult
                    {
                        ImageName = imageName,
                        Result = DialogSelectedCommand.Save
                    };
                }
            }
            catch (Exception)
            {
                if (file != null)
                    await file.DeleteAsync();
            }

            this.Hide();
        }

        private async void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = new PreviewImageChangeDialogResult
            {
                Result = DialogSelectedCommand.Cancel
            };

            this.Hide();
        }

        private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(storage, value)) return false;
            storage = value;
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }
    }
}