
using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.AlienAdrenaline.Domain.Profiles.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Classes.Factories
{
    public class RepositoryFactory
    {
        private static GameModeProfileActionImageRepository gameModeProfileActionImageRepository;
        public static GameModeProfileActionImageRepository NewGameModeProfileActionImageRepository()
        {
            return gameModeProfileActionImageRepository ??
                (gameModeProfileActionImageRepository = new GameModeProfileActionImageRepositoryClass());
        }
    }
}
