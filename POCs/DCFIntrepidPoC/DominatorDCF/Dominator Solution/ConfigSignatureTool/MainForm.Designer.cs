namespace ConfigSignatureTool
{
    partial class MainForm
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
            this.label2 = new System.Windows.Forms.Label();
            this.btnOpenSourceXmlFile = new System.Windows.Forms.Button();
            this.textBoxSourceXmlFile = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxTargetXmlFile = new System.Windows.Forms.TextBox();
            this.textBoxVersion = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.listBoxPrivateKeySelect = new System.Windows.Forms.ListBox();
            this.btnUpdateXml = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.listBoxPublicKeySelect = new System.Windows.Forms.ListBox();
            this.btnCheckXml = new System.Windows.Forms.Button();
            this.btnGenerateNewXml = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Source File";
            // 
            // btnOpenSourceXmlFile
            // 
            this.btnOpenSourceXmlFile.Location = new System.Drawing.Point(573, 19);
            this.btnOpenSourceXmlFile.Name = "btnOpenSourceXmlFile";
            this.btnOpenSourceXmlFile.Size = new System.Drawing.Size(41, 23);
            this.btnOpenSourceXmlFile.TabIndex = 3;
            this.btnOpenSourceXmlFile.Text = "...";
            this.btnOpenSourceXmlFile.UseVisualStyleBackColor = true;
            this.btnOpenSourceXmlFile.Click += new System.EventHandler(this.btnOpenSourceXmlFile_Click);
            // 
            // textBoxSourceXmlFile
            // 
            this.textBoxSourceXmlFile.Location = new System.Drawing.Point(92, 21);
            this.textBoxSourceXmlFile.Name = "textBoxSourceXmlFile";
            this.textBoxSourceXmlFile.ReadOnly = true;
            this.textBoxSourceXmlFile.Size = new System.Drawing.Size(475, 20);
            this.textBoxSourceXmlFile.TabIndex = 2;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnOpenSourceXmlFile);
            this.groupBox1.Controls.Add(this.textBoxTargetXmlFile);
            this.groupBox1.Controls.Add(this.textBoxVersion);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.textBoxSourceXmlFile);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(620, 110);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Source File";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Update Version";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Target File";
            // 
            // textBoxTargetXmlFile
            // 
            this.textBoxTargetXmlFile.Location = new System.Drawing.Point(92, 47);
            this.textBoxTargetXmlFile.Name = "textBoxTargetXmlFile";
            this.textBoxTargetXmlFile.ReadOnly = true;
            this.textBoxTargetXmlFile.Size = new System.Drawing.Size(475, 20);
            this.textBoxTargetXmlFile.TabIndex = 5;
            // 
            // textBoxVersion
            // 
            this.textBoxVersion.Location = new System.Drawing.Point(92, 73);
            this.textBoxVersion.Name = "textBoxVersion";
            this.textBoxVersion.Size = new System.Drawing.Size(475, 20);
            this.textBoxVersion.TabIndex = 7;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 73);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(447, 20);
            this.textBox1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.listBoxPrivateKeySelect);
            this.groupBox2.Controls.Add(this.btnUpdateXml);
            this.groupBox2.Location = new System.Drawing.Point(12, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(303, 228);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Private Key Select";
            // 
            // listBoxPrivateKeySelect
            // 
            this.listBoxPrivateKeySelect.FormattingEnabled = true;
            this.listBoxPrivateKeySelect.Location = new System.Drawing.Point(6, 19);
            this.listBoxPrivateKeySelect.Name = "listBoxPrivateKeySelect";
            this.listBoxPrivateKeySelect.Size = new System.Drawing.Size(291, 147);
            this.listBoxPrivateKeySelect.TabIndex = 0;
            // 
            // btnUpdateXml
            // 
            this.btnUpdateXml.Location = new System.Drawing.Point(6, 172);
            this.btnUpdateXml.Name = "btnUpdateXml";
            this.btnUpdateXml.Size = new System.Drawing.Size(291, 50);
            this.btnUpdateXml.TabIndex = 1;
            this.btnUpdateXml.Text = "Signature File";
            this.btnUpdateXml.UseVisualStyleBackColor = true;
            this.btnUpdateXml.Click += new System.EventHandler(this.btnUpdateXml_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.listBoxPublicKeySelect);
            this.groupBox3.Controls.Add(this.btnCheckXml);
            this.groupBox3.Location = new System.Drawing.Point(329, 128);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(303, 228);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Public Key Select";
            // 
            // listBoxPublicKeySelect
            // 
            this.listBoxPublicKeySelect.FormattingEnabled = true;
            this.listBoxPublicKeySelect.Location = new System.Drawing.Point(6, 18);
            this.listBoxPublicKeySelect.Name = "listBoxPublicKeySelect";
            this.listBoxPublicKeySelect.Size = new System.Drawing.Size(291, 147);
            this.listBoxPublicKeySelect.TabIndex = 0;
            // 
            // btnCheckXml
            // 
            this.btnCheckXml.Location = new System.Drawing.Point(6, 172);
            this.btnCheckXml.Name = "btnCheckXml";
            this.btnCheckXml.Size = new System.Drawing.Size(291, 49);
            this.btnCheckXml.TabIndex = 1;
            this.btnCheckXml.Text = "Check File";
            this.btnCheckXml.UseVisualStyleBackColor = true;
            this.btnCheckXml.Click += new System.EventHandler(this.btnCheckXml_Click);
            // 
            // btnGenerateNewXml
            // 
            this.btnGenerateNewXml.Location = new System.Drawing.Point(12, 362);
            this.btnGenerateNewXml.Name = "btnGenerateNewXml";
            this.btnGenerateNewXml.Size = new System.Drawing.Size(620, 23);
            this.btnGenerateNewXml.TabIndex = 10;
            this.btnGenerateNewXml.Text = "Generate New Key And Signature File";
            this.btnGenerateNewXml.UseVisualStyleBackColor = true;
            this.btnGenerateNewXml.Click += new System.EventHandler(this.btnGenerateNewXml_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 388);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnGenerateNewXml);
            this.Controls.Add(this.groupBox3);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DFS Signature Tool";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOpenSourceXmlFile;
        private System.Windows.Forms.TextBox textBoxSourceXmlFile;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxTargetXmlFile;
        private System.Windows.Forms.TextBox textBoxVersion;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ListBox listBoxPrivateKeySelect;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnGenerateNewXml;
        private System.Windows.Forms.Button btnUpdateXml;
        private System.Windows.Forms.Button btnCheckXml;
        private System.Windows.Forms.ListBox listBoxPublicKeySelect;
    }
}

