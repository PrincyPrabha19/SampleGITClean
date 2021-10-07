namespace Dominator.Domain.Enums
{
    public enum OCMetadataStatus
    {
        MetadataInvalidArguments,        
        MetadataServiceUnreachable,
        MetadataNotFound,
        MetadataDownloadFailed,
        MetadataDownloadInvalidUrl,
        MetadataDigitalSignatureFailed,
        MetadataZipExtractFailed,
        MetadataDownloadSuccess
    }
}