using System;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;

namespace VTIWindowsControlLibrary.Forms
{
    // On client
    //  [ManualCommand("SHOW SRS PTC10 SETUP", true, CommandPermissionType.None)]
    //public virtual void SRSPTC10Setup()
    //{
    //    try
    //    {
    //        Machine.SRSPTC10SetupForm.Show();
    //    }
    //    catch (Exception e)
    //    {
    //    }
    //}

    // In Machine on client
    //protected SRSPTC10SetupForm _SRSPTC10SetupForm;
    //public static SRSPTC10SetupForm SRSPTC10SetupForm { get { return Instance._SRSPTC10SetupForm; } }
    //protected virtual void InitializeSRSPTC10Form()
    //{
    //    SplashScreen.Message = "Initializing SRSTemperatureSetup...";
    //    _SRSPTC10SetupForm = new SRSPTC10SetupForm();
    //    _SRSPTC10SetupForm.SetSerialPort(IO.SerialIn.SRSPTC10);
    //}

    /// <summary>
    /// SRS PTC10 Programmable Temperature Controller Form
    /// </summary>
    public partial class SRSPTC10SetupForm : Form
    {
        private SRSPTC10 _SRSPTC10;

        /// <summary>
        /// Initializes the forms components
        /// </summary>
        public SRSPTC10SetupForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Set the Serial Port
        /// </summary>
        public void SetSerialPort(SRSPTC10 port)
        {
            _SRSPTC10 = port;
        }

        ///// <summary>  DO NOT DO THIS unless you can initialize the instance from within this class
        ///// Shows the SRS manual commands form
        ///// </summary>
        //public static void ShowSRSPTC10form()
        //{
        //    if (_SRSPTC10SetupForm == null)
        //        _SRSPTC10SetupForm = new SRSPTC10SetupForm();

        //    _SRSPTC10SetupForm.Show();
        //}

        private void btnV1OFF_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V1.Off");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnV2Off_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V2.Off");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnV3Off_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V3.Off");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnV1PIDStart_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V1.PID.Mode = On");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnV2PIDStart_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V2.PID.Mode = On");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnV3PIDStart_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag("V3.PID.Mode = On");
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_SRSPTC10 == null)
                return;
            _SRSPTC10.SendManualCommandFlag(this.txtSRSCommand.Text.ToString());

            TimerCheckReturnValue.Interval = 3500;
            TimerCheckReturnValue.Start();
        }

        private void TimerCheckReturnValue_Elapsed(object sender, EventArgs e)
        {
            Console.WriteLine("After Timer expired _SendManualCommandResult = " + _SRSPTC10.SendManualCommandResult);

            this.txtSRSReturnData.Text = _SRSPTC10.SendManualCommandResult + Environment.NewLine + this.txtSRSReturnData.Text;
            _SRSPTC10.SendManualCommandResult = "";
            TimerCheckReturnValue.Stop();
        }

        private void btnSetV1Setpoint_Click(object sender, EventArgs e)
        {
            double parseResult;
            if (double.TryParse(this.txtSetPointV1.Text, out parseResult))
            {
                _SRSPTC10.SetVirtualChannelSetPoint("V1", parseResult);
                this.txtSetPointV1.Text = parseResult.ToString();
            }
            else
            {
                this.txtSetPointV1.Text = "Error";
            }
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private Timer timer1Refresh = new Timer();

        private void SRSPTC10SetupForm_Load(object sender, EventArgs e)
        {
            timer1Refresh.Enabled = true;
            timer1Refresh.Interval = 1000;
            progressBar1.Maximum = 5;
            timer1Refresh.Tick += new EventHandler(timer1Refresh_Tick);
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void timer1Refresh_Tick(object sender, EventArgs e)
        {
            if (progressBar1.Value >= 5)
            {
                progressBar1.Value = 0;
                RefreshFormValues();
            }
            progressBar1.Value++;
        }

        private void btnSetV2Setpoint_Click(object sender, EventArgs e)
        {
            double parseResult;
            if (double.TryParse(this.txtSetPointV2.Text, out parseResult))
            {
                double setpoint = parseResult * 9 / 5 + 32; // Send value in F
                _SRSPTC10.SetVirtualChannelSetPoint("V2", setpoint);
                this.txtSetPointV2.Text = parseResult.ToString();
            }
            else
            {
                this.txtSetPointV2.Text = "Error";
            }
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnSetV3Setpoint_Click(object sender, EventArgs e)
        {
            double parseResult;
            if (double.TryParse(this.txtSetPointV3.Text, out parseResult))
            {
                _SRSPTC10.SetVirtualChannelSetPoint("V3", parseResult);
                this.txtSetPointV3.Text = parseResult.ToString();
            }
            else
            {
                this.txtSetPointV3.Text = "Error";
            }
            timer1Refresh.Start();
            _SRSPTC10.ReadPIDValues = true;
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _SRSPTC10.ReadPIDValues = true;
            RefreshFormValues();
            //timer1Refresh.Start();
        }

        private void RefreshFormValues()
        {
            timer1Refresh.Stop();
            try
            {
                this.lblV1PIDSetpoint.Text = _SRSPTC10.V1PIDTemperatureSetpoint.ToString("###.#");
                double setpoint = _SRSPTC10.V2PIDTemperatureSetpoint;
                setpoint = (setpoint - 32) * 5 / 9;
                this.lblV2PIDSetpoint.Text = setpoint.ToString("###.#");
                this.lblV3PIDSetpoint.Text = _SRSPTC10.V3PIDTemperatureSetpoint.ToString("###.#");
            }
            catch { }

            if (_SRSPTC10.V1PIDState == "On")
            {
                btnV1OFF.BackColor = Color.Transparent;
                btnV1PIDStart.BackColor = Color.Red;
            }
            else
            {
                btnV1OFF.BackColor = Color.Lime;
                btnV1PIDStart.BackColor = Color.Transparent;
            }

            if (_SRSPTC10.V2PIDState == "On")
            {
                btnV2Off.BackColor = Color.Transparent;
                btnV2PIDStart.BackColor = Color.Red;
            }
            else
            {
                btnV2Off.BackColor = Color.Lime;
                btnV2PIDStart.BackColor = Color.Transparent;
            }

            if (_SRSPTC10.V3PIDState == "On")
            {
                btnV3Off.BackColor = Color.Transparent;
                btnV3PIDStart.BackColor = Color.Red;
            }
            else
            {
                btnV3Off.BackColor = Color.Lime;
                btnV3PIDStart.BackColor = Color.Transparent;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {
        }
    }
}