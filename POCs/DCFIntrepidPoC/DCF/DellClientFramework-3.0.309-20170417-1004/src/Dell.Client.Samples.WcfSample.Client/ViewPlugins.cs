/*
 * ©Copyright Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.WcfSample.Client
{
    /// <summary>
    /// This class implements a security console plugin that display information about
    /// the services that are plugged into the Agent.  
    /// </summary>
    public partial class ViewPlugins : UserControl
    {
        delegate void UpdateInfoCallback(List<AgentPluginInfo> info);

        public ViewPlugins()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Selectable, false);
            base.BackColor = Color.Transparent;
            InitializeComponent();
            BorderStyle = BorderStyle.None;
            labelHeader.Text = string.Empty;

            listView1.View = View.Details;
            listView1.AllowColumnReorder = true;
            listView1.FullRowSelect = true;
            listView1.GridLines = true;
            listView1.Sorting = SortOrder.Ascending;

            listView1.Columns.Add("Name");
            listView1.Columns.Add("Version");
            listView1.Columns.Add("Enabled");
            listView1.Columns.Add("State");
            listView1.Columns.Add("Interface");
            //listView1.Columns.Add("Identifier"); 
            listView1.Columns.Add("Description");
        }

        /// <summary>
        /// This method updates the control with the new provider information.  It also tracks
        /// and displays the last time this information was updated.  Note that because this
        /// method may be called on any thread, we must insure that all updates to the Windows
        /// forms controls are done on the thread that created the form.  If this is not the case,
        /// then we will reinvoke the call again on the proper thread - Windows Form oddity.
        /// </summary>
        /// <param name="list"></param>
        public void UpdateInfo(List<AgentPluginInfo> list)
        {
            if (InvokeRequired)
            {
                UpdateInfoCallback d = UpdateInfo;
                Invoke(d, new object[] { list });
            }
            else
            {
                var bFirstTime = (listView1.Items.Count == 0);
                labelHeader.Text = string.Format("{0} services loaded - information last updated on {1}", list.Count, DateTime.Now);
                list.Sort((p1, p2) => String.Compare(p2.PluginName, p1.PluginName, StringComparison.Ordinal));
                listView1.Items.Clear();
                foreach (var info in list)
                {
                    var item = new ListViewItem(info.PluginName);
                    item.SubItems.Add(info.PluginVersion);
                    item.SubItems.Add(info.PluginEnabled.ToString());
                    item.SubItems.Add(info.PluginState);
                    //item.SubItems.Add(info.PluginGuid.ToString());
                    item.SubItems.Add(info.PluginApiVersion);
                    item.SubItems.Add(info.PluginDescription);
                    listView1.Items.Add(item);
                }
                if (bFirstTime)
                    listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
            }
        }

    }
}
