namespace Dell.Client.Samples.Console.Plugins
{
    partial class SampleWizardPanelSubPage
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.labelDescription = new Dell.Client.Framework.UXLib.Controls.UXLabel();
            this.checkBoxHideNext = new Dell.Client.Framework.UXLib.Controls.UXCheckBox();
            this.cbEnableNext = new Dell.Client.Framework.UXLib.Controls.UXCheckBox();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.cbEnableNext);
            this.panelControls.Controls.Add(this.checkBoxHideNext);
            this.panelControls.Controls.Add(this.labelDescription);
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(0, 8);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(581, 62);
            this.labelDescription.TabIndex = 2;
            this.labelDescription.Text = "uxLabel1";
            this.labelDescription.UseCompatibleTextRendering = true;
            // 
            // checkBoxHideNext
            // 
            this.checkBoxHideNext.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxHideNext.Location = new System.Drawing.Point(22, 46);
            this.checkBoxHideNext.Name = "checkBoxHideNext";
            this.checkBoxHideNext.Size = new System.Drawing.Size(169, 24);
            this.checkBoxHideNext.TabIndex = 4;
            this.checkBoxHideNext.Text = "Hide the next subpage!";
            this.checkBoxHideNext.CheckedChanged += new System.EventHandler(this.checkBoxHideNext_CheckedChanged);
            // 
            // cbEnableNext
            // 
            this.cbEnableNext.BackColor = System.Drawing.Color.Transparent;
            this.cbEnableNext.Location = new System.Drawing.Point(22, 76);
            this.cbEnableNext.Name = "cbEnableNext";
            this.cbEnableNext.Size = new System.Drawing.Size(272, 24);
            this.cbEnableNext.TabIndex = 5;
            this.cbEnableNext.Text = "Check to validate Next and Back buttons!";
            // 
            // SampleWizardPanelSubPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "SampleWizardPanelSubPage";
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UXLib.Controls.UXLabel labelDescription;
        private Framework.UXLib.Controls.UXCheckBox cbEnableNext;
        private Framework.UXLib.Controls.UXCheckBox checkBoxHideNext;
    }
}
