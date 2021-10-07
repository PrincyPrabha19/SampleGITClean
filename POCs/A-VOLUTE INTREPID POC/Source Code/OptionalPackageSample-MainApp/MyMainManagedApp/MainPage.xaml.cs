using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MyMainManagedApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private static OptionalPackageUiPlugin _audioUIPlugin;
        public MainPage()
        {
            this.InitializeComponent();
            ListOptionalPackages();
        }

        private void ListOptionalPackages()
        {
            WriteToTextBox("Enumerating Packages");
            var optionalPackages = EnumerateInstalledPackages();
            foreach (var package in optionalPackages)
            {
                var packageName = package.Id.FullName;
                WriteToTextBox(packageName);
                AddPluginAsTab(package.InstalledLocation.Path);
            }
        }

        private void AddPluginAsTab(string packagePath)
        {
            _audioUIPlugin = OptionalPackageUiPlugin.CreateFromPackagePath(packagePath);

            var pluginWrapper = new WRTPluginWrapper.IntepridPluginWrapper();
            _audioUIPlugin.LoadPlugins(pluginWrapper);

            //var gridHost = new Grid();
            foreach (var key in pluginWrapper.Plugin.DashboardViews.Keys)
            {
                var pivotItem = new PivotItem
                {
                    Header = key,
                    Content = pluginWrapper.Plugin.DashboardViews[key],
                };

                MainPivot.Items.Add(pivotItem);

                //gridHost.Children.Add(pluginWrapper.Plugin.userControl1);
            }                        
        }

        private static List<Package> EnumerateInstalledPackages()
        {
            // Obtain the app's package first to then find all related packages
            var currentAppPackage = Windows.ApplicationModel.Package.Current;

            // The dependencies list is where the list of optional packages (OP) can be determined
            var dependencies = currentAppPackage.Dependencies;

            //  If it is optional, then add it to our results
            return dependencies.Where(package => package.IsOptional).ToList();
        }


        private async void WriteToTextBox(string str)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, ()=>
            {
                ApptextBox.Text += str + "\n";
            });
        }

    }


}
