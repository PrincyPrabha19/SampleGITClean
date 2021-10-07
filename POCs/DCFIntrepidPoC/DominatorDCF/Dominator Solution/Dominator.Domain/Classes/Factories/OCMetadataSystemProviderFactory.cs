using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Factories
{
    public static class OCMetadataSystemProviderFactory
    {
        public static OCMetadataSystemProvider NewOCMetadataSystemProvider()
        {
            return new OCMetadataSystemProvider()
            {
                OCMetadataDownloadService = new OCMetadataDownloadService() {
                    ServiceClient = ServiceSoapFactory.NewCommandCenterServiceSoap()
                }
            };
        }
    }
}
