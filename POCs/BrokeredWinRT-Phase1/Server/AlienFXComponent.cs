using Server.Classes;

namespace Server
{
    public sealed class AlienFXComponent
    {
        private IBIOSSupport instance;

        #region Methods
        public bool InitializeAPI()
        {
            if (instance == null)
                instance = BIOSSupportFactory.NewBIOSSupport();

            return instance.Initialize();
        }

        public bool ReleaseAPI()
        {
            return instance?.Release() ?? false;
        }

        public bool SetLightColor(uint leds, uint color)
        {
            return instance?.SetLightColor(leds, color) ?? false;
        }
        #endregion
    }
}
