

using System.ServiceModel;

namespace Server.Classes
{
    public static class BIOSSupportFactory
    {
        private static IBIOSSupport biosSupport;
        public static IBIOSSupport NewBIOSSupport()
        {
            return biosSupport ?? (biosSupport = getBIOSSupportInstance());
        }

        private static IBIOSSupport getBIOSSupportInstance()
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var address = new EndpointAddress("net.pipe://localhost/IntrepidBIOSSupport");
            var channelFactory = new ChannelFactory<IBIOSSupport>(binding, address);

            try
            {
                var channel = channelFactory.CreateChannel();
                channel.Ping();
                return channel;
            }
            catch
            {
            }

            return null;
        }
    }
}