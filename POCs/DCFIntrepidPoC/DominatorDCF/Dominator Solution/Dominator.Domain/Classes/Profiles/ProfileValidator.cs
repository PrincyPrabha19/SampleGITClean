using System;
using System.Linq;
using Dominator.ServiceModel.Classes.Factories;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes.Profiles
{
    public class ProfileValidator : IProfileValidator
    {
        public bool IsValidProfileForSystem(IProfile profile)
        {
            var platform = SystemInfoRepository.Instance.PlatformInfoData.Platform;
            var processorBrand = SystemInfoRepository.Instance.CPUInfoData.ProcessorBrand;

            var profileProcessorBrand = string.Empty;
            var cpuDataComponent = profile.DataComponents.FirstOrDefault(dc => dc.Type == HWComponentType.CPU) as CPUDataComponent;
            if (cpuDataComponent != null)
                profileProcessorBrand = cpuDataComponent.Brand;

            return !string.IsNullOrEmpty(profileProcessorBrand) &&
                   processorBrand.IndexOf(profileProcessorBrand, StringComparison.InvariantCultureIgnoreCase) >= 0 &&
                   string.Compare(profile.DataHeader.Model, platform, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
    }
}