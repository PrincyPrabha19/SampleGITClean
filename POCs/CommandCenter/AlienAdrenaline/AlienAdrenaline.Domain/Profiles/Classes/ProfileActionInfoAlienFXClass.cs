

using System;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileActionInfoAlienFXClass : ProfileActionInfoProcessorClass, ProfileActionInfo
    {
        #region Public Properties
        public AlienFXActionType Type { get; set; }
        public string ThemeName { get; set; }
        public string ThemePath { get; set; }
        public bool IsAlienFXAPIEnabled { get; set; }
        public bool IsGoDark { get; set; }
        #endregion

        #region ProfileActionInfo Members
        public Guid Guid { get; private set; }
        public int Id { get; set; }
        public int ProfileActionId { get; set; }

        public override void Execute()
        {
            switch (Type)
            {
                case AlienFXActionType.PlayTheme:
                    if (!ServiceFactory.AlienFXThemeService.ExistsAlienFXTheme(ThemePath))
                        throw new Exception(Properties.Resources.AlienFXDoesNotExistsErrorText);

                    ServiceFactory.AlienFXThemeService.GoLight();
                    ServiceFactory.AlienFXThemeService.DisableAlienFXAPI();
                    ServiceFactory.AlienFXThemeService.SetActiveTheme(ThemePath);
                    break;
                case AlienFXActionType.GoDark:
                    ServiceFactory.AlienFXThemeService.GoDark();
                    break;
                case AlienFXActionType.EnableAlienFXAPI:
                    ServiceFactory.AlienFXThemeService.EnableAlienFXAPI();
                    break;
            }            
        }

        public override void Rollback()
        {
            if (IsGoDark)
                ServiceFactory.AlienFXThemeService.GoDark();

            if (!String.IsNullOrEmpty(ThemePath))
                ServiceFactory.AlienFXThemeService.SetActiveTheme(ThemePath);

            if (IsAlienFXAPIEnabled)
                ServiceFactory.AlienFXThemeService.EnableAlienFXAPI();
            else
                ServiceFactory.AlienFXThemeService.DisableAlienFXAPI();

            if (!IsGoDark)
                ServiceFactory.AlienFXThemeService.GoLight();
        }

        public ProfileActionInfo Clone()
        {
            return new ProfileActionInfoAlienFXClass()
            {
                Guid = Guid,
                Type = Type,
                ThemeName = ThemeName,
                ThemePath = ThemePath
            };
        }

        public bool Equals(ProfileActionInfo profileActionInfo)
        {
            return Guid == ((ProfileActionInfoAlienFXClass)profileActionInfo).Guid &&
                   Type == ((ProfileActionInfoAlienFXClass)profileActionInfo).Type &&
                   ThemeName == ((ProfileActionInfoAlienFXClass)profileActionInfo).ThemeName &&
                   ThemePath == ((ProfileActionInfoAlienFXClass)profileActionInfo).ThemePath;
        }

        public ProfileActionStatus GetStatus()
        {
            return ProfileActionStatus.None;
        }
        #endregion

        #region Constructors
        public ProfileActionInfoAlienFXClass()
        {
            Guid = Guid.NewGuid();
            Type = AlienFXActionType.None;
            ThemeName = String.Empty;
            ThemePath = String.Empty;
        }

        public ProfileActionInfoAlienFXClass(bool initialize)
            : this()
        {
            if (initialize)
            {
                var alienFXThemeService = ServiceFactory.AlienFXThemeService;
                if (alienFXThemeService != null)
                {
                    IsGoDark = alienFXThemeService.IsGoDark();
                    IsAlienFXAPIEnabled = alienFXThemeService.IsAlienFXAPIEnabled();
                    ThemePath = alienFXThemeService.GetActiveTheme();
                }                    
            }
        }
        #endregion
    }
}
