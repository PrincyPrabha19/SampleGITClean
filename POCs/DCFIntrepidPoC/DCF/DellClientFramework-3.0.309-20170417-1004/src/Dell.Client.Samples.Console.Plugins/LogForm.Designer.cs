namespace Dell.Client.Samples.Console.Plugins
{
    partial class LogForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ctlLog = new LogControl();
            this.SuspendLayout();
            // 
            // ctlLog
            // 
            this.ctlLog.BackColor = System.Drawing.Color.Transparent;
            this.ctlLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlLog.Location = new System.Drawing.Point(0, 45);
            this.ctlLog.Margin = new System.Windows.Forms.Padding(4);
            this.ctlLog.Name = "ctlLog";
            this.ctlLog.Padding = new System.Windows.Forms.Padding(20, 20, 20, 0);
            this.ctlLog.Size = new System.Drawing.Size(539, 250);
            this.ctlLog.TabIndex = 1;
            // 
            // LogForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderPadding = new System.Windows.Forms.Padding(0);
            this.ClientSize = new System.Drawing.Size(539, 315);
            this.Controls.Add(this.ctlLog);
            this.Name = "LogForm";
            this.Padding = new System.Windows.Forms.Padding(0, 0, 0, 20);
            this.Text = "LogForm";
            this.Controls.SetChildIndex(this.ctlLog, 0);
            this.ResumeLayout(false);

        }

        #endregion

        private LogControl ctlLog;
    }
}