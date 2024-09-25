using System;
using System.Diagnostics;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Components;

namespace VTIWindowsControlLibrary.Classes.ClientForms
{
    /// <summary>
    /// Represents a base class from which Schematic Forms can be derived.
    /// </summary>
    /// <remarks>
    /// The SchematicFormBase class has the ability to lock and unlock
    /// any <see cref="VTIWindowsControlLibrary.Components.SchematicCheckBox">SchematicCheckBox</see>
    /// controls that are contained within it. This is used to make
    /// the schematic control Active (able to control outputs) or Inactive
    /// (for viewing only).
    /// </remarks>
    public class SchematicFormBase : Form
    {
        private Boolean _active;

        /// <summary>
        /// true if form is being disposed
        /// </summary>
        public Boolean bIsDisposing { get; set; }

        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchematicFormBase">SchematicFormBase</see> class
        /// </summary>
        public SchematicFormBase()
        {
            this.Activated += new EventHandler(SchematicFormBase_Activated);
        }

        private void SchematicFormBase_Activated(object sender, EventArgs e)
        {
            if (_active) UnlockControls(this);
            else LockControls(this);
        }

        /// <summary>
        /// Shows the Schematic Form
        /// </summary>
        /// <param name="Active">Value to indicate if the
        /// <see cref="VTIWindowsControlLibrary.Components.SchematicCheckBox">SchematicCheckBox</see>
        /// controls contained within the form should be active.</param>
        public void Show(Boolean Active)
        {
            _active = Active;
            if (_active)
                this.Text = VtiLibLocalization.SystemSchematic;
            else
                this.Text = VtiLibLocalization.SystemSchematicInactive;
            //      if (bIsDisposing)
            //        MessageBox.Show("Cannot display schematic because it has already been released from memory.",
            //          "VIEW SCHEMATIC", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //      else
            //      {
            this.Show();
            this.BringToFront();
            //      }
        }

        /// <summary>
        /// Shows the Schematic Form
        /// </summary>
        /// <param name="owner">Form that will own the Schematic Form.</param>
        /// <param name="Active">Value to indicate if the
        /// <see cref="VTIWindowsControlLibrary.Components.SchematicCheckBox">SchematicCheckBox</see>
        /// controls contained within the form should be active.</param>
        public void Show(IWin32Window owner, Boolean Active)
        {
            _active = Active;
            if (_active) this.Text = VtiLibLocalization.SystemSchematic;
            else this.Text = VtiLibLocalization.SystemSchematicInactive;
            this.Show(owner);
            this.BringToFront();
        }

        private void LockControls(Control control)
        {
            foreach (Control subcontrol in control.Controls)
            {
                //Debug.WriteLine(subcontrol.Name);
                //if (subcontrol.Name == "schematicCheckBox17")
                Debug.WriteLine(subcontrol.Name);
                if (subcontrol is SchematicCheckBox)
                {
                    ((SchematicCheckBox)subcontrol).Locked = true;
                }
                else
                    //if ((subcontrol.Name == "button1") || (subcontrol.Name == "buttonClose") ||
                    //  (subcontrol.Name == "schematicPanel1") || subcontrol.Name == "schematicPanel2"
                    //  || subcontrol.Name == "schematicPanelLeft" || subcontrol.Name == "schematicPanelMain"
                    //     || subcontrol.Name == "schematicPanelCharge" || subcontrol.Name == "schematicPanelOther"
                    //    || (subcontrol.Name == "tabControl1") || subcontrol.Name == "tabPage1" || subcontrol.Name == "tabPage2" || subcontrol.Name == "tabPage3")
                    // process all controls of type
                    if ((subcontrol is TabControl) || (subcontrol is SchematicPanel) || (subcontrol is TabPage)
                        || (subcontrol is Button) || (subcontrol is GroupBox))
                {
                    if (!Text.Contains("INACTIVE"))
                        UnlockControls(subcontrol);
                    else
                        LockControls(subcontrol);
                }
                else
                {
                    Debug.WriteLine(subcontrol.Name + " Not IN Lock List");
                }
            }
        }

        private void UnlockControls(Control control)
        {
            foreach (Control subcontrol in control.Controls)
            {
                if (subcontrol is SchematicCheckBox)
                {
                    ((SchematicCheckBox)subcontrol).ResetLockedState();
                }
                else
                    UnlockControls(subcontrol);
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (_active) // schematic is active, so don't dispose of it
            {
                this.Hide();
                return;
            }
            /*
                  if (!bIsDisposing)
                  {
                    switch (MessageBox.Show("Are you sure you won't be using this schematic any more?",
                      "VIEW SCHEMATIC", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                      case DialogResult.Yes:
                        bIsDisposing = true;
                        break;

                      case DialogResult.No:
                        break;
                    }
                  }
            */
            if (bIsDisposing)
            {
                if (disposing && (components != null))
                {
                    components.Dispose();
                }
                base.Dispose(disposing);
            }
        }
    }
}