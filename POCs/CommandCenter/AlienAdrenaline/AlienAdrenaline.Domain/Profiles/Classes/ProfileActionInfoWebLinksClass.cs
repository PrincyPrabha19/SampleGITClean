
using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoWebLinksClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {
        public Guid Guid { get; private set; }

        public int Id { get; set; }
        public int ProfileActionId { get; set; }
        public ObservableCollection<string> Urls { get; set; }
        public bool EnableTabbedBrowsing { get; set; }

        public ProfileActionInfoWebLinksClass()
        {
            Guid = Guid.NewGuid();
            Urls = new ObservableCollection<string>();
            EnableTabbedBrowsing = true;
        }

        public override void Execute()
        {
            if (Urls.Count > 0)
                ServiceFactory.NewWebBrowserService().Execute(Urls, EnableTabbedBrowsing);
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoWebLinksClass()
            {
                Guid = Guid,
                Urls = new ObservableCollection<string>(Urls),
                EnableTabbedBrowsing = EnableTabbedBrowsing
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            bool areEqual = true;

            if (Guid != ((ProfileActionInfoWebLinksClass)profileActionInfo).Guid || 
                Urls.Count != ((ProfileActionInfoWebLinksClass)profileActionInfo).Urls.Count ||
                EnableTabbedBrowsing != ((ProfileActionInfoWebLinksClass)profileActionInfo).EnableTabbedBrowsing)
                return false;

            foreach (var url in Urls)
            {
                areEqual = ((ProfileActionInfoWebLinksClass)profileActionInfo).Urls.Contains(url);
                if (!areEqual)
                    break;
            }

            return areEqual;
        }

        public ProfileActionStatus GetStatus()
        {
            string urls = String.Empty;
            if (Urls.Count > 0)
                urls = Urls.Aggregate((i, j) => i + j);

            if (String.IsNullOrEmpty(urls))
                return ProfileActionStatus.NotReady;
            return ProfileActionStatus.None;
        }
    }
}
