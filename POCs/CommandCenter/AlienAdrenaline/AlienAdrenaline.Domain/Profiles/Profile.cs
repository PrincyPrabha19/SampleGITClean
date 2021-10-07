

using System;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles
{
	public interface Profile
	{
        int Id { get; set; }
        string Name { get; set; }
        Guid GameId { get; set; }        
        string GameTitle { get; set; }
        string GamePath { get; set; }
        string GameRealPath { get; set; }
        string GameInstallPath { get; set; }
        string GameShortcutPath { get; set; }
        string GameIconPath { get; set; }
        int SteamId { get; set; }
        string SteamGamePath { get; set; }        

        byte[] GameImage { get; set; }
        GameModeProfileActions GameModeProfileActions { get; set; }
	}
}
