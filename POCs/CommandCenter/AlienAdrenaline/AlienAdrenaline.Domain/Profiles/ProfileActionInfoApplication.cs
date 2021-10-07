

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
    public interface ProfileActionInfoApplication : ProfileActionInfo
    {
        string ApplicationName { get; set; }
        string ApplicationPath { get; set; }
    }
}
