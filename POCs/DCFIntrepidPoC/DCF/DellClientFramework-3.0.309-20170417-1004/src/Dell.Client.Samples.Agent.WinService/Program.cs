/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.ServiceProcess;
using Dell.Client.Framework.Agent;

namespace Dell.Client.Samples.Agent.WinService
{
    class Program 
    {
        static string myServiceName = "DCFAgent";                   // set the service name here
        static string myProductName = "Dell Client Framework";      // set the name of the product here
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
