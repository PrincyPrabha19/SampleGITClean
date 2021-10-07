using System;
using System.Collections.ObjectModel;
using System.Linq;
using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Classes;
using AlienLabs.AlienAdrenaline.Domain.Classes.Attributes;
using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Helpers;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.App.Presenters
{
    public class GameModeCreateActionPresenter
    {
        public GameModeCreateActionView View { get; set; }
        public GameModeActionSequenceService Model { get; set; }
        public ProfileService ProfileService { get; set; }        
        public ThermalCapabilities ThermalCapabilities { get; set; }

        public void Refresh()
        {
            View.AvailableActions = getGameModeActionTypes();
        }

        private ObservableCollection<GameModeActionType> getGameModeActionTypes()
        {
            var actionTypes = new ObservableCollection<GameModeActionType>();

            foreach (var item in Enum.GetNames(typeof(GameModeActionType)).OrderBy(x => x))
            {
                var actionType = (GameModeActionType)Enum.Parse(typeof(GameModeActionType), item);
                if (EnumHelper.GetAttributeValue<AllowCreationAttributeClass, bool>(actionType))
                {
                    if (EnumHelper.GetAttributeValue<AllowMultipleAttributeClass, bool>(actionType) ||
                        !Model.GameModeProfileActions.ProfileActions.ToList().Exists(pa => pa.Type == actionType))
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

                        actionTypes.Add(actionType);
                    }                    
                }                
            }

            return actionTypes;
        }

        public void AddProfileAction()
        {
            var actionType = View.GameModeActionTypeSelected;
            string resourceKey = EnumHelper.GetAttributeValue<ResourceKeyAttributeClass, string>(actionType);

            View.ProfileAction = ProfileService.NewProfileAction(
                    Properties.Resources.ResourceManager.GetString(resourceKey),
                    actionType
                );
        }
    }
}