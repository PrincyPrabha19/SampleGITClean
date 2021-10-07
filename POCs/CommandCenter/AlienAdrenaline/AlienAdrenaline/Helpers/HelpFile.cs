using System.IO;
using System.Windows.Forms;
using AlienLabs.CommandCenter.Tools.Classes;

namespace AlienLabs.AlienAdrenaline.App.Helpers
{
    public class HelpFile
    {
        public const string HELP_FILE = @"\alienadrenaline.chm";

        private static string getPath()
        {
            var langDir = @"\" + System.Globalization.CultureInfo.CurrentUICulture.Name;
            var path = ApplicationSettings.StartupPath + langDir + HELP_FILE;
            if (!File.Exists(path))
            {
                langDir = @"\" + System.Globalization.CultureInfo.CurrentUICulture.Name.Substring(0, 2);
                path = ApplicationSettings.StartupPath + langDir + HELP_FILE;

                if (!File.Exists(path))
                    path = ApplicationSettings.StartupPath + HELP_FILE;
            }

            return path;
        }

        public static void Show()
        {
            Help.ShowHelp(null, getPath());
        }

        public static void Show(HelpNavigator command, object parameter)
        {
            Help.ShowHelp(null, getPath(), command, parameter);
        }
    }
}
