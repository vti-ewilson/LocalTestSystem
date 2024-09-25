namespace VTIWindowsControlLibrary.Components.Configuration
{
    partial class NumericParameterControl
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
            this.textBoxDescription = new System.Windows.Forms.TextBox();
            this.labelMaxValue = new System.Windows.Forms.Label();
            this.labelMaxValueCap = new System.Windows.Forms.Label();
            this.labelMinValue = new System.Windows.Forms.Label();
            this.labelMinValueCap = new System.Windows.Forms.Label();
            this.labelProcessValue = new System.Windows.Forms.Label();
            this.labelProcessValueCap = new System.Windows.Forms.Label();
            this.numericSlider1 = new VTIWindowsControlLibrary.Components.NumericSlider();
            this.groupBoxParameter.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.Controls.Add(this.numericSlider1);
            this.groupBoxParameter.Controls.Add(this.textBoxDescription);
            this.groupBoxParameter.Controls.Add(this.labelMaxValue);
            this.groupBoxParameter.Controls.Add(this.labelMaxValueCap);
            this.groupBoxParameter.Controls.Add(this.labelMinValue);
            this.groupBoxParameter.Controls.Add(this.labelMinValueCap);
            this.groupBoxParameter.Controls.Add(this.labelProcessValue);
            this.groupBoxParameter.Controls.Add(this.labelProcessValueCap);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 0);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.TabIndex = 2;
            this.groupBoxParameter.TabStop = false;
            // 
            // textBoxDescription
            // 
            this.textBoxDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxDescription.Location = new System.Drawing.Point(221, 69);
            this.textBoxDescription.Multiline = true;
            this.textBoxDescription.Name = "textBoxDescription";
            this.textBoxDescription.ReadOnly = true;
            this.textBoxDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxDescription.Size = new System.Drawing.Size(291, 60);
            this.textBoxDescription.TabIndex = 11;
            // 
            // labelMaxValue
            // 
            this.labelMaxValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxValue.ForeColor = System.Drawing.Color.Blue;
            this.labelMaxValue.Location = new System.Drawing.Point(115, 109);
            this.labelMaxValue.Name = "labelMaxValue";
            this.labelMaxValue.Size = new System.Drawing.Size(100, 20);
            this.labelMaxValue.TabIndex = 9;
            this.labelMaxValue.Text = "99.9E+99";
            this.labelMaxValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelMaxValueCap
            // 
            this.labelMaxValueCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMaxValueCap.Location = new System.Drawing.Point(9, 109);
            this.labelMaxValueCap.Name = "labelMaxValueCap";
            this.labelMaxValueCap.Size = new System.Drawing.Size(100, 18);
            this.labelMaxValueCap.TabIndex = 8;
            this.labelMaxValueCap.Text = "MAXIMUM";
            this.labelMaxValueCap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelMinValue
            // 
            this.labelMinValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinValue.ForeColor = System.Drawing.Color.Blue;
            this.labelMinValue.Location = new System.Drawing.Point(115, 89);
            this.labelMinValue.Name = "labelMinValue";
            this.labelMinValue.Size = new System.Drawing.Size(100, 20);
            this.labelMinValue.TabIndex = 7;
            this.labelMinValue.Text = "99.9E+99";
            this.labelMinValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelMinValueCap
            // 
            this.labelMinValueCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelMinValueCap.Location = new System.Drawing.Point(9, 89);
            this.labelMinValueCap.Name = "labelMinValueCap";
            this.labelMinValueCap.Size = new System.Drawing.Size(100, 18);
            this.labelMinValueCap.TabIndex = 6;
            this.labelMinValueCap.Text = "MINIMUM";
            this.labelMinValueCap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelProcessValue
            // 
            this.labelProcessValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessValue.ForeColor = System.Drawing.Color.Blue;
            this.labelProcessValue.Location = new System.Drawing.Point(115, 69);
            this.labelProcessValue.Name = "labelProcessValue";
            this.labelProcessValue.Size = new System.Drawing.Size(100, 20);
            this.labelProcessValue.TabIndex = 5;
            this.labelProcessValue.Text = "99.9E+99";
            this.labelProcessValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // labelProcessValueCap
            // 
            this.labelProcessValueCap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelProcessValueCap.Location = new System.Drawing.Point(9, 69);
            this.labelProcessValueCap.Name = "labelProcessValueCap";
            this.labelProcessValueCap.Size = new System.Drawing.Size(100, 18);
            this.labelProcessValueCap.TabIndex = 4;
            this.labelProcessValueCap.Text = "PROCESS";
            this.labelProcessValueCap.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // numericSlider1
            // 
            this.numericSlider1.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.numericSlider1.LargeChange = 10;
            this.numericSlider1.Location = new System.Drawing.Point(6, 16);
            this.numericSlider1.Maximum = 100;
            this.numericSlider1.Minimum = 0;
            this.numericSlider1.Name = "numericSlider1";
            this.numericSlider1.Size = new System.Drawing.Size(503, 47);
            this.numericSlider1.SliderOnLeft = false;
            this.numericSlider1.SmallChange = 1;
            this.numericSlider1.SplitterDistance = 278;
            this.numericSlider1.TabIndex = 13;
            this.numericSlider1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericSlider1.TextBoxColor = System.Drawing.SystemColors.WindowText;
            this.numericSlider1.Value = 0;
            this.numericSlider1.ValueChanged += new System.EventHandler(this.numericSlider1_ValueChanged);
            // 
            // NumericParameterControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxParameter);
            this.Name = "NumericParameterControl";
            this.Size = new System.Drawing.Size(518, 138);
            this.groupBoxParameter.ResumeLayout(false);
            this.groupBoxParameter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxParameter;
        private System.Windows.Forms.TextBox textBoxDescription;
        private System.Windows.Forms.Label labelMaxValue;
        private System.Windows.Forms.Label labelMaxValueCap;
        private System.Windows.Forms.Label labelMinValue;
        private System.Windows.Forms.Label labelMinValueCap;
        private System.Windows.Forms.Label labelProcessValue;
        private System.Windows.Forms.Label labelProcessValueCap;
        private NumericSlider numericSlider1;

    }
}
