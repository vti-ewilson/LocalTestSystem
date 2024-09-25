using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.Graphing.DataPlot;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Components;
using VTIWindowsControlLibrary.Components.Graphing;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents the Plot Properties form for the Data Plot.
    /// </summary>
    public partial class PlotPropForm : Form
    {
        private DataPlotControl _dataPlot;

        public DataPlotControl DataPlot
        {
            get
            {
                return _dataPlot;
            }
        }

        private object currentControl;
        public object CrntControl { set { currentControl = value; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="PlotPropForm">PlotPropForm</see> class
        /// </summary>
        /// <param name="dataPlot">Refers to the <see cref="DataPlotControl">DataPlot</see> that is associated with this form.</param>
        public PlotPropForm(DataPlotControl dataPlot)
        {
            _dataPlot = dataPlot;
            InitializeComponent();
        }

        private void frmPlotProp_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                Hide();
                e.Cancel = true;
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            Hide();
        }

        private void ucNumPad1_CurrentSettingChanged(object sender, VTIWindowsControlLibrary.Components.CurrentSettingChangedEventArgs e)
        {
            if (currentControl != null)
            {
                if (currentControl is TextBox)
                {
                    ((TextBox)currentControl).Text = e.CurrentSetting.ToString();
                }
                else if (currentControl is NumericUpDown)
                {
                    ((NumericUpDown)currentControl).Value = (decimal)e.CurrentSetting;
                }
            }
        }

        private void textBoxYMax_Enter(object sender, EventArgs e)
        {
            currentControl = textBoxYMax;
        }

        private void numericUpDownYMaxExp_Enter(object sender, EventArgs e)
        {
            currentControl = numericUpDownYMaxExp;
        }

        private void textBoxYMin_Enter(object sender, EventArgs e)
        {
            currentControl = textBoxYMin;
        }

        private void numericUpDownYMinExp_Enter(object sender, EventArgs e)
        {
            currentControl = numericUpDownYMinExp;
        }

        private void textBoxXMin_Enter(object sender, EventArgs e)
        {
            currentControl = textBoxXMin;
        }

        private void textBoxXMax_Enter(object sender, EventArgs e)
        {
            currentControl = textBoxXMax;
        }

        private void textBoxXWindow_Enter(object sender, EventArgs e)
        {
            currentControl = textBoxXWindow;
        }

        public void buttonOK_Click(object sender, EventArgs e)
        {
            try
            {
                if (radioButtonLinear.Checked && (Double.Parse(textBoxYMax.Text) <= Double.Parse(textBoxYMin.Text)))
                {
                    MessageBox.Show("Y-Axis Max value must be greater than Y-Axis Min value!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (radioButtonLog.Checked && ((int)numericUpDownYMaxExp.Value <= (int)numericUpDownYMinExp.Value))
                {
                    MessageBox.Show("Y-Axis Max Exponent value must be greater than Y-Axis Min Exponent value!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (Double.Parse(textBoxXMax.Text) <= Double.Parse(textBoxXMin.Text))
                {
                    MessageBox.Show("X-Axis Max value must be greater than X-Axis Min value!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (Double.Parse(textBoxXMin.Text) < 0)
                {
                    MessageBox.Show("X-Axis Min value must be positive!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (Double.Parse(textBoxXMax.Text) < 0)
                {
                    MessageBox.Show("X-Axis Max value must be positive!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (Double.Parse(textBoxXWindow.Text) <= 0)
                {
                    MessageBox.Show("X-Axis Window value must be greater than zero!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }
                if (radioButtonLinear.Checked)
                {
                    _dataPlot.Settings.YMax = Single.Parse(textBoxYMax.Text);
                    _dataPlot.Settings.YMin = Single.Parse(textBoxYMin.Text);
                }
                _dataPlot.Settings.YMaxExp = (int)numericUpDownYMaxExp.Value;
                _dataPlot.Settings.YMinExp = (int)numericUpDownYMinExp.Value;
                if (radioButtonSeconds.Checked)
                {
                    _dataPlot.Settings.XWindow = Single.Parse(textBoxXWindow.Text);
                    _dataPlot.Settings.XMin = Single.Parse(textBoxXMin.Text);
                    _dataPlot.Settings.XMax = Single.Parse(textBoxXMax.Text);
                }
                else
                {
                    _dataPlot.Settings.XWindow = (float)60.0 * Single.Parse(textBoxXWindow.Text);
                    _dataPlot.Settings.XMin = (float)60.0 * Single.Parse(textBoxXMin.Text);
                    _dataPlot.Settings.XMax = (float)60.0 * Single.Parse(textBoxXMax.Text);
                }
                _dataPlot.Settings.AutoScaleYMaxExp = checkBoxAutoScaleMax.Checked;
                _dataPlot.Settings.AutoScaleYMinExp = checkBoxAutoScaleMin.Checked;
                _dataPlot.Settings.PlotSemiLog = radioButtonLog.Checked;
                if (_dataPlot.Settings.YAxisButtonsVisible)
                {
                    if (radioButtonLog.Checked)
                    {
                        _dataPlot.YMaxUp.Visible = true;
                        _dataPlot.YMaxDn.Visible = true;
                        _dataPlot.YMinUp.Visible = true;
                        _dataPlot.YMinDn.Visible = true;
                        _dataPlot.YMaxExpUp.Visible = false;
                        _dataPlot.YMaxExpDn.Visible = false;
                        _dataPlot.YMinExpUp.Visible = false;
                        _dataPlot.YMinExpDn.Visible = false;
                    }
                    else
                    { // Linear
                        _dataPlot.YMaxUp.Visible = true;
                        _dataPlot.YMaxDn.Visible = true;
                        _dataPlot.YMinUp.Visible = true;
                        _dataPlot.YMinDn.Visible = true;
                        _dataPlot.YMaxExpUp.Visible = true;
                        _dataPlot.YMaxExpDn.Visible = true;
                        _dataPlot.YMinExpUp.Visible = true;
                        _dataPlot.YMinExpDn.Visible = true;
                    }
                }
                if (radioButtonSeconds.Checked)
                    _dataPlot.Settings.XAxisUnits = XAxisUnitsType.Seconds;
                else
                    _dataPlot.Settings.XAxisUnits = XAxisUnitsType.Minutes;
                if (radioButtonSecondsMinutes.Checked)
                    _dataPlot.Settings.XAxisDisplayUnits = XAxisDisplayUnitsType.SecondsMinutes;
                else if (radioButtonDate.Checked)
                    _dataPlot.Settings.XAxisDisplayUnits = XAxisDisplayUnitsType.Date;
                else if (radioButtonDateTime.Checked)
                    _dataPlot.Settings.XAxisDisplayUnits = XAxisDisplayUnitsType.DateTime;
                else
                    _dataPlot.Settings.XAxisDisplayUnits = XAxisDisplayUnitsType.Time;
                if (comboBoxDataCollectionInterval.Text == "All")
                    _dataPlot.Settings.DataCollectionInterval = 0;
                else
                    _dataPlot.Settings.DataCollectionInterval = Convert.ToSingle(comboBoxDataCollectionInterval.Text);
                _dataPlot.Settings.XWindowShiftPercent = Enum<XWindowShiftType>.Parse(comboBoxXWindowShiftPercent.Text.Replace(' ', '_'));
                _dataPlot.Settings.CommentsVisible = checkBoxShowComments.Checked;
                _dataPlot.Settings.AutoSave = checkBoxAutoSave.Checked;
                _dataPlot.Settings.DecimationThreshold = numericSliderDecimationThreshold.Text;

                foreach (var comment in _dataPlot.GraphControl.GraphData.Comments)
                {
                    comment.Visible =
                        comment.CommentControl.Visible =
                            _dataPlot.Settings.CommentsVisible;
                }

                _dataPlot.Settings.TraceColors.Clear();
                _dataPlot.Settings.TraceVisibility.Clear();

                for (int i = 0; i < _dataPlot.Traces.Count; i++)
                {
                    _dataPlot.Settings.TraceColors.Add(
                        ColorTranslator.ToHtml(
                            ((PictureBox)tableLayoutPanelTraceSettings.Controls["pictureBoxTraceColor" + i.ToString()]).BackColor));
                    _dataPlot.Settings.TraceVisibility.Add(
                        ((CheckBox)tableLayoutPanelTraceSettings.Controls["checkBoxTraceVisible" + i.ToString()]).Checked);
                }
                _dataPlot.Settings.Save();
                _dataPlot.SetSettings();

                Hide();
            }
            catch
            {
                MessageBox.Show("An invalid value was entered!", "Plot Properties", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void radioButtonLog_CheckedChanged(object sender, EventArgs e)
        {
            SetYMinMax();
        }

        private void SetYMinMax()
        {
            labelx10max.Visible =
                labelx10min.Visible =
                numericUpDownYMaxExp.Visible =
                numericUpDownYMinExp.Visible =
                checkBoxAutoScaleMax.Visible =
                checkBoxAutoScaleMin.Visible =
                radioButtonLog.Checked;
            if (radioButtonLog.Checked)
            {
                textBoxYMax.Width =
                    textBoxYMin.Width = 45;
                textBoxYMax.Enabled =
                    textBoxYMin.Enabled =
                    false;
                textBoxYMax.Text =
                    textBoxYMin.Text = "1";
            }
            else
            {
                textBoxYMax.Width =
                    textBoxYMin.Width = 120;
                textBoxYMax.Enabled =
                    textBoxYMin.Enabled =
                    true;
                string format = GetShortestDistinctFormat(_dataPlot.Settings.YMin, _dataPlot.Settings.YMax, 10);
                if (_dataPlot.Settings.YMax.ToString(format) == "0E0")
                {
                    textBoxYMax.Text = "0";
                }
                else
                {
                    textBoxYMax.Text = _dataPlot.Settings.YMax.ToString(format);
                }
                if (_dataPlot.Settings.YMin.ToString(format) == "0E0")
                {
                    textBoxYMin.Text = "0";
                }
                else
                {
                    textBoxYMin.Text = _dataPlot.Settings.YMin.ToString(format);
                }
                //textBoxYMax.Text = VtiNumberFormat(_dataPlot.Settings.YMax);
                //textBoxYMin.Text = VtiNumberFormat(_dataPlot.Settings.YMin);
            }
        }

        public string GetShortestDistinctFormat(float yminF, float ymaxF, int ticks)
        {
            if (yminF == ymaxF)
            {
                return "0";
            }
            decimal ymin = Convert.ToDecimal(yminF);
            decimal ymax = Convert.ToDecimal(ymaxF);
            List<decimal> tickValues = new List<decimal>();
            List<string> tickLabels = new List<string>();
            for (int i = 0; i < ticks; i++)
            {
                tickValues.Add((((ymax - ymin) / ticks * i) + ymin));
            }
            double logYmax = Math.Truncate(Math.Log10(Convert.ToDouble(ymax)));
            double logYmin = Math.Truncate(Math.Log10(Convert.ToDouble(ymin)));
            if ((Math.Abs(ymax) < 0.01m && Math.Abs(ymin) < 0.01m)
                || (Math.Abs(ymax) > 1000 && Math.Abs(ymin) > 1000)
                || ((logYmax - logYmin) >= 3 && logYmax >= 0 && logYmin >= 0))
            {
                string format = "0E0";
                while ((tickLabels.Count != tickLabels.Distinct().Count() || tickLabels.Count == 0) && tickValues.Distinct().Count() != 1)
                {
                    tickLabels.Clear();
                    if (format.Contains("."))
                    {
                        format = format.Insert(format.IndexOf("E"), "0");
                    }
                    else
                    {
                        format = format.Insert(format.IndexOf("E"), ".0");
                    }
                    foreach (decimal val in tickValues)
                    {
                        tickLabels.Add(val.ToString(format));
                    }
                }
                if (ymin.ToString(format).EndsWith("0") && ymax.ToString(format).EndsWith("0"))
                {
                    format = format.Remove(format.Length - 1);
                    if (format.EndsWith("E"))
                    {
                        format = format.Remove(format.Length - 1);
                    }
                }
                //if format leaves ymin & ymax with a 0 before the E, take it off. (Ex. 1.80E-4 to 1.8E-4)
                else if (ymin.ToString(format)[ymin.ToString(format).IndexOf("E") - 1] == '0' && ymax.ToString(format)[ymax.ToString(format).IndexOf("E") - 1] == '0')
                {
                    format = format.Remove(format.IndexOf('E') - 1, 1);
                }
                return format;
            }
            else
            {
                string format = "0";
                while ((tickLabels.Count != tickLabels.Distinct().Count() || tickLabels.Count == 0) && tickValues.Distinct().Count() != 1)
                {
                    tickLabels.Clear();
                    if (format.Contains("."))
                    {
                        format = format + "0";
                    }
                    else
                    {
                        format = format + ".0";
                    }
                    foreach (decimal val in tickValues)
                    {
                        tickLabels.Add(val.ToString(format));
                    }
                }
                if (ymin.ToString(format).EndsWith("0") && ymax.ToString(format).EndsWith("0"))
                {
                    format = format.Remove(format.Length - 1);
                }
                return format;
            }
        }

        private string VtiNumberFormat(Double value)
        {
            string result;

            if (value == 0)
                result = "0";
            else if (Math.Abs(value) < 0.01)
                result = value.ToString("0.0E+00");
            else if (Math.Abs(value) < 0.1)
                result = value.ToString("0.0000");
            else if (Math.Abs(value) < 1)
                result = value.ToString("0.000");
            else if (Math.Abs(value) < 10)
                result = value.ToString("0.000");
            else if (Math.Abs(value) < 100)
                result = value.ToString("0.00");
            else if (Math.Abs(value) < 1000)
                result = value.ToString("0");
            else
                result = value.ToString("0.0E+00");

            return result;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            colorDialog1.Color = ((PictureBox)sender).BackColor;
            if (colorDialog1.ShowDialog() == DialogResult.OK)
                ((PictureBox)sender).BackColor = colorDialog1.Color;
        }

        private void PlotPropForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                WrappingCheckBox checkBox1;
                PictureBox pictureBox1;

                Cursor = Cursors.WaitCursor;
                SuspendLayout();
                textBoxYMax.Text = _dataPlot.Settings.YMax.ToString();
                textBoxYMin.Text = _dataPlot.Settings.YMin.ToString();
                try
                {
                    numericUpDownYMaxExp.Value = (decimal)_dataPlot.Settings.YMaxExp;
                    numericUpDownYMinExp.Value = (decimal)_dataPlot.Settings.YMinExp;
                }
                catch (ArgumentOutOfRangeException e2)
                {
                }
                if (radioButtonSeconds.Checked)
                {
                    textBoxXWindow.Text = _dataPlot.Settings.XWindow.ToString();
                    textBoxXMin.Text = _dataPlot.Settings.XMin.ToString();
                    textBoxXMax.Text = _dataPlot.Settings.XMax.ToString();
                }
                else
                {
                    textBoxXWindow.Text = (_dataPlot.Settings.XWindow / (float)60.0).ToString();
                    textBoxXMin.Text = (_dataPlot.Settings.XMin / (float)60.0).ToString();
                    textBoxXMax.Text = (_dataPlot.Settings.XMax / (float)60.0).ToString();
                }
                checkBoxAutoScaleMax.Checked = _dataPlot.Settings.AutoScaleYMaxExp;
                checkBoxAutoScaleMin.Checked = _dataPlot.Settings.AutoScaleYMinExp;
                radioButtonLinear.Checked = !_dataPlot.Settings.PlotSemiLog;
                radioButtonLog.Checked = _dataPlot.Settings.PlotSemiLog;
                radioButtonSeconds.Checked = (_dataPlot.Settings.XAxisUnits == XAxisUnitsType.Seconds);
                radioButtonMinutes.Checked = (_dataPlot.Settings.XAxisUnits == XAxisUnitsType.Minutes);
                numericSliderDecimationThreshold.Text = _dataPlot.Settings.DecimationThreshold;
                numericSliderDecimationThreshold.Value = Convert.ToDouble(_dataPlot.Settings.DecimationThreshold);
                SetYMinMax();
                switch (_dataPlot.Settings.XAxisDisplayUnits)
                {
                    case XAxisDisplayUnitsType.SecondsMinutes:
                        radioButtonSecondsMinutes.Checked = true;
                        break;

                    case XAxisDisplayUnitsType.Date:
                        radioButtonDate.Checked = true;
                        break;

                    case XAxisDisplayUnitsType.DateTime:
                        radioButtonDateTime.Checked = true;
                        break;

                    case XAxisDisplayUnitsType.Time:
                        radioButtonTime.Checked = true;
                        break;
                }
                if (_dataPlot.Settings.DataCollectionInterval == 0)
                    comboBoxDataCollectionInterval.Text = "All";
                else
                    comboBoxDataCollectionInterval.Text = _dataPlot.Settings.DataCollectionInterval.ToString();
                comboBoxXWindowShiftPercent.Text = _dataPlot.Settings.XWindowShiftPercent.ToString().Replace('_', ' ');
                checkBoxShowComments.Checked = _dataPlot.Settings.CommentsVisible;
                _dataPlot.GraphControl.toolStripMenuItemDisplayComments.Checked = checkBoxShowComments.Checked;
                checkBoxAutoSave.Checked = _dataPlot.Settings.AutoSave;

                tableLayoutPanelTraceSettings.SuspendLayout();
                tableLayoutPanelTraceSettings.Controls.Clear();
                tableLayoutPanelTraceSettings.RowCount = (_dataPlot.Traces.Count + 1) / 2;
                tableLayoutPanelTraceSettings.RowStyles.Clear();

                foreach (var trace in _dataPlot.Traces)
                {
                    int i = _dataPlot.Traces.IndexOf(trace);
                    checkBox1 = new WrappingCheckBox();
                    checkBox1.AutoSize = true;
                    checkBox1.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
                    checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
                    checkBox1.Location = new System.Drawing.Point(3, 3);
                    checkBox1.Name = "checkBoxTraceVisible" + i.ToString();// i.ToString();
                    checkBox1.Size = new System.Drawing.Size(124, 17);
                    checkBox1.TabIndex = 100 + i;
                    checkBox1.Text = trace.Label;// DataPlot.Traces[i].Label;
                    checkBox1.UseVisualStyleBackColor = true;
                    //checkBox1.Checked = DataPlot.Traces[i].AnalogInput.TraceEnabled;
                    checkBox1.Checked = trace.Visible;// DataPlot.Traces[i].Visible;
                    pictureBox1 = new PictureBox();
                    //pictureBox1.BackColor = DataPlot.Traces[i].AnalogInput.TraceColor;
                    pictureBox1.BackColor = trace.Color; // DataPlot.Traces[i].Color;
                    pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
                    pictureBox1.Location = new System.Drawing.Point(144, 3);
                    pictureBox1.Name = "pictureBoxTraceColor" + i.ToString();
                    pictureBox1.Size = new System.Drawing.Size(34, 17);
                    pictureBox1.TabIndex = 1;
                    pictureBox1.TabStop = false;
                    pictureBox1.Click += new System.EventHandler(pictureBox1_Click);
                    tableLayoutPanelTraceSettings.Controls.Add(checkBox1, (i % 2) * 2, i / 2);
                    tableLayoutPanelTraceSettings.Controls.Add(pictureBox1, (i % 2) * 2 + 1, i / 2);
                    tableLayoutPanelTraceSettings.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                }
                tableLayoutPanelTraceSettings.ResumeLayout();
                tabControlSettings.SelectedTab = tabPageAxisSettings;
                ResumeLayout();
                Cursor = Cursors.Default;
            }
        }

        public NumericUpDown YMaxExp()
        {
            return numericUpDownYMaxExp;
        }

        public NumericUpDown YMinExp()
        {
            return numericUpDownYMinExp;
        }

        private void checkBoxAutoSave_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAutoSave.Checked)
                labelDecimationThreshold.Text = "AutoSave Threshold";
            else
                labelDecimationThreshold.Text = "Decimation Threshold";
        }
    }
}