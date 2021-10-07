namespace MacroKeysSimulator
{
    partial class mainForm
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
            this.cmdMode = new System.Windows.Forms.Button();
            this.buttonM1 = new System.Windows.Forms.Button();
            this.buttonM2 = new System.Windows.Forms.Button();
            this.buttonM3 = new System.Windows.Forms.Button();
            this.buttonM4 = new System.Windows.Forms.Button();
            this.buttonM5 = new System.Windows.Forms.Button();
            this.buttonMD = new System.Windows.Forms.Button();
            this.buttonMC = new System.Windows.Forms.Button();
            this.buttonMB = new System.Windows.Forms.Button();
            this.buttonMA = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmdMode
            // 
            this.cmdMode.Location = new System.Drawing.Point(101, 71);
            this.cmdMode.Name = "cmdMode";
            this.cmdMode.Size = new System.Drawing.Size(160, 80);
            this.cmdMode.TabIndex = 0;
            this.cmdMode.Tag = "0x11";
            this.cmdMode.Text = "Mode";
            this.cmdMode.UseVisualStyleBackColor = true;
            this.cmdMode.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonM1
            // 
            this.buttonM1.Location = new System.Drawing.Point(101, 184);
            this.buttonM1.Name = "buttonM1";
            this.buttonM1.Size = new System.Drawing.Size(160, 80);
            this.buttonM1.TabIndex = 1;
            this.buttonM1.Tag = "0x12";
            this.buttonM1.Text = "M1";
            this.buttonM1.UseVisualStyleBackColor = true;
            this.buttonM1.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonM2
            // 
            this.buttonM2.Location = new System.Drawing.Point(101, 297);
            this.buttonM2.Name = "buttonM2";
            this.buttonM2.Size = new System.Drawing.Size(160, 80);
            this.buttonM2.TabIndex = 2;
            this.buttonM2.Tag = "0x13";
            this.buttonM2.Text = "M2";
            this.buttonM2.UseVisualStyleBackColor = true;
            this.buttonM2.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonM3
            // 
            this.buttonM3.Location = new System.Drawing.Point(101, 410);
            this.buttonM3.Name = "buttonM3";
            this.buttonM3.Size = new System.Drawing.Size(160, 80);
            this.buttonM3.TabIndex = 3;
            this.buttonM3.Tag = "0x14";
            this.buttonM3.Text = "M3";
            this.buttonM3.UseVisualStyleBackColor = true;
            this.buttonM3.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonM4
            // 
            this.buttonM4.Location = new System.Drawing.Point(101, 523);
            this.buttonM4.Name = "buttonM4";
            this.buttonM4.Size = new System.Drawing.Size(160, 80);
            this.buttonM4.TabIndex = 4;
            this.buttonM4.Tag = "0x15";
            this.buttonM4.Text = "M4";
            this.buttonM4.UseVisualStyleBackColor = true;
            this.buttonM4.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonM5
            // 
            this.buttonM5.Location = new System.Drawing.Point(101, 636);
            this.buttonM5.Name = "buttonM5";
            this.buttonM5.Size = new System.Drawing.Size(160, 80);
            this.buttonM5.TabIndex = 5;
            this.buttonM5.Tag = "0x16";
            this.buttonM5.Text = "M5";
            this.buttonM5.UseVisualStyleBackColor = true;
            this.buttonM5.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonMD
            // 
            this.buttonMD.Location = new System.Drawing.Point(963, 71);
            this.buttonMD.Name = "buttonMD";
            this.buttonMD.Size = new System.Drawing.Size(160, 80);
            this.buttonMD.TabIndex = 9;
            this.buttonMD.Tag = "0x1B";
            this.buttonMD.Text = "MD";
            this.buttonMD.UseVisualStyleBackColor = true;
            this.buttonMD.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonMC
            // 
            this.buttonMC.Location = new System.Drawing.Point(781, 71);
            this.buttonMC.Name = "buttonMC";
            this.buttonMC.Size = new System.Drawing.Size(160, 80);
            this.buttonMC.TabIndex = 8;
            this.buttonMC.Tag = "0x1A";
            this.buttonMC.Text = "MC";
            this.buttonMC.UseVisualStyleBackColor = true;
            this.buttonMC.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonMB
            // 
            this.buttonMB.Location = new System.Drawing.Point(599, 71);
            this.buttonMB.Name = "buttonMB";
            this.buttonMB.Size = new System.Drawing.Size(160, 80);
            this.buttonMB.TabIndex = 7;
            this.buttonMB.Tag = "0x18";
            this.buttonMB.Text = "MB";
            this.buttonMB.UseVisualStyleBackColor = true;
            this.buttonMB.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // buttonMA
            // 
            this.buttonMA.Location = new System.Drawing.Point(417, 71);
            this.buttonMA.Name = "buttonMA";
            this.buttonMA.Size = new System.Drawing.Size(160, 80);
            this.buttonMA.TabIndex = 6;
            this.buttonMA.Tag = "0x17";
            this.buttonMA.Text = "MA";
            this.buttonMA.UseVisualStyleBackColor = true;
            this.buttonMA.Click += new System.EventHandler(this.macroKeyPress);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1225, 786);
            this.Controls.Add(this.buttonMD);
            this.Controls.Add(this.buttonMC);
            this.Controls.Add(this.buttonMB);
            this.Controls.Add(this.buttonMA);
            this.Controls.Add(this.buttonM5);
            this.Controls.Add(this.buttonM4);
            this.Controls.Add(this.buttonM3);
            this.Controls.Add(this.buttonM2);
            this.Controls.Add(this.buttonM1);
            this.Controls.Add(this.cmdMode);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "mainForm";
            this.Text = "Scan Code Simulator";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdMode;
        private System.Windows.Forms.Button buttonM1;
        private System.Windows.Forms.Button buttonM2;
        private System.Windows.Forms.Button buttonM3;
        private System.Windows.Forms.Button buttonM4;
        private System.Windows.Forms.Button buttonM5;
        private System.Windows.Forms.Button buttonMD;
        private System.Windows.Forms.Button buttonMC;
        private System.Windows.Forms.Button buttonMB;
        private System.Windows.Forms.Button buttonMA;
    }
}

