using System;
using System.IO;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Tools;
using AlienLabs.CommandCenter.Tools;
using AlienLabs.GameDiscoveryHelper;
using AlienLabs.GameDiscoveryHelper.Helpers;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeCreatePresenter : GameModeProcessorPathHelper
    {
        #region Public Properties
        public GameModeCreateView View { get; set; }
        public GameModeCreateService Model { get; set; }
        public ThermalCapabilities ThermalCapabilities { get; set; }
        #endregion

        #region Public Methods
        public void Refresh()
        {
            Model.Refresh();

            View.Games = Model.Games;            
            View.SetGamesItemsSource();
            View.GameModeShortcutFolder = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        }

        public void ChangeGameSelected(GameData gameData)
        {
            View.GameSelected = gameData;

            string gamePath = String.Empty;
            if (View.GameSelected != null)
                gamePath = View.GameSelected.ApplicationExePath;

            View.GamePath = gamePath;
        }

        public string GetValidGameModeName(string gameModeName)
        {
            return Model.GetValidGameMode(gameModeName);
        }

        public void GameModeNameChanged(string gameModeName)
        {   
            if (String.IsNullOrEmpty(gameModeName) && View.GameSelected != null)
                gameModeName = View.GameSelected.Name;                

            View.GameModeName = GetValidGameModeName(gameModeName);            
        }

        public bool ShowGameModePath()
        {
            return View.GameSelected == null || String.IsNullOrEmpty(View.GameSelected.ApplicationExePath);
        }

        public void AddProfile()
        {
            string defaultProfileActionName = String.Empty;
            string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(GameModeActionType.GameApplication);
            if (!String.IsNullOrEmpty(resourceKey))
                defaultProfileActionName = Properties.Resources.ResourceManager.GetString(resourceKey);

            string gameShortcutName = String.Format(Properties.Resources.GameModeTitleText, View.GameModeName);
            string gameShortcutDescription = String.Format(Properties.Resources.LaunchGameModeText, View.GameModeName);
            string gameShortcutPath = Path.Combine(
                View.GameModeShortcutFolder, FilePathHelper.RemoveInvalidFileNameCharacters(gameShortcutName) + ".lnk");

            Guid gameId = Guid.Empty;            
            string gameName = String.Empty;
            string gameIconPath = String.Empty;
            string gameInstallPath = String.Empty;
            int steamId = 0;
            string steamGamePath = String.Empty;
         
            if (View.GameSelected != null &&
                String.Compare(View.GameSelected.ApplicationExePath, View.GamePath, StringComparison.OrdinalIgnoreCase) == 0)
            {
                gameId = View.GameSelected.Id;                
                gameName = View.GameSelected.Name;
                gameIconPath = View.GameSelected.ApplicationIconPath;
                gameInstallPath = View.GameSelected.SteamGameInstallationPath;
                steamId = View.GameSelected.SteamId;
                steamGamePath = steamId > 0 ? View.GameSelected.SteamGameExePath : String.Empty;
            }
            else
            {
                gameName = FilePathHelper.GetFileDescription(View.GamePath);
            }

            var gameDefinitionRegistryReader = new GameDefinitionRegistryHelper();
            string gameRealPath = gameDefinitionRegistryReader.GetRealApplicationPath(gameName);

            var newProfile = Model.NewProfile(
                View.GameModeName, defaultProfileActionName, gameShortcutPath, View.GamePath, gameRealPath, gameId, gameName, gameIconPath, gameInstallPath, steamId, steamGamePath);
            if (newProfile != null)
            {
                addDefaultProfileAction(newProfile);
                WScriptShortcutHelper.CreateShortcut(
                    gameShortcutName, gameShortcutDescription, gameShortcutPath, GameModeProcessorFile, View.GameModeName, 
                    (steamId > 0) ? gameIconPath : View.GamePath);
            }
        }

        public void UpdateNextButton()
        {
            View.IsNextEnabled =  
                !String.IsNullOrEmpty(View.GameModeName) &&          
                !String.IsNullOrEmpty(View.GamePath) &&
                !String.IsNullOrEmpty(View.GameModeShortcutFolder);
        }
        #endregion

        #region Private Methods
        private void addDefaultProfileAction(Profile profile)
        {
            foreach (var item in Enum.GetNames(typeof(GameModeActionType)))
            {
                var actionType = (GameModeActionType)Enum.Parse(typeof(GameModeActionType), item);
                if (EnumHelper.GetAttributeValue<AddOnProfileCreationAttributeClass, bool>(actionType))
                {
                    if (EnumHelper.GetAttributeValue<RequireThermalCapabilitiesAttributeClass, bool>(actionType) &&
                        !ThermalCapabilities.IsThermalSupported())
                        continue;
                    if (actionType == GameModeActionType.PowerPlan && !PowerPlanServiceClass.IsPowerPlanServicePresent)
                        continue;
                    if (actionType == GameModeActionType.AlienFX && !AlienFXThemeServiceClass.IsAlienFXThemeServicePresent)
                        continue;
                    if (actionType == GameModeActionType.Thermal && !ThermalProfileServiceClass.IsThermalProfileServicePresent)
                        continue;

                    string actionName = String.Empty;
                    string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(actionType);
                    if (!String.IsNullOrEmpty(resourceKey))
                        actionName = Properties.Resources.ResourceManager.GetString(resourceKey);

                    Model.AddProfileAction(actionName, actionType);
                }
            }
        }
        #endregion
    }
}
