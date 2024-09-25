namespace VTIWindowsControlLibrary.Forms
{
    partial class InquireForm
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridViewUUTRecords2 = new System.Windows.Forms.DataGridView();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dataGridViewUUTRecordDetails2 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ScanButton = new System.Windows.Forms.Button();
            this.SystemIDBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.useRemoteDBCheckBox = new System.Windows.Forms.CheckBox();
            this.checkBoxIncludeDetails = new System.Windows.Forms.CheckBox();
            this.buttonExportData = new System.Windows.Forms.Button();
            this.TestResultBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.TestTypeBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ModelBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SerialNumBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.OperatorBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTimePickerEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dateTimePickerStart = new System.Windows.Forms.DateTimePicker();
            this.buttonFilterRecords = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.bindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUUTRecords2)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUUTRecordDetails2)).BeginInit();
            this.panel2.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 1;
            this.tabControl1.Size = new System.Drawing.Size(875, 438);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(867, 409);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Custom Data Inquiry";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 80);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.groupBox3);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.groupBox4);
            this.splitContainer2.Size = new System.Drawing.Size(861, 326);
            this.splitContainer2.SplitterDistance = 160;
            this.splitContainer2.TabIndex = 11;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewUUTRecords2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(861, 160);
            this.groupBox3.TabIndex = 9;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "UUT Records";
            // 
            // dataGridViewUUTRecords2
            // 
            this.dataGridViewUUTRecords2.AllowUserToAddRows = false;
            this.dataGridViewUUTRecords2.AllowUserToDeleteRows = false;
            this.dataGridViewUUTRecords2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewUUTRecords2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewUUTRecords2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUUTRecords2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUUTRecords2.Location = new System.Drawing.Point(3, 18);
            this.dataGridViewUUTRecords2.MultiSelect = false;
            this.dataGridViewUUTRecords2.Name = "dataGridViewUUTRecords2";
            this.dataGridViewUUTRecords2.ReadOnly = true;
            this.dataGridViewUUTRecords2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUUTRecords2.Size = new System.Drawing.Size(855, 139);
            this.dataGridViewUUTRecords2.TabIndex = 7;
            this.dataGridViewUUTRecords2.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewUutRecords2_RowEnter);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dataGridViewUUTRecordDetails2);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(0, 0);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(861, 162);
            this.groupBox4.TabIndex = 10;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "UUT Record Details";
            // 
            // dataGridViewUUTRecordDetails2
            // 
            this.dataGridViewUUTRecordDetails2.AllowUserToAddRows = false;
            this.dataGridViewUUTRecordDetails2.AllowUserToDeleteRows = false;
            this.dataGridViewUUTRecordDetails2.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dataGridViewUUTRecordDetails2.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dataGridViewUUTRecordDetails2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewUUTRecordDetails2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewUUTRecordDetails2.Location = new System.Drawing.Point(3, 18);
            this.dataGridViewUUTRecordDetails2.MultiSelect = false;
            this.dataGridViewUUTRecordDetails2.Name = "dataGridViewUUTRecordDetails2";
            this.dataGridViewUUTRecordDetails2.ReadOnly = true;
            this.dataGridViewUUTRecordDetails2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewUUTRecordDetails2.Size = new System.Drawing.Size(855, 141);
            this.dataGridViewUUTRecordDetails2.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.AutoSize = true;
            this.panel2.Controls.Add(this.ScanButton);
            this.panel2.Controls.Add(this.SystemIDBox);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.useRemoteDBCheckBox);
            this.panel2.Controls.Add(this.checkBoxIncludeDetails);
            this.panel2.Controls.Add(this.buttonExportData);
            this.panel2.Controls.Add(this.TestResultBox);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.TestTypeBox);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.ModelBox);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.SerialNumBox);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.OperatorBox);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.dateTimePickerEnd);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.dateTimePickerStart);
            this.panel2.Controls.Add(this.buttonFilterRecords);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(861, 77);
            this.panel2.TabIndex = 6;
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(327, 51);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanButton.TabIndex = 25;
            this.ScanButton.Text = "Scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Visible = false;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // SystemIDBox
            // 
            this.SystemIDBox.Location = new System.Drawing.Point(751, 22);
            this.SystemIDBox.Name = "SystemIDBox";
            this.SystemIDBox.Size = new System.Drawing.Size(100, 22);
            this.SystemIDBox.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(748, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 23;
            this.label1.Text = "System ID:";
            // 
            // useRemoteDBCheckBox
            // 
            this.useRemoteDBCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.useRemoteDBCheckBox.AutoSize = true;
            this.useRemoteDBCheckBox.Location = new System.Drawing.Point(478, 52);
            this.useRemoteDBCheckBox.Name = "useRemoteDBCheckBox";
            this.useRemoteDBCheckBox.Size = new System.Drawing.Size(166, 20);
            this.useRemoteDBCheckBox.TabIndex = 22;
            this.useRemoteDBCheckBox.Text = "Use Remote Database";
            this.useRemoteDBCheckBox.UseVisualStyleBackColor = true;
            this.useRemoteDBCheckBox.CheckedChanged += new System.EventHandler(this.useRemoteDBCheckBox_CheckedChanged);
            // 
            // checkBoxIncludeDetails
            // 
            this.checkBoxIncludeDetails.AutoSize = true;
            this.checkBoxIncludeDetails.Location = new System.Drawing.Point(7, 53);
            this.checkBoxIncludeDetails.Name = "checkBoxIncludeDetails";
            this.checkBoxIncludeDetails.Size = new System.Drawing.Size(110, 20);
            this.checkBoxIncludeDetails.TabIndex = 21;
            this.checkBoxIncludeDetails.Text = "Export Details";
            this.checkBoxIncludeDetails.UseVisualStyleBackColor = true;
            // 
            // buttonExportData
            // 
            this.buttonExportData.Location = new System.Drawing.Point(123, 50);
            this.buttonExportData.Name = "buttonExportData";
            this.buttonExportData.Size = new System.Drawing.Size(198, 23);
            this.buttonExportData.TabIndex = 20;
            this.buttonExportData.Text = "Export Data";
            this.buttonExportData.UseVisualStyleBackColor = true;
            this.buttonExportData.Click += new System.EventHandler(this.buttonExportData_Click);
            // 
            // TestResultBox
            // 
            this.TestResultBox.Location = new System.Drawing.Point(539, 23);
            this.TestResultBox.Name = "TestResultBox";
            this.TestResultBox.Size = new System.Drawing.Size(100, 22);
            this.TestResultBox.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(536, 4);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 16);
            this.label7.TabIndex = 18;
            this.label7.Text = "Test Result:";
            // 
            // TestTypeBox
            // 
            this.TestTypeBox.Location = new System.Drawing.Point(433, 23);
            this.TestTypeBox.Name = "TestTypeBox";
            this.TestTypeBox.Size = new System.Drawing.Size(100, 22);
            this.TestTypeBox.TabIndex = 17;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(430, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "Test Type:";
            // 
            // ModelBox
            // 
            this.ModelBox.Location = new System.Drawing.Point(327, 23);
            this.ModelBox.Name = "ModelBox";
            this.ModelBox.Size = new System.Drawing.Size(100, 22);
            this.ModelBox.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(324, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 16);
            this.label5.TabIndex = 14;
            this.label5.Text = "Model Number:";
            // 
            // SerialNumBox
            // 
            this.SerialNumBox.Location = new System.Drawing.Point(221, 23);
            this.SerialNumBox.Name = "SerialNumBox";
            this.SerialNumBox.Size = new System.Drawing.Size(100, 22);
            this.SerialNumBox.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(218, 4);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(97, 16);
            this.label8.TabIndex = 12;
            this.label8.Text = "Serial Number:";
            // 
            // OperatorBox
            // 
            this.OperatorBox.Location = new System.Drawing.Point(645, 23);
            this.OperatorBox.Name = "OperatorBox";
            this.OperatorBox.Size = new System.Drawing.Size(100, 22);
            this.OperatorBox.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(642, 4);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 16);
            this.label2.TabIndex = 12;
            this.label2.Text = "Operator:";
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
            // 
            // buttonFilterRecords
            // 
            this.buttonFilterRecords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonFilterRecords.Location = new System.Drawing.Point(650, 50);
            this.buttonFilterRecords.Name = "buttonFilterRecords";
            this.buttonFilterRecords.Size = new System.Drawing.Size(206, 23);
            this.buttonFilterRecords.TabIndex = 7;
            this.buttonFilterRecords.Text = "Filter Records";
            this.buttonFilterRecords.UseVisualStyleBackColor = true;
            this.buttonFilterRecords.Click += new System.EventHandler(this.buttonFilterRecords_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 438);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(875, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.Filter = "Comma-Seperated Text File (*.csv)|*.csv|Tab-Seperated Text File (*.txt)|*.txt";
            // 
            // InquireForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(875, 460);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.statusStrip1);
            this.Name = "InquireForm";
            this.Text = "Data Inquire Form";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.InquireForm_FormClosing);
            this.VisibleChanged += new System.EventHandler(this.InquireForm_VisibleChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.InquireForm_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUUTRecords2)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewUUTRecordDetails2)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button buttonFilterRecords;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridViewUUTRecords2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dataGridViewUUTRecordDetails2;
        private System.Windows.Forms.BindingSource bindingSource1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTimePickerEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dateTimePickerStart;
        private System.Windows.Forms.TextBox ModelBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox OperatorBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonExportData;
        private System.Windows.Forms.TextBox TestResultBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox TestTypeBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBoxIncludeDetails;
        private System.Windows.Forms.TextBox SerialNumBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox useRemoteDBCheckBox;
        private System.Windows.Forms.TextBox SystemIDBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ScanButton;
    }
}