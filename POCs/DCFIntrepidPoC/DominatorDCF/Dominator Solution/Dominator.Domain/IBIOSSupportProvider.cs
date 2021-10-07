namespace Dominator.Domain
{
    public interface IBIOSSupportProvider
    {
        bool IsCPUOCStatusEnabled { get; }
        bool IsOCUIBIOSControlStatusEnabled { get; }
        bool IsOCFailsafeFlagStatusEnabled { get; }
        bool IsBIOSSupportAPIWrapperInitialized { get; }
        bool IsBIOSInterfaceNotSupported { get; }
        bool IsOCUIBIOSControlStatusNotSupported { get; }

        void Initialize();
        bool RefreshOverclockingReport(out int overclockingReport, bool clearOCFailSafeFlag);
        bool SetOCUIBIOSControl(bool enabled);
        bool ClearOCFailSafeFlag();
        bool Release();
        bool RefreshOverclockingReport();
        void ResetOCFailSafeFlag();
    }
}
