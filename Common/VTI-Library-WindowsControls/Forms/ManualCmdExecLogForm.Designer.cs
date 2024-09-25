
namespace VTIWindowsControlLibrary.Forms
{
    partial class ManualCmdExecLogForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel2 = new System.Windows.Forms.Panel();
            this.useRemoteDBCheckBox = new System.Windows.Forms.CheckBox();
            this.exportDataButton = new System.Windows.Forms.Button();
            this.operatorFilterTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.overrideOpIDFilterTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.systemIDFilterTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ManualCmdFilterTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.buttonFilterRecords = new System.Windows.Forms.Button();
            this.ManualCmdExecLogDataGridView = new System.Windows.Forms.DataGridView();
            this.ID3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ManualCmdExecLogDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.useRemoteDBCheckBox);
            this.panel2.Controls.Add(this.exportDataButton);
            this.panel2.Controls.Add(this.operatorFilterTextBox);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.overrideOpIDFilterTextBox);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.systemIDFilterTextBox);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.ManualCmdFilterTextBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dateTimePickerEnd);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.dateTimePickerStart);
            this.panel2.Controls.Add(this.buttonFilterRecords);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 76);
            this.panel2.TabIndex = 7;
            // 
            // useRemoteDBCheckBox
            // 
            this.useRemoteDBCheckBox.AutoSize = true;
            this.useRemoteDBCheckBox.Location = new System.Drawing.Point(348, 52);
            this.useRemoteDBCheckBox.Name = "useRemoteDBCheckBox";
            this.useRemoteDBCheckBox.Size = new System.Drawing.Size(166, 20);
            this.useRemoteDBCheckBox.TabIndex = 21;
            this.useRemoteDBCheckBox.Text = "Use Remote Database";
            this.useRemoteDBCheckBox.UseVisualStyleBackColor = true;
            this.useRemoteDBCheckBox.CheckedChanged += new System.EventHandler(this.useRemoteDBCheckBox_CheckedChanged);
            // 
            // exportDataButton
            // 
            this.exportDataButton.Location = new System.Drawing.Point(7, 50);
            this.exportDataButton.Name = "exportDataButton";
            this.exportDataButton.Size = new System.Drawing.Size(198, 23);
            this.exportDataButton.TabIndex = 20;
            this.exportDataButton.Text = "Export Data";
            this.exportDataButton.UseVisualStyleBackColor = true;
            this.exportDataButton.Click += new System.EventHandler(this.exportDataButton_Click);
            // 
            // operatorFilterTextBox
            // 
            this.operatorFilterTextBox.Location = new System.Drawing.Point(221, 23);
            this.operatorFilterTextBox.Name = "operatorFilterTextBox";
            this.operatorFilterTextBox.Size = new System.Drawing.Size(122, 22);
            this.operatorFilterTextBox.TabIndex = 19;
            this.operatorFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.operatorFilterTextBox_KeyDown);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(218, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Operator:";
            // 
            // overrideOpIDFilterTextBox
            // 
            this.overrideOpIDFilterTextBox.Location = new System.Drawing.Point(349, 23);
            this.overrideOpIDFilterTextBox.Name = "overrideOpIDFilterTextBox";
            this.overrideOpIDFilterTextBox.Size = new System.Drawing.Size(147, 22);
            this.overrideOpIDFilterTextBox.TabIndex = 15;
            this.overrideOpIDFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.overrideOpIDFilterTextBox_KeyDown);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(346, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Override Operator:";
            // 
            // systemIDFilterTextBox
            // 
            this.systemIDFilterTextBox.Location = new System.Drawing.Point(502, 23);
            this.systemIDFilterTextBox.Name = "systemIDFilterTextBox";
            this.systemIDFilterTextBox.Size = new System.Drawing.Size(121, 22);
            this.systemIDFilterTextBox.TabIndex = 13;
            this.systemIDFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.systemIDFilterTextBox_KeyDown);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(499, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(72, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "System ID:";
            // 
            // ManualCmdFilterTextBox
            // 
            this.ManualCmdFilterTextBox.Location = new System.Drawing.Point(629, 23);
            this.ManualCmdFilterTextBox.Name = "ManualCmdFilterTextBox";
            this.ManualCmdFilterTextBox.Size = new System.Drawing.Size(159, 22);
            this.ManualCmdFilterTextBox.TabIndex = 13;
            this.ManualCmdFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ManualCmdFilterTextBox_KeyDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(626, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Manual Command:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 4);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 16);
            this.label4.TabIndex = 11;
            this.label4.Text = "Ending Date:";
            // 
            // dateTimePickerEnd
            // 
            this.dateTimePickerEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerEnd.Location = new System.Drawing.Point(114, 23);
            this.dateTimePickerEnd.Name = "dateTimePickerEnd";
            this.dateTimePickerEnd.Size = new System.Drawing.Size(101, 22);
            this.dateTimePickerEnd.TabIndex = 10;
            this.dateTimePickerEnd.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePickerEnd_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(88, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "Starting Date:";
            // 
            // dateTimePickerStart
            // 
            this.dateTimePickerStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePickerStart.Location = new System.Drawing.Point(7, 23);
            this.dateTimePickerStart.Name = "dateTimePickerStart";
            this.dateTimePickerStart.Size = new System.Drawing.Size(101, 22);
            this.dateTimePickerStart.TabIndex = 8;
            this.dateTimePickerStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dateTimePickerStart_KeyDown);
            // 
            // buttonFilterRecords
            // 
            this.buttonFilterRecords.Location = new System.Drawing.Point(539, 50);
            this.buttonFilterRecords.Name = "buttonFilterRecords";
            this.buttonFilterRecords.Size = new System.Drawing.Size(206, 23);
            this.buttonFilterRecords.TabIndex = 7;
            this.buttonFilterRecords.Text = "Filter Records";
            this.buttonFilterRecords.UseVisualStyleBackColor = true;
            this.buttonFilterRecords.Click += new System.EventHandler(this.buttonFilterRecords_Click);
            // 
            // ManualCmdExecLogDataGridView
            // 
            this.ManualCmdExecLogDataGridView.AllowUserToAddRows = false;
            this.ManualCmdExecLogDataGridView.AllowUserToDeleteRows = false;
            this.ManualCmdExecLogDataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.ManualCmdExecLogDataGridView.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ManualCmdExecLogDataGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.ManualCmdExecLogDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ManualCmdExecLogDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID3,
            this.Column1,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ManualCmdExecLogDataGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.ManualCmdExecLogDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ManualCmdExecLogDataGridView.Location = new System.Drawing.Point(0, 76);
            this.ManualCmdExecLogDataGridView.MultiSelect = false;
            this.ManualCmdExecLogDataGridView.Name = "ManualCmdExecLogDataGridView";
            this.ManualCmdExecLogDataGridView.ReadOnly = true;
            this.ManualCmdExecLogDataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.ManualCmdExecLogDataGridView.Size = new System.Drawing.Size(800, 374);
            this.ManualCmdExecLogDataGridView.TabIndex = 8;
            // 
            // ID3
            // 
            this.ID3.DataPropertyName = "ID";
            this.ID3.HeaderText = "ID";
            this.ID3.Name = "ID3";
            this.ID3.ReadOnly = true;
            this.ID3.Width = 46;
            // 
            // Column1
            // 
            this.Column1.DataPropertyName = "DateTime";
            this.Column1.HeaderText = "Date / Time";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.Width = 95;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "OpID";
            this.dataGridViewTextBoxColumn4.HeaderText = "Operator";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 86;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "OverrideOpID";
            this.dataGridViewTextBoxColumn7.HeaderText = "Override Operator";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 129;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "SystemID";
            this.dataGridViewTextBoxColumn3.HeaderText = "System ID";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 87;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ManualCommand";
            this.dataGridViewTextBoxColumn2.HeaderText = "Manual Command";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 130;
            // 
            // ManualCmdExecLogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.ManualCmdExecLogDataGridView);
            this.Controls.Add(this.panel2);
            this.Name = "ManualCmdExecLogForm";
            this.Text = "Manual Command Execution Log Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ManualCmdExecLogForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.ManualCmdExecLogForm_VisibleChanged);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ManualCmdExecLogDataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button exportDataButton;
        private System.Windows.Forms.TextBox operatorFilterTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox overrideOpIDFilterTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox systemIDFilterTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ManualCmdFilterTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.Button buttonFilterRecords;
        private System.Windows.Forms.DataGridView ManualCmdExecLogDataGridView;
        private System.Windows.Forms.CheckBox useRemoteDBCheckBox;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
    }
}