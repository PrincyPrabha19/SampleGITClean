


using System;

namespace AlienLabs.GameDiscoveryHelper
{
    public interface GameData
    {
        Guid Id { get; set; }
        int SteamId { get; set; }
        string Name { get; set; }
        string InstallationPath { get; set; }
        string ApplicationExePath { get; set; }
        string ApplicationIconPath { get; set; }
        string SteamGameInstallationPath { get; set; }
        string SteamGameExePath { get; set; }
        string ConfigGDFBinaryPath { get; set; }
        string ResourceIDForGDFInfo { get; set; }        
        byte[] Image { get; set; }
    }
}
