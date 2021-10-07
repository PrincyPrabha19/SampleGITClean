/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "GearPluginBrowser", Version = "1.0", Id = PluginId)]
    public class GearPluginBrowser : UserControl, IConsolePagePlugin 
    {
        private const string PluginId = "{6954D88F-38BF-41D8-8170-C423966BC2CA}";
        private WebBrowser webBrowser1;

        private IConsole Console;

        public GearPluginBrowser(IConsole console)
        {
            Console = console;
            InitializeComponent();
            console.AddGearMenuItem("Browser Page", 100, OnGearItemClick);
        }

        #region IConsolePagePlugin Methods

        public string HeaderText { get { return "Web Browser"; } }

        #endregion

        /// <summary>
        /// This method is called when the user clicks on the menuitem in the gear.  We simply show this 
        /// page plugin.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnGearItemClick(object sender, EventArgs e)
        {
            Console.ShowPluginById(PluginId);
        }

        private void InitializeComponent()
        {
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.SuspendLayout();
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.ScriptErrorsSuppressed = true;
            this.webBrowser1.Size = new System.Drawing.Size(389, 269);
            this.webBrowser1.TabIndex = 0;
            this.webBrowser1.Url = new System.Uri("http://google.com", System.UriKind.Absolute);
            // 
            // GearPluginBrowser
            // 
            this.Controls.Add(this.webBrowser1);
            this.Name = "GearPluginBrowser";
            this.Size = new System.Drawing.Size(389, 269);
            this.ResumeLayout(false);

        }


     }
}
