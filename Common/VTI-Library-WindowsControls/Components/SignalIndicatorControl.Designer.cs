namespace VTIWindowsControlLibrary.Components
{
    partial class SignalIndicatorControl
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
            this.panelCaption = new System.Windows.Forms.Panel();
            this.buttonHide = new System.Windows.Forms.Button();
            this.lblCaption = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelIndicator = new System.Windows.Forms.Panel();
            this.panelCaption.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCaption
            // 
            this.panelCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelCaption.Controls.Add(this.buttonHide);
            this.panelCaption.Controls.Add(this.lblCaption);
            this.panelCaption.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelCaption.Location = new System.Drawing.Point(0, 0);
            this.panelCaption.Name = "panelCaption";
            this.panelCaption.Size = new System.Drawing.Size(81, 35);
            this.panelCaption.TabIndex = 8;
            // 
            // buttonHide
            // 
            this.buttonHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonHide.BackgroundImage = global::VTIWindowsControlLibrary.Properties.Resources.CloseToolbar;
            this.buttonHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHide.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonHide.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonHide.Location = new System.Drawing.Point(58, 0);
            this.buttonHide.Margin = new System.Windows.Forms.Padding(0);
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.Size = new System.Drawing.Size(19, 19);
            this.buttonHide.TabIndex = 9;
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // lblCaption
            // 
            this.lblCaption.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCaption.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(0, 0);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
            this.lblCaption.Size = new System.Drawing.Size(77, 31);
            this.lblCaption.TabIndex = 8;
            this.lblCaption.Text = "LD Signal";
            this.lblCaption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.panelIndicator);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(5);
            this.panel2.Size = new System.Drawing.Size(81, 306);
            this.panel2.TabIndex = 8;
            // 
            // panelIndicator
            // 
            this.panelIndicator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelIndicator.Location = new System.Drawing.Point(5, 5);
            this.panelIndicator.Name = "panelIndicator";
            this.panelIndicator.Size = new System.Drawing.Size(67, 292);
            this.panelIndicator.TabIndex = 0;
            // 
            // SignalIndicatorControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelCaption);
            this.Name = "SignalIndicatorControl";
            this.Size = new System.Drawing.Size(81, 341);
            this.panelCaption.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCaption;
        private System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.Label lblCaption;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelIndicator;

    }
}
