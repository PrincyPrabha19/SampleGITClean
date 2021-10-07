using System.ServiceModel;
using Server;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            var binding = new NetNamedPipeBinding(NetNamedPipeSecurityMode.None);
            var address = new EndpointAddress("net.pipe://localhost/IntrepidBIOSSupport");
            var channelFactory = new ChannelFactory<IBIOSSupport>(binding, address);

            try
            {
                var channel = channelFactory.CreateChannel();
                channel.Ping();

                var result = channel.Initialize();
                result = channel.SetLightColor(0x7, 0x0F0F0F0F);
                result = channel.Release();

                //var result = AlienFXBiosSupportAPI32Map.Initialize();
                //result = AlienFXBiosSupportAPI32Map.SetLightColor(0x7, 0x0F0F0000);
                //result = AlienFXBiosSupportAPI32Map.Release();
            }
            catch
            {
            }

        }
    }
}
