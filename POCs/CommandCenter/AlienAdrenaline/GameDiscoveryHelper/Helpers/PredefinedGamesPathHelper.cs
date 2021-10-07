
using System;

namespace AlienLabs.GameDiscoveryHelper.Helpers
{
    public abstract class PredefinedGamesPathHelper
    {
        public string PredefinedGamesFile = String.Format(@"{0}\Alienware\CommandCenter\AlienAdrenaline\PredefinedGames.xml", Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData));
    }
}
