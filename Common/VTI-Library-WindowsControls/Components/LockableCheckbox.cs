using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Implements a version of a <see cref="CheckBox">CheckBox</see> control that
    /// can be locked (i.e. not clickable, yet not greyed out).
    /// </summary>
    public partial class LockableCheckbox : Panel
    {
        #region Globals

        private TransparentPanel transparentPanel;
        private CheckBox checkBox;
        private Boolean _locked = true;
        private Action<bool> setCheckedCallback;
        private Action<bool> setLockedCallback;

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="LockableCheckbox">LockableCheckbox</see> class
        /// </summary>
        public LockableCheckbox()
        {
            InitializeComponent();

            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10);
            this.Size = new System.Drawing.Size(80, 17);
            transparentPanel = new TransparentPanel();
            this.Controls.Add(transparentPanel);
            transparentPanel.Location = new System.Drawing.Point(0, 0);
            transparentPanel.Size = this.Size;
            checkBox = new CheckBox();
            this.Controls.Add(checkBox);
            checkBox.SendToBack();
            checkBox.Location = new System.Drawing.Point(0, 0);
            checkBox.Size = this.Size;
            checkBox.AutoSize = true;
            checkBox.Text = "this is a test checkbox";
            checkBox.SizeChanged += new EventHandler(checkBox_SizeChanged);
            this.SizeChanged += new EventHandler(LockedCheckbox_SizeChanged);
            setCheckedCallback = new Action<bool>(SetChecked);
            setLockedCallback = new Action<bool>(SetLocked);
        }

        #endregion Construction

        #region Events

        private void LockedCheckbox_SizeChanged(object sender, EventArgs e)
        {
            if (checkBox.AutoSize == false)
            {
                checkBox.Size = this.Size;
                transparentPanel.Size = this.Size;
            }
        }

        private void checkBox_SizeChanged(object sender, EventArgs e)
        {
            if (checkBox.AutoSize == true)
            {
                this.Size = checkBox.Size;
                transparentPanel.Size = this.Size;
            }
        }

        #endregion Events

        #region Public Properties

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="CheckBox">CheckBox</see>
        /// is in the checked state.
        /// </summary>
        public bool Checked
        {
            get { return checkBox.Checked; }
            set { checkBox.Checked = value; }
        }

        /// <summary>
        /// Gets or sets the state of the <see cref="CheckBox">CheckBox</see>.
        /// </summary>
        public CheckState CheckState
        {
            get { return checkBox.CheckState; }
            set { checkBox.CheckState = value; }
        }

        /// <summary>
        /// Gets or sets the text of the <see cref="CheckBox">CheckBox</see>.
        /// </summary>
        [Browsable(true)]
        public new String Text
        {
            get { return checkBox.Text; }
            set { checkBox.Text = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the control resizes based on its contents.
        /// </summary>
        public new Boolean AutoSize
        {
            get { return checkBox.AutoSize; }
            set { checkBox.AutoSize = value; }
        }

        /// <summary>
        /// Gets or sets the value that determines the appearance of the control.
        /// </summary>
        public Appearance Appearance
        {
            get { return checkBox.Appearance; }
            set
            {
                checkBox.Appearance = value;
                //this.SetImage();
                //if (checkBox.Appearance == Appearance.Normal)
                //    checkBox.AutoSize = _autoSize;
                //else
                //    checkBox.AutoSize = false;
                //this.SetSize();
            }
        }

        /// <summary>
        /// Gets or sets the alignment of the text of the control
        /// </summary>
        public System.Drawing.ContentAlignment TextAlign
        {
            get { return checkBox.TextAlign; }
            set { checkBox.TextAlign = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the control can be locked
        /// </summary>
        /// <remarks>
        /// If the control is locked, the appearance will remain the same, but it will not be clickable.
        /// </remarks>
        public Boolean Locked
        {
            get { return _locked; }
            set
            {
                _locked = value;
                if (_locked) transparentPanel.BringToFront();
                else transparentPanel.SendToBack();
            }
        }

        #endregion Public Properties

        /// <summary>
        /// Sets the checked state of the check box.
        /// </summary>
        /// <param name="value">if set to <c>true</c>, the check box will be checked.</param>
        public void SetChecked(bool value)
        {
            if (this.InvokeRequired) this.Invoke(setCheckedCallback, value);
            else this.Checked = value;
        }

        /// <summary>
        /// Sets the locked state of the check box.
        /// </summary>
        /// <param name="value">if set to <c>true</c>, the check box will be locked.</param>
        public void SetLocked(bool value)
        {
            if (this.InvokeRequired) this.Invoke(setLockedCallback, value);
            else this.Locked = value;
        }
    }
}