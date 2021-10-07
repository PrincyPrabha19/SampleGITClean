using Dell.Client.Framework.Agent;
using System;
using System.ServiceProcess;

namespace Dominator.WindowsService
{
	//static class Program
	//{
	//    /// <summary>
	//    /// The main entry point for the application.
	//    /// </summary>
	//    static void Main()
	//    {
	//        var servicesToRun = new ServiceBase[]
	//        {
	//            new DominatorService()
	//        };
	//        ServiceBase.Run(servicesToRun);
	//    }
	//}

	class Program
	{
		static string myServiceName = "OCControlsWindowsService";                   // set the service name here
		static string myProductName = "OCControlsWindowsService";      // set the name of the product here
		static string myProductVersion = "1.0";                     // set the product version here

		[MTAThread]
		static void Main(string[] args)
		{
			if (args.Length == 0)
			{
				ServiceBase[] servicesToRun = { new AgentService( new AgentConfig
					{ ProductName = myProductName,
					  ProductVersion = myProductVersion,
					  ServiceName = myServiceName } ) };
				ServiceBase.Run(servicesToRun);
			}
		}

	}
}
