using VTIWindowsControlLibrary.Components;
using System.Drawing;
namespace VTIWindowsControlLibrary.Forms
{
  partial class PlotPropForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PlotPropForm));
      this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
      this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
      this.tabControlSettings = new System.Windows.Forms.TabControl();
      this.tabPageAxisSettings = new System.Windows.Forms.TabPage();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.radioButtonMinutes = new System.Windows.Forms.RadioButton();
      this.radioButtonSeconds = new System.Windows.Forms.RadioButton();
      this.textBoxXWindow = new System.Windows.Forms.TextBox();
      this.label7 = new System.Windows.Forms.Label();
      this.textBoxXMax = new System.Windows.Forms.TextBox();
      this.label6 = new System.Windows.Forms.Label();
      this.textBoxXMin = new System.Windows.Forms.TextBox();
      this.label5 = new System.Windows.Forms.Label();
      this.groupBox2 = new System.Windows.Forms.GroupBox();
      this.radioButtonLog = new System.Windows.Forms.RadioButton();
      this.radioButtonLinear = new System.Windows.Forms.RadioButton();
      this.checkBoxAutoScaleMin = new System.Windows.Forms.CheckBox();
      this.checkBoxAutoScaleMax = new System.Windows.Forms.CheckBox();
      this.numericUpDownYMinExp = new System.Windows.Forms.NumericUpDown();
      this.labelx10min = new System.Windows.Forms.Label();
      this.textBoxYMin = new System.Windows.Forms.TextBox();
      this.label4 = new System.Windows.Forms.Label();
      this.numericUpDownYMaxExp = new System.Windows.Forms.NumericUpDown();
      this.labelx10max = new System.Windows.Forms.Label();
      this.textBoxYMax = new System.Windows.Forms.TextBox();
      this.label1 = new System.Windows.Forms.Label();
      this.tabPageOptions = new System.Windows.Forms.TabPage();
      this.groupBox4 = new System.Windows.Forms.GroupBox();
      this.checkBoxAutoSave = new System.Windows.Forms.CheckBox();
      this.numericSliderDecimationThreshold = new VTIWindowsControlLibrary.Components.NumericSlider();
      this.labelDecimationThreshold = new System.Windows.Forms.Label();
      this.label11 = new System.Windows.Forms.Label();
      this.checkBoxShowComments = new System.Windows.Forms.CheckBox();
      this.label8 = new System.Windows.Forms.Label();
      this.label3 = new System.Windows.Forms.Label();
      this.comboBoxDataCollectionInterval = new System.Windows.Forms.ComboBox();
      this.label2 = new System.Windows.Forms.Label();
      this.comboBoxXWindowShiftPercent = new System.Windows.Forms.ComboBox();
      this.groupBox5 = new System.Windows.Forms.GroupBox();
      this.radioButtonDate = new System.Windows.Forms.RadioButton();
      this.radioButtonDateTime = new System.Windows.Forms.RadioButton();
      this.radioButtonTime = new System.Windows.Forms.RadioButton();
      this.radioButtonSecondsMinutes = new System.Windows.Forms.RadioButton();
      this.panel1 = new System.Windows.Forms.Panel();
      this.buttonOK = new System.Windows.Forms.Button();
      this.buttonCancel = new System.Windows.Forms.Button();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.tableLayoutPanelTraceSettings = new System.Windows.Forms.TableLayoutPanel();
      this.ucNumPad1 = new VTIWindowsControlLibrary.Components.NumberPadControl();
      this.colorDialog1 = new System.Windows.Forms.ColorDialog();
      this.tableLayoutPanel1.SuspendLayout();
      this.tableLayoutPanel2.SuspendLayout();
      this.tabControlSettings.SuspendLayout();
      this.tabPageAxisSettings.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.groupBox2.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYMinExp)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYMaxExp)).BeginInit();
      this.tabPageOptions.SuspendLayout();
      this.groupBox4.SuspendLayout();
      this.groupBox5.SuspendLayout();
      this.panel1.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // tableLayoutPanel1
      // 
      this.tableLayoutPanel1.ColumnCount = 2;
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
      this.tableLayoutPanel1.Controls.Add(this.tableLayoutPanel2, 0, 0);
      this.tableLayoutPanel1.Controls.Add(this.ucNumPad1, 1, 0);
      this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel1.Name = "tableLayoutPanel1";
      this.tableLayoutPanel1.RowCount = 1;
      this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
      this.tableLayoutPanel1.Size = new System.Drawing.Size(664, 496);
      this.tableLayoutPanel1.TabIndex = 0;
      // 
      // tableLayoutPanel2
      // 
      this.tableLayoutPanel2.ColumnCount = 1;
      this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
      this.tableLayoutPanel2.Controls.Add(this.tabControlSettings, 0, 0);
      this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 2);
      this.tableLayoutPanel2.Controls.Add(this.groupBox1, 0, 1);
      this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 3);
      this.tableLayoutPanel2.Name = "tableLayoutPanel2";
      this.tableLayoutPanel2.RowCount = 3;
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
      this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 72F));
      this.tableLayoutPanel2.Size = new System.Drawing.Size(385, 496);
      this.tableLayoutPanel2.TabIndex = 2;
      // 
      // tabControlSettings
      // 
      this.tabControlSettings.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
      this.tabControlSettings.Controls.Add(this.tabPageAxisSettings);
      this.tabControlSettings.Controls.Add(this.tabPageOptions);
      this.tabControlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tabControlSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.tabControlSettings.Location = new System.Drawing.Point(3, 3);
      this.tabControlSettings.Name = "tabControlSettings";
      this.tabControlSettings.SelectedIndex = 0;
      this.tabControlSettings.Size = new System.Drawing.Size(379, 206);
      this.tabControlSettings.TabIndex = 0;
      // 
      // tabPageAxisSettings
      // 
      this.tabPageAxisSettings.Controls.Add(this.groupBox3);
      this.tabPageAxisSettings.Controls.Add(this.groupBox2);
      this.tabPageAxisSettings.Location = new System.Drawing.Point(4, 28);
      this.tabPageAxisSettings.Name = "tabPageAxisSettings";
      this.tabPageAxisSettings.Size = new System.Drawing.Size(371, 174);
      this.tabPageAxisSettings.TabIndex = 0;
      this.tabPageAxisSettings.Text = "Axis Settings";
      this.tabPageAxisSettings.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.radioButtonMinutes);
      this.groupBox3.Controls.Add(this.radioButtonSeconds);
      this.groupBox3.Controls.Add(this.textBoxXWindow);
      this.groupBox3.Controls.Add(this.label7);
      this.groupBox3.Controls.Add(this.textBoxXMax);
      this.groupBox3.Controls.Add(this.label6);
      this.groupBox3.Controls.Add(this.textBoxXMin);
      this.groupBox3.Controls.Add(this.label5);
      this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox3.Location = new System.Drawing.Point(0, 106);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(371, 68);
      this.groupBox3.TabIndex = 1;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "X-Axis Settings";
      // 
      // radioButtonMinutes
      // 
      this.radioButtonMinutes.AutoSize = true;
      this.radioButtonMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonMinutes.Location = new System.Drawing.Point(285, 42);
      this.radioButtonMinutes.Name = "radioButtonMinutes";
      this.radioButtonMinutes.Size = new System.Drawing.Size(62, 17);
      this.radioButtonMinutes.TabIndex = 14;
      this.radioButtonMinutes.Text = "Minutes";
      this.radioButtonMinutes.UseVisualStyleBackColor = true;
      // 
      // radioButtonSeconds
      // 
      this.radioButtonSeconds.AutoSize = true;
      this.radioButtonSeconds.Checked = true;
      this.radioButtonSeconds.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonSeconds.Location = new System.Drawing.Point(285, 19);
      this.radioButtonSeconds.Name = "radioButtonSeconds";
      this.radioButtonSeconds.Size = new System.Drawing.Size(67, 17);
      this.radioButtonSeconds.TabIndex = 13;
      this.radioButtonSeconds.TabStop = true;
      this.radioButtonSeconds.Text = "Seconds";
      this.radioButtonSeconds.UseVisualStyleBackColor = true;
      // 
      // textBoxXWindow
      // 
      this.textBoxXWindow.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxXWindow.Location = new System.Drawing.Point(223, 21);
      this.textBoxXWindow.Name = "textBoxXWindow";
      this.textBoxXWindow.Size = new System.Drawing.Size(45, 26);
      this.textBoxXWindow.TabIndex = 12;
      this.textBoxXWindow.Text = "120";
      this.textBoxXWindow.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxXWindow.Enter += new System.EventHandler(this.textBoxXWindow_Enter);
      // 
      // label7
      // 
      this.label7.AutoSize = true;
      this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label7.Location = new System.Drawing.Point(171, 21);
      this.label7.Name = "label7";
      this.label7.Size = new System.Drawing.Size(46, 13);
      this.label7.TabIndex = 11;
      this.label7.Text = "Window";
      // 
      // textBoxXMax
      // 
      this.textBoxXMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxXMax.Location = new System.Drawing.Point(120, 21);
      this.textBoxXMax.Name = "textBoxXMax";
      this.textBoxXMax.Size = new System.Drawing.Size(45, 26);
      this.textBoxXMax.TabIndex = 10;
      this.textBoxXMax.Text = "120";
      this.textBoxXMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxXMax.Enter += new System.EventHandler(this.textBoxXMax_Enter);
      // 
      // label6
      // 
      this.label6.AutoSize = true;
      this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label6.Location = new System.Drawing.Point(87, 21);
      this.label6.Name = "label6";
      this.label6.Size = new System.Drawing.Size(27, 13);
      this.label6.TabIndex = 9;
      this.label6.Text = "Max";
      // 
      // textBoxXMin
      // 
      this.textBoxXMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxXMin.Location = new System.Drawing.Point(36, 21);
      this.textBoxXMin.Name = "textBoxXMin";
      this.textBoxXMin.Size = new System.Drawing.Size(45, 26);
      this.textBoxXMin.TabIndex = 8;
      this.textBoxXMin.Text = "0";
      this.textBoxXMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxXMin.Enter += new System.EventHandler(this.textBoxXMin_Enter);
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label5.Location = new System.Drawing.Point(6, 21);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(24, 13);
      this.label5.TabIndex = 7;
      this.label5.Text = "Min";
      // 
      // groupBox2
      // 
      this.groupBox2.Controls.Add(this.radioButtonLog);
      this.groupBox2.Controls.Add(this.radioButtonLinear);
      this.groupBox2.Controls.Add(this.checkBoxAutoScaleMin);
      this.groupBox2.Controls.Add(this.checkBoxAutoScaleMax);
      this.groupBox2.Controls.Add(this.numericUpDownYMinExp);
      this.groupBox2.Controls.Add(this.labelx10min);
      this.groupBox2.Controls.Add(this.textBoxYMin);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.numericUpDownYMaxExp);
      this.groupBox2.Controls.Add(this.labelx10max);
      this.groupBox2.Controls.Add(this.textBoxYMax);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(371, 106);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Y-Axis Settings";
      // 
      // radioButtonLog
      // 
      this.radioButtonLog.AutoSize = true;
      this.radioButtonLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonLog.Location = new System.Drawing.Point(251, 53);
      this.radioButtonLog.Name = "radioButtonLog";
      this.radioButtonLog.Size = new System.Drawing.Size(109, 17);
      this.radioButtonLog.TabIndex = 12;
      this.radioButtonLog.Text = "Logarithmic Scale";
      this.radioButtonLog.UseVisualStyleBackColor = true;
      this.radioButtonLog.CheckedChanged += new System.EventHandler(this.radioButtonLog_CheckedChanged);
      // 
      // radioButtonLinear
      // 
      this.radioButtonLinear.AutoSize = true;
      this.radioButtonLinear.Checked = true;
      this.radioButtonLinear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonLinear.Location = new System.Drawing.Point(251, 29);
      this.radioButtonLinear.Name = "radioButtonLinear";
      this.radioButtonLinear.Size = new System.Drawing.Size(84, 17);
      this.radioButtonLinear.TabIndex = 11;
      this.radioButtonLinear.TabStop = true;
      this.radioButtonLinear.Text = "Linear Scale";
      this.radioButtonLinear.UseVisualStyleBackColor = true;
      // 
      // checkBoxAutoScaleMin
      // 
      this.checkBoxAutoScaleMin.AutoSize = true;
      this.checkBoxAutoScaleMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.checkBoxAutoScaleMin.Location = new System.Drawing.Point(162, 65);
      this.checkBoxAutoScaleMin.Name = "checkBoxAutoScaleMin";
      this.checkBoxAutoScaleMin.Size = new System.Drawing.Size(78, 17);
      this.checkBoxAutoScaleMin.TabIndex = 10;
      this.checkBoxAutoScaleMin.Text = "Auto-Scale";
      this.checkBoxAutoScaleMin.UseVisualStyleBackColor = true;
      // 
      // checkBoxAutoScaleMax
      // 
      this.checkBoxAutoScaleMax.AutoSize = true;
      this.checkBoxAutoScaleMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.checkBoxAutoScaleMax.Location = new System.Drawing.Point(162, 25);
      this.checkBoxAutoScaleMax.Name = "checkBoxAutoScaleMax";
      this.checkBoxAutoScaleMax.Size = new System.Drawing.Size(78, 17);
      this.checkBoxAutoScaleMax.TabIndex = 9;
      this.checkBoxAutoScaleMax.Text = "Auto-Scale";
      this.checkBoxAutoScaleMax.UseVisualStyleBackColor = true;
      // 
      // numericUpDownYMinExp
      // 
      this.numericUpDownYMinExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.numericUpDownYMinExp.Location = new System.Drawing.Point(111, 54);
      this.numericUpDownYMinExp.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
      this.numericUpDownYMinExp.Minimum = new decimal(new int[] {
            14,
            0,
            0,
            -2147483648});
      this.numericUpDownYMinExp.Name = "numericUpDownYMinExp";
      this.numericUpDownYMinExp.Size = new System.Drawing.Size(48, 26);
      this.numericUpDownYMinExp.TabIndex = 8;
      this.numericUpDownYMinExp.Value = new decimal(new int[] {
            14,
            0,
            0,
            -2147483648});
      this.numericUpDownYMinExp.Enter += new System.EventHandler(this.numericUpDownYMinExp_Enter);
      // 
      // labelx10min
      // 
      this.labelx10min.AutoSize = true;
      this.labelx10min.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelx10min.Location = new System.Drawing.Point(84, 69);
      this.labelx10min.Margin = new System.Windows.Forms.Padding(0);
      this.labelx10min.Name = "labelx10min";
      this.labelx10min.Size = new System.Drawing.Size(24, 13);
      this.labelx10min.TabIndex = 7;
      this.labelx10min.Text = "x10";
      // 
      // textBoxYMin
      // 
      this.textBoxYMin.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxYMin.Location = new System.Drawing.Point(36, 61);
      this.textBoxYMin.Name = "textBoxYMin";
      this.textBoxYMin.Size = new System.Drawing.Size(45, 26);
      this.textBoxYMin.TabIndex = 6;
      this.textBoxYMin.Text = "9999";
      this.textBoxYMin.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxYMin.Enter += new System.EventHandler(this.textBoxYMin_Enter);
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label4.Location = new System.Drawing.Point(6, 61);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(24, 13);
      this.label4.TabIndex = 5;
      this.label4.Text = "Min";
      // 
      // numericUpDownYMaxExp
      // 
      this.numericUpDownYMaxExp.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.numericUpDownYMaxExp.Location = new System.Drawing.Point(111, 16);
      this.numericUpDownYMaxExp.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
      this.numericUpDownYMaxExp.Minimum = new decimal(new int[] {
            14,
            0,
            0,
            -2147483648});
      this.numericUpDownYMaxExp.Name = "numericUpDownYMaxExp";
      this.numericUpDownYMaxExp.Size = new System.Drawing.Size(48, 26);
      this.numericUpDownYMaxExp.TabIndex = 4;
      this.numericUpDownYMaxExp.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
      this.numericUpDownYMaxExp.Enter += new System.EventHandler(this.numericUpDownYMaxExp_Enter);
      // 
      // labelx10max
      // 
      this.labelx10max.AutoSize = true;
      this.labelx10max.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelx10max.Location = new System.Drawing.Point(84, 29);
      this.labelx10max.Margin = new System.Windows.Forms.Padding(0);
      this.labelx10max.Name = "labelx10max";
      this.labelx10max.Size = new System.Drawing.Size(24, 13);
      this.labelx10max.TabIndex = 2;
      this.labelx10max.Text = "x10";
      // 
      // textBoxYMax
      // 
      this.textBoxYMax.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.textBoxYMax.Location = new System.Drawing.Point(36, 21);
      this.textBoxYMax.Name = "textBoxYMax";
      this.textBoxYMax.Size = new System.Drawing.Size(45, 26);
      this.textBoxYMax.TabIndex = 1;
      this.textBoxYMax.Text = "9999";
      this.textBoxYMax.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      this.textBoxYMax.Enter += new System.EventHandler(this.textBoxYMax_Enter);
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label1.Location = new System.Drawing.Point(6, 21);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(27, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Max";
      // 
      // tabPageOptions
      // 
      this.tabPageOptions.Controls.Add(this.groupBox4);
      this.tabPageOptions.Controls.Add(this.groupBox5);
      this.tabPageOptions.Location = new System.Drawing.Point(4, 28);
      this.tabPageOptions.Name = "tabPageOptions";
      this.tabPageOptions.Size = new System.Drawing.Size(371, 174);
      this.tabPageOptions.TabIndex = 1;
      this.tabPageOptions.Text = "Options";
      this.tabPageOptions.UseVisualStyleBackColor = true;
      // 
      // groupBox4
      // 
      this.groupBox4.Controls.Add(this.checkBoxAutoSave);
      this.groupBox4.Controls.Add(this.numericSliderDecimationThreshold);
      this.groupBox4.Controls.Add(this.labelDecimationThreshold);
      this.groupBox4.Controls.Add(this.label11);
      this.groupBox4.Controls.Add(this.checkBoxShowComments);
      this.groupBox4.Controls.Add(this.label8);
      this.groupBox4.Controls.Add(this.label3);
      this.groupBox4.Controls.Add(this.comboBoxDataCollectionInterval);
      this.groupBox4.Controls.Add(this.label2);
      this.groupBox4.Controls.Add(this.comboBoxXWindowShiftPercent);
      this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox4.Location = new System.Drawing.Point(0, 70);
      this.groupBox4.Name = "groupBox4";
      this.groupBox4.Size = new System.Drawing.Size(371, 104);
      this.groupBox4.TabIndex = 11;
      this.groupBox4.TabStop = false;
      this.groupBox4.Text = "Other Options";
      // 
      // checkBoxAutoSave
      // 
      this.checkBoxAutoSave.AutoSize = true;
      this.checkBoxAutoSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.checkBoxAutoSave.Location = new System.Drawing.Point(266, 51);
      this.checkBoxAutoSave.Name = "checkBoxAutoSave";
      this.checkBoxAutoSave.Size = new System.Drawing.Size(76, 17);
      this.checkBoxAutoSave.TabIndex = 20;
      this.checkBoxAutoSave.Text = "Auto Save";
      this.checkBoxAutoSave.UseVisualStyleBackColor = true;
      this.checkBoxAutoSave.CheckedChanged += new System.EventHandler(this.checkBoxAutoSave_CheckedChanged);
      // 
      // numericSliderDecimationThreshold
      // 
      this.numericSliderDecimationThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.numericSliderDecimationThreshold.LargeChange = 10000D;
      this.numericSliderDecimationThreshold.Location = new System.Drawing.Point(132, 79);
      this.numericSliderDecimationThreshold.Maximum = 209000D;
      this.numericSliderDecimationThreshold.Minimum = 0D;
      this.numericSliderDecimationThreshold.Name = "numericSliderDecimationThreshold";
      this.numericSliderDecimationThreshold.Size = new System.Drawing.Size(180, 21);
      this.numericSliderDecimationThreshold.SliderOnLeft = true;
      this.numericSliderDecimationThreshold.SmallChange = 1000D;
      this.numericSliderDecimationThreshold.SplitterDistance = 110;
      this.numericSliderDecimationThreshold.TabIndex = 19;
      this.numericSliderDecimationThreshold.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
      this.numericSliderDecimationThreshold.TextBoxColor = System.Drawing.SystemColors.WindowText;
      this.numericSliderDecimationThreshold.Value = 0D;
      // 
      // labelDecimationThreshold
      // 
      this.labelDecimationThreshold.AutoSize = true;
      this.labelDecimationThreshold.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.labelDecimationThreshold.Location = new System.Drawing.Point(6, 81);
      this.labelDecimationThreshold.Name = "labelDecimationThreshold";
      this.labelDecimationThreshold.Size = new System.Drawing.Size(113, 13);
      this.labelDecimationThreshold.TabIndex = 17;
      this.labelDecimationThreshold.Text = "Decimation Threshold:";
      // 
      // label11
      // 
      this.label11.AutoSize = true;
      this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label11.Location = new System.Drawing.Point(316, 81);
      this.label11.Name = "label11";
      this.label11.Size = new System.Drawing.Size(35, 13);
      this.label11.TabIndex = 12;
      this.label11.Text = "points";
      // 
      // checkBoxShowComments
      // 
      this.checkBoxShowComments.AutoSize = true;
      this.checkBoxShowComments.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
      this.checkBoxShowComments.Location = new System.Drawing.Point(266, 24);
      this.checkBoxShowComments.Name = "checkBoxShowComments";
      this.checkBoxShowComments.Size = new System.Drawing.Size(105, 17);
      this.checkBoxShowComments.TabIndex = 16;
      this.checkBoxShowComments.Text = "Show Comments";
      this.checkBoxShowComments.UseVisualStyleBackColor = true;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label8.Location = new System.Drawing.Point(203, 24);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(47, 13);
      this.label8.TabIndex = 15;
      this.label8.Text = "seconds";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label3.Location = new System.Drawing.Point(6, 24);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(120, 13);
      this.label3.TabIndex = 14;
      this.label3.Text = "Data Collection Interval:";
      // 
      // comboBoxDataCollectionInterval
      // 
      this.comboBoxDataCollectionInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxDataCollectionInterval.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.comboBoxDataCollectionInterval.FormattingEnabled = true;
      this.comboBoxDataCollectionInterval.Items.AddRange(new object[] {
            "0.1",
            "0.25",
            "0.5",
            "1",
            "2",
            "5",
            "10",
            "15",
            "30",
            "60"});
      this.comboBoxDataCollectionInterval.Location = new System.Drawing.Point(132, 21);
      this.comboBoxDataCollectionInterval.Name = "comboBoxDataCollectionInterval";
      this.comboBoxDataCollectionInterval.Size = new System.Drawing.Size(65, 21);
      this.comboBoxDataCollectionInterval.TabIndex = 13;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.label2.Location = new System.Drawing.Point(6, 51);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(116, 13);
      this.label2.TabIndex = 12;
      this.label2.Text = "X-Axis Window Shift %:";
      // 
      // comboBoxXWindowShiftPercent
      // 
      this.comboBoxXWindowShiftPercent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxXWindowShiftPercent.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.comboBoxXWindowShiftPercent.FormattingEnabled = true;
      this.comboBoxXWindowShiftPercent.Items.AddRange(new object[] {
            "Continuous",
            "One Percent",
            "Five Percent",
            "Ten Percent"});
      this.comboBoxXWindowShiftPercent.Location = new System.Drawing.Point(132, 48);
      this.comboBoxXWindowShiftPercent.Name = "comboBoxXWindowShiftPercent";
      this.comboBoxXWindowShiftPercent.Size = new System.Drawing.Size(80, 21);
      this.comboBoxXWindowShiftPercent.TabIndex = 11;
      // 
      // groupBox5
      // 
      this.groupBox5.Controls.Add(this.radioButtonDate);
      this.groupBox5.Controls.Add(this.radioButtonDateTime);
      this.groupBox5.Controls.Add(this.radioButtonTime);
      this.groupBox5.Controls.Add(this.radioButtonSecondsMinutes);
      this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
      this.groupBox5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox5.Location = new System.Drawing.Point(0, 0);
      this.groupBox5.Name = "groupBox5";
      this.groupBox5.Size = new System.Drawing.Size(371, 70);
      this.groupBox5.TabIndex = 12;
      this.groupBox5.TabStop = false;
      this.groupBox5.Text = "X-Axis Display Units";
      // 
      // radioButtonDate
      // 
      this.radioButtonDate.AutoSize = true;
      this.radioButtonDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonDate.Location = new System.Drawing.Point(158, 44);
      this.radioButtonDate.Name = "radioButtonDate";
      this.radioButtonDate.Size = new System.Drawing.Size(91, 17);
      this.radioButtonDate.TabIndex = 3;
      this.radioButtonDate.TabStop = true;
      this.radioButtonDate.Text = "Date (m/d/yy)";
      this.radioButtonDate.UseVisualStyleBackColor = true;
      // 
      // radioButtonDateTime
      // 
      this.radioButtonDateTime.AutoSize = true;
      this.radioButtonDateTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonDateTime.Location = new System.Drawing.Point(158, 21);
      this.radioButtonDateTime.Name = "radioButtonDateTime";
      this.radioButtonDateTime.Size = new System.Drawing.Size(185, 17);
      this.radioButtonDateTime.TabIndex = 2;
      this.radioButtonDateTime.TabStop = true;
      this.radioButtonDateTime.Text = "Date and Time (m/d/yy hh:mm:ss)";
      this.radioButtonDateTime.UseVisualStyleBackColor = true;
      // 
      // radioButtonTime
      // 
      this.radioButtonTime.AutoSize = true;
      this.radioButtonTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonTime.Location = new System.Drawing.Point(6, 44);
      this.radioButtonTime.Name = "radioButtonTime";
      this.radioButtonTime.Size = new System.Drawing.Size(101, 17);
      this.radioButtonTime.TabIndex = 1;
      this.radioButtonTime.TabStop = true;
      this.radioButtonTime.Text = "Time (hh:mm:ss)";
      this.radioButtonTime.UseVisualStyleBackColor = true;
      // 
      // radioButtonSecondsMinutes
      // 
      this.radioButtonSecondsMinutes.AutoSize = true;
      this.radioButtonSecondsMinutes.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.radioButtonSecondsMinutes.Location = new System.Drawing.Point(6, 21);
      this.radioButtonSecondsMinutes.Name = "radioButtonSecondsMinutes";
      this.radioButtonSecondsMinutes.Size = new System.Drawing.Size(115, 17);
      this.radioButtonSecondsMinutes.TabIndex = 0;
      this.radioButtonSecondsMinutes.TabStop = true;
      this.radioButtonSecondsMinutes.Text = "Seconds / Minutes";
      this.radioButtonSecondsMinutes.UseVisualStyleBackColor = true;
      // 
      // panel1
      // 
      this.panel1.Controls.Add(this.buttonOK);
      this.panel1.Controls.Add(this.buttonCancel);
      this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.panel1.Location = new System.Drawing.Point(3, 427);
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size(379, 66);
      this.panel1.TabIndex = 2;
      // 
      // buttonOK
      // 
      this.buttonOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonOK.Location = new System.Drawing.Point(122, 3);
      this.buttonOK.Name = "buttonOK";
      this.buttonOK.Size = new System.Drawing.Size(120, 60);
      this.buttonOK.TabIndex = 8;
      this.buttonOK.Text = "OK";
      this.buttonOK.UseVisualStyleBackColor = true;
      this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
      // 
      // buttonCancel
      // 
      this.buttonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.buttonCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.buttonCancel.Location = new System.Drawing.Point(248, 3);
      this.buttonCancel.Name = "buttonCancel";
      this.buttonCancel.Size = new System.Drawing.Size(120, 60);
      this.buttonCancel.TabIndex = 7;
      this.buttonCancel.Text = "Cancel";
      this.buttonCancel.UseVisualStyleBackColor = true;
      this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
      // 
      // groupBox1
      // 
      this.groupBox1.AutoSize = true;
      this.groupBox1.Controls.Add(this.tableLayoutPanelTraceSettings);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.groupBox1.Location = new System.Drawing.Point(6, 215);
      this.groupBox1.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(373, 206);
      this.groupBox1.TabIndex = 3;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Trace Settings";
      // 
      // tableLayoutPanelTraceSettings
      // 
      this.tableLayoutPanelTraceSettings.AutoScroll = true;
      this.tableLayoutPanelTraceSettings.ColumnCount = 4;
      this.tableLayoutPanelTraceSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
      this.tableLayoutPanelTraceSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
      this.tableLayoutPanelTraceSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 134F));
      this.tableLayoutPanelTraceSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 40F));
      this.tableLayoutPanelTraceSettings.Dock = System.Windows.Forms.DockStyle.Fill;
      this.tableLayoutPanelTraceSettings.Location = new System.Drawing.Point(3, 18);
      this.tableLayoutPanelTraceSettings.Name = "tableLayoutPanelTraceSettings";
      this.tableLayoutPanelTraceSettings.Padding = new System.Windows.Forms.Padding(0, 0, 20, 0);
      this.tableLayoutPanelTraceSettings.RowCount = 1;
      this.tableLayoutPanelTraceSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 185F));
      this.tableLayoutPanelTraceSettings.Size = new System.Drawing.Size(367, 185);
      this.tableLayoutPanelTraceSettings.TabIndex = 1;
      // 
      // ucNumPad1
      // 
      this.ucNumPad1.AutoSize = true;
      this.ucNumPad1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
      this.ucNumPad1.CurrentSetting = 0D;
      this.ucNumPad1.Location = new System.Drawing.Point(394, 3);
      this.ucNumPad1.MaximumSize = new System.Drawing.Size(267, 496);
      this.ucNumPad1.MinimumSize = new System.Drawing.Size(267, 496);
      this.ucNumPad1.Name = "ucNumPad1";
      this.ucNumPad1.Size = new System.Drawing.Size(267, 496);
      this.ucNumPad1.TabIndex = 3;
      this.ucNumPad1.CurrentSettingChanged += new VTIWindowsControlLibrary.Components.CurrentSettingChangedEventHandler(this.ucNumPad1_CurrentSettingChanged);
      // 
      // PlotPropForm
      // 
      this.AcceptButton = this.buttonOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.buttonCancel;
      this.ClientSize = new System.Drawing.Size(664, 496);
      this.Controls.Add(this.tableLayoutPanel1);
      this.DoubleBuffered = true;
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "PlotPropForm";
      this.ShowInTaskbar = false;
      this.Text = "Plot Properties";
      this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPlotProp_FormClosing);
      this.VisibleChanged += new System.EventHandler(this.PlotPropForm_VisibleChanged);
      this.tableLayoutPanel1.ResumeLayout(false);
      this.tableLayoutPanel1.PerformLayout();
      this.tableLayoutPanel2.ResumeLayout(false);
      this.tableLayoutPanel2.PerformLayout();
      this.tabControlSettings.ResumeLayout(false);
      this.tabPageAxisSettings.ResumeLayout(false);
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYMinExp)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYMaxExp)).EndInit();
      this.tabPageOptions.ResumeLayout(false);
      this.groupBox4.ResumeLayout(false);
      this.groupBox4.PerformLayout();
      this.groupBox5.ResumeLayout(false);
      this.groupBox5.PerformLayout();
      this.panel1.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
    private System.Windows.Forms.TabControl tabControlSettings;
    private System.Windows.Forms.TabPage tabPageAxisSettings;
    private System.Windows.Forms.TabPage tabPageOptions;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button buttonOK;
    private System.Windows.Forms.Button buttonCancel;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.TableLayoutPanel tableLayoutPanelTraceSettings;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox textBoxYMax;
    public System.Windows.Forms.NumericUpDown numericUpDownYMinExp { get; set; }
    private System.Windows.Forms.Label labelx10min;
    private System.Windows.Forms.TextBox textBoxYMin;
    private System.Windows.Forms.Label label4;
    public System.Windows.Forms.NumericUpDown numericUpDownYMaxExp { get; set; }
    private System.Windows.Forms.Label labelx10max;
    private System.Windows.Forms.RadioButton radioButtonLog;
    private System.Windows.Forms.RadioButton radioButtonLinear;
    private System.Windows.Forms.CheckBox checkBoxAutoScaleMin;
    private System.Windows.Forms.CheckBox checkBoxAutoScaleMax;
    private System.Windows.Forms.RadioButton radioButtonMinutes;
    /// <summary>
    /// get radio button minutes setting
    /// </summary>
    public System.Windows.Forms.RadioButton RadioButtonMinutes { get { return radioButtonMinutes; } }
    public System.Windows.Forms.RadioButton RadioButtonSeconds { get { return radioButtonSeconds; } }
    private System.Windows.Forms.RadioButton radioButtonSeconds;
    private System.Windows.Forms.TextBox textBoxXWindow;
    private System.Windows.Forms.Label label7;
    private System.Windows.Forms.TextBox textBoxXMax;
    private System.Windows.Forms.Label label6;
    private System.Windows.Forms.TextBox textBoxXMin;
    private VTIWindowsControlLibrary.Components.NumberPadControl ucNumPad1;
    private System.Windows.Forms.ColorDialog colorDialog1;
    private System.Windows.Forms.Label label5;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ComboBox comboBoxXWindowShiftPercent;
    private System.Windows.Forms.GroupBox groupBox5;
    private System.Windows.Forms.RadioButton radioButtonDate;
    private System.Windows.Forms.RadioButton radioButtonDateTime;
    private System.Windows.Forms.RadioButton radioButtonTime;
    private System.Windows.Forms.RadioButton radioButtonSecondsMinutes;
    private System.Windows.Forms.Label label8;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.ComboBox comboBoxDataCollectionInterval;
    private System.Windows.Forms.CheckBox checkBoxShowComments;
    private NumericSlider numericSliderDecimationThreshold;
    private System.Windows.Forms.Label labelDecimationThreshold;
    private System.Windows.Forms.Label label11;
    private System.Windows.Forms.CheckBox checkBoxAutoSave;
  }
}