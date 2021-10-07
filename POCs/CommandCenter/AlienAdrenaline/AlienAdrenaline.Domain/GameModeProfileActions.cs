
using System.Collections.Generic;
using System.Collections.ObjectModel;
using AlienLabs.AlienAdrenaline.Domain.Profiles;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeProfileActions : IEnumerable<ProfileAction>
    {
        ObservableCollection<ProfileAction> ProfileActions { get; set; }
        ProfileAction this[int index] { get; }

        void AddProfileAction(ProfileAction profileAction);
        void DeleteProfileAction(ProfileAction profileAction);
        void RelocateProfileAction(ProfileAction profileAction, bool forward);

        GameModeProfileActions Clone();
        void ReplaceProfileActions(GameModeProfileActions gameModeProfileActions);
    }
}
