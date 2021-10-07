using System;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace Server
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed),
     ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public sealed class BIOSSupport : IBIOSSupport
    {
        #region Properties
        private bool initialized = false;
        #endregion

        #region Methods
        public string Ping()
        {
            return DateTime.Now.ToString("u");
        }

        public bool Initialize()
        {
            if (initialized) return true;

            try
            {
                initialized = AlienFXAPIWrapper.Initialize() == 0;
                return initialized;
            }
            catch (Exception e)
            {
                AlienFXAPIWrapper.log("Unable to Initialize BIOS Support: " + Environment.NewLine + e.Message);                
            }

            return false;
        }

        public bool Release()
        {
            if (!initialized) return false;

            try
            {
                return AlienFXAPIWrapper.Release() == 0;
            }
            catch (Exception e)
            {
                AlienFXAPIWrapper.log("Unable to Release BIOS Support: " + Environment.NewLine + e.Message);
            }

            return false;
        }


        public bool SetLightColor(uint leds, uint color)
        {
            if (!initialized) return false;

            try
            {
                return AlienFXAPIWrapper.SetLightColor(leds, color) == 0;
            }
            catch (Exception e)
            {
                AlienFXAPIWrapper.log("Unable to Set Color using BIOS Support: " + Environment.NewLine + e.Message);
            }

            return false;
        }
        #endregion
    }
}