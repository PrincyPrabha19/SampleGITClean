using System.ServiceModel;
namespace Server
{
    public sealed class AlienFXComponent
    {
        private IBIOSSupport instance;
        private ChannelFactory<IBIOSSupport> channelFactory; 

        #region Methods
        public bool InitializeAPI()
        {
            if (instance == null)
                instance = getBIOSSupportInstance();

            return instance?.Initialize() ?? false;
        }

        public bool ReleaseAPI()
        {
            var result = instance?.Release() ?? false;
            if (!result)
                return false;

            channelFactory.Close();
            instance = null;

            return true;
        }

        public bool SetLightColor(uint leds, uint color)
        {
            return instance?.SetLightColor(leds, color) ?? false;
        }

        private IBIOSSupport getBIOSSupportInstance()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var address = new EndpointAddress("net.pipe://localhost/IntrepidBIOSSupport");
            channelFactory = new ChannelFactory<IBIOSSupport>(binding, address);

            try
            {
                var channel = channelFactory.CreateChannel();
                channel.Ping();
                return channel;
            }
            catch {}

            return null;
        }
        #endregion
    }
}
