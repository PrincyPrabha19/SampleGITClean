using System;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using Dominator.Domain.Classes.Helpers;
using Dominator.Domain.Enums;
using Dominator.ServiceModel.Classes.SystemInfo;
using Dominator.Tools;
using Dominator.Tools.Classes;
using Dominator.Tools.Classes.Factories;
using AuthenticationManager = Dominator.Tools.Classes.Security.AuthenticationManager;

namespace Dominator.Domain.Classes
{
    public class OCMetadataSystemProvider : IOCMetadataSystemProvider
    {
        public IOCMetadataDownloadService OCMetadataDownloadService { get; set; }

        public event Action<OCMetadataStatus> MetadataDownloadCompleted;

        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        public OCMetadataStatus RetrieveMetadata()
        {
            var ocMetadataResult = retrieveAvailableMetadata();
            if (ocMetadataResult.Status != OCMetadataStatus.MetadataDownloadSuccess) return ocMetadataResult.Status;
            if (string.IsNullOrEmpty(ocMetadataResult.MetadataPath)) return OCMetadataStatus.MetadataDownloadInvalidUrl;

            try
            {
                if (!verifyZipDigitalSignature(ocMetadataResult.MetadataPath)) return OCMetadataStatus.MetadataDigitalSignatureFailed;
                if (!extractZip(ocMetadataResult.MetadataPath)) return OCMetadataStatus.MetadataZipExtractFailed;
                return OCMetadataStatus.MetadataDownloadSuccess;
            }
            finally 
            {       
                deleteFile(ocMetadataResult.MetadataPath);    
            }
        }

        public void RetrieveMetadataAsync()
        {
            var retrieveMetadataBackgroundWorker = new BackgroundWorker();
            retrieveMetadataBackgroundWorker.DoWork += retrieveMetadataBackgroundWorker_DoWork;
            retrieveMetadataBackgroundWorker.RunWorkerCompleted += retrieveMetadataBackgroundWorker_RunWorkerCompleted;
            retrieveMetadataBackgroundWorker.RunWorkerAsync();
        }

        private void retrieveMetadataBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs args)
        {
            var ocMetadataResult = args.Result as OCMetadataResult;
            if (ocMetadataResult == null) return;

            if (ocMetadataResult.Status != OCMetadataStatus.MetadataDownloadSuccess)
            {
                MetadataDownloadCompleted?.Invoke(ocMetadataResult.Status);
                return;
            }

            if (string.IsNullOrEmpty(ocMetadataResult.MetadataPath))
            {
                MetadataDownloadCompleted?.Invoke(OCMetadataStatus.MetadataDownloadInvalidUrl);
                return;
            }

            try
            {
                if (!verifyZipDigitalSignature(ocMetadataResult.MetadataPath))
                {
                    MetadataDownloadCompleted?.Invoke(OCMetadataStatus.MetadataDigitalSignatureFailed);
                    return;
                }

                if (!extractZip(ocMetadataResult.MetadataPath))
                {
                    MetadataDownloadCompleted?.Invoke(OCMetadataStatus.MetadataZipExtractFailed);
                    return;
                }

                MetadataDownloadCompleted?.Invoke(OCMetadataStatus.MetadataDownloadSuccess);
            }
            finally
            {
                deleteFile(ocMetadataResult.MetadataPath);
            }
        }

        private void retrieveMetadataBackgroundWorker_DoWork(object sender, DoWorkEventArgs args)
        {
            args.Result = retrieveAvailableMetadata();
        }

        private OCMetadataResult retrieveAvailableMetadata()
        {
            var platformInfoData = new PlatformInfoData();
            platformInfoData.Initialize();

            var platform = platformInfoData?.Platform;
            var platformType = platformInfoData?.PlatformType.ToString();
            var processorModel = ProcessorHelper.GetProcessorCode(ProcessorHelper.GetProcessorName());

            string dataConfigUrl;
            if (!OCMetadataDownloadService.IsMetadataAvailable(platform, platformType, processorModel, out dataConfigUrl))
                return new OCMetadataResult { Status = OCMetadataStatus.MetadataServiceUnreachable };
            if (string.IsNullOrEmpty(dataConfigUrl))
                return new OCMetadataResult { Status = OCMetadataStatus.MetadataNotFound };

            var uri = new Uri(dataConfigUrl);
            string metadataDownloadPath = Path.Combine(Path.GetTempPath(), Path.GetFileName(uri.AbsolutePath));
            if (!OCMetadataDownloadService.DownloadMetadata(dataConfigUrl, metadataDownloadPath))
                return new OCMetadataResult { Status = OCMetadataStatus.MetadataDownloadFailed };

            return new OCMetadataResult { Status = OCMetadataStatus.MetadataDownloadSuccess, MetadataPath = metadataDownloadPath };
        }

        private bool verifyZipDigitalSignature(string metadataZipPath)
        {
            return File.Exists(metadataZipPath) && AuthenticationManager.Singleton.IsSigned(metadataZipPath);
        }

        private bool extractZip(string metadataZipPath)
        {
            try
            {
                string outputPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls");
                extractZipArchiveToDestination(metadataZipPath, outputPath);
                return true;
            }
            catch (Exception e)
            {
                logger?.WriteError("OCMetadataSystemProvider.extractZip failed", e.ToString());
            }

            return false;
        }

        /// <summary>
        /// ZipFile.ExtractToDirectory fails if some files already exists
        /// </summary>
        /// <param name="archiveFullPath"></param>
        /// <param name="extractToPath"></param>
        private void extractZipArchiveToDestination(string archiveFullPath, string extractToPath)
        {
            using (var zipArchive = ZipFile.OpenRead(archiveFullPath))
            {
                foreach (var entry in zipArchive.Entries)
                {
                    var entryFullname = Path.Combine(extractToPath, entry.FullName);
                    var entryPath = Path.GetDirectoryName(entryFullname);
                    if (!string.IsNullOrEmpty(entryPath) && !Directory.Exists(entryPath))
                        Directory.CreateDirectory(entryPath);

                    try
                    {
                        var entryFilename = Path.GetFileName(entryFullname);
                        if (!string.IsNullOrEmpty(entryFilename))
                            entry.ExtractToFile(entryFullname, true);
                    }
                    catch (Exception e)
                    {
                        logger?.WriteError("OCMetadataSystemProvider.extractZipArchiveToDestination: " + entryFullname, e.ToString());
                    }
                }
            }
        }

        private void deleteFile(string metadataZipPath)
        {
            try
            {
                if (File.Exists(metadataZipPath))
                    File.Delete(metadataZipPath);
            }
            catch (Exception e)
            {                
            }
        }
    }
}
