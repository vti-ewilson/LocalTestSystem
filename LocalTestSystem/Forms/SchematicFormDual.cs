using System;
using System.Linq;
using System.Windows.Forms;
using LocalTestSystem.Classes.Configuration;
using LocalTestSystem.Classes.IOClasses;
using LocalTestSystem.Enums;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.ClientForms;
using VTIWindowsControlLibrary.Components;

namespace LocalTestSystem.Forms
{
    public partial class SchematicFormDual : SchematicFormBase
    {
        public SchematicFormDual()
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
            //schlblP3PVEEvac.Text = IO.Signals.PVEDegasManifoldPressureSensor.Value.ToString("F4") + " Torr";
            //schlblP2PVEPress.Text = IO.Signals.PVEDegasManifold100PSIGSensor.Value.ToString("F4") + " PSI";
            //schlblP6POEEvac.Text = IO.Signals.POEDegasManifoldPressureSensor.Value.ToString("F4") + " Torr";
            //schlblP5POEPress.Text = IO.Signals.POEDegasManifold100PSIGSensor.Value.ToString("F4") + " PSI";
            //PVEScale.Text = IO.Signals.PVEScale.Value.ToString("F1") + " lbs";
            //POEScale.Text = IO.Signals.POEScale.Value.ToString("F1") + " lbs";
        }

        private void SchematicForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!timer1.Enabled && this.Visible) timer1.Enabled = true;

        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(txtStart.Text, out double parsedVal))
            {
                Machine.Test[0].CounterValueToLoadPrimary = parsedVal;
                Machine.Test[0].bLoadCounterPrimary = true;
            }
            else MessageBox.Show("Invalid Value", "Not It", MessageBoxButtons.OK);
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(txtStop.Text, out double parsedVal))
            {
                Machine.Test[0].CounterLimitValueToLoadPrimary = parsedVal;
                Machine.Test[0].bLoadCounterLimitPrimary = true;
            }
            else MessageBox.Show("Invalid Value", "Not It", MessageBoxButtons.OK);

        }

        private void txtStart_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            /// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
            /// Keypad enum
            switch (e.KeyCode)
            {
                case Keys.E:
                case Keys.Oemplus:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.Subtract:
                case Keys.Add:
                case Keys.Decimal:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Back:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }
      

        private void txtStop_KeyDown(object sender, KeyEventArgs e)
        {
            /// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
            /// Keypad enum
            switch (e.KeyCode)
            {
                case Keys.E:
                case Keys.Oemplus:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.Subtract:
                case Keys.Add:
                case Keys.Decimal:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Back:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void btnStart2_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(txtStart2.Text, out double parsedVal))
            {
                Machine.Test[1].CounterValueToLoadSecondary = parsedVal;
                Machine.Test[1].bLoadCounterSecondary = true;
            }
            else MessageBox.Show("Invalid Value", "Not It", MessageBoxButtons.OK);

        }

        private void btnStop2_Click(object sender, EventArgs e)
        {
            if (Double.TryParse(txtStop2.Text, out double parsedVal))
            {
                Machine.Test[1].CounterLimitValueToLoadSecondary = parsedVal;
                Machine.Test[1].bLoadCounterLimitSecondary = true;
            }
            else MessageBox.Show("Invalid Value", "Not It", MessageBoxButtons.OK);

        }

        private void txtStart2_KeyDown(object sender, KeyEventArgs e)
        {
            /// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
            /// Keypad enum
            switch (e.KeyCode)
            {
                case Keys.E:
                case Keys.Oemplus:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.Subtract:
                case Keys.Add:
                case Keys.Decimal:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Back:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }

        private void txtStop2_KeyDown(object sender, KeyEventArgs e)
        {
            /// https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.keys?view=netframework-4.8
            /// Keypad enum
            switch (e.KeyCode)
            {
                case Keys.E:
                case Keys.Oemplus:
                case Keys.OemMinus:
                case Keys.OemPeriod:
                case Keys.Subtract:
                case Keys.Add:
                case Keys.Decimal:
                case Keys.D0:
                case Keys.D1:
                case Keys.D2:
                case Keys.D3:
                case Keys.D4:
                case Keys.D5:
                case Keys.D6:
                case Keys.D7:
                case Keys.D8:
                case Keys.D9:
                case Keys.NumPad0:
                case Keys.NumPad1:
                case Keys.NumPad2:
                case Keys.NumPad3:
                case Keys.NumPad4:
                case Keys.NumPad5:
                case Keys.NumPad6:
                case Keys.NumPad7:
                case Keys.NumPad8:
                case Keys.NumPad9:
                case Keys.Back:
                    break;
                default:
                    e.Handled = true;
                    break;
            }
        }
    }
}