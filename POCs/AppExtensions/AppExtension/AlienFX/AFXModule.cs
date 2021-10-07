using System;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace AppExtension.AlienFX
{
    public sealed class AFXModule
    {
        public  async Task ShowMsg()
        {
            var dialog = new MessageDialog("AFXModule message!!");
            await dialog.ShowAsync();
        }
    }
}
