using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Components
{
    /// <summary>
    /// Control to display a test history
    /// </summary>
    public partial class TestHistoryControl : UserControl
    {
        private Size _labelSize;
        private int _rows;
        private int _columns;

        //private delegate void AddEntryDelegate(String Text, Color ForeColor, Color BackColor);

        /// <summary>
        /// Initializes a new instance of the <see cref="TestHistoryControl">TestHistoryControl</see> class
        /// </summary>
        public TestHistoryControl()
        {
            _labelSize = new Size(140, 13);
            _rows = 10;
            _columns = 1;
            InitializeComponent();
            SetSize();
            SetLabels();
        }

        private void SetLabels()
        {
            Label label1;
            int i;

            // Add labels
            if (flowLayoutPanel1.Controls.Count < _rows * _columns)
            {
                for (i = flowLayoutPanel1.Controls.Count; i < _rows * _columns; i++)
                {
                    label1 = new Label();
                    label1.BackColor = System.Drawing.SystemColors.ControlLightLight;
                    label1.Margin = new System.Windows.Forms.Padding(3, 3, 3, 0);
                    label1.Name = "labelHistory" + i.ToString();
                    label1.Size = _labelSize;
                    label1.Text = string.Empty;
                    label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;

                    flowLayoutPanel1.Controls.Add(label1);
                }
            }

            // Set label visibility
            // If there are too many labels, just hide the extras.  So, if a Test History
            // control has different numbers of labels when docked vs undocked, nothing gets lost.
            for (i = 0; i < flowLayoutPanel1.Controls.Count; i++)
            {
                flowLayoutPanel1.Controls[i].Visible = (i < _rows * _columns ? true : false);
            }
        }

        private void SetSize()
        {
            this.Height = _rows * (_labelSize.Height + 3) + 3;
            this.Width = _columns * (_labelSize.Width + 6);
        }

        /// <summary>
        /// <see cref="Size">Size</see> of the labels to be displayed in the control
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public Size LabelSize
        {
            get { return _labelSize; }
            set
            {
                _labelSize = value;
                SetSize();
                foreach (Label label in flowLayoutPanel1.Controls)
                    label.Size = _labelSize;
                //flowLayoutPanel1.Controls.OfType<Label>().ToList().ForEach(L => L.Size = _labelSize);
            }
        }

        /// <summary>
        /// Number of rows of labels
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Rows
        {
            get { return _rows; }
            set
            {
                _rows = value;
                this.Height = _rows * (_labelSize.Height + 3);
                SetLabels();
            }
        }

        /// <summary>
        /// Number of columns of labels
        /// </summary>
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public int Columns
        {
            get { return _columns; }
            set
            {
                _columns = value;
                this.Width = _columns * (_labelSize.Width + 6);
                SetLabels();
            }
        }

        /// <summary>
        /// Adds an entry to the test history control
        /// </summary>
        /// <param name="Text">Text of the entry to be added</param>
        /// <remarks>
        /// <para>
        /// The foreground color will be black, and the background color will be a light grey color.
        /// </para>
        /// <para>
        /// Existing entries are always moved down to make room for the new entry at the top.
        /// If there are multiple columns, the entries are moved down the first column, then
        /// down the seconds column, etc.
        /// </para>
        /// </remarks>
        public void AddEntry(String Text)
        {
            AddEntry(Text, Color.Black, System.Drawing.SystemColors.ControlLightLight, "");
        }

        public void AddEntry(String Text, Color ForeColor, Color BackColor)
        {
            AddEntry(Text, ForeColor, BackColor, "");
        }

        /// <summary>
        /// Adds an entry to the test history control
        /// </summary>
        /// <param name="Text">Text of the entry to be added</param>
        /// <param name="ForeColor">Foreground color of the entry to be added</param>
        /// <param name="BackColor">Background color of the entry to be added</param>
        /// <param name="ToolTip2add">Text to be displayed when mouse hovers</param>
        /// <remarks>
        /// <para>
        /// Existing entries are always moved down to make room for the new entry at the top.
        /// If there are multiple columns, the entries are moved down the first column, then
        /// down the seconds column, etc.
        /// </para>
        /// </remarks>
        public void AddEntry(String Text, Color ForeColor, Color BackColor, String ToolTip2add)
        {
            if (this.InvokeRequired)
            {
                //AddEntryDelegate d = new AddEntryDelegate(AddEntry);
                this.Invoke(new Action<String, Color, Color, String>(AddEntry), Text, ForeColor, BackColor, ToolTip2add);
            }
            else
            {
                for (int i = flowLayoutPanel1.Controls.Count - 1; i >= 1; i--)
                {
                    ((Label)flowLayoutPanel1.Controls[i]).Text = ((Label)flowLayoutPanel1.Controls[i - 1]).Text;
                    ((Label)flowLayoutPanel1.Controls[i]).ForeColor = ((Label)flowLayoutPanel1.Controls[i - 1]).ForeColor;
                    ((Label)flowLayoutPanel1.Controls[i]).BackColor = ((Label)flowLayoutPanel1.Controls[i - 1]).BackColor;
                    string tempstr = toolTip1.GetToolTip((Label)flowLayoutPanel1.Controls[i - 1]);
                    toolTip1.SetToolTip((Label)flowLayoutPanel1.Controls[i], tempstr);
                }
              ((Label)flowLayoutPanel1.Controls[0]).Text = Text;
                ((Label)flowLayoutPanel1.Controls[0]).ForeColor = ForeColor;
                ((Label)flowLayoutPanel1.Controls[0]).BackColor = BackColor;
                toolTip1.SetToolTip((Label)flowLayoutPanel1.Controls[0], ToolTip2add);
            }
        }

        /// <summary>
        /// Clears the Test History
        /// </summary>
        public void ClearTestHistory()
        {
            for (int ii = 0; ii < flowLayoutPanel1.Controls.Count; ii++)
            {
                ((Label)flowLayoutPanel1.Controls[ii]).Text = "";
                ((Label)flowLayoutPanel1.Controls[ii]).ForeColor = System.Drawing.Color.White;
                ((Label)flowLayoutPanel1.Controls[ii]).BackColor = System.Drawing.Color.White;
                toolTip1.SetToolTip((Label)flowLayoutPanel1.Controls[ii], "");
            }
        }
    }
}