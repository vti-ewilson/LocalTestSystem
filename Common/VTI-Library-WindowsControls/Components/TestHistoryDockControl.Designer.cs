namespace VTIWindowsControlLibrary.Components
{
    partial class TestHistoryDockControl
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
            this.testHistoryControl1 = new VTIWindowsControlLibrary.Components.TestHistoryControl();
            this.SuspendLayout();
            // 
            // testHistoryControl1
            // 
            this.testHistoryControl1.Columns = 1;
            this.testHistoryControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryControl1.Location = new System.Drawing.Point(0, 25);
            this.testHistoryControl1.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryControl1.Name = "testHistoryControl1";
            this.testHistoryControl1.Rows = 10;
            this.testHistoryControl1.Size = new System.Drawing.Size(146, 163);
            this.testHistoryControl1.TabIndex = 1;
            // 
            // TestHistoryDockControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Caption = "Test History";
            this.Controls.Add(this.testHistoryControl1);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TestHistoryDockControl";
            this.Size = new System.Drawing.Size(146, 188);
            this.UndockFrameAutoSize = true;
            this.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.UndockFrameMaximizeBox = false;
            this.UndockFrameMinimizeBox = false;
            this.UndockFrameTopMost = true;
            this.Controls.SetChildIndex(this.testHistoryControl1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TestHistoryControl testHistoryControl1;

    }
}
