using System.Collections.Generic;
using VTIWindowsControlLibrary.Classes.IO;
namespace VTIWindowsControlLibrary.Components
{
  partial class SystemSignalsControl
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
      if (disposing && (components != null)) {
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
      this.panelCaption = new System.Windows.Forms.Panel();
      this.buttonHide = new System.Windows.Forms.Button();
      this.labelFromDataPlot = new System.Windows.Forms.Label();
      this.labelCaption = new System.Windows.Forms.Label();
      this.panelBody = new System.Windows.Forms.Panel();
      this.refreshTimer = new System.Windows.Forms.Timer(this.components);
      this.panelCaption.SuspendLayout();
      this.SuspendLayout();
      // 
      // panelCaption
      // 
      this.panelCaption.AutoSize = true;
      this.panelCaption.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelCaption.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelCaption.Controls.Add(this.buttonHide);
      this.panelCaption.Controls.Add(this.labelFromDataPlot);
      this.panelCaption.Controls.Add(this.labelCaption);
      this.panelCaption.Dock = System.Windows.Forms.DockStyle.Top;
      this.panelCaption.Location = new System.Drawing.Point(0, 0);
      this.panelCaption.MinimumSize = new System.Drawing.Size(100, 35);
      this.panelCaption.Name = "panelCaption";
      this.panelCaption.Size = new System.Drawing.Size(150, 35);
      this.panelCaption.TabIndex = 1;
      // 
      // buttonHide
      // 
      this.buttonHide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonHide.BackgroundImage = global::VTIWindowsControlLibrary.Properties.Resources.CloseToolbar;
      this.buttonHide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.buttonHide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.buttonHide.Font = new System.Drawing.Font("Lucida Console", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonHide.ForeColor = System.Drawing.SystemColors.Control;
      this.buttonHide.Location = new System.Drawing.Point(126, 0);
      this.buttonHide.Margin = new System.Windows.Forms.Padding(0);
      this.buttonHide.Name = "buttonHide";
      this.buttonHide.Size = new System.Drawing.Size(19, 19);
      this.buttonHide.TabIndex = 5;
      this.buttonHide.UseVisualStyleBackColor = true;
      this.buttonHide.Click += new System.EventHandler(this.buttonHide_Click);
      // 
      // labelFromDataPlot
      // 
      this.labelFromDataPlot.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelFromDataPlot.Font = new System.Drawing.Font("Arial", 9F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelFromDataPlot.ForeColor = System.Drawing.Color.Red;
      this.labelFromDataPlot.Location = new System.Drawing.Point(0, 16);
      this.labelFromDataPlot.Name = "labelFromDataPlot";
      this.labelFromDataPlot.Size = new System.Drawing.Size(146, 15);
      this.labelFromDataPlot.TabIndex = 4;
      this.labelFromDataPlot.Text = "FROM DATA PLOT";
      this.labelFromDataPlot.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      this.labelFromDataPlot.Visible = false;
      // 
      // labelCaption
      // 
      this.labelCaption.Dock = System.Windows.Forms.DockStyle.Top;
      this.labelCaption.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelCaption.Location = new System.Drawing.Point(0, 0);
      this.labelCaption.MaximumSize = new System.Drawing.Size(150, 32);
      this.labelCaption.MinimumSize = new System.Drawing.Size(120, 16);
      this.labelCaption.Name = "labelCaption";
      this.labelCaption.Padding = new System.Windows.Forms.Padding(15, 0, 15, 0);
      this.labelCaption.Size = new System.Drawing.Size(146, 16);
      this.labelCaption.TabIndex = 3;
      this.labelCaption.Text = "SYSTEM SIGNALS";
      this.labelCaption.TextAlign = System.Drawing.ContentAlignment.TopCenter;
      this.labelCaption.Click += new System.EventHandler(this.labelCaption_Click);
      // 
      // panelBody
      // 
      this.panelBody.AutoScroll = true;
      this.panelBody.AutoSize = true;
      this.panelBody.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.panelBody.BackColor = System.Drawing.Color.Black;
      this.panelBody.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
      this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panelBody.Location = new System.Drawing.Point(0, 35);
      this.panelBody.Name = "panelBody";
      this.panelBody.Size = new System.Drawing.Size(150, 365);
      this.panelBody.TabIndex = 2;
      // 
      // timer1
      // 
      this.refreshTimer.Interval = 200;
      this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
      // 
      // SystemSignalsControl
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.panelBody);
      this.Controls.Add(this.panelCaption);
      this.MinimumSize = new System.Drawing.Size(120, 250);
      this.Name = "SystemSignalsControl";
      this.Size = new System.Drawing.Size(150, 400);
      this.VisibleChanged += new System.EventHandler(this.SystemSignalsControl2_VisibleChanged);
      this.Resize += new System.EventHandler(this.SystemSignalsControl2_Resize);
      this.panelCaption.ResumeLayout(false);
      this.ResumeLayout(false);
      this.PerformLayout();
      // AnalogSignal List
      _analogSignalList = new List<AnalogSignal>();
    }

    #endregion

    private System.Windows.Forms.Panel panelCaption;
    private System.Windows.Forms.Button buttonHide;
    private System.Windows.Forms.Label labelFromDataPlot;
    private System.Windows.Forms.Label labelCaption;
    private System.Windows.Forms.Panel panelBody;
    private System.Windows.Forms.Timer refreshTimer;

  }
}
