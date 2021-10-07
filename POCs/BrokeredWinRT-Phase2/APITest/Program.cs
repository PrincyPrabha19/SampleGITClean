using System;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using Server;

namespace APITest
{
    class Program
    {
        private static readonly uint[] colors = { 0x0F0F0000, 0x0F000F00, 0x0F00000F };
        private static int colorIndex;

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
                processColors(channel);
                //result = channel.Release();
            }
            catch
            {
            }

        }

        private static void processColors(IBIOSSupport channel)
        {
            const int ITERATIONS = 500;
            var startTime = DateTime.Now;
            var interval = 10;
            var i = 0;
            for (i = 0; i < ITERATIONS; i++)
            {
                var result = channel?.SetLightColor(0x07, Convert.ToUInt32(colors[colorIndex])) ?? false;
                //Console.WriteLine(result ? $"Iteration {i}/{ITERATIONS}. Color set to: {colors[colorIndex]:X8}" : "Failed to set color ...");

                if (++colorIndex == 3)
                    colorIndex = 0;

                Thread.Sleep(interval);
            }
            Console.WriteLine($"Time spent (ms) in {i}/{ITERATIONS} iterations: {(DateTime.Now - startTime)}");
            Console.ReadLine();
        }
    }
}
