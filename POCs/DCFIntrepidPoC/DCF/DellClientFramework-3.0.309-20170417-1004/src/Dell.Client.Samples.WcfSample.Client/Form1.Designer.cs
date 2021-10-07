namespace Dell.Client.Samples.WcfSample.Client
{
    partial class Form1
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
            this.viewPlugins1 = new Dell.Client.Samples.WcfSample.Client.ViewPlugins();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // viewPlugins1
            // 
            this.viewPlugins1.BackColor = System.Drawing.Color.Transparent;
            this.viewPlugins1.Location = new System.Drawing.Point(0, 0);
            this.viewPlugins1.Margin = new System.Windows.Forms.Padding(0);
            this.viewPlugins1.Name = "viewPlugins1";
            this.viewPlugins1.Size = new System.Drawing.Size(334, 344);
            this.viewPlugins1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(426, 156);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.OnCLick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(533, 384);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.viewPlugins1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OnFormClosing);
            this.Load += new System.EventHandler(this.OnLoad);
            this.ResumeLayout(false);

        }

        #endregion

        private ViewPlugins viewPlugins1;
        private System.Windows.Forms.Button button1;
    }
}

