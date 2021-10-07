using Dominator.Tools.AutomaticUpdateService;

namespace Dominator.Domain
{
    public interface IOCMetadataDownloadService
    {
        CommandCenterServiceSoap ServiceClient { get; set; }
        bool IsMetadataAvailable(string platform, string platformType, string processorModel, out string metadataUrl);
        bool DownloadMetadata(string url, string metadataPath);
    }
}