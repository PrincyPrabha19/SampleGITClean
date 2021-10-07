/*
 * ©Copyright 2014 Dell, Inc., All Rights Reserved.
 * This material is confidential and a trade secret.  Permission to use this
 * work for any purpose must be obtained in writing from Dell, Inc.
 */

using System;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Controls;

namespace Dell.Client.Samples.Console.Plugins
{
    /// <summary>
    /// This class implements sample wizard panel.
    /// </summary>
    [PluginInfo(Name = "SampleWizardPanelPage2", Version = "1.0", Id = "{D4EF02BA-43DA-415D-8675-ECA6251B9DB7}")]
    public partial class SampleWizardPanelPage2 : SampleWizardPanelBase, IWizardPanelPlugin
    {
        #region Constructors

        /// <summary>
        /// Initialize the object in default state.
        /// </summary>
        public SampleWizardPanelPage2(IConsole console)
            : base(console)
        {
            InitializeComponent();
            HeaderText = "Wizard Panel 2";
            labelDescription.Text = "This is the sample panel 2 text.  Check the Radio button must be checked to proceed";
        }

        #endregion

        /// <summary>
        /// Set the position of this panel in the wizard
        /// </summary>
        protected override int PluginNavPosition
        {
            get { return 200; }
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

        private void OnCheckChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

        private void UpdateControls()
        {
            if (WizardPage != null)
                WizardPage.EnableNextButton(this, checkBoxNext.Checked);
        }

    }
}
