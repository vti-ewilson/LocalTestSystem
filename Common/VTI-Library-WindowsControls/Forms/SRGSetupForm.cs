using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Displays a window similar to a <see cref="MessageBox">MessageBox</see>
    /// with a <see cref="TextBox">TextBox</see> for the user to enter a value
    /// </summary>
    public partial class SRGSetupForm : Form
    {
        private string strReturnValue;
        private Point pntStartLocation;
        private MKSSRG _MKSSRG;
        public bool bIsGoodSRGPort;
        protected string _prevSamplingInterval, _gasTemperature, _accomodationFactor;
        protected int _gasType;

        /// <summary>
        /// Initializes a new instance of the <see cref="SRGSetupForm">InputBoxForm</see>
        /// </summary>
        public SRGSetupForm()
        {
            InitializeComponent();
            bIsGoodSRGPort = false;
            this.strReturnValue = "";
        }

        /// <summary>
        /// Populate the SRG Setup Form
        /// </summary>
        public void PopulateForm(List<string> gasList)
        {
            if (_MKSSRG == null)
                return;
            //if (gasList != null)
            //  InitializeGasType(gasList);

            int ndxGasType = _MKSSRG.ReadGasType();
            //if (ndxGasType > 0 && ndxGasType <=  this.comboBoxGasType.MaxDropDownItems) // <= gasList.Count)
            comboBoxGasType.SelectedIndex = ndxGasType;
            //else {
            //  comboBoxGasType.SelectedIndex = 0;

            //}
            UpdateFormValues();
        }

        /// <summary>
        /// Updates form controls with rotor control status
        /// </summary>
        public void UpdateRotorControlStatus()
        {
            lblRotorStatus.Text = _MKSSRG.RotorControlStatusMessage;
            ledLabelAccel.Lit = _MKSSRG._accelerate;
            ledLabelDecel.Lit = _MKSSRG._decelerate;
            ledLabelDrive.Lit = _MKSSRG._driveOperating;
            ledLabelSensorUnstable.Lit = _MKSSRG._SensorUnstable;
        }

        [Flags]
        private enum StatusSystem { SP1 = 1, SP2 = 2, RDY = 4, PrintOffline = 8, DataAvail = 16, MessagePend = 32, BackFailedDefaulted = 64, PowerFail = 128 };

        /// <summary>
        /// Updates SRG SetupForm values
        /// </summary>
        public void UpdateFormValues()
        {
            //if _MKSSRG. Exit if the SRG is offline
            bool LocalModeTemp = _MKSSRG.LocalMode;
            Thread.Sleep(100);
            _MKSSRG.LocalMode = true;
            try
            {
                // Use process in future to update public variables when a flag is set
                textBoxSamplingInterval.Text = string.Format("{0:0}", _MKSSRG.ReadInterval());
                //textBoxGasTemperature.Text = string.Format("{0:0.00}", _MKSSRG.ReadTemperature());
                //textBoxAccomodationFactor.Text = string.Format("{0:0.0000}", _MKSSRG.ReadAccomodationFactor());
                textBoxSignalStrength.Text = string.Format("{0:0.00}", _MKSSRG.ReadSignalStrength());
                textBoxSignalDampening.Text = string.Format("{0:0.000}", _MKSSRG.ReadDampening());
                textBoxRotorSpeed.Text = string.Format("{0:0.0E0}", _MKSSRG.ReadSpeed());

                // Use process in future to update public variables when a flag is set
                byte SRGSystemStatus;
                SRGSystemStatus = _MKSSRG.ReadSystemStatus();
                Console.WriteLine(SRGSystemStatus);
                byte MKSStatus;
                foreach (StatusSystem value in Enum.GetValues(typeof(StatusSystem)))
                {
                    MKSStatus = Convert.ToByte(value);
                    if ((SRGSystemStatus & MKSStatus) == MKSStatus)
                    {
                        Console.WriteLine("Status: " + value + " Set");
                        if (MKSStatus == 1) ledLabel0.Lit = true;
                        if (MKSStatus == 2) ledLabel1.Lit = true;
                        if (MKSStatus == 4) ledLabel2.Lit = true;
                        //if (MKSStatus == 8) ledLabel3.Lit = true;
                        if (MKSStatus == 16) ledLabel4.Lit = true;
                        if (MKSStatus == 32) ledLabel5.Lit = true;
                        if (MKSStatus == 64) ledLabel6.Lit = true;
                        if (MKSStatus == 128) ledLabel7.Lit = true;
                    }
                    else
                    {
                        if (MKSStatus == 1) ledLabel0.Lit = false;
                        if (MKSStatus == 2) ledLabel1.Lit = false;
                        if (MKSStatus == 4) ledLabel2.Lit = false;
                        //if (MKSStatus == 8) ledLabel3.Lit = false;
                        if (MKSStatus == 16) ledLabel4.Lit = false;
                        if (MKSStatus == 32) ledLabel5.Lit = false;
                        if (MKSStatus == 64) ledLabel6.Lit = false;
                        if (MKSStatus == 128) ledLabel7.Lit = false;
                    }
                }

                cmdPressureUnits.SelectedIndex = _MKSSRG.ReadPressureUnits();
            }
            catch
            {
            }
            finally
            {
                _MKSSRG.LocalMode = LocalModeTemp;
            }
        }

        /// <summary>
        /// Initialize Gas Type, Select a Gas
        /// </summary>
        private void InitializeGasType(List<string> gasList)
        {
            //int ndx = 0;
            //if (!gasList.Contains("Select a Gas"))
            //  comboBoxGasType.Items.Insert(ndx++, "Select a Gas");
            //foreach (string strGas in gasList) {
            //  comboBoxGasType.Items.Insert(ndx++, strGas);
            //}
        }

        /// <summary>
        /// Set the Serial Port
        /// </summary>
        public void SetSerialPort(MKSSRG port)
        {
            _MKSSRG = port;
        }

        private int SendCommand(string theCommand, string portName)
        {
            return 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            UpdateSRG();
            StoreCurrentSettings();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            RestorePreviousSettings();
            this.Hide();
        }

        // Remember what the settings were at time this form gets displayed
        private void StoreCurrentSettings()
        {
            _prevSamplingInterval = textBoxSamplingInterval.Text;
            _gasTemperature = textBoxGasTemperature.Text;
            _gasType = comboBoxGasType.SelectedIndex;
            _accomodationFactor = textBoxAccomodationFactor.Text;
        }

        // Restore settings to what they were before this form was displayed
        private void RestorePreviousSettings()
        {
            textBoxSamplingInterval.Text = _prevSamplingInterval;
            textBoxGasTemperature.Text = _gasTemperature;
            comboBoxGasType.SelectedIndex = _gasType;
            textBoxAccomodationFactor.Text = _accomodationFactor;
        }

        /// <summary>
        /// Title to display on then input box
        /// </summary>
        public string Title
        {
            set
            {
                this.Text = value;
            }
        }

        /*
            /// <summary>
            /// Prompt to display in the input box
            /// </summary>
            public string Prompt
            {
              set
              {
                this.lblManualCommand.Text = value;
              }
            }
        */

        /// <summary>
        /// Value entered by the user
        /// </summary>
        /// <remarks>
        /// If the user clicks the Cancel button, the value will be an empty string.
        /// </remarks>
        public string ReturnValue
        {
            get
            {
                return strReturnValue;
            }
        }

        /// <summary>
        /// Optional default response for the input box
        /// </summary>
        public string DefaultResponse
        {
            set
            {
                this.textBoxSamplingInterval.Text = value;
                this.textBoxSamplingInterval.SelectAll();
            }
        }

        /// <summary>
        /// Optional starting location for the input box window
        /// </summary>
        public Point StartLocation
        {
            set
            {
                this.pntStartLocation = value;
            }
        }

        private void UpdateSRG()
        {
            // Gas Type
            _MKSSRG.SetInterval(textBoxSamplingInterval.Text);
            //_MKSSRG.SetTemperature(textBoxGasTemperature.Text);
            _MKSSRG.SetGasType(comboBoxGasType.SelectedIndex);
            //_MKSSRG.SetAccomodationFactor(textBoxAccomodationFactor.Text);
            _MKSSRG.SetPressureUnit(cmdPressureUnits.SelectedIndex);
        }

        private void txtBoxSamplingInterval_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxGasTemperature_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxGasType_TextChanged(object sender, EventArgs e)
        {
        }

        private void textBoxAccomodationFactor_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnRTLSRG_Click(object sender, EventArgs e)
        {
            if (_MKSSRG.LocalMode == false)
            {
                //_MKSSRG.LocalMode = true;
                _MKSSRG.setLocalMode = true;
                //_MKSSRG.ReturnToLocal();
                this.btnRTLSRG.Text = "Auto Read SRG";
            }
            else
            {
                _MKSSRG.LocalMode = false;
                this.btnRTLSRG.Text = "Return Local Control (RTL)";
            }
        }

        private void btnStartSRG_Click(object sender, EventArgs e)
        {
            _MKSSRG.StartSRG();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            this.btnRefresh.Enabled = false;
            UpdateFormValues();
            this.btnRefresh.Enabled = true;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            this.textBoxResponse.Text = _MKSSRG.SendManualCommand(this.textBoxManualCommand.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnStopSRG_Click(object sender, EventArgs e)
        {
            _MKSSRG.StopSRG();
        }

        private void SRGSetupForm_Load(object sender, EventArgs e)
        {
        }

        private void btnClearError_Click(object sender, EventArgs e)
        {
            _MKSSRG.ClearErrors();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateRotorControlStatus();
            tmrRotorStatusRefresh.Interval = 1000;
        }

        private void btnDismount_Click(object sender, EventArgs e)
        {
            _MKSSRG.DismountSRG();
        }

        private void btnDisarmed_Click(object sender, EventArgs e)
        {
            _MKSSRG.DisarmSRG();
        }
    }
}