using System;
using System.IO;
using System.Windows.Forms;

namespace AlienLabs.AlienAdrenaline.App.Classes
{
    public class FileOperationsClass : FileOperations
    {
        private const string ALLFILES_EXTENSION = "*.*";

        public string GetFilePath()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = String.Format("{0}|{1}", String.Format(Properties.Resources.AllFilesFilterText, ALLFILES_EXTENSION), ALLFILES_EXTENSION),
                RestoreDirectory = true
            };

            DialogResult result = fileDialog.ShowDialog();
            if (result == DialogResult.OK)
                return fileDialog.FileName;

            return String.Empty;
        }

        //public bool IsValidFilePath(string path)
        //{
        //    return File.Exists(path);
        //}
    }
}