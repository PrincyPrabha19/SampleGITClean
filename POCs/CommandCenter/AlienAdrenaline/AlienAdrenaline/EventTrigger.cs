using AlienLabs.AlienAdrenaline.App.Views;
using AlienLabs.AlienAdrenaline.Domain;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.App
{
    public interface EventTrigger
    {
        void Fire(EquatableCommand equatableCommand);
        void Fire(ViewType viewType, object parameter);
        void Fire(ProfileActionType profileActionType);
    }
}