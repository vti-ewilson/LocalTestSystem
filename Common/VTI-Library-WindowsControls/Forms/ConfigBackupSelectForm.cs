using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Configuration;

namespace VTIWindowsControlLibrary.Forms
{
    public partial class ConfigBackupSelectForm : Form
    {
        List<FileInfo> goodFileListBackupFolder;
        List<FileInfo> goodFileListUserBackupFolder;
        AllUsersSettingsProvider _allUsersSettingsProvider = new AllUsersSettingsProvider();
        public ConfigBackupSelectForm()
        {
            InitializeComponent();
        }

        private void ConfigBackupSelectForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                goodFileListBackupFolder = new List<FileInfo>();
                DirectoryInfo d = new DirectoryInfo(_allUsersSettingsProvider.GetSettingsPath() + @"\Backup\");
                if (d.Exists)
                {
                    List<FileInfo> allConfigFilesBackupFolder = d.GetFiles().Where(x => x.Extension == ".config").ToList();
                    //get only the uncorrupted backup files
                    foreach (FileInfo file in allConfigFilesBackupFolder)
                    {
                        try
                        {
                            //try to load the config file
                            XmlDocument doc = new XmlDocument();
                            doc.Load(file.FullName);
                            XmlNodeList editCycleParamList = doc.GetElementsByTagName("DisplayName");
                            //if it loaded successfully, add it to the good config files list
                            goodFileListBackupFolder.Add(file);
                        }
                        catch (Exception ex)
                        { }
                    }
                }

                // Automatically use most recent backup file if any backups are present
                if(goodFileListBackupFolder.Count > 0) {
                    var backupFiles = goodFileListBackupFolder.OrderByDescending(o => o.LastWriteTime).ToList();
					FileInfo existingFile = new FileInfo(_allUsersSettingsProvider.GetSettingsPathAndFilename());
					existingFile.Delete();
                    //move the selected backup config file out of the Backup folder and rename it
                    backupFiles[0].CopyTo(existingFile.FullName);
					this.Hide();
				}

                //add good backup config files sorted by LastWriteTime to BackupFilesListBox
                BackupFilesListBox.Items.AddRange(goodFileListBackupFolder.OrderByDescending(x => x.LastWriteTime).Select(x => x.LastWriteTime.ToString()).ToArray());

                //get list of good .config files from UserBackup folder
                DirectoryInfo dG = new DirectoryInfo(_allUsersSettingsProvider.GetSettingsPath() + @"\UserBackup\");
                goodFileListUserBackupFolder = new List<FileInfo>();
                if (dG.Exists)
                {
                    List<FileInfo> allConfigFilesUserBackupFolder = dG.GetFiles().Where(x => x.Extension.Equals(".config", StringComparison.InvariantCultureIgnoreCase)).ToList();
                    //get only the uncorrupted files
                    foreach (FileInfo file in allConfigFilesUserBackupFolder)
                    {
                        try
                        {
                            //try to load the config file
                            XmlDocument doc = new XmlDocument();
                            doc.Load(file.FullName);
                            XmlNodeList editCycleParamList = doc.GetElementsByTagName("DisplayName");
                            //if it loaded successfully, add it to the good config files list
                            goodFileListUserBackupFolder.Add(file);
                        }
                        catch (Exception ex)
                        { }
                    }
                }
                else
                {
                    Directory.CreateDirectory(dG.FullName);
                }
                //add good backup config files sorted by LastWriteTime to configGoodFilesListBox
                foreach (FileInfo goodFile in goodFileListUserBackupFolder.OrderByDescending(x => x.LastWriteTime))
                {
                    configGoodFilesListBox.Items.Add(goodFile.Name + " (" + goodFile.LastWriteTime + ")");
                }
                if (BackupFilesListBox.Items.Count > 0)
                {
                    BackupFilesListBox.SetItemChecked(0, true);
                }
                else if (configGoodFilesListBox.Items.Count > 0)
                {
                    configGoodFilesListBox.SetItemChecked(0, true);
                }
            }
        }

        private void OKButton_Click(object sender, EventArgs e)
        {
            if (BackupFilesListBox.CheckedItems.Count == 0 && configGoodFilesListBox.CheckedItems.Count == 0)
            {
                if (MessageBox.Show("No backup file was selected. New config file with factory default values will be used.", "No File Selected", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    VtiEvent.Log.WriteInfo("No backup file was selected. New config file with factory default values will be used.");
                    //delete existing config file if it exists
                    FileInfo existingFile = new FileInfo(_allUsersSettingsProvider.GetSettingsPathAndFilename());
                    existingFile.Delete();
                    this.Hide();
                }
            }
            else if (BackupFilesListBox.CheckedItems.Count > 0 && configGoodFilesListBox.CheckedItems.Count == 0)
            {
                var checkedItem = BackupFilesListBox.CheckedItems[0].ToString();
                FileInfo selectedBackupFile = goodFileListBackupFolder.Where(x => x.LastWriteTime.ToString() == checkedItem).FirstOrDefault();
                //delete existing config file if it exists
                FileInfo existingFile = new FileInfo(_allUsersSettingsProvider.GetSettingsPathAndFilename());
                existingFile.Delete();
                //move the selected backup config file out of the Backup folder and rename it
                selectedBackupFile.CopyTo(existingFile.FullName);
                this.Hide();
            }
            else if (BackupFilesListBox.CheckedItems.Count == 0 && configGoodFilesListBox.CheckedItems.Count > 0)
            {
                var checkedItem = configGoodFilesListBox.CheckedItems[0].ToString();
                int indexOfFirstDateTimeChar = checkedItem.IndexOf('(') + 1;
                string parsedDateTime = checkedItem.Substring(indexOfFirstDateTimeChar, checkedItem.Length - indexOfFirstDateTimeChar - 1);
                FileInfo selectedConfigGoodFile = goodFileListUserBackupFolder.Where(x => x.LastWriteTime.ToString() == parsedDateTime).FirstOrDefault();
                //delete existing config file if it exists
                FileInfo existingFile = new FileInfo(_allUsersSettingsProvider.GetSettingsPathAndFilename());
                existingFile.Delete();
                //move the selected backup config file out of the Backup folder and rename it
                selectedConfigGoodFile.CopyTo(existingFile.FullName);
                this.Hide();
            }
            else
            {
                MessageBox.Show("Cannot select items from both list boxes. Please choose one.");
            }
        }

        private void BackupFilesListBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            for (int i = 0; i < BackupFilesListBox.Items.Count; ++i)
            {
                if (i != e.Index) BackupFilesListBox.SetItemChecked(i, false);
            }
        }
    }
}
