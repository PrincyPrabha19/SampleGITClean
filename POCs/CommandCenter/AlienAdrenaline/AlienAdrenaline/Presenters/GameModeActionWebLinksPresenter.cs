using System;
using AlienLabs.AlienAdrenaline.App.Helpers;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionWebLinksPresenter
    {
        #region Public Properties
        public GameModeActionWebLinksView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoWebLinksClass profileActionInfo;
        public ProfileActionInfoWebLinksClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoWebLinksClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            View.Urls = ProfileActionInfo.Urls;
            View.EnableTabbedBrowsing = ProfileActionInfo.EnableTabbedBrowsing;

            if (ProfileActionInfo.Urls.Count <= 0)
                AddWebLink();
            
            View.SetWebLinksItemsSource();
        }

        public void DeleteWebLink(int index)
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeDeleteWebLink, DateTime.Now);
            View.Urls.RemoveAt(index);

            if (View.Urls.Count <= 0)
                AddWebLink();

            EventTrigger.Fire(CommandFactory.NewGameModeWebLinksCommand(View.Urls, View.EnableTabbedBrowsing, ProfileActionInfo.Guid));
        }

        public void AddWebLink()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeAddWebLink, DateTime.Now);
            View.Urls.Add(String.Empty);

            EventTrigger.Fire(CommandFactory.NewGameModeWebLinksCommand(View.Urls, View.EnableTabbedBrowsing, ProfileActionInfo.Guid));
        }

        public void UpdateWebLink(string url, int index)
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeUpdateWebLink, DateTime.Now);
            if (index < View.Urls.Count)
                View.Urls[index] = url;

            EventTrigger.Fire(CommandFactory.NewGameModeWebLinksCommand(View.Urls, View.EnableTabbedBrowsing, ProfileActionInfo.Guid));
        }

        public void ToggleEnableTabbedBrowsing()
        {
			AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeToggleEnableTabbedBrowsing, DateTime.Now);
            ProfileActionInfo.EnableTabbedBrowsing = View.EnableTabbedBrowsing;
            EventTrigger.Fire(CommandFactory.NewGameModeWebLinksCommand(View.Urls, View.EnableTabbedBrowsing, ProfileActionInfo.Guid));
        }

        public bool IsValidWebLink(string url)
        {
            return !String.IsNullOrEmpty(url) && UrlHelper.IsUrlValid(url);
        }
        #endregion
    }
}
