using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dominator.Domain.Classes.Helpers;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Profiles
{
    public class BundleProfileWriter : IProfileWriter
    {
        public ICryptoManager CryptoManager { get; set; }
        public IXTUService XTUService { get; set; }

        private readonly ILogger logger = LoggerFactory.LoggerInstance;
        private readonly ProfilesPathProvider profilesPathProvider = new ProfilesPathProvider();

        public bool Save(string profilePath, IDataHeader dataHeader, List<IDataComponent> dataComponents)
        {
            var profileData = new ProfileData();
            initProfileDataHeader(profileData, dataHeader);
            initProfileDataComponents(profileData, dataComponents);
            return writeProfile(profileData, profilePath);
        }

        private void initProfileDataHeader(ProfileData profileData, IDataHeader dataHeader)
        {
            profileData.Name = dataHeader.ProfileName;
            profileData.Model = dataHeader.Model;
            profileData.Status = dataHeader.Status;
        }

        private void initProfileDataComponents(ProfileData profileData, List<IDataComponent> dataComponents)
        {
            var cpuDataComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (cpuDataComponent == null) return;

            profileData.CPUPackage = new ProfileDataCPUPackage
            {
                BaseClock = cpuDataComponent.BaseClock.ToString(CultureInfo.InvariantCulture),
                CPU = new ProfileDataCPUPackageCPU
                {
                    OCEnabled = cpuDataComponent.IsOCEnabled.ToString(),
                    Brand = cpuDataComponent.Brand,
                    Voltage = cpuDataComponent.Voltage.ToString(CultureInfo.InvariantCulture),
                    VoltageOffset = cpuDataComponent.VoltageOffset.ToString(CultureInfo.InvariantCulture),
                    VoltageMode = cpuDataComponent.VoltageMode.ToString(CultureInfo.InvariantCulture),
                    Multiplier = cpuDataComponent.Multiplier.ToString(CultureInfo.InvariantCulture),
                    Power = cpuDataComponent.Power.ToString(CultureInfo.InvariantCulture),
                    ICCMax = cpuDataComponent.ICCMax.ToString(CultureInfo.InvariantCulture),
                    CacheICCMax = cpuDataComponent.CacheICCMax.ToString(CultureInfo.InvariantCulture)
                }
            };

            if (!string.IsNullOrEmpty(cpuDataComponent.XtuPath))
                profileData.CPUPackage.CPU.XtuPath = Path.GetFileName(cpuDataComponent.XtuPath);
            else
            {
                profileData.CPUPackage.CPU.Core = new ProfileDataCPUPackageCPUCore[cpuDataComponent.CoreDataList.Count];
                for (var i = 0; i < cpuDataComponent.CoreDataList.Count; i++)
                    profileData.CPUPackage.CPU.Core[i] = new ProfileDataCPUPackageCPUCore()
                    {
                        Multiplier = cpuDataComponent.CoreDataList[i].Multiplier.ToString("#.#", CultureInfo.InvariantCulture)
                    };
            }

            var memDataComponent = dataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.Memory) as MemoryDataComponent;
            if (memDataComponent == null) return;
            profileData.CPUPackage.Memory = new ProfileDataCPUPackageMemory
            {
                OCEnabled = memDataComponent.IsOCEnabled.ToString(),
                ProfileID = memDataComponent.ProfileID.ToString(),
                ClockMultiplier = memDataComponent.ClockMultiplier.ToString(CultureInfo.InvariantCulture),
                Multiplier = memDataComponent.Multiplier.ToString(CultureInfo.InvariantCulture)
            };
        }

        private bool writeProfile(ProfileData profileData, string path)
        {
            byte[] userData = XmlSerializerHelper.SerializeObject<ProfileData>(profileData);
            if (!CryptoManager?.EncryptData(ref userData) ?? false)
            {
                logger?.WriteError("BundleProfileWriter.writeProfile: Unable to encrypt data", "Either Dominator Service is not running or encryption failed");
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
                logger?.WriteError($"BundleProfileWriter.writeProfile: Failed to write encrypted data to {path}", null, e.ToString());
            }

            return false;

            //try
            //{
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof (ProfileData));
            //    using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            //    {
            //        xmlSerializer.Serialize(fs, profileData);
            //        fs.Close();
            //    }

            //    return true;
            //}
            //catch (Exception e)
            //{
            //    logger?.WriteError("BundleProfileWriter.writeProfile", null, e.ToString());
            //}

            //return false;
        }        
    }
}