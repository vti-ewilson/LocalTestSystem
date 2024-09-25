using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Management;
using System.Text;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.Util;
using VTIWindowsControlLibrary.Enums;

namespace VTIWindowsControlLibrary.Forms
{
    /// <summary>
    /// Form used to view events in the
    /// <see cref="VTIWindowsControlLibrary.Components.VtiEventLog">VtiEventLog</see>
    /// </summary>
    public partial class EventLogViewerForm : Form
    {
        #region Delegate Definitions

        private delegate void RowStringDelegate(DataGridViewRow row, string s);

        private delegate void StringDelegate(string s);

        private delegate void EventLogDelegate(EventLogEntry entry);

        private delegate void FillMessageDelegate(DataGridViewRow row);

        #endregion Delegate Definitions

        #region Globals

        private EventLog log = null;

        //private BindingSource bindingSource;
        private int countInformation = 0;

        private int countWarning = 0;
        private int countError = 0;
        private int countSuccessAudit = 0;
        private int countFailureAudit = 0;
        private bool loadingGroups = false;
        private bool loadingCategories = false;
        private bool loadingDateFilter = false;
        private string previousGroupFilter = null;
        private string previousCategoryFilter = null;
        private string currentSearch = String.Empty;
        private DateFilterType dateFilter = DateFilterType.Last_2_Hours;
        private string currentSort = null;
        private object lockReadWrite = new object();
        private object wmiLockObject = new object();
        private EventHandler gridSelectionChangedEventHandler;
        private uint lastIndex = 0;
        private List<string> groupNames = new List<string>();
        private DataSet currentEntries = null;
        private DataTable currentTable = null;
        private DataView currentView = null;

        // Cell Styles for the different colors
        private DataGridViewCellStyle cellStyleInformation = CellStyleHelper.GetColorStyle(Properties.Settings.Default.EventViewerColorInformation);

        private DataGridViewCellStyle cellStyleWarning = CellStyleHelper.GetColorStyle(Properties.Settings.Default.EventViewerColorWarning);
        private DataGridViewCellStyle cellStyleError = CellStyleHelper.GetColorStyle(Properties.Settings.Default.EventViewerColorError);

        #endregion Globals

        #region Construction

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogViewerForm">EventLogViewerForm</see>
        /// </summary>
        /// <param name="Log">EventLog associated with this form</param>
        public EventLogViewerForm(EventLog Log)
        {
            InitializeComponent();
            //VtiLog.Log = "VTIEvent";
            //VtiLog.Source = Application.ProductName;

            loadingDateFilter = true;
            foreach (string s in System.Enum.GetNames(typeof(DateFilterType)))
                dlFilterTimeWritten.Items.Add(s.Replace('_', ' '));
            dlFilterTimeWritten.SelectedIndex = 1;
            loadingDateFilter = false;

            log = Log;
            gridSelectionChangedEventHandler = new EventHandler(dgEvents_SelectionChanged);
            //localMachine = (log.MachineName == ".") || (log.MachineName == System.Net.Dns.GetHostName());
            //this.Text = localMachine ? System.Net.Dns.GetHostName() : log.MachineName + " - [" +  log.LogDisplayName + "]";

            // Set up the grid properties for the visual event distinction
            //VisualEventDistinctionType distinctionType = (VisualEventDistinctionType) System.Enum.Parse(typeof(VisualEventDistinctionType), Settings.Default.ColorMode);

            //if (distinctionType == VisualEventDistinctionType.Colors)
            //{
            //    dgEvents.RowsAdded +=new DataGridViewRowsAddedEventHandler(dgEvents_RowsAdded);

            //    // Remove the image column - we're only going to use colours
            //    dgEvents.Columns[0].Visible = false;
            //}
            //else
            //{
            //dgEvents.CellFormatting += new DataGridViewCellFormattingEventHandler(dgEvents_CellFormatting);
            //}

            FillGrid();
        }

        #endregion Construction

        #region Properties

        /// <summary>
        /// Gets the EventLog associated with this form.
        /// </summary>
        public EventLog Log
        {
            get
            {
                return log;
            }
        }

        #endregion Properties

        #region Private Members

        private void DoSearch()
        {
            currentSearch = tbSearch.Text;

            FillGrid();

            if (!btnSearch.Checked)
            {
                btnSearch.Checked = true;
            }

            btnClearSearch.Checked = false;
        }

        private void FillMessage(DataGridViewRow selectedRow)
        {
            try
            {
                if (this.Visible == false) return;
                // Have a delay before we actually retrieve the message
                System.Threading.Thread.Sleep(200);

                if (selectedRow.Index == -1)
                {
                    return;
                }
                uint thisIndex;
                try
                {
                    // Check that the event id is still the same as the one selected
                    thisIndex = (uint)selectedRow.Cells["Index"].Value;
                }
                catch
                {
                    thisIndex = 0;
                }

                if (thisIndex == lastIndex)
                {
                    string message = String.Empty;

                    if (selectedRow.Cells["CompleteMessage"].Value == DBNull.Value ||
                        string.IsNullOrEmpty((string)selectedRow.Cells["CompleteMessage"].Value))
                    {
                        // This message has not been cached yet.  Retrieve it.
                        try
                        {
                            /*
                            int index = System.Convert.ToInt32(thisIndex - smallestRecordNumber);
                            message = log.Entries[index].Message;
                            */

                            //if (!localMachine)
                            //{
                            //    // Set the event detail text to Loading...
                            //    if (this.InvokeRequired)
                            //    {
                            //        this.Invoke(new StringDelegate(SetEventDetailText), new object[] { "Loading..." });
                            //    }
                            //    else
                            //    {
                            //        SetEventDetailText("Loading...");
                            //    }
                            //}
                            message = GetEventLogItemMessage(thisIndex);
                        }
                        catch
                        {
                            message = "Message not found.";
                        }
                    }
                    else
                    {
                        message = (string)selectedRow.Cells[7].Value;
                    }

                    //message = (string)selectedRow.Cells["CompleteMessage"].Value;

                    if (this.InvokeRequired)
                    {
                        this.Invoke(new RowStringDelegate(UpdateEventDetailText), selectedRow, message);
                    }
                    else
                    {
                        UpdateEventDetailText(selectedRow, message);
                    }
                }
            }
            catch (ObjectDisposedException) { }
        }

        private void SetEventDetailText(string str)
        {
            tbEventDetail.Text = str;
        }

        private void SetAutoRefreshEnabled(bool enabled)
        {
            toolStripButtonRefresh.Enabled = !enabled;
            log.EnableRaisingEvents = enabled;
        }

        private string GetEventLogItemMessage(uint thisIndex)
        {
            lock (wmiLockObject)
            {
                ManagementScope messageScope = new ManagementScope(
                      GetStandardPath()
                );

                messageScope.Connect();

                StringBuilder query = new StringBuilder();
                query.Append("select Message, InsertionStrings from Win32_NTLogEvent where LogFile ='");
                query.Append(log.LogDisplayName.Replace("'", "''"));
                query.Append("' AND RecordNumber='");
                query.Append(thisIndex);
                query.Append("'");

                System.Management.ObjectQuery objectQuery = new System.Management.ObjectQuery(
                  query.ToString()
                );

                using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(messageScope, objectQuery))
                {
                    using (ManagementObjectCollection collection = objectSearcher.Get())
                    {
                        // Execute the query
                        using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator enumerator = collection.GetEnumerator())
                        {
                            if (enumerator.MoveNext())
                            {
                                string message = (string)enumerator.Current["Message"];
                                string[] insertionStrings = (string[])enumerator.Current["InsertionStrings"];

                                if (message == null)
                                {
                                    if (insertionStrings.Length > 0)
                                    {
                                        StringBuilder sb = new StringBuilder();

                                        for (int i = 0; i < insertionStrings.Length; i++)
                                        {
                                            sb.Append(insertionStrings[i]);
                                            sb.Append(" ");
                                        }

                                        return sb.ToString();
                                    }
                                    else
                                    {
                                        return String.Empty;
                                    }
                                }
                                else
                                {
                                    return message;
                                }
                            }
                        }
                    }
                }

                return "Message not found.";
            }
        }

        private ManagementObjectCollection GetEvents()
        {
            lock (wmiLockObject)
            {
                ManagementScope scope = new ManagementScope(GetStandardPath());

                scope.Connect();

                StringBuilder query = new StringBuilder();
                query.Append("select EventType, TimeWritten, Category, SourceName, EventIdentifier, RecordNumber, Message from Win32_NTLogEvent where LogFile ='");
                query.Append(log.LogDisplayName.Replace("'", "''"));
                query.Append("'");

                if (currentSearch != String.Empty)
                {
                    query.Append(" AND Message LIKE '%");
                    AnalyzeAndFixSearch(ref currentSearch);
                    query.Append(currentSearch);
                    query.Append("%'");
                    /*
                    query.Append("%' OR InsertionStrings LIKE '%");
                    query.Append(currentSearch);
                    query.Append("%')");
                     */
                }

                if (dateFilter != DateFilterType.All_Records)
                {
                    if (dateFilter == DateFilterType.Last_2_Hours)
                        query.Append(" AND TimeWritten >= '" + DateTime.Now.AddHours(-2).ToShortDateString() + " " +
                            DateTime.Now.AddHours(-2).ToShortTimeString() + "'");
                    else if (dateFilter == DateFilterType.Last_12_Hours)
                        query.Append(" AND TimeWritten >= '" + DateTime.Now.AddHours(-12).ToShortDateString() + " " +
                            DateTime.Now.AddHours(-12).ToShortTimeString() + "'");
                    else
                        query.Append(" AND TimeWritten >= '" + DateTime.Now.AddDays(-7).ToShortDateString() + " " +
                            DateTime.Now.AddDays(-7).ToShortTimeString() + "'");
                }

                System.Management.ObjectQuery objectQuery = new System.Management.ObjectQuery(
                  query.ToString()
                );

                using (ManagementObjectSearcher objectSearcher = new ManagementObjectSearcher(scope, objectQuery))
                {
                    //Execute the query
                    ManagementObjectCollection collection = objectSearcher.Get();

                    return collection;
                }
            }
        }

        private string GetStandardPath()
        {
            return String.Concat(
                  @"\\",
                  log.MachineName,
                  @"\root\CIMV2"
            );
        }

        private void InsertToGrid(EventLogEntry e)
        {
            //DataSet currentEntries = (DataSet)bindingSource.DataSource;
            //DataTable table = ((DataSet)bindingSource.DataSource).Tables["Entries"];
            dgEvents.SuspendLayout();
            if (currentTable == null) return;
            InsertTableRowAt(currentTable, e, 0);
            currentView.Table = currentTable;
            dgEvents.ResumeLayout();
            //dgEvents.Refresh();
            dgEvents.Rows.Insert(0, 1);
            dgEvents.Rows[0].Selected = true;
            dgEvents.CurrentCell = dgEvents.Rows[0].Cells[0];
            FillMessage(dgEvents.Rows[0]);
            UpdateCounterButtons();
            //Debug.WriteLine("Exiting InsertToGrid");
        }

        private void InitDataSet()
        {
            //currentEntries.Tables.Add(currentTable);
        }

        private void FillGrid()
        {
            lock (lockReadWrite)
            {
                //Debug.WriteLine("Entering FillGrid");

                this.Cursor = Cursors.WaitCursor;

                Application.DoEvents();

                this.SuspendLayout();
                this.dgEvents.SuspendLayout();

                // Reset the counters for event log types
                ResetCounters();

                try
                {
                    //dgEvents.SelectionChanged -= dgEvents_SelectionChanged;
                    //dgEvents.CellFormatting -= dgEvents_CellFormatting;

                    // Retrieve the Event Log Entries via WMI
                    using (ManagementObjectCollection entries = GetEvents())
                    {
                        currentEntries = BuildDataSet(entries);
                        currentTable = currentEntries.Tables[0];
                    }

                    //if (bindingSource == null)
                    //{
                    //    bindingSource = new BindingSource(currentEntries, "Entries");
                    //}
                    //else
                    //{
                    //    bindingSource.DataSource = currentEntries;
                    //}
                    if (currentView == null)
                        currentView = new DataView(currentTable);
                    else
                        currentView.Table = currentTable;

                    //UpdateBindingSourceFilter();
                    UpdateDataViewFilter();
                    //dgEvents.DataSource = bindingSource;
                    //dgEvents.CellFormatting += dgEvents_CellFormatting;
                    dgEvents.Refresh();

                    UpdateCounterButtons();
                    //dgEvents.SelectionChanged += dgEvents_SelectionChanged;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Exception caught : " + ex.ToString(), "EventLog Viewer", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    this.Cursor = Cursors.Default;

                    this.dgEvents.ResumeLayout();
                    this.ResumeLayout();

                    this.Focus();
                }
                //Debug.WriteLine("Exiting FillGrid");
            }
        }

        private void FillSourceItems()
        {
            FillSourceItems(groupNames);
        }

        private void UpdateCountersAndButtons(ManagementBaseObject entry)
        {
            UpdateCounters(entry);
            UpdateCounterButtons();
        }

        private void ResetCounters()
        {
            countError = 0;
            countFailureAudit = 0;
            countInformation = 0;
            countSuccessAudit = 0;
            countWarning = 0;
        }

        //private void UpdateBindingSourceFilter()
        //{
        //    bindingSource.Filter = GetBindingSourceFilterString();

        //    if (dgEvents.SortedColumn == null)
        //    {
        //        currentSort = null;
        //    }
        //    else
        //    {
        //        if (dgEvents.SortOrder != SortOrder.None)
        //        {
        //            currentSort = dgEvents.SortedColumn.Name +
        //                (dgEvents.SortOrder == SortOrder.Ascending ? " ASC" : " DESC");
        //        }
        //        else
        //        {
        //            currentSort = null;
        //        }
        //    }

        //    if (currentSort == null)
        //    {
        //        bindingSource.Sort = "Index DESC";
        //    }
        //    else
        //    {
        //        bindingSource.Sort = currentSort;
        //    }
        //}

        private void UpdateDataViewFilter()
        {
            dgEvents.Rows.Clear();
            currentView.RowFilter = GetBindingSourceFilterString();

            if (dgEvents.SortedColumn == null)
            {
                currentSort = null;
            }
            else
            {
                if (dgEvents.SortOrder != SortOrder.None)
                {
                    currentSort = dgEvents.SortedColumn.Name +
                        (dgEvents.SortOrder == SortOrder.Ascending ? " ASC" : " DESC");
                }
                else
                {
                    currentSort = null;
                }
            }

            if (currentSort == null)
            {
                //bindingSource.Sort = "Index DESC";
                currentView.Sort = "Index DESC";
            }
            else
            {
                //bindingSource.Sort = currentSort;
                currentView.Sort = currentSort;
            }

            if (currentView.Count > 0)
                dgEvents.Rows.Add();    // add first blank row
            if (currentView.Count > 1)
                dgEvents.Rows.AddCopies(0, currentView.Count - 1);  // add remaining rows
        }

        private string GetBindingSourceFilterString()
        {
            List<string> filterTypes = new List<string>();

            if (btnErrors.Checked)
            {
                filterTypes.Add("(EntryType='Error')");
            }

            if (btnFailureAudit.Checked)
            {
                filterTypes.Add("(EntryType='FailureAudit')");
            }

            if (btnInformation.Checked)
            {
                filterTypes.Add("(EntryType='Information')");
            }

            if (btnSuccessAudit.Checked)
            {
                filterTypes.Add("(EntryType='SuccessAudit')");
            }

            if (btnWarnings.Checked)
            {
                filterTypes.Add("(EntryType='Warning')");
            }

            StringBuilder sb = new StringBuilder();

            if (dlFilterSource.SelectedIndex != 0)
            {
                sb.Append("(Source='" + ((string)dlFilterSource.SelectedItem).Replace("'", "''") + "')");
            }

            if (dlFilterCategory.SelectedIndex != 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND ");
                }
                sb.Append("(Category='" + ((string)dlFilterCategory.SelectedItem) + "')");
            }

            if (filterTypes.Count > 0)
            {
                if (sb.Length > 0)
                {
                    sb.Append(" AND (");
                }
                else
                {
                    sb.Append("(");
                }

                sb.Append(filterTypes[0]);

                for (int i = 1; i < filterTypes.Count; i++)
                {
                    sb.Append(" OR ");
                    sb.Append(filterTypes[i]);
                }

                sb.Append(")");
            }

            return sb.ToString();
        }

        private void AnalyzeAndFixSearch(ref string currentSearch)
        {
            for (int i = 0; i < currentSearch.Length; i++)
            {
                if (Char.IsPunctuation(currentSearch[i]))
                {
                    currentSearch = currentSearch.Replace(System.Convert.ToString(currentSearch[i]), String.Empty);
                    i--;
                }
            }

            tbSearch.Text = currentSearch;
        }

        private void UpdateCounterButtons()
        {
            btnErrors.Text = countError.ToString() + " Errors";
            btnErrors.ToolTipText = btnErrors.Text;

            btnFailureAudit.Text = countFailureAudit.ToString() + " Failure Audits";
            btnFailureAudit.ToolTipText = btnFailureAudit.Text;

            btnInformation.Text = countInformation.ToString() + " Information";
            btnInformation.ToolTipText = btnInformation.Text;

            btnSuccessAudit.Text = countSuccessAudit.ToString() + " Success Audits";
            btnSuccessAudit.ToolTipText = btnSuccessAudit.Text;

            btnWarnings.Text = countWarning.ToString() + " Warnings";
            btnWarnings.ToolTipText = btnWarnings.Text;
        }

        private DataSet BuildDataSet(ManagementObjectCollection entries)
        {
            // Create the dataset that the datagrid is going to bind to
            DataSet ds = new DataSet();
            DataTable table = new DataTable("Entries");

            table.Columns.Add("EntryType");
            table.Columns.Add("TimeWritten", typeof(DateTime));
            table.Columns.Add("Category");
            table.Columns.Add("Source");
            table.Columns.Add("EventID", typeof(uint));
            table.Columns.Add("Message");
            table.Columns.Add("CompleteMessage");
            table.Columns.Add("Index", typeof(uint));

            ds.Tables.Add(table);

            groupNames = new List<String>();

            // Enumerate through the events and add each item to the dataset
            using (System.Management.ManagementObjectCollection.ManagementObjectEnumerator enumerator = entries.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    ManagementBaseObject entry = enumerator.Current;

                    if (!groupNames.Contains((string)(entry["SourceName"])))
                    {
                        groupNames.Add((string)(entry["SourceName"]));
                    }

                    AddTableRow(table, entry);

                    //Application.DoEvents();
                }
            }

            FillSourceItems(groupNames);
            FillCategoryItems();

            return ds;
        }

        private void FillSourceItems(List<string> groupNames)
        {
            loadingGroups = true;

            dlFilterSource.BeginUpdate();

            try
            {
                dlFilterSource.Items.Clear();
                dlFilterSource.Items.Add("<All Sources>");

                groupNames.Sort();

                for (int i = 0; i < groupNames.Count; i++)
                {
                    dlFilterSource.Items.Add(groupNames[i]);
                }
            }
            catch { }
            finally
            {
                dlFilterSource.EndUpdate();
            }

            if (previousGroupFilter == null)
            {
                dlFilterSource.SelectedIndex = 0;
            }
            else
            {
                dlFilterSource.SelectedItem = previousGroupFilter;
            }

            loadingGroups = false;
        }

        private void FillCategoryItems()
        {
            loadingCategories = true;

            dlFilterCategory.BeginUpdate();

            dlFilterCategory.Items.Clear();
            dlFilterCategory.Items.Add("<All Categories>");

            foreach (string s in System.Enum.GetNames(typeof(VtiEventCatType)))
                dlFilterCategory.Items.Add(s.Replace('_', ' '));

            if (previousCategoryFilter == null)
            {
                dlFilterCategory.SelectedIndex = 0;
            }
            else
            {
                dlFilterCategory.SelectedItem = previousCategoryFilter;
            }

            loadingCategories = false;
        }

        private string GetEventTypeString(NTLogEvent.EventTypeValues val)
        {
            switch (val)
            {
                case NTLogEvent.EventTypeValues.Error:
                    return EventTypeDescription.Error;

                case NTLogEvent.EventTypeValues.Warning:
                    return EventTypeDescription.Warning;

                case NTLogEvent.EventTypeValues.Information:
                    return EventTypeDescription.Information;

                case NTLogEvent.EventTypeValues.Security_audit_success:
                    return EventTypeDescription.SuccessAudit;

                case NTLogEvent.EventTypeValues.Security_audit_failure:
                    return EventTypeDescription.FailureAudit;

                default:
                    return EventTypeDescription.Unknown;
            }
        }

        private string GetVtiEventCatString(object CatID)
        {
            string cat;
            cat = ((VtiEventCatType)Convert.ToInt16(CatID)).ToString().Replace('_', ' ');
            return cat;
        }

        //private string GetVtiEventCatString(VtiEventCat cat)
        //{
        //    switch (cat)
        //    {
        //        case VtiEventCat.Application_Error:
        //            return VtiEventCatDescription.Application_Error;
        //        case VtiEventCat.Calibration:
        //            return VtiEventCatDescription.Calibration;
        //        case VtiEventCat.Login:
        //            return VtiEventCatDescription.Login;
        //        case VtiEventCat.Manual_Command:
        //            return VtiEventCatDescription.Manual_Command;
        //        case VtiEventCat.None:
        //            return VtiEventCatDescription.None;
        //        case VtiEventCat.Parameter_Update:
        //            return VtiEventCatDescription.Parameter_Update;
        //        case VtiEventCat.Test_Cycle:
        //            return VtiEventCatDescription.Test_Cycle;
        //        default:
        //            return VtiEventCatDescription.Unknown;
        //    }
        //}

        private void AddTableRow(DataTable table, ManagementBaseObject entry)
        {
            DataRow row = table.NewRow();

            row["EntryType"] = GetEventTypeString(((NTLogEvent.EventTypeValues)(System.Convert.ToInt32(entry["EventType"]))));
            row["TimeWritten"] = WMIUtil.ToDateTime(((string)(entry["TimeWritten"])));
            //row["Category"] = ((ushort)(entry["Category"]));
            try
            {
                //row["Category"] = ((VtiEventCat)(entry["Category"])).ToString().Replace('_', ' ');
                row["Category"] = this.GetVtiEventCatString(entry["Category"]);
            }
            catch
            {
                row["Category"] = entry["Category"].ToString();
            }
            row["Source"] = ((string)(entry["SourceName"]));
            row["EventID"] = ((uint)(entry["EventIdentifier"]));
            row["Index"] = ((uint)(entry["RecordNumber"]));
            //row["Message"] = ((string)(entry["Message"]));// String.Empty;
            try
            {
                row["Message"] = ((string)(entry["Message"])).Split('\n')[0].Trim();
            }
            catch
            {
                row["Message"] = ((string)(entry["Message"]));
            }
            row["CompleteMessage"] = ((string)(entry["Message"]));

            table.Rows.Add(row);

            UpdateCounters(entry);
        }

        private void InsertTableRowAt(DataTable table, EventLogEntry entry, int pos)
        {
            DataRow row = table.NewRow();

            row["EntryType"] = entry.EntryType.ToString();
            row["TimeWritten"] = entry.TimeWritten;
            try
            {
                row["Category"] = this.GetVtiEventCatString(entry.CategoryNumber);
            }
            catch
            {
                row["Category"] = entry.CategoryNumber.ToString();
            }
            row["Source"] = entry.Source;
            row["EventID"] = entry.InstanceId;
            row["Index"] = entry.Index;
            try
            {
                row["Message"] = entry.Message.Split('\n')[0].Trim();
            }
            catch
            {
                row["Message"] = entry.Message;
            }
            row["CompleteMessage"] = entry.Message;

            table.Rows.InsertAt(row, pos);

            UpdateCounters(entry);
        }

        private void UpdateCounters(EventLogEntry entry)
        {
            switch (entry.EntryType)
            {
                case EventLogEntryType.Error:
                    countError++;
                    break;

                case EventLogEntryType.FailureAudit:
                    countFailureAudit++;
                    break;

                case EventLogEntryType.Information:
                    countInformation++;
                    break;

                case EventLogEntryType.SuccessAudit:
                    countSuccessAudit++;
                    break;

                case EventLogEntryType.Warning:
                    countWarning++;
                    break;

                default:
                    break;
            }
        }

        private void UpdateCounters(ManagementBaseObject entry)
        {
            switch (((NTLogEvent.EventTypeValues)(System.Convert.ToInt32(entry["EventType"]))))
            {
                // Update the counters for the entry type
                case NTLogEvent.EventTypeValues.Error:
                    countError++;
                    break;

                case NTLogEvent.EventTypeValues.Security_audit_failure:
                    countFailureAudit++;
                    break;

                case NTLogEvent.EventTypeValues.Information:
                    countInformation++;
                    break;

                case NTLogEvent.EventTypeValues.Security_audit_success:
                    countSuccessAudit++;
                    break;

                case NTLogEvent.EventTypeValues.Warning:
                    countWarning++;
                    break;

                default:
                    break;
            }
        }

        private void UpdateEventDetailText(DataGridViewRow row, string message)
        {
            row.Cells[7].Value = message;
            tbEventDetail.Text = message;
        }

        private void toolStripButtonClearEventLog_Click_1(object sender, EventArgs e)
        {
            //if (Settings.Default.ShowConfirmationsForCleaningEventLogs)
            //{
            //    if (MessageBox.Show("Are you sure you want to clear this log?", "Smoothy", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            //    {
            //        ClearLog();
            //    }
            //}
            //else
            //{
            ClearLog();
            //}
        }

        private void ClearLog()
        {
            log.Clear();
            RefreshGrid();
        }

        #endregion Private Members

        #region Public Members

        /// <summary>
        /// Refreshes the event grid on the form.
        /// </summary>
        public void RefreshGrid()
        {
            FillGrid();
        }

        /// <summary>
        /// Adds an <see cref="EventLogEntry">EventLogEntry</see> to the form.
        /// </summary>
        /// <param name="e"><see cref="EventLogEntry">EventLogEntry</see> to add.</param>
        public void AddToGrid(EventLogEntry e)
        {
            InsertToGrid(e);
        }

        /// <summary>
        /// Shows the Event Log Viewer Form
        /// </summary>
        public new void Show()
        {
            //dgEvents.CellFormatting -= dgEvents_CellFormatting;
            //dgEvents.SelectionChanged -= dgEvents_SelectionChanged;
            base.Show();
            //dgEvents.CellFormatting += dgEvents_CellFormatting;
            //dgEvents.SelectionChanged += dgEvents_SelectionChanged;
        }

        /// <summary>
        /// Shows the Event Log Viewer Form
        /// </summary>
        /// <param name="MdiParent">Parent for the form</param>
        public void Show(Form MdiParent)
        {
            this.MdiParent = MdiParent;
            this.Show();
            this.BringToFront();
        }

        #endregion Public Members

        #region Events

        internal void VtiLog_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            try
            {
                //if (this.Visible)
                if (this.InvokeRequired)
                    this.Invoke(new EventLogDelegate(this.AddToGrid), e.Entry);
                else
                    this.AddToGrid(e.Entry);
            }
            catch
            {
            }
        }

        private void log_EntryWritten(object sender, EntryWrittenEventArgs e)
        {
            /*
            lock (lockReadWrite)
            {
              EventLogEntry entry = e.Entry;

              if (!groupNames.Contains(entry.Source))
              {
                groupNames.Add(entry.Source);

                if (this.InvokeRequired)
                {
                  this.Invoke(new MethodInvoker(FillSourceItems));
                }
                else
                {
                  FillSourceItems(groupNames);
                }
              }

              AddTableRow(currentEntries.Tables[0], entry);

              if (this.InvokeRequired)
              {
                this.Invoke(new EventLogDelegate(UpdateCountersAndButtons), new object[] { entry });
              }
              else
              {
                UpdateCountersAndButtons(entry);
              }
            }
             */
        }

        private void EventLogViewer_Load(object sender, EventArgs e)
        {
        }

        private void toolStripButtonClearEventLog_Click(object sender, EventArgs e)
        {
            this.log.Clear();
        }

        private void On_FilterButton_CheckChanged(object sender, EventArgs e)
        {
            //UpdateBindingSourceFilter();
            UpdateDataViewFilter();
        }

        private void dlFilterSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingGroups)
            {
                //UpdateBindingSourceFilter();
                UpdateDataViewFilter();
                previousGroupFilter = (string)dlFilterSource.SelectedItem;
            }
        }

        private void EventLogViewer_KeyUp(object sender, KeyEventArgs e)
        {
            if (!log.EnableRaisingEvents)
            {
                if (e.KeyCode == Keys.F5)
                {
                    RefreshGrid();
                }
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DoSearch();
        }

        private void btnClearSearch_Click(object sender, EventArgs e)
        {
            if (btnClearSearch.Checked)
            {
                if (btnSearch.Checked)
                {
                    btnSearch.Checked = false;
                    currentSearch = String.Empty;
                    tbSearch.Text = String.Empty;
                    FillGrid();
                }
            }

            btnClearSearch.Checked = true;
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {
            RefreshGrid();
        }

        private void toolStripButtonAutoRefresh_Click(object sender, EventArgs e)
        {
            SetAutoRefreshEnabled(toolStripButtonAutoRefresh.Checked);
        }

        #region Grid Events

        //private void dgEvents_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        //{
        //    // If the typeimage column is being formatted
        //    //if (dgEvents.Columns[e.ColumnIndex].Name == "TypeImage")
        //    if (e.ColumnIndex == 0)
        //    {
        //        Debug.WriteLine("CellFormatting row: " + e.RowIndex.ToString());
        //        // Get the type from the entrytype column

        //        object objType = dgEvents["EntryType", e.RowIndex].Value;

        //        if (objType != null)
        //        {
        //            switch (objType.ToString())
        //            {
        //                case "Information":
        //                    e.Value = VTIWindowsControlLibrary.Properties.Resources.information;
        //                    break;
        //                case "Warning":
        //                    e.Value = VTIWindowsControlLibrary.Properties.Resources.warning;
        //                    break;
        //                case "Error":
        //                    e.Value = VTIWindowsControlLibrary.Properties.Resources.cancel;
        //                    break;
        //            }
        //            e.FormattingApplied = true;
        //        }
        //    }
        //}

        private void dgEvents_KeyUp(object sender, KeyEventArgs e)
        {
            if (!log.EnableRaisingEvents)
            {
                if (e.KeyCode == Keys.F5)
                {
                    RefreshGrid();
                }
            }
        }

        private void dgEvents_SelectionChanged(object sender, EventArgs e)
        {
            if (dgEvents.SelectedRows.Count > 0)
            {
                // Get the current select item
                DataGridViewRow selectedRow = dgEvents.SelectedRows[0];

                if (selectedRow.Index != -1)
                {
                    lastIndex = (uint)selectedRow.Cells["Index"].Value;

                    // Do this asynchronously
                    FillMessageDelegate del = new FillMessageDelegate(FillMessage);
                    IAsyncResult result = del.BeginInvoke(selectedRow, null, null);
                }
            }
        }

        private void dgEvents_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            int startIndex = e.RowIndex;
            int rowCount = e.RowCount;
            int endIndex = startIndex + rowCount;

            for (int i = startIndex; i < endIndex; i++)
            {
                DataGridViewRow row = dgEvents.Rows[i];
                string strType = (string)row.Cells["EntryType"].Value;

                if (strType == "Information")
                {
                    row.DefaultCellStyle = cellStyleInformation;
                }
                else if (strType == "Warning")
                {
                    row.DefaultCellStyle = cellStyleWarning;
                }
                else if (strType == "Error")
                {
                    row.DefaultCellStyle = cellStyleError;
                }
                //else if (strType == "FailureAudit")
                //{
                //    row.DefaultCellStyle = cellStyleFailureAudit;

                //}
                //else if (strType == "SuccessAudit")
                //{
                //    row.DefaultCellStyle = cellStyleSuccessAudit;
                //}
            }
        }

        #endregion Grid Events

        private void tbSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Avoid the possibility of invalid characters in the
            // filtering query by eliminating punctuation characters.
            if (Char.IsPunctuation(e.KeyChar))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void tbSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                DoSearch();
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            // Copy the current message text to the clipboard
            Clipboard.SetText(tbEventDetail.Text, TextDataFormat.Text);
        }

        private void dlFilterCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingCategories)
            {
                //UpdateBindingSourceFilter();
                UpdateDataViewFilter();
                previousCategoryFilter = (string)dlFilterCategory.SelectedItem;
            }
        }

        private void dlFilterTimeWritten_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!loadingDateFilter)
            {
                //dateFilter = (DateFilterType)System.Enum.Parse(typeof(DateFilterType), dlFilterTimeWritten.Text.Replace(' ', '_'));
                dateFilter = Enum<DateFilterType>.Parse(dlFilterTimeWritten.Text.Replace(' ', '_'));
                FillGrid();
            }
        }

        private void EventLogViewerForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                this.Hide();
                e.Cancel = true;
            }
        }

        #endregion Events

        private void dgEvents_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < currentView.Count)
            {
                if (e.ColumnIndex == 0)
                {
                    switch (currentView[e.RowIndex][0].ToString())
                    {
                        case "Information":
                            e.Value = VTIWindowsControlLibrary.Properties.Resources.information;
                            break;

                        case "Warning":
                            e.Value = VTIWindowsControlLibrary.Properties.Resources.warning;
                            break;

                        case "Error":
                            e.Value = VTIWindowsControlLibrary.Properties.Resources.cancel;
                            break;
                    }
                }
                else
                {
                    e.Value = currentView[e.RowIndex][e.ColumnIndex - 1];
                }
            }
        }
    }
}