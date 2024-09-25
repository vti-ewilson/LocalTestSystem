namespace VTIWindowsControlLibrary.Forms
{
    partial class OperatorFormTrippleNested
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
            this.miniCommandControl1 = new VTIWindowsControlLibrary.Components.MiniCommandControl();
            this.splitContainerMain = new System.Windows.Forms.SplitContainer();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.richTextPrompt1 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelPortName3 = new System.Windows.Forms.Label();
            this.richTextPrompt3 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelPortName2 = new System.Windows.Forms.Label();
            this.richTextPrompt2 = new VTIWindowsControlLibrary.Components.RichTextPrompt(this.components);
            this.labelPortName1 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.sequenceStepsControl1 = new VTIWindowsControlLibrary.Components.SequenceStepsControl();
            this.dataPlotDockControl1 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.testHistoryDockControl1 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.sequenceStepsControl2 = new VTIWindowsControlLibrary.Components.SequenceStepsControl();
            this.dataPlotDockControl2 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.testHistoryDockControl2 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.sequenceStepsControl3 = new VTIWindowsControlLibrary.Components.SequenceStepsControl();
            this.dataPlotDockControl3 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl();
            this.testHistoryDockControl3 = new VTIWindowsControlLibrary.Components.TestHistoryDockControl();
            this.panelRightPane.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).BeginInit();
            this.splitContainerMain.Panel1.SuspendLayout();
            this.splitContainerMain.Panel2.SuspendLayout();
            this.splitContainerMain.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tabPage3.SuspendLayout();
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
            this.panelRightPane.Controls.Add(this.miniCommandControl1);
            this.panelRightPane.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRightPane.Location = new System.Drawing.Point(799, 0);
            this.panelRightPane.Name = "panelRightPane";
            this.panelRightPane.Size = new System.Drawing.Size(150, 560);
            this.panelRightPane.TabIndex = 3;
            // 
            // systemSignalsControl1
            // 
            this.systemSignalsControl1.Caption = "SYSTEM SIGNALS";
            this.systemSignalsControl1.DataPlotControl = null;
            this.systemSignalsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.systemSignalsControl1.Location = new System.Drawing.Point(0, 0);
            this.systemSignalsControl1.Margin = new System.Windows.Forms.Padding(4);
            this.systemSignalsControl1.MinimumSize = new System.Drawing.Size(135, 308);
            this.systemSignalsControl1.Name = "systemSignalsControl1";
            this.systemSignalsControl1.RawValuesEnabled = true;
            this.systemSignalsControl1.refreshTimerInterval = 200;
            this.systemSignalsControl1.ShowingFromDataPlot = false;
            this.systemSignalsControl1.ShowingRawValues = false;
            this.systemSignalsControl1.ShowXButton = false;
            this.systemSignalsControl1.SignalCaptionFont = new System.Drawing.Font("Arial", 10.125F);
            this.systemSignalsControl1.SignalCaptionWidth = 300;
            this.systemSignalsControl1.SignalValueFont = new System.Drawing.Font("Microsoft Sans Serif", 7.875F);
            this.systemSignalsControl1.Size = new System.Drawing.Size(150, 480);
            this.systemSignalsControl1.TabIndex = 0;
            this.systemSignalsControl1.VisibleChanged += new System.EventHandler(this.systemSignalsControl1_VisibleChanged);
            // 
            // miniCommandControl1
            // 
            this.miniCommandControl1.AutoAdjustWidth = true;
            this.miniCommandControl1.AutoSize = true;
            this.miniCommandControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.miniCommandControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.miniCommandControl1.Location = new System.Drawing.Point(0, 480);
            this.miniCommandControl1.Margin = new System.Windows.Forms.Padding(4);
            this.miniCommandControl1.MinimumSize = new System.Drawing.Size(100, 80);
            this.miniCommandControl1.Name = "miniCommandControl1";
            this.miniCommandControl1.Size = new System.Drawing.Size(150, 80);
            this.miniCommandControl1.TabIndex = 4;
            this.miniCommandControl1.VisibleChanged += new System.EventHandler(this.miniCommandControl1_VisibleChanged);
            // 
            // splitContainerMain
            // 
            this.splitContainerMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerMain.Location = new System.Drawing.Point(0, 0);
            this.splitContainerMain.Name = "splitContainerMain";
            this.splitContainerMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainerMain.Panel1
            // 
            this.splitContainerMain.Panel1.Controls.Add(this.tableLayoutPanel2);
            // 
            // splitContainerMain.Panel2
            // 
            this.splitContainerMain.Panel2.Controls.Add(this.tabControl1);
            this.splitContainerMain.Size = new System.Drawing.Size(799, 560);
            this.splitContainerMain.SplitterDistance = 299;
            this.splitContainerMain.TabIndex = 5;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel2.Controls.Add(this.richTextPrompt1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelPortName3, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.richTextPrompt3, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelPortName2, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.richTextPrompt2, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.labelPortName1, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 32F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(799, 299);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // richTextPrompt1
            // 
            this.richTextPrompt1.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt1.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt1.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt1.Location = new System.Drawing.Point(3, 35);
            this.richTextPrompt1.Name = "richTextPrompt1";
            this.richTextPrompt1.Size = new System.Drawing.Size(260, 261);
            this.richTextPrompt1.TabIndex = 14;
            this.richTextPrompt1.Text = "";
            this.richTextPrompt1.TextChanged += new System.EventHandler(this.richTextPrompt1_TextChanged);
            // 
            // labelPortName3
            // 
            this.labelPortName3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName3.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName3.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName3.Location = new System.Drawing.Point(535, 0);
            this.labelPortName3.Name = "labelPortName3";
            this.labelPortName3.Size = new System.Drawing.Size(261, 32);
            this.labelPortName3.TabIndex = 13;
            this.labelPortName3.Text = "Port Name";
            this.labelPortName3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextPrompt3
            // 
            this.richTextPrompt3.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt3.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt3.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt3.Location = new System.Drawing.Point(535, 35);
            this.richTextPrompt3.Name = "richTextPrompt3";
            this.richTextPrompt3.Size = new System.Drawing.Size(261, 261);
            this.richTextPrompt3.TabIndex = 12;
            this.richTextPrompt3.Text = "";
            this.richTextPrompt3.TextChanged += new System.EventHandler(this.richTextPrompt3_TextChanged);
            // 
            // labelPortName2
            // 
            this.labelPortName2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName2.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName2.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName2.Location = new System.Drawing.Point(269, 0);
            this.labelPortName2.Name = "labelPortName2";
            this.labelPortName2.Size = new System.Drawing.Size(260, 32);
            this.labelPortName2.TabIndex = 11;
            this.labelPortName2.Text = "Port Name";
            this.labelPortName2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // richTextPrompt2
            // 
            this.richTextPrompt2.BackColor = System.Drawing.Color.Black;
            this.richTextPrompt2.DefaultColor = System.Drawing.Color.Yellow;
            this.richTextPrompt2.DefaultFont = new System.Drawing.Font("Arial", 18F);
            this.richTextPrompt2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextPrompt2.Location = new System.Drawing.Point(269, 35);
            this.richTextPrompt2.Name = "richTextPrompt2";
            this.richTextPrompt2.Size = new System.Drawing.Size(260, 261);
            this.richTextPrompt2.TabIndex = 10;
            this.richTextPrompt2.Text = "";
            this.richTextPrompt2.TextChanged += new System.EventHandler(this.richTextPrompt2_TextChanged);
            // 
            // labelPortName1
            // 
            this.labelPortName1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.labelPortName1.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelPortName1.Font = new System.Drawing.Font("Arial", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelPortName1.Location = new System.Drawing.Point(3, 0);
            this.labelPortName1.Name = "labelPortName1";
            this.labelPortName1.Size = new System.Drawing.Size(260, 32);
            this.labelPortName1.TabIndex = 9;
            this.labelPortName1.Text = "Port Name";
            this.labelPortName1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(799, 257);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage1.Controls.Add(this.splitContainer1);
            this.tabPage1.Controls.Add(this.testHistoryDockControl1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(791, 231);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.sequenceStepsControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataPlotDockControl1);
            this.splitContainer1.Size = new System.Drawing.Size(635, 225);
            this.splitContainer1.SplitterDistance = 210;
            this.splitContainer1.TabIndex = 1;
            // 
            // sequenceStepsControl1
            // 
            this.sequenceStepsControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sequenceStepsControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceStepsControl1.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceStepsControl1.LabelHeight = 20;
            this.sequenceStepsControl1.Location = new System.Drawing.Point(0, 0);
            this.sequenceStepsControl1.Margin = new System.Windows.Forms.Padding(4);
            this.sequenceStepsControl1.Name = "sequenceStepsControl1";
            this.sequenceStepsControl1.Size = new System.Drawing.Size(210, 225);
            this.sequenceStepsControl1.TabIndex = 0;
            // 
            // dataPlotDockControl1
            // 
            this.dataPlotDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataPlotDockControl1.Caption = "Data Plot";
            this.dataPlotDockControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl1.IsDocked = true;
            this.dataPlotDockControl1.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl1.Margin = new System.Windows.Forms.Padding(4);
            this.dataPlotDockControl1.Name = "dataPlotDockControl1";
            this.dataPlotDockControl1.Size = new System.Drawing.Size(421, 225);
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
            // testHistoryDockControl1
            // 
            this.testHistoryDockControl1.AutoSize = true;
            this.testHistoryDockControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl1.Caption = "Test History";
            this.testHistoryDockControl1.Columns = 1;
            this.testHistoryDockControl1.Dock = System.Windows.Forms.DockStyle.Right;
            this.testHistoryDockControl1.IsDocked = true;
            this.testHistoryDockControl1.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl1.Location = new System.Drawing.Point(638, 3);
            this.testHistoryDockControl1.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl1.MaximumSize = new System.Drawing.Size(151, 244);
            this.testHistoryDockControl1.Name = "testHistoryDockControl1";
            this.testHistoryDockControl1.Rows = 12;
            this.testHistoryDockControl1.Size = new System.Drawing.Size(150, 225);
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
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage2.Controls.Add(this.splitContainer2);
            this.tabPage2.Controls.Add(this.testHistoryDockControl2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(791, 231);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(3, 3);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.sequenceStepsControl2);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataPlotDockControl2);
            this.splitContainer2.Size = new System.Drawing.Size(635, 225);
            this.splitContainer2.SplitterDistance = 210;
            this.splitContainer2.TabIndex = 3;
            // 
            // sequenceStepsControl2
            // 
            this.sequenceStepsControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sequenceStepsControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceStepsControl2.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceStepsControl2.LabelHeight = 20;
            this.sequenceStepsControl2.Location = new System.Drawing.Point(0, 0);
            this.sequenceStepsControl2.Margin = new System.Windows.Forms.Padding(4);
            this.sequenceStepsControl2.Name = "sequenceStepsControl2";
            this.sequenceStepsControl2.Size = new System.Drawing.Size(210, 225);
            this.sequenceStepsControl2.TabIndex = 0;
            // 
            // dataPlotDockControl2
            // 
            this.dataPlotDockControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataPlotDockControl2.Caption = "Data Plot";
            this.dataPlotDockControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl2.IsDocked = true;
            this.dataPlotDockControl2.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl2.Margin = new System.Windows.Forms.Padding(4);
            this.dataPlotDockControl2.Name = "dataPlotDockControl2";
            this.dataPlotDockControl2.Size = new System.Drawing.Size(421, 225);
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
            // testHistoryDockControl2
            // 
            this.testHistoryDockControl2.AutoSize = true;
            this.testHistoryDockControl2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl2.Caption = "Test History";
            this.testHistoryDockControl2.Columns = 1;
            this.testHistoryDockControl2.Dock = System.Windows.Forms.DockStyle.Right;
            this.testHistoryDockControl2.IsDocked = true;
            this.testHistoryDockControl2.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl2.Location = new System.Drawing.Point(638, 3);
            this.testHistoryDockControl2.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl2.Name = "testHistoryDockControl2";
            this.testHistoryDockControl2.Rows = 12;
            this.testHistoryDockControl2.Size = new System.Drawing.Size(150, 225);
            this.testHistoryDockControl2.TabIndex = 2;
            this.testHistoryDockControl2.UndockFrameAutoSize = true;
            this.testHistoryDockControl2.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.testHistoryDockControl2.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.testHistoryDockControl2.UndockFrameMaximizeBox = false;
            this.testHistoryDockControl2.UndockFrameMinimizeBox = false;
            this.testHistoryDockControl2.UndockFrameShowInTaskbar = true;
            this.testHistoryDockControl2.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.testHistoryDockControl2.UndockFrameTopMost = true;
            // 
            // tabPage3
            // 
            this.tabPage3.BackColor = System.Drawing.SystemColors.Control;
            this.tabPage3.Controls.Add(this.splitContainer3);
            this.tabPage3.Controls.Add(this.testHistoryDockControl3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(791, 231);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "tabPage3";
            // 
            // splitContainer3
            // 
            this.splitContainer3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer3.Location = new System.Drawing.Point(3, 3);
            this.splitContainer3.Name = "splitContainer3";
            // 
            // splitContainer3.Panel1
            // 
            this.splitContainer3.Panel1.Controls.Add(this.sequenceStepsControl3);
            // 
            // splitContainer3.Panel2
            // 
            this.splitContainer3.Panel2.Controls.Add(this.dataPlotDockControl3);
            this.splitContainer3.Size = new System.Drawing.Size(635, 225);
            this.splitContainer3.SplitterDistance = 210;
            this.splitContainer3.TabIndex = 3;
            // 
            // sequenceStepsControl3
            // 
            this.sequenceStepsControl3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.sequenceStepsControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sequenceStepsControl3.LabelFont = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sequenceStepsControl3.LabelHeight = 20;
            this.sequenceStepsControl3.Location = new System.Drawing.Point(0, 0);
            this.sequenceStepsControl3.Margin = new System.Windows.Forms.Padding(4);
            this.sequenceStepsControl3.Name = "sequenceStepsControl3";
            this.sequenceStepsControl3.Size = new System.Drawing.Size(210, 225);
            this.sequenceStepsControl3.TabIndex = 0;
            // 
            // dataPlotDockControl3
            // 
            this.dataPlotDockControl3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dataPlotDockControl3.Caption = "Data Plot";
            this.dataPlotDockControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataPlotDockControl3.IsDocked = true;
            this.dataPlotDockControl3.Location = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl3.Margin = new System.Windows.Forms.Padding(4);
            this.dataPlotDockControl3.Name = "dataPlotDockControl3";
            this.dataPlotDockControl3.Size = new System.Drawing.Size(421, 225);
            this.dataPlotDockControl3.TabIndex = 0;
            this.dataPlotDockControl3.UndockFrameAutoSize = false;
            this.dataPlotDockControl3.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
            this.dataPlotDockControl3.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.dataPlotDockControl3.UndockFrameMaximizeBox = true;
            this.dataPlotDockControl3.UndockFrameMinimizeBox = true;
            this.dataPlotDockControl3.UndockFrameShowInTaskbar = true;
            this.dataPlotDockControl3.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.dataPlotDockControl3.UndockFrameTopMost = false;
            this.dataPlotDockControl3.DockChanged += new System.EventHandler(this.dataPlotDockControl3_DockChanged);
            // 
            // testHistoryDockControl3
            // 
            this.testHistoryDockControl3.AutoSize = true;
            this.testHistoryDockControl3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.testHistoryDockControl3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.testHistoryDockControl3.Caption = "Test History";
            this.testHistoryDockControl3.Columns = 1;
            this.testHistoryDockControl3.Dock = System.Windows.Forms.DockStyle.Right;
            this.testHistoryDockControl3.IsDocked = true;
            this.testHistoryDockControl3.LabelSize = new System.Drawing.Size(140, 13);
            this.testHistoryDockControl3.Location = new System.Drawing.Point(638, 3);
            this.testHistoryDockControl3.Margin = new System.Windows.Forms.Padding(0);
            this.testHistoryDockControl3.Name = "testHistoryDockControl3";
            this.testHistoryDockControl3.Rows = 12;
            this.testHistoryDockControl3.Size = new System.Drawing.Size(150, 225);
            this.testHistoryDockControl3.TabIndex = 2;
            this.testHistoryDockControl3.UndockFrameAutoSize = true;
            this.testHistoryDockControl3.UndockFrameFormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.testHistoryDockControl3.UndockFrameLocation = new System.Drawing.Point(0, 0);
            this.testHistoryDockControl3.UndockFrameMaximizeBox = false;
            this.testHistoryDockControl3.UndockFrameMinimizeBox = false;
            this.testHistoryDockControl3.UndockFrameShowInTaskbar = true;
            this.testHistoryDockControl3.UndockFrameStartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
            this.testHistoryDockControl3.UndockFrameTopMost = true;
            // 
            // OperatorFormTrippleNested
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(949, 560);
            this.Controls.Add(this.splitContainerMain);
            this.Controls.Add(this.panelRightPane);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "OperatorFormTrippleNested";
            this.Text = "Operator Form";
            this.ResizeEnd += new System.EventHandler(this.OperatorFormTrippleNested_ResizeEnd);
            this.panelRightPane.ResumeLayout(false);
            this.panelRightPane.PerformLayout();
            this.splitContainerMain.Panel1.ResumeLayout(false);
            this.splitContainerMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerMain)).EndInit();
            this.splitContainerMain.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.splitContainer3.Panel1.ResumeLayout(false);
            this.splitContainer3.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer3)).EndInit();
            this.splitContainer3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        void systemSignalsControl1_Load(object sender, System.EventArgs e)
        {
            //throw new System.NotImplementedException();
        }

        #endregion

        private VTIWindowsControlLibrary.Components.SystemSignalsControl systemSignalsControl1;
        private System.Windows.Forms.Panel panelRightPane;
        private VTIWindowsControlLibrary.Components.MiniCommandControl miniCommandControl1;
        private System.Windows.Forms.SplitContainer splitContainerMain;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private VTIWindowsControlLibrary.Components.RichTextPrompt richTextPrompt1;
        private System.Windows.Forms.Label labelPortName3;
        private VTIWindowsControlLibrary.Components.RichTextPrompt richTextPrompt3;
        private System.Windows.Forms.Label labelPortName2;
        private VTIWindowsControlLibrary.Components.RichTextPrompt richTextPrompt2;
        private System.Windows.Forms.Label labelPortName1;
        private VTIWindowsControlLibrary.Components.TestHistoryDockControl testHistoryDockControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl dataPlotDockControl1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl dataPlotDockControl2;
        private VTIWindowsControlLibrary.Components.TestHistoryDockControl testHistoryDockControl2;
        private System.Windows.Forms.SplitContainer splitContainer3;
        private VTIWindowsControlLibrary.Components.Graphing.DataPlotDockControl dataPlotDockControl3;
        private VTIWindowsControlLibrary.Components.TestHistoryDockControl testHistoryDockControl3;
        public Components.SequenceStepsControl sequenceStepsControl1;
        public Components.SequenceStepsControl sequenceStepsControl2;
        public Components.SequenceStepsControl sequenceStepsControl3;
    }
}