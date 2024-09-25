using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Classes.Util
{
    public static class SaveConfigGoodFile
    {
        static AllUsersSettingsProvider _allUsersSettingsProvider = new AllUsersSettingsProvider();
        static DirectoryInfo d = new DirectoryInfo(_allUsersSettingsProvider.GetSettingsPath() + @"\UserBackup");
        static SaveFileDialog sfd;

        public static void Save()
        {
            MessageBox.Show("Press OK to save the current .config file to be available as a backup. " +
                "This files contains all of the Common Control/Mode/Pressure/Time/Flow Parameters as well as the Default Model parameters. This will not save custom model parameters.");
            if (!d.Exists)
            {
                Directory.CreateDirectory(d.FullName);
            }
            sfd = new SaveFileDialog();
            sfd.FileOk += Sfd_FileOk;
            sfd.Filter = "config File|*.config";
            sfd.Title = "Save .config File";
            sfd.FileName = DateTime.Now.ToShortDateString().Replace(@"/", "-");
            sfd.InitialDirectory = d.FullName;
            sfd.RestoreDirectory = true;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                if (sfd.FileName != "")
                {
                    var temp = _allUsersSettingsProvider.GetSettingsPathAndFilename();
                    File.WriteAllText(sfd.FileName, File.ReadAllText(_allUsersSettingsProvider.GetSettingsPathAndFilename()));
                }
            }
        }

        private static void Sfd_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (Path.GetDirectoryName(sfd.FileName) != d.FullName)
            {
                MessageBox.Show("You must save to the UserBackup directory.");
                e.Cancel = true;
            }
        }
    }
}
