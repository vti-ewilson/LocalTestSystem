using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
using System.Transactions;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.ManualCommands;
using VTIWindowsControlLibrary.Data;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents the Permissions Form of the client application
    /// </summary>
    public partial class PermissionsForm : Form
    {
        private bool addingOperator;

        /// <summary>
        /// Initializes a new instance of the <see cref="PermissionsForm">PermissionsForm</see>
        /// </summary>
        public PermissionsForm()
        {
            InitializeComponent();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void dataGridViewCommands_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // if user right-clicks in top-left cell, select entire grid and pop up the menu
            if (e.ColumnIndex == 0)
            {
                if (e.Button == MouseButtons.Right)
                {
                    this.dataGridViewCommands.ClearSelection();
                    this.dataGridViewCommands.SelectAll();
                    this.contextMenuStripGrantDeny.Show(Cursor.Position.X, Cursor.Position.Y);
                }
            }
            // if user right-clicks on a column header, select the column and pop up the menu
            else if (e.ColumnIndex > 0)
            {
                this.dataGridViewCommands.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
                if (e.Button == MouseButtons.Right)
                {
                    this.dataGridViewCommands.ClearSelection();
                    this.dataGridViewCommands.Columns[e.ColumnIndex].Selected = true;
                    this.contextMenuStripGrantDeny.Show(Cursor.Position.X, Cursor.Position.Y);
                }
            }
        }

        private void dataGridViewCommands_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            // if user right-clicks on a command, select the row and pop up the menu
            if ((e.ColumnIndex == 0) && (e.RowIndex >= 0))
            {
                this.dataGridViewCommands.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                this.dataGridViewCommands.ClearSelection();
                this.dataGridViewCommands.Rows[e.RowIndex].Selected = true;
                if (e.Button == MouseButtons.Right)
                {
                    this.contextMenuStripGrantDeny.Show(Cursor.Position.X, Cursor.Position.Y);
                }
            }
        }

        private void grantAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGridViewCommands.SelectedCells)
            {
                if (cell.ColumnIndex > 0)
                    cell.Value = true;
            }
        }

        private void denyAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewCell cell in dataGridViewCommands.SelectedCells)
            {
                if (cell.ColumnIndex > 0)
                    cell.Value = false;
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            int i, j;

            #region show error and abort if operator name or password duplicates

            List<(string opID, string password)> usersList = new List<(string opID, string password)>();

            for (j = 0; j < dataGridViewOperators.Rows.Count; j++)
            {
                string opID = dataGridViewOperators[0, j].Value.ToString();
                //string GroupID = dataGridViewOperators[1, j].Value.ToString(),
                string password = dataGridViewOperators[2, j].Value.ToString();

                // dont check the entries that are set to be deleted for duplicates
                if (dataGridViewOperators[3, j].Value.ToString() != "deleted")
                {
                    usersList.Add((opID.ToUpper(), password.ToUpper()));
                }
            }

            // Find duplicate opIDs
            var duplicateOpIDs = usersList
                .GroupBy(user => user.opID)
                .Where(group => group.Count() > 1)
                .SelectMany(group => group)
                .ToList();

            // Find duplicate passwords
            var duplicatePasswords = usersList
                .GroupBy(user => user.password)
                .Where(group => group.Count() > 1)
                .SelectMany(group => group)
                .ToList();

            string msg = "";

            if (duplicateOpIDs.Any())
            {
                msg += $"Duplicate operator name entries:{Environment.NewLine}";
                foreach (var opID in duplicateOpIDs.Select(x => x.opID).Distinct())
                {
                    msg += $"{opID}{Environment.NewLine}";
                }
                msg += Environment.NewLine;
            }
            if (duplicatePasswords.Any())
            {
                msg += $"Duplicate passwords detected.{Environment.NewLine}{Environment.NewLine}";
            }

            if (msg != "")
            {
                msg += "Changes were not applied.";
                MessageBox.Show(msg);
                return;
            }

            #endregion show error and abort if operator name or password duplicates

            #region delete the operators set to be deleted first to avoid duplicates error

            using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    // Process operators
                    for (j = 0; j < dataGridViewOperators.Rows.Count; j++)
                    {
                        if (dataGridViewOperators[3, j].Value.ToString() == "deleted")
                        {
                            if (db.Users.Where(x => x.OpID == dataGridViewOperators[0, j].Value.ToString() && x.Password == dataGridViewOperators[2, j].Value.ToString() && x.GroupID == dataGridViewOperators[1, j].Value.ToString()).Count() > 0)
                            {
                                db.Users.DeleteOnSubmit(
                                    db.Users.Single(u => (u.OpID == dataGridViewOperators[0, j].Value.ToString()) && (u.Password == dataGridViewOperators[2, j].Value.ToString()) && (u.GroupID == dataGridViewOperators[1, j].Value.ToString())));
                            }
                        }
                    }
                    db.SubmitChanges();
                    ts.Complete();
                }
            }

            #endregion delete the operators set to be deleted first to avoid duplicates error

            using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
            {
                using (TransactionScope ts = new TransactionScope())
                {
                    // Process GroupCommands many-to-many link
                    for (i = 1; i <= 9; i++)
                    {
                        for (j = 0; j < dataGridViewCommands.Rows.Count; j++)
                        {
                            if ((bool)dataGridViewCommands[i, j].Value == true)
                            {
                                db.GroupCommands.InsertOnSubmit(
                                    new GroupCommand
                                    {
                                        GroupID = "GROUP" + dataGridViewCommands.Columns[i].HeaderText,
                                        CommandID = dataGridViewCommands[0, j].Value.ToString()
                                    });
                            }
                            else
                            {
                                int cnt = db.GroupCommands.Count(gc => gc.GroupID == "GROUP" + dataGridViewCommands.Columns[i].HeaderText &&
                                        gc.CommandID == dataGridViewCommands[0, j].Value.ToString());
                                if (cnt >= 1)
                                {
                                    for (int ii = 0; ii < cnt; ii++)
                                    {
                                        db.GroupCommands.DeleteOnSubmit(
                                            db.GroupCommands.First(
                                                gc => gc.GroupID == "GROUP" + dataGridViewCommands.Columns[i].HeaderText &&
                                                gc.CommandID == dataGridViewCommands[0, j].Value.ToString()));
                                        if (cnt > 1)
                                            db.SubmitChanges();
                                    }
                                }
                            }
                        }
                    }
                    // Process operators
                    for (j = 0; j < dataGridViewOperators.Rows.Count; j++)
                    {
                        if (dataGridViewOperators[3, j].Value.ToString() == "new")
                        {
                            db.Users.InsertOnSubmit(
                                new User
                                {
                                    OpID = dataGridViewOperators[0, j].Value.ToString(),
                                    GroupID = dataGridViewOperators[1, j].Value.ToString(),
                                    Password = dataGridViewOperators[2, j].Value.ToString()
                                });
                        }
                        else if (dataGridViewOperators[3, j].Value.ToString() == "changed")
                        {
                            User user = db.Users.Single(u => u.OpID == dataGridViewOperators[0, j].Value.ToString());
                            user.GroupID = dataGridViewOperators[1, j].Value.ToString();
                            user.Password = dataGridViewOperators[2, j].Value.ToString();
                            //var t = VtiLib.Data.Groups.Where(x => x.GroupID == dataGridViewOperators[1, j].Value.ToString());
                            //user.Group = VtiLib.Data.Groups.Where(x=> x.GroupID == dataGridViewOperators[1, j].Value.ToString()).FirstOrDefault();
                        }
                    }
                    db.SubmitChanges();
                    ts.Complete();
                }
            }

            this.Hide();
        }

        private void dataGridViewOperators_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                this.groupBoxOperatorDetails.Visible = true;
                this.dataGridViewOperators.Enabled = false;
                this.buttonOperatorAddNew.Enabled = false;
                this.buttonOperatorRemove.Enabled = false;
                this.buttonOK.Enabled = false;
                this.textBoxOpID.Text = this.dataGridViewOperators[0, e.RowIndex].Value.ToString();
                this.textBoxOpID.Enabled = false;
                this.comboBoxGroup.Text = this.dataGridViewOperators[1, e.RowIndex].Value.ToString();
                this.textBoxPassword.Text = this.dataGridViewOperators[2, e.RowIndex].Value.ToString();
                this.textBoxPassword2.Text = this.dataGridViewOperators[2, e.RowIndex].Value.ToString();
                this.addingOperator = false;
                this.textBoxPassword.Select();
                this.AcceptButton = this.buttonOperatorAccept;
                this.CancelButton = this.buttonOperatorCancel;
            }
        }

        private void buttonOperatorCancel_Click(object sender, EventArgs e)
        {
            this.groupBoxOperatorDetails.Visible = false;
            this.dataGridViewOperators.Enabled = true;
            this.buttonOperatorAddNew.Enabled = true;
            this.buttonOperatorRemove.Enabled = true;
            this.buttonOK.Enabled = true;
            this.AcceptButton = this.buttonOK;
            this.CancelButton = this.buttonCancel;
        }

        private void frmPermissions_Load(object sender, EventArgs e)
        {
        }

        private void buttonOperatorAccept_Click(object sender, EventArgs e)
        {
            if ((this.textBoxPassword.Text == "") && (this.textBoxPassword2.Text == ""))
            {
                MessageBox.Show(VtiLibLocalization.PleaseEnterPassword, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (this.textBoxPassword.Text != this.textBoxPassword2.Text)
            {
                MessageBox.Show(VtiLibLocalization.PasswordsDontMatch, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                this.groupBoxOperatorDetails.Visible = false;
                this.dataGridViewOperators.Enabled = true;
                this.buttonOperatorAddNew.Enabled = true;
                this.buttonOperatorRemove.Enabled = true;
                this.buttonOK.Enabled = true;
                this.AcceptButton = this.buttonOK;
                this.CancelButton = this.buttonCancel;

                //if the newly added user has the same OpID as one that is set to be deleted, 
                //then mark the existing user from "deleted" to "changed" and give it the new properties
                bool changeUserFromDeletedToChanged = false;
                int changeUserFromDeletedToChanged_rowIndex = -1;
                foreach (DataGridViewRow row in dataGridViewOperators.Rows)
                {
                    if (this.dataGridViewOperators[0, row.Index].Value.ToString() == this.textBoxOpID.Text
                            && dataGridViewOperators[3, row.Index].Value.ToString() == "deleted")
                    {
                        changeUserFromDeletedToChanged = true;
                        changeUserFromDeletedToChanged_rowIndex = row.Index;
                    }
                }

                if (this.addingOperator && !changeUserFromDeletedToChanged)
                {
                    int i = this.dataGridViewOperators.Rows.Add();
                    this.dataGridViewOperators[0, i].Value = this.textBoxOpID.Text;
                    this.dataGridViewOperators[1, i].Value = this.comboBoxGroup.Text;
                    this.dataGridViewOperators[2, i].Value = this.textBoxPassword.Text;
                    this.dataGridViewOperators[3, i].Value = "new";
                }
                else if (changeUserFromDeletedToChanged)
                {
                    this.dataGridViewOperators[1, changeUserFromDeletedToChanged_rowIndex].Value = this.comboBoxGroup.Text;
                    this.dataGridViewOperators[2, changeUserFromDeletedToChanged_rowIndex].Value = this.textBoxPassword.Text;
                    this.dataGridViewOperators[3, changeUserFromDeletedToChanged_rowIndex].Value = "changed";
                    this.dataGridViewOperators.Rows[changeUserFromDeletedToChanged_rowIndex].Visible = true;

                }
                else
                {
                    int i = this.dataGridViewOperators.CurrentRow.Index;
                    //dataGridViewOperators[0, e.RowIndex].Value = this.textBoxOpID.Text;
                    this.dataGridViewOperators[1, i].Value = this.comboBoxGroup.Text;
                    this.dataGridViewOperators[2, i].Value = this.textBoxPassword.Text;
                    var users =
                        from u in VtiLib.Data.Users
                        where u.GroupID != "GROUP10" && u.IsLocked != true
                        select u;
                    //if this user already exists in the table, mark it to be changed
                    if (users.Any(x => x.OpID == this.dataGridViewOperators[0, i].Value.ToString()))
                    {
                        this.dataGridViewOperators[3, i].Value = "changed";
                    }
                    //If not, they are just editing a newly added user which has not been inserted into the table yet
                    else
                    {
                        this.dataGridViewOperators[3, i].Value = "new";
                    }
                }
                this.dataGridViewOperators.Select();
            }
        }

        private void buttonOperatorAddNew_Click(object sender, EventArgs e)
        {
            this.groupBoxOperatorDetails.Visible = true;
            this.dataGridViewOperators.Enabled = false;
            this.buttonOperatorAddNew.Enabled = false;
            this.buttonOperatorRemove.Enabled = false;
            this.buttonOK.Enabled = false;
            this.textBoxOpID.Text = string.Empty;
            this.textBoxOpID.Enabled = true;
            this.comboBoxGroup.Text = "GROUP01";
            this.textBoxPassword.Text = "";
            this.textBoxPassword2.Text = "";
            this.addingOperator = true;
            this.textBoxOpID.Select();
        }

        private void buttonOperatorRemove_Click(object sender, EventArgs e)
        {
            String OpID = this.dataGridViewOperators.CurrentRow.Cells[0].Value.ToString();
            if (MessageBox.Show(
                string.Format(VtiLibLocalization.AskRemoveOperator, OpID),
                Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                this.dataGridViewOperators.CurrentRow.Cells[3].Value = "deleted";
                this.dataGridViewOperators.CurrentRow.Visible = false;
            }
        }

        private void PermissionsForm_VisibleChanged(object sender, EventArgs e)
        {
            int i, j;

            if (Visible)
            {
                // Commands
                this.dataGridViewCommands.Rows.Clear();

                var commandList =
                    from m in VtiLib.ManualCommands.GetType().GetMethods(BindingFlags.Public |
                        BindingFlags.Instance | BindingFlags.Static |
                        BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy)
                    from a in m.GetCustomAttributes(false).OfType<ManualCommandAttribute>()
                    where
                        (a.Permission == CommandPermissionType.CheckPermission ||
                        a.Permission == CommandPermissionType.CheckPermissionWithWarning &&
                        a.ShowInPermissionsForm)
                    orderby a.CommandText
                    select a.CommandText;

                // add them to the form
                foreach (String commandText in commandList)
                {
                    j = dataGridViewCommands.Rows.Add();
                    dataGridViewCommands[0, j].Value = commandText;
                    dataGridViewCommands[0, j].ReadOnly = true;
                    for (i = 1; i <= 9; i++)
                        dataGridViewCommands[i, j].Value = VtiLib.Data.CheckGroupCommand("GROUP0" + i.ToString(), commandText);
                }

                this.dataGridViewCommands.ClearSelection();
                this.dataGridViewCommands[1, 0].Selected = true;

                // Operators
                this.dataGridViewOperators.Rows.Clear();
                using (VtiDataContext db = new VtiDataContext(VtiLib.ConnectionString))
                {
                    System.Linq.IQueryable<User> users =
                        from u in db.Users
                        where u.GroupID != "GROUP10" && u.IsLocked != true
                        select u;
                    users = users.OrderBy(x => x.OpID);
                    foreach (User user in users)
                    {
                        this.dataGridViewOperators.Rows.Add(user.OpID, user.GroupID, user.Password, "unchanged");
                    }
                }
                this.dataGridViewOperators.Rows[0].Selected = true;

                int tableWidth = 0;
                int tableHeight = 0;
                foreach (DataGridViewTextBoxColumn column in dataGridViewCommands.Columns.OfType<DataGridViewTextBoxColumn>())
                {
                    tableWidth += column.Width;
                }
                foreach (DataGridViewCheckBoxColumn column in dataGridViewCommands.Columns.OfType<DataGridViewCheckBoxColumn>())
                {
                    tableWidth += column.Width;
                }
                foreach (DataGridViewRow row in dataGridViewCommands.Rows)
                {
                    tableHeight += row.Height;
                }
                int targetHeight = tableHeight + 110;
                //set height and width of form
                this.Width = tableWidth + 185;

                int maxHeight = 700;
                int minHeight = 495;
                if (targetHeight > maxHeight)
                {
                    this.Height = maxHeight;
                }
                else if (targetHeight < minHeight)
                {
                    this.Height = minHeight;
                }
                else
                {
                    this.Height = targetHeight;
                }
                //this.Height = (targetHeight < maxHeight && targetHeight > minHeight) ? targetHeight : minHeight;
            }
        }
    }
}