using System.Runtime.Serialization;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    [DataContract]
    public class XMPInfoData : IXMPInfoData
    {
        [DataMember]
        public int NumberOfXMPProfiles { get; set; }

        [DataMember]
        public bool IsXMPSupported { get; set; }

        public override string ToString()
        {
            return $"\tIsXMPSupported: {IsXMPSupported}\nNumberOfXMPProfiles: {NumberOfXMPProfiles}";
        }
    }
}