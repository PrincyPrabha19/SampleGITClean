/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This class implements a console plugin that implements the displaying of log messages in a separate
    /// window.  This plugin is used primarily for debugging and problem determination.
    /// </summary>
    [PluginInfo(Name = "GearPluginLog", Version = "1.0", Id = "{2D1918D1-3423-475B-8C53-E27F79E7E42E}")]
    public partial class GearPluginLog : IConsolePlugin
    {
        private LogForm form;

        /// <summary>
        /// This constructor is called by Console when it instantiates this plugin. 
        /// </summary>
        /// <param name="console"></param>
        public GearPluginLog(IConsole console)
        {
            form = new LogForm(console.ProductRegKey);
            form.Text = Resources.strConsoleGearPluginLogText;
            console.LogEvent += OnConsoleLogMessage;
            //
            // Make this log accessible from the gear
            //
            console.AddGearMenuItem(Resources.strConsoleGearPluginLogText, 900, OnShowLog);
        }

        /// <summary>
        /// This method is called when the management console receives a new log message.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnConsoleLogMessage(object sender, LogEventHandlerArgs e)
        {
            form.AddLogMessage(e.Msg);
        }

        /// <summary>
        /// This method is called when the user clicks on the menu item to show the log.  The form is already
        /// active, so we just make sure it is visible in case the user hid the form.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnShowLog(object sender, EventArgs e)
        {
            if (!form.Visible)
                form.Show();
        }
    
    }
}
