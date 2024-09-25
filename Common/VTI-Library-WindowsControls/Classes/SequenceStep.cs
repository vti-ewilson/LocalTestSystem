using System;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Classes
{
    /// <summary>
    /// Represents a Sequence Step Indicator on the Operator Form
    /// </summary>
    public class SequenceStep
    {
        private String _Text;
        private Color _foreColor;
        private Color _backColor;

        /// <summary>
        /// Occurs when any property of the Sequence Step changes
        /// </summary>
        public event EventHandler StepChanged;

        /// <summary>
        /// Raises the <see cref="StepChanged">StepChanged</see> event
        /// </summary>
        protected virtual void OnStepChanged()
        {
            if (StepChanged != null) StepChanged(this, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceStep">SequenceStep</see> class
        /// </summary>
        /// <param name="Text">Text to be displayed for the Sequence Step</param>
        public SequenceStep(String Text)
        {
            _Text = Text;
            _foreColor = Color.FromKnownColor(KnownColor.ControlText);
            _backColor = Color.FromKnownColor(KnownColor.Control);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SequenceStep">SequenceStep</see> class
        /// </summary>
        /// <param name="Text">Text to be displayed for the Sequence Step</param>
        /// <param name="ForeColor">Foreground color of the Sequence Step</param>
        /// <param name="BackColor">Background color of the Sequence Step</param>
        public SequenceStep(String Text, Color ForeColor, Color BackColor)
        {
            _Text = Text;
            _foreColor = ForeColor;
            _backColor = BackColor;
        }

        /// <summary>
        /// Gets or sets the text to be displayed in the sequence step
        /// </summary>
        public String Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
                OnStepChanged();
            }
        }

        /// <summary>
        /// Gets or sets the foreground color of the sequence step
        /// </summary>
        public Color ForeColor
        {
            get { return _foreColor; }
            set
            {
                _foreColor = value;
                OnStepChanged();
            }
        }

        /// <summary>
        /// Gets or sets the background color of the sequence step
        /// </summary>
        public Color BackColor
        {
            get { return _backColor; }
            set
            {
                _backColor = value;
                OnStepChanged();
            }
        }

        internal Label Label { get; set; }
    }
}