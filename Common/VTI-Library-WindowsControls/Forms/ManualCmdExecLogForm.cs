using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Data.Linq.SqlClient;
using VTIWindowsControlLibrary.Data;
using System.Text;
using System.IO;

namespace VTIWindowsControlLibrary.Forms
{
    public partial class ManualCmdExecLogForm : Form
    {
        private VtiDataContext db;
        public ManualCmdExecLogForm()
        {
            InitializeComponent();
        }

        private void FilterRecords()
        {
            ManualCmdExecLogDataGridView.DataSource =
                from cmdRecord in db.ManualCmdExecLogs
                where
                    cmdRecord.DateTime.Date >= dateTimePickerStart.Value.Date &&
                    cmdRecord.DateTime.Date <= dateTimePickerEnd.Value.Date &&

                    (SqlMethods.Like(cmdRecord.OpID, operatorFilterTextBox.Text) ||
                      String.IsNullOrEmpty(operatorFilterTextBox.Text) ||
                      cmdRecord.OpID.Contains(operatorFilterTextBox.Text)) &&

                    (SqlMethods.Like(cmdRecord.SystemID, systemIDFilterTextBox.Text) ||
                      String.IsNullOrEmpty(systemIDFilterTextBox.Text) ||
                      cmdRecord.SystemID.Contains(systemIDFilterTextBox.Text)) &&

                      (SqlMethods.Like(cmdRecord.OverrideOpID, overrideOpIDFilterTextBox.Text) ||
                      String.IsNullOrEmpty(overrideOpIDFilterTextBox.Text) ||
                      cmdRecord.OverrideOpID.Contains(overrideOpIDFilterTextBox.Text)) &&

                (SqlMethods.Like(cmdRecord.ManualCommand, ManualCmdFilterTextBox.Text) ||
                  String.IsNullOrEmpty(ManualCmdFilterTextBox.Text) ||
                  cmdRecord.ManualCommand.Contains(ManualCmdFilterTextBox.Text))

                select cmdRecord;

            if (ManualCmdExecLogDataGridView.Rows.Count > 0)
            {
                //sort by DateTime desc
                this.ManualCmdExecLogDataGridView.Sort(this.ManualCmdExecLogDataGridView.Columns[1], ListSortDirection.Descending);
            }
            ResizeFormToShowEntireWidthOfDGV();
        }

        private void ManualCmdExecLogForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (Properties.Settings.Default.UsesVtiDataDatabase)
                {
                    if (useRemoteDBCheckBox.Checked)
                    {
                        db = new VtiDataContext(VtiLib.ConnectionString2);
                    }
                    else
                    {
                        db = new VtiDataContext(VtiLib.ConnectionString);
                    }

                    dateTimePickerStart.Value = System.DateTime.Now.Date;
                    dateTimePickerEnd.Value = System.DateTime.Now.Date;
                    operatorFilterTextBox.Text = String.Empty;
                    overrideOpIDFilterTextBox.Text = String.Empty;
                    ManualCmdFilterTextBox.Text = String.Empty;
                    systemIDFilterTextBox.Text = String.Empty;
                }
                else
                {
                    Hide();
                    MessageBox.Show("Manual Command Execution Log Form is not supported with this application.");
                }
            }
        }

        private void useRemoteDBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useRemoteDBCheckBox.Checked)
            {
                if (VtiLib.Data2 == null || !VtiLib.Data2.CheckConnStatus2())
                {
                    MessageBox.Show("Error establishing connection to Remote database. Define the remote database connection string in Edit Cycle, restart the software, and try again.");
                    useRemoteDBCheckBox.Checked = false;
                }
                else
                {
                    db = new VtiDataContext(VtiLib.ConnectionString2);
                }
            }
        }

        private void ResizeFormToShowEntireWidthOfDGV()
        {
            int newFormWidth = 0;
            foreach (DataGridViewColumn col in ManualCmdExecLogDataGridView.Columns)
            {
                if (col.Visible)
                {
                    newFormWidth += col.Width;
                }
            }
            newFormWidth += 80;
            if (newFormWidth > this.Width)
            {
                this.Size = new System.Drawing.Size(newFormWidth, this.Height);
            }
        }

        private void ManualCmdExecLogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
            db.Dispose();
        }

        private void buttonFilterRecords_Click(object sender, EventArgs e)
        {
            FilterRecords();
        }

        private void systemIDFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void operatorFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void overrideOpIDFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void ManualCmdFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void dateTimePickerStart_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void dateTimePickerEnd_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                FilterRecords();
            }
        }

        private void exportDataButton_Click(object sender, EventArgs e)
        {
            var sb = new StringBuilder();

            var headers = ManualCmdExecLogDataGridView.Columns.Cast<DataGridViewColumn>();
            sb.AppendLine(string.Join(",", headers.Select(column => "\"" + column.HeaderText + "\"").ToArray()));

            foreach (DataGridViewRow row in ManualCmdExecLogDataGridView.Rows)
            {
                var cells = row.Cells.Cast<DataGridViewCell>();
                sb.AppendLine(string.Join(",", cells.Select(cell => "\"" + cell.Value + "\"").ToArray()));
            }
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.Filter = "CSV files (*.csv)|*.csv";
            saveFileDialog1.FilterIndex = 2;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllText(saveFileDialog1.FileName, sb.ToString());
            }
        }
    }
}
