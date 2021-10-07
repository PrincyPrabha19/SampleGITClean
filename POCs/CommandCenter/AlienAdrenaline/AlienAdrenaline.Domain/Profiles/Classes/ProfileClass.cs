
using System;
using System.Collections.Generic;
using AlienLabs.AlienAdrenaline.Domain.Classes;

namespace AlienLabs.AlienAdrenaline.Domain.Profiles.Classes
{
    public class ProfileClass : Profile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Guid GameId { get; set; }        
        public string GameTitle { get; set; }
        public string GamePath { get; set; }
        public string GameRealPath { get; set; }
        public string GameInstallPath { get; set; }
        public string GameShortcutPath { get; set; }
        public string GameIconPath { get; set; }
        public int SteamId { get; set; }
        public string SteamGamePath { get; set; }        

        public byte[] GameImage { get; set; }
        public GameModeProfileActions GameModeProfileActions { get; set; }

        #region Constructors
        public ProfileClass()
        {
            Name = String.Empty;
            GameId = Guid.Empty;
            SteamId = 0;
            GameTitle = String.Empty;
            GamePath = String.Empty;
            GameRealPath = String.Empty;
            GameInstallPath = String.Empty;
            GameShortcutPath = String.Empty;
            GameIconPath = String.Empty;
            GameModeProfileActions = new GameModeProfileActionsClass();            
        }
        #endregion
    }
}
