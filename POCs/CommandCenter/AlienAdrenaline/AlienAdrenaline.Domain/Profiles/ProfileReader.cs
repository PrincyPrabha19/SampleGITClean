using System.Collections.ObjectModel;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileReader
    {
        string FilePath { get; }
        ObservableCollection<Profile> Read();
        Profile ReadProfile(string path);
    }
}