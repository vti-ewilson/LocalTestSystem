namespace VTIWindowsControlLibrary.Components
{
    partial class SimpleDockControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SimpleDockControl));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabelCaption = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButtonUndock = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabelCaption,
            this.toolStripButtonUndock});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(476, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabelCaption
            // 
            this.toolStripLabelCaption.Name = "toolStripLabelCaption";
            this.toolStripLabelCaption.Size = new System.Drawing.Size(128, 22);
            this.toolStripLabelCaption.Text = "Dockable Control Caption";
            // 
            // toolStripButtonUndock
            // 
            this.toolStripButtonUndock.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonUndock.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonUndock.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonUndock.Image")));
            this.toolStripButtonUndock.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonUndock.Name = "toolStripButtonUndock";
            this.toolStripButtonUndock.Size = new System.Drawing.Size(46, 22);
            this.toolStripButtonUndock.Text = "Undock";
            this.toolStripButtonUndock.Click += new System.EventHandler(this.toolStripButtonUndock_Click);
            // 
            // DockControl2
            // 
            this.Controls.Add(this.toolStrip1);
            this.Name = "DockControl2";
            this.Size = new System.Drawing.Size(476, 333);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabelCaption;
        private System.Windows.Forms.ToolStripButton toolStripButtonUndock;
    }
}
