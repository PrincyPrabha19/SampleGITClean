using System.Runtime.Serialization;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    [DataContract]
    public sealed class CPUInfoData : ICPUInfoData
    {
        [DataMember]
        public string ProcessorBrand { get; set; }

        [DataMember]
        public string CodeName { get; set; }

        [DataMember]
        public string FeatureFlags { get; set; }

        [DataMember]
        public uint PhysicalCpuCores { get; set; }

        [DataMember]
        public uint LogicalCpuCores { get; set; }
       
        [DataMember]
        public bool IsOverclockSupported { get; set; }

        [DataMember]
        public bool IsTurboBoostTechnologyEnabled { get; set; }

        public override string ToString()
        {
            return $"ProcessorBrand: {ProcessorBrand}\nFeatureFlags: {FeatureFlags}\nPhysicalCpuCores: {PhysicalCpuCores}\nLogicalCpuCores: {LogicalCpuCores}\nIsOverclockSupported: {IsOverclockSupported}\nIsTurboBoostTechnologyEnabled: {IsTurboBoostTechnologyEnabled}";
        }
    }
}