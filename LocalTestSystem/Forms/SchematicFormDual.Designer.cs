using LocalTestSystem.Classes;

namespace LocalTestSystem.Forms
{
    partial class SchematicFormDual
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SchematicFormDual));
            this.schematicPanelLeft = new VTIWindowsControlLibrary.Components.SchematicPanel();
            this.btnStart2 = new System.Windows.Forms.Button();
            this.txtStart2 = new System.Windows.Forms.TextBox();
            this.btnStop2 = new System.Windows.Forms.Button();
            this.txtStop2 = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.schematicCheckBox12 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.schematicCheckBox14 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox10 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox13 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox6 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox11 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicPanelMain = new VTIWindowsControlLibrary.Components.SchematicPanel();
            this.schlblP5POEPress = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.schlblP6POEEvac = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.schlblP2PVEPress = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.POEScale = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.PVEScale = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.schlblP3PVEEvac = new VTIWindowsControlLibrary.Components.SchematicLabel();
            this.schematicCheckBox21 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox22 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox23 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox24 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox25 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox26 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox27 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox28 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox29 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox30 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox31 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox32 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox33 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox34 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox35 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox36 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox2 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox1 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox8 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox9 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox7 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox5 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox19 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox18 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox20 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox17 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox4 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox15 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox3 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox16 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.schematicCheckBox37 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicCheckBox38 = new VTIWindowsControlLibrary.Components.SchematicCheckBox();
            this.schematicPanelLeft.SuspendLayout();
            this.schematicPanelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // schematicPanelLeft
            // 
            this.schematicPanelLeft.Controls.Add(this.btnStart2);
            this.schematicPanelLeft.Controls.Add(this.txtStart2);
            this.schematicPanelLeft.Controls.Add(this.btnStop2);
            this.schematicPanelLeft.Controls.Add(this.txtStop2);
            this.schematicPanelLeft.Controls.Add(this.btnStart);
            this.schematicPanelLeft.Controls.Add(this.buttonClose);
            this.schematicPanelLeft.Controls.Add(this.txtStart);
            this.schematicPanelLeft.Controls.Add(this.btnStop);
            this.schematicPanelLeft.Controls.Add(this.schematicCheckBox12);
            this.schematicPanelLeft.Controls.Add(this.txtStop);
            this.schematicPanelLeft.Controls.Add(this.schematicCheckBox14);
            this.schematicPanelLeft.Controls.Add(this.schematicCheckBox10);
            this.schematicPanelLeft.Controls.Add(this.schematicCheckBox13);
            this.schematicPanelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.schematicPanelLeft.Location = new System.Drawing.Point(0, 0);
            this.schematicPanelLeft.MetafileName = null;
            this.schematicPanelLeft.Name = "schematicPanelLeft";
            this.schematicPanelLeft.Size = new System.Drawing.Size(141, 620);
            this.schematicPanelLeft.TabIndex = 3;
            // 
            // btnStart2
            // 
            this.btnStart2.Location = new System.Drawing.Point(26, 332);
            this.btnStart2.Name = "btnStart2";
            this.btnStart2.Size = new System.Drawing.Size(90, 19);
            this.btnStart2.TabIndex = 46;
            this.btnStart2.Text = "Set Start Cnt";
            this.btnStart2.UseVisualStyleBackColor = true;
            this.btnStart2.Visible = false;
            this.btnStart2.Click += new System.EventHandler(this.btnStart2_Click);
            // 
            // txtStart2
            // 
            this.txtStart2.Location = new System.Drawing.Point(26, 314);
            this.txtStart2.Name = "txtStart2";
            this.txtStart2.Size = new System.Drawing.Size(90, 20);
            this.txtStart2.TabIndex = 44;
            this.txtStart2.Visible = false;
            this.txtStart2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStart2_KeyDown);
            // 
            // btnStop2
            // 
            this.btnStop2.Location = new System.Drawing.Point(26, 367);
            this.btnStop2.Name = "btnStop2";
            this.btnStop2.Size = new System.Drawing.Size(90, 19);
            this.btnStop2.TabIndex = 47;
            this.btnStop2.Text = "Set Stop Cnt";
            this.btnStop2.UseVisualStyleBackColor = true;
            this.btnStop2.Visible = false;
            this.btnStop2.Click += new System.EventHandler(this.btnStop2_Click);
            // 
            // txtStop2
            // 
            this.txtStop2.Location = new System.Drawing.Point(26, 349);
            this.txtStop2.Name = "txtStop2";
            this.txtStop2.Size = new System.Drawing.Size(90, 20);
            this.txtStop2.TabIndex = 45;
            this.txtStop2.Visible = false;
            this.txtStop2.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStop2_KeyDown);
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(26, 210);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(90, 19);
            this.btnStart.TabIndex = 43;
            this.btnStart.Text = "Set Start Cnt";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonClose.Location = new System.Drawing.Point(12, 12);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(120, 50);
            this.buttonClose.TabIndex = 5;
            this.buttonClose.Text = "CLOSE";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(26, 192);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(90, 20);
            this.txtStart.TabIndex = 42;
            this.txtStart.Visible = false;
            this.txtStart.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStart_KeyDown);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(26, 245);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(90, 19);
            this.btnStop.TabIndex = 43;
            this.btnStop.Text = "Set Stop Cnt";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Visible = false;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
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
            // txtStop
            // 
            this.txtStop.Location = new System.Drawing.Point(26, 227);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(90, 20);
            this.txtStop.TabIndex = 42;
            this.txtStop.Visible = false;
            this.txtStop.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtStop_KeyDown);
            // 
            // schematicCheckBox14
            // 
            this.schematicCheckBox14.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox14.AutoSize = true;
            this.schematicCheckBox14.Checked = false;
            this.schematicCheckBox14.DigitalIO = null;
            this.schematicCheckBox14.DigitalIOName = "POEAlarm";
            this.schematicCheckBox14.DisplayAsIndicator = true;
            this.schematicCheckBox14.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox14.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox14.Location = new System.Drawing.Point(12, 492);
            this.schematicCheckBox14.Name = "schematicCheckBox14";
            this.schematicCheckBox14.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox14.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox14.ReverseStates = false;
            this.schematicCheckBox14.Size = new System.Drawing.Size(120, 50);
            this.schematicCheckBox14.TabIndex = 5;
            // 
            // schematicCheckBox10
            // 
            this.schematicCheckBox10.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox10.AutoSize = true;
            this.schematicCheckBox10.Checked = false;
            this.schematicCheckBox10.DigitalIO = null;
            this.schematicCheckBox10.DigitalIOName = "PVEAlarm";
            this.schematicCheckBox10.DisplayAsIndicator = true;
            this.schematicCheckBox10.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox10.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox10.Location = new System.Drawing.Point(12, 419);
            this.schematicCheckBox10.Name = "schematicCheckBox10";
            this.schematicCheckBox10.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox10.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox10.ReverseStates = false;
            this.schematicCheckBox10.Size = new System.Drawing.Size(120, 50);
            this.schematicCheckBox10.TabIndex = 5;
            // 
            // schematicCheckBox13
            // 
            this.schematicCheckBox13.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox13.AutoSize = true;
            this.schematicCheckBox13.Checked = false;
            this.schematicCheckBox13.DigitalIO = null;
            this.schematicCheckBox13.DigitalIOName = "Acknowledge1";
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
            // schematicCheckBox6
            // 
            this.schematicCheckBox6.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox6.AutoSize = true;
            this.schematicCheckBox6.Checked = false;
            this.schematicCheckBox6.DigitalIO = null;
            this.schematicCheckBox6.DigitalIOName = "LightPVEAmber";
            this.schematicCheckBox6.DisplayAsIndicator = false;
            this.schematicCheckBox6.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox6.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox6.Location = new System.Drawing.Point(315, 413);
            this.schematicCheckBox6.Name = "schematicCheckBox6";
            this.schematicCheckBox6.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox6.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox6.ReverseStates = false;
            this.schematicCheckBox6.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox6.TabIndex = 5;
            // 
            // schematicCheckBox11
            // 
            this.schematicCheckBox11.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox11.AutoSize = true;
            this.schematicCheckBox11.Checked = false;
            this.schematicCheckBox11.DigitalIO = null;
            this.schematicCheckBox11.DigitalIOName = "LightPVEGreen";
            this.schematicCheckBox11.DisplayAsIndicator = false;
            this.schematicCheckBox11.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox11.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox11.Location = new System.Drawing.Point(315, 367);
            this.schematicCheckBox11.Name = "schematicCheckBox11";
            this.schematicCheckBox11.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox11.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox11.ReverseStates = false;
            this.schematicCheckBox11.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox11.TabIndex = 4;
            // 
            // schematicPanelMain
            // 
            // this.schematicPanelMain.BackgroundImage = global::LocalTestSystem.Properties.Resources._18890_G001_REV0;
            this.schematicPanelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.schematicPanelMain.Controls.Add(this.schlblP5POEPress);
            this.schematicPanelMain.Controls.Add(this.schlblP6POEEvac);
            this.schematicPanelMain.Controls.Add(this.schlblP2PVEPress);
            this.schematicPanelMain.Controls.Add(this.POEScale);
            this.schematicPanelMain.Controls.Add(this.PVEScale);
            this.schematicPanelMain.Controls.Add(this.schlblP3PVEEvac);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox21);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox22);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox23);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox24);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox25);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox26);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox27);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox28);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox29);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox30);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox31);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox32);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox33);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox34);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox35);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox36);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox2);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox1);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox8);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox9);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox7);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox6);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox5);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox11);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox19);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox18);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox20);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox17);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox4);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox38);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox37);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox15);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox3);
            this.schematicPanelMain.Controls.Add(this.schematicCheckBox16);
            this.schematicPanelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.schematicPanelMain.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.schematicPanelMain.Location = new System.Drawing.Point(141, 0);
            this.schematicPanelMain.MetafileName = "C:\\LocalTestSystem\\LocalTestSystem\\Resources\\schematic.wmf";
            this.schematicPanelMain.Name = "schematicPanelMain";
            this.schematicPanelMain.Size = new System.Drawing.Size(887, 620);
            this.schematicPanelMain.TabIndex = 4;
            // 
            // schlblP5POEPress
            // 
            this.schlblP5POEPress.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.schlblP5POEPress.AutoSize = true;
            this.schlblP5POEPress.BackColor = System.Drawing.Color.Black;
            this.schlblP5POEPress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.schlblP5POEPress.ForeColor = System.Drawing.Color.Yellow;
            this.schlblP5POEPress.Location = new System.Drawing.Point(608, 12);
            this.schlblP5POEPress.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.schlblP5POEPress.Name = "schlblP5POEPress";
            this.schlblP5POEPress.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schlblP5POEPress.OriginalSize = new System.Drawing.Size(0, 0);
            this.schlblP5POEPress.Size = new System.Drawing.Size(70, 15);
            this.schlblP5POEPress.TabIndex = 41;
            this.schlblP5POEPress.Text = "100.001 Torr";
            // 
            // schlblP6POEEvac
            // 
            this.schlblP6POEEvac.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.schlblP6POEEvac.AutoSize = true;
            this.schlblP6POEEvac.BackColor = System.Drawing.Color.Black;
            this.schlblP6POEEvac.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.schlblP6POEEvac.ForeColor = System.Drawing.Color.Yellow;
            this.schlblP6POEEvac.Location = new System.Drawing.Point(494, 35);
            this.schlblP6POEEvac.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.schlblP6POEEvac.Name = "schlblP6POEEvac";
            this.schlblP6POEEvac.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schlblP6POEEvac.OriginalSize = new System.Drawing.Size(0, 0);
            this.schlblP6POEEvac.Size = new System.Drawing.Size(70, 15);
            this.schlblP6POEEvac.TabIndex = 40;
            this.schlblP6POEEvac.Text = "100.001 Torr";
            // 
            // schlblP2PVEPress
            // 
            this.schlblP2PVEPress.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.schlblP2PVEPress.AutoSize = true;
            this.schlblP2PVEPress.BackColor = System.Drawing.Color.Black;
            this.schlblP2PVEPress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.schlblP2PVEPress.ForeColor = System.Drawing.Color.Yellow;
            this.schlblP2PVEPress.Location = new System.Drawing.Point(154, 12);
            this.schlblP2PVEPress.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.schlblP2PVEPress.Name = "schlblP2PVEPress";
            this.schlblP2PVEPress.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schlblP2PVEPress.OriginalSize = new System.Drawing.Size(0, 0);
            this.schlblP2PVEPress.Size = new System.Drawing.Size(70, 15);
            this.schlblP2PVEPress.TabIndex = 40;
            this.schlblP2PVEPress.Text = "100.001 Torr";
            // 
            // POEScale
            // 
            this.POEScale.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.POEScale.AutoSize = true;
            this.POEScale.BackColor = System.Drawing.Color.Black;
            this.POEScale.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.POEScale.ForeColor = System.Drawing.Color.Yellow;
            this.POEScale.Location = new System.Drawing.Point(716, 570);
            this.POEScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.POEScale.Name = "POEScale";
            this.POEScale.OriginalLocation = new System.Drawing.Point(0, 0);
            this.POEScale.OriginalSize = new System.Drawing.Size(0, 0);
            this.POEScale.Size = new System.Drawing.Size(64, 15);
            this.POEScale.TabIndex = 40;
            this.POEScale.Text = "100.001 lbs";
            // 
            // PVEScale
            // 
            this.PVEScale.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.PVEScale.AutoSize = true;
            this.PVEScale.BackColor = System.Drawing.Color.Black;
            this.PVEScale.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.PVEScale.ForeColor = System.Drawing.Color.Yellow;
            this.PVEScale.Location = new System.Drawing.Point(256, 570);
            this.PVEScale.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.PVEScale.Name = "PVEScale";
            this.PVEScale.OriginalLocation = new System.Drawing.Point(0, 0);
            this.PVEScale.OriginalSize = new System.Drawing.Size(0, 0);
            this.PVEScale.Size = new System.Drawing.Size(64, 15);
            this.PVEScale.TabIndex = 40;
            this.PVEScale.Text = "100.001 lbs";
            // 
            // schlblP3PVEEvac
            // 
            this.schlblP3PVEEvac.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.schlblP3PVEEvac.AutoSize = true;
            this.schlblP3PVEEvac.BackColor = System.Drawing.Color.Black;
            this.schlblP3PVEEvac.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.schlblP3PVEEvac.ForeColor = System.Drawing.Color.Yellow;
            this.schlblP3PVEEvac.Location = new System.Drawing.Point(51, 35);
            this.schlblP3PVEEvac.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.schlblP3PVEEvac.Name = "schlblP3PVEEvac";
            this.schlblP3PVEEvac.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schlblP3PVEEvac.OriginalSize = new System.Drawing.Size(0, 0);
            this.schlblP3PVEEvac.Size = new System.Drawing.Size(70, 15);
            this.schlblP3PVEEvac.TabIndex = 40;
            this.schlblP3PVEEvac.Text = "100.001 Torr";
            // 
            // schematicCheckBox21
            // 
            this.schematicCheckBox21.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox21.AutoSize = true;
            this.schematicCheckBox21.Checked = false;
            this.schematicCheckBox21.DigitalIO = null;
            this.schematicCheckBox21.DigitalIOName = "POEOilRecircPumpEnable";
            this.schematicCheckBox21.DisplayAsIndicator = false;
            this.schematicCheckBox21.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox21.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox21.Location = new System.Drawing.Point(608, 475);
            this.schematicCheckBox21.Name = "schematicCheckBox21";
            this.schematicCheckBox21.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox21.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox21.ReverseStates = false;
            this.schematicCheckBox21.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox21.TabIndex = 27;
            // 
            // schematicCheckBox22
            // 
            this.schematicCheckBox22.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox22.AutoSize = true;
            this.schematicCheckBox22.Checked = false;
            this.schematicCheckBox22.DigitalIO = null;
            this.schematicCheckBox22.DigitalIOName = "POEVacuumPumpEnable";
            this.schematicCheckBox22.DisplayAsIndicator = false;
            this.schematicCheckBox22.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox22.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox22.Location = new System.Drawing.Point(479, 556);
            this.schematicCheckBox22.Name = "schematicCheckBox22";
            this.schematicCheckBox22.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox22.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox22.ReverseStates = false;
            this.schematicCheckBox22.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox22.TabIndex = 28;
            // 
            // schematicCheckBox23
            // 
            this.schematicCheckBox23.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox23.AutoSize = true;
            this.schematicCheckBox23.Checked = false;
            this.schematicCheckBox23.DigitalIO = null;
            this.schematicCheckBox23.DigitalIOName = "POEDegasRateOfRiseValve";
            this.schematicCheckBox23.DisplayAsIndicator = false;
            this.schematicCheckBox23.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox23.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox23.Location = new System.Drawing.Point(494, 74);
            this.schematicCheckBox23.Name = "schematicCheckBox23";
            this.schematicCheckBox23.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox23.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox23.ReverseStates = false;
            this.schematicCheckBox23.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox23.TabIndex = 29;
            // 
            // schematicCheckBox24
            // 
            this.schematicCheckBox24.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox24.AutoSize = true;
            this.schematicCheckBox24.Checked = false;
            this.schematicCheckBox24.DigitalIO = null;
            this.schematicCheckBox24.DigitalIOName = "LightPOERed";
            this.schematicCheckBox24.DisplayAsIndicator = false;
            this.schematicCheckBox24.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox24.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox24.Location = new System.Drawing.Point(765, 459);
            this.schematicCheckBox24.Name = "schematicCheckBox24";
            this.schematicCheckBox24.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox24.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox24.ReverseStates = false;
            this.schematicCheckBox24.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox24.TabIndex = 25;
            // 
            // schematicCheckBox25
            // 
            this.schematicCheckBox25.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox25.AutoSize = true;
            this.schematicCheckBox25.Checked = false;
            this.schematicCheckBox25.DigitalIO = null;
            this.schematicCheckBox25.DigitalIOName = "POEDegasEvacuationValve";
            this.schematicCheckBox25.DisplayAsIndicator = false;
            this.schematicCheckBox25.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox25.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox25.Location = new System.Drawing.Point(548, 74);
            this.schematicCheckBox25.Name = "schematicCheckBox25";
            this.schematicCheckBox25.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox25.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox25.ReverseStates = false;
            this.schematicCheckBox25.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox25.TabIndex = 30;
            // 
            // schematicCheckBox26
            // 
            this.schematicCheckBox26.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox26.AutoSize = true;
            this.schematicCheckBox26.Checked = false;
            this.schematicCheckBox26.DigitalIO = null;
            this.schematicCheckBox26.DigitalIOName = "LightPOEAmber";
            this.schematicCheckBox26.DisplayAsIndicator = false;
            this.schematicCheckBox26.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox26.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox26.Location = new System.Drawing.Point(765, 413);
            this.schematicCheckBox26.Name = "schematicCheckBox26";
            this.schematicCheckBox26.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox26.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox26.ReverseStates = false;
            this.schematicCheckBox26.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox26.TabIndex = 26;
            // 
            // schematicCheckBox27
            // 
            this.schematicCheckBox27.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox27.AutoSize = true;
            this.schematicCheckBox27.Checked = false;
            this.schematicCheckBox27.DigitalIO = null;
            this.schematicCheckBox27.DigitalIOName = "POEDegasOutletValve";
            this.schematicCheckBox27.DisplayAsIndicator = false;
            this.schematicCheckBox27.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox27.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox27.Location = new System.Drawing.Point(634, 415);
            this.schematicCheckBox27.Name = "schematicCheckBox27";
            this.schematicCheckBox27.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox27.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox27.ReverseStates = false;
            this.schematicCheckBox27.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox27.TabIndex = 31;
            // 
            // schematicCheckBox28
            // 
            this.schematicCheckBox28.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox28.AutoSize = true;
            this.schematicCheckBox28.Checked = false;
            this.schematicCheckBox28.DigitalIO = null;
            this.schematicCheckBox28.DigitalIOName = "LightPOEGreen";
            this.schematicCheckBox28.DisplayAsIndicator = false;
            this.schematicCheckBox28.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox28.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox28.Location = new System.Drawing.Point(765, 367);
            this.schematicCheckBox28.Name = "schematicCheckBox28";
            this.schematicCheckBox28.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox28.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox28.ReverseStates = false;
            this.schematicCheckBox28.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox28.TabIndex = 24;
            // 
            // schematicCheckBox29
            // 
            this.schematicCheckBox29.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox29.AutoSize = true;
            this.schematicCheckBox29.Checked = false;
            this.schematicCheckBox29.DigitalIO = null;
            this.schematicCheckBox29.DigitalIOName = "PVEVacuumPumpOilLevelOK";
            this.schematicCheckBox29.DisplayAsIndicator = false;
            this.schematicCheckBox29.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox29.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox29.Location = new System.Drawing.Point(558, 492);
            this.schematicCheckBox29.Name = "schematicCheckBox29";
            this.schematicCheckBox29.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox29.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox29.ReverseStates = false;
            this.schematicCheckBox29.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox29.TabIndex = 32;
            // 
            // schematicCheckBox30
            // 
            this.schematicCheckBox30.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox30.AutoSize = true;
            this.schematicCheckBox30.Checked = false;
            this.schematicCheckBox30.DigitalIO = null;
            this.schematicCheckBox30.DigitalIOName = "POEDegasTankLowLevelSensor";
            this.schematicCheckBox30.DisplayAsIndicator = false;
            this.schematicCheckBox30.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox30.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox30.Location = new System.Drawing.Point(589, 352);
            this.schematicCheckBox30.Name = "schematicCheckBox30";
            this.schematicCheckBox30.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox30.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox30.ReverseStates = false;
            this.schematicCheckBox30.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox30.TabIndex = 33;
            // 
            // schematicCheckBox31
            // 
            this.schematicCheckBox31.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox31.AutoSize = true;
            this.schematicCheckBox31.Checked = false;
            this.schematicCheckBox31.DigitalIO = null;
            this.schematicCheckBox31.DigitalIOName = "POEDegasTankHighLevelSensor";
            this.schematicCheckBox31.DisplayAsIndicator = false;
            this.schematicCheckBox31.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox31.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox31.Location = new System.Drawing.Point(589, 227);
            this.schematicCheckBox31.Name = "schematicCheckBox31";
            this.schematicCheckBox31.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox31.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox31.ReverseStates = false;
            this.schematicCheckBox31.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox31.TabIndex = 34;
            // 
            // schematicCheckBox32
            // 
            this.schematicCheckBox32.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox32.AutoSize = true;
            this.schematicCheckBox32.Checked = false;
            this.schematicCheckBox32.DigitalIO = null;
            this.schematicCheckBox32.DigitalIOName = "POEDegasTankTopLevelSensor";
            this.schematicCheckBox32.DisplayAsIndicator = false;
            this.schematicCheckBox32.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox32.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox32.Location = new System.Drawing.Point(589, 190);
            this.schematicCheckBox32.Name = "schematicCheckBox32";
            this.schematicCheckBox32.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox32.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox32.ReverseStates = false;
            this.schematicCheckBox32.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox32.TabIndex = 35;
            // 
            // schematicCheckBox33
            // 
            this.schematicCheckBox33.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox33.AutoSize = true;
            this.schematicCheckBox33.Checked = false;
            this.schematicCheckBox33.DigitalIO = null;
            this.schematicCheckBox33.DigitalIOName = "POEOilDrumSupplyValve";
            this.schematicCheckBox33.DisplayAsIndicator = false;
            this.schematicCheckBox33.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox33.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox33.Location = new System.Drawing.Point(705, 91);
            this.schematicCheckBox33.Name = "schematicCheckBox33";
            this.schematicCheckBox33.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox33.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox33.ReverseStates = false;
            this.schematicCheckBox33.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox33.TabIndex = 36;
            // 
            // schematicCheckBox34
            // 
            this.schematicCheckBox34.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox34.AutoSize = true;
            this.schematicCheckBox34.Checked = false;
            this.schematicCheckBox34.DigitalIO = null;
            this.schematicCheckBox34.DigitalIOName = "POEFeedTankFillValve";
            this.schematicCheckBox34.DisplayAsIndicator = false;
            this.schematicCheckBox34.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox34.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox34.Location = new System.Drawing.Point(758, 28);
            this.schematicCheckBox34.Name = "schematicCheckBox34";
            this.schematicCheckBox34.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox34.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox34.ReverseStates = false;
            this.schematicCheckBox34.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox34.TabIndex = 37;
            // 
            // schematicCheckBox35
            // 
            this.schematicCheckBox35.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox35.AutoSize = true;
            this.schematicCheckBox35.Checked = false;
            this.schematicCheckBox35.DigitalIO = null;
            this.schematicCheckBox35.DigitalIOName = "POEOilFilIsoValve";
            this.schematicCheckBox35.DisplayAsIndicator = false;
            this.schematicCheckBox35.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox35.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox35.Location = new System.Drawing.Point(825, 302);
            this.schematicCheckBox35.Name = "schematicCheckBox35";
            this.schematicCheckBox35.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox35.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox35.ReverseStates = false;
            this.schematicCheckBox35.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox35.TabIndex = 38;
            // 
            // schematicCheckBox36
            // 
            this.schematicCheckBox36.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox36.AutoSize = true;
            this.schematicCheckBox36.Checked = false;
            this.schematicCheckBox36.DigitalIO = null;
            this.schematicCheckBox36.DigitalIOName = "POEDegasRecircValve";
            this.schematicCheckBox36.DisplayAsIndicator = false;
            this.schematicCheckBox36.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox36.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox36.Location = new System.Drawing.Point(579, 91);
            this.schematicCheckBox36.Name = "schematicCheckBox36";
            this.schematicCheckBox36.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox36.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox36.ReverseStates = false;
            this.schematicCheckBox36.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox36.TabIndex = 39;
            // 
            // schematicCheckBox2
            // 
            this.schematicCheckBox2.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox2.AutoSize = true;
            this.schematicCheckBox2.Checked = false;
            this.schematicCheckBox2.DigitalIO = null;
            this.schematicCheckBox2.DigitalIOName = "PVEOilRecircPumpEnable";
            this.schematicCheckBox2.DisplayAsIndicator = false;
            this.schematicCheckBox2.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox2.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox2.Location = new System.Drawing.Point(154, 475);
            this.schematicCheckBox2.Name = "schematicCheckBox2";
            this.schematicCheckBox2.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox2.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox2.ReverseStates = false;
            this.schematicCheckBox2.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox2.TabIndex = 23;
            // 
            // schematicCheckBox1
            // 
            this.schematicCheckBox1.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox1.AutoSize = true;
            this.schematicCheckBox1.Checked = false;
            this.schematicCheckBox1.DigitalIO = null;
            this.schematicCheckBox1.DigitalIOName = "PVEVacuumPumpEnable";
            this.schematicCheckBox1.DisplayAsIndicator = false;
            this.schematicCheckBox1.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox1.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox1.Location = new System.Drawing.Point(21, 556);
            this.schematicCheckBox1.Name = "schematicCheckBox1";
            this.schematicCheckBox1.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox1.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox1.ReverseStates = false;
            this.schematicCheckBox1.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox1.TabIndex = 23;
            // 
            // schematicCheckBox8
            // 
            this.schematicCheckBox8.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox8.AutoSize = true;
            this.schematicCheckBox8.Checked = false;
            this.schematicCheckBox8.DigitalIO = null;
            this.schematicCheckBox8.DigitalIOName = "PVEDegasRateOfRiseValve";
            this.schematicCheckBox8.DisplayAsIndicator = false;
            this.schematicCheckBox8.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox8.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox8.Location = new System.Drawing.Point(51, 74);
            this.schematicCheckBox8.Name = "schematicCheckBox8";
            this.schematicCheckBox8.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox8.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox8.ReverseStates = false;
            this.schematicCheckBox8.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox8.TabIndex = 23;
            // 
            // schematicCheckBox9
            // 
            this.schematicCheckBox9.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox9.AutoSize = true;
            this.schematicCheckBox9.Checked = false;
            this.schematicCheckBox9.DigitalIO = null;
            this.schematicCheckBox9.DigitalIOName = "LightPVERed";
            this.schematicCheckBox9.DisplayAsIndicator = false;
            this.schematicCheckBox9.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox9.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox9.Location = new System.Drawing.Point(315, 459);
            this.schematicCheckBox9.Name = "schematicCheckBox9";
            this.schematicCheckBox9.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox9.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox9.ReverseStates = false;
            this.schematicCheckBox9.Size = new System.Drawing.Size(110, 40);
            this.schematicCheckBox9.TabIndex = 5;
            // 
            // schematicCheckBox7
            // 
            this.schematicCheckBox7.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox7.AutoSize = true;
            this.schematicCheckBox7.Checked = false;
            this.schematicCheckBox7.DigitalIO = null;
            this.schematicCheckBox7.DigitalIOName = "PVEDegasEvacuationValve";
            this.schematicCheckBox7.DisplayAsIndicator = false;
            this.schematicCheckBox7.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox7.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox7.Location = new System.Drawing.Point(99, 74);
            this.schematicCheckBox7.Name = "schematicCheckBox7";
            this.schematicCheckBox7.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox7.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox7.ReverseStates = false;
            this.schematicCheckBox7.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox7.TabIndex = 23;
            // 
            // schematicCheckBox5
            // 
            this.schematicCheckBox5.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox5.AutoSize = true;
            this.schematicCheckBox5.Checked = false;
            this.schematicCheckBox5.DigitalIO = null;
            this.schematicCheckBox5.DigitalIOName = "PVEDegasOutletValve";
            this.schematicCheckBox5.DisplayAsIndicator = false;
            this.schematicCheckBox5.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox5.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox5.Location = new System.Drawing.Point(154, 414);
            this.schematicCheckBox5.Name = "schematicCheckBox5";
            this.schematicCheckBox5.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox5.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox5.ReverseStates = false;
            this.schematicCheckBox5.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox5.TabIndex = 23;
            // 
            // schematicCheckBox19
            // 
            this.schematicCheckBox19.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox19.AutoSize = true;
            this.schematicCheckBox19.Checked = false;
            this.schematicCheckBox19.DigitalIO = null;
            this.schematicCheckBox19.DigitalIOName = "PVEVacuumPumpOilLevelOK";
            this.schematicCheckBox19.DisplayAsIndicator = false;
            this.schematicCheckBox19.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox19.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox19.Location = new System.Drawing.Point(109, 493);
            this.schematicCheckBox19.Name = "schematicCheckBox19";
            this.schematicCheckBox19.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox19.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox19.ReverseStates = false;
            this.schematicCheckBox19.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox19.TabIndex = 23;
            // 
            // schematicCheckBox18
            // 
            this.schematicCheckBox18.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox18.AutoSize = true;
            this.schematicCheckBox18.Checked = false;
            this.schematicCheckBox18.DigitalIO = null;
            this.schematicCheckBox18.DigitalIOName = "PVEDegasTankLowLevelSensor";
            this.schematicCheckBox18.DisplayAsIndicator = false;
            this.schematicCheckBox18.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox18.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox18.Location = new System.Drawing.Point(109, 352);
            this.schematicCheckBox18.Name = "schematicCheckBox18";
            this.schematicCheckBox18.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox18.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox18.ReverseStates = false;
            this.schematicCheckBox18.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox18.TabIndex = 23;
            // 
            // schematicCheckBox20
            // 
            this.schematicCheckBox20.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox20.AutoSize = true;
            this.schematicCheckBox20.Checked = false;
            this.schematicCheckBox20.DigitalIO = null;
            this.schematicCheckBox20.DigitalIOName = "PVEDegasTankHighLevelSensor";
            this.schematicCheckBox20.DisplayAsIndicator = false;
            this.schematicCheckBox20.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox20.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox20.Location = new System.Drawing.Point(109, 227);
            this.schematicCheckBox20.Name = "schematicCheckBox20";
            this.schematicCheckBox20.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox20.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox20.ReverseStates = false;
            this.schematicCheckBox20.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox20.TabIndex = 23;
            // 
            // schematicCheckBox17
            // 
            this.schematicCheckBox17.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox17.AutoSize = true;
            this.schematicCheckBox17.Checked = false;
            this.schematicCheckBox17.DigitalIO = null;
            this.schematicCheckBox17.DigitalIOName = "PVEDegasTankTopLevelSensor";
            this.schematicCheckBox17.DisplayAsIndicator = false;
            this.schematicCheckBox17.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox17.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox17.Location = new System.Drawing.Point(109, 190);
            this.schematicCheckBox17.Name = "schematicCheckBox17";
            this.schematicCheckBox17.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox17.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox17.ReverseStates = false;
            this.schematicCheckBox17.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox17.TabIndex = 23;
            // 
            // schematicCheckBox4
            // 
            this.schematicCheckBox4.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox4.AutoSize = true;
            this.schematicCheckBox4.Checked = false;
            this.schematicCheckBox4.DigitalIO = null;
            this.schematicCheckBox4.DigitalIOName = "PVEOilDrumSupplyValve";
            this.schematicCheckBox4.DisplayAsIndicator = false;
            this.schematicCheckBox4.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox4.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox4.Location = new System.Drawing.Point(252, 91);
            this.schematicCheckBox4.Name = "schematicCheckBox4";
            this.schematicCheckBox4.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox4.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox4.ReverseStates = false;
            this.schematicCheckBox4.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox4.TabIndex = 23;
            // 
            // schematicCheckBox15
            // 
            this.schematicCheckBox15.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox15.AutoSize = true;
            this.schematicCheckBox15.Checked = false;
            this.schematicCheckBox15.DigitalIO = null;
            this.schematicCheckBox15.DigitalIOName = "PVEFeedTankFillValve";
            this.schematicCheckBox15.DisplayAsIndicator = false;
            this.schematicCheckBox15.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox15.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox15.Location = new System.Drawing.Point(298, 28);
            this.schematicCheckBox15.Name = "schematicCheckBox15";
            this.schematicCheckBox15.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox15.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox15.ReverseStates = false;
            this.schematicCheckBox15.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox15.TabIndex = 23;
            // 
            // schematicCheckBox3
            // 
            this.schematicCheckBox3.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox3.AutoSize = true;
            this.schematicCheckBox3.Checked = false;
            this.schematicCheckBox3.DigitalIO = null;
            this.schematicCheckBox3.DigitalIOName = "PVEOilFilIsoValve";
            this.schematicCheckBox3.DisplayAsIndicator = false;
            this.schematicCheckBox3.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox3.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox3.Location = new System.Drawing.Point(372, 302);
            this.schematicCheckBox3.Name = "schematicCheckBox3";
            this.schematicCheckBox3.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox3.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox3.ReverseStates = false;
            this.schematicCheckBox3.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox3.TabIndex = 23;
            // 
            // schematicCheckBox16
            // 
            this.schematicCheckBox16.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox16.AutoSize = true;
            this.schematicCheckBox16.Checked = false;
            this.schematicCheckBox16.DigitalIO = null;
            this.schematicCheckBox16.DigitalIOName = "PVEDegasRecircValve";
            this.schematicCheckBox16.DisplayAsIndicator = false;
            this.schematicCheckBox16.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox16.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox16.Location = new System.Drawing.Point(127, 91);
            this.schematicCheckBox16.Name = "schematicCheckBox16";
            this.schematicCheckBox16.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox16.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox16.ReverseStates = false;
            this.schematicCheckBox16.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox16.TabIndex = 23;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // schematicCheckBox37
            // 
            this.schematicCheckBox37.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox37.AutoSize = true;
            this.schematicCheckBox37.Checked = false;
            this.schematicCheckBox37.DigitalIO = null;
            this.schematicCheckBox37.DigitalIOName = "PVEFlowMeter";
            this.schematicCheckBox37.DisplayAsIndicator = false;
            this.schematicCheckBox37.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox37.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox37.Location = new System.Drawing.Point(342, 55);
            this.schematicCheckBox37.Name = "schematicCheckBox37";
            this.schematicCheckBox37.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox37.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox37.ReverseStates = false;
            this.schematicCheckBox37.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox37.TabIndex = 23;
            // 
            // schematicCheckBox38
            // 
            this.schematicCheckBox38.Appearance = System.Windows.Forms.Appearance.Button;
            this.schematicCheckBox38.AutoSize = true;
            this.schematicCheckBox38.Checked = false;
            this.schematicCheckBox38.DigitalIO = null;
            this.schematicCheckBox38.DigitalIOName = "POEFlowMeter";
            this.schematicCheckBox38.DisplayAsIndicator = false;
            this.schematicCheckBox38.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.schematicCheckBox38.IndicatorColor = System.Drawing.Color.Yellow;
            this.schematicCheckBox38.Location = new System.Drawing.Point(792, 55);
            this.schematicCheckBox38.Name = "schematicCheckBox38";
            this.schematicCheckBox38.OriginalLocation = new System.Drawing.Point(0, 0);
            this.schematicCheckBox38.OriginalSize = new System.Drawing.Size(0, 0);
            this.schematicCheckBox38.ReverseStates = false;
            this.schematicCheckBox38.Size = new System.Drawing.Size(22, 22);
            this.schematicCheckBox38.TabIndex = 23;
            // 
            // SchematicFormDual
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 620);
            this.Controls.Add(this.schematicPanelMain);
            this.Controls.Add(this.schematicPanelLeft);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SchematicFormDual";
            this.Text = "Plumbing Schematic";
            this.VisibleChanged += new System.EventHandler(this.SchematicForm_VisibleChanged);
            this.schematicPanelLeft.ResumeLayout(false);
            this.schematicPanelLeft.PerformLayout();
            this.schematicPanelMain.ResumeLayout(false);
            this.schematicPanelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private VTIWindowsControlLibrary.Components.SchematicPanel schematicPanelLeft;
        private System.Windows.Forms.Button buttonClose;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox6;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox11;

        private VTIWindowsControlLibrary.Components.SchematicPanel schematicPanelMain;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox12;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox13;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox16;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox1;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox2;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox3;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox4;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox5;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox8;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox7;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox9;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox10;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox14;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox15;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox18;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox17;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox19;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox20;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox21;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox22;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox23;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox24;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox25;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox26;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox27;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox28;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox29;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox30;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox31;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox32;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox33;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox34;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox35;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox36;
        private System.Windows.Forms.Timer timer1;
        private VTIWindowsControlLibrary.Components.SchematicLabel schlblP3PVEEvac;
        private VTIWindowsControlLibrary.Components.SchematicLabel schlblP6POEEvac;
        private VTIWindowsControlLibrary.Components.SchematicLabel schlblP2PVEPress;
        private VTIWindowsControlLibrary.Components.SchematicLabel schlblP5POEPress;
        private VTIWindowsControlLibrary.Components.SchematicLabel POEScale;
        private VTIWindowsControlLibrary.Components.SchematicLabel PVEScale;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.Button btnStart2;
        private System.Windows.Forms.TextBox txtStart2;
        private System.Windows.Forms.Button btnStop2;
        private System.Windows.Forms.TextBox txtStop2;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox37;
        private VTIWindowsControlLibrary.Components.SchematicCheckBox schematicCheckBox38;
    }
}