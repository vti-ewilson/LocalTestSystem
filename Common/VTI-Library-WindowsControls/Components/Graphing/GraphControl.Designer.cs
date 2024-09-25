namespace VTIWindowsControlLibrary.Components.Graphing
{
  partial class GraphControl<TData, TCollection, TTrace, TPoint, TSettings>
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
            this.timerPlotCursor = new System.Windows.Forms.Timer(this.components);
            this.timerPlotCursorUpdates = new System.Windows.Forms.Timer(this.components);
            this.timerSlideXAxis = new System.Windows.Forms.Timer(this.components);
            this.timerSlideYAxis = new System.Windows.Forms.Timer(this.components);
            this.timerResize = new System.Windows.Forms.Timer(this.components);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.flowLayoutPanelLegend = new System.Windows.Forms.FlowLayoutPanel();
            this.contextMenuStripLegend = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemOverlayTraceLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemHideTraceLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemTraceColorLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveOverlayLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemOverlayColorLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemBringToFrontLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemSendToBackLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorTraceZOrderLegend = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemOverlayAllLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemRemoveAllOverlaysLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemShowLegendLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripMenuItemPropertiesLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.contextMenuStripBorders = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItemShowLegend2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.panelAxes = new System.Windows.Forms.Panel();
            this.panelGraph = new System.Windows.Forms.Panel();
            this.contextMenuStripGraph = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.overlayThisTracetoolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideThisTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.traceColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeThisOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.overlayColorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.bringTraceToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendTraceToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorTraceZOrder = new System.Windows.Forms.ToolStripSeparator();
            this.overlayAllTracesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeAllOverlaysToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.commentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemDisplayComments = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorLegend = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripToggleTimeUnits = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItemShowLegend = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transparentPanelYAxis = new VTIWindowsControlLibrary.Components.TransparentPanel();
            this.transparentPanelXAxis = new VTIWindowsControlLibrary.Components.TransparentPanel();
            this.contextMenuStripComments = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.showCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hideCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteCommentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.bringToFrontToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendToBackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.timerTraceHighlight = new System.Windows.Forms.Timer(this.components);
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuStripLegend.SuspendLayout();
            this.panel1.SuspendLayout();
            this.contextMenuStripBorders.SuspendLayout();
            this.panelAxes.SuspendLayout();
            this.contextMenuStripGraph.SuspendLayout();
            this.contextMenuStripComments.SuspendLayout();
            this.SuspendLayout();
            // 
            // timerPlotCursor
            // 
            this.timerPlotCursor.Tick += new System.EventHandler(this.timerPlotCursor_Tick);
            // 
            // timerPlotCursorUpdates
            // 
            this.timerPlotCursorUpdates.Tick += new System.EventHandler(this.timerPlotCursorUpdates_Tick);
            // 
            // timerSlideXAxis
            // 
            this.timerSlideXAxis.Tick += new System.EventHandler(this.timerSlideXAxis_Tick);
            // 
            // timerSlideYAxis
            // 
            this.timerSlideYAxis.Tick += new System.EventHandler(this.timerSlideYAxis_Tick);
            // 
            // timerResize
            // 
            this.timerResize.Tick += new System.EventHandler(this.timerResize_Tick);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.MinimumSize = new System.Drawing.Size(300, 200);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.flowLayoutPanelLegend);
            this.splitContainer1.Panel1.Controls.Add(this.panel1);
            this.splitContainer1.Panel1MinSize = 85;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.panelAxes);
            this.splitContainer1.Panel2MinSize = 200;
            this.splitContainer1.Size = new System.Drawing.Size(500, 300);
            this.splitContainer1.SplitterDistance = 100;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.SplitterMoved += new System.Windows.Forms.SplitterEventHandler(this.splitContainer1_SplitterMoved);
            // 
            // flowLayoutPanelLegend
            // 
            this.flowLayoutPanelLegend.AutoScroll = true;
            this.flowLayoutPanelLegend.AutoSize = true;
            this.flowLayoutPanelLegend.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flowLayoutPanelLegend.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.flowLayoutPanelLegend.ContextMenuStrip = this.contextMenuStripLegend;
            this.flowLayoutPanelLegend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanelLegend.Location = new System.Drawing.Point(0, 26);
            this.flowLayoutPanelLegend.Name = "flowLayoutPanelLegend";
            this.flowLayoutPanelLegend.Padding = new System.Windows.Forms.Padding(5);
            this.flowLayoutPanelLegend.Size = new System.Drawing.Size(100, 274);
            this.flowLayoutPanelLegend.TabIndex = 8;
            // 
            // contextMenuStripLegend
            // 
            this.contextMenuStripLegend.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripLegend.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemOverlayTraceLegend,
            this.toolStripMenuItemHideTraceLegend,
            this.toolStripMenuItemTraceColorLegend,
            this.toolStripMenuItemRemoveOverlayLegend,
            this.toolStripMenuItemOverlayColorLegend,
            this.toolStripMenuItemBringToFrontLegend,
            this.toolStripMenuItemSendToBackLegend,
            this.toolStripSeparatorTraceZOrderLegend,
            this.toolStripMenuItemOverlayAllLegend,
            this.toolStripMenuItemRemoveAllOverlaysLegend,
            this.toolStripSeparator3,
            this.toolStripMenuItemShowLegendLegend,
            this.toolStripSeparator5,
            this.toolStripMenuItemPropertiesLegend});
            this.contextMenuStripLegend.Name = "contextMenuStrip1";
            this.contextMenuStripLegend.Size = new System.Drawing.Size(193, 352);
            // 
            // toolStripMenuItemOverlayTraceLegend
            // 
            this.toolStripMenuItemOverlayTraceLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.add;
            this.toolStripMenuItemOverlayTraceLegend.Name = "toolStripMenuItemOverlayTraceLegend";
            this.toolStripMenuItemOverlayTraceLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemOverlayTraceLegend.Text = "&Overlay This Trace";
            this.toolStripMenuItemOverlayTraceLegend.Click += new System.EventHandler(this.toolStripMenuItemOverlayTraceLegend_Click);
            // 
            // toolStripMenuItemHideTraceLegend
            // 
            this.toolStripMenuItemHideTraceLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.cancel;
            this.toolStripMenuItemHideTraceLegend.Name = "toolStripMenuItemHideTraceLegend";
            this.toolStripMenuItemHideTraceLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemHideTraceLegend.Text = "H&ide This Trace";
            this.toolStripMenuItemHideTraceLegend.Click += new System.EventHandler(this.toolStripMenuItemHideTraceLegend_Click);
            // 
            // toolStripMenuItemTraceColorLegend
            // 
            this.toolStripMenuItemTraceColorLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.color_swatch;
            this.toolStripMenuItemTraceColorLegend.Name = "toolStripMenuItemTraceColorLegend";
            this.toolStripMenuItemTraceColorLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemTraceColorLegend.Text = "Trace &Color";
            this.toolStripMenuItemTraceColorLegend.Click += new System.EventHandler(this.toolStripMenuItemTraceColorLegend_Click);
            // 
            // toolStripMenuItemRemoveOverlayLegend
            // 
            this.toolStripMenuItemRemoveOverlayLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
            this.toolStripMenuItemRemoveOverlayLegend.Name = "toolStripMenuItemRemoveOverlayLegend";
            this.toolStripMenuItemRemoveOverlayLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemRemoveOverlayLegend.Text = "&Remove This Overlay";
            this.toolStripMenuItemRemoveOverlayLegend.Click += new System.EventHandler(this.toolStripMenuItemRemoveOverlayLegend_Click);
            // 
            // toolStripMenuItemOverlayColorLegend
            // 
            this.toolStripMenuItemOverlayColorLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.color_swatch;
            this.toolStripMenuItemOverlayColorLegend.Name = "toolStripMenuItemOverlayColorLegend";
            this.toolStripMenuItemOverlayColorLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemOverlayColorLegend.Text = "Overlay Co&lor";
            this.toolStripMenuItemOverlayColorLegend.Click += new System.EventHandler(this.toolStripMenuItemOverlayColorLegend_Click);
            // 
            // toolStripMenuItemBringToFrontLegend
            // 
            this.toolStripMenuItemBringToFrontLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.BringToFront;
            this.toolStripMenuItemBringToFrontLegend.Name = "toolStripMenuItemBringToFrontLegend";
            this.toolStripMenuItemBringToFrontLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemBringToFrontLegend.Text = "Bring to &Front";
            this.toolStripMenuItemBringToFrontLegend.Click += new System.EventHandler(this.toolStripMenuItemBringToFrontLegend_Click);
            // 
            // toolStripMenuItemSendToBackLegend
            // 
            this.toolStripMenuItemSendToBackLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.SendToBack;
            this.toolStripMenuItemSendToBackLegend.Name = "toolStripMenuItemSendToBackLegend";
            this.toolStripMenuItemSendToBackLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemSendToBackLegend.Text = "Send to &Back";
            this.toolStripMenuItemSendToBackLegend.Click += new System.EventHandler(this.toolStripMenuItemSendToBackLegend_Click);
            // 
            // toolStripSeparatorTraceZOrderLegend
            // 
            this.toolStripSeparatorTraceZOrderLegend.Name = "toolStripSeparatorTraceZOrderLegend";
            this.toolStripSeparatorTraceZOrderLegend.Size = new System.Drawing.Size(189, 6);
            // 
            // toolStripMenuItemOverlayAllLegend
            // 
            this.toolStripMenuItemOverlayAllLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.add;
            this.toolStripMenuItemOverlayAllLegend.Name = "toolStripMenuItemOverlayAllLegend";
            this.toolStripMenuItemOverlayAllLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemOverlayAllLegend.Text = "Overlay &All Traces";
            this.toolStripMenuItemOverlayAllLegend.Click += new System.EventHandler(this.toolStripMenuItemOverlayAllLegend_Click);
            // 
            // toolStripMenuItemRemoveAllOverlaysLegend
            // 
            this.toolStripMenuItemRemoveAllOverlaysLegend.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
            this.toolStripMenuItemRemoveAllOverlaysLegend.Name = "toolStripMenuItemRemoveAllOverlaysLegend";
            this.toolStripMenuItemRemoveAllOverlaysLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemRemoveAllOverlaysLegend.Text = "R&emove All Overlays";
            this.toolStripMenuItemRemoveAllOverlaysLegend.Visible = false;
            this.toolStripMenuItemRemoveAllOverlaysLegend.Click += new System.EventHandler(this.toolStripMenuItemRemoveAllOverlaysLegend_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(189, 6);
            // 
            // toolStripMenuItemShowLegendLegend
            // 
            this.toolStripMenuItemShowLegendLegend.Checked = true;
            this.toolStripMenuItemShowLegendLegend.CheckOnClick = true;
            this.toolStripMenuItemShowLegendLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemShowLegendLegend.Name = "toolStripMenuItemShowLegendLegend";
            this.toolStripMenuItemShowLegendLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemShowLegendLegend.Text = "Show Legend";
            this.toolStripMenuItemShowLegendLegend.CheckedChanged += new System.EventHandler(this.toolStripMenuItemShowLegendLegend_CheckedChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(189, 6);
            // 
            // toolStripMenuItemPropertiesLegend
            // 
            this.toolStripMenuItemPropertiesLegend.Name = "toolStripMenuItemPropertiesLegend";
            this.toolStripMenuItemPropertiesLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemPropertiesLegend.Text = "P&roperties";
            this.toolStripMenuItemPropertiesLegend.Click += new System.EventHandler(this.toolStripMenuItemPropertiesLegend_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 26);
            this.panel1.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.ContextMenuStrip = this.contextMenuStripBorders;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(0, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(96, 22);
            this.label2.TabIndex = 8;
            this.label2.Text = "Legend";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // contextMenuStripBorders
            // 
            this.contextMenuStripBorders.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripBorders.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItemShowLegend2,
            this.toolStripMenuItem2});
            this.contextMenuStripBorders.Name = "contextMenuStrip2";
            this.contextMenuStripBorders.Size = new System.Drawing.Size(146, 48);
            // 
            // toolStripMenuItemShowLegend2
            // 
            this.toolStripMenuItemShowLegend2.Checked = true;
            this.toolStripMenuItemShowLegend2.CheckOnClick = true;
            this.toolStripMenuItemShowLegend2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemShowLegend2.Name = "toolStripMenuItemShowLegend2";
            this.toolStripMenuItemShowLegend2.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItemShowLegend2.Text = "Show Legend";
            this.toolStripMenuItemShowLegend2.Click += new System.EventHandler(this.toolStripMenuItemShowLegend2_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(145, 22);
            this.toolStripMenuItem2.Text = "P&roperties";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.toolStripMenuItem2_Click);
            // 
            // panelAxes
            // 
            this.panelAxes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panelAxes.ContextMenuStrip = this.contextMenuStripBorders;
            this.panelAxes.Controls.Add(this.panelGraph);
            this.panelAxes.Controls.Add(this.transparentPanelYAxis);
            this.panelAxes.Controls.Add(this.transparentPanelXAxis);
            this.panelAxes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelAxes.Location = new System.Drawing.Point(0, 0);
            this.panelAxes.Name = "panelAxes";
            this.panelAxes.Padding = new System.Windows.Forms.Padding(40, 10, 10, 10);
            this.panelAxes.Size = new System.Drawing.Size(396, 300);
            this.panelAxes.TabIndex = 0;
            this.panelAxes.Paint += new System.Windows.Forms.PaintEventHandler(this.panelAxes_Paint);
            // 
            // panelGraph
            // 
            this.panelGraph.BackColor = System.Drawing.Color.White;
            this.panelGraph.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelGraph.ContextMenuStrip = this.contextMenuStripGraph;
            this.panelGraph.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraph.Location = new System.Drawing.Point(70, 10);
            this.panelGraph.Name = "panelGraph";
            this.panelGraph.Size = new System.Drawing.Size(312, 251);
            this.panelGraph.TabIndex = 15;
            this.panelGraph.Click += new System.EventHandler(this.panelGraph_Click);
            this.panelGraph.Paint += new System.Windows.Forms.PaintEventHandler(this.panelGraph_Paint);
            this.panelGraph.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.panelGraph_MouseDoubleClick);
            this.panelGraph.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelGraph_MouseDown);
            this.panelGraph.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelGraph_MouseMove);
            this.panelGraph.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelGraph_MouseUp);
            this.panelGraph.Resize += new System.EventHandler(this.panelGraph_Resize);
            // 
            // contextMenuStripGraph
            // 
            this.contextMenuStripGraph.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripGraph.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.overlayThisTracetoolStripMenuItem,
            this.hideThisTraceToolStripMenuItem,
            this.traceColorToolStripMenuItem,
            this.removeThisOverlayToolStripMenuItem,
            this.overlayColorToolStripMenuItem,
            this.bringTraceToFrontToolStripMenuItem,
            this.sendTraceToBackToolStripMenuItem,
            this.toolStripSeparatorTraceZOrder,
            this.overlayAllTracesToolStripMenuItem,
            this.removeAllOverlaysToolStripMenuItem,
            this.toolStripSeparator1,
            this.commentToolStripMenuItem,
            this.toolStripMenuItemDisplayComments,
            this.toolStripSeparatorLegend,
            this.toolStripToggleTimeUnits,
            this.toolStripMenuItemShowLegend,
            this.toolStripSeparator12,
            this.propertiesToolStripMenuItem});
            this.contextMenuStripGraph.Name = "contextMenuStrip1";
            this.contextMenuStripGraph.Size = new System.Drawing.Size(193, 448);
            // 
            // overlayThisTracetoolStripMenuItem
            // 
            this.overlayThisTracetoolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.add;
            this.overlayThisTracetoolStripMenuItem.Name = "overlayThisTracetoolStripMenuItem";
            this.overlayThisTracetoolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.overlayThisTracetoolStripMenuItem.Text = "&Overlay This Trace";
            this.overlayThisTracetoolStripMenuItem.Visible = false;
            this.overlayThisTracetoolStripMenuItem.Click += new System.EventHandler(this.overlayThisTracetoolStripMenuItem_Click);
            // 
            // hideThisTraceToolStripMenuItem
            // 
            this.hideThisTraceToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.cancel;
            this.hideThisTraceToolStripMenuItem.Name = "hideThisTraceToolStripMenuItem";
            this.hideThisTraceToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.hideThisTraceToolStripMenuItem.Text = "H&ide This Trace";
            this.hideThisTraceToolStripMenuItem.Visible = false;
            this.hideThisTraceToolStripMenuItem.Click += new System.EventHandler(this.hideThisTraceToolStripMenuItem_Click);
            // 
            // traceColorToolStripMenuItem
            // 
            this.traceColorToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.color_swatch;
            this.traceColorToolStripMenuItem.Name = "traceColorToolStripMenuItem";
            this.traceColorToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.traceColorToolStripMenuItem.Text = "Trace &Color";
            this.traceColorToolStripMenuItem.Visible = false;
            this.traceColorToolStripMenuItem.Click += new System.EventHandler(this.traceColorToolStripMenuItem_Click);
            // 
            // removeThisOverlayToolStripMenuItem
            // 
            this.removeThisOverlayToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
            this.removeThisOverlayToolStripMenuItem.Name = "removeThisOverlayToolStripMenuItem";
            this.removeThisOverlayToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.removeThisOverlayToolStripMenuItem.Text = "&Remove This Overlay";
            this.removeThisOverlayToolStripMenuItem.Visible = false;
            this.removeThisOverlayToolStripMenuItem.Click += new System.EventHandler(this.removeThisOverlayToolStripMenuItem_Click);
            // 
            // overlayColorToolStripMenuItem
            // 
            this.overlayColorToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.color_swatch;
            this.overlayColorToolStripMenuItem.Name = "overlayColorToolStripMenuItem";
            this.overlayColorToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.overlayColorToolStripMenuItem.Text = "Overlay Co&lor";
            this.overlayColorToolStripMenuItem.Visible = false;
            this.overlayColorToolStripMenuItem.Click += new System.EventHandler(this.overlayColorToolStripMenuItem_Click);
            // 
            // bringTraceToFrontToolStripMenuItem
            // 
            this.bringTraceToFrontToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.BringToFront;
            this.bringTraceToFrontToolStripMenuItem.Name = "bringTraceToFrontToolStripMenuItem";
            this.bringTraceToFrontToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.bringTraceToFrontToolStripMenuItem.Text = "Bring to &Front";
            this.bringTraceToFrontToolStripMenuItem.Visible = false;
            this.bringTraceToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringTraceToFrontToolStripMenuItem_Click);
            // 
            // sendTraceToBackToolStripMenuItem
            // 
            this.sendTraceToBackToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.SendToBack;
            this.sendTraceToBackToolStripMenuItem.Name = "sendTraceToBackToolStripMenuItem";
            this.sendTraceToBackToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.sendTraceToBackToolStripMenuItem.Text = "Send to &Back";
            this.sendTraceToBackToolStripMenuItem.Visible = false;
            this.sendTraceToBackToolStripMenuItem.Click += new System.EventHandler(this.sendTraceToBackToolStripMenuItem_Click);
            // 
            // toolStripSeparatorTraceZOrder
            // 
            this.toolStripSeparatorTraceZOrder.Name = "toolStripSeparatorTraceZOrder";
            this.toolStripSeparatorTraceZOrder.Size = new System.Drawing.Size(189, 6);
            this.toolStripSeparatorTraceZOrder.Visible = false;
            // 
            // overlayAllTracesToolStripMenuItem
            // 
            this.overlayAllTracesToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.add;
            this.overlayAllTracesToolStripMenuItem.Name = "overlayAllTracesToolStripMenuItem";
            this.overlayAllTracesToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.overlayAllTracesToolStripMenuItem.Text = "Overlay &All Traces";
            this.overlayAllTracesToolStripMenuItem.Click += new System.EventHandler(this.overlayAllTracesToolStripMenuItem_Click);
            // 
            // removeAllOverlaysToolStripMenuItem
            // 
            this.removeAllOverlaysToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
            this.removeAllOverlaysToolStripMenuItem.Name = "removeAllOverlaysToolStripMenuItem";
            this.removeAllOverlaysToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.removeAllOverlaysToolStripMenuItem.Text = "R&emove All Overlays";
            this.removeAllOverlaysToolStripMenuItem.Click += new System.EventHandler(this.removeAllOverlaysToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(189, 6);
            // 
            // commentToolStripMenuItem
            // 
            this.commentToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.add;
            this.commentToolStripMenuItem.Name = "commentToolStripMenuItem";
            this.commentToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.commentToolStripMenuItem.Text = "&Add Comment";
            this.commentToolStripMenuItem.Click += new System.EventHandler(this.commentToolStripMenuItem_Click);
            // 
            // toolStripMenuItemDisplayComments
            // 
            this.toolStripMenuItemDisplayComments.Checked = true;
            this.toolStripMenuItemDisplayComments.CheckOnClick = true;
            this.toolStripMenuItemDisplayComments.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemDisplayComments.Name = "toolStripMenuItemDisplayComments";
            this.toolStripMenuItemDisplayComments.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemDisplayComments.Text = "&Display Comments";
            this.toolStripMenuItemDisplayComments.Click += new System.EventHandler(this.toolStripMenuItemDisplayComments_CheckedChanged);
            // 
            // toolStripSeparatorLegend
            // 
            this.toolStripSeparatorLegend.Name = "toolStripSeparatorLegend";
            this.toolStripSeparatorLegend.Size = new System.Drawing.Size(189, 6);
            // 
            // toolStripToggleTimeUnits
            // 
            this.toolStripToggleTimeUnits.Name = "toolStripToggleTimeUnits";
            this.toolStripToggleTimeUnits.Size = new System.Drawing.Size(192, 30);
            this.toolStripToggleTimeUnits.Text = "&Toggle Time Units";
            this.toolStripToggleTimeUnits.Click += new System.EventHandler(this.toolStripToggleTimeUnits_Click);
            // 
            // toolStripMenuItemShowLegend
            // 
            this.toolStripMenuItemShowLegend.Checked = true;
            this.toolStripMenuItemShowLegend.CheckOnClick = true;
            this.toolStripMenuItemShowLegend.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripMenuItemShowLegend.Name = "toolStripMenuItemShowLegend";
            this.toolStripMenuItemShowLegend.Size = new System.Drawing.Size(192, 30);
            this.toolStripMenuItemShowLegend.Text = "Show Legend";
            this.toolStripMenuItemShowLegend.CheckedChanged += new System.EventHandler(this.toolStripMenuItemShowLegend_CheckedChanged);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(189, 6);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(192, 30);
            this.propertiesToolStripMenuItem.Text = "P&roperties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // transparentPanelYAxis
            // 
            this.transparentPanelYAxis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transparentPanelYAxis.Dock = System.Windows.Forms.DockStyle.Left;
            this.transparentPanelYAxis.Location = new System.Drawing.Point(40, 10);
            this.transparentPanelYAxis.MinimumSize = new System.Drawing.Size(21, 21);
            this.transparentPanelYAxis.Name = "transparentPanelYAxis";
            this.transparentPanelYAxis.Size = new System.Drawing.Size(30, 251);
            this.transparentPanelYAxis.TabIndex = 16;
            this.transparentPanelYAxis.Paint += new System.Windows.Forms.PaintEventHandler(this.transparentPanelYAxis_Paint);
            this.transparentPanelYAxis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.transparentPanelYAxis_MouseDown);
            this.transparentPanelYAxis.MouseMove += new System.Windows.Forms.MouseEventHandler(this.transparentPanelYAxis_MouseMove);
            this.transparentPanelYAxis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.transparentPanelYAxis_MouseUp);
            // 
            // transparentPanelXAxis
            // 
            this.transparentPanelXAxis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.transparentPanelXAxis.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.transparentPanelXAxis.Location = new System.Drawing.Point(40, 261);
            this.transparentPanelXAxis.Name = "transparentPanelXAxis";
            this.transparentPanelXAxis.Size = new System.Drawing.Size(342, 25);
            this.transparentPanelXAxis.TabIndex = 14;
            this.transparentPanelXAxis.Paint += new System.Windows.Forms.PaintEventHandler(this.transparentPanelXAxis_Paint);
            this.transparentPanelXAxis.MouseDown += new System.Windows.Forms.MouseEventHandler(this.transparentPanelXAxis_MouseDown);
            this.transparentPanelXAxis.MouseMove += new System.Windows.Forms.MouseEventHandler(this.transparentPanelXAxis_MouseMove);
            this.transparentPanelXAxis.MouseUp += new System.Windows.Forms.MouseEventHandler(this.transparentPanelXAxis_MouseUp);
            // 
            // contextMenuStripComments
            // 
            this.contextMenuStripComments.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStripComments.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.showCommentToolStripMenuItem,
            this.hideCommentToolStripMenuItem,
            this.deleteCommentToolStripMenuItem,
            this.toolStripSeparator11,
            this.bringToFrontToolStripMenuItem,
            this.sendToBackToolStripMenuItem});
            this.contextMenuStripComments.Name = "contextMenuStripComments";
            this.contextMenuStripComments.Size = new System.Drawing.Size(173, 160);
            // 
            // showCommentToolStripMenuItem
            // 
            this.showCommentToolStripMenuItem.Name = "showCommentToolStripMenuItem";
            this.showCommentToolStripMenuItem.Size = new System.Drawing.Size(172, 30);
            this.showCommentToolStripMenuItem.Text = "Show Comment";
            this.showCommentToolStripMenuItem.Click += new System.EventHandler(this.showCommentToolStripMenuItem_Click);
            // 
            // hideCommentToolStripMenuItem
            // 
            this.hideCommentToolStripMenuItem.Name = "hideCommentToolStripMenuItem";
            this.hideCommentToolStripMenuItem.Size = new System.Drawing.Size(172, 30);
            this.hideCommentToolStripMenuItem.Text = "Hide Comment";
            this.hideCommentToolStripMenuItem.Click += new System.EventHandler(this.hideCommentToolStripMenuItem_Click);
            // 
            // deleteCommentToolStripMenuItem
            // 
            this.deleteCommentToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
            this.deleteCommentToolStripMenuItem.Name = "deleteCommentToolStripMenuItem";
            this.deleteCommentToolStripMenuItem.Size = new System.Drawing.Size(172, 30);
            this.deleteCommentToolStripMenuItem.Text = "Delete Comment";
            this.deleteCommentToolStripMenuItem.Click += new System.EventHandler(this.deleteCommentToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(169, 6);
            // 
            // bringToFrontToolStripMenuItem
            // 
            this.bringToFrontToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.BringToFront;
            this.bringToFrontToolStripMenuItem.Name = "bringToFrontToolStripMenuItem";
            this.bringToFrontToolStripMenuItem.Size = new System.Drawing.Size(172, 30);
            this.bringToFrontToolStripMenuItem.Text = "Bring to Front";
            this.bringToFrontToolStripMenuItem.Click += new System.EventHandler(this.bringToFrontToolStripMenuItem_Click);
            // 
            // sendToBackToolStripMenuItem
            // 
            this.sendToBackToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.SendToBack;
            this.sendToBackToolStripMenuItem.Name = "sendToBackToolStripMenuItem";
            this.sendToBackToolStripMenuItem.Size = new System.Drawing.Size(172, 30);
            this.sendToBackToolStripMenuItem.Text = "Send to Back";
            this.sendToBackToolStripMenuItem.Click += new System.EventHandler(this.sendToBackToolStripMenuItem_Click);
            // 
            // timerTraceHighlight
            // 
            this.timerTraceHighlight.Tick += new System.EventHandler(this.timerTraceHighlight_Tick);
            // 
            // printDocument1
            // 
            this.printDocument1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.printDocument1_PrintPage);
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // GraphControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "GraphControl";
            this.Size = new System.Drawing.Size(500, 300);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuStripLegend.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.contextMenuStripBorders.ResumeLayout(false);
            this.panelAxes.ResumeLayout(false);
            this.contextMenuStripGraph.ResumeLayout(false);
            this.contextMenuStripComments.ResumeLayout(false);
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.Timer timerPlotCursor;
    private System.Windows.Forms.Timer timerPlotCursorUpdates;
    private System.Windows.Forms.Timer timerSlideXAxis;
    private System.Windows.Forms.Timer timerSlideYAxis;
    private System.Windows.Forms.Timer timerResize;
    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelLegend;
    private System.Windows.Forms.Panel panelAxes;
    private System.Windows.Forms.Panel panelGraph;
    private TransparentPanel transparentPanelXAxis;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripGraph;
    private System.Windows.Forms.ToolStripMenuItem commentToolStripMenuItem;
    //private System.Windows.Forms.ToolStripMenuItem showAllCommentsToolStripMenuItem;
    //private System.Windows.Forms.ToolStripMenuItem hideAllCommentsToolStripMenuItem;
    public System.Windows.Forms.ToolStripMenuItem toolStripMenuItemDisplayComments;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripToggleTimeUnits;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowLegend;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
    private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripBorders;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowLegend2;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripComments;
    private System.Windows.Forms.ToolStripMenuItem showCommentToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem hideCommentToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteCommentToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
    private System.Windows.Forms.ToolStripMenuItem bringToFrontToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem sendToBackToolStripMenuItem;
    private System.Windows.Forms.Timer timerTraceHighlight;
    private System.Windows.Forms.ToolStripMenuItem overlayThisTracetoolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem hideThisTraceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem traceColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeThisOverlayToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem overlayColorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem overlayAllTracesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem removeAllOverlaysToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    private System.Windows.Forms.ColorDialog colorDialog1;
    private System.Windows.Forms.ToolStripMenuItem bringTraceToFrontToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem sendTraceToBackToolStripMenuItem;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorTraceZOrder;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOverlayTraceLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemHideTraceLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemTraceColorLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveOverlayLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOverlayColorLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemBringToFrontLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemSendToBackLegend;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparatorTraceZOrderLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemOverlayAllLegend;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemRemoveAllOverlaysLegend;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemShowLegendLegend;
    private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
    private System.Windows.Forms.ToolStripMenuItem toolStripMenuItemPropertiesLegend;
    private System.Drawing.Printing.PrintDocument printDocument1;
    private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
    private System.Windows.Forms.PrintDialog printDialog1;
        public TransparentPanel transparentPanelYAxis;
    }
}
