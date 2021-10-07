using AlienLabs.AlienAdrenaline.Domain.Profiles;
using AlienLabs.GameDiscoveryHelper.Providers;

namespace AlienLabs.AlienAdrenaline.Domain
{
    public interface GameModeService
    {
        ProfileService ProfileService { get; set; }
        GameServiceProvider GameServiceProvider { get; set; }

        Profile Profile { get; }
        string GameTitle { get; }
        byte[] GameImage { get; }
        string GamePath { get; }
        string GameRealPath { get; }
                
        void Refresh();
    }
}