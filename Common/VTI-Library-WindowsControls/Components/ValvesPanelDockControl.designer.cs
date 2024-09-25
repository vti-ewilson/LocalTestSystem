namespace VTIWindowsControlLibrary.Components
{
  partial class ValvesPanelDockControl
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
            this.components = new System.ComponentModel.Container();
            this.ValvesPanelControl1 = new VTIWindowsControlLibrary.Components.ValvesPanelControl();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.SuspendLayout();
            // 
            // ValvesPanelControl1
            // 
            this.ValvesPanelControl1.Columns = 1;
            this.ValvesPanelControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.ValvesPanelControl1.Location = new System.Drawing.Point(0, 25);
            this.ValvesPanelControl1.Margin = new System.Windows.Forms.Padding(0);
            this.ValvesPanelControl1.Name = "ValvesPanelControl1";
            this.ValvesPanelControl1.Rows = 10;
            this.ValvesPanelControl1.Size = new System.Drawing.Size(146, 163);
            this.ValvesPanelControl1.TabIndex = 1;
            // 
            // ValvesPanelDockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Caption = "Valves Panel";
            this.Controls.Add(this.ValvesPanelControl1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "ValvesPanelDockControl";
            this.Size = new System.Drawing.Size(146, 188);
            this.UndockFrameAutoSize = true;
            this.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.UndockFrameMaximizeBox = false;
            this.UndockFrameMinimizeBox = false;
            this.UndockFrameTopMost = true;
            this.Controls.SetChildIndex(this.ValvesPanelControl1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ValvesPanelControl ValvesPanelControl1;
        private System.Windows.Forms.ToolTip toolTip1;

    }
}
