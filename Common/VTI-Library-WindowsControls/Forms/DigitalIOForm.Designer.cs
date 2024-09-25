namespace VTIWindowsControlLibrary.Forms
{
    partial class DigitalIOForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DigitalIOForm));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanelDigitalInputs = new System.Windows.Forms.FlowLayoutPanel();
            this.DigitalInputsLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanelDigitalOutputs = new System.Windows.Forms.FlowLayoutPanel();
            this.DigitalOutputsLabel = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.buttonAccept = new System.Windows.Forms.Button();
            this.buttonHide = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanelDigitalInputs);
            this.splitContainer1.Panel1.Controls.Add(this.DigitalInputsLabel);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.flowLayoutPanelDigitalOutputs);
            this.splitContainer1.Panel2.Controls.Add(this.DigitalOutputsLabel);
            // 
            // flowLayoutPanelDigitalInputs
            // 
            resources.ApplyResources(this.flowLayoutPanelDigitalInputs, "flowLayoutPanelDigitalInputs");
            this.flowLayoutPanelDigitalInputs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelDigitalInputs.Name = "flowLayoutPanelDigitalInputs";
            // 
            // DigitalInputsLabel
            // 
            this.DigitalInputsLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.DigitalInputsLabel, "DigitalInputsLabel");
            this.DigitalInputsLabel.Name = "DigitalInputsLabel";
            this.DigitalInputsLabel.DoubleClick += new System.EventHandler(this.DigitalInputsLabel_DoubleClick);
            // 
            // flowLayoutPanelDigitalOutputs
            // 
            resources.ApplyResources(this.flowLayoutPanelDigitalOutputs, "flowLayoutPanelDigitalOutputs");
            this.flowLayoutPanelDigitalOutputs.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelDigitalOutputs.Name = "flowLayoutPanelDigitalOutputs";
            // 
            // DigitalOutputsLabel
            // 
            this.DigitalOutputsLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.DigitalOutputsLabel, "DigitalOutputsLabel");
            this.DigitalOutputsLabel.Name = "DigitalOutputsLabel";
            this.DigitalOutputsLabel.DoubleClick += new System.EventHandler(this.DigitalOutputsLabel_DoubleClick);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.buttonAccept);
            this.flowLayoutPanel1.Controls.Add(this.buttonHide);
            resources.ApplyResources(this.flowLayoutPanel1, "flowLayoutPanel1");
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            // 
            // buttonAccept
            // 
            resources.ApplyResources(this.buttonAccept, "buttonAccept");
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // buttonHide
            // 
            resources.ApplyResources(this.buttonHide, "buttonHide");
            this.buttonHide.Name = "buttonHide";
            this.buttonHide.UseVisualStyleBackColor = true;
            this.buttonHide.Click += new System.EventHandler(this.button1_Click);
            // 
            // DigitalIOForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Name = "DigitalIOForm";
            this.Activated += new System.EventHandler(this.DigitalIOForm_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DigitalIOForm_FormClosing);
            this.Load += new System.EventHandler(this.DigitalIOForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        public System.Windows.Forms.Button buttonAccept;
        public System.Windows.Forms.Button buttonHide;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label DigitalInputsLabel;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDigitalInputs;
        public System.Windows.Forms.FlowLayoutPanel flowLayoutPanelDigitalOutputs;
        private System.Windows.Forms.Label DigitalOutputsLabel;
    }
}