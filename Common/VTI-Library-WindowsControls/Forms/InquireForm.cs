using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Data;

//using Excel12 = Microsoft.Office.Interop.Excel;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Represents the Inquire Form of the client application
    /// </summary>
    public partial class InquireForm : Form
    {
        private VtiDataContext db;
        public event EventHandler ScanButtonPressed;

        /// <summary>
        /// Initializes a new instance of the Inquire Form
        /// </summary>
        public InquireForm()
        {
            InitializeComponent();
            this.KeyPreview = true;
            //if there is a remote VtiData connection, use remote DB for inquire by default
            if (VtiLib.Data2 != null && VtiLib.Data2.CheckConnStatus2())
            {
                useRemoteDBCheckBox.Checked = true;
            }
            else
            {
                useRemoteDBCheckBox.Checked = false;
            }
        }
       
        /// <summary>
        /// Finds all data related to the specified serial number.
        /// </summary>
        /// <param name="SerialNumber">Serial number to look up</param>
        public void LookupSerialNumber(string SerialNumber)
        {
            if (dataGridViewUUTRecords2.InvokeRequired)
            {
                dataGridViewUUTRecords2.Invoke(new Action<string>(LookupSerialNumber), SerialNumber);
            }
            else
            {
                ClearSearchFilters();
                dateTimePickerStart.Value = DateTimePicker.MinimumDateTime;
                SerialNumBox.Text = SerialNumber;
                FilterRecords();
            }
        }

        private void buttonFilterRecords_Click(object sender, EventArgs e)
        {
            FilterRecords();
        }

        private void FilterRecords()
        {
            Cursor.Current = Cursors.WaitCursor;
            buttonFilterRecords.Enabled = false;

            if (useRemoteDBCheckBox.Checked)
            {
                dataGridViewUUTRecords2.Columns.Clear();
                // custom query in case dbo.uutRecords or dbo.UutRecordDetails has extra columns
                string sql = $"SELECT * FROM UutRecords WHERE" +
                $" DateTime >= '{dateTimePickerStart.Value.Date.ToShortDateString()}' AND DateTime < '{dateTimePickerEnd.Value.AddDays(1).ToShortDateString()}'" +
                GetSerialNoFrag() + GetModelNoFrag() + GetOperatorFrag() + GetTestTypeFrag() + GetTestResultFrag() + GetSystemIDFrag() +
                " ORDER BY DateTime DESC";
                SqlConnection connection = new SqlConnection(db.Connection.ConnectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                connection.Open();
                dataadapter.Fill(ds, "UutRecords");
                connection.Close();
                dataGridViewUUTRecords2.DataSource = ds;
                dataGridViewUUTRecords2.DataMember = "UutRecords";
                dataGridViewUUTRecords2.Columns["ID"].Visible = false;
            }
            else
            {

                dataGridViewUUTRecords2.DataSource =
                    from uutRecord in db.UutRecords
                    where
                        uutRecord.DateTime.Date >= dateTimePickerStart.Value.Date &&
                        uutRecord.DateTime.Date <= dateTimePickerEnd.Value.Date &&

                        (SqlMethods.Like(uutRecord.SerialNo, SerialNumBox.Text) ||
                          String.IsNullOrEmpty(SerialNumBox.Text) ||
                          uutRecord.SerialNo.Contains(SerialNumBox.Text)) &&

                        (SqlMethods.Like(uutRecord.ModelNo, ModelBox.Text) ||
                          String.IsNullOrEmpty(ModelBox.Text) ||
                          uutRecord.ModelNo.Contains(ModelBox.Text)) &&

                        (SqlMethods.Like(uutRecord.OpID, OperatorBox.Text) ||
                          String.IsNullOrEmpty(OperatorBox.Text) ||
                          uutRecord.OpID.Contains(OperatorBox.Text)) &&

                        (SqlMethods.Like(uutRecord.TestType, TestTypeBox.Text) ||
                          String.IsNullOrEmpty(TestTypeBox.Text) ||
                          uutRecord.TestType.Contains(TestTypeBox.Text)) &&

                        (SqlMethods.Like(uutRecord.TestResult, TestResultBox.Text) ||
                          String.IsNullOrEmpty(TestResultBox.Text) ||
                          uutRecord.TestResult.Contains(TestResultBox.Text)) &&

                           (SqlMethods.Like(uutRecord.SystemID, SystemIDBox.Text) ||
                          String.IsNullOrEmpty(SystemIDBox.Text) ||
                          uutRecord.SystemID.Contains(SystemIDBox.Text))

                    select uutRecord;
                //VtiLib.Data.UutRecords.Where(uutRecord => uutRecord.DateTime >= System.DateTime.Today);

                if (dataGridViewUUTRecords2.Rows.Count > 0)
                {
                    dataGridViewUUTRecordDetails2.DataSource =
                        from uutRecordDetail in db.UutRecordDetails
                        where uutRecordDetail.UutRecordID == dataGridViewUUTRecords2.Rows[0].Cells[0].Value as Nullable<long>
                        select uutRecordDetail;

                    //Columns[3] = "DateTime3"
                    this.dataGridViewUUTRecords2.Sort(this.dataGridViewUUTRecords2.Columns[3], ListSortDirection.Descending);
                    //Columns[2] = "DateTime4"
                    this.dataGridViewUUTRecordDetails2.Sort(this.dataGridViewUUTRecordDetails2.Columns[2], ListSortDirection.Ascending);
                    //Columns[0] = "ID3"
                    this.dataGridViewUUTRecords2.Columns[0].Visible = false;
                    //Columns[0] = "ID4"
                    this.dataGridViewUUTRecordDetails2.Columns[0].Visible = false;
                    dataGridViewUUTRecords2.Columns["Model"].Visible = false;
                }
                else
                {
                    dataGridViewUUTRecordDetails2.DataSource = null;
                }
            }

            Cursor.Current = Cursors.Default;
            buttonFilterRecords.Enabled = true;
        }

        private void dataGridViewUutRecords2_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (useRemoteDBCheckBox.Checked && dataGridViewUUTRecords2.Rows[e.RowIndex].Cells[0].Value != null)
            {
                dataGridViewUUTRecordDetails2.Columns.Clear();
                string sql = $"SELECT * FROM UutRecordDetails WHERE UutRecordID = {dataGridViewUUTRecords2.Rows[e.RowIndex].Cells[0].Value} ORDER BY ID ASC";
                SqlConnection connection = new SqlConnection(db.Connection.ConnectionString);
                SqlDataAdapter dataadapter = new SqlDataAdapter(sql, connection);
                DataSet ds = new DataSet();
                connection.Open();
                dataadapter.Fill(ds, "UutRecordDetails");
                connection.Close();
                dataGridViewUUTRecordDetails2.DataSource = ds;
                dataGridViewUUTRecordDetails2.DataMember = "UutRecordDetails";
                dataGridViewUUTRecordDetails2.Columns["ID"].Visible = false;
                dataGridViewUUTRecordDetails2.Columns["UutRecordID"].Visible = false;
            }
            else if (!useRemoteDBCheckBox.Checked)
            {
                dataGridViewUUTRecordDetails2.DataSource =
                from uutRecordDetail in db.UutRecordDetails
                where uutRecordDetail.UutRecordID == dataGridViewUUTRecords2.Rows[e.RowIndex].Cells[0].Value as Nullable<long>
                select uutRecordDetail;
                //Columns[2] = "DateTime4"
                dataGridViewUUTRecordDetails2.Columns["UutRecordID"].Visible = false;
                dataGridViewUUTRecordDetails2.Columns["UutRecord"].Visible = false;
                this.dataGridViewUUTRecordDetails2.Sort(this.dataGridViewUUTRecordDetails2.Columns[2], ListSortDirection.Ascending);
            }
        }

        private void buttonExportData_Click(object sender, EventArgs e)
        {
            #region Excel - Microsoft Interopt
            // The following commented-out block of code used Microsoft Interopt features to
            // create a spreadsheet.  I'm leaving it here for now since it worked and it's a
            // good example.  RWP 7/23/09
            //Object missing = System.Reflection.Missing.Value;
            //Excel12.Application ExcelApp = new Excel12.Application();
            //Excel12.Workbook workbook = ExcelApp.Workbooks.Add(missing);
            //Excel12.Sheets sheets = ExcelApp.Worksheets;
            //Excel12.Worksheet sheet1 = (Excel12.Worksheet)ExcelApp.Worksheets.get_Item("Sheet1");
            //Excel12.Range range;
            //sheet1.Name = "UUT Records";

            //// Export titles
            //for (int j = 0; j < dataGridViewUUTRecords2.Columns.Count; j++)
            //{
            //    range = (Excel12.Range)sheet1.Cells[1, j + 1];
            //    range.Value2 = dataGridViewUUTRecords2.Columns[j].HeaderText;
            //}

            //// Export data
            //for (int i = 0; i < dataGridViewUUTRecords2.Rows.Count - 1; i++)
            //{
            //    for (int j = 0; j < dataGridViewUUTRecords2.Columns.Count; j++)
            //    {
            //        range = (Excel12.Range)sheet1.Cells[i + 2, j + 1];
            //        range.Value2 = dataGridViewUUTRecords2[j, i].Value;
            //        if (j == 3) range.NumberFormat = "m/d/yyyy";
            //    }
            //}

            ////Excel12.Worksheet sheet2 = (Excel12.Worksheet)ExcelApp.Worksheets.get_Item("Sheet2");
            ////sheet2.Name = "UUT Record Details";

            //workbook.SaveAs(@"C:\exceltest.xls",
            //    Excel12.XlFileFormat.xlXMLSpreadsheet, missing, missing,
            //    false, false, Excel12.XlSaveAsAccessMode.xlNoChange,
            //    missing, missing, missing, missing, missing);

            //workbook.Close(false, missing, missing);
            //workbook = null;

            //ExcelApp.Quit();
            //ExcelApp = null;

            //GC.Collect();
            #endregion

            #region old
            //String delimiter;
            //if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            //{
            //    System.IO.StreamWriter sw = new System.IO.StreamWriter(saveFileDialog1.FileName);

            //    if (saveFileDialog1.FilterIndex == 1)
            //        delimiter = ",";
            //    else
            //        delimiter = "\t";

            //    if (!checkBoxIncludeDetails.Checked)
            //    {
            //        // Write header line
            //        sw.WriteLine(String.Join(delimiter,
            //            new[] {
            //                "ID",
            //                "Serial Number",
            //                "System ID",
            //                "Operator",
            //                "Model Number",
            //                "Date Time",
            //                "Test Type",
            //                "Test Result",
            //                "Test Port"
            //            }));
            //    }
            //    else
            //    {
            //        // Write header line
            //        sw.WriteLine(String.Join(delimiter,
            //            new[] {
            //                "ID",
            //                "Serial Number",
            //                "System ID",
            //                "Operator",
            //                "Model Number",
            //                "Date Time",
            //                "Test Type",
            //                "Test Result",
            //                "Detail ID",
            //                "Detail Date Time",
            //                "Detail Test Type",
            //                "Detail Test Result",
            //                "Value Name",
            //                "Value",
            //                "Min Setpoint Name",
            //                "Min Setpoint Value",
            //                "Max Setpoint Name",
            //                "Max Setpoint Value",
            //                "Units",
            //                "Elapsed Time"
            //                      }));
            //    }

            //    // Write UUT Records
            //    foreach (var uutRecord in
            //        from uutRecord in db.UutRecords
            //        where
            //            uutRecord.DateTime.Date >= dateTimePickerStart.Value.Date &&
            //            uutRecord.DateTime.Date <= dateTimePickerEnd.Value.Date &&
            //            (String.IsNullOrEmpty(SerialNumBox.Text) ||
            //             uutRecord.SerialNo.Contains(SerialNumBox.Text)) &&
            //            (String.IsNullOrEmpty(ModelBox.Text) ||
            //             uutRecord.ModelNo.Contains(ModelBox.Text)) &&
            //            (String.IsNullOrEmpty(OperatorBox.Text) ||
            //             uutRecord.OpID.Contains(OperatorBox.Text)) &&
            //            (String.IsNullOrEmpty(TestTypeBox.Text) ||
            //             uutRecord.TestType.Contains(TestTypeBox.Text)) &&
            //            (String.IsNullOrEmpty(TestResultBox.Text) ||
            //             uutRecord.TestResult.Contains(TestResultBox.Text))
            //        select uutRecord)
            //    {
            //        sw.WriteLine(String.Join(delimiter,
            //            new[] {
            //                uutRecord.ID.ToString(),
            //                uutRecord.SerialNo,
            //                uutRecord.SystemID,
            //                uutRecord.OpID,
            //                uutRecord.ModelNo,
            //                uutRecord.DateTime.ToString(),
            //                uutRecord.TestType,
            //                uutRecord.TestResult,
            //                uutRecord.TestPort,
            //            }));

            //        if (checkBoxIncludeDetails.Checked)
            //        {
            //            foreach (var uutRecordDetail in uutRecord.UutRecordDetails)
            //            {
            //                sw.WriteLine(String.Join(delimiter,
            //                    new[] {
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        String.Empty,
            //                        uutRecordDetail.ID.ToString(),
            //                        uutRecordDetail.DateTime.ToString(),
            //                        uutRecordDetail.Test,
            //                        uutRecordDetail.Result,
            //                        uutRecordDetail.ValueName,
            //                        uutRecordDetail.Value.ToString(),
            //                        uutRecordDetail.MinSetpointName,
            //                        uutRecordDetail.MinSetpoint.ToString(),
            //                        uutRecordDetail.MaxSetpointName,
            //                        uutRecordDetail.MaxSetpoint.ToString(),
            //                        uutRecordDetail.Units,
            //                        uutRecordDetail.ElapsedTime.ToString()
            //                                  }));
            //            }
            //        }
            //    }
            //    sw.Close();
            //}
            #endregion

            String delimiter;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                System.IO.StreamWriter sw;
                try
                {
                    sw = new System.IO.StreamWriter(saveFileDialog1.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                    return;
                }
                if (saveFileDialog1.FilterIndex == 1)
                    delimiter = ",";
                else
                    delimiter = "\t";

                #region Get DGV column names

                List<string> UutColumns = new List<string>();
                for (int i = 0; i < dataGridViewUUTRecords2.Columns.Count; i++)
                {
                    if (dataGridViewUUTRecords2.Columns[i].Name == "ID")
                    {
                        UutColumns.Add("UutRecordID");
                    }
                    //do not include "Model" column when using local DB (always blank, not used)
                    else if (dataGridViewUUTRecords2.Columns[i].Name != "Model")
                    {
                        UutColumns.Add(dataGridViewUUTRecords2.Columns[i].Name);
                    }
                }
                List<string> DetailsColumns = new List<string>();
                for (int i = 0; i < dataGridViewUUTRecordDetails2.Columns.Count; i++)
                {
                    if (dataGridViewUUTRecordDetails2.Columns[i].Name == "ID")
                    {
                        DetailsColumns.Add("UutRecordDetailID");
                    }
                    // do not use "UutRecord" column when using local DB (always blank, not usd)
                    else if (dataGridViewUUTRecordDetails2.Columns[i].Name != "UutRecord")
                    {
                        DetailsColumns.Add(dataGridViewUUTRecordDetails2.Columns[i].Name);
                    }
                }

                #endregion

                int ID_record_index = -1;

                for (int i = 0; i < dataGridViewUUTRecords2.Columns.Count; i++)
                {
                    if (dataGridViewUUTRecords2.Columns[i].Name == "ID")
                    {
                        ID_record_index = dataGridViewUUTRecords2.Columns[i].Index;
                        break;
                    }
                }

                if (!checkBoxIncludeDetails.Checked)
                {
                    // Write header line
                    sw.WriteLine(String.Join(delimiter, UutColumns.ToArray())); 
                }
                else
                {
                    // Write header line
                    UutColumns.AddRange(DetailsColumns);
                    sw.WriteLine(String.Join(delimiter, UutColumns.ToArray()));
                }

                for (int i = 0; i < dataGridViewUUTRecords2.Rows.Count; i++)
                {
                    if (!checkBoxIncludeDetails.Checked)
                    {
                        string t = "";
                        for (int j = 0; j < UutColumns.Count; j++)
                        {
                            t += dataGridViewUUTRecords2.Rows[i].Cells[j].Value + delimiter;
                        }
                        t = t.Substring(0, t.LastIndexOf(delimiter));
                        sw.WriteLine(t);
                    }
                    else
                    {
                        int ID_detail_index = UutColumns.IndexOf("UutRecordDetailID");
                        bool uutRecordIDWritten = false;
                        using (SqlConnection conn = new SqlConnection(db.Connection.ConnectionString))
                        {
                            SqlCommand cmd = new SqlCommand($"SELECT r.*, d.* FROM UutRecordDetails d INNER JOIN UutRecords r on r.ID = d.UutRecordID WHERE d.UutRecordID = {dataGridViewUUTRecords2.Rows[i].Cells[ID_record_index].Value}", conn);
                            conn.Open();
                            SqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                if (!uutRecordIDWritten)
                                {
                                    Object[] values = new object[reader.FieldCount];
                                    reader.GetValues(values);
                                    string[] valuesArray = values.Select(x => x.ToString()).ToArray();
                                    sw.WriteLine(String.Join(delimiter, valuesArray));
                                    uutRecordIDWritten = true;
                                }
                                else
                                {
                                    Object[] values = new object[reader.FieldCount];
                                    reader.GetValues(values);
                                    List<object> detailsOnly = values.ToList();
                                    detailsOnly.RemoveRange(0, ID_detail_index);
                                    string[] detailsOnlyArray = detailsOnly.Select(x => x.ToString()).ToArray();
                                    sw.WriteLine(new StringBuilder().Insert(0, delimiter, ID_detail_index).ToString() + String.Join(delimiter, detailsOnlyArray));
                                    //sw.WriteLine(string.Concat(Enumerable.Repeat(delimiter, ID_detail_index)) + String.Join(delimiter, detailsOnlyArray));
                                }
                            }
                        }
                    }
                }
                sw.Close();

                try
                {
                    System.Diagnostics.Process.Start(System.IO.Path.GetDirectoryName(saveFileDialog1.FileName));
                }
                catch (Exception ex)
                {
                    string temp = ex.Message;
                }
            }
        }

        private void InquireForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
            db.Dispose();
        }

        private void InquireForm_VisibleChanged(object sender, EventArgs e)
        {
            if (Visible)
            {
                if (Properties.Settings.Default.UsesVtiDataDatabase)
                {
                    if (useRemoteDBCheckBox.Checked)
                    {
                        db = new VtiDataContext(VtiLib.ConnectionString2);
                    }
                    else
                    {
                        db = new VtiDataContext(VtiLib.ConnectionString);
                    }
                    ClearSearchFilters();
                }
                else
                {
                    Hide();
                    MessageBox.Show("Inquire Form is not supported with this application.");
                }
            }
        }
        private void InquireForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                    //custom data inquiry
                    FilterRecords();
            }
        }

        private void ClearSearchFilters()
        {
            dateTimePickerStart.Value = System.DateTime.Now.Date;
            dateTimePickerEnd.Value = System.DateTime.Now.Date;
            SerialNumBox.Text = String.Empty;
            ModelBox.Text = String.Empty;
            TestTypeBox.Text = String.Empty;
            TestResultBox.Text = String.Empty;
        }

        private void useRemoteDBCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (useRemoteDBCheckBox.Checked)
            {
                if (VtiLib.Data2 == null || !VtiLib.Data2.CheckConnStatus2())
                {
                    MessageBox.Show("Error establishing connection to Remote database. Define the remote database connection string in Edit Cycle, restart the software, and try again.");
                    useRemoteDBCheckBox.Checked = false;
                }
                else
                {
                    db = new VtiDataContext(VtiLib.ConnectionString2);
                }
            }
            else
            {
                db = new VtiDataContext(VtiLib.ConnectionString);
            }
        }

        private string GetSerialNoFrag()
        {
            if (SerialNumBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND SerialNo LIKE '%{SerialNumBox.Text}%'";
            }
        }

        private string GetModelNoFrag()
        {
            if (ModelBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND ModelNo LIKE '%{ModelBox.Text}%'";
            }
        }

        private string GetOperatorFrag()
        {
            if (OperatorBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND OpID LIKE '%{OperatorBox.Text}%'";
            }
        }

        private string GetTestTypeFrag()
        {
            if (TestTypeBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND TestType LIKE '%{TestTypeBox.Text}%'";
            }
        }

        private string GetTestResultFrag()
        {
            if (TestResultBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND TestResult LIKE '%{TestResultBox.Text}%'";
            }
        }

        private string GetSystemIDFrag()
        {
            if (SystemIDBox.Text.Trim() == "")
            {
                return "";
            }
            else
            {
                return $" AND SystemID LIKE '%{SystemIDBox.Text}%'";
            }
        }

        public void SetScanButtonEvent(EventHandler scanButtonPressed = null)
        {
            if (scanButtonPressed != null)
            {
                ScanButtonPressed += scanButtonPressed;
                ScanButton.Visible = true;
            }
            else
            {
                ScanButton.Visible = false;
            }
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {
            ScanButtonPressed(sender, e);
        }
    }
}