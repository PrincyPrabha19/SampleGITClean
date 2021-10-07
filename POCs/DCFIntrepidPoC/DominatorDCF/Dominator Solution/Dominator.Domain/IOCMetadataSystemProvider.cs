using System;
using Dominator.Domain.Enums;

namespace Dominator.Domain
{
    public interface IOCMetadataSystemProvider
    {
        IOCMetadataDownloadService OCMetadataDownloadService { get; set; }
        event Action<OCMetadataStatus> MetadataDownloadCompleted;
    }
}