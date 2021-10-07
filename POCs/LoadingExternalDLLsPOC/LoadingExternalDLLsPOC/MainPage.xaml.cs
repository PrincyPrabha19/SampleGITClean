using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace LoadingExternalDLLsPOC
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void loaded(object sender, RoutedEventArgs e)
        {
            var asms = await AssemblyLoaderHelper.GetAssemblyList();
            if (asms == null)
                return;

            var backimg = await getEmbbededImage(asms, "M15x", "AlienLabs.AlienFX.ModelResources.M15x.thumb.png");
            //var backimg = await getResourceImage(asms, "M15x");
            imgCtrl.Source = backimg;

            
            var types = await getDeviceInstance(asms, "0x527");

        }

        private async Task<BitmapImage> getEmbbededImage(List<Assembly> asms, string model, string imgName)
        {
            var asm = asms.FirstOrDefault(a => a.FullName.Contains(model));
            if (asm == null)
                return null;

            try
            {
                BitmapImage bitmap = null;
                using (var imageStream = asm.GetManifestResourceStream(imgName))
                    using (var memStream = new MemoryStream())
                    {
                        await imageStream.CopyToAsync(memStream);
                        memStream.Position = 0;

                        using (var raStream = memStream.AsRandomAccessStream())
                        {
                            bitmap = new BitmapImage();
                            await bitmap.SetSourceAsync(raStream);
                        }
                    }

                return bitmap;
            }
            catch
            {
            }

            return null;
        }

        //private async Task<BitmapImage> getResourceImage(List<Assembly> asms, string model)
        //{
        //    var asm = asms.FirstOrDefault(a => a.FullName.Contains(model));
        //    if (asm == null)
        //        return null;

        //    try
        //    {
        //        //var uri = $"ms-appx://{asm.GetName().Name}/blackBackground.png";

        //        string url = $"ms-appx://{asm.GetName().Name}/Resources/blackBackground.png";
        //        //var source = RandomAccessStreamReference.CreateFromUri(new Uri(url));

        //        BitmapImage bitmapImage = new BitmapImage(new Uri(url));

        //        return bitmapImage;
        //    }
        //    catch
        //    {
        //    }

        //    return null;
        //}

        private async Task<object> getDeviceInstance(List<Assembly> asms, string model)
        {
            var asm = asms.FirstOrDefault(a => a.FullName.Contains(model));
            if (asm == null)
                return null;

            try
            {
                object objectHandle;
                try
                {
                    //var fullName = asm.FullName;
                    //var assemblyName = $"{assembly}{fullName.Substring(fullName.IndexOf(", Version=", StringComparison.InvariantCultureIgnoreCase))}";
                    //objectHandle = asm.cre.CreateInstance(fullName, "AlienLabs.AlienFX.Communication.PID0x527.Classes.DeviceSettings", true, BindingFlags.Public | BindingFlags.Instance, null, null, null, null);

                    List<Type> derivedClassList = asm.GetTypes().ToList();
                }
                catch (Exception e)
                {
                    // ignored
                }

                return null;
            }
            catch
            {
            }

            return null;
        }

    }
}
