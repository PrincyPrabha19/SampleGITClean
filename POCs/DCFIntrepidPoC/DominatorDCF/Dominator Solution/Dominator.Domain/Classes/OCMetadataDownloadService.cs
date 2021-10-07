using System;
using Dominator.Tools.AutomaticUpdateService;
using Dominator.Tools.Classes;

namespace Dominator.Domain.Classes
{
    public class OCMetadataDownloadService : IOCMetadataDownloadService
    {
        public CommandCenterServiceSoap ServiceClient { get; set; }
        private const string OCCONTROLS_AKAMAI = "http://dellupdater.dell.com/non_du/alienware/";

        public bool IsMetadataAvailable(string platform, string platformType, string processorModel, out string metadataUrl)
        {
            metadataUrl = string.Empty;

            var versionPlatformData = new VersionPlatformData()
            {
                ApplicationName = "OCMetadata",
                SystemModel = $"{platform}-{processorModel}",
                Platform = platformType,
                VersionNumber = "0.0.0"
            };

            try
            {
                var latestVersionData = ServiceClient.IsThereAnyUpdatebyPlatform(versionPlatformData);
                metadataUrl = latestVersionData?.HttpVersionLocation;
                if (!string.IsNullOrEmpty(metadataUrl))
                    metadataUrl = OCCONTROLS_AKAMAI + metadataUrl;
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
        
        public bool DownloadMetadata(string url, string metadataPath)
        {
            try
            {
                var webDownload = new WebDownload(5000);
                webDownload.DownloadFile(url, metadataPath);
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}