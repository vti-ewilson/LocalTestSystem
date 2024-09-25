namespace VTIWindowsControlLibrary.Components.Graphing
{
    partial class CommentControl
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
            this.pictureBoxCallout = new System.Windows.Forms.PictureBox();
            this.textBox1 = new VTIWindowsControlLibrary.Components.AutoSizeTextBox();
            this.lineControl1 = new VTIWindowsControlLibrary.Components.LineControl();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCallout)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxCallout
            // 
            this.pictureBoxCallout.Cursor = System.Windows.Forms.Cursors.SizeAll;
            this.pictureBoxCallout.Image = global::VTIWindowsControlLibrary.Properties.Resources.callout;
            this.pictureBoxCallout.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxCallout.Name = "pictureBoxCallout";
            this.pictureBoxCallout.Size = new System.Drawing.Size(5, 5);
            this.pictureBoxCallout.TabIndex = 4;
            this.pictureBoxCallout.TabStop = false;
            this.pictureBoxCallout.MouseLeave += new System.EventHandler(this.pictureBoxCallout_MouseLeave);
            this.pictureBoxCallout.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCallout_MouseMove);
            this.pictureBoxCallout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCallout_MouseDown);
            this.pictureBoxCallout.MouseUp += new System.Windows.Forms.MouseEventHandler(this.pictureBoxCallout_MouseUp);
            this.pictureBoxCallout.MouseEnter += new System.EventHandler(this.pictureBoxCallout_MouseEnter);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Info;
            this.textBox1.Location = new System.Drawing.Point(0, 36);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 19);
            this.textBox1.TabIndex = 3;
            this.textBox1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseMove);
            this.textBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseDown);
            this.textBox1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseUp);
            this.textBox1.SizeChanged += new System.EventHandler(this.textBox1_SizeChanged);
            // 
            // lineControl1
            // 
            this.lineControl1.BackColor = System.Drawing.SystemColors.Control;
            this.lineControl1.EndPoint = new System.Drawing.Point(50, 50);
            this.lineControl1.Location = new System.Drawing.Point(2, 2);
            this.lineControl1.Name = "lineControl1";
            this.lineControl1.Size = new System.Drawing.Size(49, 49);
            this.lineControl1.StartPoint = new System.Drawing.Point(2, 2);
            this.lineControl1.TabIndex = 2;
            this.lineControl1.Text = "lineControl21";
            // 
            // CommentControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pictureBoxCallout);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lineControl1);
            this.DoubleBuffered = true;
            this.Name = "CommentControl";
            this.Size = new System.Drawing.Size(100, 55);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCallout)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private LineControl lineControl1;
        private AutoSizeTextBox textBox1;
        private System.Windows.Forms.PictureBox pictureBoxCallout;
    }
}
