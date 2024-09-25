namespace VTIWindowsControlLibrary.Forms
{
    partial class SRSPTC10SetupForm
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
            this.TimerCheckReturnValue = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lblV3PIDSetpoint = new System.Windows.Forms.Label();
            this.btnSetV3Setpoint = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.txtSetPointV3 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnV3PIDStart = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnV3Off = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblV2PIDSetpoint = new System.Windows.Forms.Label();
            this.btnSetV2Setpoint = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSetPointV2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnV2PIDStart = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.btnV2Off = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lblV1PIDSetpoint = new System.Windows.Forms.Label();
            this.btnSetV1Setpoint = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSetPointV1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnV1PIDStart = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.btnV1OFF = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSRSCommand = new System.Windows.Forms.TextBox();
            this.txtSRSReturnData = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // TimerCheckReturnValue
            // 
            this.TimerCheckReturnValue.Tick += new System.EventHandler(this.TimerCheckReturnValue_Elapsed);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(572, 425);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.progressBar1);
            this.tabPage1.Controls.Add(this.btnClose);
            this.tabPage1.Controls.Add(this.btnRefresh);
            this.tabPage1.Controls.Add(this.groupBox3);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(564, 399);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Commands";
            this.tabPage1.UseVisualStyleBackColor = true;
            this.tabPage1.Click += new System.EventHandler(this.tabPage1_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(235, 340);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(106, 23);
            this.progressBar1.TabIndex = 4;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(405, 331);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(106, 32);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(81, 331);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(106, 32);
            this.btnRefresh.TabIndex = 3;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = true;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lblV3PIDSetpoint);
            this.groupBox3.Controls.Add(this.btnSetV3Setpoint);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.txtSetPointV3);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnV3PIDStart);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.btnV3Off);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(8, 225);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(528, 100);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Virtual Channel V3 (Bakeout Oven)";
            // 
            // lblV3PIDSetpoint
            // 
            this.lblV3PIDSetpoint.AutoSize = true;
            this.lblV3PIDSetpoint.Location = new System.Drawing.Point(334, 64);
            this.lblV3PIDSetpoint.Name = "lblV3PIDSetpoint";
            this.lblV3PIDSetpoint.Size = new System.Drawing.Size(15, 16);
            this.lblV3PIDSetpoint.TabIndex = 7;
            this.lblV3PIDSetpoint.Text = "x";
            // 
            // btnSetV3Setpoint
            // 
            this.btnSetV3Setpoint.Location = new System.Drawing.Point(443, 54);
            this.btnSetV3Setpoint.Name = "btnSetV3Setpoint";
            this.btnSetV3Setpoint.Size = new System.Drawing.Size(60, 32);
            this.btnSetV3Setpoint.TabIndex = 9;
            this.btnSetV3Setpoint.Text = "Set";
            this.btnSetV3Setpoint.UseVisualStyleBackColor = true;
            this.btnSetV3Setpoint.Click += new System.EventHandler(this.btnSetV3Setpoint_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(334, 33);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(93, 15);
            this.label11.TabIndex = 7;
            this.label11.Text = "Set Point (°F)";
            // 
            // txtSetPointV3
            // 
            this.txtSetPointV3.Location = new System.Drawing.Point(386, 64);
            this.txtSetPointV3.Name = "txtSetPointV3";
            this.txtSetPointV3.Size = new System.Drawing.Size(51, 22);
            this.txtSetPointV3.TabIndex = 6;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(90, 70);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(122, 16);
            this.label7.TabIndex = 3;
            this.label7.Text = "Start PID Control";
            // 
            // btnV3PIDStart
            // 
            this.btnV3PIDStart.Location = new System.Drawing.Point(24, 62);
            this.btnV3PIDStart.Name = "btnV3PIDStart";
            this.btnV3PIDStart.Size = new System.Drawing.Size(60, 32);
            this.btnV3PIDStart.TabIndex = 2;
            this.btnV3PIDStart.Text = "On";
            this.btnV3PIDStart.UseVisualStyleBackColor = true;
            this.btnV3PIDStart.Click += new System.EventHandler(this.btnV3PIDStart_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(90, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(196, 16);
            this.label8.TabIndex = 1;
            this.label8.Text = "Disable PID control outputs";
            // 
            // btnV3Off
            // 
            this.btnV3Off.Location = new System.Drawing.Point(24, 21);
            this.btnV3Off.Name = "btnV3Off";
            this.btnV3Off.Size = new System.Drawing.Size(60, 32);
            this.btnV3Off.TabIndex = 0;
            this.btnV3Off.Text = "Off";
            this.btnV3Off.UseVisualStyleBackColor = true;
            this.btnV3Off.Click += new System.EventHandler(this.btnV3Off_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lblV2PIDSetpoint);
            this.groupBox2.Controls.Add(this.btnSetV2Setpoint);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtSetPointV2);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.btnV2PIDStart);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.btnV2Off);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(8, 116);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(528, 100);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Virtual Channel V2 (Standard)";
            // 
            // lblV2PIDSetpoint
            // 
            this.lblV2PIDSetpoint.AutoSize = true;
            this.lblV2PIDSetpoint.Location = new System.Drawing.Point(334, 61);
            this.lblV2PIDSetpoint.Name = "lblV2PIDSetpoint";
            this.lblV2PIDSetpoint.Size = new System.Drawing.Size(15, 16);
            this.lblV2PIDSetpoint.TabIndex = 7;
            this.lblV2PIDSetpoint.Text = "x";
            // 
            // btnSetV2Setpoint
            // 
            this.btnSetV2Setpoint.Location = new System.Drawing.Point(443, 51);
            this.btnSetV2Setpoint.Name = "btnSetV2Setpoint";
            this.btnSetV2Setpoint.Size = new System.Drawing.Size(60, 32);
            this.btnSetV2Setpoint.TabIndex = 8;
            this.btnSetV2Setpoint.Text = "Set";
            this.btnSetV2Setpoint.UseVisualStyleBackColor = true;
            this.btnSetV2Setpoint.Click += new System.EventHandler(this.btnSetV2Setpoint_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(334, 30);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(94, 15);
            this.label10.TabIndex = 7;
            this.label10.Text = "Set Point (°C)";
            // 
            // txtSetPointV2
            // 
            this.txtSetPointV2.Location = new System.Drawing.Point(386, 61);
            this.txtSetPointV2.Name = "txtSetPointV2";
            this.txtSetPointV2.Size = new System.Drawing.Size(51, 22);
            this.txtSetPointV2.TabIndex = 6;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(90, 67);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(122, 16);
            this.label5.TabIndex = 3;
            this.label5.Text = "Start PID Control";
            // 
            // btnV2PIDStart
            // 
            this.btnV2PIDStart.Location = new System.Drawing.Point(24, 59);
            this.btnV2PIDStart.Name = "btnV2PIDStart";
            this.btnV2PIDStart.Size = new System.Drawing.Size(60, 32);
            this.btnV2PIDStart.TabIndex = 2;
            this.btnV2PIDStart.Text = "On";
            this.btnV2PIDStart.UseVisualStyleBackColor = true;
            this.btnV2PIDStart.Click += new System.EventHandler(this.btnV2PIDStart_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(196, 16);
            this.label6.TabIndex = 1;
            this.label6.Text = "Disable PID control outputs";
            // 
            // btnV2Off
            // 
            this.btnV2Off.Location = new System.Drawing.Point(24, 21);
            this.btnV2Off.Name = "btnV2Off";
            this.btnV2Off.Size = new System.Drawing.Size(60, 32);
            this.btnV2Off.TabIndex = 0;
            this.btnV2Off.Text = "Off";
            this.btnV2Off.UseVisualStyleBackColor = true;
            this.btnV2Off.Click += new System.EventHandler(this.btnV2Off_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lblV1PIDSetpoint);
            this.groupBox1.Controls.Add(this.btnSetV1Setpoint);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtSetPointV1);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnV1PIDStart);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnV1OFF);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(528, 100);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Virtual Channel V1 (UUT)";
            // 
            // lblV1PIDSetpoint
            // 
            this.lblV1PIDSetpoint.AutoSize = true;
            this.lblV1PIDSetpoint.Location = new System.Drawing.Point(334, 60);
            this.lblV1PIDSetpoint.Name = "lblV1PIDSetpoint";
            this.lblV1PIDSetpoint.Size = new System.Drawing.Size(15, 16);
            this.lblV1PIDSetpoint.TabIndex = 7;
            this.lblV1PIDSetpoint.Text = "x";
            // 
            // btnSetV1Setpoint
            // 
            this.btnSetV1Setpoint.Location = new System.Drawing.Point(443, 53);
            this.btnSetV1Setpoint.Name = "btnSetV1Setpoint";
            this.btnSetV1Setpoint.Size = new System.Drawing.Size(60, 32);
            this.btnSetV1Setpoint.TabIndex = 6;
            this.btnSetV1Setpoint.Text = "Set";
            this.btnSetV1Setpoint.UseVisualStyleBackColor = true;
            this.btnSetV1Setpoint.Click += new System.EventHandler(this.btnSetV1Setpoint_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(311, 28);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 15);
            this.label9.TabIndex = 5;
            this.label9.Text = "Set Point (°F)";
            // 
            // txtSetPointV1
            // 
            this.txtSetPointV1.Location = new System.Drawing.Point(386, 60);
            this.txtSetPointV1.Name = "txtSetPointV1";
            this.txtSetPointV1.Size = new System.Drawing.Size(51, 22);
            this.txtSetPointV1.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(90, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(122, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "Start PID Control";
            // 
            // btnV1PIDStart
            // 
            this.btnV1PIDStart.BackColor = System.Drawing.Color.Transparent;
            this.btnV1PIDStart.Location = new System.Drawing.Point(24, 60);
            this.btnV1PIDStart.Name = "btnV1PIDStart";
            this.btnV1PIDStart.Size = new System.Drawing.Size(60, 32);
            this.btnV1PIDStart.TabIndex = 2;
            this.btnV1PIDStart.Text = "On";
            this.btnV1PIDStart.UseVisualStyleBackColor = false;
            this.btnV1PIDStart.Click += new System.EventHandler(this.btnV1PIDStart_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(90, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 16);
            this.label3.TabIndex = 1;
            this.label3.Text = "Disable PID control outputs";
            // 
            // btnV1OFF
            // 
            this.btnV1OFF.BackColor = System.Drawing.Color.Transparent;
            this.btnV1OFF.ForeColor = System.Drawing.Color.Black;
            this.btnV1OFF.Location = new System.Drawing.Point(24, 21);
            this.btnV1OFF.Name = "btnV1OFF";
            this.btnV1OFF.Size = new System.Drawing.Size(60, 32);
            this.btnV1OFF.TabIndex = 0;
            this.btnV1OFF.Text = "Off";
            this.btnV1OFF.UseVisualStyleBackColor = false;
            this.btnV1OFF.Click += new System.EventHandler(this.btnV1OFF_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.txtSRSCommand);
            this.tabPage2.Controls.Add(this.txtSRSReturnData);
            this.tabPage2.Controls.Add(this.btnSend);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(564, 399);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Manual Entry Terminal";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(8, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(185, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Enter program commands";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(8, 136);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 16);
            this.label1.TabIndex = 6;
            this.label1.Text = "Returned Values";
            // 
            // txtSRSCommand
            // 
            this.txtSRSCommand.Location = new System.Drawing.Point(6, 24);
            this.txtSRSCommand.Multiline = true;
            this.txtSRSCommand.Name = "txtSRSCommand";
            this.txtSRSCommand.Size = new System.Drawing.Size(472, 93);
            this.txtSRSCommand.TabIndex = 4;
            // 
            // txtSRSReturnData
            // 
            this.txtSRSReturnData.Location = new System.Drawing.Point(11, 160);
            this.txtSRSReturnData.Multiline = true;
            this.txtSRSReturnData.Name = "txtSRSReturnData";
            this.txtSRSReturnData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtSRSReturnData.Size = new System.Drawing.Size(236, 143);
            this.txtSRSReturnData.TabIndex = 5;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(270, 128);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(66, 28);
            this.btnSend.TabIndex = 3;
            this.btnSend.Text = "Send";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // SRSPTC10SetupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 425);
            this.ControlBox = false;
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SRSPTC10SetupForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "SRS Temperature Controller";
            this.Load += new System.EventHandler(this.SRSPTC10SetupForm_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer TimerCheckReturnValue;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSRSCommand;
        private System.Windows.Forms.TextBox txtSRSReturnData;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnV3PIDStart;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnV3Off;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnV2PIDStart;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnV2Off;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnV1PIDStart;
        private System.Windows.Forms.Button btnV1OFF;
        private System.Windows.Forms.Button btnSetV3Setpoint;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtSetPointV3;
        private System.Windows.Forms.Button btnSetV2Setpoint;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSetPointV2;
        private System.Windows.Forms.Button btnSetV1Setpoint;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSetPointV1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Label lblV3PIDSetpoint;
        private System.Windows.Forms.Label lblV2PIDSetpoint;
        private System.Windows.Forms.Label lblV1PIDSetpoint;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Timer timer1;
    }
}