/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This class implements a console panel plugin that plugs into the Setup navigation page.  
    /// </summary>
    [PluginInfo(Name = "SampleWizardPanelSummary", Version = "1.0", Id = "{153B08F4-A59C-4773-B0F5-30299CE38EFE}")]
    public partial class SampleWizardPanelSummary : SampleWizardPanelBase, IWizardPanelPlugin
    {
        #region Constructors

        /// <summary>
        /// Initialize the object in default state.
        /// </summary>
        public SampleWizardPanelSummary(IConsole console)
            : base(console)
        {
            InitializeComponent();
            HeaderText = "Wizard Panel 4";
            PromptText =
                "This is the summary or final panel where it is expected that you will allow the user to confirm that they want to apply their changes.";
        }

        #endregion

        /// <summary>
        /// Set the position of this panel in the wizard
        /// </summary>
        protected override int PluginNavPosition
        {
            get { return 900; }
        }

        /// <summary>
        /// Called when the page is activated.  We just enable the next button.  The base class will change the "Next" button
        /// into a "Apply" button for the final panel in the summary.
        /// </summary>
        public override void OnActivate()
        {
            if (WizardPage != null)
                WizardPage.EnableNextButton(this, true);
        }

    }
}
