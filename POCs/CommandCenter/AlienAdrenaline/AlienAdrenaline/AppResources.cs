using System;
using System.Windows.Resources;

namespace AlienLabs.AlienAdrenaline.App
{
    public interface AppResources
    {
        string ImagesPath { get; }
        string ModelsPath { get; }

        StreamResourceInfo GetResourceStream(Uri uriResource);
        bool ExistsResourceStream(string uriResource);
        bool ExistsResourceStream(Uri uriResource);
    }
}