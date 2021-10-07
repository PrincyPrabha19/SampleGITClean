/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.UXLib.Console;

namespace Dell.Client.Samples.Console
{
    class Program 
    {
        [STAThread]
        static void Main(string[] args)
        {
            var app = new ConsoleApp("Local\\{B186F188-BB2B-4F6C-ADD6-3E0032EDE868}");
            var config = new ConsoleConfig()
            {
                AppIcon = Properties.Resources.appIcon,
                ProductName = "SampleProductName",
                ServiceName = "DellServiceName",
                FooterText = "Console Footer Goes Here",
                HeaderText = "Console Header Goes Here",
                HelpFileWindowName = "ConsoleHelpFileName",
                SplashScreenText = "Insert Splash Text",
                PluginWildcards = new[] { "Dell.Client.Samples.Console.Plugins*.dll" },
                WindowText = "SampleConsole"
            };
            app.Run(config, args);
        }

    }
}
