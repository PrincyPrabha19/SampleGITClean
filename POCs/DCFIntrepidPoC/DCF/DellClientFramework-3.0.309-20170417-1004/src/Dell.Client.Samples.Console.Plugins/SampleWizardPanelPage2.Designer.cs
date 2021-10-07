namespace Dell.Client.Samples.Console.Plugins
{
    partial class SampleWizardPanelPage2
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
            this.checkBoxNext = new Dell.Client.Framework.UXLib.Controls.UXCheckBox();
            this.labelDescription = new Dell.Client.Framework.UXLib.Controls.UXLabel();
            this.panelControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelControls
            // 
            this.panelControls.Controls.Add(this.labelDescription);
            this.panelControls.Controls.Add(this.checkBoxNext);
            this.panelControls.Margin = new System.Windows.Forms.Padding(5);
            this.panelControls.Size = new System.Drawing.Size(600, 328);
            // 
            // checkBoxNext
            // 
            this.checkBoxNext.BackColor = System.Drawing.Color.Transparent;
            this.checkBoxNext.Location = new System.Drawing.Point(17, 102);
            this.checkBoxNext.Name = "checkBoxNext";
            this.checkBoxNext.Size = new System.Drawing.Size(190, 24);
            this.checkBoxNext.TabIndex = 0;
            this.checkBoxNext.Text = "Check this to enable Next";
            this.checkBoxNext.CheckedChanged += new System.EventHandler(this.OnCheckChanged);
            // 
            // labelDescription
            // 
            this.labelDescription.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelDescription.Location = new System.Drawing.Point(0, 8);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(600, 62);
            this.labelDescription.TabIndex = 1;
            this.labelDescription.Text = "uxLabel1";
            this.labelDescription.UseCompatibleTextRendering = true;
            // 
            // SampleWizardPanelPage2
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "SampleWizardPanelPage2";
            this.Size = new System.Drawing.Size(600, 380);
            this.panelControls.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Framework.UXLib.Controls.UXLabel labelDescription;
        private Framework.UXLib.Controls.UXCheckBox checkBoxNext;


    }
}
