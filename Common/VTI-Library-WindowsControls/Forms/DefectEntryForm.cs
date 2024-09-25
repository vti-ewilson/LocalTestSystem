using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.DefectTracking;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Form used to enter <see cref="DefectCode">DefectCodes</see>
    /// </summary>
    public partial class DefectEntryForm : Form
    {
        #region Fields (2)

        #region Private Fields (2)

        private List<DefectCode> _defectCodeList;
        private List<DefectCode> _defects;

        #endregion Private Fields

        #endregion Fields

        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="DefectEntryForm">DefectEntryForm</see>
        /// </summary>
        public DefectEntryForm()
        {
            InitializeComponent();

            _defectCodeList = new List<DefectCode>();
            _defects = new List<DefectCode>();
        }

        #endregion Constructors

        #region Properties (1)

        /// <summary>
        /// Gets the list of <see cref="DefectCode">DefectCodes</see> for the form
        /// </summary>
        public List<DefectCode> Defects
        {
            get { return _defects; }
        }

        //public DataGridView DataGridView { get { return dataGridViewDefects; } }

        #endregion Properties

        #region Delegates and Events (1)

        #region Events (1)

        /// <summary>
        /// Occurs when a Defect Code is entered by the user.
        /// </summary>
        public event EventHandler<DefectsEnteredEventArgs> DefectsEntered;

        #endregion Events

        #endregion Delegates and Events

        #region Methods (10)

        #region Public Methods (1)

        /// <summary>
        /// Reloads the VtiDefectCodes.xml file and refreshes the buttons on the form
        /// </summary>
        public void RefreshButtons()
        {
            Button newButton;
            this.Cursor = Cursors.WaitCursor;
            this.SuspendLayout();

            // clear the existing buttons
            this.flowLayoutPanel1.Controls.Clear();

            // clear the list
            _defectCodeList.Clear();

            // copy the list from the main list
            _defectCodeList.AddRange(DefectCodes.DefectCodeList);

            // sort the list
            _defectCodeList.Sort(new DefectCodes.DefectCodeDescriptionComparer());

            // add them to the form
            foreach (DefectCode defect in _defectCodeList)
            {
                newButton = new Button();
                newButton.Text = defect.Description;
                newButton.Name = "button" + this.flowLayoutPanel1.Controls.Count.ToString();
                newButton.Tag = defect;
                newButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12, FontStyle.Bold);
                newButton.Width = 220;
                newButton.Height = 60;
                newButton.Click += new System.EventHandler(this.buttonDefect_Click);
                this.flowLayoutPanel1.Controls.Add(newButton);
                this.flowLayoutPanel1.Controls[newButton.Name].SendToBack();
            }

            textBoxCustomDefect.Text = "Enter Custom Defect";

            // clear the Defects Entered list
            _defects.Clear();

            this.ResumeLayout();
            this.Cursor = Cursors.Default;
        }

        public bool CheckForDefect(string ScannerText)
        {
            if (ScannerText == "DONE")
            {
                // build the list of defects entered
                foreach (DataGridViewRow row in dataGridViewDefects.Rows)
                    _defects.Add(new DefectCode
                    {
                        Value = row.Cells[0].Value.ToString(),
                        Category = row.Cells[1].Value.ToString(),
                        Description = row.Cells[2].Value.ToString()
                    });

                this.Hide();

                OnDefectsEntered();
                return true;
            }
            else
            {
                if (ScannerText == "NO DEFECT FOUND")
                {
                    if (buttonNoDefect.Enabled)
                    {
                        dataGridViewDefects.Rows.Add("NO DEFECT", "None", "No Defect Found");
                    }
                }
                else
                {
                    foreach (DefectCode defect in _defectCodeList)
                    {
                        if (ScannerText == defect.Value)
                        {
                            dataGridViewDefects.Rows.Add(defect.Value, defect.Category, defect.Description);
                            buttonNoDefect.Enabled = false;
                            return true;
                        }
                    }
                }
                return false;
            }
        }

        public void ClearAllDefectRows()
        {
            buttonNoDefect.Enabled = true;
            _defects.Clear();
            dataGridViewDefects.Rows.Clear();
        }

        #endregion Public Methods
        #region Protected Methods (1)

        /// <summary>
        /// Raises the <see cref="DefectsEntered">DefectsEntered</see> event.
        /// </summary>
        protected virtual void OnDefectsEntered()
        {
            if (DefectsEntered != null)
                DefectsEntered(this, new DefectsEnteredEventArgs(_defects));
        }

        #endregion Protected Methods
        #region Private Methods (8)

        private void buttonDefect_Click(object sender, EventArgs e)
        {
            DefectCode defect = ((DefectCode)((Button)sender).Tag);
            dataGridViewDefects.Rows.Add(defect.Value, defect.Category, defect.Description);
            buttonNoDefect.Enabled = false;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            // build the list of defects entered
            foreach (DataGridViewRow row in dataGridViewDefects.Rows)
                _defects.Add(new DefectCode
                {
                    Value = row.Cells[0].Value.ToString(),
                    Category = row.Cells[1].Value.ToString(),
                    Description = row.Cells[2].Value.ToString()
                });

            this.Hide();

            OnDefectsEntered();
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.flowLayoutPanel1.Top -= 66;
            if (this.flowLayoutPanel1.Height + this.flowLayoutPanel1.Top < 200)
                this.flowLayoutPanel1.Top += 66;
            this.ResumeLayout();
        }

        private void buttonNoDefect_Click(object sender, EventArgs e)
        {
            dataGridViewDefects.Rows.Add("NO DEFECT", "None", "No Defect Found");
        }

        private void buttonRemoveLast_Click(object sender, EventArgs e)
        {
            if (dataGridViewDefects.Rows.Count > 0)
                dataGridViewDefects.Rows.RemoveAt(dataGridViewDefects.Rows.Count - 1);
            if (dataGridViewDefects.Rows.Count == 0)
                buttonNoDefect.Enabled = true;
        }

        private void buttonSubmitDefect_Click(object sender, EventArgs e)
        {
            dataGridViewDefects.Rows.Add(textBoxCustomDefect.Text, "Custom", textBoxCustomDefect.Text);
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            this.SuspendLayout();
            this.flowLayoutPanel1.Top += 66;
            if (this.flowLayoutPanel1.Top > 0) this.flowLayoutPanel1.Top = 0;
            this.ResumeLayout();
        }

        private void DefectEntryForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible) this.RefreshButtons();
        }

        #endregion Private Methods

        #endregion Methods

        #region Nested Classes (1)

        /// <summary>
        /// Event arguments for Defect Entry events
        /// </summary>
        public class DefectsEnteredEventArgs : EventArgs
        {
            #region Fields (1)

            #region Private Fields (1)

            private List<DefectCode> _defects;

            #endregion Private Fields

            #endregion Fields

            #region Constructors (1)

            /// <summary>
            /// Initializes a new instance of the <see cref="DefectsEnteredEventArgs">DefectsEnteredEventArgs</see> class
            /// </summary>
            /// <param name="Defects"></param>
            public DefectsEnteredEventArgs(List<DefectCode> Defects)
            {
                _defects = Defects;
            }

            #endregion Constructors

            #region Properties (1)

            /// <summary>
            /// Gets the list of <see cref="DefectCode">Defect Codes</see>
            /// </summary>
            public List<DefectCode> Defects
            {
                get { return _defects; }
            }

            #endregion Properties
        }

        #endregion Nested Classes
    }
}