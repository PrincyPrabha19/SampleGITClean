using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper
{
    public interface GameDiscovery
    {
        List<GameData> Discover();
    }
}
