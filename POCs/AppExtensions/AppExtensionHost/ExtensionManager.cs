using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using Windows.ApplicationModel.AppExtensions;

namespace AppExtensionHost
{
    class ExtensionManager
    {
        #region Properties and fields
        private string _contract;
        public string Contract
        {
            get { return _contract; }
        }

        private AppExtensionCatalog _catalog;
        #endregion

        #region Constructor
        public ExtensionManager(string contract)
        {
            _contract = contract;
            _catalog = AppExtensionCatalog.Open(_contract);
        }
        #endregion

        public void Initialize()
        {
            FindAllExtensions();
        }

        public async void FindAllExtensions()
        {
            IReadOnlyList<AppExtension> extensions = await _catalog.FindAllAsync();
            foreach (AppExtension ext in extensions)
                await LoadExtension(ext);
        }

        public async Task LoadExtension(AppExtension ext)
        {
            // TODO: Load extension
            // QUESTION: How the extension can be associated to the class AFXModule so we can create instances and/or call methods
            Debug.WriteLine(ext.Description);
        }
    }
}
