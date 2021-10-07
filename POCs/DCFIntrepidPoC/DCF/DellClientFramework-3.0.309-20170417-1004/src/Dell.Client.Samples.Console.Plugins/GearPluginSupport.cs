/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "GearPluginSupport", Version = "1.0", Id = "{9B7E9F73-F544-4F15-8051-84F762744980}")]
    public class GearPluginSupport : IConsolePlugin
    {
        public GearPluginSupport(IConsole console)
        {
            console.AddGearMenuItem("Support", 800, OnGearItemClick);
        }

        /// <summary>
        /// This method is called when the user clicks on the menuitem in the gear.  We simply show this 
        /// page plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearItemClick(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://support.dell.com");
        }


     }
}
