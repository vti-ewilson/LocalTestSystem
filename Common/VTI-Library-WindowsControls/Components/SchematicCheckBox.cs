using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;
using VTIWindowsControlLibrary.Classes.Util;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Represents a check box to be used on a schematic form.
    /// </summary>
    public partial class SchematicCheckBox : Panel
    {
        #region Nested Classes

        /// <summary>
        /// Event arguments for the CheckChanging event of the SchematicCheckBox
        /// </summary>
        public class CheckChangingEventArgs
        {
            /// <summary>
            /// The <see cref="IDigitalIO">Digital I/O</see> being changed.
            /// </summary>
            public IDigitalIO DigitalIO { get; private set; }

            /// <summary>
            /// The current state of the Digital I/O
            /// </summary>
            public Boolean CurrentState { get; private set; }

            /// <summary>
            /// The new state of the Digital I/O
            /// </summary>
            public Boolean NewState { get; private set; }

            /// <summary>
            /// A value that the event handler can set to true to cancel the change.
            /// </summary>
            public Boolean Cancel { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="CheckChangingEventArgs">CheckChangingEventArgs</see> class
            /// </summary>
            /// <param name="digitalIO">The <see cref="IDigitalIO">Digital I/O</see> being changed.</param>
            /// <param name="currentState">The current state of the Digital I/O</param>
            /// <param name="newState">The new state of the Digital I/O</param>
            public CheckChangingEventArgs(IDigitalIO digitalIO, Boolean currentState, Boolean newState)
            {
                DigitalIO = digitalIO;
                CurrentState = currentState;
                NewState = newState;
                Cancel = false;
            }
        }

        #endregion Nested Classes

        #region Delegates

        private delegate void SetCheckBoxValueCallback(Boolean Value);

        /// <summary>
        /// Event handler for when the SchematicCheckBox checked state is changing.
        /// </summary>
        /// <remarks>
        /// An event hander can set the Cancel property to true to cancel the change.  If you
        /// want to cancel a digital output change only in schematic mode (not automatic operations
        /// or the Digital I/O Form), use this event handler.  If you want to be able to cancel
        /// the digital output change globally, use the <see cref="Classes.IO.Interfaces.IDigitalOutput.ValueChanging">ValueChanging</see>
        /// event of the <see cref="Classes.IO.Interfaces.IDigitalOutput">Digital Output</see>.
        /// </remarks>
        /// <param name="sender">The <see cref="SchematicCheckBox">SchematicCheckBox</see> whose checked state is changing.</param>
        /// <param name="e">Event arguments</param>
        public delegate void CheckedUserChangingEventHandler(SchematicCheckBox sender, CheckChangingEventArgs e);

        #endregion Delegates

        #region Events

        /// <summary>
        /// Occurs when the <see cref="Checked">Checked</see> property changes because the underlying Digital I/O changed.
        /// </summary>
        public event EventHandler CheckedChanged;

        /// <summary>
        /// Raises the CheckChanged event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnCheckedChanged(EventArgs e)
        {
            if (CheckedChanged != null)
                CheckedChanged(this, e);
        }

        /// <summary>
        /// Occurs when the <see cref="Checked">Checked</see> property changes because the user clicked the button.
        /// </summary>
        public event EventHandler CheckedUserChanged;

        /// <summary>
        /// Raises the <see cref="CheckedUserChanged">CheckUserChanged</see> event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnCheckedUserChanged(EventArgs e)
        {
            if (CheckedUserChanged != null)
                if (!Locked)
                    CheckedUserChanged(this, e);
        }

        /// <summary>
        /// Occurs when the <see cref="Checked">Checked</see> property is about to change because the user clicked the button.
        /// </summary>
        public event CheckedUserChangingEventHandler CheckedUserChanging;

        /// <summary>
        /// Raises the <see cref="CheckedUserChanging">CheckedUserChanging</see> event.
        /// </summary>
        /// <param name="e">Event arguments</param>
        protected virtual void OnCheckedUserChanging(CheckChangingEventArgs e)
        {
            if (CheckedUserChanging != null)
                CheckedUserChanging(this, e);
        }

        #endregion Events

        #region Globals

        private CheckBox _CheckBox;
        private string _Text = string.Empty;
        private ToolTip _ToolTip;
        private Boolean _AutoSize = true;
        private Size _OriginalSize;
        private Point _OriginalLocation;
        private IDigitalIO _DigitalIO;
        private string _DigitalIOName;
        private TransparentPanel _TransparentPanel;
        private Boolean _Locked = false;
        private Boolean _ReverseStates = false;
        private Color _IndicatorColor = Color.Yellow;
        private HSL _InputBorder;
        private Boolean _DisplayAsIndicator = false;
        private Pen _IndicatorPen;
        private Brush _IndicatorBrush;
        private Brush _IndicatorBrushDim;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicCheckBox">SchematicCheckBox</see>
        /// </summary>
        public SchematicCheckBox()
        {
            InitializeComponent();

            _InputBorder = HSL.FromRGB(_IndicatorColor);
            _InputBorder.Luminance = _InputBorder.Luminance * 0.5F; // 50% as bright
            _IndicatorPen = new Pen(_InputBorder.RGB);
            _IndicatorBrush = new SolidBrush(_IndicatorColor);
            _IndicatorBrushDim = new SolidBrush(_InputBorder.RGB);

            this.Size = new Size(40, 40);
            _CheckBox = new CheckBox();
            this.Controls.Add(_CheckBox);
            //checkBox.SendToBack();
            _CheckBox.Location = new System.Drawing.Point(0, 0);
            _CheckBox.Size = this.Size;
            _CheckBox.Appearance = Appearance.Button;
            if ((_DigitalIO != null) && (_DigitalIO.IsInput))
                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.InputOff.ToBitmap();
            else
                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.Red_X;
            _CheckBox.CheckedChanged += new EventHandler(checkBox_CheckedChanged);
            _CheckBox.Click += new EventHandler(checkBox_Click);
            _CheckBox.MouseEnter += new EventHandler(checkBox_MouseEnter);
            _CheckBox.MouseLeave += new EventHandler(checkBox_MouseLeave);
            this.SizeChanged += new EventHandler(VtiCheckBox_SizeChanged);
            _ToolTip = new ToolTip();
            _TransparentPanel = new TransparentPanel();
            this.Controls.Add(_TransparentPanel);
            _TransparentPanel.Location = new Point(0, 0);
            _TransparentPanel.Size = this.Size;
            _TransparentPanel.SendToBack();
        }

        #endregion Construction

        #region Private Methods

        private void SetImage()
        {
            if (this.DesignMode)
                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.Red_X;
            else
            {
                if (_CheckBox.Appearance == Appearance.Button)
                {
                    if (_DigitalIO == null)
                        _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.large_circle_slash;
                    else
                      if (_CheckBox.Checked)
                    {
                        if (_DigitalIO.IsInput || _DisplayAsIndicator)
                        {
                            try
                            {
                                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.InputOn.ToBitmap();
                                if (_DisplayAsIndicator)
                                {
                                    Graphics g = Graphics.FromImage(_CheckBox.Image);
                                    g.FillEllipse(_IndicatorBrush, 1, 1, 29, 29);
                                    g.DrawEllipse(_IndicatorPen, 1, 1, 29, 29);
                                }
                            }
                            catch (Exception ex)
                            {
                                VtiEvent.Log.WriteError(ex.Message + Environment.NewLine + ex.StackTrace);
                            }
                        }
                        else
                        {
                            if (!this.ReverseStates)
                                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.GreenCheck;
                            else
                                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.Red_X;
                        }
                    }
                    else
                    {
                        if (_DigitalIO.IsInput || _DisplayAsIndicator)
                        {
                            _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.InputOff.ToBitmap();
                            if (_DisplayAsIndicator)
                            {
                                Graphics g = Graphics.FromImage(_CheckBox.Image);
                                g.FillEllipse(_IndicatorBrushDim, 1, 1, 29, 29);
                                g.DrawEllipse(_IndicatorPen, 1, 1, 29, 29);
                            }
                        }
                        else
                        {
                            if (!this.ReverseStates)
                                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.Red_X;
                            else
                                _CheckBox.Image = VTIWindowsControlLibrary.Properties.Resources.GreenCheck;
                        }
                    }
                }
                else
                    _CheckBox.Image = null;
            }
        }

        private void SetSize()
        {
            int iSize;

            if (_AutoSize)
            {
                if (_CheckBox.Appearance == Appearance.Button)
                {
                    if (this.Height >= 40 && this.Height < 90 && this.Width >= 70)
                    {
                        _CheckBox.Text = _Text;
                        _CheckBox.Padding = new Padding(0, 0, 0, 0);
                        _CheckBox.ImageAlign = ContentAlignment.MiddleLeft;
                        _CheckBox.TextAlign = ContentAlignment.MiddleRight;
                        if (this.Width >= 150)
                            _CheckBox.Size = new Size(150, 60);
                        else if (this.Width >= 120)
                            _CheckBox.Size = new Size(120, 50);
                        else if (Width >= 100)
                            _CheckBox.Size = new Size(110, 40);
                        else if (Width >= 90)
                            _CheckBox.Size = new Size(90, 40);
                        else
                            _CheckBox.Size = new Size(70, 40);
                    }
                    else
                    {
                        iSize = (this.Width > this.Height ? this.Height : this.Width);
                        if (iSize >= 90)
                        {
                            _CheckBox.Size = new Size(90, 90);
                            _CheckBox.Text = _Text;
                            _CheckBox.Padding = new Padding(0, 6, 0, 6);
                            _CheckBox.ImageAlign = ContentAlignment.TopCenter;
                            _CheckBox.TextAlign = ContentAlignment.BottomCenter;
                        }
                        else if (iSize >= 40)
                        {
                            _CheckBox.Size = new Size(40, 40);
                            _CheckBox.Text = string.Empty;
                            _CheckBox.Padding = new Padding(0, 0, 0, 0);
                            _CheckBox.ImageAlign = ContentAlignment.MiddleCenter;
                        }
                        else if (iSize >= 30)
                        {
                            _CheckBox.Size = new Size(30, 30);
                            _CheckBox.Text = string.Empty;
                            _CheckBox.Padding = new Padding(0, 0, 0, 0);
                            _CheckBox.ImageAlign = ContentAlignment.MiddleCenter;
                        }
                        else// if (iSize >= 22)
                        {
                            _CheckBox.Size = new Size(22, 22);
                            _CheckBox.Text = string.Empty;
                            _CheckBox.Padding = new Padding(0, 0, 0, 0);
                            _CheckBox.ImageAlign = ContentAlignment.MiddleCenter;
                        }
                    }
                }
                this.Size = _CheckBox.Size;
            }
            else
                _CheckBox.Size = this.Size;
            _TransparentPanel.Size = this.Size;
        }

        private void SetCheckBoxValue(Boolean Value)
        {
            if (_CheckBox.InvokeRequired)
            {
                SetCheckBoxValueCallback d = new SetCheckBoxValueCallback(SetCheckBoxValue);
                _CheckBox.Invoke(d, Value);
            }
            else
            {
                _CheckBox.Checked = Value;
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void checkBox_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
        }

        private void checkBox_MouseLeave(object sender, EventArgs e)
        {
            _ToolTip.Active = false;
        }

        private void checkBox_MouseEnter(object sender, EventArgs e)
        {
            _ToolTip.Active = true;
        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            if (_DigitalIO != null)
            {
                // If values differ at this point, it is because user clicked the button
                if (_CheckBox.Checked != _DigitalIO.Value)
                {
                    if (_DigitalIO.IsInput)
                    {
                        _CheckBox.Checked = _DigitalIO.Value;
                    }
                    else
                    {
                        CheckChangingEventArgs cce = new CheckChangingEventArgs(_DigitalIO, _DigitalIO.Value, !_DigitalIO.Value);
                        this.OnCheckedUserChanging(cce);
                        // If client app didn't cancel the change, then pass the value on to the digital output
                        if (!cce.Cancel)
                        {
                            try
                            {
                                (_DigitalIO as IDigitalOutput).Value = _CheckBox.Checked;
                                this.OnCheckedUserChanged(e);
                            }
                            catch (DigitalOutputChangeCanceledException canceledException)
                            {
                                _CheckBox.Checked = _DigitalIO.Value;
                                MessageBox.Show(canceledException.Reason, VtiLibLocalization.SchematicWarning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                        // If client app canceled the change, reset the checkbox
                        else
                            _CheckBox.Checked = _DigitalIO.Value;
                    }
                }
                // If values are the same at this point, the Digital I/O changed, which is causing the checkbox to change
                else
                    this.OnCheckedChanged(e);

                this.SetImage();
            }
            else
            {
                this.SetImage();
                this.OnCheckedChanged(e);
            }
        }

        private void VtiCheckBox_SizeChanged(object sender, EventArgs e)
        {
            this.SetSize();
        }

        private void _digitalIO_ValueChanged(object sender, EventArgs e)
        {
            IDigitalIO digio = sender as IDigitalIO;
            this.SetCheckBoxValue(digio.Value);
        }

        #endregion Event Handlers

        #region Public Properties

        /// <summary>
        /// Gets or sets a value to indicate if the control resizes based on its contents.
        /// </summary>
        public new Boolean AutoSize
        {
            get { return _AutoSize; }
            set
            {
                _AutoSize = value;
                if (_CheckBox.Appearance == Appearance.Normal)
                    _CheckBox.AutoSize = _AutoSize;
                else
                    _CheckBox.AutoSize = false;
                this.SetSize();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate whether the control is in its checked state.
        /// </summary>
        public Boolean Checked
        {
            get { return _CheckBox.Checked; }
            set { _CheckBox.Checked = value; }
        }

        /// <summary>
        /// Gets the appearance of the border and the colors used to indicate the check state and mouse state.
        /// </summary>
        public FlatButtonAppearance FlatAppearance
        {
            get { return _CheckBox.FlatAppearance; }
        }

        /// <summary>
        /// Gets or sets the flat appearance of the button control.
        /// </summary>
        public FlatStyle FlatStyle
        {
            get { return _CheckBox.FlatStyle; }
            set { _CheckBox.FlatStyle = value; }
        }

        /// <summary>
        /// Gets or sets the value that determines the appearance of a
        /// <see cref="System.Windows.Forms.CheckBox">System.Windows.Forms.CheckBox</see> control.
        /// </summary>
        public Appearance Appearance
        {
            get { return _CheckBox.Appearance; }
            set
            {
                _CheckBox.Appearance = value;
                this.SetImage();
                if (_CheckBox.Appearance == Appearance.Normal)
                    _CheckBox.AutoSize = _AutoSize;
                else
                    _CheckBox.AutoSize = false;
                this.SetSize();
            }
        }

        /// <summary>
        /// Gets or sets the original size of the SchematicCheckBox. Used in resizing the schematic form.
        /// </summary>
        [Browsable(false)]
        public Size OriginalSize
        {
            get { return _OriginalSize; }
            set { _OriginalSize = value; }
        }

        /// <summary>
        /// Gets or sets the original location of the SchematicCheckBox. Used in resizing the schematic form.
        /// </summary>
        [Browsable(false)]
        public Point OriginalLocation
        {
            get { return _OriginalLocation; }
            set { _OriginalLocation = value; }
        }

        /// <summary>
        /// Gets or sets the name of the Digital I/O point associated with this SchematicCheckBox.
        /// When set, the control tries to locate the I/O point in the IO class of the client
        /// application.
        /// </summary>
        public string DigitalIOName
        {
            get { return _DigitalIOName; }
            set
            {
                _DigitalIOName = value;
                if (this.DesignMode)
                {
                    _Text = _DigitalIOName;
                }
                else
                {
                    if (_DigitalIOName != null && VtiLib.IO.DigitalInputs.ContainsKey(_DigitalIOName))
                    {
                        _DigitalIO = VtiLib.IO.DigitalInputs[_DigitalIOName];
                        _Locked = true;
                        _TransparentPanel.BringToFront();    // cover the checkBox with a transparent panel, to make it un-clickable
                        _Text = _DigitalIO.Description;
                        _ToolTip.SetToolTip(_TransparentPanel, _Text);
                    }
                    else if (_DigitalIOName != null && VtiLib.IO.DigitalOutputs.ContainsKey(_DigitalIOName))
                    {
                        _DigitalIO = VtiLib.IO.DigitalOutputs[_DigitalIOName];
                        _Locked = false;
                        _TransparentPanel.SendToBack();
                        _Text = _DigitalIO.Description;
                        _ToolTip.SetToolTip(_CheckBox, _Text);
                    }
                    else
                    {
                        this.Visible = false;
                    }

                    if (_DigitalIO != null)
                    {
                        _DigitalIO.ValueChanged += new EventHandler(_digitalIO_ValueChanged);
                        this.SetCheckBoxValue(_DigitalIO.Value);
                    }
                    this.SetImage();
                }
                if (_CheckBox.Width >= 90) _CheckBox.Text = _Text;
                else _CheckBox.Text = string.Empty;
            }
        }

        /// <summary>
        /// Gets or sets the <see cref="IDigitalIO">IDigitalIO</see> associated with this
        /// <see cref="SchematicCheckBox">SchematicCheckBox</see>
        /// </summary>
        /// <remarks>
        /// This value should only be set for digital I/O that occurs outside of the
        /// normal I/O, such as digital I/O associated with a serial device.
        /// </remarks>
        /// <example>
        /// The following code is an example of using the DigitalIO property to tie
        /// schematic check boxes to the digital inputs from some Solo Temperature Controllers.
        /// <code>
        ///    schematicCheckBoxTempControllerOutput.DigitalIO =
        ///        IO.SerialIn.TemperatureControl.DigitalInputs.Output1;
        ///    schematicCheckBoxTempControllerOutput.Text = "Heater Output";
        ///
        ///    schematicCheckBoxOverTempAlarm.DigitalIO =
        ///        IO.SerialIn.OverTempControl.DigitalInputs.Alarm1;
        ///    schematicCheckBoxOverTempAlarm.Text = "Over-Temperature Alarm";
        /// </code>
        /// </example>
        public IDigitalIO DigitalIO
        {
            get { return _DigitalIO; }
            set
            {
                _DigitalIO = value;
                if (_DigitalIO != null)
                {
                    if (_DigitalIO.IsInput)
                    {
                        _Locked = true;
                        _TransparentPanel.BringToFront();    // cover the checkBox with a transparent panel, to make it un-clickable
                        _Text = _DigitalIO.Description;
                        _ToolTip.SetToolTip(_TransparentPanel, _Text);
                    }
                    else
                    {
                        _Locked = false;
                        _TransparentPanel.SendToBack();
                        _Text = _DigitalIO.Description;
                        _ToolTip.SetToolTip(_CheckBox, _Text);
                    }
                    _DigitalIO.ValueChanged += new EventHandler(_digitalIO_ValueChanged);
                    this.SetCheckBoxValue(_DigitalIO.Value);
                    this.SetImage();
                    if (_CheckBox.Width >= 90) _CheckBox.Text = _Text;
                    else _CheckBox.Text = string.Empty;
                    this.Visible = true;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate if the control should be locked (unclickable)
        /// </summary>
        public Boolean Locked
        {
            get { return _Locked; }
            set
            {
                _Locked = value;
                if (_Locked) _TransparentPanel.BringToFront();
                else _TransparentPanel.SendToBack();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate that the Schematic Check Box should display its state reversed from that of the Digital I/O
        /// </summary>
        /// <remarks>
        /// This can be used, for instance, in the case of a Normally Open Valve.
        /// </remarks>
        public Boolean ReverseStates
        {
            get { return _ReverseStates; }
            set
            {
                _ReverseStates = value;
                SetImage();
            }
        }

        /// <summary>
        /// Gets or sets the text of the control.
        /// </summary>
        public new String Text
        {
            get { return _CheckBox.Text; }
            set { _CheckBox.Text = value; }
        }

        /// <summary>
        /// If <see cref="DisplayAsIndicator">DisplayAsIndicator</see> is set to true, this
        /// property gets or sets the color of the indicator light.
        /// </summary>
        public Color IndicatorColor
        {
            get { return _IndicatorColor; }
            set
            {
                _IndicatorColor = value;
                _InputBorder = HSL.FromRGB(_IndicatorColor);
                _InputBorder.Luminance = _InputBorder.Luminance * 0.5F; // 50% as bright
                _IndicatorPen = new Pen(_InputBorder.RGB);
                _IndicatorBrush = new SolidBrush(_IndicatorColor);
                _IndicatorBrushDim = new SolidBrush(_InputBorder.RGB);
                SetImage();
            }
        }

        /// <summary>
        /// Gets or sets a value to indicate that the SchematicCheckBox should be displayed
        /// as a solid, round, "indicator" light even if the I/O is an output.  This can be used,
        /// for instance, to cause light stack outputs to appear as lights instead of check boxes.
        /// </summary>
        public Boolean DisplayAsIndicator
        {
            get { return _DisplayAsIndicator; }
            set
            {
                _DisplayAsIndicator = value;
                SetImage();
            }
        }

        #endregion Public Properties

        #region Public Methods

        /// <summary>
        /// Resets the locked state to its original value
        /// </summary>
        public void ResetLockedState()
        {
            if (_DigitalIO != null)
            {
                if (_DigitalIO.IsInput) this.Locked = true;
                else this.Locked = false;
            }
        }

        #endregion Public Methods
    }
}