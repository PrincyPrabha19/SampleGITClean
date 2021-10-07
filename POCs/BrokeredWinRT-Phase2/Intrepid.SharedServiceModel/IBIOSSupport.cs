using System.ServiceModel;

namespace Server
{
    [ServiceContract]
    public interface IBIOSSupport
    {
        [OperationContract]
        string Ping();

        [OperationContract]
        bool Initialize();

        [OperationContract]
        bool Release();

        [OperationContract]
        bool SetLightColor(uint leds, uint color);
    }
}