//#define NON_XTUPROFILE

using System.IO;
using Dominator.Domain.Classes.Profiles;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Factories
{
    public static class ProfileFactory
    {
        public static IProfile NewPredefinedProfile(string profileName, string profilePath)
        {
            var xtuService = ServiceRepository.XTUServiceInstance;

            return new Profile
            {
                Name = profileName,
                Path = profilePath,
                IsPredefinedProfile = true,
                Reader = new BundleProfileReader { XTUService = xtuService },
                Writer = new BundleProfileWriter { XTUService = xtuService },
                //TODO:Switch these processors to make predefined profiles .xtu or .ocp
#if NON_XTUPROFILE
                Processor = new PredefinedProfileProcessor { TuningManager = TuningFactory.NewTuningManager() }
#else                
                Processor = new PredefinedProfileProcessor { XTUService = xtuService, TuningManager = TuningFactory.NewTuningManager() }
#endif
            };
        }

        public static IProfile NewCustomProfile(string profileName, string profilePath)
        {
            var xtuService = ServiceRepository.XTUServiceInstance;

            return new Profile
            {
                Name = profileName,
                Path = profilePath,
                IsPredefinedProfile = false,
                Reader = new BundleProfileReader { XTUService = xtuService, CryptoManager = EncryptionFactory.NewCryptoManager() },
                Writer = new BundleProfileWriter { XTUService = xtuService, CryptoManager = EncryptionFactory.NewCryptoManager() },
                Processor = new CustomProfileProcessor { TuningManager = TuningFactory.NewTuningManager() }
            };
        }

        public static IProfile NewCustomProfile(string profileName)
        {
            var profilesPathProvider = new ProfilesPathProvider();
            var profilePath = Path.Combine(profilesPathProvider.ProfilesPath, $"{profileName}.{profilesPathProvider.getCustomProfileExtension()}");
            return NewCustomProfile(profileName, profilePath);
        }

        private static IProfileManager profileManager;
        public static IProfileManager NewProfileManager()
        {
            if (profileManager == null)
            {
                profileManager = new ProfileManager()
                {
                    ProfileNameRepository = NewProfileNameRepository(),
                    ProfileValidator = new ProfileValidator(),
                    SignatureVerifier = SignatureVerifierFactory.NewSignatureVerifier()
                };
                profileManager.Initialize();
            }

            return profileManager;
        }

        private static IProfileNameRepository profileNameRepository;
        public static IProfileNameRepository NewProfileNameRepository()
        {
            if (profileNameRepository == null)
            {
                profileNameRepository = new ProfileNameRepository() { ProfileDiscovery = new ProfileDiscovery() };
                profileNameRepository.RefreshProfileNameLists();
            }

            return profileNameRepository;
        }
    }
}
