using System;
using System.Linq;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;
using UsageTacking.Domain;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeActionAlienFXPresenter
    {
        #region Public Properties
        public GameModeActionAlienFXView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public AlienFXThemeService AlienFXThemeService { get; set; }
        public EventTrigger EventTrigger { get; set; }

        private ProfileActionInfoAlienFXClass profileActionInfo;
        public ProfileActionInfoAlienFXClass ProfileActionInfo
        {
            get
            {
                return profileActionInfo ??
                       (profileActionInfo = Model.CurrentProfileAction.ProfileActionInfo as ProfileActionInfoAlienFXClass);
            }
        }
        #endregion

        #region Public Methods
        public void Initialize()
        {
        }

        public void Refresh()
        {
            setAlienFXActionType(ProfileActionInfo.Type);

            var themes = AlienFXThemeService.GetAllThemes();

            AlienFXTheme themeSelected = null;
            string activeThemePath = AlienFXThemeService.GetActiveTheme();
            AlienFXTheme activeTheme = themes.ToList().Find(pp => String.Compare(pp.Path, activeThemePath, true) == 0);
            if (activeTheme != null)
            {
                themeSelected = new AlienFXThemeClass()
                {
                    Name = activeTheme.Name,
                    TempName = Properties.Resources.ActiveThemeText,
                    Path = activeTheme.Path
                };

                themes.Insert(0, themeSelected);
            }

            string profileActionInfoThemePath = ProfileActionInfo.ThemePath;
            AlienFXTheme profileActionInfoTheme = themes.ToList().Find(pp => String.Compare(pp.Path, profileActionInfoThemePath, true) == 0);

            View.Themes = themes;
            View.ThemeSelected =
                !String.IsNullOrEmpty(profileActionInfoThemePath) && profileActionInfoThemePath != activeThemePath && profileActionInfoTheme != null ?
                    profileActionInfoTheme : themeSelected;            

            if (!String.IsNullOrEmpty(profileActionInfoThemePath) && profileActionInfoTheme == null)
                View.ShowThemeErrorMessage(true, profileActionInfoThemePath);            
        }

        public void SetApplicationInfo()
        {
            View.ShowThemeErrorMessage(false);
            
            if (View.ThemeSelected != null)
            {
				AlienLabs.Tools.Classes.UsageTacking.LogFeatureUsage(Properties.Resources.ProcessIdentifier, "GameMode", Features.GameModeActionAlienFX, DateTime.Now);

                ProfileActionInfo.ThemeName = String.Empty;
                ProfileActionInfo.ThemePath = String.Empty;

                ProfileActionInfo.Type = getAlienFXActionType();
                if (ProfileActionInfo.Type == AlienFXActionType.PlayTheme)
                {
                    ProfileActionInfo.ThemeName = View.ThemeSelected.Name;
                    ProfileActionInfo.ThemePath = View.ThemeSelected.Path;
                }

                EventTrigger.Fire(
                        CommandFactory.NewGameModeAlienFXCommand(
                            ProfileActionInfo.Type, ProfileActionInfo.ThemeName, ProfileActionInfo.ThemePath, ProfileActionInfo.Guid));
            }
        }
        #endregion

        #region Private Methods
        private AlienFXActionType getAlienFXActionType()
        {
            AlienFXActionType type = AlienFXActionType.None;

            if (!View.IsUseCurrentAlienFXStateSelected)
            {
                if (View.IsEnableAlienFXAPISelected)
                    type = AlienFXActionType.EnableAlienFXAPI;
                else
                if (View.IsPlayThemeSelected)
                    type = AlienFXActionType.PlayTheme;
                else
                if (View.IsGoDarkSelected)
                    type = AlienFXActionType.GoDark;
            }

            return type;
        }

        private void setAlienFXActionType(AlienFXActionType type)
        {
            switch (type)
            {
                case AlienFXActionType.None:                    
                    View.IsUseCurrentAlienFXStateSelected = true;
                    View.IsPlayThemeSelected = true;
                    break;
                case AlienFXActionType.EnableAlienFXAPI:
                    View.IsEnableAlienFXAPISelected = true;
                    break;
                case AlienFXActionType.PlayTheme:
                    View.IsPlayThemeSelected = true;
                    break;
                case AlienFXActionType.GoDark:
                    View.IsGoDarkSelected = true;
                    break;
            }
        }
        #endregion
    }
}
