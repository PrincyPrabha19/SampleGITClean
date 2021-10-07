/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using System.Windows.Forms;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Console;
using Dell.Client.Framework.UXLib.Controls;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This class implements a base class for all panels 
    /// </summary>
    public partial class SampleWizardPanelBase : WizardPanel
    {
        protected virtual int PluginNavPosition { get; set; }

        protected virtual string ParentNavText { get; set; }
        protected virtual string ParentNavDesc { get; set; }
        protected virtual int ParentNavPosition { get; set; }

        protected readonly IConsole Console;
        protected IWizardPagePlugin WizardPage;
        protected UXLeftNavItem NavItem = null;

        #region Constructors

        protected SampleWizardPanelBase()
        {
            InitializeComponent();
        }

        protected SampleWizardPanelBase(IConsole console)
            : this()
        {
            Console = console;
            Console.PluginsLoadedEvent += OnPluginsLoaded;
        }

        #endregion

        #region IWizardPanelPlugin

        public string PagePluginId
        {
            get { return "samplewizard"; }
        }

        public void AssociatePage(IAcceptsPanelPlugins pagePlugin)
        {
            UXLeftNavItem parentItem = null;
            if (!string.IsNullOrEmpty(ParentNavText))
            {
                parentItem = pagePlugin.FindNavItem(ParentNavText);
                if (parentItem == null)
                    parentItem = pagePlugin.CreateNavItem(null, ParentNavText, ParentNavDesc, ParentNavPosition, null);
            }
            pagePlugin.CreateNavItem(parentItem, HeaderText, string.Empty, PluginNavPosition, this);
            if (pagePlugin is IWizardPagePlugin)
                WizardPage = (IWizardPagePlugin)pagePlugin;
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method sets the correct font and text color for the given content control.
        /// </summary>
        /// <param name="ctl"></param>
        protected void SetContextTextStyles(Control ctl)
        {
            ctl.Font = UXStyleGuides.ConsolePanelContentFont;
            ctl.ForeColor = UXStyleGuides.ConsolePanelContentColor;
        }

        #endregion

        private void OnPluginsLoaded(object sender, EventArgs e)
        {
            Console.PluginsLoadedEvent -= OnPluginsLoaded;
            //
            // This is the place where a panel might choose to unload itself
            //
        }
    }
}
