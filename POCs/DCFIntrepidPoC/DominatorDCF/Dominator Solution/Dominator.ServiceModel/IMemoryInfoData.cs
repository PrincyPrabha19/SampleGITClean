using System.Collections.Generic;
using Dominator.ServiceModel.Classes.SystemInfo;

namespace Dominator.ServiceModel
{
    public interface IMemoryInfoData
    {
        double InstalledMemorySize { get; }
        string Mode { get; }
        int NumberBanks { get; }
        bool? IsMemoryOCSupported { get; set; }
        List<BankMemoryData> BankMemoryList { get; }

        void Initialize();
    }
}