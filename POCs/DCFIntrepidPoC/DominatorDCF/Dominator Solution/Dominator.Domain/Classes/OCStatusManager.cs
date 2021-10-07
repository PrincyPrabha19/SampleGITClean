using System;
using System.IO;
using Dominator.Domain.Classes.Helpers;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes
{
    public class OCStatusManager : IOCStatusManager
    {
        public ICryptoManager CryptoManager { get; set; }

        public bool IsOCEnabled { get; private set; }
        public string ActiveProfileName { get; private set; }
        public string CurrentProfileName { get; private set; }

        private readonly ILogger logger = LoggerFactory.LoggerInstance;

        private static readonly string ocStatusPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"Alienware\OCControls\OCStatus.xml");

        public bool SaveOCStatus(bool enabled)
        {
            try
            {
                var ocStatusData = deserializeOCStatusData(ocStatusPath) ?? new OCStatusData();
                ocStatusData.Enabled = enabled.ToString();
                if (!serializeOCStatusData(ocStatusData, ocStatusPath)) return false;
                IsOCEnabled = enabled;
            }
            catch (Exception e)
            {
                logger?.WriteError("OCStatusManager.SaveOCStatus failed", null, e.ToString());
                return false;
            }

            return true;
        }

        public bool SaveOCActiveProfile(string profileName)
        {
            try
            {
                if (profileName == null) throw new Exception("profileName is NULL");
                var ocStatusData = deserializeOCStatusData(ocStatusPath) ?? new OCStatusData();
                ocStatusData.ActiveProfile = profileName;
                if (!serializeOCStatusData(ocStatusData, ocStatusPath)) return false;
                ActiveProfileName = profileName;
            }
            catch (Exception e)
            {
                logger?.WriteError("OCStatusManager.SaveOCActiveProfile failed", null, e.ToString());
                return false;
            }

            return true;
        }

        public bool SaveOCCurrentProfile(string profileName)
        {
            try
            {
                if (profileName == null) throw  new Exception("profileName is NULL");
                var ocStatusData = deserializeOCStatusData(ocStatusPath) ?? new OCStatusData();
                ocStatusData.CurrentProfile = profileName;
                if (!serializeOCStatusData(ocStatusData, ocStatusPath)) return false;
                CurrentProfileName = profileName;
            }
            catch (Exception e)
            {
                logger?.WriteError("OCStatusManager.SaveOCCurrentProfile failed", null, e.ToString());
                return false;
            }

            return true;
        }

        public bool SaveOCActiveAndCurrentProfile(string profileName)
        {
            try
            {
                if (profileName == null) throw new Exception("profileName is NULL");
                var ocStatusData = deserializeOCStatusData(ocStatusPath) ?? new OCStatusData();
                ocStatusData.ActiveProfile = profileName;
                ocStatusData.CurrentProfile = profileName;
                if (!serializeOCStatusData(ocStatusData, ocStatusPath)) return false;
                ActiveProfileName = profileName;
                CurrentProfileName = profileName;
            }
            catch (Exception e)
            {
                logger?.WriteError("OCStatusManager.SaveOCActiveAndCurrentProfile failed", null, e.ToString());
                return false;
            }

            return true;
        }

        public void Initialize()
        {
            try
            {
                var ocStatusData = deserializeOCStatusData(ocStatusPath);
                if (ocStatusData != null)
                {
                    IsOCEnabled = Convert.ToBoolean(ocStatusData.Enabled);
                    ActiveProfileName = ocStatusData.ActiveProfile;
                    CurrentProfileName = ocStatusData.CurrentProfile;
                }
            }
            catch (Exception e)
            {
                logger?.WriteError("OCStatusManager.Initialize failed", null, e.ToString());
            }
        }

        private bool serializeOCStatusData(OCStatusData ocStatusData, string path)
        {
            byte[] userData = XmlSerializerHelper.SerializeObject<OCStatusData>(ocStatusData);
            if (!CryptoManager.EncryptData(ref userData))
            {
                logger?.WriteError("OCStatusManager.serializeOCStatusData: Unable to encrypt data", "Either Dominator Service is not running or encryption failed");
                return false;
            }

            try
            {
                using (var fs = new FileStream(path, FileMode.Create))
                {
                    using (var bw = new BinaryWriter(fs))
                    {
                        bw.Write(userData);
                        fs.Flush(true);
                    }

                    fs.Close();
                }

                return true;
            }
            catch (Exception e)
            {
                logger?.WriteError($"OCStatusManager.serializeOCStatusData: Failed to write encrypted data to {path}", null, e.ToString());
            }

            return false;

            //XmlSerializer xmlSerializer = new XmlSerializer(typeof (OCStatusData));
            //using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            //{
            //    try
            //    {
            //        xmlSerializer.Serialize(fs, ocStatusData);
            //    }
            //    finally
            //    {
            //        fs.Close();
            //    }
            //}
        }

        private OCStatusData deserializeOCStatusData(string path)
        {                       
            if (!File.Exists(ocStatusPath)) return null;

            byte[] encryptedData = null;

            try
            {
                using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    using (var br = new BinaryReader(fs))
                    {
                        encryptedData = new byte[fs.Length];
                        if (br.Read(encryptedData, 0, encryptedData.Length) <= 0)
                            encryptedData = null;
                        br.Close();
                    }

                    fs.Close();
                }
            }
            catch (Exception e)
            {
                logger?.WriteError($"OCStatusManager.deserializeOCStatusData: Failed to read encrypted data from {path}", null, e.ToString());
            }

            if (!CryptoManager.DecryptData(ref encryptedData))
            {
                logger?.WriteError("OCStatusManager.deserializeOCStatusData: Unable to decrypt data", "Either Dominator Service is not running or decryption failed");
                return null;
            }

            var ocStatusData = XmlSerializerHelper.DeserializeObject<OCStatusData>(encryptedData);
            return ocStatusData;

            //try
            //{
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof (OCStatusData));
            //    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    {
            //        ocStatusData = xmlSerializer.Deserialize(fs) as OCStatusData;
            //        fs.Close();
            //    }

            //}
            //catch (Exception e)
            //{
            //}
        }
    }
}