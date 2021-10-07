using System;
using System.IO;
using Dominator.Domain.Classes.Factories;
using Dominator.Domain.Enums;

namespace OTAOCDataSetupTool
{
    class Program
    {
        static int Main(string[] args)
        {
            if (args.Length <= 0 || 
                string.Compare(args[0], "/start", StringComparison.InvariantCultureIgnoreCase) != 0) return 0;
            var provider = OCMetadataSystemProviderFactory.NewOCMetadataSystemProvider();
            var ocMetadataStatus = provider.RetrieveMetadata();

            try
            {
                string outputLog = Path.Combine(Path.GetTempPath(), "OTAOCDataSetupTool.log");
                using (var sw = new StreamWriter(outputLog, false))
                {
                    sw.Write($"{DateTime.Now} {ocMetadataStatus}");
                    sw.Close();
                }
            }
            catch {}

            return ocMetadataStatus == OCMetadataStatus.MetadataDownloadSuccess ? 1 : 0;
        }
    }
}
