using System;
using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class NavigationPresenter
    {
        public NavigationView View { get; set; }
        public ProfileRepository ProfileRepository { get; set; }
        public CommandContainer CommandContainer { get; set; }
        public EventTrigger EventTrigger { get; set; }

        public virtual bool IsDirty
        {
            get { return !CommandContainer.IsEmpty; }
        }

        public void Load()
        {
            View.Links = Links;
        }

        public void Refresh()
        {
            View.Profiles = ProfileRepository.Profiles;
        }

        public void ChangeViewLinkSelected()
        {
            if (View.ViewLinkSelected == ViewType.RealTimePerformanceMonitoring ||
                View.ViewLinkSelected == ViewType.PerformanceSnapshots)
                EventTrigger.Fire(ProfileActionType.CloseActivatePerformance);

            EventTrigger.Fire(View.ViewLinkSelected, null);
        }

        public void RefreshCurrentProfile(Profile profile = null)
        {
            if (profile != null)
                ServiceFactory.ProfileService.SetCurrentProfile(profile);
            else
                EventTrigger.Fire(ProfileActionType.Save);

            EventTrigger.Fire(ProfileActionType.EnableDelete);
            EventTrigger.Fire(ViewType.GameMode, null);            
        }

        public void RefreshViews()
        {            
            EventTrigger.Fire(ProfileActionType.RefreshAllView);
        }

        public void SaveProfile()
        {
            EventTrigger.Fire(ProfileActionType.Save);
        }

        public void Cancel()
        {
            EventTrigger.Fire(ProfileActionType.Cancel);
        }

        public Profile GetCurrentProfile()
        {
            if (ProfileRepository != null)
                return ProfileRepository.CurrentProfile;
            return null;
        }

        public string GetCurrentProfileName()
        {
            if (ProfileRepository != null &&
                ProfileRepository.CurrentProfile != null)
                return ProfileRepository.CurrentProfile.Name;
            return String.Empty;
        }

        public bool IsCurrentProfile(string name)
        {
            return String.Compare(name, GetCurrentProfileName(), true) == 0;
        }

        public void ActivateGameModeProfileListView()
        {
            EventTrigger.Fire(ProfileActionType.ActivateGameModeProfileListView);
        }

        public virtual IList<ViewType> Links
        {
            get
            {
                var resultList = new List<ViewType>() { ViewType.GameMode, ViewType.PerformanceMonitoring };
                return resultList;
            }
        }
    }
}