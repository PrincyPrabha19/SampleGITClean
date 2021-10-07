
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileActionFormatter
    {
        void Format(ProfileAction profileAction, GameModeActionSummaryData gameModeActionSummaryData);
    }
}
