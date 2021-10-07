/*
 * @Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Controls;

namespace Dell.Client.Samples.Console.Plugins
{
     /// <summary>
    /// This class implements a sample Console Home Page plugin.  This plugin supports UXTiles that can be 
    /// automatically positioned in the client area based on the size of the Window.
    /// </summary>
    [PluginInfo(Name = "HomePagePlugin", Version = "1.0", Id = "{F138F473-CF66-4A83-8A6A-5A256CB81A39}")]
    public class SampleHomePagePlugin : UserControl, IConsoleHomePagePlugin
    {
        private Panel panelLeft;
        private UXScrollablePanel panelTiles;

        /// <summary>
        /// Initializes the object in default state.
        /// </summary>
        public SampleHomePagePlugin(IConsole console)
        {
            InitializeComponent();
            base.BackColor = Color.Transparent;
            panelLeft.Visible = false;
        }

        #region IConsoleHomePagePlugin Methods

        /// <summary>
        /// Gets or sets the Header text string
        /// </summary>
        public string HeaderText { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Adds a tile to the home page at the specified position.
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="position"></param>
        public void AddControl(Control tile, int position)
        {
            tile.Tag = position;
            List<Control> ctls = new List<Control>();
            foreach (Control ctl in panelTiles.Controls)
                ctls.Add(ctl);
            panelTiles.Controls.Clear();
            int Idx = 0;
            while (Idx < ctls.Count)
            {
                if (ctls[Idx].Tag != null && (int)ctls[Idx].Tag > position)
                    break;
                ++Idx;
            }
            ctls.Insert(Idx, tile);
            foreach (Control ctl in ctls)
                panelTiles.Controls.Add(ctl);
        }

        /// <summary>
        /// Removes a tile from the home page.
        /// </summary>
        /// <param name="tile"></param>
        public void RemoveControl(Control tile)
        {
            panelTiles.Controls.Remove(tile);
        }

        #endregion

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelTiles = new Dell.Client.Framework.UXLib.Controls.UXScrollablePanel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // panelTiles
            // 
            this.panelTiles.AutoFlowControls = true;
            this.panelTiles.AutoFlowXSpacing = 20;
            this.panelTiles.AutoFlowYSpacing = 20;
            this.panelTiles.BackColor = System.Drawing.Color.Transparent;
            this.panelTiles.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTiles.Location = new System.Drawing.Point(187, 0);
            this.panelTiles.Name = "panelTiles";
            this.panelTiles.Padding = new System.Windows.Forms.Padding(8, 12, 0, 0);
            this.panelTiles.Size = new System.Drawing.Size(402, 378);
            this.panelTiles.TabIndex = 43;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(187, 378);
            this.panelLeft.TabIndex = 44;
            // 
            // HomePage
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.panelTiles);
            this.Controls.Add(this.panelLeft);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SampleHomePagePlugin";
            this.Size = new System.Drawing.Size(589, 378);
            this.ResumeLayout(false);

        }

    }
}
