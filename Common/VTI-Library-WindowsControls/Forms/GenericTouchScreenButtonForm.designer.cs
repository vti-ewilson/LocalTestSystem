namespace VTIWindowsControlLibrary.Forms
{
    partial class TouchScreenButtonForm<T>
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
          this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
          this.panel1 = new System.Windows.Forms.Panel();
          this.buttonDown = new System.Windows.Forms.Button();
          this.buttonUp = new System.Windows.Forms.Button();
          this.buttonClose = new System.Windows.Forms.Button();
          this.panel1.SuspendLayout();
          this.SuspendLayout();
          // 
          // flowLayoutPanel1
          // 
          this.flowLayoutPanel1.AutoSize = true;
          this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
          this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(455, 10000);
          this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(455, 265);
          this.flowLayoutPanel1.Name = "flowLayoutPanel1";
          this.flowLayoutPanel1.Size = new System.Drawing.Size(455, 265);
          this.flowLayoutPanel1.TabIndex = 0;
          // 
          // panel1
          // 
          this.panel1.Controls.Add(this.buttonDown);
          this.panel1.Controls.Add(this.buttonUp);
          this.panel1.Controls.Add(this.buttonClose);
          this.panel1.Location = new System.Drawing.Point(455, 0);
          this.panel1.Name = "panel1";
          this.panel1.Size = new System.Drawing.Size(179, 268);
          this.panel1.TabIndex = 1;
          // 
          // buttonDown
          // 
          this.buttonDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.buttonDown.Image = global::VTIWindowsControlLibrary.Properties.Resources._60px_Go_down;
          this.buttonDown.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
          this.buttonDown.Location = new System.Drawing.Point(6, 181);
          this.buttonDown.Name = "buttonDown";
          this.buttonDown.Size = new System.Drawing.Size(75, 75);
          this.buttonDown.TabIndex = 4;
          this.buttonDown.Text = "DOWN";
          this.buttonDown.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
          this.buttonDown.UseVisualStyleBackColor = true;
          this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
          // 
          // buttonUp
          // 
          this.buttonUp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.buttonUp.Image = global::VTIWindowsControlLibrary.Properties.Resources._60px_Go_up;
          this.buttonUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
          this.buttonUp.Location = new System.Drawing.Point(6, 78);
          this.buttonUp.Name = "buttonUp";
          this.buttonUp.Size = new System.Drawing.Size(75, 75);
          this.buttonUp.TabIndex = 3;
          this.buttonUp.Text = "UP";
          this.buttonUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
          this.buttonUp.UseVisualStyleBackColor = true;
          this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
          // 
          // buttonClose
          // 
          this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
          this.buttonClose.Location = new System.Drawing.Point(6, 12);
          this.buttonClose.Name = "buttonClose";
          this.buttonClose.Size = new System.Drawing.Size(161, 60);
          this.buttonClose.TabIndex = 2;
          this.buttonClose.Text = "&Close";
          this.buttonClose.UseVisualStyleBackColor = true;
          this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
          // 
          // TouchScreenButtonForm
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(634, 268);
          this.Controls.Add(this.panel1);
          this.Controls.Add(this.flowLayoutPanel1);
          this.DoubleBuffered = true;
          this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
          this.MaximizeBox = false;
          this.MinimizeBox = false;
          this.Name = "TouchScreenButtonForm";
          this.Text = "Manual Commands";
          this.Load += new System.EventHandler(this.TouchScreenButtonForm_Load);
          this.VisibleChanged += new System.EventHandler(this.TouchScreenButtonForm_VisibleChanged);
          this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.TouchScreenButtonForm_FormClosing);
          this.panel1.ResumeLayout(false);
          this.ResumeLayout(false);
          this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Button buttonClose;
        public System.Windows.Forms.Button buttonUp;
        public System.Windows.Forms.Button buttonDown;
    }
}