namespace VTIWindowsControlLibrary.Forms
{
  partial class OperatorFormDualNested
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
            this.miniCommandControl1 = new VTIWindowsControlLibrary.Components.MiniCommandControl();
            this.systemSignalsControl1 = new VTIWindowsControlLibrary.Components.SystemSignalsControl();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.richTextPrompt3 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.richTextPrompt2 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelPortName2 = new System.Windows.Forms.Label();
            this.panelRightMid = new System.Windows.Forms.Panel();
            this.sequenceStepsControl2 = new VTIWindowsControlLibrary.Components.SequenceStepsControl();
            this.testHistoryDockControl2 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.valvesPanelDockControl2 = new VTIWindowsControlLibrary.Components.ValvesPanelDockControl();
            this.dataPlotDockControl2 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.panel3 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.richTextPrompt1 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelPortName1 = new System.Windows.Forms.Label();
            this.panelLeftMid = new System.Windows.Forms.Panel();
            this.sequenceStepsControl1 = new VTIWindowsControlLibrary.Components.SequenceStepsControl();
            this.testHistoryDockControl1 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.valvesPanelDockControl1 = new VTIWindowsControlLibrary.Components.ValvesPanelDockControl();
            this.dataPlotDockControl1 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.panelRightPane.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.panelRightMid.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.panelLeftMid.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelRightPane
            // 
            this.panelRightPane.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelRightPane.Controls.Add(this.miniCommandControl1);
            this.panelRightPane.Controls.Add(this.systemSignalsControl1);
            this.panelRightPane.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightPane.Location = new System.Drawing.Point(799, 0);
            this.panelRightPane.Name = "panelRightPane";
            this.panelRightPane.Size = new System.Drawing.Size(150, 560);
            this.panelRightPane.TabIndex = 3;
            // 
            // miniCommandControl1
            // 
            this.miniCommandControl1.AutoAdjustWidth = true;
            this.miniCommandControl1.AutoSize = true;
            this.miniCommandControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.miniCommandControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.miniCommandControl1.Location = new System.Drawing.Point(0, 480);
            this.miniCommandControl1.MinimumSize = new System.Drawing.Size(100, 80);
            this.miniCommandControl1.Name = "miniCommandControl1";
            this.miniCommandControl1.Size = new System.Drawing.Size(150, 80);
            this.miniCommandControl1.TabIndex = 4;
            this.miniCommandControl1.VisibleChanged += new System.EventHandler(this.miniCommandControl1_VisibleChanged);
            // 
            // systemSignalsControl1
            // 
            this.systemSignalsControl1.Caption = "SYSTEM SIGNALS";
            this.systemSignalsControl1.DataPlotControl = null;
            this.systemSignalsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemSignalsControl1.Location = new System.Drawing.Point(0, 0);
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
            this.systemSignalsControl1.Size = new System.Drawing.Size(150, 560);
            this.systemSignalsControl1.TabIndex = 0;
            this.systemSignalsControl1.VisibleChanged += new System.EventHandler(this.systemSignalsControl1_VisibleChanged);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80.32201F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(799, 560);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.splitContainer2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(402, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(394, 554);
            this.panel4.TabIndex = 3;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.richTextPrompt3);
            this.splitContainer2.Panel1.Controls.Add(this.richTextPrompt2);
            this.splitContainer2.Panel1.Controls.Add(this.labelPortName2);
            this.splitContainer2.Panel1.Controls.Add(this.panelRightMid);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataPlotDockControl2);
            this.splitContainer2.Size = new System.Drawing.Size(394, 554);
            this.splitContainer2.SplitterDistance = 332;
            this.splitContainer2.TabIndex = 8;
            // 
            // richTextPrompt3
            // 
            this.richTextPrompt3.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt3.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt3.Location = new System.Drawing.Point(284, 60);
            this.richTextPrompt3.Name = "richTextPrompt3";
            this.richTextPrompt3.Size = new System.Drawing.Size(100, 96);
            this.richTextPrompt3.TabIndex = 9;
            this.richTextPrompt3.Text = "";
            // 
            // richTextPrompt2
            // 
            this.richTextPrompt2.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt2.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt2.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt2.Location = new System.Drawing.Point(0, 32);
            this.richTextPrompt2.Name = "richTextPrompt2";
            this.richTextPrompt2.Size = new System.Drawing.Size(394, 110);
            this.richTextPrompt2.TabIndex = 7;
            this.richTextPrompt2.Text = "";
            this.richTextPrompt2.TextChanged += new System.EventHandler(this.richTextPrompt2_TextChanged);
            // 
            // labelPortName2
            // 
            this.labelPortName2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName2.Location = new System.Drawing.Point(0, 0);
            this.labelPortName2.Name = "labelPortName2";
            this.labelPortName2.Size = new System.Drawing.Size(394, 32);
            this.labelPortName2.TabIndex = 8;
            this.labelPortName2.Text = "Port Name";
            this.labelPortName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelRightMid
            // 
            this.panelRightMid.Controls.Add(this.sequenceStepsControl2);
            this.panelRightMid.Controls.Add(this.testHistoryDockControl2);
            this.panelRightMid.Controls.Add(this.valvesPanelDockControl2);
            this.panelRightMid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelRightMid.Location = new System.Drawing.Point(0, 142);
            this.panelRightMid.Name = "panelRightMid";
            this.panelRightMid.Size = new System.Drawing.Size(394, 190);
            this.panelRightMid.TabIndex = 1;
            // 
            // sequenceStepsControl2
            // 
            this.sequenceStepsControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sequenceStepsControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceStepsControl2.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceStepsControl2.LabelHeight = 20;
            this.sequenceStepsControl2.Location = new System.Drawing.Point(0, 0);
            this.sequenceStepsControl2.Name = "sequenceStepsControl2";
            this.sequenceStepsControl2.Size = new System.Drawing.Size(94, 190);
            this.sequenceStepsControl2.TabIndex = 1;
            // 
            // testHistoryDockControl2
            // 
            this.testHistoryDockControl2.AutoSize = true;
            this.testHistoryDockControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl2.Caption = "Blue Port History";
            this.testHistoryDockControl2.Columns = 1;
            this.testHistoryDockControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.testHistoryDockControl2.IsDocked = true;
            this.testHistoryDockControl2.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl2.Location = new System.Drawing.Point(94, 0);
            this.testHistoryDockControl2.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl2.Name = "testHistoryDockControl2";
            this.testHistoryDockControl2.Rows = 15;
            this.testHistoryDockControl2.Size = new System.Drawing.Size(150, 190);
            this.testHistoryDockControl2.TabIndex = 0;
            this.testHistoryDockControl2.UndockFrameAutoSize = true;
            this.testHistoryDockControl2.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.testHistoryDockControl2.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.testHistoryDockControl2.UndockFrameMaximizeBox = false;
            this.testHistoryDockControl2.UndockFrameMinimizeBox = false;
            this.testHistoryDockControl2.UndockFrameShowInTaskbar = true;
            this.testHistoryDockControl2.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.testHistoryDockControl2.UndockFrameTopMost = true;
            // 
            // valvesPanelDockControl2
            // 
            this.valvesPanelDockControl2.AutoSize = true;
            this.valvesPanelDockControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.valvesPanelDockControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valvesPanelDockControl2.Caption = "Blue Port History";
            this.valvesPanelDockControl2.Columns = 1;
            this.valvesPanelDockControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.valvesPanelDockControl2.IsDocked = true;
            this.valvesPanelDockControl2.LabelSize = new System.Drawing.Size(140, 13);
            this.valvesPanelDockControl2.Location = new System.Drawing.Point(244, 0);
            this.valvesPanelDockControl2.Margin = new System.Windows.Forms.Padding(0);
            this.valvesPanelDockControl2.Name = "valvesPanelDockControl2";
            this.valvesPanelDockControl2.Rows = 15;
            this.valvesPanelDockControl2.Size = new System.Drawing.Size(150, 190);
            this.valvesPanelDockControl2.TabIndex = 0;
            this.valvesPanelDockControl2.UndockFrameAutoSize = true;
            this.valvesPanelDockControl2.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.valvesPanelDockControl2.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.valvesPanelDockControl2.UndockFrameMaximizeBox = false;
            this.valvesPanelDockControl2.UndockFrameMinimizeBox = false;
            this.valvesPanelDockControl2.UndockFrameShowInTaskbar = true;
            this.valvesPanelDockControl2.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.valvesPanelDockControl2.UndockFrameTopMost = true;
            // 
            // dataPlotDockControl2
            // 
            this.dataPlotDockControl2.Caption = "Blue Port Data Plot";
            this.dataPlotDockControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl2.IsDocked = true;
            this.dataPlotDockControl2.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl2.Name = "dataPlotDockControl2";
            this.dataPlotDockControl2.Size = new System.Drawing.Size(394, 218);
            this.dataPlotDockControl2.TabIndex = 0;
            this.dataPlotDockControl2.UndockFrameAutoSize = false;
            this.dataPlotDockControl2.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.dataPlotDockControl2.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl2.UndockFrameMaximizeBox = true;
            this.dataPlotDockControl2.UndockFrameMinimizeBox = true;
            this.dataPlotDockControl2.UndockFrameShowInTaskbar = true;
            this.dataPlotDockControl2.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.dataPlotDockControl2.UndockFrameTopMost = false;
            this.dataPlotDockControl2.DockChanged += new System.EventHandler(this.dataPlotDockControl2_DockChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.splitContainer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(393, 554);
            this.panel3.TabIndex = 2;
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
            this.splitContainer1.Panel1.Controls.Add(this.labelPortName1);
            this.splitContainer1.Panel1.Controls.Add(this.panelLeftMid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataPlotDockControl1);
            this.splitContainer1.Size = new System.Drawing.Size(393, 554);
            this.splitContainer1.SplitterDistance = 330;
            this.splitContainer1.TabIndex = 7;
            // 
            // richTextPrompt1
            // 
            this.richTextPrompt1.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt1.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt1.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt1.Location = new System.Drawing.Point(0, 32);
            this.richTextPrompt1.Name = "richTextPrompt1";
            this.richTextPrompt1.Size = new System.Drawing.Size(393, 108);
            this.richTextPrompt1.TabIndex = 7;
            this.richTextPrompt1.Text = "";
            this.richTextPrompt1.TextChanged += new System.EventHandler(this.richTextPrompt1_TextChanged);
            // 
            // labelPortName1
            // 
            this.labelPortName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName1.Location = new System.Drawing.Point(0, 0);
            this.labelPortName1.Name = "labelPortName1";
            this.labelPortName1.Size = new System.Drawing.Size(393, 32);
            this.labelPortName1.TabIndex = 8;
            this.labelPortName1.Text = "Port Name";
            this.labelPortName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelLeftMid
            // 
            this.panelLeftMid.Controls.Add(this.sequenceStepsControl1);
            this.panelLeftMid.Controls.Add(this.testHistoryDockControl1);
            this.panelLeftMid.Controls.Add(this.valvesPanelDockControl1);
            this.panelLeftMid.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelLeftMid.Location = new System.Drawing.Point(0, 140);
            this.panelLeftMid.Name = "panelLeftMid";
            this.panelLeftMid.Size = new System.Drawing.Size(393, 190);
            this.panelLeftMid.TabIndex = 1;
            // 
            // sequenceStepsControl1
            // 
            this.sequenceStepsControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sequenceStepsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceStepsControl1.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceStepsControl1.LabelHeight = 20;
            this.sequenceStepsControl1.Location = new System.Drawing.Point(0, 0);
            this.sequenceStepsControl1.Name = "sequenceStepsControl1";
            this.sequenceStepsControl1.Size = new System.Drawing.Size(93, 190);
            this.sequenceStepsControl1.TabIndex = 1;
            // 
            // testHistoryDockControl1
            // 
            this.testHistoryDockControl1.AutoSize = true;
            this.testHistoryDockControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl1.Caption = "Blue Port History";
            this.testHistoryDockControl1.Columns = 1;
            this.testHistoryDockControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.testHistoryDockControl1.IsDocked = true;
            this.testHistoryDockControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl1.Location = new System.Drawing.Point(93, 0);
            this.testHistoryDockControl1.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl1.Name = "testHistoryDockControl1";
            this.testHistoryDockControl1.Rows = 15;
            this.testHistoryDockControl1.Size = new System.Drawing.Size(150, 190);
            this.testHistoryDockControl1.TabIndex = 0;
            this.testHistoryDockControl1.UndockFrameAutoSize = true;
            this.testHistoryDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.testHistoryDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.testHistoryDockControl1.UndockFrameMaximizeBox = false;
            this.testHistoryDockControl1.UndockFrameMinimizeBox = false;
            this.testHistoryDockControl1.UndockFrameShowInTaskbar = true;
            this.testHistoryDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.testHistoryDockControl1.UndockFrameTopMost = true;
            // 
            // valvesPanelDockControl1
            // 
            this.valvesPanelDockControl1.AutoSize = true;
            this.valvesPanelDockControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.valvesPanelDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.valvesPanelDockControl1.Caption = "Blue Port History";
            this.valvesPanelDockControl1.Columns = 1;
            this.valvesPanelDockControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.valvesPanelDockControl1.IsDocked = true;
            this.valvesPanelDockControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.valvesPanelDockControl1.Location = new System.Drawing.Point(243, 0);
            this.valvesPanelDockControl1.Margin = new System.Windows.Forms.Padding(0);
            this.valvesPanelDockControl1.Name = "valvesPanelDockControl1";
            this.valvesPanelDockControl1.Rows = 15;
            this.valvesPanelDockControl1.Size = new System.Drawing.Size(150, 190);
            this.valvesPanelDockControl1.TabIndex = 0;
            this.valvesPanelDockControl1.UndockFrameAutoSize = true;
            this.valvesPanelDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.valvesPanelDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.valvesPanelDockControl1.UndockFrameMaximizeBox = false;
            this.valvesPanelDockControl1.UndockFrameMinimizeBox = false;
            this.valvesPanelDockControl1.UndockFrameShowInTaskbar = true;
            this.valvesPanelDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.valvesPanelDockControl1.UndockFrameTopMost = true;
            // 
            // dataPlotDockControl1
            // 
            this.dataPlotDockControl1.Caption = "Blue Port Data Plot";
            this.dataPlotDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl1.IsDocked = true;
            this.dataPlotDockControl1.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl1.Name = "dataPlotDockControl1";
            this.dataPlotDockControl1.Size = new System.Drawing.Size(393, 220);
            this.dataPlotDockControl1.TabIndex = 0;
            this.dataPlotDockControl1.UndockFrameAutoSize = false;
            this.dataPlotDockControl1.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.dataPlotDockControl1.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl1.UndockFrameMaximizeBox = true;
            this.dataPlotDockControl1.UndockFrameMinimizeBox = true;
            this.dataPlotDockControl1.UndockFrameShowInTaskbar = true;
            this.dataPlotDockControl1.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.dataPlotDockControl1.UndockFrameTopMost = false;
            this.dataPlotDockControl1.DockChanged += new System.EventHandler(this.dataPlotDockControl1_DockChanged);
            // 
            // OperatorFormDualNested
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 560);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.panelRightPane);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OperatorFormDualNested";
            this.Text = "Operator Form";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OperatorFormDualNested_FormClosed);
            this.panelRightPane.ResumeLayout(false);
            this.panelRightPane.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.panelRightMid.ResumeLayout(false);
            this.panelRightMid.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.panelLeftMid.ResumeLayout(false);
            this.panelLeftMid.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Panel panelRightPane;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.Panel panel4;
    private System.Windows.Forms.Panel panel3;
    private System.Windows.Forms.Panel panelLeftMid;
    public VTIWindowsControlLibrary.Components.MiniCommandControl miniCommandControl1;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.Panel panelRightMid;
    private Components.SystemSignalsControl systemSignalsControl1;
    private Components.RichTextPrompt richTextPrompt3;
    private Components.RichTextPrompt richTextPrompt2;
    private System.Windows.Forms.Label labelPortName2;
    private Components.TestHistoryDockControl testHistoryDockControl2;
    private Components.ValvesPanelDockControl valvesPanelDockControl2;
    private Components.Graphing.DataPlotDockControl dataPlotDockControl2;
    private Components.RichTextPrompt richTextPrompt1;
    private System.Windows.Forms.Label labelPortName1;
    private Components.TestHistoryDockControl testHistoryDockControl1;
    private Components.ValvesPanelDockControl valvesPanelDockControl1;
    private Components.Graphing.DataPlotDockControl dataPlotDockControl1;
        public Components.SequenceStepsControl sequenceStepsControl1;
        public Components.SequenceStepsControl sequenceStepsControl2;
    }
}