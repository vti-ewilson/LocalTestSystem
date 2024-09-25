namespace VTIWindowsControlLibrary.Components.Graphing
{
    partial class DataPlotControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataPlotControl));
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.timerCollectData = new System.Windows.Forms.Timer(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelCaption = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.pageSetupToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorClose = new System.Windows.Forms.ToolStripSeparator();
            this.dataPlotToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorRunStopMenu = new System.Windows.Forms.ToolStripSeparator();
            this.autoRun1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.autoRun2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorAutoRunMenu = new System.Windows.Forms.ToolStripSeparator();
            this.autoShowAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorShowAllMenu = new System.Windows.Forms.ToolStripSeparator();
            this.autoShowEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showEndToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorShowEndMenu = new System.Windows.Forms.ToolStripSeparator();
            this.yAxisLinearModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.yAxisLogModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.overlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.fromFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allTracesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearOverlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toggleAutoRunMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.assignYAxisPrintLabelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparatorMenus = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorRunStop = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorAutoRun = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorShowAll = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorShowEnd = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorLinLog = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.dataPlotToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripButtonRun = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStop = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoRun1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoRun2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoShowAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonAutoShowEnd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonShowEnd = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLinear = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonLog = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonClose = new System.Windows.Forms.ToolStripButton();
            this.YMinDn = new System.Windows.Forms.Button();
            this.YMinUp = new System.Windows.Forms.Button();
            this.YMaxDn = new System.Windows.Forms.Button();
            this.YMaxUp = new System.Windows.Forms.Button();
            this.YMaxExpUp = new System.Windows.Forms.Button();
            this.YMinExpUp = new System.Windows.Forms.Button();
            this.YMaxExpDn = new System.Windows.Forms.Button();
            this.YMinExpDn = new System.Windows.Forms.Button();
            this.graphControl1 = new VTIWindowsControlLibrary.Components.Graphing.DataPlotGraphControl();
            this.graphControl1.AssignPlotPropForm(frmPlotProp1);
            this.graphControl1.GraphData.Settings.StoreDataPlot(frmPlotProp1.DataPlot);
            this.graphControl1.StoreDataPlot(frmPlotProp1.DataPlot);
            this.YMaxUpLbl = new System.Windows.Forms.Label();
            this.YMinUpLbl = new System.Windows.Forms.Label();
            this.YMaxExpUpLbl = new System.Windows.Forms.Label();
            this.YMinExpUpLbl = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "aio";
            this.saveFileDialog1.Filter = "Data Plot XML files (*.aiox)|*.aiox|Data Plot AIO files (*.aio)|*.aio|CSV (*.csv)|*.csv|All Files (" +
    "*.*)|*.*";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "aiox";
            this.openFileDialog1.Filter = "Data Plot XML files (*.aiox)|*.aiox|Data Plot AIO files (*.aio)|*.aio";
            // 
            // timerCollectData
            // 
            this.timerCollectData.Tick += new System.EventHandler(this.timerCollectData_Tick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelCaption});
            this.statusStrip1.Location = new System.Drawing.Point(0, 371);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(652, 22);
            this.statusStrip1.TabIndex = 10;
            this.statusStrip1.Text = "statusStrip1";
            this.statusStrip1.Visible = false;
            // 
            // toolStripStatusLabelCaption
            // 
            this.toolStripStatusLabelCaption.Name = "toolStripStatusLabelCaption";
            this.toolStripStatusLabelCaption.Size = new System.Drawing.Size(55, 17);
            this.toolStripStatusLabelCaption.Text = "Data Plot";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataPlotToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(652, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.Visible = false;
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripSeparator10,
            this.pageSetupToolStripMenuItem1,
            this.printToolStripMenuItem1,
            this.toolStripSeparator11,
            this.closeToolStripMenuItem,
            this.toolStripSeparatorClose});
            this.fileToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.MatchOnly;
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.save;
            this.saveToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveToolStripMenuItem.MergeIndex = 2;
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveToolStripMenuItem.Text = "&Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.saveAsToolStripMenuItem.MergeIndex = 3;
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.saveAsToolStripMenuItem.Text = "Save &As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator10.MergeIndex = 4;
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(139, 6);
            // 
            // pageSetupToolStripMenuItem1
            // 
            this.pageSetupToolStripMenuItem1.Image = global::VTIWindowsControlLibrary.Properties.Resources.pagesetup;
            this.pageSetupToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.pageSetupToolStripMenuItem1.MergeIndex = 5;
            this.pageSetupToolStripMenuItem1.Name = "pageSetupToolStripMenuItem1";
            this.pageSetupToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.pageSetupToolStripMenuItem1.Text = "Page Set&up...";
            this.pageSetupToolStripMenuItem1.Click += new System.EventHandler(this.pageSetupToolStripMenuItem1_Click);
            // 
            // printToolStripMenuItem1
            // 
            this.printToolStripMenuItem1.Image = global::VTIWindowsControlLibrary.Properties.Resources.print;
            this.printToolStripMenuItem1.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.printToolStripMenuItem1.MergeIndex = 6;
            this.printToolStripMenuItem1.Name = "printToolStripMenuItem1";
            this.printToolStripMenuItem1.Size = new System.Drawing.Size(142, 22);
            this.printToolStripMenuItem1.Text = "&Print...";
            this.printToolStripMenuItem1.Click += new System.EventHandler(this.printToolStripMenuItem1_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparator11.MergeIndex = 7;
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(139, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.closeToolStripMenuItem.MergeIndex = 8;
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.closeToolStripMenuItem.Text = "&Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // toolStripSeparatorClose
            // 
            this.toolStripSeparatorClose.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.toolStripSeparatorClose.MergeIndex = 9;
            this.toolStripSeparatorClose.Name = "toolStripSeparatorClose";
            this.toolStripSeparatorClose.Size = new System.Drawing.Size(139, 6);
            // 
            // dataPlotToolStripMenuItem
            // 
            this.dataPlotToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.stopToolStripMenuItem,
            this.toolStripSeparatorRunStopMenu,
            this.autoRun1ToolStripMenuItem,
            this.autoRun2ToolStripMenuItem,
            this.toolStripSeparatorAutoRunMenu,
            this.autoShowAllToolStripMenuItem,
            this.showAllToolStripMenuItem,
            this.toolStripSeparatorShowAllMenu,
            this.autoShowEndToolStripMenuItem,
            this.showEndToolStripMenuItem,
            this.toolStripSeparatorShowEndMenu,
            this.yAxisLinearModeToolStripMenuItem,
            this.yAxisLogModeToolStripMenuItem,
            this.toolStripSeparator1,
            this.overlayToolStripMenuItem,
            this.toggleAutoRunMenuItem,
            this.assignYAxisPrintLabelMenuItem,
            this.propertiesToolStripMenuItem});
            this.dataPlotToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.dataPlotToolStripMenuItem.MergeIndex = 1;
            this.dataPlotToolStripMenuItem.Name = "dataPlotToolStripMenuItem";
            this.dataPlotToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.dataPlotToolStripMenuItem.Text = "&Data Plot";
            // 
            // runToolStripMenuItem
            // 
            this.runToolStripMenuItem.CheckOnClick = true;
            this.runToolStripMenuItem.Name = "runToolStripMenuItem";
            this.runToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.runToolStripMenuItem.Text = "&Run";
            this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
            // 
            // stopToolStripMenuItem
            // 
            this.stopToolStripMenuItem.CheckOnClick = true;
            this.stopToolStripMenuItem.Name = "stopToolStripMenuItem";
            this.stopToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.stopToolStripMenuItem.Text = "&Stop";
            this.stopToolStripMenuItem.Click += new System.EventHandler(this.stopToolStripMenuItem_Click);
            // 
            // toolStripSeparatorRunStopMenu
            // 
            this.toolStripSeparatorRunStopMenu.Name = "toolStripSeparatorRunStopMenu";
            this.toolStripSeparatorRunStopMenu.Size = new System.Drawing.Size(201, 6);
            // 
            // autoRun1ToolStripMenuItem
            // 
            this.autoRun1ToolStripMenuItem.CheckOnClick = true;
            this.autoRun1ToolStripMenuItem.Name = "autoRun1ToolStripMenuItem";
            this.autoRun1ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.autoRun1ToolStripMenuItem.Text = "Auto Run &1";
            this.autoRun1ToolStripMenuItem.Click += new System.EventHandler(this.autoRun1ToolStripMenuItem_Click);
            // 
            // autoRun2ToolStripMenuItem
            // 
            this.autoRun2ToolStripMenuItem.CheckOnClick = true;
            this.autoRun2ToolStripMenuItem.Name = "autoRun2ToolStripMenuItem";
            this.autoRun2ToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.autoRun2ToolStripMenuItem.Text = "Auto Run &2";
            this.autoRun2ToolStripMenuItem.Click += new System.EventHandler(this.autoRun2ToolStripMenuItem_Click);
            // 
            // toolStripSeparatorAutoRunMenu
            // 
            this.toolStripSeparatorAutoRunMenu.Name = "toolStripSeparatorAutoRunMenu";
            this.toolStripSeparatorAutoRunMenu.Size = new System.Drawing.Size(201, 6);
            // 
            // autoShowAllToolStripMenuItem
            // 
            this.autoShowAllToolStripMenuItem.CheckOnClick = true;
            this.autoShowAllToolStripMenuItem.Name = "autoShowAllToolStripMenuItem";
            this.autoShowAllToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.autoShowAllToolStripMenuItem.Text = "Auto Show &All";
            this.autoShowAllToolStripMenuItem.Click += new System.EventHandler(this.autoShowAllToolStripMenuItem_Click);
            // 
            // showAllToolStripMenuItem
            // 
            this.showAllToolStripMenuItem.Name = "showAllToolStripMenuItem";
            this.showAllToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showAllToolStripMenuItem.Text = "Show A&ll";
            this.showAllToolStripMenuItem.Click += new System.EventHandler(this.showAllToolStripMenuItem_Click);
            // 
            // toolStripSeparatorShowAllMenu
            // 
            this.toolStripSeparatorShowAllMenu.Name = "toolStripSeparatorShowAllMenu";
            this.toolStripSeparatorShowAllMenu.Size = new System.Drawing.Size(201, 6);
            // 
            // autoShowEndToolStripMenuItem
            // 
            this.autoShowEndToolStripMenuItem.CheckOnClick = true;
            this.autoShowEndToolStripMenuItem.Name = "autoShowEndToolStripMenuItem";
            this.autoShowEndToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.autoShowEndToolStripMenuItem.Text = "Auto Show &End";
            this.autoShowEndToolStripMenuItem.Click += new System.EventHandler(this.autoShowEndToolStripMenuItem_Click);
            // 
            // showEndToolStripMenuItem
            // 
            this.showEndToolStripMenuItem.Name = "showEndToolStripMenuItem";
            this.showEndToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.showEndToolStripMenuItem.Text = "Show E&nd";
            this.showEndToolStripMenuItem.Click += new System.EventHandler(this.showEndToolStripMenuItem_Click);
            // 
            // toolStripSeparatorShowEndMenu
            // 
            this.toolStripSeparatorShowEndMenu.Name = "toolStripSeparatorShowEndMenu";
            this.toolStripSeparatorShowEndMenu.Size = new System.Drawing.Size(201, 6);
            // 
            // yAxisLinearModeToolStripMenuItem
            // 
            this.yAxisLinearModeToolStripMenuItem.CheckOnClick = true;
            this.yAxisLinearModeToolStripMenuItem.Name = "yAxisLinearModeToolStripMenuItem";
            this.yAxisLinearModeToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.yAxisLinearModeToolStripMenuItem.Text = "Y-Axis L&inear Mode";
            this.yAxisLinearModeToolStripMenuItem.Click += new System.EventHandler(this.yAxisLinearModeToolStripMenuItem_Click);
            // 
            // yAxisLogModeToolStripMenuItem
            // 
            this.yAxisLogModeToolStripMenuItem.CheckOnClick = true;
            this.yAxisLogModeToolStripMenuItem.Name = "yAxisLogModeToolStripMenuItem";
            this.yAxisLogModeToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.yAxisLogModeToolStripMenuItem.Text = "Y-Axis L&og Mode";
            this.yAxisLogModeToolStripMenuItem.Click += new System.EventHandler(this.yAxisLogModeToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(201, 6);
            // 
            // overlayToolStripMenuItem
            // 
            this.overlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fromFileToolStripMenuItem,
            this.addOverlayToolStripMenuItem,
            this.clearOverlayToolStripMenuItem});
            this.overlayToolStripMenuItem.Name = "overlayToolStripMenuItem";
            this.overlayToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.overlayToolStripMenuItem.Text = "&Overlay";
            // 
            // fromFileToolStripMenuItem
            // 
            this.fromFileToolStripMenuItem.Name = "fromFileToolStripMenuItem";
            this.fromFileToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.fromFileToolStripMenuItem.Text = "&From File...";
            this.fromFileToolStripMenuItem.Click += new System.EventHandler(this.fromFileToolStripMenuItem_Click);
            // 
            // addOverlayToolStripMenuItem
            // 
            this.addOverlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allTracesToolStripMenuItem});
            this.addOverlayToolStripMenuItem.Name = "addOverlayToolStripMenuItem";
            this.addOverlayToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.addOverlayToolStripMenuItem.Text = "&Add Overlay";
            // 
            // allTracesToolStripMenuItem
            // 
            this.allTracesToolStripMenuItem.Name = "allTracesToolStripMenuItem";
            this.allTracesToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.allTracesToolStripMenuItem.Text = "&All Traces";
            this.allTracesToolStripMenuItem.Click += new System.EventHandler(this.allTracesToolStripMenuItem_Click);
            // 
            // clearOverlayToolStripMenuItem
            // 
            this.clearOverlayToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allToolStripMenuItem});
            this.clearOverlayToolStripMenuItem.Name = "clearOverlayToolStripMenuItem";
            this.clearOverlayToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.clearOverlayToolStripMenuItem.Text = "&Clear Overlay";
            // 
            // allToolStripMenuItem
            // 
            this.allToolStripMenuItem.Name = "allToolStripMenuItem";
            this.allToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.allToolStripMenuItem.Text = "&All Overlays";
            this.allToolStripMenuItem.Click += new System.EventHandler(this.allToolStripMenuItem_Click);
            // 
            // toggleAutoRunMenuItem
            // 
            this.toggleAutoRunMenuItem.Name = "toggleAutoRunMenuItem";
            this.toggleAutoRunMenuItem.Size = new System.Drawing.Size(204, 22);
            this.toggleAutoRunMenuItem.Text = "&Enable AutoRun";
            this.toggleAutoRunMenuItem.Click += new System.EventHandler(this.toggleAutoRunMenuItem_Click);
            // 
            // assignYAxisPrintLabelMenuItem
            // 
            this.assignYAxisPrintLabelMenuItem.Name = "assignYAxisPrintLabelMenuItem";
            this.assignYAxisPrintLabelMenuItem.Size = new System.Drawing.Size(204, 22);
            this.assignYAxisPrintLabelMenuItem.Text = "&Assign Y-Axis Print Label";
            this.assignYAxisPrintLabelMenuItem.Click += new System.EventHandler(this.assignYAxisPrintLabelMenuItem_Click);
            // 
            // propertiesToolStripMenuItem
            // 
            this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
            this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.propertiesToolStripMenuItem.Text = "&Properties";
            this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
            // 
            // dockToolStripMenuItem
            // 
            this.dockToolStripMenuItem.CheckOnClick = true;
            this.dockToolStripMenuItem.Name = "dockToolStripMenuItem";
            this.dockToolStripMenuItem.Size = new System.Drawing.Size(47, 25);
            this.dockToolStripMenuItem.Text = "Dock";
            this.dockToolStripMenuItem.Click += new System.EventHandler(this.dockToolStripMenuItem_Click);
            // 
            // toolStripSeparatorMenus
            // 
            this.toolStripSeparatorMenus.Name = "toolStripSeparatorMenus";
            this.toolStripSeparatorMenus.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorMenus.Visible = false;
            // 
            // toolStripSeparatorRunStop
            // 
            this.toolStripSeparatorRunStop.Name = "toolStripSeparatorRunStop";
            this.toolStripSeparatorRunStop.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparatorAutoRun
            // 
            this.toolStripSeparatorAutoRun.Name = "toolStripSeparatorAutoRun";
            this.toolStripSeparatorAutoRun.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparatorShowAll
            // 
            this.toolStripSeparatorShowAll.Name = "toolStripSeparatorShowAll";
            this.toolStripSeparatorShowAll.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparatorShowEnd
            // 
            this.toolStripSeparatorShowEnd.Name = "toolStripSeparatorShowEnd";
            this.toolStripSeparatorShowEnd.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparatorLinLog
            // 
            this.toolStripSeparatorLinLog.Name = "toolStripSeparatorLinLog";
            this.toolStripSeparatorLinLog.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorLinLog.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataPlotToolStripDropDownButton,
            this.toolStripSeparatorMenus,
            this.toolStripButtonRun,
            this.toolStripButtonStop,
            this.toolStripSeparatorRunStop,
            this.toolStripButtonAutoRun1,
            this.toolStripButtonAutoRun2,
            this.toolStripSeparatorAutoRun,
            this.toolStripButtonAutoShowAll,
            this.toolStripButtonShowAll,
            this.toolStripSeparatorShowAll,
            this.toolStripButtonAutoShowEnd,
            this.toolStripButtonShowEnd,
            this.toolStripSeparatorShowEnd,
            this.toolStripButtonLinear,
            this.toolStripButtonLog,
            this.dockToolStripMenuItem,
            this.toolStripSeparatorLinLog,
            this.toolStripButtonClose});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(652, 25);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // dataPlotToolStripDropDownButton
            // 
            this.dataPlotToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.dataPlotToolStripDropDownButton.Image = ((System.Drawing.Image)(resources.GetObject("dataPlotToolStripDropDownButton.Image")));
            this.dataPlotToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.dataPlotToolStripDropDownButton.Name = "dataPlotToolStripDropDownButton";
            this.dataPlotToolStripDropDownButton.Size = new System.Drawing.Size(72, 22);
            this.dataPlotToolStripDropDownButton.Text = "Data Plot";
            this.dataPlotToolStripDropDownButton.Visible = false;
            // 
            // toolStripButtonRun
            // 
            this.toolStripButtonRun.CheckOnClick = true;
            this.toolStripButtonRun.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonRun.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonRun.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonRun.Image")));
            this.toolStripButtonRun.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonRun.Name = "toolStripButtonRun";
            this.toolStripButtonRun.Size = new System.Drawing.Size(34, 22);
            this.toolStripButtonRun.Text = "Run";
            this.toolStripButtonRun.ToolTipText = "Click to clear graph and start collecting data";
            this.toolStripButtonRun.Click += new System.EventHandler(this.toolStripButtonRun_Click);
            // 
            // toolStripButtonStop
            // 
            this.toolStripButtonStop.Checked = true;
            this.toolStripButtonStop.CheckOnClick = true;
            this.toolStripButtonStop.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonStop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonStop.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonStop.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonStop.Image")));
            this.toolStripButtonStop.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStop.Name = "toolStripButtonStop";
            this.toolStripButtonStop.Size = new System.Drawing.Size(38, 22);
            this.toolStripButtonStop.Text = "Stop";
            this.toolStripButtonStop.ToolTipText = "Click to stop collecting data";
            this.toolStripButtonStop.Click += new System.EventHandler(this.toolStripButtonStop_Click);
            // 
            // toolStripButtonAutoRun1
            // 
            this.toolStripButtonAutoRun1.Checked = true;
            this.toolStripButtonAutoRun1.CheckOnClick = true;
            this.toolStripButtonAutoRun1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonAutoRun1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAutoRun1.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toolStripButtonAutoRun1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoRun1.Image")));
            this.toolStripButtonAutoRun1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoRun1.Name = "toolStripButtonAutoRun1";
            this.toolStripButtonAutoRun1.Size = new System.Drawing.Size(64, 22);
            this.toolStripButtonAutoRun1.Text = "Auto Run";
            this.toolStripButtonAutoRun1.ToolTipText = "Click to automatically run the graph when port or side starts";
            this.toolStripButtonAutoRun1.Click += new System.EventHandler(this.toolStripButtonAutoRun1_Click);
            // 
            // toolStripButtonAutoRun2
            // 
            this.toolStripButtonAutoRun2.Checked = true;
            this.toolStripButtonAutoRun2.CheckOnClick = true;
            this.toolStripButtonAutoRun2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonAutoRun2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAutoRun2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoRun2.Image")));
            this.toolStripButtonAutoRun2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoRun2.Name = "toolStripButtonAutoRun2";
            this.toolStripButtonAutoRun2.Size = new System.Drawing.Size(75, 22);
            this.toolStripButtonAutoRun2.Text = "Auto Run 2";
            this.toolStripButtonAutoRun2.ToolTipText = "Click to automatically run the graph when port or side starts";
            this.toolStripButtonAutoRun2.Click += new System.EventHandler(this.toolStripButtonAutoRun2_Click);
            // 
            // toolStripButtonAutoShowAll
            // 
            this.toolStripButtonAutoShowAll.CheckOnClick = true;
            this.toolStripButtonAutoShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAutoShowAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoShowAll.Image")));
            this.toolStripButtonAutoShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoShowAll.Name = "toolStripButtonAutoShowAll";
            this.toolStripButtonAutoShowAll.Size = new System.Drawing.Size(38, 22);
            this.toolStripButtonAutoShowAll.Text = "Auto";
            this.toolStripButtonAutoShowAll.ToolTipText = "Click to continuously show all data as it is being collected";
            this.toolStripButtonAutoShowAll.Click += new System.EventHandler(this.toolStripButtonAutoShowAll_Click);
            // 
            // toolStripButtonShowAll
            // 
            this.toolStripButtonShowAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShowAll.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowAll.Image")));
            this.toolStripButtonShowAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowAll.Name = "toolStripButtonShowAll";
            this.toolStripButtonShowAll.Size = new System.Drawing.Size(62, 22);
            this.toolStripButtonShowAll.Text = "Show All";
            this.toolStripButtonShowAll.ToolTipText = "Click to show all data";
            this.toolStripButtonShowAll.Click += new System.EventHandler(this.toolStripButtonShowAll_Click);
            // 
            // toolStripButtonAutoShowEnd
            // 
            this.toolStripButtonAutoShowEnd.CheckOnClick = true;
            this.toolStripButtonAutoShowEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonAutoShowEnd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonAutoShowEnd.Image")));
            this.toolStripButtonAutoShowEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonAutoShowEnd.Name = "toolStripButtonAutoShowEnd";
            this.toolStripButtonAutoShowEnd.Size = new System.Drawing.Size(38, 22);
            this.toolStripButtonAutoShowEnd.Text = "Auto";
            this.toolStripButtonAutoShowEnd.ToolTipText = "Click to continuously show most recent data as it is being collected";
            this.toolStripButtonAutoShowEnd.Click += new System.EventHandler(this.toolStripButtonAutoShowEnd_Click);
            // 
            // toolStripButtonShowEnd
            // 
            this.toolStripButtonShowEnd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonShowEnd.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonShowEnd.Image")));
            this.toolStripButtonShowEnd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonShowEnd.Name = "toolStripButtonShowEnd";
            this.toolStripButtonShowEnd.Size = new System.Drawing.Size(69, 22);
            this.toolStripButtonShowEnd.Text = "Show End";
            this.toolStripButtonShowEnd.ToolTipText = "Click to show the most recent data";
            this.toolStripButtonShowEnd.Click += new System.EventHandler(this.toolStripButtonShowEnd_Click);
            // 
            // toolStripButtonLinear
            // 
            this.toolStripButtonLinear.Checked = true;
            this.toolStripButtonLinear.CheckOnClick = true;
            this.toolStripButtonLinear.CheckState = System.Windows.Forms.CheckState.Checked;
            this.toolStripButtonLinear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLinear.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLinear.Image")));
            this.toolStripButtonLinear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLinear.Name = "toolStripButtonLinear";
            this.toolStripButtonLinear.Size = new System.Drawing.Size(47, 22);
            this.toolStripButtonLinear.Text = "Linear";
            this.toolStripButtonLinear.ToolTipText = "Click to display graph in Linear mode";
            this.toolStripButtonLinear.Click += new System.EventHandler(this.toolStripButtonLinear_Click);
            // 
            // toolStripButtonLog
            // 
            this.toolStripButtonLog.CheckOnClick = true;
            this.toolStripButtonLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonLog.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonLog.Image")));
            this.toolStripButtonLog.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonLog.Name = "toolStripButtonLog";
            this.toolStripButtonLog.Size = new System.Drawing.Size(32, 22);
            this.toolStripButtonLog.Text = "Log";
            this.toolStripButtonLog.ToolTipText = "Click to display graph in Log mode";
            this.toolStripButtonLog.Click += new System.EventHandler(this.toolStripButtonLog_Click);
            // 
            // toolStripButtonClose
            // 
            this.toolStripButtonClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButtonClose.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonClose.Image")));
            this.toolStripButtonClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonClose.Name = "toolStripButtonClose";
            this.toolStripButtonClose.Size = new System.Drawing.Size(43, 22);
            this.toolStripButtonClose.Text = "Close";
            this.toolStripButtonClose.ToolTipText = "Click to close the data plot window";
            this.toolStripButtonClose.Visible = false;
            this.toolStripButtonClose.Click += new System.EventHandler(this.toolStripButtonClose_Click);
            // 
            // YMinDn
            // 
            this.YMinDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinDn.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_down2;
            this.YMinDn.Location = new System.Drawing.Point(116, 351);
            this.YMinDn.Name = "YMinDn";
            this.YMinDn.Size = new System.Drawing.Size(13, 13);
            this.YMinDn.TabIndex = 16;
            this.YMinDn.UseVisualStyleBackColor = true;
            this.YMinDn.Click += new System.EventHandler(this.YMinDn_Click);
            // 
            // YMinUp
            // 
            this.YMinUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinUp.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_up2;
            this.YMinUp.Location = new System.Drawing.Point(116, 334);
            this.YMinUp.Name = "YMinUp";
            this.YMinUp.Size = new System.Drawing.Size(13, 13);
            this.YMinUp.TabIndex = 15;
            this.YMinUp.UseVisualStyleBackColor = true;
            this.YMinUp.Click += new System.EventHandler(this.YMinUp_Click);
            // 
            // YMaxDn
            // 
            this.YMaxDn.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_down2;
            this.YMaxDn.Location = new System.Drawing.Point(116, 124);
            this.YMaxDn.Name = "YMaxDn";
            this.YMaxDn.Size = new System.Drawing.Size(13, 13);
            this.YMaxDn.TabIndex = 14;
            this.YMaxDn.UseVisualStyleBackColor = true;
            this.YMaxDn.Click += new System.EventHandler(this.YMaxDn_Click);
            // 
            // YMaxUp
            // 
            this.YMaxUp.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_up2;
            this.YMaxUp.Location = new System.Drawing.Point(116, 107);
            this.YMaxUp.Name = "YMaxUp";
            this.YMaxUp.Size = new System.Drawing.Size(13, 13);
            this.YMaxUp.TabIndex = 13;
            this.YMaxUp.UseVisualStyleBackColor = true;
            this.YMaxUp.Click += new System.EventHandler(this.YMaxUp_Click);
            // 
            // YMaxExpUp
            // 
            this.YMaxExpUp.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_up2;
            this.YMaxExpUp.Location = new System.Drawing.Point(116, 53);
            this.YMaxExpUp.Name = "YMaxExpUp";
            this.YMaxExpUp.Size = new System.Drawing.Size(13, 13);
            this.YMaxExpUp.TabIndex = 17;
            this.YMaxExpUp.UseVisualStyleBackColor = true;
            this.YMaxExpUp.Click += new System.EventHandler(this.YMaxExpUp_Click);
            // 
            // YMinExpUp
            // 
            this.YMinExpUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinExpUp.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_up2;
            this.YMinExpUp.Location = new System.Drawing.Point(116, 281);
            this.YMinExpUp.Name = "YMinExpUp";
            this.YMinExpUp.Size = new System.Drawing.Size(13, 13);
            this.YMinExpUp.TabIndex = 18;
            this.YMinExpUp.UseVisualStyleBackColor = true;
            this.YMinExpUp.Click += new System.EventHandler(this.YMinExpUp_Click);
            // 
            // YMaxExpDn
            // 
            this.YMaxExpDn.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_down2;
            this.YMaxExpDn.Location = new System.Drawing.Point(116, 70);
            this.YMaxExpDn.Name = "YMaxExpDn";
            this.YMaxExpDn.Size = new System.Drawing.Size(13, 13);
            this.YMaxExpDn.TabIndex = 19;
            this.YMaxExpDn.UseVisualStyleBackColor = true;
            this.YMaxExpDn.Click += new System.EventHandler(this.YMaxExpDn_Click);
            // 
            // YMinExpDn
            // 
            this.YMinExpDn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinExpDn.Image = global::VTIWindowsControlLibrary.Properties.Resources.arrow_down2;
            this.YMinExpDn.Location = new System.Drawing.Point(116, 298);
            this.YMinExpDn.Name = "YMinExpDn";
            this.YMinExpDn.Size = new System.Drawing.Size(13, 13);
            this.YMinExpDn.TabIndex = 20;
            this.YMinExpDn.UseVisualStyleBackColor = true;
            this.YMinExpDn.Click += new System.EventHandler(this.YMinExpDn_Click);
            // 
            // graphControl1
            // 
            this.graphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.graphControl1.GraphTypeName = "Graph";
            this.graphControl1.Header = null;
            this.graphControl1.HeaderLeft = null;
            this.graphControl1.HeaderRight = null;
            this.graphControl1.LegendForPrintedPlot = false;
            this.graphControl1.Location = new System.Drawing.Point(0, 25);
            this.graphControl1.MinimumSize = new System.Drawing.Size(300, 200);
            this.graphControl1.Name = "graphControl1";
            this.graphControl1.PlotName = null;
            this.graphControl1.SettingsSection = "VTIWindowsControlLibrary.Components.DataPlotGraphControl.Settings";
            this.graphControl1.Size = new System.Drawing.Size(652, 368);
            this.graphControl1.TabIndex = 11;
            this.graphControl1.XAxisLabel = null;
            // 
            // YMaxUpLbl
            // 
            this.YMaxUpLbl.AutoSize = true;
            this.YMaxUpLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YMaxUpLbl.Location = new System.Drawing.Point(114, 91);
            this.YMaxUpLbl.Name = "YMaxUpLbl";
            this.YMaxUpLbl.Size = new System.Drawing.Size(19, 13);
            this.YMaxUpLbl.TabIndex = 21;
            this.YMaxUpLbl.Text = "±1";
            // 
            // YMinUpLbl
            // 
            this.YMinUpLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinUpLbl.AutoSize = true;
            this.YMinUpLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YMinUpLbl.Location = new System.Drawing.Point(114, 318);
            this.YMinUpLbl.Name = "YMinUpLbl";
            this.YMinUpLbl.Size = new System.Drawing.Size(19, 13);
            this.YMinUpLbl.TabIndex = 22;
            this.YMinUpLbl.Text = "±1";
            // 
            // YMaxExpUpLbl
            // 
            this.YMaxExpUpLbl.AutoSize = true;
            this.YMaxExpUpLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YMaxExpUpLbl.Location = new System.Drawing.Point(114, 37);
            this.YMaxExpUpLbl.Name = "YMaxExpUpLbl";
            this.YMaxExpUpLbl.Size = new System.Drawing.Size(24, 13);
            this.YMaxExpUpLbl.TabIndex = 23;
            this.YMaxExpUpLbl.Text = "x10";
            // 
            // YMinExpUpLbl
            // 
            this.YMinExpUpLbl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.YMinExpUpLbl.AutoSize = true;
            this.YMinExpUpLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YMinExpUpLbl.Location = new System.Drawing.Point(114, 265);
            this.YMinExpUpLbl.Name = "YMinExpUpLbl";
            this.YMinExpUpLbl.Size = new System.Drawing.Size(24, 13);
            this.YMinExpUpLbl.TabIndex = 24;
            this.YMinExpUpLbl.Text = "x10";
            // 
            // DataPlotControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Controls.Add(this.YMinExpUpLbl);
            this.Controls.Add(this.YMaxExpUpLbl);
            this.Controls.Add(this.YMinUpLbl);
            this.Controls.Add(this.YMaxUpLbl);
            this.Controls.Add(this.YMinExpDn);
            this.Controls.Add(this.YMaxExpDn);
            this.Controls.Add(this.YMinExpUp);
            this.Controls.Add(this.YMaxExpUp);
            this.Controls.Add(this.YMinDn);
            this.Controls.Add(this.YMinUp);
            this.Controls.Add(this.YMaxDn);
            this.Controls.Add(this.YMaxUp);
            this.Controls.Add(this.graphControl1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Name = "DataPlotControl";
            this.Size = new System.Drawing.Size(652, 393);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Timer timerCollectData;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCaption;
        private DataPlotGraphControl graphControl1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataPlotToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pageSetupToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem stopToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRunStopMenu;
        private System.Windows.Forms.ToolStripMenuItem autoRun1ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem autoRun2ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAutoRunMenu;
        private System.Windows.Forms.ToolStripMenuItem autoShowAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorShowAllMenu;
        private System.Windows.Forms.ToolStripMenuItem autoShowEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem showEndToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorShowEndMenu;
        private System.Windows.Forms.ToolStripMenuItem yAxisLinearModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem yAxisLogModeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dockToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toggleAutoRunMenuItem;
        //private System.Windows.Forms.ToolStripMenuItem toggleTimeUnitsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem assignYAxisPrintLabelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem overlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fromFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearOverlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addOverlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allTracesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorClose;
        private System.Windows.Forms.ToolStripDropDownButton dataPlotToolStripDropDownButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorMenus;
        private System.Windows.Forms.ToolStripButton toolStripButtonRun;
        private System.Windows.Forms.ToolStripButton toolStripButtonStop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorRunStop;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoRun1;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoRun2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAutoRun;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoShowAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorShowAll;
        private System.Windows.Forms.ToolStripButton toolStripButtonAutoShowEnd;
        private System.Windows.Forms.ToolStripButton toolStripButtonShowEnd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorShowEnd;
        private System.Windows.Forms.ToolStripButton toolStripButtonLinear;
        private System.Windows.Forms.ToolStripButton toolStripButtonLog;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorLinLog;
        private System.Windows.Forms.ToolStripButton toolStripButtonClose;
        private System.Windows.Forms.ToolStrip toolStrip1;

        public System.Windows.Forms.Button YMaxUp;
        public System.Windows.Forms.Button YMaxDn;
        public System.Windows.Forms.Button YMinDn;
        public System.Windows.Forms.Button YMinUp;
        public System.Windows.Forms.Button YMaxExpUp;
        public System.Windows.Forms.Button YMinExpUp;
        public System.Windows.Forms.Button YMaxExpDn;
        public System.Windows.Forms.Button YMinExpDn;
        public System.Windows.Forms.Label YMaxUpLbl;
        public System.Windows.Forms.Label YMinUpLbl;
        public System.Windows.Forms.Label YMaxExpUpLbl;
        public System.Windows.Forms.Label YMinExpUpLbl;
    }
}
