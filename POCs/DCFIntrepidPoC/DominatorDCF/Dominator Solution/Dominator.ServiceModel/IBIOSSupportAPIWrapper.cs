using System.ServiceModel;

namespace Dominator.ServiceModel
{
    [ServiceContract]
    public interface IBIOSSupportAPIWrapper
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        int Initialize();

        [OperationContract]
        bool ReturnOverclockingReport(out int status);

        [OperationContract]
        bool SetOCUIBIOSControl(bool enabled);

        [OperationContract]
        bool ClearOCFailSafeFlag();

        [OperationContract]
        bool Release();

        [OperationContract]
        bool IsInitialized();
    }
}