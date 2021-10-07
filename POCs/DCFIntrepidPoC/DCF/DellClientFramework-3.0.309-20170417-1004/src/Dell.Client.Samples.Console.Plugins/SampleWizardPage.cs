/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Drawing;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Console;
using Dell.Client.Framework.UXLib.Controls;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This plugin demonstrates a sample wizard
    /// </summary>
    [PluginInfo(Name = "SampleWizardPage", Version = "1.0", Id = PluginId)]
    public class SampleWizardPage : WizardPage, IWizardPagePlugin
    {
        private const string PluginId = "{517D98E3-7EB7-4B20-8938-B94B5EBCFA3E}";

        #region Constructors

        /// <summary>
        /// This method initializes the class in default state.
        /// </summary>
        /// <param name="console"></param>
        public SampleWizardPage(IConsole console)
            : base(console)
        {
            HeaderText = "This is the Wizard Header";
            PageId = "samplewizard";
            var tileSetup = new UXTile(Resources.TileSettingsNormal, Resources.TileSettingsHover,
                "Sample Wizard", "Runs a sample wizard", "",
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff));
            tileSetup.Click += (o, args) => ShowDialog();
            Console.AddTileToHomePage(tileSetup, 999);
        }

        #endregion

        #region IConsoleNavPagePlugin 

        /// <summary>
        /// Returns an identifier to be used to map associated panels.
        /// </summary>
        public string PageId { get; private set; }

        #endregion

    }
}
