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
    /// This class implements the Welcome panel of the wizard.
    /// </summary>
    [PluginInfo(Name = "SampleWizardPanelPage1", Version = "1.0", Id = "{998F551B-F114-41FB-885D-268F081113EF}")]
    public partial class SampleWizardPanelPage1 : SampleWizardPanelBase, IWizardPanelPlugin
    {
        /// <summary>
        /// Initialize the object in default state.
        /// </summary>
        public SampleWizardPanelPage1(IConsole console)
            : base(console)
        {
            InitializeComponent();
            HeaderText = "Wizard Panel 1";
            PromptText = string.Empty;
            SetContextText(labelDescription);
            labelDescription.Text = "This is a sample welcome page in a wizard.  Hit 'next' to continue.";
        }

        /// <summary>
        /// Set the position of this panel in the wizard
        /// </summary>
        protected override int PluginNavPosition
        {
            get { return 100; }
        }

        /// <summary>
        /// Called when the page is activated.  We just enable the next button.
        /// </summary>
        public override void OnActivate()
        {
            if (WizardPage != null)
                WizardPage.EnableNextButton(this, true);
        }

    }
}
