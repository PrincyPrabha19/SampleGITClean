using System;
using System.Windows;
using System.Windows.Resources;

namespace AlienLabs.AlienAdrenaline.App.Classes
{
    public class AppResourcesClass : AppResources
    {
        #region AppResources Members
        public string ImagesPath { get { return "pack://application:,,,/AlienAdrenaline;component/media/"; } }
        public string ModelsPath { get { return string.Format("{0}models/", ImagesPath); } }

        public StreamResourceInfo GetResourceStream(Uri uriResource)
        {
            try { return Application.GetResourceStream(uriResource); }
            catch { return null; }
        }

        public virtual bool ExistsResourceStream(string uriResource)
        {
            return ExistsResourceStream(new Uri(uriResource));
        }

        public virtual bool ExistsResourceStream(Uri uriResource)
        {
            return GetResourceStream(uriResource) != null;
        }
        #endregion
    }
}