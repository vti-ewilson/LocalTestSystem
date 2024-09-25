using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Form that provides manual control for a <see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.IAIRoboCylinder">IAIRoboCylinder</see>
    /// </summary>
    public partial class IAIRoboCylinderManualControlForm : Form
    {
        private IAIRoboCylinder _roboCylinder;

        /// <summary>
        /// Initializes a new instance of the <see cref="IAIRoboCylinderManualControlForm">IAIRoboCylinderManualControlForm</see> class
        /// </summary>
        public IAIRoboCylinderManualControlForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IAIRoboCylinderManualControlForm">IAIRoboCylinderManualControlForm</see> class
        /// </summary>
        /// <param name="RoboCylinder"><see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.IAIRoboCylinder">IAIRoboCylinder</see>
        /// to be assiciated with the form</param>
        public IAIRoboCylinderManualControlForm(IAIRoboCylinder RoboCylinder)
        {
            InitializeComponent();
            _roboCylinder = RoboCylinder;
            _roboCylinder.ValueChanged += new EventHandler(_roboCylinder_ValueChanged);
        }

        /// <summary>
        /// Gets or sets the <see cref="VTIWindowsControlLibrary.Classes.IO.SerialIO.IAIRoboCylinder">IAIRoboCylinder</see>
        /// associated with the form
        /// </summary>
        public IAIRoboCylinder RoboCylinder
        {
            get { return _roboCylinder; }
            set
            {
                _roboCylinder = value;
                _roboCylinder.ValueChanged += new EventHandler(_roboCylinder_ValueChanged);
            }
        }

        private void _roboCylinder_ValueChanged(object sender, EventArgs e)
        {
            toolStripStatusLabelPosition.Text = "Position: " + _roboCylinder.FormattedValue + " " + _roboCylinder.Units;
            toolStripStatusLabelPower.Text = "Power ON";
            toolStripStatusLabelServo.Text = "Server " + (_roboCylinder.Status.Servo ? "ON" : "OFF");
            toolStripStatusLabelReady.Text = (_roboCylinder.Status.Run ? "" : "NOT") + "Ready to Move";
            toolStripStatusLabelCommand.Text = "Command" + (_roboCylinder.Status.CommandRefused ? "Refused" : "OK");
            if (_roboCylinder.Status.Alarms == 0)
                toolStripStatusLabelAlarm.Text = "No Alarm";
            else
                toolStripStatusLabelAlarm.Text = String.Format("Alarm: {0:X2}", _roboCylinder.Status.Alarms);
        }

        private void buttonPositionMove_Click(object sender, EventArgs e)
        {
            try
            {
                _roboCylinder.MovePosition(Convert.ToByte(numericUpDownPosNum.Value));
            }
            catch
            {
                MessageBox.Show("An error occured, check the value.", "RoboCylinder Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonHome_Click(object sender, EventArgs e)
        {
            _roboCylinder.MoveHome();
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            _roboCylinder.StopMotion();
        }

        private void buttonAbsMove_Click(object sender, EventArgs e)
        {
            try
            {
                _roboCylinder.MoveAbsolute(Convert.ToSingle(textBoxPosition.Text));
            }
            catch
            {
                MessageBox.Show("An error occured, check the value.", "RoboCylinder Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonIncMove_Click(object sender, EventArgs e)
        {
            try
            {
                _roboCylinder.MoveIncremental(Convert.ToSingle(textBoxIncPos.Text));
            }
            catch
            {
                MessageBox.Show("An error occured, check the value.", "RoboCylinder Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonLoadPosData_Click(object sender, EventArgs e)
        {
            try
            {
                _roboCylinder.SetUpPositionData(
                    Convert.ToByte(numericUpDownSetPosNum.Value),
                    Convert.ToSingle(textBoxSetPosition.Text),
                    Convert.ToSingle(textBoxSetVelocity.Text),
                    Convert.ToSingle(textBoxSetAccel.Text),
                    Convert.ToByte(textBoxSetPushPercent.Text),
                    Convert.ToByte(numericUpDownSetPushTime.Value),
                    Convert.ToSingle(textBoxSetPosBand.Text),
                    checkBoxSetMaxAccel.Checked);
            }
            catch
            {
                MessageBox.Show("An error occured, check the position data.", "RoboCylinder Control", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void buttonServoOn_Click(object sender, EventArgs e)
        {
            _roboCylinder.ServoOn();
        }

        private void buttonServoOff_Click(object sender, EventArgs e)
        {
            _roboCylinder.ServoOff();
        }

        private void IAIRoboCylinderManualControlForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }
    }
}