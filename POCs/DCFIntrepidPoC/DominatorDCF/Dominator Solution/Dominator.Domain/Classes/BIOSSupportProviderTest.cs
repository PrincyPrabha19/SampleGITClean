using System;
using Dominator.Domain.Classes.Factories;
using Dominator.ServiceModel;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.Domain.Classes
{
    public class BIOSSupportProviderTest : IBIOSSupportProvider
    {
        public bool IsCPUOCStatusEnabled { get; private set; }
        public bool IsOCUIBIOSControlStatusEnabled { get; private set; }
        public bool IsOCUIBIOSControlStatusNotSupported { get; private set; }
        public bool IsOCFailsafeFlagStatusEnabled { get; private set; }
        public bool IsBIOSSupportAPIWrapperInitialized { get; private set; }
        public bool IsBIOSInterfaceNotSupported { get; private set; }

        private readonly IBIOSSupportAPIWrapper biosSupportAPIWrapper = BIOSSupportFactory.NewBIOSSupport();

        public void Initialize()
        {
            var result = (BIOSInitializationStates)biosSupportAPIWrapper.Initialize();
            IsBIOSSupportAPIWrapperInitialized = result == BIOSInitializationStates.InitializationSuccessful;
            IsBIOSInterfaceNotSupported = result == BIOSInitializationStates.NotSupportedBIOSInterface;
        }

        public bool RefreshOverclockingReport()
        {
            int overclockingReport;
            return RefreshOverclockingReport(out overclockingReport);
        }

        public bool RefreshOverclockingReport(out int overclockingReport, bool clearOCFailSafeFlag = true)
        {
            overclockingReport = 0;

            int isCPUOCStatusEnabled;
            if (BIOSSupportRegistryHelper.ReadCPUOCStatus(out isCPUOCStatusEnabled))
                IsCPUOCStatusEnabled = Convert.ToBoolean(isCPUOCStatusEnabled);

            int ocUIBIOSControlStatus;
            if (BIOSSupportRegistryHelper.ReadOCUIBIOSControlStatus(out ocUIBIOSControlStatus))
            {
                IsOCUIBIOSControlStatusEnabled = ocUIBIOSControlStatus == (int) OCUIBIOSControlStates.Enabled;
                IsOCUIBIOSControlStatusNotSupported = ocUIBIOSControlStatus == (int) OCUIBIOSControlStates.NotSupported;
            }

            int isOCFailsafeFlagStatusEnabled;
            if (BIOSSupportRegistryHelper.ReadOCFailsafeFlagStatus(out isOCFailsafeFlagStatusEnabled))
                IsOCFailsafeFlagStatusEnabled = Convert.ToBoolean(isOCFailsafeFlagStatusEnabled);

            if (clearOCFailSafeFlag)
            {
                if (IsOCFailsafeFlagStatusEnabled)
                    ClearOCFailSafeFlag();
            }

            return true;
        }

        public bool SetOCUIBIOSControl(bool enabled)
        {
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
            IsBIOSSupportAPIWrapperInitialized = false;
            return true;
        }
    }
}