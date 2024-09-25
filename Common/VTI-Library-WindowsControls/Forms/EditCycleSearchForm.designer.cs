namespace VTIWindowsControlLibrary.Forms
{
    partial class EditCycleSearchForm
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
      this.btnCancel = new System.Windows.Forms.Button();
      this.lblText = new System.Windows.Forms.Label();
      this.txtSearchFor = new System.Windows.Forms.TextBox();
      this.btnSearch = new System.Windows.Forms.Button();
      this.listView1 = new System.Windows.Forms.ListView();
      this.DisplayName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.Value = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.ToolTip = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.SequenceStep = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.OperatingSeq = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
      this.comboBoxFilterBySeq = new System.Windows.Forms.ComboBox();
      this.label1 = new System.Windows.Forms.Label();
      this.checkBoxName = new System.Windows.Forms.CheckBox();
      this.checkBoxValue = new System.Windows.Forms.CheckBox();
      this.checkBoxToolTip = new System.Windows.Forms.CheckBox();
      this.checkBoxSequenceStep = new System.Windows.Forms.CheckBox();
      this.label2 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.label4 = new System.Windows.Forms.Label();
      this.label5 = new System.Windows.Forms.Label();
      this.label6 = new System.Windows.Forms.Label();
      this.IncludeInSearchGroupBox = new System.Windows.Forms.GroupBox();
      this.SideGroupBox = new System.Windows.Forms.GroupBox();
      this.checkBoxWhite = new System.Windows.Forms.CheckBox();
      this.checkBoxBlue = new System.Windows.Forms.CheckBox();
      this.label7 = new System.Windows.Forms.Label();
      this.comboBoxFilterByOpSeq = new System.Windows.Forms.ComboBox();
      this.buttonCopyBlueToWhite = new System.Windows.Forms.Button();
      this.buttonCopyWhiteToBlue = new System.Windows.Forms.Button();
      this.LegendGroupBox = new System.Windows.Forms.GroupBox();
      this.SideGroupBox.SuspendLayout();
      this.LegendGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(549, 0);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(64, 22);
      this.btnCancel.TabIndex = 7;
      this.btnCancel.Text = "Close";
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // lblText
      // 
      this.lblText.Location = new System.Drawing.Point(11, 59);
      this.lblText.Name = "lblText";
      this.lblText.Size = new System.Drawing.Size(64, 16);
      this.lblText.TabIndex = 6;
      this.lblText.Text = "Search For:";
      this.lblText.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // txtSearchFor
      // 
      this.txtSearchFor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.txtSearchFor.Location = new System.Drawing.Point(81, 56);
      this.txtSearchFor.Name = "txtSearchFor";
      this.txtSearchFor.Size = new System.Drawing.Size(159, 20);
      this.txtSearchFor.TabIndex = 5;
      // 
      // btnSearch
      // 
      this.btnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnSearch.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.btnSearch.Location = new System.Drawing.Point(549, 22);
      this.btnSearch.Name = "btnSearch";
      this.btnSearch.Size = new System.Drawing.Size(64, 21);
      this.btnSearch.TabIndex = 4;
      this.btnSearch.Text = "Search";
      this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
      // 
      // listView1
      // 
      this.listView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.listView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.DisplayName,
            this.Value,
            this.ToolTip,
            this.SequenceStep,
            this.OperatingSeq});
      this.listView1.LabelWrap = false;
      this.listView1.Location = new System.Drawing.Point(0, 95);
      this.listView1.Name = "listView1";
      this.listView1.Size = new System.Drawing.Size(619, 256);
      this.listView1.TabIndex = 9;
      this.listView1.UseCompatibleStateImageBehavior = false;
      this.listView1.View = System.Windows.Forms.View.Details;
      this.listView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.listView1_ColumnClick);
      this.listView1.SelectedIndexChanged += new System.EventHandler(this.listView1_SelectedIndexChanged);
      // 
      // DisplayName
      // 
      this.DisplayName.Text = "Name";
      this.DisplayName.Width = 160;
      // 
      // Value
      // 
      this.Value.Text = "Value";
      this.Value.Width = 90;
      // 
      // ToolTip
      // 
      this.ToolTip.Text = "ToolTip";
      this.ToolTip.Width = 283;
      // 
      // SequenceStep
      // 
      this.SequenceStep.Text = "SequenceStep";
      this.SequenceStep.Width = 122;
      // 
      // OperatingSeq
      // 
      this.OperatingSeq.Text = "OperatingSeq";
      this.OperatingSeq.Width = 122;
      // 
      // comboBoxFilterBySeq
      // 
      this.comboBoxFilterBySeq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxFilterBySeq.FormattingEnabled = true;
      this.comboBoxFilterBySeq.Location = new System.Drawing.Point(81, 3);
      this.comboBoxFilterBySeq.Name = "comboBoxFilterBySeq";
      this.comboBoxFilterBySeq.Size = new System.Drawing.Size(159, 21);
      this.comboBoxFilterBySeq.TabIndex = 10;
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(6, 6);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(69, 16);
      this.label1.TabIndex = 11;
      this.label1.Text = "Filter by Seq:";
      this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // checkBoxName
      // 
      this.checkBoxName.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxName.AutoSize = true;
      this.checkBoxName.Checked = true;
      this.checkBoxName.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxName.Location = new System.Drawing.Point(247, 18);
      this.checkBoxName.Name = "checkBoxName";
      this.checkBoxName.Size = new System.Drawing.Size(54, 17);
      this.checkBoxName.TabIndex = 12;
      this.checkBoxName.Text = "Name";
      this.checkBoxName.UseVisualStyleBackColor = true;
      // 
      // checkBoxValue
      // 
      this.checkBoxValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxValue.AutoSize = true;
      this.checkBoxValue.Checked = true;
      this.checkBoxValue.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxValue.Location = new System.Drawing.Point(247, 33);
      this.checkBoxValue.Name = "checkBoxValue";
      this.checkBoxValue.Size = new System.Drawing.Size(53, 17);
      this.checkBoxValue.TabIndex = 13;
      this.checkBoxValue.Text = "Value";
      this.checkBoxValue.UseVisualStyleBackColor = true;
      // 
      // checkBoxToolTip
      // 
      this.checkBoxToolTip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxToolTip.AutoSize = true;
      this.checkBoxToolTip.Checked = true;
      this.checkBoxToolTip.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxToolTip.Location = new System.Drawing.Point(303, 18);
      this.checkBoxToolTip.Name = "checkBoxToolTip";
      this.checkBoxToolTip.Size = new System.Drawing.Size(62, 17);
      this.checkBoxToolTip.TabIndex = 14;
      this.checkBoxToolTip.Text = "ToolTip";
      this.checkBoxToolTip.UseVisualStyleBackColor = true;
      // 
      // checkBoxSequenceStep
      // 
      this.checkBoxSequenceStep.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.checkBoxSequenceStep.AutoSize = true;
      this.checkBoxSequenceStep.Checked = true;
      this.checkBoxSequenceStep.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxSequenceStep.Location = new System.Drawing.Point(303, 33);
      this.checkBoxSequenceStep.Name = "checkBoxSequenceStep";
      this.checkBoxSequenceStep.Size = new System.Drawing.Size(97, 17);
      this.checkBoxSequenceStep.TabIndex = 15;
      this.checkBoxSequenceStep.Text = "SequenceStep";
      this.checkBoxSequenceStep.UseVisualStyleBackColor = true;
      // 
      // label2
      // 
      this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
      this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label2.Location = new System.Drawing.Point(8, 13);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(50, 16);
      this.label2.TabIndex = 16;
      this.label2.Text = "Control";
      this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label3
      // 
      this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(190)))), ((int)(((byte)(255)))));
      this.label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label3.Location = new System.Drawing.Point(8, 34);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(36, 16);
      this.label3.TabIndex = 17;
      this.label3.Text = "Mode";
      this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label4
      // 
      this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(190)))), ((int)(((byte)(190)))));
      this.label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label4.Location = new System.Drawing.Point(60, 13);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(52, 16);
      this.label4.TabIndex = 18;
      this.label4.Text = "Pressure";
      this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // label5
      // 
      this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(190)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
      this.label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label5.Location = new System.Drawing.Point(46, 34);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(32, 16);
      this.label5.TabIndex = 19;
      this.label5.Text = "Time";
      this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // label6
      // 
      this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(190)))));
      this.label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.label6.Location = new System.Drawing.Point(80, 34);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(32, 16);
      this.label6.TabIndex = 20;
      this.label6.Text = "Flow";
      this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
      // 
      // IncludeInSearchGroupBox
      // 
      this.IncludeInSearchGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.IncludeInSearchGroupBox.Location = new System.Drawing.Point(238, 3);
      this.IncludeInSearchGroupBox.Name = "IncludeInSearchGroupBox";
      this.IncludeInSearchGroupBox.Size = new System.Drawing.Size(162, 47);
      this.IncludeInSearchGroupBox.TabIndex = 21;
      this.IncludeInSearchGroupBox.TabStop = false;
      this.IncludeInSearchGroupBox.Text = "Include in Search";
      // 
      // SideGroupBox
      // 
      this.SideGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.SideGroupBox.Controls.Add(this.checkBoxWhite);
      this.SideGroupBox.Controls.Add(this.checkBoxBlue);
      this.SideGroupBox.Location = new System.Drawing.Point(406, 3);
      this.SideGroupBox.Name = "SideGroupBox";
      this.SideGroupBox.Size = new System.Drawing.Size(80, 47);
      this.SideGroupBox.TabIndex = 22;
      this.SideGroupBox.TabStop = false;
      this.SideGroupBox.Text = "Include Side";
      // 
      // checkBoxWhite
      // 
      this.checkBoxWhite.AutoSize = true;
      this.checkBoxWhite.Checked = true;
      this.checkBoxWhite.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxWhite.Location = new System.Drawing.Point(6, 30);
      this.checkBoxWhite.Name = "checkBoxWhite";
      this.checkBoxWhite.Size = new System.Drawing.Size(54, 17);
      this.checkBoxWhite.TabIndex = 23;
      this.checkBoxWhite.Text = "White";
      this.checkBoxWhite.UseVisualStyleBackColor = true;
      // 
      // checkBoxBlue
      // 
      this.checkBoxBlue.AutoSize = true;
      this.checkBoxBlue.Checked = true;
      this.checkBoxBlue.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxBlue.Location = new System.Drawing.Point(6, 15);
      this.checkBoxBlue.Name = "checkBoxBlue";
      this.checkBoxBlue.Size = new System.Drawing.Size(47, 17);
      this.checkBoxBlue.TabIndex = 23;
      this.checkBoxBlue.Text = "Blue";
      this.checkBoxBlue.UseVisualStyleBackColor = true;
      // 
      // label7
      // 
      this.label7.Location = new System.Drawing.Point(-4, 31);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(79, 18);
      this.label7.TabIndex = 23;
      this.label7.Text = "    by Op Seq:";
      this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // comboBoxFilterByOpSeq
      // 
      this.comboBoxFilterByOpSeq.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxFilterByOpSeq.FormattingEnabled = true;
      this.comboBoxFilterByOpSeq.Location = new System.Drawing.Point(81, 28);
      this.comboBoxFilterByOpSeq.Name = "comboBoxFilterByOpSeq";
      this.comboBoxFilterByOpSeq.Size = new System.Drawing.Size(159, 21);
      this.comboBoxFilterByOpSeq.TabIndex = 24;
      // 
      // buttonCopyBlueToWhite
      // 
      this.buttonCopyBlueToWhite.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCopyBlueToWhite.Location = new System.Drawing.Point(259, 59);
      this.buttonCopyBlueToWhite.Name = "buttonCopyBlueToWhite";
      this.buttonCopyBlueToWhite.Size = new System.Drawing.Size(106, 23);
      this.buttonCopyBlueToWhite.TabIndex = 25;
      this.buttonCopyBlueToWhite.Text = "Copy Blue to White";
      this.buttonCopyBlueToWhite.UseVisualStyleBackColor = true;
      this.buttonCopyBlueToWhite.Click += new System.EventHandler(this.buttonCopyBlueToWhite_Click);
      // 
      // buttonCopyWhiteToBlue
      // 
      this.buttonCopyWhiteToBlue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonCopyWhiteToBlue.Location = new System.Drawing.Point(380, 59);
      this.buttonCopyWhiteToBlue.Name = "buttonCopyWhiteToBlue";
      this.buttonCopyWhiteToBlue.Size = new System.Drawing.Size(106, 23);
      this.buttonCopyWhiteToBlue.TabIndex = 26;
      this.buttonCopyWhiteToBlue.Text = "Copy White to Blue";
      this.buttonCopyWhiteToBlue.UseVisualStyleBackColor = true;
      this.buttonCopyWhiteToBlue.Click += new System.EventHandler(this.buttonCopyWhiteToBlue_Click);
      // 
      // LegendGroupBox
      // 
      this.LegendGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.LegendGroupBox.Controls.Add(this.label3);
      this.LegendGroupBox.Controls.Add(this.label6);
      this.LegendGroupBox.Controls.Add(this.label4);
      this.LegendGroupBox.Controls.Add(this.label5);
      this.LegendGroupBox.Controls.Add(this.label2);
      this.LegendGroupBox.Location = new System.Drawing.Point(497, 38);
      this.LegendGroupBox.Name = "LegendGroupBox";
      this.LegendGroupBox.Size = new System.Drawing.Size(116, 55);
      this.LegendGroupBox.TabIndex = 27;
      this.LegendGroupBox.TabStop = false;
      this.LegendGroupBox.Text = "Legend";
      // 
      // EditCycleSearchForm
      // 
      this.AcceptButton = this.btnSearch;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(619, 370);
      this.ControlBox = false;
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnSearch);
      this.Controls.Add(this.LegendGroupBox);
      this.Controls.Add(this.buttonCopyWhiteToBlue);
      this.Controls.Add(this.buttonCopyBlueToWhite);
      this.Controls.Add(this.comboBoxFilterByOpSeq);
      this.Controls.Add(this.label7);
      this.Controls.Add(this.checkBoxSequenceStep);
      this.Controls.Add(this.SideGroupBox);
      this.Controls.Add(this.checkBoxValue);
      this.Controls.Add(this.checkBoxName);
      this.Controls.Add(this.checkBoxToolTip);
      this.Controls.Add(this.IncludeInSearchGroupBox);
      this.Controls.Add(this.label1);
      this.Controls.Add(this.comboBoxFilterBySeq);
      this.Controls.Add(this.listView1);
      this.Controls.Add(this.lblText);
      this.Controls.Add(this.txtSearchFor);
      this.DoubleBuffered = true;
      this.MinimumSize = new System.Drawing.Size(435, 386);
      this.Name = "EditCycleSearchForm";
      this.Text = "Edit Cycle Search";
      this.Load += new System.EventHandler(this.frmInputBox_Load);
      this.SideGroupBox.ResumeLayout(false);
      this.SideGroupBox.PerformLayout();
      this.LegendGroupBox.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblText;
        private System.Windows.Forms.TextBox txtSearchFor;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ColumnHeader DisplayName;
        private System.Windows.Forms.ColumnHeader Value;
        private System.Windows.Forms.ColumnHeader ToolTip;
        private System.Windows.Forms.ComboBox comboBoxFilterBySeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ColumnHeader SequenceStep;
        private System.Windows.Forms.CheckBox checkBoxName;
        private System.Windows.Forms.CheckBox checkBoxValue;
        private System.Windows.Forms.CheckBox checkBoxToolTip;
        private System.Windows.Forms.CheckBox checkBoxSequenceStep;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox IncludeInSearchGroupBox;
        private System.Windows.Forms.GroupBox SideGroupBox;
        private System.Windows.Forms.CheckBox checkBoxWhite;
        private System.Windows.Forms.CheckBox checkBoxBlue;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox comboBoxFilterByOpSeq;
        private System.Windows.Forms.ColumnHeader OperatingSeq;
        private System.Windows.Forms.Button buttonCopyBlueToWhite;
        private System.Windows.Forms.Button buttonCopyWhiteToBlue;
        private System.Windows.Forms.GroupBox LegendGroupBox;
    }
}