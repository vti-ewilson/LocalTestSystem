namespace VTIWindowsControlLibrary.Components.Configuration
{
    partial class BooleanParameterControl
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
            this.checkBoxEnable = new System.Windows.Forms.CheckBox();
            this.labelDescriptionTallCap = new System.Windows.Forms.Label();
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.groupBoxParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.Controls.Add(this.checkBoxEnable);
            this.groupBoxParameter.Controls.Add(this.labelDescriptionTallCap);
            this.groupBoxParameter.Controls.Add(this.textBoxDescription);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.TabIndex = 2;
            this.groupBoxParameter.TabStop = false;
            // 
            // checkBoxEnable
            // 
            this.checkBoxEnable.Appearance = System.Windows.Forms.Appearance.Button;
            this.checkBoxEnable.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxEnable.Image = global::VTIWindowsControlLibrary.Properties.Resources.Red_X;
            this.checkBoxEnable.Location = new System.Drawing.Point(66, 31);
            this.checkBoxEnable.Name = "checkBoxEnable";
            this.checkBoxEnable.Size = new System.Drawing.Size(80, 80);
            this.checkBoxEnable.TabIndex = 17;
            this.checkBoxEnable.Text = "DISABLED";
            this.checkBoxEnable.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.checkBoxEnable.UseVisualStyleBackColor = true;
            this.checkBoxEnable.Leave += new System.EventHandler(this.checkBoxEnable_Leave);
            this.checkBoxEnable.CheckedChanged += new System.EventHandler(this.checkBoxEnable_CheckedChanged);
            // 
            // labelDescriptionTallCap
            // 
            this.labelDescriptionTallCap.AutoSize = true;
            this.labelDescriptionTallCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelDescriptionTallCap.Location = new System.Drawing.Point(221, 18);
            this.labelDescriptionTallCap.Name = "labelDescriptionTallCap";
            this.labelDescriptionTallCap.Size = new System.Drawing.Size(129, 20);
            this.labelDescriptionTallCap.TabIndex = 13;
            this.labelDescriptionTallCap.Text = "DESCRIPTION";
            this.labelDescriptionTallCap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(221, 38);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(294, 91);
            this.textBoxDescription.TabIndex = 14;
            // 
            // BooleanParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParameter);
            this.Name = "BooleanParameterControl";
            this.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.ResumeLayout(false);
            this.groupBoxParameter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParameter;
        private System.Windows.Forms.CheckBox checkBoxEnable;
        private System.Windows.Forms.Label labelDescriptionTallCap;
        private System.Windows.Forms.TextBox textBoxDescription;
    }
}
