using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameBattleNetWindowsClass : GameBattleNetWindows
    {
        #region GameBattleNetWindows Methods
        public List<GameData> Discover()
        {
            var games = getBattleNetGamesFromRegistry();

            return games;
        }
        #endregion

        #region Private Methods
        private List<GameData> getBattleNetGamesFromRegistry()
        {
            var games = new List<GameData>();


            return games;
        }

        private byte[] getImageFromIconPath(string iconPath)
        {
            byte[] bytes = null;



            return bytes;
        }

        private bool isBattleNetGameDownloading(string installationPath, int steamId)
        {

            return false;
        }
        
        #endregion
    }
}
