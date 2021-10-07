using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper
{
    public interface GameUXWMI
    {
        List<GameData> Discover();
    }
}
