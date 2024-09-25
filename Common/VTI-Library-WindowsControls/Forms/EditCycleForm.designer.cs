using VTIWindowsControlLibrary.Components;
namespace VTIWindowsControlLibrary.Forms
{
  partial class EditCycleForm//<ConfigClass>//, ModelSettings>//, ControlSettings, FlowSettings, ModeSettings, PressureSettings, TimeSettings, ModelSettings>
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EditCycleForm));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.panel1 = new System.Windows.Forms.Panel();
			this.buttonImport = new System.Windows.Forms.Button();
			this.buttonExport = new System.Windows.Forms.Button();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.numberPadControl = new VTIWindowsControlLibrary.Components.NumberPadControl();
			this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.contextMenuStripModel = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.newModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.deleteModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolTipExport = new System.Windows.Forms.ToolTip(this.components);
			this.tableLayoutPanel1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.tableLayoutPanel2.SuspendLayout();
			this.contextMenuStripModel.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			this.tableLayoutPanel1.ColumnCount = 1;
			this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.numberPadControl, 0, 1);
			this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Right;
			this.tableLayoutPanel1.Location = new System.Drawing.Point(524, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.tableLayoutPanel1.RowCount = 2;
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.38028F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 86.61972F));
			this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel1.Size = new System.Drawing.Size(270, 568);
			this.tableLayoutPanel1.TabIndex = 0;
			// 
			// panel1
			// 
			this.panel1.AutoSize = true;
			this.panel1.Controls.Add(this.buttonImport);
			this.panel1.Controls.Add(this.buttonExport);
			this.panel1.Controls.Add(this.buttonOK);
			this.panel1.Controls.Add(this.buttonCancel);
			this.panel1.Location = new System.Drawing.Point(3, 3);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(261, 69);
			this.panel1.TabIndex = 1;
			// 
			// buttonImport
			// 
			this.buttonImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonImport.Location = new System.Drawing.Point(6, 34);
			this.buttonImport.Name = "buttonImport";
			this.buttonImport.Size = new System.Drawing.Size(120, 33);
			this.buttonImport.TabIndex = 8;
			this.buttonImport.Text = "Import";
			this.toolTipExport.SetToolTip(this.buttonImport, "Export to folder");
			this.buttonImport.UseVisualStyleBackColor = true;
			this.buttonImport.Click += new System.EventHandler(this.buttonImport_Click);
			// 
			// buttonExport
			// 
			this.buttonExport.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonExport.Location = new System.Drawing.Point(138, 34);
			this.buttonExport.Name = "buttonExport";
			this.buttonExport.Size = new System.Drawing.Size(120, 33);
			this.buttonExport.TabIndex = 7;
			this.buttonExport.Text = "Export";
			this.toolTipExport.SetToolTip(this.buttonExport, "Export to folder");
			this.buttonExport.UseVisualStyleBackColor = true;
			this.buttonExport.Click += new System.EventHandler(this.buttonExport_Click);
			// 
			// buttonOK
			// 
			this.buttonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonOK.Location = new System.Drawing.Point(6, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(120, 30);
			this.buttonOK.TabIndex = 6;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonCancel.Location = new System.Drawing.Point(138, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(120, 30);
			this.buttonCancel.TabIndex = 5;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// numberPadControl
			// 
			this.numberPadControl.AutoSize = true;
			this.numberPadControl.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.numberPadControl.CurrentSetting = 0D;
			this.numberPadControl.Location = new System.Drawing.Point(0, 75);
			this.numberPadControl.Margin = new System.Windows.Forms.Padding(0);
			this.numberPadControl.MaximumSize = new System.Drawing.Size(267, 496);
			this.numberPadControl.MinimumSize = new System.Drawing.Size(267, 496);
			this.numberPadControl.Name = "numberPadControl";
			this.numberPadControl.Size = new System.Drawing.Size(267, 496);
			this.numberPadControl.TabIndex = 2;
			this.numberPadControl.CurrentSettingChanged += new VTIWindowsControlLibrary.Components.CurrentSettingChangedEventHandler(this.numberPadControl_CurrentSettingChanged);
			// 
			// tableLayoutPanel2
			// 
			this.tableLayoutPanel2.ColumnCount = 1;
			this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
			this.tableLayoutPanel2.Controls.Add(this.treeView1, 0, 0);
			this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
			this.tableLayoutPanel2.Name = "tableLayoutPanel2";
			this.tableLayoutPanel2.RowCount = 2;
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 74.82394F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.17606F));
			this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
			this.tableLayoutPanel2.Size = new System.Drawing.Size(524, 568);
			this.tableLayoutPanel2.TabIndex = 1;
			// 
			// treeView1
			// 
			this.treeView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.treeView1.HideSelection = false;
			this.treeView1.Location = new System.Drawing.Point(3, 3);
			this.treeView1.Name = "treeView1";
			this.treeView1.Size = new System.Drawing.Size(518, 418);
			this.treeView1.TabIndex = 0;
			this.treeView1.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeExpand);
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			this.treeView1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.treeView1_MouseClick);
			// 
			// contextMenuStripModel
			// 
			this.contextMenuStripModel.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newModelToolStripMenuItem,
            this.copyModelToolStripMenuItem,
            this.deleteModelToolStripMenuItem});
			this.contextMenuStripModel.Name = "contextMenuStripModel";
			this.contextMenuStripModel.Size = new System.Drawing.Size(145, 70);
			// 
			// newModelToolStripMenuItem
			// 
			this.newModelToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.page;
			this.newModelToolStripMenuItem.Name = "newModelToolStripMenuItem";
			this.newModelToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.newModelToolStripMenuItem.Text = "&New Model";
			this.newModelToolStripMenuItem.Click += new System.EventHandler(this.newModelToolStripMenuItem_Click);
			// 
			// copyModelToolStripMenuItem
			// 
			this.copyModelToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.page_copy;
			this.copyModelToolStripMenuItem.Name = "copyModelToolStripMenuItem";
			this.copyModelToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.copyModelToolStripMenuItem.Text = "&Copy Model";
			this.copyModelToolStripMenuItem.Click += new System.EventHandler(this.copyModelToolStripMenuItem_Click);
			// 
			// deleteModelToolStripMenuItem
			// 
			this.deleteModelToolStripMenuItem.Image = global::VTIWindowsControlLibrary.Properties.Resources.cut;
			this.deleteModelToolStripMenuItem.Name = "deleteModelToolStripMenuItem";
			this.deleteModelToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
			this.deleteModelToolStripMenuItem.Text = "&Delete Model";
			this.deleteModelToolStripMenuItem.Click += new System.EventHandler(this.deleteModelToolStripMenuItem_Click);
			// 
			// EditCycleForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(794, 568);
			this.Controls.Add(this.tableLayoutPanel2);
			this.Controls.Add(this.tableLayoutPanel1);
			this.DoubleBuffered = true;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "EditCycleForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Edit Cycle";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.EditCycleForm_FormClosing);
			this.VisibleChanged += new System.EventHandler(this.EditCycleForm_VisibleChanged);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.EditCycleForm_KeyDown);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			this.panel1.ResumeLayout(false);
			this.tableLayoutPanel2.ResumeLayout(false);
			this.contextMenuStripModel.ResumeLayout(false);
			this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    public System.Windows.Forms.TreeView treeView1;
    private System.Windows.Forms.ContextMenuStrip contextMenuStripModel;
    private System.Windows.Forms.ToolStripMenuItem newModelToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem copyModelToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteModelToolStripMenuItem;
    private NumberPadControl numberPadControl;
    private System.Windows.Forms.Button buttonExport;
    private System.Windows.Forms.ToolTip toolTipExport;
		private System.Windows.Forms.Button buttonImport;
	}
}
