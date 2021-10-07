using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using AlienLabs.GameDiscoveryHelper.Helpers;
using AlienLabs.WindowsIconHelper;
using Microsoft.Win32;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    class GameOriginWindowsClass
    {
        #region GameOriginWindows Methods
        public List<GameData> Discover()
        {
            var games = getOriginGames();

            return games;
        }
        #endregion

        #region Private Methods
        private List<GameData> getOriginGames()
        {
            var games = new List<GameData>();

            return games;
        }


        private byte[] getImageFromIconPath(string iconPath)
        {
            byte[] bytes = null;

            if (File.Exists(iconPath))
            {
                Icon icon = IconHelper.ExtractBestFitIcon(iconPath, 0, new Size(256, 256));
                if (icon != null)
                    bytes = IconUtils.GetBytesFromIcon(icon);
            }

            return bytes;
        }

        private bool isOriginGameDownloading(string installationPath, int steamId)
        {
            return true;
        }
        #endregion
    }
}
