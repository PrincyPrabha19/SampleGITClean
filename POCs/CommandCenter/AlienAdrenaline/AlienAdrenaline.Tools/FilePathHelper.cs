
using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace AlienLabs.AlienAdrenaline.Tools
{
    public class FilePathHelper
    {
        public static bool IsValidPath(string path)
        {
            string filename;
            return IsValidPath(path, out filename);    
        }

        public static bool IsValidPath(string path, out string filename)
        {
            string arguments;
            return IsValidPath(path, out filename, out arguments);
        }

        public static bool IsValidPath(string path, out string filename, out string arguments)
        {
            filename = String.Empty;
            arguments = String.Empty;
            if (String.IsNullOrEmpty(path))
                return false;

            GetFilenameWithArguments(path, out filename, out arguments);
            return !String.IsNullOrEmpty(filename) && File.Exists(filename);
        }

        public static void GetFilenameWithArguments(string path, out string filename, out string arguments)
        {
            filename = String.Empty;
            arguments = String.Empty;
            
            //path = path.Trim(Path.GetInvalidFileNameChars());
            //path = path.Trim(Path.GetInvalidPathChars()); 

            path = path.Trim();
            Match m = Regex.Match(path, @"^(([A-Z]\:\\)?.+\.exe?)(\s+?(.*))?$", RegexOptions.IgnoreCase);
            if (m.Success && m.Groups.Count > 0)
            {
                if (m.Groups[1].Success)
                    filename = m.Groups[1].Value;

                if (m.Groups[4].Success)
                {
                    arguments = m.Groups[4].Value;
                    arguments = arguments.Replace('"', '"');
                }
            }            
        }

        public static string GetFileDescription(string path)
        {
            String filename;            
            if (IsValidPath(path, out filename))
            {
                try
                {
                    FileVersionInfo fileVersionInfo = FileVersionInfo.GetVersionInfo(filename);
                    if (!String.IsNullOrEmpty(fileVersionInfo.FileDescription))
                        return fileVersionInfo.FileDescription;
                }
                catch (FileNotFoundException)
                {
                }
            }

            return String.Empty;
        }

        public static string RemoveInvalidFileNameCharacters(string path)
        {
            string regex = String.Format("[{0}]", Regex.Escape(new string(Path.GetInvalidFileNameChars())));
            var removeInvalidChars = new Regex(regex, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.CultureInvariant);
            return removeInvalidChars.Replace(path, String.Empty);
        }
    }
}
