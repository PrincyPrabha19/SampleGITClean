namespace Dominator.ServiceModel
{
    public interface IXMPInfoData
    {
        int NumberOfXMPProfiles { get; set; }
        bool IsXMPSupported { get; set; }
    }
}