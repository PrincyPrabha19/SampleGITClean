using System.Runtime.Serialization;

namespace Dominator.ServiceModel.Classes.SystemInfo
{
    [DataContract]
    public class WatchdogTimerInfo : IWatchdogTimerInfo
    {
        [DataMember]
        public bool IsWatchdogPresent { get; set; }
        [DataMember]
        public bool IsWatchdogRunningAtBoot { get; set; }
        [DataMember]
        public bool HasWatchdogFailed { get; set; }
        public override string ToString()
        {
            return $"\tIsWatchdogPresent: {IsWatchdogPresent}\nIsWatchdogRunningAtBoot: {IsWatchdogRunningAtBoot}\nIsWatchdogFailed: {HasWatchdogFailed}";
        }
    }
}
