/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Controls;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "SamplePagePlugin1", Version = "1.0", Id = PluginId)]
    public class SamplePagePlugin1 : UserControl, IConsolePagePlugin
    {
        private const string PluginId = "{43F13225-78F4-49C9-A65A-BE022A502DF3}";

        private Label label1;
        private IConsole Console;

        #region Constructors

        public SamplePagePlugin1(IConsole console) 
        {
            Console = console;
            InitializeComponent();

            AddTile(Resources.TileUsersNormal, Resources.TileUsersHover, 200,
                "Sample1", "Sample1 Description", "Show Sample1 Page",
                Color.FromArgb(0x62, 0x62, 0x62), Color.FromArgb(0x62, 0x62, 0x62), Color.FromArgb(0xff, 0xff, 0xff));
        }

        #endregion

        #region IConsolePagePlugin Methods

        public string HeaderText { get { return "Sample1"; } }

        #endregion

        private void AddTile(Image imageNormal, Image imageHover, int position,
            string strTitleText, string strSubTitleText, string strDetailText,
            Color colorTitleText, Color colorSubTitle, Color colorDetailText)
        {
            UXTile ctl = new UXTile(imageNormal, imageHover, strTitleText, strSubTitleText, strDetailText,
                colorTitleText, colorSubTitle, colorDetailText);
            ctl.Click += OnTileClick;
            Console.AddTileToHomePage(ctl, position);
        }

        private void OnTileClick(object sender, EventArgs e)
        {
            if (sender is UXTile)
            {
                label1.Text = string.Format("{0} Page plugin", HeaderText);
                Console.ShowPluginById(PluginId);
            }
        }

        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkGray;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(524, 320);
            this.label1.TabIndex = 0;
            this.label1.Text = "Page Plugin Not Available";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TileStubPlugin
            // 
            this.Controls.Add(this.label1);
            this.Name = "TileStubPlugin";
            this.Size = new System.Drawing.Size(524, 320);
            this.ResumeLayout(false);

        }


    }
}
