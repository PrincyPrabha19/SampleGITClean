using System;
using System.IO;
using System.Windows.Forms;

namespace AlienLabs.AlienAdrenaline.App.Classes
{
    public class FolderOperationsClass : FolderOperations
    {
        public string GetSelectedPath(string defaultShotcutPath = null)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (!String.IsNullOrEmpty(defaultShotcutPath) && IsValidFolderPath(defaultShotcutPath))
                folderBrowserDialog.SelectedPath = defaultShotcutPath;

            DialogResult result = folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                return folderBrowserDialog.SelectedPath;

            return String.Empty;
        }

        public bool IsValidFolderPath(string path)
        {
            return Directory.Exists(path);
        }
    }
}