namespace Dell.Client.Samples.Console.Plugins
{
    partial class LogControl
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
            this.components = new System.ComponentModel.Container();
            this.ListBox = new System.Windows.Forms.ListBox();
            this.contextLogMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemClearLog = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemCopyToClipboard = new System.Windows.Forms.ToolStripMenuItem();
            this.panelBkgnd = new System.Windows.Forms.Panel();
            this.contextLogMenuStrip1.SuspendLayout();
            this.panelBkgnd.SuspendLayout();
            this.SuspendLayout();
            // 
            // ListBox
            // 
            this.ListBox.BackColor = System.Drawing.Color.White;
            this.ListBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ListBox.ContextMenuStrip = this.contextLogMenuStrip1;
            this.ListBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ListBox.Font = new System.Drawing.Font("Courier New", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ListBox.FormattingEnabled = true;
            this.ListBox.HorizontalScrollbar = true;
            this.ListBox.ItemHeight = 12;
            this.ListBox.Location = new System.Drawing.Point(0, 0);
            this.ListBox.Margin = new System.Windows.Forms.Padding(0);
            this.ListBox.Name = "ListBox";
            this.ListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.ListBox.Size = new System.Drawing.Size(991, 466);
            this.ListBox.TabIndex = 3;
            // 
            // contextLogMenuStrip1
            // 
            this.contextLogMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemClearLog,
            this.menuItemCopyToClipboard});
            this.contextLogMenuStrip1.Name = "contextLogMenuStrip1";
            this.contextLogMenuStrip1.Size = new System.Drawing.Size(240, 48);
            // 
            // menuItemClearLog
            // 
            this.menuItemClearLog.Name = "menuItemClearLog";
            this.menuItemClearLog.Size = new System.Drawing.Size(239, 22);
            this.menuItemClearLog.Text = "<menuItemClearLog>";
            this.menuItemClearLog.Click += new System.EventHandler(this.OnClearLogClick);
            // 
            // menuItemCopyToClipboard
            // 
            this.menuItemCopyToClipboard.Name = "menuItemCopyToClipboard";
            this.menuItemCopyToClipboard.Size = new System.Drawing.Size(239, 22);
            this.menuItemCopyToClipboard.Text = "<menuItemCopyToClipboard>";
            this.menuItemCopyToClipboard.Click += new System.EventHandler(this.OnCopyToClipboardClick);
            // 
            // panelBkgnd
            // 
            this.panelBkgnd.BackColor = System.Drawing.Color.Transparent;
            this.panelBkgnd.Controls.Add(this.ListBox);
            this.panelBkgnd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBkgnd.Location = new System.Drawing.Point(0, 0);
            this.panelBkgnd.Margin = new System.Windows.Forms.Padding(4);
            this.panelBkgnd.Name = "panelBkgnd";
            this.panelBkgnd.Size = new System.Drawing.Size(991, 466);
            this.panelBkgnd.TabIndex = 4;
            // 
            // LogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.panelBkgnd);
            this.DoubleBuffered = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "LogControl";
            this.Size = new System.Drawing.Size(991, 466);
            this.contextLogMenuStrip1.ResumeLayout(false);
            this.panelBkgnd.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox ListBox;
        private System.Windows.Forms.ContextMenuStrip contextLogMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuItemClearLog;
        private System.Windows.Forms.Panel panelBkgnd;
        private System.Windows.Forms.ToolStripMenuItem menuItemCopyToClipboard;
    }
}
