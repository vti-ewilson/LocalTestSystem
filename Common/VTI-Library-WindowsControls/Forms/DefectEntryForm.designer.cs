using System.Windows.Forms;
namespace VTIWindowsControlLibrary.Forms
{
  public partial class DefectEntryForm : Form
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DefectEntryForm));
      this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
      this.panel1 = new System.Windows.Forms.Panel();
      this.buttonSubmitDefect = new System.Windows.Forms.Button();
      this.textBoxCustomDefect = new System.Windows.Forms.TextBox();
      this.buttonNoDefect = new System.Windows.Forms.Button();
      this.buttonRemoveLast = new System.Windows.Forms.Button();
      this.buttonDown = new System.Windows.Forms.Button();
      this.buttonUp = new System.Windows.Forms.Button();
      this.buttonDone = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.dataGridViewDefects = new System.Windows.Forms.DataGridView();
      this.ColumnValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnCategory = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.ColumnDescription = new System.Windows.Forms.DataGridViewTextBoxColumn();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDefects)).BeginInit();
      this.SuspendLayout();
      // 
      // flowLayoutPanel1
      // 
      this.flowLayoutPanel1.AutoSize = true;
      this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.flowLayoutPanel1.MaximumSize = new System.Drawing.Size(455, 10000);
      this.flowLayoutPanel1.MinimumSize = new System.Drawing.Size(455, 330);
      this.flowLayoutPanel1.Name = "flowLayoutPanel1";
      this.flowLayoutPanel1.Size = new System.Drawing.Size(455, 330);
      this.flowLayoutPanel1.TabIndex = 0;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonSubmitDefect);
      this.panel1.Controls.Add(this.textBoxCustomDefect);
      this.panel1.Controls.Add(this.buttonNoDefect);
      this.panel1.Controls.Add(this.buttonRemoveLast);
      this.panel1.Controls.Add(this.buttonDown);
      this.panel1.Controls.Add(this.buttonUp);
      this.panel1.Controls.Add(this.buttonDone);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
      this.panel1.Location = new System.Drawing.Point(455, 0);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(179, 330);
      this.panel1.TabIndex = 1;
      // 
      // buttonSubmitDefect
      // 
      this.buttonSubmitDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonSubmitDefect.Location = new System.Drawing.Point(6, 191);
      this.buttonSubmitDefect.Name = "buttonSubmitDefect";
      this.buttonSubmitDefect.Size = new System.Drawing.Size(161, 45);
      this.buttonSubmitDefect.TabIndex = 8;
      this.buttonSubmitDefect.Text = "&Submit Defect";
      this.buttonSubmitDefect.UseVisualStyleBackColor = true;
      this.buttonSubmitDefect.Click += new System.EventHandler(this.buttonSubmitDefect_Click);
      // 
      // textBoxCustomDefect
      // 
      this.textBoxCustomDefect.Location = new System.Drawing.Point(6, 165);
      this.textBoxCustomDefect.Name = "textBoxCustomDefect";
      this.textBoxCustomDefect.Size = new System.Drawing.Size(160, 20);
      this.textBoxCustomDefect.TabIndex = 7;
      this.textBoxCustomDefect.Text = "Enter Custom Defect";
      // 
      // buttonNoDefect
      // 
      this.buttonNoDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonNoDefect.Location = new System.Drawing.Point(6, 114);
      this.buttonNoDefect.Name = "buttonNoDefect";
      this.buttonNoDefect.Size = new System.Drawing.Size(161, 45);
      this.buttonNoDefect.TabIndex = 6;
      this.buttonNoDefect.Text = "&No Defect Found";
      this.buttonNoDefect.UseVisualStyleBackColor = true;
      this.buttonNoDefect.Click += new System.EventHandler(this.buttonNoDefect_Click);
      // 
      // buttonRemoveLast
      // 
      this.buttonRemoveLast.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonRemoveLast.Location = new System.Drawing.Point(6, 63);
      this.buttonRemoveLast.Name = "buttonRemoveLast";
      this.buttonRemoveLast.Size = new System.Drawing.Size(161, 45);
      this.buttonRemoveLast.TabIndex = 5;
      this.buttonRemoveLast.Text = "&Remove Last";
      this.buttonRemoveLast.UseVisualStyleBackColor = true;
      this.buttonRemoveLast.Click += new System.EventHandler(this.buttonRemoveLast_Click);
      // 
      // buttonDown
      // 
      this.buttonDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonDown.Image = global::VTIWindowsControlLibrary.Properties.Resources._60px_Go_down;
      this.buttonDown.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
      this.buttonDown.Location = new System.Drawing.Point(92, 249);
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
      this.buttonUp.Location = new System.Drawing.Point(6, 249);
      this.buttonUp.Name = "buttonUp";
      this.buttonUp.Size = new System.Drawing.Size(75, 75);
      this.buttonUp.TabIndex = 3;
      this.buttonUp.Text = "UP";
      this.buttonUp.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
      this.buttonUp.UseVisualStyleBackColor = true;
      this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
      // 
      // buttonDone
      // 
      this.buttonDone.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonDone.Location = new System.Drawing.Point(6, 12);
      this.buttonDone.Name = "buttonDone";
      this.buttonDone.Size = new System.Drawing.Size(161, 45);
      this.buttonDone.TabIndex = 2;
      this.buttonDone.Text = "&Done";
      this.buttonDone.UseVisualStyleBackColor = true;
      this.buttonDone.Click += new System.EventHandler(this.buttonDone_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.dataGridViewDefects);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.groupBox1.Location = new System.Drawing.Point(0, 330);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(634, 138);
      this.groupBox1.TabIndex = 2;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Defect Codes";
      // 
      // dataGridViewDefects
      // 
      this.dataGridViewDefects.AllowUserToAddRows = false;
      this.dataGridViewDefects.AllowUserToResizeColumns = false;
      this.dataGridViewDefects.AllowUserToResizeRows = false;
      this.dataGridViewDefects.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
      this.dataGridViewDefects.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnValue,
            this.ColumnCategory,
            this.ColumnDescription});
      this.dataGridViewDefects.Dock = System.Windows.Forms.DockStyle.Fill;
      this.dataGridViewDefects.Location = new System.Drawing.Point(3, 16);
      this.dataGridViewDefects.MultiSelect = false;
      this.dataGridViewDefects.Name = "dataGridViewDefects";
      this.dataGridViewDefects.ReadOnly = true;
      this.dataGridViewDefects.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dataGridViewDefects.Size = new System.Drawing.Size(628, 119);
      this.dataGridViewDefects.TabIndex = 3;
      // 
      // ColumnValue
      // 
      this.ColumnValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
      this.ColumnValue.HeaderText = "Defect Code";
      this.ColumnValue.MinimumWidth = 100;
      this.ColumnValue.Name = "ColumnValue";
      this.ColumnValue.ReadOnly = true;
      // 
      // ColumnCategory
      // 
      this.ColumnCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
      this.ColumnCategory.HeaderText = "Category";
      this.ColumnCategory.MinimumWidth = 100;
      this.ColumnCategory.Name = "ColumnCategory";
      this.ColumnCategory.ReadOnly = true;
      // 
      // ColumnDescription
      // 
      this.ColumnDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
      this.ColumnDescription.HeaderText = "Description";
      this.ColumnDescription.Name = "ColumnDescription";
      this.ColumnDescription.ReadOnly = true;
      // 
      // DefectEntryForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(634, 468);
      this.ControlBox = false;
      this.Controls.Add(this.panel1);
      this.Controls.Add(this.flowLayoutPanel1);
      this.Controls.Add(this.groupBox1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(174, 158);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "DefectEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
      this.Text = "Enter Defect Codes";
      this.TopMost = true;
      this.VisibleChanged += new System.EventHandler(this.DefectEntryForm_VisibleChanged);
      this.panel1.ResumeLayout(false);
      this.panel1.PerformLayout();
      this.groupBox1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.dataGridViewDefects)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button buttonDone;
    private System.Windows.Forms.Button buttonUp;
    private System.Windows.Forms.Button buttonDown;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.DataGridView dataGridViewDefects;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnValue;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnCategory;
    private System.Windows.Forms.DataGridViewTextBoxColumn ColumnDescription;
    private System.Windows.Forms.Button buttonSubmitDefect;
    private System.Windows.Forms.TextBox textBoxCustomDefect;
    /// <summary>
    /// Public so can Enable/Disable as needed
    /// </summary>
    public System.Windows.Forms.Button buttonNoDefect;
    private System.Windows.Forms.Button buttonRemoveLast;
  }
}