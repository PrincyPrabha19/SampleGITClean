using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Dominator.ServiceModel.Classes.Helpers;
using Dominator.ServiceModel.Enums;

namespace Dominator.ServiceModel.Classes.BIOSSupport
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class BIOSSupportAPIWrapperTest : IBIOSSupportAPIWrapper
    {
        #region Properties
        private bool initialized;
        #endregion

        #region Methods
        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public int Initialize()
        {
            int status;
            if (BIOSSupportRegistryHelper.ReadBIOSInitStatus(out status))
            {
                initialized = true;
                return status;
            }

            return (int)BIOSInitializationStates.InitializationFailed;
        }

        public bool ReturnOverclockingReport(out int status)
        {
            status = 0;
            return true;
        }

        public bool SetOCUIBIOSControl(bool enabled)
        {
            return BIOSSupportRegistryHelper.WriteOCUIBIOSControlStatus(enabled ? (int) OCUIBIOSControlStates.Enabled : (int) OCUIBIOSControlStates.Disabled);
        }

        public bool ClearOCFailSafeFlag()
        {
            return BIOSSupportRegistryHelper.WriteOCFailsafeFlagStatus(0);
        }
        public bool Release()
        {
            initialized = false;
            return true;
        }

        public bool IsInitialized()
        {
            return initialized;
        }
        #endregion
    }
}