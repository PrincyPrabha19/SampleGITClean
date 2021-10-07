using System;
using Server;

namespace APITest
{
    class Program
    {
        static void Main(string[] args)
        {
            var discoveryService = new ObjectFactory().NewAlienFXDeviceDiscovery();
            var devices = discoveryService.AllDevices;
            foreach (var info in devices)
                Console.WriteLine($"VendorId: {info.VendorId}{Environment.NewLine}" +
                                  $"ProductID:{info.ProductId}{Environment.NewLine}" +
                                  $"IsInstalled:{info.IsInstalled}{Environment.NewLine}" +
                                  $"IsPresent:{info.IsPresent}{Environment.NewLine}" +
                                   "*************************************************");

            Console.ReadLine();
        }
    }
}
