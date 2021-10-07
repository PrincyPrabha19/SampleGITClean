
namespace AlienLabs.GameDiscoveryHelper.Helpers
{
    public abstract class GameUXPathHelper
    {
        public string GameUXNamespace = @"ROOT\CIMV2\Applications\Games";
        public string GameUXGamesBoxArtFolder = @"{0}\Microsoft\Windows\GameExplorer\GamesBoxArt\{{{1}}}.jpg";
        public string GameUXPlayShortcutFile = @"{0}\Microsoft\Windows\GameExplorer\{{{1}}}\PlayTasks\0\Play.lnk";
        //public string GameUXPlayShortcutFolder = @"{0}\Microsoft\Windows\GameExplorer\{{{1}}}\PlayTasks\0\";
        public string GameUXRegistryPath = @"SOFTWARE\Microsoft\Windows\CurrentVersion\GameUX\Games\{{{0}}}";
    }
}
