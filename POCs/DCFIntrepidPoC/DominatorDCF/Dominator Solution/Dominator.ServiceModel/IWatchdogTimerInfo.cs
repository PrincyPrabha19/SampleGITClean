namespace Dominator.ServiceModel
{
    public interface IWatchdogTimerInfo
    {
        bool IsWatchdogPresent { get; set; }
        bool IsWatchdogRunningAtBoot { get; set; }
        bool HasWatchdogFailed { get; set; }
    }
}
