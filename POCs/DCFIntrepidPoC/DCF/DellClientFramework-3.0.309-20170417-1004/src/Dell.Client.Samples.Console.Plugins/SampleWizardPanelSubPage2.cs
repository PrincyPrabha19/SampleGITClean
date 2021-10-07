/*
* ©Copyright 2014 Dell, Inc., All Rights Reserved.
* This material is confidential and a trade secret.  Permission to use this
* work for any purpose must be obtained in writing from Dell, Inc.
*/

using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.Common;

namespace Dell.Client.Samples.Console.Plugins
{
    [PluginInfo(Name = "SampleWizardPanelSubPage2", Version = "1.0", Id = "{898A93DB-9987-4ED4-8F7F-D713AECDEF3C}")]
    public partial class SampleWizardPanelSubPage2 : SampleWizardPanelBase, IWizardPanelPlugin
    {
        protected override string ParentNavText { get { return "Wizard Panel 3"; } }
        protected override string ParentNavDesc { get { return ""; } }
        protected override int ParentNavPosition { get { return 300; } }
        protected override int PluginNavPosition { get { return 202; } }

        public SampleWizardPanelSubPage2(IConsole console)
            : base(console)
        {
            InitializeComponent();
            HeaderText = "Sub Page 2";
            labelDescription.Text = "This is sub page number 2.";
        }

        public override void OnActivate()
        {
            UpdateControls();
        }

        /// <summary>
        /// This method is called for each panel when the wizard is to be applied.  Each panel can then perform their
        /// specific task.
        /// </summary>
        /// <returns></returns>
        public override bool OnPerformWizard(ref string summaryInfo, ref string errorInfo)
        {
            return true;
        }

        /*private void OnCheckChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }*/

        private void UpdateControls()
        {
            if (WizardPage != null)
                WizardPage.EnableNextButton(this, true);
        }

    }
}
