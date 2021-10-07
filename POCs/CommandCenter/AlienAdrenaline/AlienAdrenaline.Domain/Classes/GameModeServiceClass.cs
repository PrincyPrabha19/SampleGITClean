using System;
using System.IO;
using System.Windows;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.GameDiscoveryHelper.Providers;

namespace AlienLabs.AlienAdrenaline.Domain.Classes
{
    public class GameModeServiceClass : GameModeService
    {
        #region Private Properties
        private byte[] defaultGameImage;
        #endregion
        
        #region GameModeService Members
        public ProfileService ProfileService { get; set; }
        public GameServiceProvider GameServiceProvider { get; set; }    

        public Profile Profile
        {
            get { return ProfileService.Profile; }
        }

        public string GameTitle
        {
            get { return Profile.GameTitle; }
        }

        public byte[] GameImage
        {
            get
            {
                if (ProfileService.Profile.GameId != Guid.Empty)
                {
                    var gameData = GameServiceProvider.GetGameById(ProfileService.Profile.GameId);
                    if (gameData != null)
                        return gameData.Image;
                }
                else
                {
                    var gameData = GameServiceProvider.GetGameByPath(ProfileService.Profile.GamePath);
                    if (gameData != null)
                        return gameData.Image;
                }

                if (defaultGameImage == null)
                    defaultGameImage = loadDefaultDefaultGameImage();
                return defaultGameImage;
            }
        }

        public string GamePath
        {
            get { return Profile.GamePath; }
        }

        public string GameRealPath
        {
            get { return Profile.GameRealPath; }
        }

        public void Refresh()
        {
        }
        #endregion

        #region Constructors
        public GameModeServiceClass()
        {
        }
        #endregion

        #region Private Methods
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
