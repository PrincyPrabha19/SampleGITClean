using Dell.Foundation.Services.Features.ConfigSelector;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace ConfigSignatureTool
{
    public partial class MainForm : Form
    {
        private string defaultOutput = string.Empty;   // output path
        private const string keyPath = "KeyPath";         // private key file path

        private string currentSourceXmlFile = string.Empty;
        private string outputTargetXmlFile = string.Empty;
        private Dictionary<string, string> keyMap = new Dictionary<string, string>();

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            defaultOutput = ConfigSignatureTool.Properties.Settings.Default.OutputPath;
            if (string.IsNullOrEmpty(defaultOutput))
            {
                defaultOutput = "Output";
            }
            string fullOutputPath = Path.Combine(Application.StartupPath, defaultOutput);
            if (!Directory.Exists(fullOutputPath))
            {
                Directory.CreateDirectory(fullOutputPath);
            }
            string keyPath = Path.Combine(Application.StartupPath, MainForm.keyPath);
            if (!Directory.Exists(keyPath))
            {
                Directory.CreateDirectory(keyPath);
            }
            else
            {
                // read exist private key
                DirectoryInfo keyPathInfo = new DirectoryInfo(keyPath);
                foreach (FileSystemInfo fsi in keyPathInfo.GetFileSystemInfos())
                {
                    FileInfo fi = (FileInfo)fsi;
                    if (Path.GetExtension(fi.FullName).ToLower().CompareTo(".rsa") == 0 && ConfigurationAuthentication.IsPrivateKeyFile(fi.FullName))
                    {
                        listBoxPrivateKeySelect.Items.Add(Path.GetFileName(fi.FullName));
                        keyMap.Add(Path.GetFileName(fi.FullName), fi.FullName);
                    }
                    if (Path.GetExtension(fi.FullName).ToLower().CompareTo(".snk") == 0)
                    {
                        listBoxPublicKeySelect.Items.Add(Path.GetFileName(fi.FullName));
                        keyMap.Add(Path.GetFileName(fi.FullName), fi.FullName);
                    }
                }
                if (listBoxPrivateKeySelect.Items.Count > 0)
                {
                    listBoxPrivateKeySelect.SelectedIndex = 0;
                }
                if (listBoxPublicKeySelect.Items.Count > 0)
                {
                    listBoxPublicKeySelect.SelectedIndex = 0;
                }
            }
        }

        private void btnOpenSourceXmlFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.InitialDirectory = Application.StartupPath;
            openFileDialog.Title = "Open source xml file";
            openFileDialog.Filter = "OC Predefined Profile (*.opp)|*.opp|OC Config File (*.occ)|*.occ|Config file (*.config)|*.config|Applications source xml file (*.xml)|*.xml";
            var result = openFileDialog.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                currentSourceXmlFile = openFileDialog.FileName;
                textBoxSourceXmlFile.Text = currentSourceXmlFile;

                outputTargetXmlFile = Path.Combine(Application.StartupPath, defaultOutput, Path.GetFileName(currentSourceXmlFile));
                textBoxTargetXmlFile.Text = outputTargetXmlFile;

                this.textBoxVersion.Text = ConfigurationAuthentication.GetVersion(currentSourceXmlFile);
            }
        }

        private bool CheckSourceXmlFile(string XmlFile)
        {
            if (!File.Exists(XmlFile))
            {
                MessageBox.Show("Please type source xml file.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                textBoxSourceXmlFile.Focus();
                return false;
            }

            return true;
        }

        private void btnUpdateXml_Click(object sender, EventArgs e)
        {
            string version = this.textBoxVersion.Text.Trim();

            if (!CheckSourceXmlFile(currentSourceXmlFile))
            {
                return;
            }

            string privateKeyFile = (string)listBoxPrivateKeySelect.SelectedItem;
            if (!string.IsNullOrEmpty(privateKeyFile))
            {
                privateKeyFile = keyMap[privateKeyFile];
                if (!File.Exists(privateKeyFile))
                {
                    MessageBox.Show("Please select an exist private key file.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    listBoxPrivateKeySelect.Focus();
                    return;
                }
                if (ConfigurationAuthentication.GenerateAuthentication(currentSourceXmlFile, outputTargetXmlFile, privateKeyFile, version) == 0)
                {
                    MessageBox.Show("xml update success.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
                else
                {
                    MessageBox.Show("Generate authentication failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Private key file invalidate.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
        }

        private void btnGenerateNewXml_Click(object sender, EventArgs e)
        {
            string version = this.textBoxVersion.Text.Trim();

            if (!CheckSourceXmlFile(currentSourceXmlFile))
            {
                return;
            }

            string newPrivateFileName = string.Empty;
            string privateKeyFile = string.Empty;
            do
            {
                newPrivateFileName = "Dell.Auth.Private." + DateTime.Now.ToString("yyyyMMddHHmmss") + ".rsa";
                privateKeyFile = Path.Combine(Application.StartupPath, MainForm.keyPath, newPrivateFileName);
            } while (keyMap.ContainsKey(newPrivateFileName));

            if (ConfigurationAuthentication.GenerateAuthentication(currentSourceXmlFile, outputTargetXmlFile, privateKeyFile, version) == 0)
            {
                listBoxPrivateKeySelect.SelectedIndex = listBoxPrivateKeySelect.Items.Add(Path.GetFileName(privateKeyFile));
                keyMap.Add(Path.GetFileName(privateKeyFile), privateKeyFile);

                string newPublicFileName = privateKeyFile.Replace("Private", "Public");
                newPublicFileName = Path.Combine(Path.GetDirectoryName(newPublicFileName), Path.GetFileNameWithoutExtension(newPublicFileName) + ".snk");
                listBoxPublicKeySelect.SelectedIndex = listBoxPublicKeySelect.Items.Add(Path.GetFileName(newPublicFileName));
                keyMap.Add(Path.GetFileName(newPublicFileName), newPublicFileName);

                MessageBox.Show("Authentication generate success.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
            else
            {
                MessageBox.Show("Generate authentication failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
            }
        }

        private void btnCheckXml_Click(object sender, EventArgs e)
        {
            string version = this.textBoxVersion.Text.Trim();

            if (!CheckSourceXmlFile(currentSourceXmlFile))
            {
                return;
            }

            string publicKeyFile = (string)listBoxPublicKeySelect.SelectedItem;
            if (!string.IsNullOrEmpty(publicKeyFile))
            {
                publicKeyFile = keyMap[publicKeyFile];
                if (!File.Exists(publicKeyFile))
                {
                    MessageBox.Show("Please select an exist public key file.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    listBoxPrivateKeySelect.Focus();
                    return;
                }

                try
                {
                    using (FileStream fs = new FileStream(publicKeyFile, FileMode.Open))
                    {
                        if (ConfigurationAuthentication.CheckAuthenticationFile(currentSourceXmlFile, fs))
                        {
                            MessageBox.Show("Configure file check success.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        }
                        else
                        {
                            MessageBox.Show("Check authentication failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                            return;
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Check authentication failed.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                }
            }
            else
            {
                MessageBox.Show("Key file invalidate.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }
        }

    }
}
