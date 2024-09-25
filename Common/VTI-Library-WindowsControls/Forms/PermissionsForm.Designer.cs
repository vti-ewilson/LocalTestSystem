namespace VTIWindowsControlLibrary.Forms
{
    partial class PermissionsForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermissionsForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.buttonOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.dataGridViewCommands = new System.Windows.Forms.DataGridView();
            this.Command = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Group01 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group02 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group03 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group04 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group05 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group06 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group07 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group08 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Group09 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.groupBoxOperatorDetails = new System.Windows.Forms.GroupBox();
            this.buttonOperatorCancel = new System.Windows.Forms.Button();
            this.buttonOperatorAccept = new System.Windows.Forms.Button();
            this.comboBoxGroup = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxPassword2 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPassword = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxOpID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridViewOperators = new System.Windows.Forms.DataGridView();
            this.Operator = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Group = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Password = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonOperatorRemove = new System.Windows.Forms.Button();
            this.buttonOperatorAddNew = new System.Windows.Forms.Button();
            this.contextMenuStripGrantDeny = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.grantAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.denyAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommands)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.groupBoxOperatorDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOperators)).BeginInit();
            this.contextMenuStripGrantDeny.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.buttonCancel);
            this.panel2.Controls.Add(this.buttonOK);
            this.panel2.Name = "panel2";
            // 
            // buttonCancel
            // 
            this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonCancel, "buttonCancel");
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
            // 
            // buttonOK
            // 
            resources.ApplyResources(this.buttonOK, "buttonOK");
            this.buttonOK.Name = "buttonOK";
            this.buttonOK.UseVisualStyleBackColor = true;
            this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.dataGridViewCommands);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // dataGridViewCommands
            // 
            this.dataGridViewCommands.AllowUserToAddRows = false;
            this.dataGridViewCommands.AllowUserToDeleteRows = false;
            this.dataGridViewCommands.AllowUserToResizeColumns = false;
            this.dataGridViewCommands.AllowUserToResizeRows = false;
            this.dataGridViewCommands.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewCommands.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Command,
            this.Group01,
            this.Group02,
            this.Group03,
            this.Group04,
            this.Group05,
            this.Group06,
            this.Group07,
            this.Group08,
            this.Group09});
            resources.ApplyResources(this.dataGridViewCommands, "dataGridViewCommands");
            this.dataGridViewCommands.Name = "dataGridViewCommands";
            this.dataGridViewCommands.RowHeadersVisible = false;
            this.dataGridViewCommands.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.ColumnHeaderSelect;
            this.dataGridViewCommands.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewCommands_CellMouseClick);
            this.dataGridViewCommands.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridViewCommands_ColumnHeaderMouseClick);
            // 
            // Command
            // 
            this.Command.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            resources.ApplyResources(this.Command, "Command");
            this.Command.Name = "Command";
            this.Command.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Group01
            // 
            this.Group01.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group01, "Group01");
            this.Group01.Name = "Group01";
            // 
            // Group02
            // 
            this.Group02.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group02, "Group02");
            this.Group02.Name = "Group02";
            // 
            // Group03
            // 
            this.Group03.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group03, "Group03");
            this.Group03.Name = "Group03";
            // 
            // Group04
            // 
            this.Group04.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group04, "Group04");
            this.Group04.Name = "Group04";
            // 
            // Group05
            // 
            this.Group05.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group05, "Group05");
            this.Group05.Name = "Group05";
            // 
            // Group06
            // 
            this.Group06.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group06, "Group06");
            this.Group06.Name = "Group06";
            // 
            // Group07
            // 
            this.Group07.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group07, "Group07");
            this.Group07.Name = "Group07";
            // 
            // Group08
            // 
            this.Group08.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group08, "Group08");
            this.Group08.Name = "Group08";
            // 
            // Group09
            // 
            this.Group09.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            resources.ApplyResources(this.Group09, "Group09");
            this.Group09.Name = "Group09";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.groupBoxOperatorDetails);
            this.tabPage3.Controls.Add(this.dataGridViewOperators);
            this.tabPage3.Controls.Add(this.buttonOperatorRemove);
            this.tabPage3.Controls.Add(this.buttonOperatorAddNew);
            resources.ApplyResources(this.tabPage3, "tabPage3");
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBoxOperatorDetails
            // 
            this.groupBoxOperatorDetails.Controls.Add(this.buttonOperatorCancel);
            this.groupBoxOperatorDetails.Controls.Add(this.buttonOperatorAccept);
            this.groupBoxOperatorDetails.Controls.Add(this.comboBoxGroup);
            this.groupBoxOperatorDetails.Controls.Add(this.label4);
            this.groupBoxOperatorDetails.Controls.Add(this.textBoxPassword2);
            this.groupBoxOperatorDetails.Controls.Add(this.label3);
            this.groupBoxOperatorDetails.Controls.Add(this.textBoxPassword);
            this.groupBoxOperatorDetails.Controls.Add(this.label2);
            this.groupBoxOperatorDetails.Controls.Add(this.textBoxOpID);
            this.groupBoxOperatorDetails.Controls.Add(this.label1);
            resources.ApplyResources(this.groupBoxOperatorDetails, "groupBoxOperatorDetails");
            this.groupBoxOperatorDetails.Name = "groupBoxOperatorDetails";
            this.groupBoxOperatorDetails.TabStop = false;
            // 
            // buttonOperatorCancel
            // 
            resources.ApplyResources(this.buttonOperatorCancel, "buttonOperatorCancel");
            this.buttonOperatorCancel.Image = global::VTIWindowsControlLibrary.Properties.Resources.Red_X;
            this.buttonOperatorCancel.Name = "buttonOperatorCancel";
            this.buttonOperatorCancel.UseVisualStyleBackColor = true;
            this.buttonOperatorCancel.Click += new System.EventHandler(this.buttonOperatorCancel_Click);
            // 
            // buttonOperatorAccept
            // 
            resources.ApplyResources(this.buttonOperatorAccept, "buttonOperatorAccept");
            this.buttonOperatorAccept.Image = global::VTIWindowsControlLibrary.Properties.Resources.GreenCheck;
            this.buttonOperatorAccept.Name = "buttonOperatorAccept";
            this.buttonOperatorAccept.UseVisualStyleBackColor = true;
            this.buttonOperatorAccept.Click += new System.EventHandler(this.buttonOperatorAccept_Click);
            // 
            // comboBoxGroup
            // 
            this.comboBoxGroup.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxGroup.FormattingEnabled = true;
            this.comboBoxGroup.Items.AddRange(new object[] {
            resources.GetString("comboBoxGroup.Items"),
            resources.GetString("comboBoxGroup.Items1"),
            resources.GetString("comboBoxGroup.Items2"),
            resources.GetString("comboBoxGroup.Items3"),
            resources.GetString("comboBoxGroup.Items4"),
            resources.GetString("comboBoxGroup.Items5"),
            resources.GetString("comboBoxGroup.Items6"),
            resources.GetString("comboBoxGroup.Items7"),
            resources.GetString("comboBoxGroup.Items8")});
            resources.ApplyResources(this.comboBoxGroup, "comboBoxGroup");
            this.comboBoxGroup.Name = "comboBoxGroup";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // textBoxPassword2
            // 
            resources.ApplyResources(this.textBoxPassword2, "textBoxPassword2");
            this.textBoxPassword2.Name = "textBoxPassword2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // textBoxPassword
            // 
            resources.ApplyResources(this.textBoxPassword, "textBoxPassword");
            this.textBoxPassword.Name = "textBoxPassword";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // textBoxOpID
            // 
            resources.ApplyResources(this.textBoxOpID, "textBoxOpID");
            this.textBoxOpID.Name = "textBoxOpID";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // dataGridViewOperators
            // 
            this.dataGridViewOperators.AllowUserToAddRows = false;
            this.dataGridViewOperators.AllowUserToDeleteRows = false;
            this.dataGridViewOperators.AllowUserToResizeColumns = false;
            this.dataGridViewOperators.AllowUserToResizeRows = false;
            this.dataGridViewOperators.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewOperators.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Operator,
            this.Group,
            this.Password,
            this.Status});
            resources.ApplyResources(this.dataGridViewOperators, "dataGridViewOperators");
            this.dataGridViewOperators.MultiSelect = false;
            this.dataGridViewOperators.Name = "dataGridViewOperators";
            this.dataGridViewOperators.ReadOnly = true;
            this.dataGridViewOperators.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewOperators.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewOperators_CellDoubleClick);
            // 
            // Operator
            // 
            this.Operator.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Operator.DefaultCellStyle = dataGridViewCellStyle1;
            resources.ApplyResources(this.Operator, "Operator");
            this.Operator.Name = "Operator";
            this.Operator.ReadOnly = true;
            // 
            // Group
            // 
            this.Group.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Group.DefaultCellStyle = dataGridViewCellStyle2;
            resources.ApplyResources(this.Group, "Group");
            this.Group.Name = "Group";
            this.Group.ReadOnly = true;
            // 
            // Password
            // 
            resources.ApplyResources(this.Password, "Password");
            this.Password.Name = "Password";
            this.Password.ReadOnly = true;
            // 
            // Status
            // 
            resources.ApplyResources(this.Status, "Status");
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // buttonOperatorRemove
            // 
            resources.ApplyResources(this.buttonOperatorRemove, "buttonOperatorRemove");
            this.buttonOperatorRemove.Image = global::VTIWindowsControlLibrary.Properties.Resources.Red_Minus;
            this.buttonOperatorRemove.Name = "buttonOperatorRemove";
            this.buttonOperatorRemove.UseVisualStyleBackColor = true;
            this.buttonOperatorRemove.Click += new System.EventHandler(this.buttonOperatorRemove_Click);
            // 
            // buttonOperatorAddNew
            // 
            resources.ApplyResources(this.buttonOperatorAddNew, "buttonOperatorAddNew");
            this.buttonOperatorAddNew.Image = global::VTIWindowsControlLibrary.Properties.Resources.Blue_Plus;
            this.buttonOperatorAddNew.Name = "buttonOperatorAddNew";
            this.buttonOperatorAddNew.UseVisualStyleBackColor = true;
            this.buttonOperatorAddNew.Click += new System.EventHandler(this.buttonOperatorAddNew_Click);
            // 
            // contextMenuStripGrantDeny
            // 
            this.contextMenuStripGrantDeny.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.grantAllToolStripMenuItem,
            this.denyAllToolStripMenuItem});
            this.contextMenuStripGrantDeny.Name = "contextMenuStripGrantDeny";
            resources.ApplyResources(this.contextMenuStripGrantDeny, "contextMenuStripGrantDeny");
            // 
            // grantAllToolStripMenuItem
            // 
            this.grantAllToolStripMenuItem.Name = "grantAllToolStripMenuItem";
            resources.ApplyResources(this.grantAllToolStripMenuItem, "grantAllToolStripMenuItem");
            this.grantAllToolStripMenuItem.Click += new System.EventHandler(this.grantAllToolStripMenuItem_Click);
            // 
            // denyAllToolStripMenuItem
            // 
            this.denyAllToolStripMenuItem.Name = "denyAllToolStripMenuItem";
            resources.ApplyResources(this.denyAllToolStripMenuItem, "denyAllToolStripMenuItem");
            this.denyAllToolStripMenuItem.Click += new System.EventHandler(this.denyAllToolStripMenuItem_Click);
            // 
            // PermissionsForm
            // 
            this.AcceptButton = this.buttonOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonCancel;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PermissionsForm";
            this.Load += new System.EventHandler(this.frmPermissions_Load);
            this.VisibleChanged += new System.EventHandler(this.PermissionsForm_VisibleChanged);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewCommands)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.groupBoxOperatorDetails.ResumeLayout(false);
            this.groupBoxOperatorDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewOperators)).EndInit();
            this.contextMenuStripGrantDeny.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonOK;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.DataGridView dataGridViewCommands;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripGrantDeny;
        private System.Windows.Forms.ToolStripMenuItem grantAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem denyAllToolStripMenuItem;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button buttonOperatorAddNew;
        private System.Windows.Forms.Button buttonOperatorRemove;
        private System.Windows.Forms.DataGridView dataGridViewOperators;
        private System.Windows.Forms.GroupBox groupBoxOperatorDetails;
        private System.Windows.Forms.TextBox textBoxPassword2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPassword;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxOpID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonOperatorCancel;
        private System.Windows.Forms.Button buttonOperatorAccept;
        private System.Windows.Forms.DataGridViewTextBoxColumn Operator;
        private System.Windows.Forms.DataGridViewTextBoxColumn Group;
        private System.Windows.Forms.DataGridViewTextBoxColumn Password;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Command;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group01;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group02;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group03;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group04;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group05;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group06;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group07;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group08;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Group09;
    }
}