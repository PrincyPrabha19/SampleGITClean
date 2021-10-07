/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Console;
using Dell.Client.Framework.UXLib.Controls;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "StubPanels", Version = "1.0", Id = "{7A218F95-A37C-42FF-8A3B-874ED4BE75C0}")]
    public partial class SamplePanelPlugins : ConsolePanelLeftNavSettings, IConsolePanelPlugin
    {
        private IConsole Console;
        private string Filler = "Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum";

        public SamplePanelPlugins()
        {
            InitializeComponent();
            HeaderText = "Sample Panel Plugins";
            PromptText = Filler;
        }

        public SamplePanelPlugins(IConsole console)
            : this()
        {
            Console = console;
        }

        public string PagePluginId { get { return "SampleLeftNav1Page"; } }

        public void AssociatePage(IAcceptsPanelPlugins pagePlugin)
        {
            UXLeftNavItem navItem;
            pagePlugin.CreateNavItem(null, "NavItem 5-3", "", 503, this);
            pagePlugin.CreateNavItem(null, "NavItem 5-6", "", 506, this);
            pagePlugin.CreateNavItem(null, "NavItem 5-1", "", 501, this);
            pagePlugin.CreateNavItem(null, "NavItem 5-4", "", 504, this);
            pagePlugin.CreateNavItem(null, "NavItem 5-5", "", 505, this);
            pagePlugin.CreateNavItem(null, "NavItem 5-2", "", 502, this);
            pagePlugin.CreateNavItem(null, "NavItem 1", Filler.Substring(0, 42), 200, this);
            navItem = pagePlugin.CreateNavItem(null, "NavItem 4", "This is the description field", 500, null);
            pagePlugin.CreateNavItem(null, "NavItem 2", Filler.Substring(0, 42), 300, this);
            pagePlugin.CreateNavItem(navItem, "SubItem 4.3", "", 30, this);
            pagePlugin.CreateNavItem(navItem, "SubItem 4.2", "", 20, this);
            pagePlugin.CreateNavItem(navItem, "SubItem 4.1", "", 10, this);
            pagePlugin.CreateNavItem(null, "NavItem 3", Filler.Substring(0, 42), 400, this);
            if (pagePlugin is IConsolePolicyPagePlugin)
                PagePlugin = (IConsolePolicyPagePlugin)pagePlugin;
        }

        public override void OnActivate()
        {
            if (PagePlugin != null)
            {
                PagePlugin.HideApplyButton(this);
                PagePlugin.HideDefaultsButton(this);
            }
        }

    }
}
