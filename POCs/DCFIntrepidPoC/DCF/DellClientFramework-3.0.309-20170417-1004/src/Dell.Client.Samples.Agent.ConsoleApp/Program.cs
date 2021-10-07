/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.Agent;

namespace Dell.Client.Samples.Agent.ConsoleApp
{
    /// <summary>
    /// This sample demonstrates how to run the Agent in a console application.  Just need
    /// to instance the Agent object and call Start() and Stop() methods.
    /// </summary>
    static class Program
    {
        [MTAThread]
        static void Main(string[] args)
        {
            var productName = Properties.Settings.Default.ProductName;
            var productVersion = Properties.Settings.Default.ProductVersion;
            var serviceName = Properties.Settings.Default.ServiceName;
            var companyName = Properties.Settings.Default.CompanyName;

            foreach (var arg in args)
            {
                if (arg.ToLower().StartsWith("servicename="))
                    serviceName = arg.Substring("servicename=".Length);
                else if (arg.ToLower().StartsWith("productname="))
                    productName = arg.Substring("productname=".Length);
            }

            Console.WriteLine("Running the Dell Client Framework Agent as a console application\n");
            Console.WriteLine("CompanyName is \"{0}\"", companyName);
            Console.WriteLine("ServiceName is \"{0}\"", serviceName);
            Console.WriteLine("ProductName is \"{0}\"", productName);
            Console.WriteLine("ProductVersion is \"{0}\"", productVersion);

            //
            // Now, create the AgentConfig object and start the agent thread
            //
            var agentConfig = new AgentConfig
            {
                ServiceName = serviceName,
                ProductName = productName,
                ProductVersion = productVersion,
                CompanyName = companyName,
                AllowUnelevatedExecution = true         // set to true to allow the Agent to run unelevated
            };
            try
            {
                using (var agent = new Framework.Agent.Agent(agentConfig))
                {
                    agent.StartThread();
                    //
                    // Stay in this loop until the app is requested to close
                    //
                    var bQuit = false;
                    while (!bQuit)
                    {
                        Console.WriteLine("\nMenu Selection");
                        Console.WriteLine(" 1 -> {0} the agent", agent.IsThreadAlive ? "Stop" : "Start");
                        Console.WriteLine(" Q -> Quit program");
                        Console.Write("\nEnter selection > ");
                        string r;
                        do
                        {
                            r = Console.ReadLine();
                            if (r == null)
                                System.Threading.Thread.Sleep(1000);
                        } while (r == null);
                        r = r.ToUpper();
                        if (r == "1")
                        {
                            if (agent.IsThreadAlive)
                                agent.StopThread();
                            else
                                agent.StartThread();
                        }
                        else if (r == "Q")
                        {
                            if (agent.IsThreadAlive)
                                agent.StopThread();
                            bQuit = true;
                        }
                        else
                            Console.WriteLine("ERROR! Invalid Selection!");
                    }
                    //
                    // Stop the agent
                    //
                    agent.StopThread();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception occurred - {0}", ex);
                Console.ReadLine();                
            }
        }
    }
}
