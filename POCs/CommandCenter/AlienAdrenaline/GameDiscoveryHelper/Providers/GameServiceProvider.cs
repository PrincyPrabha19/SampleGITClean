using System;
using System.Collections.Generic;

namespace AlienLabs.GameDiscoveryHelper.Providers
{
    public interface GameServiceProvider
    {
        List<GameProvider> GameProviders { get; set; }
        List<GameData> Games { get; }
        bool Initialized { get; }
        
        void Initialize();        
        GameData GetGameById(Guid id);
        GameData GetGameByPath(string gamePath);
    }
}
