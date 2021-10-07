using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using Windows.ApplicationModel.Resources;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ExternalDllLoading
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        
        public MainPage()
        {
            this.InitializeComponent();
            //ControlContent1.Content = new ClassLibrary1.DynamicControl();
        }
        
        private async void loaded(object sender, RoutedEventArgs e)
        {
            var asms = await AssemblyLoaderHelper.GetAssemblyList();
            if (asms == null)
                return;

            Assembly asm = asms.FirstOrDefault(a => a.FullName.Contains("ClassLibrary1"));
            if (asm == null)
                return;
           

            var resources = asm.GetManifestResourceNames();
            Type[] tlist = asm.GetTypes();
            
            //load a custom buttom from dll

            foreach (Type t in tlist)
            {
                if (t.Name == "DynamicControl")
                {
                    var obj = Activator.CreateInstance(t);
                    ControlContent1.Content = obj as UserControl;
                    break;
                }
            }
        }
    }
}
