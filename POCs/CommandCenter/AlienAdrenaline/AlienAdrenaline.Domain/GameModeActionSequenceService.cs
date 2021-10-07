using AlienLabs.AlienAdrenaline.Domain.Enums;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeActionSequenceService
    {
        ProfileService ProfileService { get; set; }
        GameModeProfileActionImageRepository GameModeProfileActionImageRepository { get; set; }
        GameModeProfileActions GameModeProfileActions { get; }
        ProfileAction CurrentProfileAction { get; }
        byte[] CurrentProfileActionImage { get; set; }
        GameModeActionType CurrentProfileActionType { get; }
                
        void Refresh();
        void SetCurrentProfileAction(ProfileAction profileAction);
        void UpdateCurrentProfileActionImage(string applicationPath);
        byte[] GetProfileActionImage(ProfileAction profileAction, string applicationPath = null);
        byte[] GetProfileActionImage(GameModeActionType type);

        EquatableCommand NewGameModeProfileActionsCommand(GameModeProfileActions gameModeProfileActions);        
    }
}