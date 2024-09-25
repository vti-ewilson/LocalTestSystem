namespace VTIWindowsControlLibrary.Forms
{
  partial class SRGSetupForm
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

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SRGSetupForm));
            this.textBoxSamplingInterval = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.labelSamplingInterval = new System.Windows.Forms.Label();
            this.labelGasTemperature = new System.Windows.Forms.Label();
            this.labelGasType = new System.Windows.Forms.Label();
            this.labelAccomodationFactor = new System.Windows.Forms.Label();
            this.labelSignalStrength = new System.Windows.Forms.Label();
            this.labelSignalDampening = new System.Windows.Forms.Label();
            this.labelRotorSpeed = new System.Windows.Forms.Label();
            this.textBoxGasTemperature = new System.Windows.Forms.TextBox();
            this.textBoxAccomodationFactor = new System.Windows.Forms.TextBox();
            this.textBoxSignalStrength = new System.Windows.Forms.TextBox();
            this.textBoxSignalDampening = new System.Windows.Forms.TextBox();
            this.textBoxRotorSpeed = new System.Windows.Forms.TextBox();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.btnStartSRG = new System.Windows.Forms.Button();
            this.btnStopSRG = new System.Windows.Forms.Button();
            this.btnRTLSRG = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.textBoxManualCommand = new System.Windows.Forms.TextBox();
            this.groupBoxManualCommand = new System.Windows.Forms.GroupBox();
            this.btnDisarmed = new System.Windows.Forms.Button();
            this.btnDismount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxResponse = new System.Windows.Forms.TextBox();
            this.labelSeeInterfaceManual = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSend = new System.Windows.Forms.Button();
            this.comboBoxGasType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdPressureUnits = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.ledLabel7 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel6 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel5 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel4 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel2 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel0 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabel1 = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ledLabelDecel = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabelSensorUnstable = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabelDrive = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.ledLabelAccel = new VTIWindowsControlLibrary.Components.LEDLabel();
            this.lblRotorStatus = new System.Windows.Forms.Label();
            this.tmrRotorStatusRefresh = new System.Windows.Forms.Timer(this.components);
            this.groupBoxManualCommand.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // textBoxSamplingInterval
            // 
            this.textBoxSamplingInterval.Location = new System.Drawing.Point(126, 14);
            this.textBoxSamplingInterval.Name = "textBoxSamplingInterval";
            this.textBoxSamplingInterval.Size = new System.Drawing.Size(72, 20);
            this.textBoxSamplingInterval.TabIndex = 5;
            this.textBoxSamplingInterval.TextChanged += new System.EventHandler(this.txtBoxSamplingInterval_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(56, 165);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 26);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Apply Change(s)";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // labelSamplingInterval
            // 
            this.labelSamplingInterval.AutoSize = true;
            this.labelSamplingInterval.Location = new System.Drawing.Point(6, 17);
            this.labelSamplingInterval.Name = "labelSamplingInterval";
            this.labelSamplingInterval.Size = new System.Drawing.Size(114, 13);
            this.labelSamplingInterval.TabIndex = 8;
            this.labelSamplingInterval.Text = "Sampling Interval (sec)";
            // 
            // labelGasTemperature
            // 
            this.labelGasTemperature.AutoSize = true;
            this.labelGasTemperature.Location = new System.Drawing.Point(14, 202);
            this.labelGasTemperature.Name = "labelGasTemperature";
            this.labelGasTemperature.Size = new System.Drawing.Size(105, 13);
            this.labelGasTemperature.TabIndex = 9;
            this.labelGasTemperature.Text = "Gas Temperature (K)";
            this.labelGasTemperature.Visible = false;
            // 
            // labelGasType
            // 
            this.labelGasType.AutoSize = true;
            this.labelGasType.Location = new System.Drawing.Point(6, 133);
            this.labelGasType.Name = "labelGasType";
            this.labelGasType.Size = new System.Drawing.Size(53, 13);
            this.labelGasType.TabIndex = 10;
            this.labelGasType.Text = "Gas Type";
            // 
            // labelAccomodationFactor
            // 
            this.labelAccomodationFactor.AutoSize = true;
            this.labelAccomodationFactor.Location = new System.Drawing.Point(20, 206);
            this.labelAccomodationFactor.Name = "labelAccomodationFactor";
            this.labelAccomodationFactor.Size = new System.Drawing.Size(108, 13);
            this.labelAccomodationFactor.TabIndex = 11;
            this.labelAccomodationFactor.Text = "Accomodation Factor";
            this.labelAccomodationFactor.Visible = false;
            // 
            // labelSignalStrength
            // 
            this.labelSignalStrength.AutoSize = true;
            this.labelSignalStrength.Location = new System.Drawing.Point(6, 18);
            this.labelSignalStrength.Name = "labelSignalStrength";
            this.labelSignalStrength.Size = new System.Drawing.Size(111, 13);
            this.labelSignalStrength.TabIndex = 12;
            this.labelSignalStrength.Text = "Signal Strength (Volts)";
            // 
            // labelSignalDampening
            // 
            this.labelSignalDampening.AutoSize = true;
            this.labelSignalDampening.Location = new System.Drawing.Point(6, 40);
            this.labelSignalDampening.Name = "labelSignalDampening";
            this.labelSignalDampening.Size = new System.Drawing.Size(93, 13);
            this.labelSignalDampening.TabIndex = 13;
            this.labelSignalDampening.Text = "Signal Dampening";
            // 
            // labelRotorSpeed
            // 
            this.labelRotorSpeed.AutoSize = true;
            this.labelRotorSpeed.Location = new System.Drawing.Point(6, 61);
            this.labelRotorSpeed.Name = "labelRotorSpeed";
            this.labelRotorSpeed.Size = new System.Drawing.Size(89, 13);
            this.labelRotorSpeed.TabIndex = 14;
            this.labelRotorSpeed.Text = "Rotor Speed (Hz)";
            // 
            // textBoxGasTemperature
            // 
            this.textBoxGasTemperature.Location = new System.Drawing.Point(68, 199);
            this.textBoxGasTemperature.Name = "textBoxGasTemperature";
            this.textBoxGasTemperature.Size = new System.Drawing.Size(72, 20);
            this.textBoxGasTemperature.TabIndex = 15;
            this.textBoxGasTemperature.Visible = false;
            this.textBoxGasTemperature.TextChanged += new System.EventHandler(this.textBoxGasTemperature_TextChanged);
            // 
            // textBoxAccomodationFactor
            // 
            this.textBoxAccomodationFactor.Location = new System.Drawing.Point(36, 204);
            this.textBoxAccomodationFactor.Name = "textBoxAccomodationFactor";
            this.textBoxAccomodationFactor.Size = new System.Drawing.Size(72, 20);
            this.textBoxAccomodationFactor.TabIndex = 17;
            this.textBoxAccomodationFactor.Visible = false;
            this.textBoxAccomodationFactor.TextChanged += new System.EventHandler(this.textBoxAccomodationFactor_TextChanged);
            // 
            // textBoxSignalStrength
            // 
            this.textBoxSignalStrength.Location = new System.Drawing.Point(123, 15);
            this.textBoxSignalStrength.Name = "textBoxSignalStrength";
            this.textBoxSignalStrength.ReadOnly = true;
            this.textBoxSignalStrength.Size = new System.Drawing.Size(72, 20);
            this.textBoxSignalStrength.TabIndex = 18;
            // 
            // textBoxSignalDampening
            // 
            this.textBoxSignalDampening.Location = new System.Drawing.Point(123, 37);
            this.textBoxSignalDampening.Name = "textBoxSignalDampening";
            this.textBoxSignalDampening.ReadOnly = true;
            this.textBoxSignalDampening.Size = new System.Drawing.Size(72, 20);
            this.textBoxSignalDampening.TabIndex = 19;
            // 
            // textBoxRotorSpeed
            // 
            this.textBoxRotorSpeed.Location = new System.Drawing.Point(123, 60);
            this.textBoxRotorSpeed.Name = "textBoxRotorSpeed";
            this.textBoxRotorSpeed.ReadOnly = true;
            this.textBoxRotorSpeed.Size = new System.Drawing.Size(72, 20);
            this.textBoxRotorSpeed.TabIndex = 20;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRefresh.Location = new System.Drawing.Point(55, 230);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(118, 46);
            this.btnRefresh.TabIndex = 21;
            this.btnRefresh.Text = "Refresh All Values";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // btnStartSRG
            // 
            this.btnStartSRG.Location = new System.Drawing.Point(6, 106);
            this.btnStartSRG.Name = "btnStartSRG";
            this.btnStartSRG.Size = new System.Drawing.Size(64, 20);
            this.btnStartSRG.TabIndex = 22;
            this.btnStartSRG.Text = "Start SRG";
            this.btnStartSRG.UseVisualStyleBackColor = true;
            this.btnStartSRG.Click += new System.EventHandler(this.btnStartSRG_Click);
            // 
            // btnStopSRG
            // 
            this.btnStopSRG.Location = new System.Drawing.Point(95, 106);
            this.btnStopSRG.Name = "btnStopSRG";
            this.btnStopSRG.Size = new System.Drawing.Size(64, 20);
            this.btnStopSRG.TabIndex = 23;
            this.btnStopSRG.Text = "Stop SRG";
            this.btnStopSRG.UseVisualStyleBackColor = true;
            this.btnStopSRG.Click += new System.EventHandler(this.btnStopSRG_Click);
            // 
            // btnRTLSRG
            // 
            this.btnRTLSRG.Location = new System.Drawing.Point(486, 230);
            this.btnRTLSRG.Name = "btnRTLSRG";
            this.btnRTLSRG.Size = new System.Drawing.Size(151, 26);
            this.btnRTLSRG.TabIndex = 24;
            this.btnRTLSRG.Text = "Return Local Control (RTL)";
            this.btnRTLSRG.UseVisualStyleBackColor = true;
            this.btnRTLSRG.Click += new System.EventHandler(this.btnRTLSRG_Click);
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(573, 283);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(64, 27);
            this.btnClose.TabIndex = 25;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // textBoxManualCommand
            // 
            this.textBoxManualCommand.Location = new System.Drawing.Point(5, 32);
            this.textBoxManualCommand.Name = "textBoxManualCommand";
            this.textBoxManualCommand.Size = new System.Drawing.Size(112, 20);
            this.textBoxManualCommand.TabIndex = 26;
            // 
            // groupBoxManualCommand
            // 
            this.groupBoxManualCommand.Controls.Add(this.btnDisarmed);
            this.groupBoxManualCommand.Controls.Add(this.btnDismount);
            this.groupBoxManualCommand.Controls.Add(this.label1);
            this.groupBoxManualCommand.Controls.Add(this.textBoxResponse);
            this.groupBoxManualCommand.Controls.Add(this.textBoxManualCommand);
            this.groupBoxManualCommand.Controls.Add(this.labelSeeInterfaceManual);
            this.groupBoxManualCommand.Controls.Add(this.button1);
            this.groupBoxManualCommand.Controls.Add(this.btnStopSRG);
            this.groupBoxManualCommand.Controls.Add(this.btnStartSRG);
            this.groupBoxManualCommand.Controls.Add(this.btnSend);
            this.groupBoxManualCommand.Location = new System.Drawing.Point(452, 12);
            this.groupBoxManualCommand.Name = "groupBoxManualCommand";
            this.groupBoxManualCommand.Size = new System.Drawing.Size(201, 207);
            this.groupBoxManualCommand.TabIndex = 27;
            this.groupBoxManualCommand.TabStop = false;
            this.groupBoxManualCommand.Text = "Manual Command";
            // 
            // btnDisarmed
            // 
            this.btnDisarmed.Location = new System.Drawing.Point(95, 159);
            this.btnDisarmed.Name = "btnDisarmed";
            this.btnDisarmed.Size = new System.Drawing.Size(64, 20);
            this.btnDisarmed.TabIndex = 32;
            this.btnDisarmed.Text = "Disarmed";
            this.btnDisarmed.UseVisualStyleBackColor = true;
            this.btnDisarmed.Click += new System.EventHandler(this.btnDisarmed_Click);
            // 
            // btnDismount
            // 
            this.btnDismount.Location = new System.Drawing.Point(95, 132);
            this.btnDismount.Name = "btnDismount";
            this.btnDismount.Size = new System.Drawing.Size(64, 20);
            this.btnDismount.TabIndex = 31;
            this.btnDismount.Text = "Dismount";
            this.btnDismount.UseVisualStyleBackColor = true;
            this.btnDismount.Click += new System.EventHandler(this.btnDismount_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Command Response";
            // 
            // textBoxResponse
            // 
            this.textBoxResponse.Location = new System.Drawing.Point(5, 71);
            this.textBoxResponse.Multiline = true;
            this.textBoxResponse.Name = "textBoxResponse";
            this.textBoxResponse.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxResponse.Size = new System.Drawing.Size(178, 30);
            this.textBoxResponse.TabIndex = 26;
            // 
            // labelSeeInterfaceManual
            // 
            this.labelSeeInterfaceManual.AutoSize = true;
            this.labelSeeInterfaceManual.Location = new System.Drawing.Point(3, 16);
            this.labelSeeInterfaceManual.Name = "labelSeeInterfaceManual";
            this.labelSeeInterfaceManual.Size = new System.Drawing.Size(182, 13);
            this.labelSeeInterfaceManual.TabIndex = 28;
            this.labelSeeInterfaceManual.Text = "For Commands See Interface Manual";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(11, 178);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(64, 20);
            this.button1.TabIndex = 23;
            this.button1.Text = "Clear Error";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnClearError_Click);
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(119, 32);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(64, 20);
            this.btnSend.TabIndex = 29;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // comboBoxGasType
            // 
            this.comboBoxGasType.FormattingEnabled = true;
            this.comboBoxGasType.Items.AddRange(new object[] {
            "  Select a gas",
            "    1 user-definable (Usr1)",
            "    2 user-definable (Usr2)",
            "    3 user-definable (Usr3)",
            "    4 user-definable (Usr4)",
            "    5 user-definable (Usr5)",
            "    6 user-definable (Usr6)",
            "    7 user-definable (Usr7)",
            "    8 user-definable (Usr8)",
            "    9 Air (Air)",
            "    10 Argon (Ar)",
            "    11 Acethylene (C2H2)",
            "    12 Freon-14 (CF4)",
            "    13 Methane (CH4)",
            "    14 Carbon dioxide (CO2)",
            "    15 Deuterium (D2)",
            "    16 Hydrogen (H2)",
            "    17 Helium (He)",
            "    18 Hydrogen fluoride (HF)",
            "    19 Nitrogen (N2)",
            "    20 Nitrous oxide (N2O)",
            "    21 Neon (Ne)",
            "    22 Oxygen (O2)",
            "    23 Sulfur dioxide (SO2)",
            "    24 Sulfur hexafluoride (SF6)",
            "    25 Xenon (Xe)</remarks>"});
            this.comboBoxGasType.Location = new System.Drawing.Point(60, 131);
            this.comboBoxGasType.Name = "comboBoxGasType";
            this.comboBoxGasType.Size = new System.Drawing.Size(138, 21);
            this.comboBoxGasType.TabIndex = 32;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.labelSamplingInterval);
            this.groupBox1.Controls.Add(this.labelGasType);
            this.groupBox1.Controls.Add(this.textBoxSamplingInterval);
            this.groupBox1.Controls.Add(this.btnOK);
            this.groupBox1.Controls.Add(this.comboBoxGasType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(8, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(217, 208);
            this.groupBox1.TabIndex = 33;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SRG Setup";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 44);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(139, 65);
            this.label3.TabIndex = 28;
            this.label3.Text = "Sample interval is based\r\non leak size and pressure\r\nLower SI for higher pressure" +
    "\r\nRaise SI for lower pressure\r\n5-20 second typical";
            // 
            // cmdPressureUnits
            // 
            this.cmdPressureUnits.FormattingEnabled = true;
            this.cmdPressureUnits.Items.AddRange(new object[] {
            "0 = Deceleration Rate",
            "1 = Pascals",
            "2 = millibar",
            "3 = Torr"});
            this.cmdPressureUnits.Location = new System.Drawing.Point(101, 204);
            this.cmdPressureUnits.Name = "cmdPressureUnits";
            this.cmdPressureUnits.Size = new System.Drawing.Size(111, 21);
            this.cmdPressureUnits.TabIndex = 34;
            this.cmdPressureUnits.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 207);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(75, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Pressure Units";
            this.label2.Visible = false;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.labelSignalStrength);
            this.groupBox2.Controls.Add(this.labelRotorSpeed);
            this.groupBox2.Controls.Add(this.textBoxSignalStrength);
            this.groupBox2.Controls.Add(this.textBoxSignalDampening);
            this.groupBox2.Controls.Add(this.labelSignalDampening);
            this.groupBox2.Controls.Add(this.textBoxRotorSpeed);
            this.groupBox2.Location = new System.Drawing.Point(231, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(215, 100);
            this.groupBox2.TabIndex = 34;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "SRG Status";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.ledLabel7);
            this.groupBox3.Controls.Add(this.ledLabel6);
            this.groupBox3.Controls.Add(this.ledLabel5);
            this.groupBox3.Controls.Add(this.ledLabel4);
            this.groupBox3.Controls.Add(this.ledLabel2);
            this.groupBox3.Controls.Add(this.ledLabel0);
            this.groupBox3.Controls.Add(this.ledLabel1);
            this.groupBox3.Location = new System.Drawing.Point(231, 113);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(215, 106);
            this.groupBox3.TabIndex = 36;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "System Status Register (STS)";
            // 
            // ledLabel7
            // 
            this.ledLabel7.AutoSize = true;
            this.ledLabel7.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel7.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel7.LEDBorderWidth = 0;
            this.ledLabel7.LEDColor = System.Drawing.Color.Red;
            this.ledLabel7.LEDSize = 10;
            this.ledLabel7.Lit = false;
            this.ledLabel7.Location = new System.Drawing.Point(87, 56);
            this.ledLabel7.Name = "ledLabel7";
            this.ledLabel7.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel7.Size = new System.Drawing.Size(90, 22);
            this.ledLabel7.TabIndex = 35;
            this.ledLabel7.Text = "Power failure";
            // 
            // ledLabel6
            // 
            this.ledLabel6.AutoSize = true;
            this.ledLabel6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel6.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel6.LEDBorderWidth = 0;
            this.ledLabel6.LEDColor = System.Drawing.Color.Red;
            this.ledLabel6.LEDSize = 10;
            this.ledLabel6.Lit = false;
            this.ledLabel6.Location = new System.Drawing.Point(9, 77);
            this.ledLabel6.Name = "ledLabel6";
            this.ledLabel6.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel6.Size = new System.Drawing.Size(155, 22);
            this.ledLabel6.TabIndex = 35;
            this.ledLabel6.Text = "Backup fail/Setup defaults";
            // 
            // ledLabel5
            // 
            this.ledLabel5.AutoSize = true;
            this.ledLabel5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel5.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel5.LEDBorderWidth = 0;
            this.ledLabel5.LEDColor = System.Drawing.Color.Red;
            this.ledLabel5.LEDSize = 10;
            this.ledLabel5.Lit = false;
            this.ledLabel5.Location = new System.Drawing.Point(87, 37);
            this.ledLabel5.Name = "ledLabel5";
            this.ledLabel5.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel5.Size = new System.Drawing.Size(137, 22);
            this.ledLabel5.TabIndex = 35;
            this.ledLabel5.Text = "Message (See Display)";
            // 
            // ledLabel4
            // 
            this.ledLabel4.AutoSize = true;
            this.ledLabel4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel4.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel4.LEDBorderWidth = 0;
            this.ledLabel4.LEDColor = System.Drawing.Color.Red;
            this.ledLabel4.LEDSize = 10;
            this.ledLabel4.Lit = false;
            this.ledLabel4.Location = new System.Drawing.Point(87, 19);
            this.ledLabel4.Name = "ledLabel4";
            this.ledLabel4.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel4.Size = new System.Drawing.Size(98, 22);
            this.ledLabel4.TabIndex = 35;
            this.ledLabel4.Text = "Data Available";
            // 
            // ledLabel2
            // 
            this.ledLabel2.AutoSize = true;
            this.ledLabel2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel2.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel2.LEDBorderWidth = 0;
            this.ledLabel2.LEDColor = System.Drawing.Color.Red;
            this.ledLabel2.LEDSize = 10;
            this.ledLabel2.Lit = false;
            this.ledLabel2.Location = new System.Drawing.Point(9, 56);
            this.ledLabel2.Name = "ledLabel2";
            this.ledLabel2.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel2.Size = new System.Drawing.Size(60, 22);
            this.ledLabel2.TabIndex = 35;
            this.ledLabel2.Text = "Ready";
            // 
            // ledLabel0
            // 
            this.ledLabel0.AutoSize = true;
            this.ledLabel0.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel0.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel0.LEDBorderWidth = 0;
            this.ledLabel0.LEDColor = System.Drawing.Color.Red;
            this.ledLabel0.LEDSize = 10;
            this.ledLabel0.Lit = false;
            this.ledLabel0.Location = new System.Drawing.Point(9, 17);
            this.ledLabel0.Name = "ledLabel0";
            this.ledLabel0.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel0.Size = new System.Drawing.Size(81, 22);
            this.ledLabel0.TabIndex = 35;
            this.ledLabel0.Text = "Set Point 1";
            // 
            // ledLabel1
            // 
            this.ledLabel1.AutoSize = true;
            this.ledLabel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabel1.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabel1.LEDBorderWidth = 0;
            this.ledLabel1.LEDColor = System.Drawing.Color.Red;
            this.ledLabel1.LEDSize = 10;
            this.ledLabel1.Lit = false;
            this.ledLabel1.Location = new System.Drawing.Point(9, 36);
            this.ledLabel1.Name = "ledLabel1";
            this.ledLabel1.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabel1.Size = new System.Drawing.Size(81, 22);
            this.ledLabel1.TabIndex = 35;
            this.ledLabel1.Text = "Set Point 2";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ledLabelDecel);
            this.groupBox4.Controls.Add(this.ledLabelSensorUnstable);
            this.groupBox4.Controls.Add(this.ledLabelDrive);
            this.groupBox4.Controls.Add(this.ledLabelAccel);
            this.groupBox4.Controls.Add(this.lblRotorStatus);
            this.groupBox4.Location = new System.Drawing.Point(231, 222);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(215, 94);
            this.groupBox4.TabIndex = 37;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Rotor Status (RCS)";
            // 
            // ledLabelDecel
            // 
            this.ledLabelDecel.AutoSize = true;
            this.ledLabelDecel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabelDecel.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabelDecel.LEDBorderWidth = 0;
            this.ledLabelDecel.LEDColor = System.Drawing.Color.Red;
            this.ledLabelDecel.LEDSize = 10;
            this.ledLabelDecel.Lit = false;
            this.ledLabelDecel.Location = new System.Drawing.Point(107, 35);
            this.ledLabelDecel.Name = "ledLabelDecel";
            this.ledLabelDecel.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabelDecel.Size = new System.Drawing.Size(89, 22);
            this.ledLabelDecel.TabIndex = 39;
            this.ledLabelDecel.Text = "Decelerating";
            // 
            // ledLabelSensorUnstable
            // 
            this.ledLabelSensorUnstable.AutoSize = true;
            this.ledLabelSensorUnstable.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabelSensorUnstable.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabelSensorUnstable.LEDBorderWidth = 0;
            this.ledLabelSensorUnstable.LEDColor = System.Drawing.Color.Red;
            this.ledLabelSensorUnstable.LEDSize = 10;
            this.ledLabelSensorUnstable.Lit = false;
            this.ledLabelSensorUnstable.Location = new System.Drawing.Point(16, 71);
            this.ledLabelSensorUnstable.Name = "ledLabelSensorUnstable";
            this.ledLabelSensorUnstable.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabelSensorUnstable.Size = new System.Drawing.Size(107, 22);
            this.ledLabelSensorUnstable.TabIndex = 36;
            this.ledLabelSensorUnstable.Text = "Sensor Unstable";
            // 
            // ledLabelDrive
            // 
            this.ledLabelDrive.AutoSize = true;
            this.ledLabelDrive.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabelDrive.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabelDrive.LEDBorderWidth = 0;
            this.ledLabelDrive.LEDColor = System.Drawing.Color.Red;
            this.ledLabelDrive.LEDSize = 10;
            this.ledLabelDrive.Lit = false;
            this.ledLabelDrive.Location = new System.Drawing.Point(16, 53);
            this.ledLabelDrive.Name = "ledLabelDrive";
            this.ledLabelDrive.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabelDrive.Size = new System.Drawing.Size(103, 22);
            this.ledLabelDrive.TabIndex = 37;
            this.ledLabelDrive.Text = "Drive Operating";
            // 
            // ledLabelAccel
            // 
            this.ledLabelAccel.AutoSize = true;
            this.ledLabelAccel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ledLabelAccel.LEDBorderColor = System.Drawing.Color.Empty;
            this.ledLabelAccel.LEDBorderWidth = 0;
            this.ledLabelAccel.LEDColor = System.Drawing.Color.Red;
            this.ledLabelAccel.LEDSize = 10;
            this.ledLabelAccel.Lit = false;
            this.ledLabelAccel.Location = new System.Drawing.Point(16, 35);
            this.ledLabelAccel.Name = "ledLabelAccel";
            this.ledLabelAccel.Padding = new System.Windows.Forms.Padding(2, 2, 2, 4);
            this.ledLabelAccel.Size = new System.Drawing.Size(88, 22);
            this.ledLabelAccel.TabIndex = 38;
            this.ledLabelAccel.Text = "Accelerating";
            // 
            // lblRotorStatus
            // 
            this.lblRotorStatus.AutoSize = true;
            this.lblRotorStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblRotorStatus.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.lblRotorStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRotorStatus.ForeColor = System.Drawing.Color.ForestGreen;
            this.lblRotorStatus.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.lblRotorStatus.Location = new System.Drawing.Point(13, 16);
            this.lblRotorStatus.Name = "lblRotorStatus";
            this.lblRotorStatus.Size = new System.Drawing.Size(190, 18);
            this.lblRotorStatus.TabIndex = 0;
            this.lblRotorStatus.Text = "Waiting for Status of Rotor";
            // 
            // tmrRotorStatusRefresh
            // 
            this.tmrRotorStatusRefresh.Enabled = true;
            this.tmrRotorStatusRefresh.Interval = 500;
            this.tmrRotorStatusRefresh.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // SRGSetupForm
            // 
            this.AcceptButton = this.btnOK;
            this.ClientSize = new System.Drawing.Size(667, 392);
            this.ControlBox = false;
            this.Controls.Add(this.cmdPressureUnits);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.textBoxGasTemperature);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.textBoxAccomodationFactor);
            this.Controls.Add(this.labelGasTemperature);
            this.Controls.Add(this.groupBoxManualCommand);
            this.Controls.Add(this.labelAccomodationFactor);
            this.Controls.Add(this.btnRTLSRG);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnRefresh);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SRGSetupForm";
            this.Text = "SRG Setup";
            this.Load += new System.EventHandler(this.SRGSetupForm_Load);
            this.groupBoxManualCommand.ResumeLayout(false);
            this.groupBoxManualCommand.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

    }

    #endregion

    public System.Windows.Forms.TextBox textBoxSamplingInterval;
    private System.Windows.Forms.Button btnOK;
    private System.Windows.Forms.Label labelSamplingInterval;
    private System.Windows.Forms.Label labelGasTemperature;
    private System.Windows.Forms.Label labelGasType;
    private System.Windows.Forms.Label labelAccomodationFactor;
    private System.Windows.Forms.Label labelSignalStrength;
    private System.Windows.Forms.Label labelSignalDampening;
    private System.Windows.Forms.Label labelRotorSpeed;
    public System.Windows.Forms.TextBox textBoxGasTemperature;
    public System.Windows.Forms.TextBox textBoxAccomodationFactor;
    public System.Windows.Forms.TextBox textBoxSignalStrength;
    public System.Windows.Forms.TextBox textBoxSignalDampening;
    public System.Windows.Forms.TextBox textBoxRotorSpeed;
    private System.Windows.Forms.Button btnRefresh;
    private System.Windows.Forms.Button btnStartSRG;
    private System.Windows.Forms.Button btnStopSRG;
    private System.Windows.Forms.Button btnRTLSRG;
    private System.Windows.Forms.Button btnClose;
    public System.Windows.Forms.TextBox textBoxManualCommand;
    private System.Windows.Forms.GroupBox groupBoxManualCommand;
    private System.Windows.Forms.Label labelSeeInterfaceManual;
    private System.Windows.Forms.Button btnSend;
    public System.Windows.Forms.TextBox textBoxResponse;
    public System.Windows.Forms.ComboBox comboBoxGasType;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.GroupBox groupBox1;
    private System.Windows.Forms.GroupBox groupBox2;
    private System.Windows.Forms.ComboBox cmdPressureUnits;
    private System.Windows.Forms.Label label2;
    private Components.LEDLabel ledLabel7;
    private Components.LEDLabel ledLabel6;
    private Components.LEDLabel ledLabel5;
    private Components.LEDLabel ledLabel4;
    private Components.LEDLabel ledLabel2;
    private Components.LEDLabel ledLabel1;
    private Components.LEDLabel ledLabel0;
    private System.Windows.Forms.GroupBox groupBox3;
    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.GroupBox groupBox4;
    private System.Windows.Forms.Label lblRotorStatus;
    private Components.LEDLabel ledLabelDecel;
    private Components.LEDLabel ledLabelSensorUnstable;
    private Components.LEDLabel ledLabelDrive;
    private Components.LEDLabel ledLabelAccel;
    private System.Windows.Forms.Timer tmrRotorStatusRefresh;
    private System.Windows.Forms.Button btnDisarmed;
    private System.Windows.Forms.Button btnDismount;
    private System.Windows.Forms.Label label3;
  }
}