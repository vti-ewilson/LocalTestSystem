namespace VTIWindowsControlLibrary.Components
{
    partial class LightedLabel
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
            this.labelText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelText
            // 
            this.labelText.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.labelText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelText.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelText.ForeColor = System.Drawing.Color.Black;
            this.labelText.Location = new System.Drawing.Point(0, 0);
            this.labelText.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelText.Name = "labelText";
            this.labelText.Size = new System.Drawing.Size(70, 16);
            this.labelText.TabIndex = 28;
            this.labelText.Text = "TEXT";
            this.labelText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // LightedLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelText);
            this.Name = "LightedLabel";
            this.Size = new System.Drawing.Size(70, 16);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelText;
    }
}
