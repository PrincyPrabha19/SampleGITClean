using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper.Providers
{
    public interface GameProvider
    {
        List<GameData> GetGameList();
    }
}
