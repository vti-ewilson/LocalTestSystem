using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Components;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Form for displaying Digital I/O's and allowing the user to turn them on and off via checkboxes.
    /// </summary>
    public partial class DigitalIOForm : Form
    {
        #region Delegates

        //delegate void LockedCheckBoxCallback(LockableCheckbox lockedCheckBox, Boolean Value);

        #endregion Delegates

        #region Globals

        //private LockedCheckBoxCallback _SetLockedCheckboxCallback;
        //private LockedCheckBoxCallback _SetLockedCheckboxStateCallback;
        private Boolean _active;

        private bool bIsToggledCheckBox;
        private static bool bSetToManualMode;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalIOForm">DigitalIOForm</see> class
        /// </summary>
        public DigitalIOForm()
        {
            InitializeComponent();
            //_SetLockedCheckboxCallback = new LockedCheckBoxCallback(SetLockedCheckbox);
            //_SetLockedCheckboxStateCallback = new LockedCheckBoxCallback(SetLockedCheckboxState);
        }

        #endregion Construction

        #region Events

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void DigitalIOForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        private void DigitalIOForm_Load(object sender, EventArgs e)
        {
            LockableCheckbox lockableCheckbox;
            //CheckBox checkBox;
            int longestDin = 0;
            int textHeight = 0;

            foreach (IDigitalInput din in VtiLib.IO.DigitalInputs.Values.OrderBy(I => I.Description))
            {
                if (din.Description != string.Empty)
                {
                    lockableCheckbox = new LockableCheckbox();
                    lockableCheckbox.Name = "chkbox" + din.Name;
                    lockableCheckbox.AutoSize = true;
                    lockableCheckbox.Text = din.Description;
                    lockableCheckbox.Checked = din.Value;
                    lockableCheckbox.Margin = new Padding(3, 0, 3, 0);
                    textHeight = TextRenderer.MeasureText(lockableCheckbox.Text, lockableCheckbox.Font).Height;
                    longestDin = Math.Max(longestDin, lockableCheckbox.Size.Width);
                    din.ValueChanged += new EventHandler(din_ValueChanged);
                    //din.ValueChanged += delegate
                    //{
                    //    SetLockedCheckbox(lockedCheckbox, din.Value);
                    //};

                    this.flowLayoutPanelDigitalInputs.Controls.Add(lockableCheckbox);
                    lockableCheckbox.SendToBack();
                }
            }

            foreach (Control i in flowLayoutPanelDigitalInputs.Controls)
            {
                i.Width = longestDin;
            }

            int longestDout = 0;

            foreach (IDigitalOutput dout in VtiLib.IO.DigitalOutputs.Values.OrderBy(O => O.Description))
            {
                if (dout.Description != string.Empty)
                {
                    lockableCheckbox = new LockableCheckbox();
                    lockableCheckbox.Name = "chkbox" + dout.Name;
                    lockableCheckbox.AutoSize = true;
                    lockableCheckbox.Text = dout.Description;
                    lockableCheckbox.Checked = dout.Value;
                    lockableCheckbox.Margin = new Padding(3, 0, 3, 0);
                    textHeight = TextRenderer.MeasureText(lockableCheckbox.Text, lockableCheckbox.Font).Height;
                    lockableCheckbox.Tag = dout;
                    longestDout = Math.Max(longestDout, lockableCheckbox.Size.Width);
                    dout.ValueChanged += new EventHandler(dout_ValueChanged);
                    //dout.ValueChanged += delegate
                    //{
                    //    if (checkBox.Checked != dout.Value)
                    //        SetCheckbox(checkBox, dout.Value);
                    //};

                    this.flowLayoutPanelDigitalOutputs.Controls.Add(lockableCheckbox);
                    lockableCheckbox.SendToBack();
                }
            }

            foreach (Control i in flowLayoutPanelDigitalOutputs.Controls)
            {
                i.Width = longestDout;
            }

            //set form width based on length of longest Digital Input and Digital Output
            if (longestDin != 0 && longestDout != 0)
            {
                this.Width = longestDin + longestDout + 180;
            }
            this.splitContainer1.SplitterDistance = longestDin + 20;
            //set height of form based on number of inputs or outputs (whichever is greater) times the height of the text
            int maxFormHeight = 550;
            if (textHeight != 0)
            {
                int inputsHeight = textHeight * flowLayoutPanelDigitalInputs.Controls.Count;
                int outputsHeight = textHeight * flowLayoutPanelDigitalOutputs.Controls.Count;
                int targetHeight = (outputsHeight > inputsHeight ? outputsHeight : inputsHeight) + 170;
                if (targetHeight < maxFormHeight)
                {
                    this.Height = targetHeight;
                }
                else
                {
                    this.Height = maxFormHeight;
                }
            }
        }

        private void din_ValueChanged(object sender, EventArgs e)
        {
            IDigitalInput din = sender as IDigitalInput;
            LockableCheckbox lockableCheckBox = this.flowLayoutPanelDigitalInputs.Controls["chkbox" + din.Name] as LockableCheckbox;
            lockableCheckBox.SetChecked(din.Value);
            bIsToggledCheckBox = true;
            //SetLockedCheckbox(lockableCheckBox, din.Value);
        }

        private void dout_ValueChanged(object sender, EventArgs e)
        {
            IDigitalOutput dout = sender as IDigitalOutput;
            LockableCheckbox lockableCheckBox = this.flowLayoutPanelDigitalOutputs.Controls["chkbox" + dout.Name] as LockableCheckbox;
            if (lockableCheckBox.Checked != dout.Value)
                //SetLockedCheckbox(lockableCheckBox, dout.Value);
                lockableCheckBox.SetChecked(dout.Value);
            bIsToggledCheckBox = true;
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (LockableCheckbox checkBox in flowLayoutPanelDigitalOutputs.Controls)
                {
                    (checkBox.Tag as IDigitalOutput).Value = checkBox.Checked;
                }
                if (bIsToggledCheckBox)
                    bSetToManualMode = true;
            }
            catch (DigitalOutputChangeCanceledException canceledException)
            {
                MessageBox.Show(canceledException.Reason, VtiLibLocalization.DigitalIOWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                // Reset the state of the checkboxes to match the outputs
                foreach (LockableCheckbox checkBox in flowLayoutPanelDigitalOutputs.Controls)
                {
                    checkBox.Checked = (checkBox.Tag as IDigitalOutput).Value;
                }
            }
        }

        //private void SetLockedCheckbox(LockableCheckbox lockedCheckBox, Boolean Value)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new Action<LockableCheckbox, Boolean>(SetLockedCheckbox), lockedCheckBox, Value);
        //    }
        //    else
        //    {
        //        lockedCheckBox.Checked = Value;
        //    }
        //}

        //private void SetLockedCheckboxState(LockableCheckbox lockedCheckBox, Boolean Value)
        //{
        //    if (this.InvokeRequired)
        //    {
        //        this.Invoke(new Action<LockableCheckbox, Boolean>(SetLockedCheckboxState), lockedCheckBox, Value);
        //    }
        //    else
        //    {
        //        lockedCheckBox.Locked = Value;
        //    }
        //}

        private void DigitalIOForm_Activated(object sender, EventArgs e)
        {
            if (_active)
            {
                this.Text = VtiLibLocalization.DigitalIOControl;
                this.buttonAccept.Visible = true;
                UnlockControls();
            }
            else
            {
                this.Text = VtiLibLocalization.DigitalIOControlInactive;
                this.buttonAccept.Visible = false;
                LockControls();
            }
            if (_active) UnlockControls();
            else LockControls();
            RefreshControls();
        }

        #endregion Events

        #region Private Members

        private void LockControls()
        {
            LockableCheckbox lockableCheckBox;
            foreach (IDigitalInput din in VtiLib.IO.DigitalInputs.Values)
            {
                if (din.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalInputs.Controls["chkbox" + din.Name] as LockableCheckbox;
                    //SetLockedCheckboxState(lockableCheckBox, true);
                    lockableCheckBox.SetLocked(true);
                }
            }
            foreach (IDigitalOutput dout in VtiLib.IO.DigitalOutputs.Values)
            {
                if (dout.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalOutputs.Controls["chkbox" + dout.Name] as LockableCheckbox;
                    //SetLockedCheckboxState(lockableCheckBox, true);
                    lockableCheckBox.SetLocked(true);
                }
            }
        }

        private void UnlockControls()
        {
            LockableCheckbox lockableCheckBox;
            foreach (IDigitalInput din in VtiLib.IO.DigitalInputs.Values)
            {
                if (din.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalInputs.Controls["chkbox" + din.Name] as LockableCheckbox;
                    //SetLockedCheckboxState(lockableCheckBox, true);
                    lockableCheckBox.SetLocked(true);
                }
            }
            foreach (IDigitalOutput dout in VtiLib.IO.DigitalOutputs.Values)
            {
                if (dout.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalOutputs.Controls["chkbox" + dout.Name] as LockableCheckbox;
                    //SetLockedCheckboxState(lockableCheckBox, false);
                    lockableCheckBox.SetLocked(false);
                }
            }
        }

        private void RefreshControls()
        {
            LockableCheckbox lockableCheckBox;
            foreach (IDigitalInput din in VtiLib.IO.DigitalInputs.Values)
            {
                if (din.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalInputs.Controls["chkbox" + din.Name] as LockableCheckbox;
                    //SetLockedCheckbox(lockableCheckBox, din.Value);
                    lockableCheckBox.SetChecked(din.Value);
                }
            }
            foreach (IDigitalOutput dout in VtiLib.IO.DigitalOutputs.Values)
            {
                if (dout.Description != string.Empty)
                {
                    lockableCheckBox = this.flowLayoutPanelDigitalOutputs.Controls["chkbox" + dout.Name] as LockableCheckbox;
                    //SetLockedCheckbox(lockableCheckBox, dout.Value);
                    lockableCheckBox.SetChecked(dout.Value);
                }
            }
        }

        #endregion Private Members

        #region Public Members

        /// <summary>
        /// Shows the Digital I/O form to the user
        /// </summary>
        /// <param name="Active">Indicates if the outputs on the Digital I/O form should be active</param>
        public void Show(Boolean Active)
        {
            _active = Active;
            this.Show();
        }

        /// <summary>
        /// Shows the Digital I/O form with the specified owner to the user
        /// </summary>
        /// <param name="owner">Represents the top-level window that will own this form</param>
        /// <param name="Active">Indicates if the outputs on the Digital I/O form should be active</param>
        public void Show(IWin32Window owner, Boolean Active)
        {
            _active = Active;
            this.Show(owner);
        }

        /// <summary>
        /// Get ManualMode to see whether changes to Digital IO should cause UI to toggle to Manual Mode
        /// Set ManualMode to cause DigitalIOForm to set bSetToManualMode = true or false
        /// </summary>
        public static bool ManualMode
        {
            get { return bSetToManualMode; }
            set { bSetToManualMode = value; }
        }

        #endregion Public Members

        private void DigitalInputsLabel_DoubleClick(object sender, EventArgs e)
        {
            ExportDigitalIO();
        }

        private void DigitalOutputsLabel_DoubleClick(object sender, EventArgs e)
        {
            ExportDigitalIO();
        }

        private void ExportDigitalIO()
        {
            var csv = new System.Text.StringBuilder();
            var comma = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;
            csv.AppendLine($"Direction{comma}Name{comma}Description");
            foreach (IDigitalInput din in VtiLib.IO.DigitalInputs.Values)
            {
                if (din.Description != string.Empty)
                {
                    csv.AppendLine($"Input{comma + din.Name + comma + din.Description}");
                }
            }
            foreach (IDigitalOutput dout in VtiLib.IO.DigitalOutputs.Values)
            {
                if (dout.Description != string.Empty)
                {
                    csv.AppendLine($"Output{comma + dout.Name + comma + dout.Description}");
                }
            }
            string folderName = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string filename = folderName + @"\" + String.Format("DigitalIO_{0}.csv", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-ff"));
            File.WriteAllText(filename, csv.ToString());
            MessageBox.Show("Digital IO exported to " + Environment.NewLine + filename, "Digital IO Exported",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
            try
            {
                System.Diagnostics.Process.Start(folderName);
            }
            catch (Exception ex)
            {
                string exc = ex.Message;
            }
        }
    }
}