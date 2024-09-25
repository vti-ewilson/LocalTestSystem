namespace VTIWindowsControlLibrary.Components
{
    partial class SystemSignalControl
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
            this.labelCaption = new System.Windows.Forms.Label();
            this.horizSignalIndicatorValue = new VTIWindowsControlLibrary.Components.HorizSignalIndicator();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            this.labelCaption.AutoSize = true;
            this.labelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelCaption.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelCaption.ForeColor = System.Drawing.Color.Cyan;
            this.labelCaption.Location = new System.Drawing.Point(0, 0);
            this.labelCaption.MaximumSize = new System.Drawing.Size(100, 30);
            this.labelCaption.MinimumSize = new System.Drawing.Size(100, 15);
            this.labelCaption.Name = "labelCaption";
            this.labelCaption.Size = new System.Drawing.Size(100, 16);
            this.labelCaption.TabIndex = 0;
            this.labelCaption.Text = "System Signal";
            this.labelCaption.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // horizSignalIndicatorValue
            // 
            this.horizSignalIndicatorValue.Dock = System.Windows.Forms.DockStyle.Top;
            this.horizSignalIndicatorValue.ForeColor = System.Drawing.Color.Yellow;
            this.horizSignalIndicatorValue.IndicatorColor = System.Drawing.Color.Red;
            this.horizSignalIndicatorValue.LinMax = 100F;
            this.horizSignalIndicatorValue.LinMin = 0F;
            this.horizSignalIndicatorValue.Location = new System.Drawing.Point(0, 16);
            this.horizSignalIndicatorValue.LogMaxExp = -2;
            this.horizSignalIndicatorValue.LogMinExp = -7;
            this.horizSignalIndicatorValue.Name = "horizSignalIndicatorValue";
            this.horizSignalIndicatorValue.SemiLog = true;
            this.horizSignalIndicatorValue.Size = new System.Drawing.Size(100, 18);
            this.horizSignalIndicatorValue.TabIndex = 1;
            this.horizSignalIndicatorValue.Text = "INITIALIZING";
            this.horizSignalIndicatorValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.horizSignalIndicatorValue.Value = 0F;
            // 
            // SystemSignalControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.horizSignalIndicatorValue);
            this.Controls.Add(this.labelCaption);
            this.MinimumSize = new System.Drawing.Size(100, 23);
            this.Name = "SystemSignalControl";
            this.Size = new System.Drawing.Size(100, 34);
            this.Resize += new System.EventHandler(this.SystemSignalControl_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label labelCaption;
        public HorizSignalIndicator horizSignalIndicatorValue;
    }
}
