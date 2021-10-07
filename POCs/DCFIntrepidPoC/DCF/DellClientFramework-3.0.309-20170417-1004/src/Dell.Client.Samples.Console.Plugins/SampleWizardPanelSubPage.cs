using System;
using System.Windows.Forms;
using Dell.Client.Framework.Common;
using Dell.Client.Framework.UXLib;
using Dell.Client.Framework.UXLib.Controls;

namespace Dell.Client.Samples.Console.Plugins
{
    public delegate void HideNextPageRequest(bool hide);

    [PluginInfo(Name = "SampleWizardPanelSubPage", Version = "1.0", Id = "{D4EF02BA-43DA-415D-8675-ECA6251B9DB8}")]
    public partial class SampleWizardPanelSubPage : SampleWizardPanelBase, IWizardPanelPlugin
    {
        protected override string ParentNavText { get { return "Wizard Panel 3"; } }
        protected override string ParentNavDesc { get { return ""; } }
        protected override int ParentNavPosition { get { return 300; } }

        public event HideNextPageRequest HideNextPage;
        
        public SampleWizardPanelSubPage(IConsole console)
            : base(console)
        {
            InitializeComponent();
            HeaderText = "Sub Page 1";
            labelDescription.Text = "This is sub page number 1.";
       }

        protected override int PluginNavPosition
        {
            get
            {
                return 201;
            }
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

        private void checkBoxHideNext_CheckedChanged(object sender, System.EventArgs e)
        {
            if (HideNextPage != null) 
                HideNextPage(checkBoxHideNext.Checked);
        }

        /// <summary>
        /// This method validates that the user should be able to proceed.
        /// </summary>
        /// <returns></returns>
        public override bool OnNextButtonClicked()
        {
            return cbEnableNext.Checked;
        }

        /// <summary>
        /// This method validates that the user should be able to return to the previous page.
        /// </summary>
        /// <returns></returns>
        public override bool OnBackButtonClicked()
        {
            return cbEnableNext.Checked;
        }
        
    }
}
