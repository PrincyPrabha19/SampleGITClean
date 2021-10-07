
using AlienLabs.AlienAdrenaline.Domain.Classes.Factories;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes.Factories
{
    public class ProfileRepositoryFactory
    {
        private static ProfileRepository profileRepository;
        public static ProfileRepository ProfileRepository
        {
            get
            {
                if (profileRepository != null) 
                    return profileRepository;

                profileRepository = new ProfileRepositoryClass
                {
                    ProfileCreator = new ProfileCreatorClass(),
                    ProfileReader = new ProfileReaderClass(),                    
                    ProfileWriter = new ProfileWriterClass()
                };

                return profileRepository;
            }
        }
    }
}