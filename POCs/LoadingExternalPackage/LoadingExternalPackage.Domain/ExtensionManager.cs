using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppExtensions;
using Windows.Storage;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Markup;

namespace LoadingExternalPackage.Domain
{
    public class ExtensionManager: IExtensionManager
    {
        #region Properties and Fields

        public List<GroupMapping> LedMapping { get; set; }
        public UIElement DeviceContent { get; set; }
        public string DeviceContentText { get; set; }

        private AppExtensionCatalog _catalog;
        private IReadOnlyList<AppExtension> _extensions;

        #endregion

        #region Constructor

        public ExtensionManager(string contract)
        {
            _catalog = AppExtensionCatalog.Open(contract);
        }

        #endregion

        #region Methods

        public async Task LoadExtension()
        {
            _extensions = await _catalog.FindAllAsync();
            LedMapping = await GetLedMapping();
            DeviceContent = await GetContent();
            DeviceContentText = await GetContentText();
        } 

        public async Task<List<GroupMapping>> GetLedMapping()
        {
            List<GroupMapping> ledMapping = null;

            foreach (var extension in _extensions)
            {
                var folder = await extension.GetPublicFolderAsync();
                var files = await folder.GetFilesAsync();

                var file = files.Where(f => string.Equals(f.Name, "mapping.json", StringComparison.CurrentCultureIgnoreCase))?.FirstOrDefault();

                string jsonText = await FileIO.ReadTextAsync(file);
                ledMapping= JsonConvert.DeserializeObject<List<GroupMapping>>(jsonText);

                //we found our mapping, so avoid looking for other files
                if (file != null)
                    break;

                //var stream = await file.OpenAsync(FileAccessMode.Read);
                //ulong size = stream.Size;
                //using (var inputStream = stream.GetInputStreamAt(1))
                //{
                //    using (var dataReader = new Windows.Storage.Streams.DataReader(inputStream))
                //    {
                //        uint numBytesLoaded = await dataReader.LoadAsync((uint)size);
                //        var mappingString = dataReader.ReadString(numBytesLoaded);
                //        var freshString = mappingString.TrimStart(new char[] { '.' });
                //        zoneMapping = JsonConvert.DeserializeObject<List<GroupMapping>>(freshString);
                //    }
                //}
            }
            return ledMapping;
        }

        public async Task<UIElement> GetContent()
        {
            UIElement content = null;

            await RunInUIThread(async () => {
                foreach (var extension in _extensions)
                {
                    var folder = await extension.GetPublicFolderAsync();
                    var files = await folder.GetFilesAsync();

                    var file = files.Where(f => string.Equals(f.Name, "deviceContent.xaml", StringComparison.CurrentCultureIgnoreCase))?.FirstOrDefault();

                    string xamlText = await FileIO.ReadTextAsync(file);
                    var controlContent = XamlReader.Load(xamlText) as UIElement;

                    content = ((Windows.UI.Xaml.Controls.UserControl)controlContent).Content;

                    //we found our content, so avoid looking for other files
                    if (file != null)
                        break;
                }
            });            

            return content;
        }

        public async Task<string> GetContentText()
        {
            string content = string.Empty;

            foreach (var extension in _extensions)
            {
                var folder = await extension.GetPublicFolderAsync();
                var files = await folder.GetFilesAsync();

                var file = files.Where(f => string.Equals(f.Name, "deviceContent.xaml", StringComparison.CurrentCultureIgnoreCase))?.FirstOrDefault();

                content = await FileIO.ReadTextAsync(file);

                //we found our content, so avoid looking for other files
                if (file != null)
                    break;
            }
            return content;
        }

        private async Task RunInUIThread(DispatchedHandler callback)
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal, callback);
        }

        #endregion
    }
}
