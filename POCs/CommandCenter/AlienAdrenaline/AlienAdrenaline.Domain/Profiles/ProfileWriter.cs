using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileWriter
    {
        void Write(ObservableCollection<Profile> profiles, string path);
        void Write(Profile profile, string path);
    }
}