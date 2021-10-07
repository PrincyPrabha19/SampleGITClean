using System;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.Domain.Classes
{
    public class BIOSSupportProvider : IBIOSSupportProvider
    {
        public bool IsCPUOCStatusEnabled { get; private set; }
        public bool IsOCUIBIOSControlStatusEnabled { get; private set; }
        public bool IsOCUIBIOSControlStatusNotSupported { get; private set; }
        public bool IsOCFailsafeFlagStatusEnabled { get; private set; }
        public bool IsBIOSSupportAPIWrapperInitialized { get; private set; }
        public bool IsBIOSInterfaceNotSupported { get; private set; }

        private readonly IBIOSSupportAPIWrapper biosSupportAPIWrapper = BIOSSupportFactory.NewBIOSSupport();
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;

        public void Initialize()
        {
            try
            {
                var result = (BIOSInitializationStates)biosSupportAPIWrapper.Initialize();
                IsBIOSSupportAPIWrapperInitialized = result == BIOSInitializationStates.InitializationSuccessful;
                IsBIOSInterfaceNotSupported = result == BIOSInitializationStates.NotSupportedBIOSInterface;
            }
            catch
            {
            }
        }

        public bool RefreshOverclockingReport()
        {
            int overclockingReport;
            return RefreshOverclockingReport(out overclockingReport);
        }

        public bool RefreshOverclockingReport(out int overclockingReport, bool clearOCFailSafeFlag = true)
        {
            overclockingReport = 0;

            try
            {
                IsCPUOCStatusEnabled = false;
                IsOCUIBIOSControlStatusEnabled = false;
                IsOCFailsafeFlagStatusEnabled = false;

                logger?.WriteLine($"BIOSSupport.Initialized: {biosSupportAPIWrapper.IsInitialized()}");
                if (!biosSupportAPIWrapper.IsInitialized()) return false;

                if (!biosSupportAPIWrapper.ReturnOverclockingReport(out overclockingReport)) return false;
                logger?.WriteLine($"BIOSSupport.ReturnOverclockingReport: 0x{overclockingReport.ToString("X8")}");
                if (overclockingReport < 0) return false;

                bool isCPUOCStatusEnabled, isOCUIBIOSStatusControlEnabled, isOCUIBIOSControlStatusNotSupported, isOCFailsafeFlagEnabled;
                getOCStatusFromOCReport(overclockingReport, out isCPUOCStatusEnabled, out isOCUIBIOSStatusControlEnabled, out isOCUIBIOSControlStatusNotSupported, out isOCFailsafeFlagEnabled);

                IsCPUOCStatusEnabled = isCPUOCStatusEnabled;
                IsOCUIBIOSControlStatusEnabled = isOCUIBIOSStatusControlEnabled;
                IsOCUIBIOSControlStatusNotSupported = isOCUIBIOSControlStatusNotSupported;
                IsOCFailsafeFlagStatusEnabled = isOCFailsafeFlagEnabled;

                if (clearOCFailSafeFlag)
                {
                    if (IsOCFailsafeFlagStatusEnabled)
                        ClearOCFailSafeFlag();
                }

                logger?.WriteLine($"IsCPUOCStatusEnabled: {IsCPUOCStatusEnabled} IsOCUIBIOSControlStatusEnabled: {IsOCUIBIOSControlStatusEnabled} IsOCFailsafeFlagEnabled: {IsOCFailsafeFlagStatusEnabled}");
            }
            catch (Exception e)
            {
                logger?.WriteError("RefreshOverclockingReport failed!", null, e.ToString());
                return false;
            }

            return true;
        }

        public bool SetOCUIBIOSControl(bool enabled)
        {
            if (IsOCUIBIOSControlStatusNotSupported)
                return true;
            return biosSupportAPIWrapper.SetOCUIBIOSControl(enabled);
        }

        public bool ClearOCFailSafeFlag()
        {
            return biosSupportAPIWrapper.ClearOCFailSafeFlag();
        }

        public void ResetOCFailSafeFlag()
        {
            IsOCFailsafeFlagStatusEnabled = false;
        }

        public bool Release()
        {
            return biosSupportAPIWrapper.Release();
        }

        private void getOCStatusFromOCReport(int overclockingReport, out bool isCPUOCStatusEnabled, out bool isOCUIBIOSControlStatusEnabled, out bool isOCUIBIOSControlStatusNotSupported, out bool isOCFailsafeFlagEnabled)
        {
            var _cpuOCStatus = (byte)(overclockingReport & 0xff);
            var _ocBIOSUIControlStatus = (byte)((overclockingReport >> 8) & 0xff);
            var _failSafeStatus = (byte)((overclockingReport >> 16) & 0xff);

            isCPUOCStatusEnabled = _cpuOCStatus == 0x01;
            isOCUIBIOSControlStatusEnabled = _ocBIOSUIControlStatus == 0x01;
            isOCUIBIOSControlStatusNotSupported = _ocBIOSUIControlStatus == 0x00;
            isOCFailsafeFlagEnabled = _failSafeStatus == 0x01;
        }
    }
}