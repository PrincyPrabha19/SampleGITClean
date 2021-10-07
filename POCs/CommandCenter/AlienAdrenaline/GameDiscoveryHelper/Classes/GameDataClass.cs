


using System;

namespace AlienLabs.GameDiscoveryHelper.Classes
{
    public class GameDataClass : GameData
    {
        public Guid Id { get; set; }
        public int SteamId { get; set; }
        public string Name { get; set; }
        public string InstallationPath { get; set; }
        public string ApplicationExePath { get; set; }
        public string ApplicationIconPath { get; set; }
        public string ConfigGDFBinaryPath { get; set; }
        public string ResourceIDForGDFInfo { get; set; }
        public string SteamGameInstallationPath { get; set; }
        public string SteamGameExePath { get; set; }
        public byte[] Image { get; set; }
    }
}
