namespace VTIWindowsControlLibrary.Forms
{
    partial class ConfigBackupSelectForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.OKButton = new System.Windows.Forms.Button();
            this.BackupFilesListBox = new System.Windows.Forms.CheckedListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.configGoodFilesListBox = new System.Windows.Forms.CheckedListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.MaximumSize = new System.Drawing.Size(270, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(269, 96);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select an automatically-created backup config file from one of the dates listed b" +
    "elow.";
            // 
            // OKButton
            // 
            this.OKButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OKButton.Location = new System.Drawing.Point(486, 372);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(114, 67);
            this.OKButton.TabIndex = 2;
            this.OKButton.Text = "OK";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // BackupFilesListBox
            // 
            this.BackupFilesListBox.CheckOnClick = true;
            this.BackupFilesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BackupFilesListBox.FormattingEnabled = true;
            this.BackupFilesListBox.HorizontalScrollbar = true;
            this.BackupFilesListBox.Location = new System.Drawing.Point(16, 120);
            this.BackupFilesListBox.Name = "BackupFilesListBox";
            this.BackupFilesListBox.Size = new System.Drawing.Size(264, 319);
            this.BackupFilesListBox.TabIndex = 3;
            this.BackupFilesListBox.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.BackupFilesListBox_ItemCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(275, 26);
            this.label2.MaximumSize = new System.Drawing.Size(300, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 24);
            this.label2.TabIndex = 4;
            this.label2.Text = "OR";
            // 
            // configGoodFilesListBox
            // 
            this.configGoodFilesListBox.CheckOnClick = true;
            this.configGoodFilesListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.configGoodFilesListBox.FormattingEnabled = true;
            this.configGoodFilesListBox.HorizontalScrollbar = true;
            this.configGoodFilesListBox.Location = new System.Drawing.Point(336, 99);
            this.configGoodFilesListBox.Name = "configGoodFilesListBox";
            this.configGoodFilesListBox.Size = new System.Drawing.Size(264, 256);
            this.configGoodFilesListBox.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(332, 9);
            this.label3.MaximumSize = new System.Drawing.Size(250, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(241, 72);
            this.label3.TabIndex = 6;
            this.label3.Text = "Select a manually saved config file from the list below. ";
            // 
            // ConfigBackupSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(611, 450);
            this.ControlBox = false;
            this.Controls.Add(this.label3);
            this.Controls.Add(this.configGoodFilesListBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BackupFilesListBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.label1);
            this.Name = "ConfigBackupSelectForm";
            this.Text = "Config Backup Select Form";
            this.VisibleChanged += new System.EventHandler(this.ConfigBackupSelectForm_VisibleChanged);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.CheckedListBox BackupFilesListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckedListBox configGoodFilesListBox;
        private System.Windows.Forms.Label label3;
    }
}