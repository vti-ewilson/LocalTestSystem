namespace VTIWindowsControlLibrary.Forms
{
    partial class IAIRoboCylinderManualControlForm
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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabelPower = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelServo = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelReady = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelMove = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelCommand = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelAlarm = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabelPosition = new System.Windows.Forms.ToolStripStatusLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDownPosNum = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.buttonPositionMove = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBoxPosition = new System.Windows.Forms.TextBox();
            this.buttonAbsMove = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.buttonServoOff = new System.Windows.Forms.Button();
            this.buttonServoOn = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.buttonStop = new System.Windows.Forms.Button();
            this.buttonHome = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.textBoxIncPos = new System.Windows.Forms.TextBox();
            this.buttonIncMove = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.buttonLoadPosData = new System.Windows.Forms.Button();
            this.checkBoxSetMaxAccel = new System.Windows.Forms.CheckBox();
            this.textBoxSetPushPercent = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxSetAccel = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxSetVelocity = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxSetPosBand = new System.Windows.Forms.TextBox();
            this.textBoxSetPosition = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numericUpDownSetPushTime = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownSetPosNum = new System.Windows.Forms.NumericUpDown();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSetPushTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSetPosNum)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabelPower,
            this.toolStripStatusLabelServo,
            this.toolStripStatusLabelReady,
            this.toolStripStatusLabelMove,
            this.toolStripStatusLabelCommand,
            this.toolStripStatusLabelAlarm,
            this.toolStripStatusLabelPosition});
            this.statusStrip1.Location = new System.Drawing.Point(0, 197);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(613, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelPower
            // 
            this.toolStripStatusLabelPower.Name = "toolStripStatusLabelPower";
            this.toolStripStatusLabelPower.Size = new System.Drawing.Size(56, 17);
            this.toolStripStatusLabelPower.Text = "Power Off";
            // 
            // toolStripStatusLabelServo
            // 
            this.toolStripStatusLabelServo.Name = "toolStripStatusLabelServo";
            this.toolStripStatusLabelServo.Size = new System.Drawing.Size(54, 17);
            this.toolStripStatusLabelServo.Text = "Servo Off";
            // 
            // toolStripStatusLabelReady
            // 
            this.toolStripStatusLabelReady.Name = "toolStripStatusLabelReady";
            this.toolStripStatusLabelReady.Size = new System.Drawing.Size(104, 17);
            this.toolStripStatusLabelReady.Text = "NOT Ready to Move";
            // 
            // toolStripStatusLabelMove
            // 
            this.toolStripStatusLabelMove.Name = "toolStripStatusLabelMove";
            this.toolStripStatusLabelMove.Size = new System.Drawing.Size(105, 17);
            this.toolStripStatusLabelMove.Text = "Move NOT Complete";
            // 
            // toolStripStatusLabelCommand
            // 
            this.toolStripStatusLabelCommand.Name = "toolStripStatusLabelCommand";
            this.toolStripStatusLabelCommand.Size = new System.Drawing.Size(97, 17);
            this.toolStripStatusLabelCommand.Text = "Command Refused";
            // 
            // toolStripStatusLabelAlarm
            // 
            this.toolStripStatusLabelAlarm.Name = "toolStripStatusLabelAlarm";
            this.toolStripStatusLabelAlarm.Size = new System.Drawing.Size(50, 17);
            this.toolStripStatusLabelAlarm.Text = "No Alarm";
            // 
            // toolStripStatusLabelPosition
            // 
            this.toolStripStatusLabelPosition.Name = "toolStripStatusLabelPosition";
            this.toolStripStatusLabelPosition.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabelPosition.Text = "Position: 0.00 mm";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(6, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 31);
            this.label1.TabIndex = 1;
            this.label1.Text = "Position Number:";
            // 
            // numericUpDownPosNum
            // 
            this.numericUpDownPosNum.Location = new System.Drawing.Point(69, 57);
            this.numericUpDownPosNum.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownPosNum.Name = "numericUpDownPosNum";
            this.numericUpDownPosNum.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownPosNum.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox1.Controls.Add(this.buttonPositionMove);
            this.groupBox1.Controls.Add(this.numericUpDownPosNum);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(231, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(122, 97);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Position Move";
            // 
            // buttonPositionMove
            // 
            this.buttonPositionMove.Location = new System.Drawing.Point(6, 19);
            this.buttonPositionMove.Name = "buttonPositionMove";
            this.buttonPositionMove.Size = new System.Drawing.Size(110, 32);
            this.buttonPositionMove.TabIndex = 1;
            this.buttonPositionMove.Text = "Position Move";
            this.buttonPositionMove.UseVisualStyleBackColor = true;
            this.buttonPositionMove.Click += new System.EventHandler(this.buttonPositionMove_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox2.Controls.Add(this.textBoxPosition);
            this.groupBox2.Controls.Add(this.buttonAbsMove);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(359, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(122, 97);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Absolute Move";
            // 
            // textBoxPosition
            // 
            this.textBoxPosition.Location = new System.Drawing.Point(59, 58);
            this.textBoxPosition.Name = "textBoxPosition";
            this.textBoxPosition.Size = new System.Drawing.Size(57, 20);
            this.textBoxPosition.TabIndex = 2;
            // 
            // buttonAbsMove
            // 
            this.buttonAbsMove.Location = new System.Drawing.Point(6, 19);
            this.buttonAbsMove.Name = "buttonAbsMove";
            this.buttonAbsMove.Size = new System.Drawing.Size(110, 32);
            this.buttonAbsMove.TabIndex = 1;
            this.buttonAbsMove.Text = "Absolute Move";
            this.buttonAbsMove.UseVisualStyleBackColor = true;
            this.buttonAbsMove.Click += new System.EventHandler(this.buttonAbsMove_Click);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(6, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 30);
            this.label2.TabIndex = 1;
            this.label2.Text = "Position (mm):";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.groupBox6);
            this.flowLayoutPanel1.Controls.Add(this.groupBox4);
            this.flowLayoutPanel1.Controls.Add(this.groupBox1);
            this.flowLayoutPanel1.Controls.Add(this.groupBox2);
            this.flowLayoutPanel1.Controls.Add(this.groupBox3);
            this.flowLayoutPanel1.Controls.Add(this.groupBox5);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(613, 197);
            this.flowLayoutPanel1.TabIndex = 7;
            // 
            // groupBox6
            // 
            this.groupBox6.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox6.Controls.Add(this.buttonServoOff);
            this.groupBox6.Controls.Add(this.buttonServoOn);
            this.groupBox6.Location = new System.Drawing.Point(3, 3);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(87, 97);
            this.groupBox6.TabIndex = 9;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Servo";
            // 
            // buttonServoOff
            // 
            this.buttonServoOff.Location = new System.Drawing.Point(6, 56);
            this.buttonServoOff.Name = "buttonServoOff";
            this.buttonServoOff.Size = new System.Drawing.Size(75, 32);
            this.buttonServoOff.TabIndex = 0;
            this.buttonServoOff.Text = "Servo Off";
            this.buttonServoOff.UseVisualStyleBackColor = true;
            this.buttonServoOff.Click += new System.EventHandler(this.buttonServoOff_Click);
            // 
            // buttonServoOn
            // 
            this.buttonServoOn.Location = new System.Drawing.Point(6, 19);
            this.buttonServoOn.Name = "buttonServoOn";
            this.buttonServoOn.Size = new System.Drawing.Size(75, 32);
            this.buttonServoOn.TabIndex = 0;
            this.buttonServoOn.Text = "Servo On";
            this.buttonServoOn.UseVisualStyleBackColor = true;
            this.buttonServoOn.Click += new System.EventHandler(this.buttonServoOn_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox4.Controls.Add(this.buttonStop);
            this.groupBox4.Controls.Add(this.buttonHome);
            this.groupBox4.Location = new System.Drawing.Point(96, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(129, 97);
            this.groupBox4.TabIndex = 7;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Motion";
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(6, 57);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(117, 32);
            this.buttonStop.TabIndex = 1;
            this.buttonStop.Text = "Stop Motion";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // buttonHome
            // 
            this.buttonHome.Location = new System.Drawing.Point(6, 19);
            this.buttonHome.Name = "buttonHome";
            this.buttonHome.Size = new System.Drawing.Size(117, 32);
            this.buttonHome.TabIndex = 1;
            this.buttonHome.Text = "Home";
            this.buttonHome.UseVisualStyleBackColor = true;
            this.buttonHome.Click += new System.EventHandler(this.buttonHome_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.textBoxIncPos);
            this.groupBox3.Controls.Add(this.buttonIncMove);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Location = new System.Drawing.Point(487, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(122, 97);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Incremental Move";
            // 
            // textBoxIncPos
            // 
            this.textBoxIncPos.Location = new System.Drawing.Point(59, 56);
            this.textBoxIncPos.Name = "textBoxIncPos";
            this.textBoxIncPos.Size = new System.Drawing.Size(57, 20);
            this.textBoxIncPos.TabIndex = 2;
            // 
            // buttonIncMove
            // 
            this.buttonIncMove.Location = new System.Drawing.Point(6, 19);
            this.buttonIncMove.Name = "buttonIncMove";
            this.buttonIncMove.Size = new System.Drawing.Size(110, 32);
            this.buttonIncMove.TabIndex = 1;
            this.buttonIncMove.Text = "Incremental Move";
            this.buttonIncMove.UseVisualStyleBackColor = true;
            this.buttonIncMove.Click += new System.EventHandler(this.buttonIncMove_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "Position (mm):";
            // 
            // groupBox5
            // 
            this.groupBox5.AutoSize = true;
            this.groupBox5.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox5.Controls.Add(this.buttonLoadPosData);
            this.groupBox5.Controls.Add(this.checkBoxSetMaxAccel);
            this.groupBox5.Controls.Add(this.textBoxSetPushPercent);
            this.groupBox5.Controls.Add(this.label9);
            this.groupBox5.Controls.Add(this.textBoxSetAccel);
            this.groupBox5.Controls.Add(this.label8);
            this.groupBox5.Controls.Add(this.textBoxSetVelocity);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.textBoxSetPosBand);
            this.groupBox5.Controls.Add(this.textBoxSetPosition);
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.numericUpDownSetPushTime);
            this.groupBox5.Controls.Add(this.numericUpDownSetPosNum);
            this.groupBox5.Location = new System.Drawing.Point(3, 106);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(605, 85);
            this.groupBox5.TabIndex = 8;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Set Up Position Data";
            // 
            // buttonLoadPosData
            // 
            this.buttonLoadPosData.Location = new System.Drawing.Point(551, 42);
            this.buttonLoadPosData.Name = "buttonLoadPosData";
            this.buttonLoadPosData.Size = new System.Drawing.Size(48, 23);
            this.buttonLoadPosData.TabIndex = 5;
            this.buttonLoadPosData.Text = "LOAD";
            this.buttonLoadPosData.UseVisualStyleBackColor = true;
            this.buttonLoadPosData.Click += new System.EventHandler(this.buttonLoadPosData_Click);
            // 
            // checkBoxSetMaxAccel
            // 
            this.checkBoxSetMaxAccel.AutoSize = true;
            this.checkBoxSetMaxAccel.Location = new System.Drawing.Point(436, 48);
            this.checkBoxSetMaxAccel.Name = "checkBoxSetMaxAccel";
            this.checkBoxSetMaxAccel.Size = new System.Drawing.Size(108, 17);
            this.checkBoxSetMaxAccel.TabIndex = 4;
            this.checkBoxSetMaxAccel.Text = "Max Acceleration";
            this.checkBoxSetMaxAccel.UseVisualStyleBackColor = true;
            // 
            // textBoxSetPushPercent
            // 
            this.textBoxSetPushPercent.Location = new System.Drawing.Point(61, 45);
            this.textBoxSetPushPercent.Name = "textBoxSetPushPercent";
            this.textBoxSetPushPercent.Size = new System.Drawing.Size(57, 20);
            this.textBoxSetPushPercent.TabIndex = 2;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(124, 47);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(82, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Push Time (ms):";
            // 
            // textBoxSetAccel
            // 
            this.textBoxSetAccel.Location = new System.Drawing.Point(542, 18);
            this.textBoxSetAccel.Name = "textBoxSetAccel";
            this.textBoxSetAccel.Size = new System.Drawing.Size(57, 20);
            this.textBoxSetAccel.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Push %:";
            // 
            // textBoxSetVelocity
            // 
            this.textBoxSetVelocity.Location = new System.Drawing.Point(387, 19);
            this.textBoxSetVelocity.Name = "textBoxSetVelocity";
            this.textBoxSetVelocity.Size = new System.Drawing.Size(57, 20);
            this.textBoxSetVelocity.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(450, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 13);
            this.label7.TabIndex = 1;
            this.label7.Text = "Acceleration (G):";
            // 
            // textBoxSetPosBand
            // 
            this.textBoxSetPosBand.Location = new System.Drawing.Point(372, 46);
            this.textBoxSetPosBand.Name = "textBoxSetPosBand";
            this.textBoxSetPosBand.Size = new System.Drawing.Size(57, 20);
            this.textBoxSetPosBand.TabIndex = 2;
            // 
            // textBoxSetPosition
            // 
            this.textBoxSetPosition.Location = new System.Drawing.Point(234, 19);
            this.textBoxSetPosition.Name = "textBoxSetPosition";
            this.textBoxSetPosition.Size = new System.Drawing.Size(57, 20);
            this.textBoxSetPosition.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(299, 21);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Velocity (mm/s):";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(266, 46);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(100, 13);
            this.label10.TabIndex = 1;
            this.label10.Text = "Position Band (mm):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Position Number:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(155, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 1;
            this.label5.Text = "Position (mm):";
            // 
            // numericUpDownSetPushTime
            // 
            this.numericUpDownSetPushTime.Location = new System.Drawing.Point(212, 45);
            this.numericUpDownSetPushTime.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.numericUpDownSetPushTime.Name = "numericUpDownSetPushTime";
            this.numericUpDownSetPushTime.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownSetPushTime.TabIndex = 3;
            // 
            // numericUpDownSetPosNum
            // 
            this.numericUpDownSetPosNum.Location = new System.Drawing.Point(102, 19);
            this.numericUpDownSetPosNum.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDownSetPosNum.Name = "numericUpDownSetPosNum";
            this.numericUpDownSetPosNum.Size = new System.Drawing.Size(47, 20);
            this.numericUpDownSetPosNum.TabIndex = 3;
            // 
            // IAIRoboCylinderManualControlForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(613, 219);
            this.Controls.Add(this.flowLayoutPanel1);
            this.Controls.Add(this.statusStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "IAIRoboCylinderManualControlForm";
            this.Text = "IAI RoboCylinder Manual Control";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.IAIRoboCylinderManualControlForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownPosNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSetPushTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownSetPosNum)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPower;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelServo;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelReady;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelMove;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelCommand;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelAlarm;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPosition;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button buttonPositionMove;
        private System.Windows.Forms.NumericUpDown numericUpDownPosNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBoxPosition;
        private System.Windows.Forms.Button buttonAbsMove;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Button buttonHome;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox textBoxIncPos;
        private System.Windows.Forms.Button buttonIncMove;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDownSetPosNum;
        private System.Windows.Forms.TextBox textBoxSetPosition;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxSetVelocity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxSetAccel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textBoxSetPushPercent;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDownSetPushTime;
        private System.Windows.Forms.CheckBox checkBoxSetMaxAccel;
        private System.Windows.Forms.TextBox textBoxSetPosBand;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button buttonLoadPosData;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button buttonServoOn;
        private System.Windows.Forms.Button buttonServoOff;
    }
}