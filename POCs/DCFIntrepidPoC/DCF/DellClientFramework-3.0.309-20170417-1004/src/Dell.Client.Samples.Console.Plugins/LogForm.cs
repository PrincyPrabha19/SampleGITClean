/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Drawing;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Dialogs.WinForms;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    public partial class LogForm : MastheadForm
    {
        private readonly static Size DefWindowSize = new Size(1180, 700);

        private readonly SaveFormBounds formBounds;

        /// <summary>
        /// Initialize the class in default state.
        /// </summary>
        public LogForm(string strProductRegKey)
        {
            InitializeComponent();
            MaximizeBox = true;
            MinimizeBox = true;
            HelpBox = false;
            if (!string.IsNullOrEmpty(strProductRegKey))
                formBounds = new SaveFormBounds(strProductRegKey + "\\ConsoleLog");
        }

        /// <summary>
        /// This public methods adds a message to the log.
        /// </summary>
        /// <param name="message"></param>
        public void AddLogMessage(string message)
        {
            ctlLog.AddLogMessage(message);
        }

        /// <summary>
        /// Override the close button handler and just hide this form
        /// </summary>
        protected override void OnCloseButtonClick()
        {
            Hide();
        }

        protected override void OnClosed(EventArgs e)
        {
            if (formBounds != null)
                formBounds.SaveState(this);
            base.OnClosed(e);
        }

        protected override void OnLoad(EventArgs e)
        {
            if (formBounds == null)
                Size = DefWindowSize;
            else if (!formBounds.LoadState(this, DefWindowSize, Size))
                CenterToScreen();
 	        base.OnLoad(e);
        }

    }
}
