namespace KeyboardEventsWatcher
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
			this.logTXT = new System.Windows.Forms.TextBox();
			this.buttonClear = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// logTXT
			// 
			this.logTXT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.logTXT.Location = new System.Drawing.Point(0, 0);
			this.logTXT.Multiline = true;
			this.logTXT.Name = "logTXT";
			this.logTXT.ReadOnly = true;
			this.logTXT.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.logTXT.Size = new System.Drawing.Size(517, 317);
			this.logTXT.TabIndex = 0;
			this.logTXT.WordWrap = false;
			// 
			// buttonClear
			// 
			this.buttonClear.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.buttonClear.Location = new System.Drawing.Point(0, 323);
			this.buttonClear.Name = "buttonClear";
			this.buttonClear.Size = new System.Drawing.Size(517, 23);
			this.buttonClear.TabIndex = 1;
			this.buttonClear.Text = "Clear";
			this.buttonClear.UseVisualStyleBackColor = true;
			this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(517, 346);
			this.Controls.Add(this.buttonClear);
			this.Controls.Add(this.logTXT);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox logTXT;
		private System.Windows.Forms.Button buttonClear;
	}
}

