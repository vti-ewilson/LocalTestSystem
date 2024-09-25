using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace VTIWindowsControlLibrary.Forms
{
    partial class AboutBoxForm : Form
    {
        public AboutBoxForm()
        {
            InitializeComponent();

            //  Initialize the AboutBox to display the product information from the assembly information.
            //  Change assembly information settings for your application through either:
            //  - Project->Properties->Application->Assembly Information
            //  - AssemblyInfo.cs
            //this.Text = String.Format("About {0}", AssemblyTitle);
            //this.labelProductName.Text = AssemblyProduct;
            //this.labelVersion.Text = String.Format("Version {0}", AssemblyVersion);
            //this.labelCopyright.Text = AssemblyCopyright;
            //this.labelCompanyName.Text = AssemblyCompany;
            //this.textBoxDescription.Text = AssemblyDescription;
        }

        #region Assembly Attribute Accessors

        public string Title
        {
            get { return this.Text; }
            set { this.Text = value; }
        }

        public string Version
        {
            get { return this.labelVersion.Text; }
            set { this.labelVersion.Text = value; }
        }

        public string ControlLibVersion
        {
            get { return this.labelControlLibVersion.Text; }
            set { this.labelControlLibVersion.Text = value; }
        }

        public string PLCLibVersion
        {
            get { return this.labelPLCLibVersion.Text; }
            set { this.labelPLCLibVersion.Text = value; }
        }

        public string Description
        {
            get { return this.textBoxDescription.Text; }
            set { this.textBoxDescription.Text = value; }
        }

        public new string ProductName
        {
            get { return this.labelProductName.Text; }
            set { this.labelProductName.Text = value; }
        }

        public string Copyright
        {
            get { return this.labelCopyright.Text; }
            set
            {
                string strDate = "";
                if (value.Contains("20")) // already contains the year
                    strDate = value;
                else
                {
                    DateTime buildDate = new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime;
                    strDate = value + String.Format(" {0:MM/dd/yyyy hh:mm tt}", buildDate);
                }
                this.labelCopyright.Text = strDate;
            }
        }

        public string Company
        {
            get { return this.labelCompanyName.Text; }
            set { this.labelCompanyName.Text = value; }
        }

        #endregion Assembly Attribute Accessors
    }
}