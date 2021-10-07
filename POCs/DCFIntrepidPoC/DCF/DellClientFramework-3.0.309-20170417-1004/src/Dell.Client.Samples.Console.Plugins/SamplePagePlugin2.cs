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
using Dell.Client.Framework.UXLib.Dialogs;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "SamplePagePlugin2", Version = "1.0", Id = PluginId)]
    public class SamplePagePlugin2 : UserControl, IConsolePagePlugin
    {
        private const string PluginId = "{AEC634D3-BEC8-45C3-80A9-925D5B1F7A00}";
        private UXSlider uxSlider1;
        private UXButton uxButton1;
        private IConsole Console;

        #region Constructors

        public SamplePagePlugin2(IConsole console) 
        {
            Console = console;
            InitializeComponent();

            AddTile(Resources.TileUpdatesNormal, Resources.TileUpdatesHover, 400,
                "Sample2", "Sample2 Description", "Show Sample2 Page",
                 Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff), Color.FromArgb(0xff, 0xff, 0xff));
        }

        #endregion

        #region IConsolePagePlugin Methods

        public string HeaderText { get { return "Sample2"; } }

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
                Console.ShowPluginById(PluginId);
            }
        }

        private void InitializeComponent()
        {
            this.uxButton1 = new Dell.Client.Framework.UXLib.Controls.UXButton();
            this.uxSlider1 = new Dell.Client.Framework.UXLib.Controls.UXSlider();
            ((System.ComponentModel.ISupportInitialize)(this.uxSlider1)).BeginInit();
            this.SuspendLayout();
            // 
            // uxButtonLightUI1
            // 
            this.uxButton1.Location = new System.Drawing.Point(30, 37);
            this.uxButton1.Name = "uxButton1";
            this.uxButton1.Size = new System.Drawing.Size(96, 32);
            this.uxButton1.TabIndex = 2;
            this.uxButton1.Text = "Button1";
            this.uxButton1.Click += new System.EventHandler(this.uxButtonLightUI1_Click);
            // 
            // uxSliderLightUI1
            // 
            this.uxSlider1.AutomationId = null;
            this.uxSlider1.FlipDirection = false;
            this.uxSlider1.Location = new System.Drawing.Point(190, 46);
            this.uxSlider1.MaximumLabel = "Max";
            this.uxSlider1.MinimumLabel = "Min";
            this.uxSlider1.Name = "uxSlider1";
            this.uxSlider1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.uxSlider1.Size = new System.Drawing.Size(214, 38);
            this.uxSlider1.TabIndex = 1;
            this.uxSlider1.Text = "uxSliderLightUI1";
            // 
            // SamplePagePlugin2
            // 
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.uxButton1);
            this.Controls.Add(this.uxSlider1);
            this.Name = "SamplePagePlugin2";
            this.Size = new System.Drawing.Size(524, 320);
            ((System.ComponentModel.ISupportInitialize)(this.uxSlider1)).EndInit();
            this.ResumeLayout(false);

        }

        private void uxButtonLightUI1_Click(object sender, EventArgs e)
        {
            Console.ShowMessageBox("You clicked the button", "Sample 2", UXDialogButtons.Ok);
        }


    }
}
