namespace VTIWindowsControlLibrary.Forms
{
	partial class DataPlotForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataPlotForm));
            this.dataPlotControl1 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotControl();
            this.SuspendLayout();
            // 
            // dataPlotControl1
            // 
            this.dataPlotControl1.AllowClose = true;
            this.dataPlotControl1.AutoRun1Visible = true;
            //this.dataPlotControl1.AutoRun2Visible = true;
            this.dataPlotControl1.AutoShowAllVisible = true;
            this.dataPlotControl1.AutoShowEndVisible = true;
            this.dataPlotControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataPlotControl1.Caption = "Data Plot";
            this.dataPlotControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotControl1.Location = new System.Drawing.Point(0, 0);
            this.dataPlotControl1.Name = "dataPlotControl1";
            this.dataPlotControl1.PlotName = "";
            this.dataPlotControl1.RunStopVisible = true;
            this.dataPlotControl1.ShowAllVisible = true;
            this.dataPlotControl1.ShowEndVisible = true;
            this.dataPlotControl1.Size = new System.Drawing.Size(650, 387);
            this.dataPlotControl1.StatusStripVisible = false;
            this.dataPlotControl1.TabIndex = 0;
            this.dataPlotControl1.CaptionChanged += new System.EventHandler(this.dataPlotControl1_CaptionChanged);
            this.dataPlotControl1.Close += new System.EventHandler(this.dataPlotControl1_Close);
            // 
            // DataPlotForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(650, 387);
            this.Controls.Add(this.dataPlotControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataPlotForm";
            this.Text = "Data Plot";
            this.ResumeLayout(false);

		}

		#endregion

        private VTIWindowsControlLibrary.Components.Graphing.DataPlotControl dataPlotControl1;
	}
}