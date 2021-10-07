using Dominator.ServiceModel.Enums;
using Dominator.Tools.Enums;

namespace Dominator.ServiceModel
{
    public interface IPlatformInfoData
    {
        string Platform { get; set; }
        string Motherboard { get; set; }
        string BiosVersion { get; set; }
        string BiosDate { get; set; }
        bool UEFI { get; set; }
        OSType OS { get; set; }
        PlatformType PlatformType { get; set; }

        void Initialize();
    }
}