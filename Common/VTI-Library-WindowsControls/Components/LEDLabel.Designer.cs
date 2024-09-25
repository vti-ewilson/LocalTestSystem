namespace VTIWindowsControlLibrary.Components
{
    partial class LEDLabel
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
            this.label1 = new System.Windows.Forms.Label();
            this.ovalControl1 = new VTIWindowsControlLibrary.Components.OvalControl();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "LED Label";
            // 
            // ovalControl1
            // 
            this.ovalControl1.BorderColor = System.Drawing.Color.DimGray;
            this.ovalControl1.BorderWidth = 0;
            this.ovalControl1.DrawFilled = true;
            this.ovalControl1.FillColor = System.Drawing.Color.Red;
            this.ovalControl1.Location = new System.Drawing.Point(3, 3);
            this.ovalControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ovalControl1.Name = "ovalControl1";
            this.ovalControl1.Size = new System.Drawing.Size(15, 15);
            this.ovalControl1.TabIndex = 2;
            // 
            // LEDLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.ovalControl1);
            this.Controls.Add(this.label1);
            this.Name = "LEDLabel";
            this.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.Size = new System.Drawing.Size(86, 22);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private OvalControl ovalControl1;
    }
}
