using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using AlienLabs.GameDiscoveryHelper.Providers;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeProfileListPresenter
    {
        #region Public Properties
        public GameModeProfileListView View { get; set; }
        public ProfileRepository ProfileRepository { get; set; }
        public GameServiceProvider GameServiceProvider { get; set; }
        public EventTrigger EventTrigger { get; set; }        
        #endregion

        #region Private Properties
        private byte[] defaultGameImage;
        #endregion

        #region Public Methods
        public void Refresh()
        {
            refreshProfileImages(ProfileRepository.Profiles);
            
            var profiles = new ObservableCollection<Profile>(ProfileRepository.Profiles);
            var emptyProfile = new EmptyProfileClass()
                                   {
                                       Name = Properties.Resources.CreateNewModeText,
                                       GameImage = loadDefaultDefaultGameImage()
                                   };
            profiles.Insert(0, emptyProfile);

            View.Profiles = profiles;
            View.SetProfileListItemsSource();
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
            EventTrigger.Fire(ProfileActionType.RefreshNavigation);
        }

        public bool IsCreateNewProfileSelected(Profile profile)
        {
            return profile is EmptyProfileClass;
        }
        #endregion

        #region Private Methods
        private void refreshProfileImages(ObservableCollection<Profile> profiles)
        {
            foreach (var profile in profiles)
            {
                if (profile.GameImage != null)
                    continue;
                profile.GameImage = getProfileImage(profile);
            }
        }

        private byte[] getProfileImage(Profile profile)
        {
            if (profile.GameId != Guid.Empty)
            {
                var gameData = GameServiceProvider.GetGameById(profile.GameId);
                if (gameData != null)
                    return gameData.Image;
            }
            else
            {
                var gameData = GameServiceProvider.GetGameByPath(profile.GamePath);
                if (gameData != null)
                    return gameData.Image;
            }

            if (defaultGameImage == null)
                defaultGameImage = loadDefaultDefaultGameImage();
            return defaultGameImage;
        }

        private byte[] loadDefaultDefaultGameImage()
        {
            byte[] bytes = null;

            try
            {
                var resourceUri = new Uri("pack://application:,,,/AlienAdrenaline.Domain;component/media/nogameboxart.png");

                var streamResourceInfo = Application.GetResourceStream(resourceUri);
                if (streamResourceInfo != null)
                {
                    using (var binaryReader = new BinaryReader(streamResourceInfo.Stream))
                    {
                        bytes = binaryReader.ReadBytes((int)streamResourceInfo.Stream.Length);
                    }
                }
            }
            catch (Exception)
            {
            }

            return bytes;
        }
        #endregion
    }
}
