namespace Dominator.ServiceModel
{
    public interface ICPUInfoData
    {
        string ProcessorBrand { get; set; }
        string CodeName { get; set; }
        string FeatureFlags { get; set; }
        uint PhysicalCpuCores { get; set; }
        uint LogicalCpuCores { get; set; }
        bool IsOverclockSupported { get; set; }
        bool IsTurboBoostTechnologyEnabled { get; set; }
    }
}