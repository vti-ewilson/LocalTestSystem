using VTIWindowsControlLibrary.Components;
namespace VTIWindowsControlLibrary.Forms
{
  partial class OperatorFormSingleNestedNoSeq
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
            this.components = new System.ComponentModel.Container();
            this.panelRightPane = new System.Windows.Forms.Panel();
            this.systemSignalsControl1 = new VTIWindowsControlLibrary.Components.SystemSignalsControl();
            this.testHistoryDockControl1 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.valvesPanelDockControl1 = new VTIWindowsControlLibrary.Components.ValvesPanelDockControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.richTextPrompt1 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelFlowRate = new System.Windows.Forms.Label();
            this.labelPortName = new System.Windows.Forms.Label();
            this.flowLayoutPanelCommands = new System.Windows.Forms.FlowLayoutPanel();
            this.dataPlotDockControl1 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.miniCommandControl1 = new VTIWindowsControlLibrary.Components.MiniCommandControl();
            this.signalIndicatorControl1 = new VTIWindowsControlLibrary.Components.SignalIndicatorControl();
            this.panelRightPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).BeginInit();
            this.splitContainer3.Panel1.SuspendLayout();
            this.splitContainer3.Panel2.SuspendLayout();
            this.splitContainer3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRightPane
            // 
            this.panelRightPane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRightPane.Controls.Add(this.systemSignalsControl1);
            this.panelRightPane.Controls.Add(this.testHistoryDockControl1);
            this.panelRightPane.Controls.Add(this.valvesPanelDockControl1);
            this.panelRightPane.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightPane.Location = new System.Drawing.Point(633, 0);
            this.panelRightPane.Name = "panelRightPane";
            this.panelRightPane.Size = new System.Drawing.Size(150, 564);
            this.panelRightPane.TabIndex = 1;
            // 
            // systemSignalsControl1
            // 
            this.systemSignalsControl1.Caption = "SYSTEM SIGNALS";
            this.systemSignalsControl1.DataPlotControl = null;
            this.systemSignalsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemSignalsControl1.Location = new System.Drawing.Point(0, 0);
            this.systemSignalsControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.systemSignalsControl1.MinimumSize = new System.Drawing.Size(120, 250);
            this.systemSignalsControl1.Name = "systemSignalsControl1";
            this.systemSignalsControl1.RawValuesEnabled = true;
            this.systemSignalsControl1.refreshTimerInterval = 200;
            this.systemSignalsControl1.ShowingFromDataPlot = false;
            this.systemSignalsControl1.ShowingRawValues = false;
            this.systemSignalsControl1.ShowXButton = true;
            this.systemSignalsControl1.SignalCaptionFont = new System.Drawing.Font("Arial", 10.125F);
            this.systemSignalsControl1.SignalCaptionWidth = 300;
            this.systemSignalsControl1.SignalValueFont = new System.Drawing.Font("Microsoft Sans Serif", 7.875F);
            this.systemSignalsControl1.Size = new System.Drawing.Size(150, 250);
            this.systemSignalsControl1.TabIndex = 0;
            this.systemSignalsControl1.VisibleChanged += new System.EventHandler(this.systemSignalsControl1_VisibleChanged);
            // 
            // testHistoryDockControl1
            // 
            this.testHistoryDockControl1.AutoSize = true;
            this.testHistoryDockControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl1.Caption = "Test History";
            this.testHistoryDockControl1.Columns = 1;
            this.testHistoryDockControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.testHistoryDockControl1.IsDocked = true;
            this.testHistoryDockControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl1.Location = new System.Drawing.Point(0, 186);
            this.testHistoryDockControl1.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl1.MinimumSize = new System.Drawing.Size(150, 150);
            this.testHistoryDockControl1.Name = "testHistoryDockControl1";
            this.testHistoryDockControl1.Rows = 10;
            this.testHistoryDockControl1.Size = new System.Drawing.Size(150, 189);
            this.testHistoryDockControl1.TabIndex = 1;
            this.testHistoryDockControl1.UndockFrameAutoSize = true;
            this.testHistoryDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.testHistoryDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.testHistoryDockControl1.UndockFrameMaximizeBox = false;
            this.testHistoryDockControl1.UndockFrameMinimizeBox = false;
            this.testHistoryDockControl1.UndockFrameShowInTaskbar = true;
            this.testHistoryDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.testHistoryDockControl1.UndockFrameTopMost = true;
            this.testHistoryDockControl1.DockChanged += new System.EventHandler(this.testHistoryDockControl1_DockChanged);
            this.testHistoryDockControl1.VisibleChanged += new System.EventHandler(this.testHistoryDockControl1_VisibleChanged);
            // 
            // valvesPanelDockControl1
            // 
            this.valvesPanelDockControl1.AutoSize = true;
            this.valvesPanelDockControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.valvesPanelDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valvesPanelDockControl1.Caption = "Valves Panel";
            this.valvesPanelDockControl1.Columns = 1;
            this.valvesPanelDockControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.valvesPanelDockControl1.IsDocked = true;
            this.valvesPanelDockControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.valvesPanelDockControl1.Location = new System.Drawing.Point(0, 375);
            this.valvesPanelDockControl1.Margin = new System.Windows.Forms.Padding(0);
            this.valvesPanelDockControl1.MinimumSize = new System.Drawing.Size(150, 150);
            this.valvesPanelDockControl1.Name = "valvesPanelDockControl1";
            this.valvesPanelDockControl1.Rows = 10;
            this.valvesPanelDockControl1.Size = new System.Drawing.Size(150, 189);
            this.valvesPanelDockControl1.TabIndex = 1;
            this.valvesPanelDockControl1.UndockFrameAutoSize = true;
            this.valvesPanelDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.valvesPanelDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.valvesPanelDockControl1.UndockFrameMaximizeBox = false;
            this.valvesPanelDockControl1.UndockFrameMinimizeBox = false;
            this.valvesPanelDockControl1.UndockFrameShowInTaskbar = true;
            this.valvesPanelDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.valvesPanelDockControl1.UndockFrameTopMost = true;
            this.valvesPanelDockControl1.Visible = false;
            this.valvesPanelDockControl1.DockChanged += new System.EventHandler(this.valvesPanelDockControl1_DockChanged);
            this.valvesPanelDockControl1.VisibleChanged += new System.EventHandler(this.valvesPanelDockControl1_VisibleChanged);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.richTextPrompt1);
            this.splitContainer1.Panel1.Controls.Add(this.labelFlowRate);
            this.splitContainer1.Panel1.Controls.Add(this.labelPortName);
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanelCommands);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataPlotDockControl1);
            this.splitContainer1.Size = new System.Drawing.Size(414, 564);
            this.splitContainer1.SplitterDistance = 251;
            this.splitContainer1.TabIndex = 0;
            // 
            // richTextPrompt1
            // 
            this.richTextPrompt1.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt1.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt1.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt1.Location = new System.Drawing.Point(0, 11);
            this.richTextPrompt1.Name = "richTextPrompt1";
            this.richTextPrompt1.Size = new System.Drawing.Size(410, 223);
            this.richTextPrompt1.TabIndex = 1;
            this.richTextPrompt1.Text = "";
            this.richTextPrompt1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // labelFlowRate
            // 
            this.labelFlowRate.BackColor = System.Drawing.Color.Red;
            this.labelFlowRate.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelFlowRate.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelFlowRate.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelFlowRate.ForeColor = System.Drawing.Color.Yellow;
            this.labelFlowRate.Location = new System.Drawing.Point(0, 234);
            this.labelFlowRate.Name = "labelFlowRate";
            this.labelFlowRate.Size = new System.Drawing.Size(410, 17);
            this.labelFlowRate.TabIndex = 6;
            this.labelFlowRate.Text = "Flow Rate: 0.0E0 atm-cc/sec";
            this.labelFlowRate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelPortName
            // 
            this.labelPortName.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName.Location = new System.Drawing.Point(0, 0);
            this.labelPortName.Name = "labelPortName";
            this.labelPortName.Size = new System.Drawing.Size(410, 11);
            this.labelPortName.TabIndex = 4;
            this.labelPortName.Text = "Port Name";
            this.labelPortName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanelCommands
            // 
            this.flowLayoutPanelCommands.AutoSize = true;
            this.flowLayoutPanelCommands.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelCommands.Dock = System.Windows.Forms.DockStyle.Right;
            this.flowLayoutPanelCommands.Location = new System.Drawing.Point(410, 0);
            this.flowLayoutPanelCommands.Name = "flowLayoutPanelCommands";
            this.flowLayoutPanelCommands.Size = new System.Drawing.Size(4, 251);
            this.flowLayoutPanelCommands.TabIndex = 2;
            // 
            // dataPlotDockControl1
            // 
            this.dataPlotDockControl1.Caption = "Data Plot";
            this.dataPlotDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl1.IsDocked = true;
            this.dataPlotDockControl1.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataPlotDockControl1.Name = "dataPlotDockControl1";
            this.dataPlotDockControl1.Size = new System.Drawing.Size(414, 309);
            this.dataPlotDockControl1.TabIndex = 0;
            this.dataPlotDockControl1.UndockFrameAutoSize = false;
            this.dataPlotDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.dataPlotDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl1.UndockFrameMaximizeBox = true;
            this.dataPlotDockControl1.UndockFrameMinimizeBox = true;
            this.dataPlotDockControl1.UndockFrameShowInTaskbar = true;
            this.dataPlotDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.dataPlotDockControl1.UndockFrameTopMost = false;
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitContainer3.Location = new System.Drawing.Point(414, 0);
            this.splitContainer3.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.miniCommandControl1);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.signalIndicatorControl1);
            this.splitContainer3.Size = new System.Drawing.Size(219, 564);
            this.splitContainer3.SplitterDistance = 168;
            this.splitContainer3.SplitterWidth = 3;
            this.splitContainer3.TabIndex = 2;
            // 
            // miniCommandControl1
            // 
            this.miniCommandControl1.AutoAdjustWidth = true;
            this.miniCommandControl1.AutoSize = true;
            this.miniCommandControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.miniCommandControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.miniCommandControl1.Location = new System.Drawing.Point(0, 0);
            this.miniCommandControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.miniCommandControl1.MinimumSize = new System.Drawing.Size(100, 40);
            this.miniCommandControl1.Name = "miniCommandControl1";
            this.miniCommandControl1.Size = new System.Drawing.Size(168, 564);
            this.miniCommandControl1.TabIndex = 3;
            // 
            // signalIndicatorControl1
            // 
            this.signalIndicatorControl1.Caption = "";
            this.signalIndicatorControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.signalIndicatorControl1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.signalIndicatorControl1.IndicatorColor = System.Drawing.Color.Red;
            this.signalIndicatorControl1.LinMax = 100F;
            this.signalIndicatorControl1.LinMin = 0F;
            this.signalIndicatorControl1.Location = new System.Drawing.Point(0, 0);
            this.signalIndicatorControl1.LogMaxExp = -2;
            this.signalIndicatorControl1.LogMinExp = -9;
            this.signalIndicatorControl1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.signalIndicatorControl1.Name = "signalIndicatorControl1";
            this.signalIndicatorControl1.SemiLog = true;
            this.signalIndicatorControl1.ShowXButton = true;
            this.signalIndicatorControl1.Size = new System.Drawing.Size(48, 564);
            this.signalIndicatorControl1.TabIndex = 5;
            this.signalIndicatorControl1.Value = 50F;
            // 
            // OperatorFormSingleNestedNoSeq
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(783, 564);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.splitContainer3);
            this.Controls.Add(this.panelRightPane);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OperatorFormSingleNestedNoSeq";
            this.ShowInTaskbar = false;
            this.Text = "Operator Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OperatorFormSingleNestedNoSeq_FormClosed);
            this.panelRightPane.ResumeLayout(false);
            this.panelRightPane.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel1.PerformLayout();
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    public System.Windows.Forms.SplitContainer splitContainer1;
    private RichTextPrompt richTextPrompt1;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelCommands;
    private System.Windows.Forms.Panel panelRightPane;
    private VTIWindowsControlLibrary.Components.SystemSignalsControl systemSignalsControl1;
    private VTIWindowsControlLibrary.Components.TestHistoryDockControl testHistoryDockControl1;
    private VTIWindowsControlLibrary.Components.ValvesPanelDockControl valvesPanelDockControl1;
    public VTIWindowsControlLibrary.Components.MiniCommandControl miniCommandControl1;
    private System.Windows.Forms.Label labelPortName;
    private SignalIndicatorControl signalIndicatorControl1;
    private System.Windows.Forms.Label labelFlowRate;
    private Components.Graphing.DataPlotDockControl dataPlotDockControl1;
        private System.Windows.Forms.SplitContainer splitContainer3;
    }
}
