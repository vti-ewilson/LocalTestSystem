﻿namespace VTIWindowsControlLibrary.Components.Configuration
{
    partial class DropDownParameterControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxParameter = new System.Windows.Forms.GroupBox();
            this.comboBoxProcessValue = new System.Windows.Forms.ComboBox();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelDescriptionWideCap = new System.Windows.Forms.Label();
            this.groupBoxParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.Controls.Add(this.comboBoxProcessValue);
            this.groupBoxParameter.Controls.Add(this.textBoxDescription);
            this.groupBoxParameter.Controls.Add(this.labelDescriptionWideCap);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.TabIndex = 2;
            this.groupBoxParameter.TabStop = false;
            // 
            // comboBoxProcessValue
            // 
            this.comboBoxProcessValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxProcessValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBoxProcessValue.FormattingEnabled = true;
            this.comboBoxProcessValue.Location = new System.Drawing.Point(9, 20);
            this.comboBoxProcessValue.Name = "comboBoxProcessValue";
            this.comboBoxProcessValue.Size = new System.Drawing.Size(503, 37);
            this.comboBoxProcessValue.TabIndex = 18;
            this.comboBoxProcessValue.TextChanged += new System.EventHandler(this.comboBoxProcessValue_TextChanged);
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(9, 89);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(503, 40);
            this.textBoxDescription.TabIndex = 16;
            // 
            // labelDescriptionWideCap
            // 
            this.labelDescriptionWideCap.AutoSize = true;
            this.labelDescriptionWideCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescriptionWideCap.Location = new System.Drawing.Point(9, 69);
            this.labelDescriptionWideCap.Name = "labelDescriptionWideCap";
            this.labelDescriptionWideCap.Size = new System.Drawing.Size(129, 20);
            this.labelDescriptionWideCap.TabIndex = 15;
            this.labelDescriptionWideCap.Text = "DESCRIPTION";
            this.labelDescriptionWideCap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // DropDownParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParameter);
            this.Name = "DropDownParameterControl";
            this.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.ResumeLayout(false);
            this.groupBoxParameter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParameter;
        private System.Windows.Forms.ComboBox comboBoxProcessValue;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelDescriptionWideCap;
    }
}