using System;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents a stand-alone Data Plot form
    /// </summary>
    public partial class DataPlotForm : Form
    {
        #region Constructors (3) 

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotForm">DataPlotForm</see>,
        /// opens the specified file, and sets the form to be a child of the specified MdiParent.
        /// </summary>
        /// <param name="filename">Name of the file to be opened.</param>
        /// <param name="mdiParent">Parent form</param>
        public DataPlotForm(string filename, Form mdiParent)
            : this()
        {
            this.Open(filename);
            this.MdiParent = mdiParent;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotForm">DataPlotForm</see>
        /// and opens the specified file.
        /// </summary>
        /// <param name="filename">Name of the file to be opened.</param>
        public DataPlotForm(string filename)
            : this()
        {
            this.Open(filename);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataPlotForm">DataPlotForm</see>
        /// </summary>
        public DataPlotForm()
        {
            InitializeComponent();
        }

        #endregion Constructors 

        #region Properties (1) 

        /// <summary>
        /// Gets the <see cref="DataPlotControl">DataPlotControl</see> contained
        /// within the form.
        /// </summary>
        public DataPlotControl DataPlotControl
        {
            get { return dataPlotControl1; }
        }

        #endregion Properties 

        #region Methods (3) 

        #region Public Methods (1) 

        /// <summary>
        /// Opens the specified Data Plot file. The file can be in the
        /// compressed-XML .aiox file format, or the older .aio format.
        /// </summary>
        /// <param name="filename"></param>
        public bool Open(string filename)
        {
            return dataPlotControl1.Open(filename);
        }

        #endregion Public Methods 
        #region Private Methods (2) 

        private void dataPlotControl1_CaptionChanged(object sender, EventArgs e)
        {
            this.Text = dataPlotControl1.Caption;
        }

        private void dataPlotControl1_Close(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion Private Methods 

        #endregion Methods 
    }
}