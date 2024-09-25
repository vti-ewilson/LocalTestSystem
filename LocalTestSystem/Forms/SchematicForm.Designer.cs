using LocalTestSystem.Classes;

namespace LocalTestSystem.Forms
{
    partial class SchematicForm
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
            // let base class know that the form is being disposed
            base.bIsDisposing = true;
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchematicForm));
			this.schematicPanelLeft = new VTIWindowsControlLibrary.Components.SchematicPanel();
			this.schematicCheckBox22 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox15 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.button1 = new System.Windows.Forms.Button();
			this.schematicCheckBox13 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox12 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.schematicPanelMain = new VTIWindowsControlLibrary.Components.SchematicPanel();
			this.schematicCheckBox3 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox20 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox10 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox18 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox21 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox19 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox17 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox16 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schlblP3Torrcon = new VTIWindowsControlLibrary.Components.SchematicLabel();
			this.schematicCheckBox2 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.SchLblScale = new VTIWindowsControlLibrary.Components.SchematicLabel();
			this.schematicCheckBox9 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox7 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.SchLblP2PSI = new VTIWindowsControlLibrary.Components.SchematicLabel();
			this.schematicCheckBox6 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox5 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox1 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox11 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox4 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.buttonClose = new System.Windows.Forms.Button();
			this.schematicCheckBox14 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicCheckBox8 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
			this.schematicPanelLeft.SuspendLayout();
			this.schematicPanelMain.SuspendLayout();
			this.schematicCheckBox4.SuspendLayout();
			this.SuspendLayout();
			// 
			// schematicPanelLeft
			// 
			this.schematicPanelLeft.Controls.Add(this.schematicCheckBox22);
			this.schematicPanelLeft.Controls.Add(this.schematicCheckBox15);
			this.schematicPanelLeft.Controls.Add(this.button1);
			this.schematicPanelLeft.Controls.Add(this.schematicCheckBox13);
			this.schematicPanelLeft.Controls.Add(this.schematicCheckBox12);
			this.schematicPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
			this.schematicPanelLeft.Location = new System.Drawing.Point(0, 0);
			this.schematicPanelLeft.MetafileName = null;
			this.schematicPanelLeft.Name = "schematicPanelLeft";
			this.schematicPanelLeft.Size = new System.Drawing.Size(141, 733);
			this.schematicPanelLeft.TabIndex = 3;
			// 
			// schematicCheckBox22
			// 
			this.schematicCheckBox22.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox22.AutoSize = true;
			this.schematicCheckBox22.Checked = false;
			this.schematicCheckBox22.DigitalIO = null;
			this.schematicCheckBox22.DigitalIOName = "DrainButton";
			this.schematicCheckBox22.DisplayAsIndicator = true;
			this.schematicCheckBox22.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox22.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox22.Location = new System.Drawing.Point(12, 296);
			this.schematicCheckBox22.Name = "schematicCheckBox22";
			this.schematicCheckBox22.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox22.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox22.ReverseStates = false;
			this.schematicCheckBox22.Size = new System.Drawing.Size(120, 50);
			this.schematicCheckBox22.TabIndex = 6;
			// 
			// schematicCheckBox15
			// 
			this.schematicCheckBox15.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox15.AutoSize = true;
			this.schematicCheckBox15.Checked = false;
			this.schematicCheckBox15.DigitalIO = null;
			this.schematicCheckBox15.DigitalIOName = "FillButton";
			this.schematicCheckBox15.DisplayAsIndicator = true;
			this.schematicCheckBox15.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox15.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox15.Location = new System.Drawing.Point(12, 240);
			this.schematicCheckBox15.Name = "schematicCheckBox15";
			this.schematicCheckBox15.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox15.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox15.ReverseStates = false;
			this.schematicCheckBox15.Size = new System.Drawing.Size(120, 50);
			this.schematicCheckBox15.TabIndex = 6;
			// 
			// button1
			// 
			this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.button1.Location = new System.Drawing.Point(12, 12);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(120, 50);
			this.button1.TabIndex = 24;
			this.button1.Text = "CLOSE";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click);
			// 
			// schematicCheckBox13
			// 
			this.schematicCheckBox13.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox13.AutoSize = true;
			this.schematicCheckBox13.Checked = false;
			this.schematicCheckBox13.DigitalIO = null;
			this.schematicCheckBox13.DigitalIOName = "Acknowledge";
			this.schematicCheckBox13.DisplayAsIndicator = true;
			this.schematicCheckBox13.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox13.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox13.Location = new System.Drawing.Point(12, 130);
			this.schematicCheckBox13.Name = "schematicCheckBox13";
			this.schematicCheckBox13.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox13.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox13.ReverseStates = false;
			this.schematicCheckBox13.Size = new System.Drawing.Size(120, 50);
			this.schematicCheckBox13.TabIndex = 5;
			// 
			// schematicCheckBox12
			// 
			this.schematicCheckBox12.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox12.AutoSize = true;
			this.schematicCheckBox12.Checked = false;
			this.schematicCheckBox12.DigitalIO = null;
			this.schematicCheckBox12.DigitalIOName = "MCRPower";
			this.schematicCheckBox12.DisplayAsIndicator = true;
			this.schematicCheckBox12.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox12.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox12.Location = new System.Drawing.Point(12, 74);
			this.schematicCheckBox12.Name = "schematicCheckBox12";
			this.schematicCheckBox12.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox12.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox12.ReverseStates = false;
			this.schematicCheckBox12.Size = new System.Drawing.Size(120, 50);
			this.schematicCheckBox12.TabIndex = 5;
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 500;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// schematicPanelMain
			// 
			this.schematicPanelMain.BackgroundImage = global::LocalTestSystem.Properties.Resources.D545_schematic3;
			this.schematicPanelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox3);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox20);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox10);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox18);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox21);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox19);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox17);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox16);
			this.schematicPanelMain.Controls.Add(this.schlblP3Torrcon);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox2);
			this.schematicPanelMain.Controls.Add(this.SchLblScale);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox9);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox7);
			this.schematicPanelMain.Controls.Add(this.SchLblP2PSI);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox6);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox5);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox1);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox11);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox4);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox14);
			this.schematicPanelMain.Controls.Add(this.schematicCheckBox8);
			this.schematicPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
			this.schematicPanelMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.schematicPanelMain.Location = new System.Drawing.Point(141, 0);
			this.schematicPanelMain.MetafileName = "";
			this.schematicPanelMain.Name = "schematicPanelMain";
			this.schematicPanelMain.Size = new System.Drawing.Size(754, 733);
			this.schematicPanelMain.TabIndex = 4;
			// 
			// schematicCheckBox3
			// 
			this.schematicCheckBox3.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox3.AutoSize = true;
			this.schematicCheckBox3.Checked = false;
			this.schematicCheckBox3.DigitalIO = null;
			this.schematicCheckBox3.DigitalIOName = "RecoveryTankLowLevelSensor";
			this.schematicCheckBox3.DisplayAsIndicator = true;
			this.schematicCheckBox3.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox3.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox3.Location = new System.Drawing.Point(144, 331);
			this.schematicCheckBox3.Name = "schematicCheckBox3";
			this.schematicCheckBox3.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox3.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox3.ReverseStates = false;
			this.schematicCheckBox3.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox3.TabIndex = 7;
			// 
			// schematicCheckBox20
			// 
			this.schematicCheckBox20.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox20.AutoSize = true;
			this.schematicCheckBox20.Checked = false;
			this.schematicCheckBox20.DigitalIO = null;
			this.schematicCheckBox20.DigitalIOName = "RecoveryTankTopLevelSensor";
			this.schematicCheckBox20.DisplayAsIndicator = true;
			this.schematicCheckBox20.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox20.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox20.Location = new System.Drawing.Point(144, 188);
			this.schematicCheckBox20.Name = "schematicCheckBox20";
			this.schematicCheckBox20.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox20.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox20.ReverseStates = false;
			this.schematicCheckBox20.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox20.TabIndex = 7;
			// 
			// schematicCheckBox10
			// 
			this.schematicCheckBox10.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox10.AutoSize = true;
			this.schematicCheckBox10.Checked = false;
			this.schematicCheckBox10.DigitalIO = null;
			this.schematicCheckBox10.DigitalIOName = "SupplyTankTopLevelSensor";
			this.schematicCheckBox10.DisplayAsIndicator = true;
			this.schematicCheckBox10.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox10.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox10.Location = new System.Drawing.Point(428, 188);
			this.schematicCheckBox10.Name = "schematicCheckBox10";
			this.schematicCheckBox10.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox10.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox10.ReverseStates = false;
			this.schematicCheckBox10.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox10.TabIndex = 7;
			// 
			// schematicCheckBox18
			// 
			this.schematicCheckBox18.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox18.AutoSize = true;
			this.schematicCheckBox18.Checked = false;
			this.schematicCheckBox18.DigitalIO = null;
			this.schematicCheckBox18.DigitalIOName = "SupplyTankHighLevelSensor";
			this.schematicCheckBox18.DisplayAsIndicator = true;
			this.schematicCheckBox18.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox18.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox18.Location = new System.Drawing.Point(428, 218);
			this.schematicCheckBox18.Name = "schematicCheckBox18";
			this.schematicCheckBox18.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox18.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox18.ReverseStates = false;
			this.schematicCheckBox18.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox18.TabIndex = 7;
			// 
			// schematicCheckBox21
			// 
			this.schematicCheckBox21.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox21.AutoSize = true;
			this.schematicCheckBox21.Checked = false;
			this.schematicCheckBox21.DigitalIO = null;
			this.schematicCheckBox21.DigitalIOName = "SupplyTankLowLevelSensor";
			this.schematicCheckBox21.DisplayAsIndicator = true;
			this.schematicCheckBox21.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox21.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox21.Location = new System.Drawing.Point(428, 333);
			this.schematicCheckBox21.Name = "schematicCheckBox21";
			this.schematicCheckBox21.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox21.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox21.ReverseStates = false;
			this.schematicCheckBox21.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox21.TabIndex = 7;
			// 
			// schematicCheckBox19
			// 
			this.schematicCheckBox19.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox19.AutoSize = true;
			this.schematicCheckBox19.Checked = false;
			this.schematicCheckBox19.DigitalIO = null;
			this.schematicCheckBox19.DigitalIOName = "RecoveryTankHighLevelSensor";
			this.schematicCheckBox19.DisplayAsIndicator = true;
			this.schematicCheckBox19.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox19.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox19.Location = new System.Drawing.Point(144, 216);
			this.schematicCheckBox19.Name = "schematicCheckBox19";
			this.schematicCheckBox19.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox19.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox19.ReverseStates = false;
			this.schematicCheckBox19.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox19.TabIndex = 6;
			// 
			// schematicCheckBox17
			// 
			this.schematicCheckBox17.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox17.AutoSize = true;
			this.schematicCheckBox17.Checked = false;
			this.schematicCheckBox17.DigitalIO = null;
			this.schematicCheckBox17.DigitalIOName = "BlowerRORValve";
			this.schematicCheckBox17.DisplayAsIndicator = false;
			this.schematicCheckBox17.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox17.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox17.Location = new System.Drawing.Point(229, 438);
			this.schematicCheckBox17.Name = "schematicCheckBox17";
			this.schematicCheckBox17.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox17.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox17.ReverseStates = false;
			this.schematicCheckBox17.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox17.TabIndex = 24;
			// 
			// schematicCheckBox16
			// 
			this.schematicCheckBox16.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox16.AutoSize = true;
			this.schematicCheckBox16.Checked = false;
			this.schematicCheckBox16.DigitalIO = null;
			this.schematicCheckBox16.DigitalIOName = "BlowerEnable";
			this.schematicCheckBox16.DisplayAsIndicator = false;
			this.schematicCheckBox16.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox16.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox16.Location = new System.Drawing.Point(101, 521);
			this.schematicCheckBox16.Name = "schematicCheckBox16";
			this.schematicCheckBox16.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox16.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox16.ReverseStates = false;
			this.schematicCheckBox16.Size = new System.Drawing.Size(120, 50);
			this.schematicCheckBox16.TabIndex = 42;
			// 
			// schlblP3Torrcon
			// 
			this.schlblP3Torrcon.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.schlblP3Torrcon.AutoSize = true;
			this.schlblP3Torrcon.BackColor = System.Drawing.Color.Black;
			this.schlblP3Torrcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.schlblP3Torrcon.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.schlblP3Torrcon.ForeColor = System.Drawing.Color.Yellow;
			this.schlblP3Torrcon.Location = new System.Drawing.Point(239, 48);
			this.schlblP3Torrcon.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.schlblP3Torrcon.Name = "schlblP3Torrcon";
			this.schlblP3Torrcon.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schlblP3Torrcon.OriginalSize = new System.Drawing.Size(0, 0);
			this.schlblP3Torrcon.Size = new System.Drawing.Size(143, 22);
			this.schlblP3Torrcon.TabIndex = 41;
			this.schlblP3Torrcon.Text = "999999 microns ";
			this.schlblP3Torrcon.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// schematicCheckBox2
			// 
			this.schematicCheckBox2.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox2.AutoSize = true;
			this.schematicCheckBox2.Checked = false;
			this.schematicCheckBox2.DigitalIO = null;
			this.schematicCheckBox2.DigitalIOName = "SupplyVacuumValve";
			this.schematicCheckBox2.DisplayAsIndicator = false;
			this.schematicCheckBox2.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox2.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox2.Location = new System.Drawing.Point(467, 84);
			this.schematicCheckBox2.Name = "schematicCheckBox2";
			this.schematicCheckBox2.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox2.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox2.ReverseStates = false;
			this.schematicCheckBox2.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox2.TabIndex = 23;
			// 
			// SchLblScale
			// 
			this.SchLblScale.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.SchLblScale.AutoSize = true;
			this.SchLblScale.BackColor = System.Drawing.Color.Black;
			this.SchLblScale.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.SchLblScale.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SchLblScale.ForeColor = System.Drawing.Color.Yellow;
			this.SchLblScale.Location = new System.Drawing.Point(265, 84);
			this.SchLblScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.SchLblScale.Name = "SchLblScale";
			this.SchLblScale.OriginalLocation = new System.Drawing.Point(0, 0);
			this.SchLblScale.OriginalSize = new System.Drawing.Size(0, 0);
			this.SchLblScale.Size = new System.Drawing.Size(113, 22);
			this.SchLblScale.TabIndex = 41;
			this.SchLblScale.Text = "100.001 Torr";
			// 
			// schematicCheckBox9
			// 
			this.schematicCheckBox9.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox9.AutoSize = true;
			this.schematicCheckBox9.Checked = false;
			this.schematicCheckBox9.DigitalIO = null;
			this.schematicCheckBox9.DigitalIOName = "MakeupIntakeValve";
			this.schematicCheckBox9.DisplayAsIndicator = false;
			this.schematicCheckBox9.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox9.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox9.Location = new System.Drawing.Point(161, 367);
			this.schematicCheckBox9.Name = "schematicCheckBox9";
			this.schematicCheckBox9.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox9.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox9.ReverseStates = false;
			this.schematicCheckBox9.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox9.TabIndex = 23;
			// 
			// schematicCheckBox7
			// 
			this.schematicCheckBox7.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox7.AutoSize = true;
			this.schematicCheckBox7.Checked = false;
			this.schematicCheckBox7.DigitalIO = null;
			this.schematicCheckBox7.DigitalIOName = "RecoveryOutletValve";
			this.schematicCheckBox7.DisplayAsIndicator = false;
			this.schematicCheckBox7.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox7.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox7.Location = new System.Drawing.Point(239, 367);
			this.schematicCheckBox7.Name = "schematicCheckBox7";
			this.schematicCheckBox7.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox7.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox7.ReverseStates = false;
			this.schematicCheckBox7.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox7.TabIndex = 23;
			// 
			// SchLblP2PSI
			// 
			this.SchLblP2PSI.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.SchLblP2PSI.AutoSize = true;
			this.SchLblP2PSI.BackColor = System.Drawing.Color.Black;
			this.SchLblP2PSI.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.SchLblP2PSI.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.SchLblP2PSI.ForeColor = System.Drawing.Color.Yellow;
			this.SchLblP2PSI.Location = new System.Drawing.Point(420, 12);
			this.SchLblP2PSI.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.SchLblP2PSI.Name = "SchLblP2PSI";
			this.SchLblP2PSI.OriginalLocation = new System.Drawing.Point(0, 0);
			this.SchLblP2PSI.OriginalSize = new System.Drawing.Size(0, 0);
			this.SchLblP2PSI.Size = new System.Drawing.Size(109, 22);
			this.SchLblP2PSI.TabIndex = 41;
			this.SchLblP2PSI.Text = "10000 PSIG";
			this.SchLblP2PSI.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// schematicCheckBox6
			// 
			this.schematicCheckBox6.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox6.AutoSize = true;
			this.schematicCheckBox6.Checked = false;
			this.schematicCheckBox6.DigitalIO = null;
			this.schematicCheckBox6.DigitalIOName = "PumpDrainIntakeValve";
			this.schematicCheckBox6.DisplayAsIndicator = false;
			this.schematicCheckBox6.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox6.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox6.Location = new System.Drawing.Point(212, 150);
			this.schematicCheckBox6.Name = "schematicCheckBox6";
			this.schematicCheckBox6.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox6.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox6.ReverseStates = false;
			this.schematicCheckBox6.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox6.TabIndex = 23;
			// 
			// schematicCheckBox5
			// 
			this.schematicCheckBox5.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox5.AutoSize = true;
			this.schematicCheckBox5.Checked = false;
			this.schematicCheckBox5.DigitalIO = null;
			this.schematicCheckBox5.DigitalIOName = "OilFillValve";
			this.schematicCheckBox5.DisplayAsIndicator = false;
			this.schematicCheckBox5.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox5.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox5.Location = new System.Drawing.Point(538, 40);
			this.schematicCheckBox5.Name = "schematicCheckBox5";
			this.schematicCheckBox5.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox5.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox5.ReverseStates = false;
			this.schematicCheckBox5.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox5.TabIndex = 23;
			// 
			// schematicCheckBox1
			// 
			this.schematicCheckBox1.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox1.AutoSize = true;
			this.schematicCheckBox1.Checked = false;
			this.schematicCheckBox1.DigitalIO = null;
			this.schematicCheckBox1.DigitalIOName = "VacuumPumpEnable";
			this.schematicCheckBox1.DisplayAsIndicator = false;
			this.schematicCheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox1.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox1.Location = new System.Drawing.Point(71, 636);
			this.schematicCheckBox1.Name = "schematicCheckBox1";
			this.schematicCheckBox1.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox1.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox1.ReverseStates = false;
			this.schematicCheckBox1.Size = new System.Drawing.Size(150, 60);
			this.schematicCheckBox1.TabIndex = 23;
			// 
			// schematicCheckBox11
			// 
			this.schematicCheckBox11.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox11.AutoSize = true;
			this.schematicCheckBox11.Checked = false;
			this.schematicCheckBox11.DigitalIO = null;
			this.schematicCheckBox11.DigitalIOName = "OilRecircPumpEnable";
			this.schematicCheckBox11.DisplayAsIndicator = false;
			this.schematicCheckBox11.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox11.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox11.Location = new System.Drawing.Point(592, 417);
			this.schematicCheckBox11.Name = "schematicCheckBox11";
			this.schematicCheckBox11.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox11.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox11.ReverseStates = false;
			this.schematicCheckBox11.Size = new System.Drawing.Size(150, 60);
			this.schematicCheckBox11.TabIndex = 23;
			// 
			// schematicCheckBox4
			// 
			this.schematicCheckBox4.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox4.AutoSize = true;
			this.schematicCheckBox4.Checked = false;
			this.schematicCheckBox4.Controls.Add(this.buttonClose);
			this.schematicCheckBox4.DigitalIO = null;
			this.schematicCheckBox4.DigitalIOName = "SupplyOutletValve";
			this.schematicCheckBox4.DisplayAsIndicator = false;
			this.schematicCheckBox4.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox4.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox4.Location = new System.Drawing.Point(525, 367);
			this.schematicCheckBox4.Name = "schematicCheckBox4";
			this.schematicCheckBox4.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox4.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox4.ReverseStates = false;
			this.schematicCheckBox4.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox4.TabIndex = 23;
			// 
			// buttonClose
			// 
			this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.buttonClose.Location = new System.Drawing.Point(-45, -10);
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.Size = new System.Drawing.Size(120, 50);
			this.buttonClose.TabIndex = 6;
			this.buttonClose.Text = "CLOSE";
			this.buttonClose.UseVisualStyleBackColor = true;
			// 
			// schematicCheckBox14
			// 
			this.schematicCheckBox14.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox14.AutoSize = true;
			this.schematicCheckBox14.Checked = false;
			this.schematicCheckBox14.DigitalIO = null;
			this.schematicCheckBox14.DigitalIOName = "RecoveryVacuumValve";
			this.schematicCheckBox14.DisplayAsIndicator = false;
			this.schematicCheckBox14.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox14.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox14.Location = new System.Drawing.Point(248, 130);
			this.schematicCheckBox14.Name = "schematicCheckBox14";
			this.schematicCheckBox14.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox14.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox14.ReverseStates = false;
			this.schematicCheckBox14.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox14.TabIndex = 23;
			// 
			// schematicCheckBox8
			// 
			this.schematicCheckBox8.Appearance = System.Windows.Forms.Appearance.Button;
			this.schematicCheckBox8.AutoSize = true;
			this.schematicCheckBox8.Checked = false;
			this.schematicCheckBox8.DigitalIO = null;
			this.schematicCheckBox8.DigitalIOName = "SupplyRecircValve";
			this.schematicCheckBox8.DisplayAsIndicator = false;
			this.schematicCheckBox8.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			this.schematicCheckBox8.IndicatorColor = System.Drawing.Color.Yellow;
			this.schematicCheckBox8.Location = new System.Drawing.Point(525, 103);
			this.schematicCheckBox8.Name = "schematicCheckBox8";
			this.schematicCheckBox8.OriginalLocation = new System.Drawing.Point(0, 0);
			this.schematicCheckBox8.OriginalSize = new System.Drawing.Size(0, 0);
			this.schematicCheckBox8.ReverseStates = false;
			this.schematicCheckBox8.Size = new System.Drawing.Size(30, 30);
			this.schematicCheckBox8.TabIndex = 23;
			// 
			// SchematicForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(895, 733);
			this.Controls.Add(this.schematicPanelMain);
			this.Controls.Add(this.schematicPanelLeft);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "SchematicForm";
			this.Text = "Plumbing Schematic";
			this.VisibleChanged += new System.EventHandler(this.SchematicForm_VisibleChanged);
			this.schematicPanelLeft.ResumeLayout(false);
			this.schematicPanelMain.ResumeLayout(false);
			this.schematicPanelMain.PerformLayout();
			this.schematicCheckBox4.ResumeLayout(false);
			this.ResumeLayout(false);

        }

        #endregion

        private VTIWindowsControlLibrary.Components.SchematicPanel schematicPanelLeft;

        private VTIWindowsControlLibrary.Components.SchematicPanel schematicPanelMain;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox12;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox8;
        private System.Windows.Forms.Timer timer1;
        private VTIWindowsControlLibrary.Components.SchematicLabel schlblP3Torrcon;
        private VTIWindowsControlLibrary.Components.SchematicLabel SchLblP2PSI;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox2;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox9;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox7;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox6;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox5;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox4;
        private VTIWindowsControlLibrary.Components.SchematicLabel SchLblScale;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox13;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox11;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox14;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button buttonClose;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox16;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox17;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox19;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox3;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox20;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox10;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox18;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox21;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox22;
		private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox15;
	}
}