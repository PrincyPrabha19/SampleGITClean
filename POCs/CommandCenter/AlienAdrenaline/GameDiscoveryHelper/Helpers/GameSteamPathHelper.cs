
namespace AlienLabs.GameDiscoveryHelper.Helpers
{
    public abstract class GameSteamPathHelper
    {
        public string GameSteamRegistryPath = @"SOFTWARE\Valve\Steam";
        public string GameSteamRegistryAppsPath = @"SOFTWARE\Valve\Steam\Apps";
        public string GameSteamRegistryUninstallPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public string GameSteamRegistryUninstallAppsPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {0}";
        public string GameSteamRegistrySteamPIDPath = @"SOFTWARE\Wow6432Node\Valve\Steam";
        //public string GameSteamRegistryFirewallRulesPath = @"SYSTEM\ControlSet001\services\SharedAccess\Parameters\FirewallPolicy\FirewallRules";
        public string GameSteamApplicationPath = @"{0} -applaunch {1}";
        public string GameSteamDownloadingPath = @"{0}\steamapps\downloading\{1}";
    }


    public abstract class GameGOGPathHelper
    {
        public string GameGOGRegistryPath = @"SOFTWARE\Valve\GOG.com";
        public string GameGOGRegistryAppsPath = @"SOFTWARE\Valve\Steam\Apps";
        public string GameGOGRegistryUninstallPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall";
        public string GameGOGRegistryUninstallAppsPath = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Steam App {0}";
        public string GameGOGRegistryGOGPIDPath = @"SOFTWARE\Wow6432Node\Valve\Steam";
        public string GameGOGApplicationPath = @"{0} -applaunch {1}";
        public string GameGOGDownloadingPath = @"{0}\steamapps\downloading\{1}";
    }
}
