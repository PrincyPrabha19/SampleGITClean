/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Drawing;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Console;
using Dell.Client.Framework.UXLib.Controls;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This plugin installs a "Setup" tile and displays a LeftNavPage that includes all of
    /// the setup panels.
    /// </summary>
    [PluginInfo(Name = "SampleLeftNavPagePlugin", Version = "1.0", Id = PluginId)]
    public class SampleLeftNavPagePlugin : ConsolePageLeftNav, IConsoleNavPagePlugin, IConsolePluginSupportsDirtyPages 
    {
        private const string PluginId = "{2683DD8D-6996-4DA5-873F-F914F3DA5547}";

        private UXTile tileSetup; 
        private Log Log;

        #region Constructors

        /// <summary>
        /// This method initializes the class in default state.
        /// </summary>
        /// <param name="console"></param>
        public SampleLeftNavPagePlugin(IConsole console)
            : base(console)
        {
            Console = console;
            Log = console.Log;
            CollapsedText = "Expand LeftNav";
            //
            // Add the tile that shows this page
            //
            tileSetup = new UXTile(Resources.TileSettingsNormal, Resources.TileSettingsHover,
                "Show LeftNav", "LeftNav Sample1", "",
                Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff));
            tileSetup.Click += new EventHandler(OnTileClick);
            Console.AddTileToHomePage(tileSetup, 100);
        }

        #endregion

        #region IConsoleNavPagePlugin 

        /// <summary>
        /// Returns the text to display in the masthead header when this plugin is active.
        /// </summary>
        public string HeaderText { get { return "Sample LeftNav1 Page"; } }

        /// <summary>
        /// Returns an identifier to be used to map associated panels.
        /// </summary>
        public string PageId 
        { 
            get { return "SampleLeftNav1Page"; }
        }

        #endregion

        #region Private Methods

        protected override void OnLeftNavSizeChanged(object sender, EventArgs e)
        {
            Console.RedrawWindow();
        }

        /// <summary>
        /// This method will be called if there are no navigation items.  In this case, we unload
        /// ourselves.
        /// </summary>
        protected override void UnloadPlugin()
        {
            Console.RemoveTileFromHomePage(tileSetup);
            Console.UnloadPlugin(this);
        }

        /// <summary>
        /// This is called when the tile on the home page is clicked.  This gives us access to the
        /// Setup features.  We must first insure that we are running elevated with administrator
        /// privileges.  If not, we request the console to rerun elevated.  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnTileClick(object sender, EventArgs e)
        {
            Console.ShowPluginById(PluginId);
        }

        #endregion

    }
}
