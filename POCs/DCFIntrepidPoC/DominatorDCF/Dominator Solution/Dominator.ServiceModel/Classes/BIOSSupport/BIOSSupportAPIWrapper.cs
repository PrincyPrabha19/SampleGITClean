using System;
using System.ServiceModel;
using System.ServiceModel.Activation;
using Dominator.ServiceModel.Enums;
using Dominator.Tools;
using Dominator.Tools.Classes.Factories;

namespace Dominator.ServiceModel.Classes.BIOSSupport
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class BIOSSupportAPIWrapper : IBIOSSupportAPIWrapper
    {
        #region Properties        
        private static readonly ILogger logger = LoggerFactory.LoggerInstance;
        private bool initialized;
        #endregion

        #region Methods
        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public int Initialize()
        {
            if (initialized) return (int)BIOSInitializationStates.InitializationSuccessful;

            try
            {
                var result = BIOSSupportAPIMap.Initialize();
                if (result == (int) BIOSInitializationStates.InitializationSuccessful)
                    initialized = true;
                return result;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to Initialize BIOS Support", null, e.Message);                
            }

            return (int)BIOSInitializationStates.InitializationFailed;
        }

        public bool ReturnOverclockingReport(out int status)
        {
            status = 0;
            if (!initialized) return false;

            try
            {
                status = BIOSSupportAPIMap.ReturnOverclockingReport();
                return status >= 0;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to call BIOS' ReturnOverclockingReport function.", e.Message);
            }

            return false;
        }

        public bool SetOCUIBIOSControl(bool enabled)
        {
            if (!initialized) return false;

            try
            {
                return BIOSSupportAPIMap.SetOCUIBIOSControl(enabled) == 0;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to call BIOS' SetOCUIBIOSControl function.", e.Message);
            }

            return false;
        }

        public bool ClearOCFailSafeFlag()
        {
            if (!initialized) return false;

            try
            {
                return BIOSSupportAPIMap.ClearOCFailSafeFlag() == 0;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to call BIOS' ClearOCFailSafeFlag function.", e.Message);
            }

            return false;
        }
        public bool Release()
        {
            if (!initialized) return false;

            try
            {
                initialized = false;
                return BIOSSupportAPIMap.Release() == 0;
            }
            catch (Exception e)
            {
                logger?.WriteError("Unable to call BIOS' Release function.", e.Message);
            }

            return false;
        }

        public bool IsInitialized()
        {
            return initialized;
        }
        #endregion
    }
}