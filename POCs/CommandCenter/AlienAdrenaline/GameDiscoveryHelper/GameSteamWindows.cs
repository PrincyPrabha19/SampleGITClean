using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper
{
    public interface GameSteamWindows
    {
        List<GameData> Discover();
    }
}
