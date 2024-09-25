using System;
using System.Linq;
using LocalTestSystem.Classes;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Components;

namespace LocalTestSystem.Forms
{
    public partial class SchematicForm : SchematicFormBase
    {
        public SchematicForm()
        {
            InitializeComponent();
            foreach (SchematicCheckBox schematicCheckBox in schematicPanelLeft.Controls.OfType<SchematicCheckBox>())
            {
                schematicCheckBox.CheckedUserChanging += SchematicCheckBox_CheckedUserChanging;
            }
            foreach (SchematicCheckBox schematicCheckBox in schematicPanelMain.Controls.OfType<SchematicCheckBox>())
            {
                schematicCheckBox.CheckedUserChanging += SchematicCheckBox_CheckedUserChanging;
            }

        }

        private void SchematicCheckBox_CheckedUserChanging(SchematicCheckBox sender, SchematicCheckBox.CheckChangingEventArgs e)
        {
            Config.TestMode = TestModes.Manual;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (MyStaticVariables.AnalogInitialized == true)
            {
                schlblP3Torrcon.Text = IO.Signals.CDG10TorrPressureSensor.Value.ToString("F3") + " Torr";
                SchLblP2PSI.Text = IO.Signals.VacuumManifold100PSIGTransducer.Value.ToString("F1") + " PSI";
            }
        }

        private void SchematicForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!timer1.Enabled && this.Visible) timer1.Enabled = true;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();

        }
    }
}