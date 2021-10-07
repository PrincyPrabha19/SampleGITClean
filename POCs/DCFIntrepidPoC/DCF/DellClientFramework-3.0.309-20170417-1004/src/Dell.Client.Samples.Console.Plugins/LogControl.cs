/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Drawing;
using System.Windows.Forms;
using Dell.Client.Samples.Console.Plugins.Properties;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This implements a custom control that displays log messages.
    /// </summary>
    public partial class LogControl : UserControl
    {
        static private readonly Color LogTextColor = Color.FromArgb(0x4e, 0x4e, 0x4f);
        static private readonly Font LogTextFont = new Font("Courier New", 8.25F, FontStyle.Regular);
        private const int LogMaxListItems = 500;

        private delegate void AddLogMessageCallback(string s);

        /// <summary>
        /// This constructs the class in default state.
        /// </summary>
        public LogControl()
        {
            InitializeComponent();
            ListBox.Font = LogTextFont;
            ListBox.ForeColor = LogTextColor;
            ListBox.HorizontalScrollbar = true; 
            menuItemClearLog.Text = Resources.strConsoleGearPluginLogClearLog;
            menuItemCopyToClipboard.Text = Resources.strConsoleGearPluginLogCopyToClipboard;
        }

        /// <summary>
        /// This method adds a log message to the listview.  We first make sure that the calling
        /// thread is the same thread as the creator.  If not, we must use Invoke to dispatch the method
        /// back or Windows Forms will complain.  This method will add the new message to the end of the 
        /// listview, remove any old messages if the max count of the listview has been exceeded.
        /// </summary>
        /// <param name="message"></param>
        public void AddLogMessage(string message)
        {
            if (ListBox.InvokeRequired)
            {
                AddLogMessageCallback d = AddLogMessage;
                ListBox.Invoke(d, new object[] { message });
            }
            else
            {
                ListBox.Items.Add(message);
                while (ListBox.Items.Count > LogMaxListItems)
                    ListBox.Items.RemoveAt(0);
                ListBox.SelectedIndex = ListBox.Items.Count - 1;
                ListBox.ClearSelected();
            }
        }

        /// <summary>
        /// This method is called when the user clicks on the clear log menu item.
        /// We delete all the entries in the list box, and update the display.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnClearLogClick(object sender, EventArgs e)
        {
            ListBox.Items.Clear();
        }

        /// <summary>
        /// This method is called when the user clicks on the context menu and chooses
        /// to copy selected lines to the clipboard.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCopyToClipboardClick(object sender, EventArgs e)
        {
            var s = string.Empty;
            foreach (int idx in ListBox.SelectedIndices)
            {
                s += ListBox.Items[idx];
                s += Environment.NewLine;
            }
            if (!string.IsNullOrEmpty(s))
                Clipboard.SetText(s);
        }
        
    }
}
