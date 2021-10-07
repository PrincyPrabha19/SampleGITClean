

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileCreatorClass : ProfileCreator
    {
        public Profile New(string name)
        {
            var profile = new ProfileClass()
                              {
                                  Name = name
                              };

            return profile;
        }
    }
}
