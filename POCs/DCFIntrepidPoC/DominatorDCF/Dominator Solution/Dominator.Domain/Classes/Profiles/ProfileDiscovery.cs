using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Dominator.ServiceModel.Classes.Factories;

namespace Dominator.Domain.Classes.Profiles
{
    public class ProfileDiscovery : IProfileDiscovery
    {
        private readonly ProfilesPathProvider profilesPathProvider = new ProfilesPathProvider();

        public Dictionary<string, string> DiscoverPredefinedProfiles()
        {
            var result = new Dictionary<string, string>();
            if (Directory.Exists(profilesPathProvider.ProfilesPath))
            {
                var platform = SystemInfoRepository.Instance.PlatformInfoData.Platform;

                var files = Directory.GetFiles(profilesPathProvider.ProfilesPath, $"*.{profilesPathProvider.getPredefinedProfileExtension()}", SearchOption.TopDirectoryOnly);
                foreach (var filePath in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    string profileName, model, version;
                    if (!string.IsNullOrEmpty(fileName) && 
                        isValidPredefinedProfileFilename(Path.GetFileName(filePath), out profileName, out model, out version) &&
                        string.Compare(model, platform, StringComparison.InvariantCultureIgnoreCase) == 0)
                        result.Add(profileName, filePath);
                }
            }

            return result;
        }

        public Dictionary<string, string> DiscoverCustomProfiles()
        {
            var result = new Dictionary<string, string>();
            if (Directory.Exists(profilesPathProvider.ProfilesPath))
            {
                //var files = Directory.GetFiles(profilesPathProvider.ProfilesPath, $"*.{profilesPathProvider.getCustomProfileExtension()}", SearchOption.TopDirectoryOnly);
                DirectoryInfo dir = new DirectoryInfo(profilesPathProvider.ProfilesPath);
                string[] files = dir.GetFiles($"*.{profilesPathProvider.getCustomProfileExtension()}", SearchOption.TopDirectoryOnly)
                    .OrderBy(p => p.CreationTime).Take(5).Select(x => x.FullName).ToArray();

                foreach (var filePath in files)
                {
                    var fileName = Path.GetFileNameWithoutExtension(filePath);
                    if (!string.IsNullOrEmpty(fileName))
                        result.Add(fileName, filePath);
                }
            }

            return result;
        }


        private bool isValidPredefinedProfileFilename(string fileName, out string profileName, out string model, out string version)
        {
            profileName = string.Empty;
            model = string.Empty;
            version = string.Empty;

            var re = new Regex(@"^(OC\d+)_(\w+)_(\d+(\.\d+)*)\.opp$", RegexOptions.IgnoreCase);
            var ma = re.Match(fileName);
            if (ma.Success && ma.Groups.Count > 3 
                && ma.Groups[1].Success && ma.Groups[2].Success && ma.Groups[2].Success)
            {
                profileName = ma.Groups[1].Value;
                model = ma.Groups[2].Value;
                version = ma.Groups[3].Value;
                return true;
            }


            return false;
        }
    }
}