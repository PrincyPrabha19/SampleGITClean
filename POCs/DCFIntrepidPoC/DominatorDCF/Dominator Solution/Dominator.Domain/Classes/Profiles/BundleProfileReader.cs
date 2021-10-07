using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Dominator.Domain.Classes.Helpers;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes.Profiles
{
    public class BundleProfileReader : IProfileReader
    {
        public ICryptoManager CryptoManager { get; set; }
        public IXTUService XTUService { get; set; }
        public IDataHeader DataHeader { get; private set; }
        public List<IDataComponent> DataComponents { get; private set; }

        private readonly ILogger logger = LoggerFactory.LoggerInstance;
        private readonly ProfilesPathProvider profilesPathProvider = new ProfilesPathProvider();
        private readonly ISettingIDGenerator settingIDGenerator = new SettingIDGenerator();
        private List<ProfileSetting> profileSettingList;

        public bool Load(string path, out bool loadProfileValuesFailed)
        {
            loadProfileValuesFailed = false;
            var profileData = readProfile(path);
            if (profileData == null) return false;
            initDataHeader(profileData);
            initDataComponents(profileData, out loadProfileValuesFailed);
            return true;
        }

        private void initDataHeader(ProfileData profileData)
        {
            DataHeader = new DataHeader()
            {
                ProfileName = profileData.Name,
                Model = profileData.Model,
                Status = profileData.Status
            };
        }

        private void initDataComponents(ProfileData profileData, out bool loadXTUProfileValueFailed)
        {
            DataComponents = new List<IDataComponent>();

            loadXTUProfileValueFailed = false;
            var isPredefinedProfile = false;
            var xtuPath = string.Empty;
            if (!string.IsNullOrEmpty(profileData.CPUPackage?.CPU.XtuPath))
            {
                xtuPath = Path.Combine(profilesPathProvider.ProfilesPath, profileData.CPUPackage?.CPU.XtuPath);
                if (!XTUService.LoadXTUProfile(xtuPath)) return;
                loadXTUProfileValueFailed = !XTUService.LoadXTUProfileValues(Path.GetFileNameWithoutExtension(xtuPath), out profileSettingList);
                //if (loadXTUProfileValueFailed) return;
                isPredefinedProfile = true;
            }

            if (profileData?.CPUPackage?.CPU != null)
            {
                var dataComponent = isPredefinedProfile
                    ? getCPUDataComponentFromXtuProfileData(profileData.CPUPackage, xtuPath) : getCPUDataComponentFromCustomProfileData(profileData.CPUPackage);
                if (dataComponent != null)
                    DataComponents.Add(dataComponent);
            }

            if (profileData?.CPUPackage?.Memory != null)
            {
                var dataComponent = isPredefinedProfile
                    ? getMemoryDataComponentFromXtuProfileData(profileData.CPUPackage) : getMemoryDataComponentFromCustomProfileData(profileData.CPUPackage);
                if (dataComponent != null)
                    DataComponents.Add(dataComponent);
            }
        }

        private IDataComponent getCPUDataComponentFromXtuProfileData(ProfileDataCPUPackage cpuPackage, string xtuPath)
        {

            var dataComponent = new CPUDataComponent();
            dataComponent.IsOCEnabled = Convert.ToBoolean(cpuPackage.CPU.OCEnabled);
            dataComponent.BaseClock = Convert.ToDecimal(cpuPackage.BaseClock ?? "1");
            dataComponent.Brand = cpuPackage.CPU.Brand;
            dataComponent.XtuPath = xtuPath;

            var currentBaseClock = profileSettingList.FirstOrDefault(s => s.Id ==
                SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.CPU, SettingType.BClockFrequency, 0)])?.Value ?? 100;
            dataComponent.Voltage = profileSettingList.FirstOrDefault(s => s.Id ==
                    SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.CPU, SettingType.ActualVoltage, 0)])?.Value ?? 0;
            dataComponent.VoltageOffset = profileSettingList.FirstOrDefault(s => s.Id ==
                    SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.CPU, SettingType.VoltageOffset, 0)])?.Value ?? 0;
            dataComponent.VoltageMode = Convert.ToInt32(profileSettingList.FirstOrDefault(s => s.Id ==
                    SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.CPU, SettingType.VoltageMode, 0)])?.Value ?? 0);

            dataComponent.Multiplier = profileSettingList.FirstOrDefault(s => s.Id ==
                SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.CPU, SettingType.ActualFrequency, 0)])?.Value ?? 0;
            if (dataComponent.Multiplier == 0)
                dataComponent.Multiplier = profileSettingList.FirstOrDefault(s => s.Id ==
                        SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.Core, SettingType.ActualFrequency, 0)])?.Value ?? 0;

            dataComponent.CoreDataList = new List<ICoreData>();
            for (var coreIndex = 0; coreIndex < SystemInfoRepository.Instance.CPUInfoData.PhysicalCpuCores; coreIndex++)
            {
                var actualFrequency = profileSettingList.FirstOrDefault(s => s.Id ==
                        SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.Core, SettingType.ActualFrequency, (byte)coreIndex)])?.Value ?? 0;
                dataComponent.CoreDataList.Add(new CoreData(currentBaseClock, actualFrequency));
            }

            return dataComponent;
        }

        private IDataComponent getCPUDataComponentFromCustomProfileData(ProfileDataCPUPackage cpuPackage)
        {
            var dataComponent = new CPUDataComponent();
            dataComponent.IsOCEnabled = Convert.ToBoolean(cpuPackage.CPU.OCEnabled);
            dataComponent.BaseClock = Convert.ToDecimal(cpuPackage.BaseClock ?? "100", CultureInfo.InvariantCulture);
            dataComponent.Brand = cpuPackage.CPU.Brand;
            dataComponent.Voltage = Convert.ToDecimal(cpuPackage.CPU.Voltage ?? "0", CultureInfo.InvariantCulture);
            dataComponent.Multiplier = Convert.ToInt32(cpuPackage.CPU.Multiplier ?? "0", CultureInfo.InvariantCulture);
            dataComponent.VoltageOffset = Convert.ToDecimal(cpuPackage.CPU.VoltageOffset ?? "0", CultureInfo.InvariantCulture);
            dataComponent.VoltageMode = Convert.ToInt32(cpuPackage.CPU.VoltageMode ?? "0", CultureInfo.InvariantCulture);
            dataComponent.Power = Convert.ToDecimal(cpuPackage.CPU.Power ?? "0", CultureInfo.InvariantCulture);
            dataComponent.ICCMax = Convert.ToDecimal(cpuPackage.CPU.ICCMax ?? "0", CultureInfo.InvariantCulture);
            dataComponent.CacheICCMax = Convert.ToDecimal(cpuPackage.CPU.CacheICCMax ?? "0", CultureInfo.InvariantCulture);

            if (cpuPackage.CPU.Core != null)
            {
                dataComponent.CoreDataList = new List<ICoreData>();
                foreach (var core in cpuPackage.CPU.Core)
                {
                    dataComponent.CoreDataList.Add(
                        new CoreData(dataComponent.BaseClock, Convert.ToDecimal(core.Multiplier ?? "0")));
                }
            }

            return dataComponent;
        }

        private IDataComponent getMemoryDataComponentFromXtuProfileData(ProfileDataCPUPackage cpuPackage)
        {
            var dataComponent = new MemoryDataComponent
            {
                IsOCEnabled = Convert.ToBoolean(cpuPackage.Memory.OCEnabled),
                ProfileID = Convert.ToInt32(cpuPackage.Memory.ProfileID, CultureInfo.InvariantCulture)
            };

            dataComponent.BaseClock = Convert.ToDecimal(cpuPackage.BaseClock ?? "100", CultureInfo.InvariantCulture);
            dataComponent.ClockMultiplier = profileSettingList.FirstOrDefault(s => s.Id ==
                SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.Memory, SettingType.BClockFrequency, 0)])?.Value ?? 0;
            dataComponent.Multiplier = profileSettingList.FirstOrDefault(s => s.Id ==
                SettingsIDRepository.ControlIDs[settingIDGenerator.GetID(HWComponentType.Memory, SettingType.ActualFrequency, 0)])?.Value ?? 0;

            return dataComponent;
        }

        private IDataComponent getMemoryDataComponentFromCustomProfileData(ProfileDataCPUPackage cpuPackage)
        {
            var dataComponent = new MemoryDataComponent
            {
                IsOCEnabled = Convert.ToBoolean(cpuPackage?.Memory?.OCEnabled ?? "false"),
                ProfileID = Convert.ToInt32(cpuPackage?.Memory?.ProfileID ?? "0", CultureInfo.InvariantCulture),
                Voltage = Convert.ToDecimal(cpuPackage?.Memory?.Voltage ?? "0", CultureInfo.InvariantCulture),
                Multiplier = Convert.ToDecimal(cpuPackage?.Memory?.Multiplier ?? "0", CultureInfo.InvariantCulture)
            };
            return dataComponent;
        }

        private ProfileData readProfile(string path)
        {
            if (!File.Exists(path)) return null;

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
                logger?.WriteError($"BundleProfileReader.readProfile: Failed to read encrypted data from {path}", null, e.ToString());
            }

            if (!CryptoManager?.DecryptData(ref encryptedData) ?? false)
            {
                logger?.WriteError("BundleProfileReader.readProfile: Unable to decrypt data", "Either Dominator Service is not running or decryption failed");
                return null;
            }

            var profileData = XmlSerializerHelper.DeserializeObject<ProfileData>(encryptedData);
            return profileData;

            //ProfileData profileData = null;

            //try
            //{
            //    XmlSerializer xmlSerializer = new XmlSerializer(typeof(ProfileData));
            //    using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            //    {
            //        profileData = (ProfileData) xmlSerializer.Deserialize(fs);
            //        fs.Close();
            //    }                    
            //}
            //catch (Exception e)
            //{
            //    logger?.WriteError("BundleProfileReader.readProfile", null, e.ToString());
            //}

            //return profileData;
        }
    }
}