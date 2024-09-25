# VTI Windows Control Library

## Revision History

### 9/22/22 1.4.1.15 - NJ

- Fixed bad config parameters which appeared in edit cycle config file for SerialPortParameters and EthernetPortParameters (duplicated BaudRate, ProcessValue, IpAddress, etc. tags).
- Added TorrconIV ethernet component.
- Added new AnalogSignal(String Label) override for simple string-valued AnalogSignals. When assigning a value to the signal, use SignalName.StringValue = "value".
- Improved efficiency of data plot when dragging cursor over traecs to view historical signal values.
- When a config file is missing/corrupt and the ConfigBackupSelectForm appears, if there is a good automatic backup, select the most recent one. Otherwise, if there is a good user-saved config file, select the most recent one.
- ParameterChangeLog now records EthernetPortParameter changes.
- Can now press the Enter key when EditCycleForm is focused as an alternative to clicking the OK button on the form.
- Added checking to make sure EthernetPortParameter has the PortName, Port, and IPAddress tags.
- Changed .NET Framework from 3.5 to 4.0 to allow for parallel for loop method used in TorrconIV ethernet component which scans for TorrconIV IP addresses on the local network.

```
Name                                      Size      Modified                 Name                                      Size      Modified
------------------------------------------------------------------------------------------------------------------------------------------------------
Classes                                   2,604,686 9/22/2022 9:29:02 AM     Classes                                   2,488,964 7/26/2022 10:08:54 AM
+Configuration                              189,960 9/22/2022 9:29:00 AM     +Configuration                              189,536 7/26/2022 10:08:52 AM
|+EditCycleParameter.cs                      16,687 8/30/2022 8:09:50 AM  >> |+EditCycleParameter.cs                      16,634 2/9/2022 8:54:10 AM
|+EthernetPortParameter.cs                   10,576 8/30/2022 9:04:25 AM  >> |+EthernetPortParameter.cs                   10,367 10/7/2020 4:11:14 PM
|\SerialPortParameter.cs                     21,305 8/19/2022 1:18:57 PM  >> |\SerialPortParameter.cs                     21,143 11/9/2021 6:21:00 PM
\IO                                       1,787,088 9/22/2022 9:29:02 AM     \IO                                       1,671,790 7/26/2022 10:08:54 AM
 +EthernetIO                                159,543 9/22/2022 9:29:01 AM      +EthernetIO                                 45,945 7/26/2022 10:08:52 AM
 |+EthernetIOBase.cs                         15,965 9/1/2022 2:18:46 PM   >>  ||
 |+EthernetIOBase.Designer.cs                 1,416 8/30/2022 11:09:03 AM >>  ||
 |+EthernetIOBase.resx                        6,400 8/18/2022 8:16:07 AM  >>  ||
 |+KeithleyDMM6500.cs                        20,925 9/1/2022 2:27:33 PM   >>  |\KeithleyDMM6500.cs                        20,569 10/7/2020 4:13:16 PM
 |\TorrconIV.cs                              89,274 9/15/2022 2:40:22 PM  >>  |
 +SerialIO                                1,476,156 9/22/2022 9:29:02 AM      +SerialIO                                1,475,760 7/26/2022 10:08:54 AM
 |+RS485ModbusInterface.cs                   14,677 8/19/2022 1:15:23 PM  >>  |+RS485ModbusInterface.cs                   14,627 4/26/2021 4:05:06 PM
 |\SerialIOBase.cs                           16,244 9/22/2022 10:28:57 AM >>  |\SerialIOBase.cs                           15,898 6/8/2021 10:14:16 AM
 \AnalogSignal.cs                            24,009 8/16/2022 10:39:44 AM >>  \AnalogSignal.cs                            22,705 2/5/2021 2:08:42 PM
Components                                1,044,616 9/22/2022 9:29:03 AM     Components                                1,041,301 7/26/2022 10:08:56 AM
+Graphing                                   445,258 9/22/2022 9:29:03 AM     +Graphing                                   442,234 7/26/2022 10:08:56 AM
|\GraphControl.cs                           151,813 9/22/2022 10:33:29 AM >> |\GraphControl.cs                           148,928 6/1/2021 12:13:50 PM
\SchematicCheckBox.cs                        28,048 8/29/2022 10:30:55 AM >> \SchematicCheckBox.cs                        27,757 2/5/2021 2:08:44 PM
Forms                                     1,404,593 9/22/2022 9:29:05 AM     Forms                                     1,401,506 7/26/2022 10:12:28 AM
+ConfigBackupSelectForm.cs                    7,520 9/20/2022 3:25:48 PM  >> +ConfigBackupSelectForm.cs                    7,187 9/1/2021 11:54:24 AM
+EditCycleForm.cs                            79,533 9/22/2022 9:18:31 AM  >> +EditCycleForm.cs                            78,049 2/8/2022 6:06:34 PM
+EditCycleForm.designer.cs                   14,941 9/22/2022 9:15:34 AM  >> +EditCycleForm.designer.cs                   13,778 2/5/2021 2:08:44 PM
+EditCycleForm.resx                           8,314 9/22/2022 9:15:34 AM  >> +EditCycleForm.resx                           8,314 2/5/2021 2:08:44 PM
\EditCycleSearchForm.cs                      54,107 8/30/2022 8:09:50 AM  >> \EditCycleSearchForm.cs                      54,000 2/5/2021 2:08:44 PM
Required Client Code                         21,320 9/22/2022 9:29:06 AM     Required Client Code                         21,271 7/26/2022 10:08:58 AM
\1.4.1.1 Add RestartSerialInDevices().txt     2,338 7/28/2022 8:20:24 AM  << \1.4.1.1 Add RestartSerialInDevices().txt     2,289 8/12/2022 11:38:08 AM
app.config                                    8,197 9/15/2022 10:21:07 AM >> app.config                                    8,170 7/20/2022 10:04:12 AM
VtiLib.cs                                    21,037 8/17/2022 3:18:08 PM  >> VtiLib.cs                                    18,852 11/10/2021 8:12:36 AM
VTIWindowsControlLibrary.csproj              59,258 9/15/2022 10:21:07 AM >> VTIWindowsControlLibrary.csproj              58,641 7/20/2022 10:04:12 AM
------------------------------------------------------------------------------------------------------------------------------------------------------
```


### 7/20/22 1.4.1.14 - NJ

- Updated inquire form to be able to display extra custom columns in dbo.UutRecords and dbo.UutRecordDetails when loading records from remote database.

```
Name                             Size       Modified                Name                             Size       Modified
--------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary         37,333,482                         vtiwindowscontrollibrary         37,090,786
+Forms                            1,401,179 7/20/2022 9:51:22 AM    +Forms                            1,414,481 6/22/2022 2:12:24 PM
|+InquireForm.cs                     26,474 7/20/2022 9:51:22 AM >> |+InquireForm.cs                     15,604 12/21/2021 2:51:32 PM
|+InquireForm.Designer.cs            25,519 7/20/2022 9:27:02 AM >> |+InquireForm.Designer.cs            40,489 12/21/2021 2:51:32 PM
|\InquireForm.resx                    6,421 7/20/2022 9:27:02 AM >> |\InquireForm.resx                   15,623 12/21/2021 2:51:32 PM
+app.config                           8,199 7/20/2022 9:01:16 AM >> +app.config                           8,223 11/16/2021 12:42:12 PM
\VTIWindowsControlLibrary.csproj     58,643 7/20/2022 9:01:16 AM >> \VTIWindowsControlLibrary.csproj     58,641 12/21/2021 3:00:00 PM
--------------------------------------------------------------------------------------------------------------------------------------
```

### 6/22/22 1.4.1.13 - NJ

-Added dock button to undocked test history form.
-Changed auto-sizing of permission form.

```
Name                               Size       Modified                 Name                               Size       Modified
-----------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary           37,089,169                          vtiwindowscontrollibrary           36,196,623
+Components                         1,041,301 6/22/2022 2:12:22 PM     +Components                         1,040,152 5/3/2022 9:20:52 AM
|\SimpleDockControl.cs                 22,452 6/22/2022 2:07:02 PM  >> |\SimpleDockControl.cs                 21,303 2/9/2022 9:02:06 AM
\Forms                              1,414,481 6/22/2022 2:12:24 PM     \Forms                              1,414,093 5/3/2022 9:20:54 AM
 +OperatorFormDualNested2.cs           27,141 6/22/2022 2:11:26 PM  >>  +OperatorFormDualNested2.cs           27,137 2/5/2021 2:08:44 PM
 +OperatorFormSingleNested.cs          18,275 6/22/2022 7:15:36 AM  >>  +OperatorFormSingleNested.cs          18,273 4/28/2022 4:37:20 PM
 +OperatorFormSingleNestedNoSeq.cs     18,507 6/22/2022 7:15:36 AM  >>  +OperatorFormSingleNestedNoSeq.cs     18,505 4/12/2021 1:01:00 PM
 \PermissionsForm.cs                   18,610 6/17/2022 11:39:08 AM >>  \PermissionsForm.cs                   18,230 9/1/2021 1:45:22 PM
-----------------------------------------------------------------------------------------------------------------------------------------
```

### 5/3/22 1.4.1.12 - NJ

-Added a splitter between the mini command toolbar and the prompt in the OperatorFormSingleNested.cs form, which the user can drag to make the Mini Command Toolbar wider.
-Mini command toolbar width is also automatically adjusted to fit the command buttons.
-Buttons now wrap inside the toolbar.

To make the Mini Command Toolbar visible, in Machine.cs, in InitializeOperatorForm():
Under:
	_OpFormSingle = new OperatorFormSingleNested(Properties.Settings.Default.PortNames[0], Properties.Settings.Default.PortColors[0], _MainForm);
Add:
	_OpFormSingle.CommandWindowVisible = true;
        _OpFormSingle.Commands.Add(ManualCommands.StartTest);
        _OpFormSingle.Commands.Add(ManualCommands.Reset);
	_OpFormSingle.Commands.Add(ManualCommands.Acknowledge);

To disable automatic adjusting of Mini Command Toolbar width when a command is added or removed:
under:
_OpFormSingle.CommandWindowVisible = true;
add:
_OpFormSingle.AutoAdjustCommandWindowWidth = false;

Folder Compare
Produced: 5/3/2022 9:23:55 AM

```
Name                                                      Size      Modified                 Name                                                      Size      Modified
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Components                                                1,040,152 5/3/2022 9:20:50 AM      Components                                                1,037,816 4/22/2022 7:55:44 AM
\MiniCommandControl.cs                                       20,406 4/28/2022 11:33:22 AM >> \MiniCommandControl.cs                                       18,070 8/11/2021 1:09:58 PM
Forms                                                     1,414,093 5/3/2022 9:20:52 AM      Forms                                                     1,413,279 4/22/2022 7:55:44 AM
+OperatorFormSingleNested.cs                                 18,273 4/28/2022 4:37:20 PM  >> +OperatorFormSingleNested.cs                                 17,455 2/5/2021 2:08:44 PM
\OperatorFormSingleNested.Designer.cs                        20,291 4/28/2022 11:34:54 AM >> \OperatorFormSingleNested.Designer.cs                        20,295 2/5/2021 2:08:44 PM
Required Client Code                                         21,219 5/3/2022 9:20:54 AM      Required Client Code                                         11,544 4/22/2022 7:59:00 AM
+1.4.0.0 Adjust System Signal And Test History Width.txt      8,118 4/28/2022 1:14:26 PM  >> |
+1.4.0.6 Add ParamChangeLog Viewer Form to MainForm.txt       1,466 4/28/2022 9:57:22 AM  >> +1.4.0.6 Add ParamChangeLog Viewer Form to MainForm.txt       1,345 11/9/2021 6:20:28 PM
+1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt     1,463 4/28/2022 9:58:52 AM  >> \1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt     1,343 11/9/2021 6:20:34 PM
\1.4.1.12 Add Commands To Mini Commands Toolbar.txt           1,316 4/28/2022 4:01:30 PM  >>
_Revision History for VTIWindowsControlLibrary.txt           76,857 5/3/2022 9:20:18 AM   >> _Revision History for VTIWindowsControlLibrary.txt           75,851 4/22/2022 8:47:22 AM
-------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 4/22/22 1.4.1.11 - NJ

-Can now view historical states of digital inputs and digital outputs when clicking and dragging the mouse over the data plot.
	In Machine.cs -> InitializeDataPlot()
	To monitor only certain IO in your data plot: _DataPlot[Port.Blue].MonitoredIO.Add(IO.DOut.BlueEvac);
	To monitor all IO in your data plot except for the audible alarm and MCR24VPowerSense:
```
			var _DOutList = IO.DOut.GetType()
                     .GetFields()
                     .Select(field => field.GetValue(IO.DOut))
                     .Where(x => x != null)
                     .OfType<VTIWindowsControlLibrary.Classes.IO.Interfaces.IDigitalOutput>()
                     .Where(x => x.Description != string.Empty)
                     .ToList();

            foreach (var dout in _DOutList)
            {
                _DataPlot[Port.Blue].MonitoredIO.Add(dout);
                if (Properties.Settings.Default.DualPortSystem)
                {
                    _DataPlot[Port.White].MonitoredIO.Add(dout);
                }
            }
            _DataPlot[Port.Blue].MonitoredIO.Remove(IO.DOut.Alarm);
			if (Properties.Settings.Default.DualPortSystem)
            {
				_DataPlot[Port.White].MonitoredIO.Remove(IO.DOut.Alarm);
			}

            var _DInList = IO.DIn.GetType()
                    .GetFields()
                    .Select(field => field.GetValue(IO.DIn))
                    .Where(x => x != null)
                    .OfType<VTIWindowsControlLibrary.Classes.IO.Interfaces.IDigitalInput>()
                    .Where(x => x.Description != string.Empty)
                    .ToList();

            foreach (var din in _DInList)
            {
                _DataPlot[Port.Blue].MonitoredIO.Add(din);
                if (Properties.Settings.Default.DualPortSystem)
                {
                    _DataPlot[Port.White].MonitoredIO.Add(din);
                }
            }
            _DataPlot[Port.Blue].MonitoredIO.Remove(IO.DIn.MCRPower24VoltSense);
			if (Properties.Settings.Default.DualPortSystem)
            {
				_DataPlot[Port.White].MonitoredIO.Remove(IO.DIn.MCRPower24VoltSense);
			}
```	
	
-When saving data plot as CSV, exclude data such as comments, trace colors, comments, etc.
-Added required client code for text-based event logging

```
Name                                  Size       Modified                Name                       Size       Modified
-------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary              36,294,705                         vtiwindowscontrollibrary   36,074,262
+Classes                               2,488,964 4/22/2022 7:55:42 AM    +Classes                    2,488,878 2/9/2022 8:08:24 AM
|\ManualCommands                          29,942 4/22/2022 7:55:42 AM    |\ManualCommands               29,856 2/9/2022 8:08:24 AM
| \ManualCommandsBase.cs                  11,978 2/17/2022 4:08:26 PM >> | \ManualCommandsBase.cs       11,892 2/9/2022 8:01:04 AM
+Components                            1,037,816 4/22/2022 7:55:44 AM    +Components                 1,035,982 2/9/2022 8:08:26 AM
|\Graphing                               442,234 4/22/2022 7:55:44 AM    |\Graphing                    440,400 2/9/2022 8:08:26 AM
| +DataPlotControl.cs                    117,352 4/21/2022 5:59:00 PM >> | +DataPlotControl.cs         116,215 11/16/2021 11:23:56 AM
| \DataPlotGraphControl.cs                 4,845 2/9/2022 4:05:56 PM  >> | \DataPlotGraphControl.cs      4,148 2/5/2021 1:08:42 PM
\Required Client Code                      9,700 4/22/2022 7:59:00 AM    \Required Client Code           6,681 2/9/2022 8:08:28 AM
 \1.4.0.0 Text Event Logging only.txt      3,019 4/22/2022 7:58:52 AM >>
-------------------------------------------------------------------------------------------------------------------------------------
```

### 2/9/22 - 1.4.1.10 - NJ

-Can add analog signals to display during your CycleStep by adding, for example, "step.SignalsToDisplay.Add(signal.EvacPressure);" to your CycleStep started event.
-When using a Localization variable for your ManualCommand's displayed text, the variable name match is now case-insensitive and can use the method name instead of the displayed text.
	- If manual command method name is "ResetBlue()" and the displayed text is "RESET - BLUE PORT", the Localization variable must be ManualCommandResetBlue or ManualCommandRESETBLUEPORT (case-insensitive).
-When minimize button is clicked on the undocked version of the data plot, do not dock it. Only dock it if the dock button is clicked.
-Fixed automatic window sizing bug for Digital IO form.
-Can now double-click "Digital Inputs" or "Digital Outputs" label in Digital IO form to export digital IO list in CSV format (makes manual writing easier).

```
Name                            Size      Modified                  Name                            Size      Modified
-----------------------------------------------------------------------------------------------------------------------------------
Classes                         2,487,696 11/29/2021 12:21:44 PM    Classes                         2,466,426 2/8/2022 1:32:58 PM
+Configuration                    189,450 11/29/2021 12:21:42 PM    +Configuration                    189,606 2/8/2022 1:32:56 PM
|+EditCycleParameter.cs            16,548 6/8/2021 9:12:56 AM    << |+EditCycleParameter.cs            16,634 1/19/2022 11:08:12 AM
|\ModelSettingsBase.cs              6,706 2/5/2021 1:08:42 PM    << |\ModelSettingsBase.cs              6,714 1/19/2022 11:23:12 AM
+CycleSteps                       103,597 11/29/2021 12:21:42 PM    +CycleSteps                       104,001 2/8/2022 1:32:56 PM
|\CycleStep.cs                     50,035 6/8/2021 9:13:52 AM    << |\CycleStep.cs                     50,439 1/14/2022 9:22:52 AM
+Graphing                          91,886 11/29/2021 12:21:42 PM    +Graphing                          91,930 2/8/2022 1:32:57 PM
|\DataPlot                         27,447 11/29/2021 12:21:42 PM    |\DataPlot                         27,491 2/8/2022 1:32:57 PM
| \DataPlotTraceCollection.cs       4,965 2/5/2021 1:08:42 PM    << | \DataPlotTraceCollection.cs       5,009 1/26/2022 11:32:20 AM
\ManualCommands                    29,208 11/29/2021 12:21:44 PM    \ManualCommands                    29,856 2/8/2022 4:53:15 PM
 \ManualCommandsBase.cs            11,244 8/6/2021 6:51:38 AM    <<  \ManualCommandsBase.cs            11,892 2/8/2022 4:53:15 PM
Components                      1,035,877 11/29/2021 12:21:46 PM    Components                      1,035,982 2/8/2022 1:32:59 PM
\SimpleDockControl.cs              21,198 2/5/2021 1:08:44 PM    << \SimpleDockControl.cs              21,303 1/19/2022 3:09:06 PM
Forms                           1,413,907 11/29/2021 12:21:50 PM    Forms                           1,407,859 2/8/2022 5:06:33 PM
+DigitalIOForm.cs                  13,710 9/1/2021 2:31:32 PM    << +DigitalIOForm.cs                  15,673 2/4/2022 3:11:30 PM
+DigitalIOForm.Designer.cs          8,780 2/5/2021 1:08:44 PM    << +DigitalIOForm.Designer.cs          6,821 2/4/2022 2:54:32 PM
+DigitalIOForm.resx                20,470 2/5/2021 1:08:44 PM    << +DigitalIOForm.resx                20,770 2/4/2022 2:54:32 PM
+EditCycleForm.cs                  78,981 11/16/2021 11:50:52 AM << +EditCycleForm.cs                  78,049 2/8/2022 5:06:33 PM
-----------------------------------------------------------------------------------------------------------------------------------
```


### 12/21/21 - 1.4.1.9 - PBW

-Added Keyence scanner and added scan button to inquire form for use with serially-triggered barcode scanners.

```
Name                                                Size       Modified                  Name                                                Size       Modified
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary                            36,067,136                           vtiwindowscontrollibrary                            36,040,496
+Classes                                             2,487,696 11/29/2021 12:21:44 PM    +Classes                                             2,466,879 11/29/2021 12:21:44 PM
|+ClientForms                                           51,545 11/29/2021 12:21:42 PM    |+ClientForms                                           50,726 11/29/2021 12:21:42 PM
||\Inquire.cs                                            3,111 12/21/2021 1:51:32 PM  <> ||\Inquire.cs                                            2,292 8/2/2021 8:18:44 AM
|\IO                                                 1,671,790 11/29/2021 12:21:44 PM    |\IO                                                 1,651,792 11/29/2021 12:21:44 PM
| \SerialIO                                          1,475,760 11/29/2021 12:21:44 PM    | \SerialIO                                          1,455,762 11/29/2021 12:21:44 PM
|  +KeyenceScanner.cs                                   18,894 12/21/2021 1:51:32 PM  >> |
|  \KeyenceScanner.Designer.cs                           1,104 12/21/2021 1:51:32 PM  >> |
+Forms                                               1,413,907 11/29/2021 12:21:50 PM    +Forms                                               1,408,487 11/29/2021 12:21:50 PM
|+InquireForm.cs                                        15,604 12/21/2021 1:51:32 PM  <> |+InquireForm.cs                                        15,411 8/6/2021 7:17:38 AM
|+InquireForm.Designer.cs                               40,489 12/21/2021 1:51:32 PM  <> |+InquireForm.Designer.cs                               39,863 8/5/2021 3:01:24 PM
|\InquireForm.resx                                      15,623 12/21/2021 1:51:32 PM  <> |\InquireForm.resx                                      11,022 8/5/2021 3:01:24 PM
+_Revision History for VTIWindowsControlLibrary.txt     65,407 12/21/2021 1:59:34 PM  <> +_Revision History for VTIWindowsControlLibrary.txt     65,263 11/29/2021 12:20:34 PM
\VTIWindowsControlLibrary.csproj                        58,641 12/21/2021 2:00:00 PM  <> \VTIWindowsControlLibrary.csproj                        58,382 11/18/2021 9:21:58 AM
------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 11/29/21 - 1.4.1.8 - NJ

-Added serial badge reader device component (RFIDeas badge reader - serial version)

```
Name                            Size      Modified                  Name                              Size      Modified
-------------------------------------------------------------------------------------------------------------------------------------
Classes                         2,453,954 11/16/2021 11:53:26 AM    Classes                           2,465,298 2/5/2021 12:08:41 PM
\IO                             1,638,867 11/16/2021 11:53:26 AM    \IO                               1,649,999 9/21/2021 7:14:44 AM
 \SerialIO                      1,442,837 11/16/2021 11:53:26 AM     \SerialIO                        1,454,082 11/18/2021 9:20:53 AM
                                                                 <<   +RFIDeasBadgeReader.cs              5,624 11/17/2021 2:53:52 PM
                                                                 <<   +RFIDeasBadgeReader.Designer.cs     1,107 11/17/2021 2:54:06 PM
                                                                 <<   \RFIDeasBadgeReader.resx            6,194 11/17/2021 2:50:56 PM
VTIWindowsControlLibrary.csproj    57,948 6/24/2021 5:54:28 AM   <> VTIWindowsControlLibrary.csproj      58,382 11/18/2021 9:21:56 AM
-------------------------------------------------------------------------------------------------------------------------------------
```

### 11/16/21 - 1.4.1.7 - NJ

-Data plot bug fix. When in linear mode with YMin at 1E-6 clicking the +- 1 down arrow would make the lower bound 0 instead of 9E-7.
-When running the software with a new database (no rows in dbo.UutRecords), automatically give GROUP09 users all permissions.
-When Export button is clicked in Edit Cycle form, export parameters of all models with appropriate display names, values, and descriptions.

```
Name                     Size       Modified                  Name                     Size       Modified
------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary 36,025,275                           vtiwindowscontrollibrary 36,019,471
+Components               1,035,877 11/16/2021 11:53:28 AM    +Components               1,035,751 10/27/2021 11:20:34 AM
|\Graphing                  440,400 11/16/2021 11:53:28 AM    |\Graphing                  440,274 10/27/2021 11:20:34 AM
| \DataPlotControl.cs       116,215 11/16/2021 11:23:56 AM <> | \DataPlotControl.cs       116,089 5/28/2021 1:49:30 PM
+Forms                    1,408,487 11/16/2021 11:53:30 AM    +Forms                    1,406,340 10/27/2021 11:20:36 AM
|\EditCycleForm.cs           78,981 11/16/2021 11:50:52 AM <> |\EditCycleForm.cs           76,834 5/28/2021 2:07:26 PM
\VtiLib.cs                   18,852 11/10/2021 7:12:36 AM  <> \VtiLib.cs                   16,578 6/22/2021 5:16:54 AM
------------------------------------------------------------------------------------------------------------------------
```

### 10/27/21 - 1.4.1.6 - NJ

-Default value of “VerifyLocalVtiDataConfiguration” in Settings.settings is changed to false now due to the new SQL Setup script that Jeremy will execute on all future system PCs. The VtiData database should already be attached when the PC gets into the hands of the software developer.
-Eventually, the “VerifyLocalVtiDataConfiguration” feature will be removed.

-Added new file: Required Client Code\1.4.1.2 CycleStep ParametersToDisplay.txt which describes how to change the CycleStep’s ParametersToDisplay after update 1.4.1.2.

```
Name                                        Size       Modified                  Name                     Size       Modified
------------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary                    36,017,543                           vtiwindowscontrollibrary 36,017,245
+Properties                                    103,538 9/1/2021 2:26:18 PM       +Properties                 103,537 9/1/2021 2:26:18 PM
|\Settings.settings                              9,523 10/27/2021 11:11:34 AM <> |\Settings.settings           9,522 2/24/2021 12:30:10 PM
+Required Client Code                            6,591 9/1/2021 2:26:18 PM       +Required Client Code         6,295 9/1/2021 2:26:18 PM
|\1.4.1.2 CycleStep ParametersToDisplay.txt        296 10/27/2021 10:54:16 AM >> |
\app.config                                      8,170 10/27/2021 11:19:50 AM <> \app.config                   8,169 4/26/2021 4:05:04 PM
------------------------------------------------------------------------------------------------------------------------------------------
```

### 09/01/21 - 1.4.1.5 - NJ

- When manually saving the current .config file from the MainForm File -> Save Config File, save it as a .config file, not a .configGood file.
- Slightly increase the font of the LockableCheckbox so the inputs and outputs are easier to read in the DigitalIO Form.
- Allow displayed text on command button in MiniCommandControl to switch languages based on current language.
- After a new VtiData database is automatically attached, grant all command permissions to Group 09.
- Bug which would cut off I/O descriptions in the DigitalIO Form is fixed.
- Digital I/O form height and width is programmatically set based on how many inputs and outputs there are and how long the names are.
- Permissions form height and width is programmatically set based on size of the commands table DataGridView.
- Fixed typo in file "1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt"
- Include a copy of this revision history file in the vtiwindowscontrollibrary folder.

```
Name                                                      Size       Modified                 Name                                                      Size       Modified
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
Classes                                                    2,453,906 8/6/2021 10:42:22 AM     Classes                                                    2,280,476 9/1/2021 9:54:28 AM
\Util                                                        177,538 8/6/2021 10:42:22 AM     \Util                                                        177,505 9/1/2021 10:18:36 AM
 \SaveConfigGoodFile.cs                                        2,118 2/23/2021 12:07:54 PM <>  \SaveConfigGoodFile.cs                                        2,085 9/1/2021 10:18:36 AM
Components                                                 1,035,229 8/6/2021 10:42:24 AM     Components                                                 1,035,751 9/1/2021 11:41:15 AM
+LockableCheckbox.cs                                           5,968 2/5/2021 1:08:42 PM   <> +LockableCheckbox.cs                                           6,046 9/1/2021 11:41:15 AM
\MiniCommandControl.cs                                        17,626 2/5/2021 1:08:42 PM   <> \MiniCommandControl.cs                                        18,070 8/11/2021 12:09:58 PM
Data                                                      15,153,695 8/6/2021 10:42:24 AM     Data                                                      15,153,952 9/1/2021 11:06:09 AM
\VtiDataContext.cs                                            32,080 6/22/2021 5:15:24 AM  <> \VtiDataContext.cs                                            32,337 9/1/2021 11:06:09 AM
Forms                                                      1,403,400 8/6/2021 10:42:26 AM     Forms                                                      1,433,950 9/1/2021 12:45:20 PM
+ConfigBackupSelectForm.cs                                     7,017 2/24/2021 2:25:34 PM  <> +ConfigBackupSelectForm.cs                                     7,187 9/1/2021 10:54:22 AM
+DigitalIOForm.cs                                             11,952 4/26/2021 5:55:50 PM  <> +DigitalIOForm.cs                                             13,622 9/1/2021 12:13:55 PM
\PermissionsForm.cs                                           17,218 6/8/2021 9:19:40 AM   <> \PermissionsForm.cs                                           18,230 9/1/2021 12:45:20 PM
Required Client Code                                           6,296 8/6/2021 10:42:28 AM     Required Client Code                                           6,120 9/1/2021 9:54:36 AM
\1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt      1,299 4/26/2021 5:13:08 PM  <> \1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt      1,298 9/1/2021 9:56:43 AM
                                                                                           << _Revision History for VTIWindowsControlLibrary.txt            56,134 8/6/2021 10:37:11 AM
----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 08/06/21 - 1.4.1.4 - NJ

-Can now add a ToolTip to a Manual Command button. To make this work in multiple languages (like the manual command text on the button does now), you must add a new Localization item: "ManualCommandToolTip" + command name. Example:

Localization.resx:
ManualCommandToolTipAUTOTEST	AUTOTEST DECSCRIPTION HERE

Localization.es.resx:
ManualCommandToolTipAUTOTEST	AUTO PRUEBA DESCRIPCIÓN AQUÍ

-Inquire Form:
	-Can now press Enter key to search.
	-Can now search by System ID (when data source is a remote database containing records from multiple machines).
	-Removed Serial Number Inquiry tab, as the Custom Data inquiry tab does the same thing with more search options.
	-If there is a valid connection to a remote VtiData database, use the remote database for searching by default instead of the local VtiData database.

-Changed BackupEditCycleConfig() to force saving backups to C:\VTI PC\Config instead of ProgramData in case client-side code was not updated with previous library changed, which would result in no backups being created.
	-Client-side change needed:
		In your Config.cs -> protected void BackupConfigFile():
		comment-out/remove everything and add:
		_control.BackupEditCycleConfig();

-Edited file vtiwindowscontrollibrary\Required Client Code\1.4.1.1 Add RestartSerialInDevices().txt
	-Changes the way the Barcode Scanner is implemeted to fix a bug.

-Minor bug fix in P3000 interface file.

```
Name                                      Size      Modified                 Name                                      Size      Modified
------------------------------------------------------------------------------------------------------------------------------------------------------
Classes                                   2,453,906 6/16/2021 12:45:00 PM    Classes                                   2,452,920 6/16/2021 12:45:00 PM
+Configuration                              189,369 6/24/2021 1:00:14 PM     +Configuration                              188,853 6/24/2021 5:30:44 AM
|\EditCycleApplicationSettingsBase.cs         9,076 6/24/2021 1:00:14 PM  <> |\EditCycleApplicationSettingsBase.cs         8,560 6/18/2021 11:15:20 AM
+IO                                       1,638,867 6/24/2021 6:11:10 AM     +IO                                       1,638,535 6/24/2021 6:11:10 AM
|\SerialIO                                1,442,837 7/29/2021 9:22:20 AM     |\SerialIO                                1,442,505 6/24/2021 5:33:56 AM
| \InficonP3000ASCIIMDB.cs                   75,485 7/29/2021 9:22:20 AM  <> | \InficonP3000ASCIIMDB.cs                   75,153 2/5/2021 1:08:42 PM
\ManualCommands                              29,208 8/6/2021 6:51:48 AM      \ManualCommands                              29,070 6/16/2021 12:45:00 PM
 \ManualCommandsBase.cs                      11,244 8/6/2021 6:51:38 AM   <>  \ManualCommandsBase.cs                      11,106 6/8/2021 9:15:34 AM
Forms                                     1,403,400 8/6/2021 7:17:38 AM      Forms                                     1,431,219 6/22/2021 7:08:50 AM
+GenericTouchScreenButtonForm.cs             10,938 8/6/2021 6:50:06 AM   <> +GenericTouchScreenButtonForm.cs              8,418 2/5/2021 1:08:44 PM
+InquireForm.cs                              15,411 8/6/2021 7:17:38 AM   <> +InquireForm.cs                              17,715 2/5/2021 1:08:44 PM
+InquireForm.Designer.cs                     39,863 8/5/2021 3:01:24 PM   <> +InquireForm.Designer.cs                     63,643 2/5/2021 1:08:44 PM
\InquireForm.resx                            11,022 8/5/2021 3:01:24 PM   <> \InquireForm.resx                            15,277 2/5/2021 1:08:44 PM
Properties                                  103,537 7/26/2021 12:59:46 PM    Properties                                  103,537 6/24/2021 6:13:16 AM
\AssemblyInfo.cs                              1,473 7/26/2021 12:59:46 PM <> \AssemblyInfo.cs                              1,473 6/24/2021 6:13:16 AM
Required Client Code                          6,296 8/6/2021 7:29:16 AM      Required Client Code                          6,121 6/16/2021 12:45:04 PM
+1.4.1.1 Add RestartSerialInDevices().txt     2,237 8/5/2021 12:12:56 PM  <> \1.4.1.1 Add RestartSerialInDevices().txt     2,200 5/21/2021 8:59:02 AM
\1.4.1.4 BackupConfigFile().txt                 138 8/6/2021 7:30:08 AM   >>
------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 06/24/21 - 1.4.1.3 - NJ

-MUST ALSO UPDATE PLC LIBRARY WITH THIS VERSION

- Added TAS edits:
	+ EthernetPortParameter
		Need to create code snippet for this parameter type
	+ IO\EthernetIO\EthernetPortSettings.cs
	\InficonCubes.cs – file is not added to the project (Requires RestClient to use)
	+ IO\EthernetIO\KeithleyDMM6500.cs 
	+ IO\SerialIO\Keithley2000scan.cs – supports Keithley 2000 scan card
	+ TorrConIVSerialPortParameter
		Adds TorrCon Name: i.e. LeakHold for future find and configure features
	+ Updates to IO\SErialIO\MKS670.cs
	+ Updates to IO\SerialIO\MKSSRG.cs – Works with SRG3
	+ Updates to IO\SerialIO\TerraNova934a.cs – error trapping 
	+ Enums\VtiEvenCattypes.cs – added Ethernet as an event message category
	+ Forms\SRGSetupForm.cs – refresh bug fix

- Added ConfigFileExists() method to easily determine if the main config file (excluding any backups) exists. Usage:
	var _allUsersSettingsProvider = new VTIWindowsControlLibrary.Classes.Configuration.AllUsersSettingsProvider();
            if (_allUsersSettingsProvider.ConfigFileExists()) { do stuff }

- When creating a backup of the config file, the library will try to load it in as an XML document first. If this fails, the config file is likely corrupted and will not be backed up.

- Added a property which sets a flag to force the PLC process thread to read the values of the DigitalOutputs from the PLC into the VTI app. 
- This property is then automatically set to false when the read is complete. This is useful for projects where the program logic is in the PLC and the PC only acts as an HMI.
- Usage: Config.IO.Interface.ReadPLCDigOutputsIntoPC = true;

-Can now press on the escape button when the KeyPadForm is visible to close it.

```
Name                                  Size       Modified                Name                                  Size       Modified
-----------------------------------------------------------------------------------------------------------------------------------------------
Classes                                2,280,509 6/8/2021 2:20:14 PM     Classes                                2,452,920 6/16/2021 12:44:59 PM
+Configuration                           154,942 6/8/2021 2:20:14 PM     +Configuration                           188,853 6/24/2021 5:30:43 AM
|+AllUsersSettingsProvider.cs             25,019 2/24/2021 3:47:52 PM << |+AllUsersSettingsProvider.cs             25,373 6/24/2021 5:30:13 AM
|\EditCycleApplicationSettingsBase.cs      7,935 6/8/2021 9:12:20 AM  << |+EditCycleApplicationSettingsBase.cs      8,560 6/18/2021 11:15:18 AM
|                                                                     << |+EthernetPortParameter.cs                10,367 10/7/2020 3:11:14 PM
|                                                                     << |\TorrConIVSerialParameter.cs             22,565 6/9/2021 4:09:30 PM
\IO                                    1,500,035 6/8/2021 2:20:14 PM     \IO                                    1,638,535 6/24/2021 6:11:09 AM
 |                                                                        +EthernetIO                              45,945 6/24/2021 5:31:10 AM
 |                                                                    <<  |+EthernetPortSettings.cs                 5,821 10/7/2020 3:13:08 PM
 |                                                                    <<  |+InficonCube.cs                         19,555 6/9/2021 12:17:10 PM
 |                                                                    <<  |\KeithleyDMM6500.cs                     20,569 10/7/2020 3:13:16 PM
 +Interfaces                              13,520 6/8/2021 2:20:14 PM      +Interfaces                              13,798 6/23/2021 10:58:21 AM
 |\IIoConfig.cs                            2,316 2/5/2021 1:08:42 PM  <<  |\IIoConfig.cs                            2,594 6/23/2021 10:58:21 AM
 +SerialIO                             1,397,061 6/8/2021 2:20:14 PM      +SerialIO                             1,442,505 6/24/2021 5:33:54 AM
 ||                                                                   <<  |+Keithley2000scan.cs                    13,886 10/7/2020 3:23:54 PM
 |+MKS670.cs                              17,810 2/5/2021 1:08:42 PM  >>  |+MKS670.cs                              37,812 10/7/2020 3:24:00 PM
 |+MKSSRG.cs                              39,037 2/5/2021 1:08:42 PM  >>  |+MKSSRG.cs                              39,559 10/7/2020 3:24:06 PM
 |\TerraNova934a.cs                       33,341 2/5/2021 1:08:42 PM  >>  |+TerraNova934a.cs                       35,006 10/7/2020 3:29:06 PM
 |                                                                    <<  |\TorrConnIVPortSettings.cs               9,369 6/9/2021 4:09:12 PM
 |                                                                    <<  +EthernetPortSettings.cs                  5,821 6/9/2021 11:58:48 AM
 |                                                                    <<  +InficonCube.cs                          19,555 6/9/2021 11:58:48 AM
 \IOInterface.cs                           8,964 2/5/2021 1:08:42 PM  <<  +IOInterface.cs                           9,852 6/24/2021 6:11:09 AM
                                                                      <<  \KeithleyDMM6500.cs                      20,569 6/9/2021 11:58:48 AM
Data                                  15,153,247 6/8/2021 3:33:56 PM     Data                                  15,153,143 6/22/2021 5:15:23 AM
\VtiDataContext.cs                        32,184 6/8/2021 3:33:56 PM  << \VtiDataContext.cs                        32,080 6/22/2021 5:15:23 AM
Enums                                      7,245 6/8/2021 2:20:16 PM     Enums                                      7,397 6/24/2021 5:35:42 AM
\VtiEventCatType.cs                        2,531 2/5/2021 1:08:44 PM  << \VtiEventCatType.cs                        2,683 6/24/2021 5:35:42 AM
Forms                                  1,431,098 6/8/2021 2:20:16 PM     Forms                                  1,431,219 6/22/2021 7:08:49 AM
+KeyPadForm.cs                             5,607 2/5/2021 1:08:44 PM  << +KeyPadForm.cs                             5,940 6/22/2021 7:08:49 AM
+KeyPadForm.Designer.cs                    4,787 2/5/2021 1:08:44 PM  << +KeyPadForm.Designer.cs                    4,427 6/22/2021 7:08:01 AM
\SRGSetupForm.cs                          11,504 2/5/2021 1:08:44 PM  << \SRGSetupForm.cs                          11,652 6/9/2021 10:56:48 AM
Properties                               103,537 6/8/2021 2:20:16 PM     Properties                               103,537 6/24/2021 6:13:15 AM
\AssemblyInfo.cs                           1,473 6/8/2021 9:21:18 AM  << \AssemblyInfo.cs                           1,473 6/24/2021 6:13:15 AM
VtiLib.cs                                 16,645 6/8/2021 9:10:50 AM  << VtiLib.cs                                 16,578 6/22/2021 5:16:53 AM
VTIWindowsControlLibrary.csproj           57,466 6/8/2021 9:11:14 AM  << VTIWindowsControlLibrary.csproj           57,948 6/24/2021 5:54:26 AM
-----------------------------------------------------------------------------------------------------------------------------------------------
```

### 6/8/21 - 1.4.1.2 - NJ

- The SplashScreen will display  the current year for the copyright.
- Available COM port numbers are ordered correctly in the Edit Cycle serial port parameter dropdown.
- CycleStep parameters to display are now in a List instead of an Array. This allows the client-side code to be simpler. Usage example: 
		step.ParametersToDisplay.Add(Config.Pressure.BasePressureCheckLimit);
- InficonXL3000flex component added to SerialIO.
- SerialIO process thread uses Thread.Sleep() instead of exitEvent.WaitOne() to drastically improve CPU usage of the app.
- Changed data plot logic per GMS. If the plot is in linear mode, the Ymin or Ymax values are in scientific notation, and the +/- 1 up arrow is clicked, the first digit of the value is incremented/decremented.
	-down-click from 1.3E-5 -> set it to 9.3E-6
	-down-click from 9.3E-6 -> set it to 8.3E-6
	-up-click from 9.3E-6 -> set it to 1.3E-5
	-up-click from 8.3E-6 -> set it to 9.3E-6
- You can change the System Signals panel’s refresh frequency from the client-side code.
	-Machine -> InitializeOperatorForm() -> 
				_OpFormDual.SystemSignals[0].refreshTimerInterval = 100;
                _OpFormDual.SystemSignals[1].refreshTimerInterval = 100;
-When export button is clicked in the Edit Cycle form, do not export the invisible parameters.
-On startup, the config file is checked to make sure every parameter has the necessary tags based on the parameter type. Ex. ProcessValue, DisplayName, Tooltip, etc.

```
Name                                       Size       Modified                 Name                                       Size       Modified
----------------------------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary                   35,480,187                          vtiwindowscontrollibrary                   35,104,363
+Classes                                    2,280,509 6/8/2021 2:20:14 PM      +Classes                                    2,199,426 4/27/2021 9:01:12 AM
|+ClientForms                                  50,726 6/8/2021 2:20:14 PM      |+ClientForms                                  50,732 4/27/2021 9:01:08 AM
||\SplashScreen.cs                              2,016 6/8/2021 8:48:40 AM   <> ||\SplashScreen.cs                              1,934 2/5/2021 1:08:42 PM
|+Configuration                               154,942 6/8/2021 2:20:14 PM      |+Configuration                               154,719 4/27/2021 9:01:10 AM
||+EditCycleApplicationSettingsBase.cs          7,935 6/8/2021 9:12:20 AM   <> ||+EditCycleApplicationSettingsBase.cs          7,835 2/24/2021 1:11:40 PM
||\SerialPortParameter.cs                      21,062 6/8/2021 9:13:12 AM   <> ||\SerialPortParameter.cs                      20,815 4/26/2021 6:16:58 PM
|+CycleSteps                                  103,597 6/8/2021 2:20:14 PM      |+CycleSteps                                  103,113 4/27/2021 9:01:10 AM
||\CycleStep.cs                                50,035 6/8/2021 9:13:52 AM   <> ||\CycleStep.cs                                49,551 2/5/2021 1:08:42 PM
|+IO                                        1,500,035 6/8/2021 2:20:14 PM      |+IO                                        1,423,353 4/27/2021 9:01:12 AM
||\SerialIO                                 1,397,061 6/8/2021 2:20:14 PM      ||\SerialIO                                 1,320,379 4/27/2021 11:22:44 AM
|| +InficonXL3000flex.cs                       75,404 5/19/2021 10:56:38 AM >> || |
|| +InficonXL3000flex.Designer.cs               1,102 5/17/2021 4:37:42 PM  >> || |
|| \SerialIOBase.cs                            15,898 6/8/2021 9:14:16 AM   <> || \SerialIOBase.cs                            15,722 4/26/2021 6:22:52 PM
|\ManualCommands                               29,070 6/8/2021 2:20:14 PM      |\ManualCommands                               25,370 4/27/2021 9:01:12 AM
| \ManualCommandAttribute.cs                    4,958 6/8/2021 9:15:00 AM   <> | \ManualCommandAttribute.cs                    2,843 2/5/2021 1:08:42 PM
+Components                                 1,035,229 6/8/2021 2:43:06 PM      +Components                                 1,030,484 4/27/2021 9:01:14 AM
|+Graphing                                    440,274 6/8/2021 2:20:16 PM      |+Graphing                                    436,103 4/27/2021 9:01:14 AM
||+DataPlotControl.cs                         116,089 5/28/2021 1:49:30 PM  <> ||+DataPlotControl.cs                         113,044 2/5/2021 1:08:42 PM
||\GraphControl.cs                            148,928 6/1/2021 11:13:50 AM  <> ||\GraphControl.cs                            147,802 2/11/2021 5:56:04 PM
|+SystemSignalsControl.cs                       9,696 6/8/2021 2:43:06 PM   <> |+SystemSignalsControl.cs                       9,152 2/5/2021 1:08:44 PM
|\SystemSignalsControl.Designer.cs              7,104 6/8/2021 9:18:00 AM   <> |\SystemSignalsControl.Designer.cs              7,074 2/5/2021 1:08:44 PM
+Data                                      15,152,750 6/8/2021 3:33:56 PM      +Data                                      15,152,353 4/27/2021 12:17:04 PM
|\VtiDataContext.cs                            32,184 6/8/2021 3:33:56 PM   <> |\VtiDataContext.cs                            31,787 4/27/2021 12:17:04 PM
+Forms                                      1,431,098 6/8/2021 2:20:16 PM      +Forms                                      1,430,310 4/27/2021 9:01:16 AM
|+EditCycleForm.cs                             76,834 5/28/2021 2:07:26 PM  <> |+EditCycleForm.cs                             76,103 4/27/2021 8:15:40 AM
|\PermissionsForm.cs                           17,218 6/8/2021 9:19:40 AM   <> |\PermissionsForm.cs                           17,161 2/25/2021 2:46:54 PM
+Required Client Code                           6,121 6/8/2021 2:20:16 PM      +Required Client Code                           6,079 4/27/2021 12:43:28 PM
|\1.4.1.1 Add RestartSerialInDevices().txt      2,200 5/21/2021 8:59:02 AM  <> |\1.4.1.1 Add RestartSerialInDevices().txt      2,158 4/27/2021 12:38:40 PM
+VtiLib.cs                                     16,645 6/8/2021 9:10:50 AM   <> +VtiLib.cs                                     10,361 4/27/2021 8:30:44 AM
\VTIWindowsControlLibrary.csproj               57,466 6/8/2021 9:11:14 AM   <> \VTIWindowsControlLibrary.csproj               57,153 5/11/2021 2:38:40 PM
----------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 04/27/21 - 1.4.1.1 - NJ

You can now change COM ports in the Edit Cycle form without needing to restart the software.
Some client-side code changes are needed (attached). With these changes, the barcode scanner is now a Serial Input defined in SerialInputs.cs instead of a standalone serial port defined in Machine.cs.

When you click on a Serial Port Parameter in the Edit Cycle window, the software will refresh the available COM ports and the full name of the COM port will appear as is does in Device Manager. Previously, the COM ports would not refresh correctly.
There is now a BarcodeScanner.cs component in the SerialIO folder.
I’ve added one more decimal place to the analog signal raw value format so it is easier to see small changes in the voltage (“0.00” to “0.000”).

The ParameterChangeLog now logs Serial Port Parameter changes correctly.
The ParamChangeLogForm and ManualCmdExecLogForm now resize based on the size of the size of the DataGridView after the search is performed.

I’ve created a folder on the server which contains instructions for client-side code changes required for various control library versions: 
`\\SVR-STORAGE\Central\I-Teams\Software I-Team\CSharp Development\_Procedures and Notes\Required Client Code for Lib Changes`

These files are now also included in the library in case you are upgrading the library in the field and do not have access to the server:
…\Common\vtiwindowscontrollibrary\Required Client Code

```
Name                                                      Size       Modified                 Name                              Size       Modified
----------------------------------------------------------------------------------------------------------------------------------------------------------------
Classes                                                    2,199,426 4/27/2021 9:01:10 AM     Classes                            2,180,176 2/26/2021 2:07:38 PM
+Configuration                                               154,719 4/27/2021 9:01:08 AM     +Configuration                       152,024 2/26/2021 2:07:34 PM
|\SerialPortParameter.cs                                      20,815 4/26/2021 6:16:58 PM  <> |\SerialPortParameter.cs              18,120 2/5/2021 12:08:42 PM
\IO                                                        1,423,353 4/27/2021 9:01:10 AM     \IO                                1,406,798 2/26/2021 2:07:38 PM
 \SerialIO                                                 1,320,379 4/27/2021 11:22:43 AM     \SerialIO                         1,303,937 2/26/2021 2:07:38 PM
  +BarcodeScanner.cs                                           5,560 4/27/2021 11:22:43 AM >>   |
  +BarcodeScanner.Designer.cs                                  1,103 4/27/2021 10:40:46 AM >>   |
  +BarcodeScanner.resx                                         6,194 4/27/2021 10:39:04 AM >>   |
  \SerialIOBase.cs                                            15,722 4/26/2021 6:22:52 PM  <>   \SerialIOBase.cs                    13,817 2/5/2021 12:08:42 PM
Components                                                 1,030,277 4/27/2021 9:01:13 AM     Components                         1,030,275 2/26/2021 2:07:40 PM
\SystemSignalControl.cs                                        7,697 4/22/2021 1:46:28 PM  <> \SystemSignalControl.cs                7,695 2/5/2021 12:08:44 PM
Data                                                      15,152,353 4/27/2021 12:17:03 PM    Data                              15,149,743 2/26/2021 2:07:40 PM
+TEST_RESULTS script.sql                                         918 4/14/2021 2:48:06 PM  >> |
\VtiDataContext.cs                                            31,787 4/27/2021 12:17:03 PM <> \VtiDataContext.cs                    30,017 2/24/2021 11:30:38 AM
Forms                                                      1,385,381 4/27/2021 9:01:15 AM     Forms                              1,383,688 2/26/2021 2:07:42 PM
+EditCycleForm.cs                                             76,103 4/27/2021 8:15:40 AM  <> +EditCycleForm.cs                     75,586 3/1/2021 9:18:58 AM
+ManualCmdExecLogForm.cs                                       6,992 4/26/2021 4:40:34 PM  <> +ManualCmdExecLogForm.cs               6,403 2/5/2021 12:08:44 PM
+ManualCmdExecLogForm.Designer.cs                             19,836 4/26/2021 4:24:22 PM  <> +ManualCmdExecLogForm.Designer.cs     19,836 2/5/2021 12:08:44 PM
+ParamChangeLogForm.cs                                         7,038 4/26/2021 4:40:36 PM  <> +ParamChangeLogForm.cs                 6,451 2/5/2021 12:08:44 PM
\ParamChangeLogForm.Designer.cs                               21,669 4/26/2021 4:21:22 PM  <> \ParamChangeLogForm.Designer.cs       21,669 2/5/2021 12:08:44 PM
Interfaces                                                     5,034 4/27/2021 9:01:15 AM     Interfaces                             5,010 2/26/2021 2:07:42 PM
\IMachine.cs                                                     821 4/23/2021 10:59:00 AM <> \IMachine.cs                             797 2/5/2021 12:08:44 PM
Properties                                                   103,537 4/27/2021 9:01:18 AM     Properties                           103,536 2/26/2021 2:08:48 PM
+AssemblyInfo.cs                                               1,473 4/26/2021 6:26:26 PM  <> +AssemblyInfo.cs                       1,473 2/26/2021 2:08:48 PM
+Resources.Designer.cs                                        37,023 4/26/2021 4:05:06 PM  <> +Resources.Designer.cs                37,022 2/5/2021 12:08:44 PM
\Settings.Designer.cs                                         25,512 4/26/2021 4:05:06 PM  <> \Settings.Designer.cs                 25,512 2/24/2021 11:30:10 AM
Required Client Code                                           6,079 4/27/2021 12:43:27 PM
+1.4.0.6 Add ParamChangeLog Viewer Form to MainForm.txt        1,300 4/26/2021 5:12:52 PM  >>
+1.4.0.8 Add ManualCmdExecLog Viewer Form to MainForm.txt      1,299 4/26/2021 5:13:08 PM  >>
+1.4.1.0 Add 'Save Config File' to MainForm.txt                1,322 2/24/2021 2:34:00 PM  >>
\1.4.1.1 Add RestartSerialInDevices().txt                      2,158 4/27/2021 12:38:38 PM >>
app.config                                                     8,169 4/26/2021 4:05:04 PM  <> app.config                             8,167 2/24/2021 11:30:10 AM
VtiLib.cs                                                     10,361 4/27/2021 8:30:44 AM  <> VtiLib.cs                              9,209 2/24/2021 10:42:30 AM
VtiStandardMessages.Designer.cs                                6,232 4/26/2021 4:05:06 PM  <> VtiStandardMessages.Designer.cs        6,232 2/5/2021 12:08:44 PM
VTIWindowsControlLibrary.csproj                               56,711 4/27/2021 11:21:15 AM <> VTIWindowsControlLibrary.csproj       56,150 2/22/2021 5:29:12 PM
----------------------------------------------------------------------------------------------------------------------------------------------------------------
```

### 02/26/21 - 1.4.1.0 - NJ

You no longer have to drag a copy of VtiData.mdf or VtiData_log.ldf to the C:\VTI PC\Databases folder. The software will check if it exists there and if not, copy it to that location from the library’s Data folder. The software will also attach the database to the local SQL Server if it is not already attached and then add the dbo.ParamChangeLog and dbo.ManualCmdExecLog tables if they do not exist. If the software attaches the VtiData database automatically, you will need to shut down the software and change the VtiData connection string to the new connection. All of this only happens if the control library setting “VerifyLocalVtiDataConfiguration” is set to true (true by default).

Added the edit cycle parameters .config file restore feature, which allows the user to select an automatically-created backup config file to select from or a manually-saved .configGood file.

The user can save a config file by selecting File -> Save Config File… in the Main Form. Instructions for adding this to your MainForm.cs are attached and also located here: \\Svr-storage\central\I-Teams\Software I-Team\CSharp Development\_Procedures and Notes\Add 'Save Config File' to MainForm 1.4.1.0.txt

Fixed a bug where multiple copies of each model’s parameter was created in the dbo.ModelParameters table. You can recreate this by creating a model “test” then press OK on the Edit Cycle form. Then reopen the form, delete “test”, create “test” again, and press OK. There will now be duplicate model parameter entries in your dbo.ModelParameters table. Now, even if you change any parameters for the “test” model, the software will still use the old (deleted) “test” parameters.

Fixed three bugs in the Operators tab of the Permissions form:
1.	 You can recreate this by adding a new user, giving it a password, clicking Accept, then double-clicking the new user, changing the password, clicking Accept, and then clicking OK on the Permissions form. If the app is not running from the source, nothing will happen when OK is clicked. This is now fixed.
2.	Deleting a user and then adding a new user with the same OpID all before clicking the OK button will produce the same result; if the app is not running from the source, nothing will happen when OK is clicked. This is not fixed.
3.	Could not edit the Group number for an existing user. Now you can.

```
Name                                  Size       Modified                 Name                                  Size       Modified
------------------------------------------------------------------------------------------------------------------------------------------------
Classes                                2,171,680 9/29/2020 10:03:28 AM    Classes                                2,180,176 2/26/2021 2:07:36 PM
+ClientForms                              48,444 1/26/2021 10:38:58 AM    +ClientForms                              50,732 2/26/2021 2:07:33 PM
|                                                                      << |\ConfigBackupSelect.cs                    2,288 2/22/2021 11:29:28 AM
+Configuration                           150,941 10/20/2020 8:30:36 AM    +Configuration                           152,024 2/26/2021 2:07:33 PM
|+EditCycleApplicationSettingsBase.cs      7,126 9/29/2020 9:58:36 AM  <> |+EditCycleApplicationSettingsBase.cs      7,835 2/24/2021 12:11:40 PM
|\ObjectConfiguration.cs                  16,303 9/30/2020 2:11:48 PM  <> |\ObjectConfiguration.cs                  16,677 2/22/2021 5:11:30 PM
+Util                                    175,420 9/29/2020 10:03:26 AM    +Util                                    177,538 2/26/2021 2:07:37 PM
+VtiEvent.cs                               2,779 9/29/2020 10:03:28 AM <> +VtiEvent.cs                               5,764 2/24/2021 12:19:00 PM
\SystemDiagnostics.cs                     62,060 9/29/2020 10:02:52 AM <> \SystemDiagnostics.cs                     62,082 2/22/2021 12:43:24 PM
Components                             1,030,059 1/22/2021 11:05:44 AM    Components                             1,030,275 2/26/2021 2:07:39 PM
\Graphing                                435,887 12/15/2020 3:56:22 PM    \Graphing                                436,103 2/26/2021 2:07:39 PM
Data                                  15,141,258 1/28/2021 8:18:26 AM     Data                                  15,149,743 2/26/2021 2:07:39 PM
\VtiDataContext.cs                        21,610 1/26/2021 10:18:30 AM <> \VtiDataContext.cs                        30,017 2/24/2021 11:30:38 AM
Forms                                  1,359,961 1/26/2021 10:31:38 AM    Forms                                  1,383,271 2/26/2021 2:07:41 PM
|                                                                      << +ConfigBackupSelectForm.resx               5,817 2/22/2021 4:58:46 PM
|                                                                      << +ConfigBackupSelectForm.Designer.cs        6,922 2/22/2021 4:58:46 PM
|                                                                      << +ConfigBackupSelectForm.cs                 7,017 2/24/2021 1:25:34 PM
+PermissionsForm.cs                       14,207 9/29/2020 10:05:26 AM <> +PermissionsForm.cs                       17,161 2/25/2021 1:46:54 PM
\EditCycleForm.cs                         74,569 1/25/2021 2:18:42 PM  <> \EditCycleForm.cs                         75,169 2/24/2021 5:16:04 PM
Properties                               102,968 1/26/2021 3:13:00 PM     Properties                               103,536 2/26/2021 2:08:47 PM
+AssemblyInfo.cs                           1,473 2/4/2021 4:09:02 PM   <> +AssemblyInfo.cs                           1,473 2/26/2021 2:08:47 PM
+Settings.settings                         9,362 1/11/2021 1:55:46 PM  <> +Settings.settings                         9,522 2/24/2021 11:30:10 AM
\Settings.Designer.cs                     25,104 1/11/2021 1:55:46 PM  <> \Settings.Designer.cs                     25,512 2/24/2021 11:30:10 AM
app.config                                 8,042 1/11/2021 1:55:46 PM  <> app.config                                 8,167 2/24/2021 11:30:10 AM
VtiLib.cs                                  9,207 12/1/2020 9:04:44 AM  <> VtiLib.cs                                  9,209 2/24/2021 10:42:30 AM
VTIWindowsControlLibrary.csproj           55,612 1/26/2021 10:39:56 AM <> VTIWindowsControlLibrary.csproj           56,150 2/22/2021 5:29:12 PM
------------------------------------------------------------------------------------------------------------------------------------------------
```


### 02/05/21 - 1.4.0.9 - NJ

Made some changes to the TorrconIV.cs file to avoid error in older versions of Visual Studio. Added UutRecords and UutRecordDetails table scripts in the Data folder.

```
Name                     Size       Modified                  Name                         Size       Modified
---------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary 39,137,776                           vtiwindowscontrollibrary     39,132,515
+Properties                 102,968 1/26/2021 3:13:50 PM      +Properties                     102,968 1/26/2021 3:13:00 PM
|\AssemblyInfo.cs             1,473 1/26/2021 3:13:50 PM   <> |\AssemblyInfo.cs                 1,473 2/4/2021 4:09:02 PM
+Classes                  2,171,535 1/26/2021 11:02:48 AM     +Classes                      2,171,680 9/29/2020 10:03:28 AM
|\IO                      1,406,653 1/26/2021 11:02:48 AM     |\IO                          1,406,798 9/29/2020 10:02:30 AM
| \SerialIO               1,303,792 1/26/2021 11:02:48 AM     | \SerialIO                   1,303,937 2/4/2021 3:56:02 PM
|  \TorrConIV.cs             25,759 11/24/2020 12:14:08 PM <> |  \TorrConIV.cs                 25,904 2/4/2021 3:52:10 PM
\Data                    15,138,988 1/26/2021 11:02:50 AM     \Data                        15,141,258 1/28/2021 8:18:26 AM
                                                           <<  +UutRecordsScript.sql              909 1/28/2021 8:18:02 AM
                                                           <<  \UutRecordDetailsScript.sql      1,361 1/28/2021 8:18:26 AM
---------------------------------------------------------------------------------------------------------------------------
```

### 01/26/21 - 1.4.0.8 - NJ

I've added the dbo.ManualCmdExecLog table in VtiData.mdf and a corresponding data viewer form.
Instructions to add the dbo.ManualCmdExecLog table viewer to your main form here:
\\Svr-storage\central\I-Teams\Software I-Team\CSharp Development\_Procedures and Notes\Add ManualCmdExecLog Viewer Form to MainForm 1.4.0.8.txt
The library now adds a row to the table (local and remote if applicable) every time a Manual Command which relies on a permission level is executed.
The new table is included in the VtiData.mdf.SEED file and the script to add the new table is also included in the Data folder.
Rows in the new table include DateTime, OpID, OverrideOpID, and ManualCommand.
Also included a script in the Data folder which creates the newest version of VtiData.mdf with all necessary tables.

```
Name                               Size       Modified                 Name                             Size       Modified
-----------------------------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary           39,137,420                          vtiwindowscontrollibrary         38,969,903
+Forms                              1,359,961 1/26/2021 11:02:52 AM    +Forms                            1,326,703 1/18/2021 10:47:52 AM
|+ManualCmdExecLogForm.cs               6,403 1/26/2021 10:31:38 AM >> ||
|+ManualCmdExecLogForm.resx             6,993 1/26/2021 10:11:00 AM >> ||
|+ManualCmdExecLogForm.Designer.cs     19,836 1/26/2021 10:11:00 AM >> ||
|\EditCycleForm.cs                     74,569 1/25/2021 2:18:42 PM  <> |\EditCycleForm.cs                   74,543 1/12/2021 8:16:52 AM
+Classes                            2,171,179 1/26/2021 11:02:48 AM    +Classes                          2,168,937 9/29/2020 10:03:28 AM
|\ClientForms                          48,444 1/26/2021 11:02:46 AM    |\ClientForms                        46,202 1/18/2021 10:47:52 AM
| \ManualCmdExecLog.cs                  2,242 1/26/2021 10:38:58 AM >> |
+Data                              15,138,988 1/26/2021 11:02:50 AM    +Data                            15,078,428 12/8/2020 10:00:28 AM
|+ManualCmdExecLogScript.sql              464 1/25/2021 2:16:40 PM  >> ||
|+VtiDataContext.dbml.layout            8,466 1/26/2021 10:13:40 AM <> |+VtiDataContext.dbml.layout          7,977 1/11/2021 2:16:36 PM
|+VtiDataContext.dbml                  10,928 1/26/2021 10:13:40 AM <> |+VtiDataContext.dbml                10,163 1/11/2021 2:16:36 PM
|+VtiDataContext.cs                    21,610 1/26/2021 10:18:30 AM <> |+VtiDataContext.cs                  15,886 1/11/2021 2:16:36 PM
|+VtiDataScript.sql                    50,726 1/26/2021 9:20:18 AM  >> ||
||                                                                  << |+VtiDataContext.designer.cs         62,518 1/11/2021 2:16:36 PM
|+VtiDataContext1.designer.cs          64,910 1/26/2021 10:25:04 AM >> ||
|+VtiData.mdf.SEED                  4,456,448 1/26/2021 8:13:06 AM  <> |+VtiData.mdf.SEED                4,456,448 11/30/2020 12:32:34 PM
|\VtiData_log.ldf.SEED             10,420,224 1/26/2021 8:13:06 AM  <> |\VtiData_log.ldf.SEED           10,420,224 11/30/2020 12:32:34 PM
\VTIWindowsControlLibrary.csproj       55,612 1/26/2021 10:39:56 AM <> \VTIWindowsControlLibrary.csproj     55,077 1/12/2021 7:51:02 AM
-----------------------------------------------------------------------------------------------------------------------------------------
```

### 01/22/21 - 1.4.0.7 - NJ

Added a requested override in MiniCommandsControl.cs: Commands.Replace(oldCommand, newCommand, newButtonBackColor);

```
Name                     Size       Modified                Name                     Size       Modified
--------------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary 38,969,796                         vtiwindowscontrollibrary 38,699,134
\Components               1,030,057 1/22/2021 8:24:34 AM    \Components               1,028,971 1/18/2021 7:44:58 AM
 \MiniCommandControl.cs      17,624 1/22/2021 8:24:34 AM <>  \MiniCommandControl.cs      16,538 1/18/2021 7:44:58 AM
--------------------------------------------------------------------------------------------------------------------
```

### 01/18/21 - 1.4.0.6 - NJ
	
	I’ve added a form/data viewer for the dbo.ParamChangeLog table. It operates and looks like the current Inquire form. 
	Since there is no library version of the Main Form, you must add it to yours as instructed here:
	\\SVR-STORAGE\Central\I-Teams\Software I-Team\CSharp Development\_Procedures and Notes\Add ParamChangeLog Viewer Form to MainForm 1.4.0.6.txt

I’ve edited the MiniCommandControl component and code. You may now specify the button size, back color, and font when adding a new command 
(ex. _OpFormSingle.Commands.Add(ManualCommands.SelectModel, Color.LightSkyBlue, new Size(200, 50), new Font(FontFamily.Families. GenericSansSerif(), 15.75F, FontStyle.Bold));). 
You can also replace a manual command with another one
(ex. _OpFormSingle.Commands.Replace(ManualCommands.SelectModel, ManualCommands.AutoTest);).
The container width is automatically adjusted based on the widest button.
A scroll bar appears when there are too many buttons in the container and padding is added so that the scroll bar does not overlap the button.

In the System Signals panel, when switching to the “Raw Values” mode, if the system signal label is string.Empty(), the panel will not display a raw value (previously it displayed 0.00 V). This was done so that if you have a fake signal with no label used to space out the other system signals, its raw value would not be displayed to avoid confusion.

In DefectCodes.cs, when searching for the "VtiDefectCodes.xml" file in the project’s directories, if there is more than one file found, use the one with the shorter file path.

	Beyond Compare report for Files Changed:
```
Name                             Modified                 Name                             Modified
----------------------------------------------------------------------------------------------------------------
vtiwindowscontrollibrary                                  vtiwindowscontrollibrary
+Classes                         1/15/2021 8:00:14 AM     +Classes                         1/4/2021 3:35:36 PM
|+ClientForms                    1/15/2021 8:00:12 AM     |+ClientForms                    1/4/2021 3:35:34 PM
|\DefectTracking                 1/15/2021 8:00:14 AM     |\DefectTracking                 1/4/2021 3:35:34 PM
+Components                      1/18/2021 7:44:58 AM     +Components                      1/4/2021 3:35:38 PM
|+MiniCommandControl.cs          1/18/2021 7:44:58 AM  >> |+MiniCommandControl.cs          9/29/2020 10:04:06 AM
|+MiniCommandControl.Designer.cs 1/14/2021 10:32:14 AM >> |+MiniCommandControl.Designer.cs 6/19/2020 12:30:28 PM
|\SystemSignalControl.cs         1/11/2021 10:47:22 AM >> |\SystemSignalControl.cs         9/29/2020 10:04:30 AM
+Data                            1/15/2021 8:00:16 AM     +Data                            1/4/2021 3:35:38 PM
|+VtiDataContext.cs              1/11/2021 2:16:36 PM  >> |+VtiDataContext.cs              12/1/2020 9:02:00 AM
|+VtiDataContext.dbml            1/11/2021 2:16:36 PM  >> |+VtiDataContext.dbml            6/19/2020 12:30:28 PM
|+VtiDataContext.dbml.layout     1/11/2021 2:16:36 PM  >> |+VtiDataContext.dbml.layout     6/19/2020 12:30:28 PM
|\VtiDataContext.designer.cs     1/11/2021 2:16:36 PM  >> |\VtiDataContext.designer.cs     6/19/2020 12:30:28 PM
+Forms                           1/15/2021 8:00:18 AM     +Forms                           1/6/2021 1:20:10 PM
|+ParamChangeLogForm.cs          1/12/2021 9:25:54 AM  >> |
|+ParamChangeLogForm.Designer.cs 1/11/2021 4:16:12 PM  >> |
|\ParamChangeLogForm.resx        1/11/2021 4:16:12 PM  >> |
+Properties                      1/18/2021 8:03:08 AM     +Properties                      1/6/2021 1:04:38 PM
|+AssemblyInfo.cs                1/18/2021 8:03:08 AM  >> |+AssemblyInfo.cs                1/6/2021 1:04:38 PM
|+Settings.Designer.cs           1/11/2021 1:55:46 PM  >> |+Settings.Designer.cs           9/30/2020 1:59:30 PM
|\Settings.settings              1/11/2021 1:55:46 PM  >> |\Settings.settings              9/30/2020 1:59:30 PM
+app.config                      1/11/2021 1:55:46 PM  >> +app.config                      9/30/2020 1:59:30 PM
\VTIWindowsControlLibrary.csproj 1/12/2021 7:51:02 AM  >> \VTIWindowsControlLibrary.csproj 11/30/2020 9:17:50 AM
----------------------------------------------------------------------------------------------------------------
```

### 01/06/21 - 1.4.0.5 - NJ

	Files Changed:
	EditCycleForm.cs	--> Bug fix. If string variable contains single quote, replace with two single quotes 
				--> before writing to dbo.ParamChangeLog to avoid syntax error.
	
### 01/05/21 - 1.4.0.4 - NJ

	Files Changed:
	Inquire.cs		--> Added a GetName() function to get the name of the Inquire Form.
				--> Can now scan a serial number into the inquire form:
				--> In your project's ParseScannerText(), after the `"TempScannerText = ScannerText.Trim();"` line, add:
```
				if (Form.ActiveForm != null && Form.ActiveForm.Name == Inquire.GetName())
            				{
                				if (TempScannerText.StartsWith(Config.Control.SerialNumberStartingChar.ProcessValue)
                                    			&& Config.Control.SerialNumberStartingChar.ProcessValue != "")
                					{
                    						TempScannerText = TempScannerText.Substring(1, TempScannerText.Length - 1);
                					}
                					Inquire.LookupSerialNumber(TempScannerText);
                					return;
            				}
```
	InquireForm.cs		--> Can now use the remote VtiData database to view data from all VTI machines. This option is set in the third tab of the form.
	SelectModelForm.cs	--> Update model list before showing the form.
	CycleStep.cs		--> New CycleStep variable "WriteUutRecordDetail". True by default. Set to false in your CycleStep definition to prevent a UutRecordDetail from automatically being written after the step if you want to write custom data instead.
	CycleStepBase.cs	--> Round elapsed time (seconds) to two decimal places when creating a new UutRecordDetail.
	DataPlotControl.cs	--> Can save plot as a .csv file. 
				--> Saved data plot file now reports time in minutes if the x-axis is set to minutes. 
				--> Write "string.Empty" instead of a 0 when a trace's value cannot be written to avoid false data. 
	ParamChangeLogScript.sql--> Added this file so that it can be available in the field to add the necessary database table.
	VtiData.mdf.SEED	--> Added a .SEED extension so that it will work better with git. To use it to replace an old database file, just removed the .SEED extension.
	VtiData_log.lfd.SEED	--> Added a .SEED extension so that it will work better with git. To use it to replace an old database file, just removed the .SEED extension.
	AssemblyInfo.cs		--> Increased AssemblyVersion and AssemblyFileVersion from 1.4.0.3 to 1.4.0.4.


### 11/25/20 - 1.4.0.3 - NJ

	Files Added:
	TorrconIV.cs/resx/designer	--> Torrcon IV component by Steven.
	
	Files Changed:
	VtiData.mdf/VtiData_log.ldf	--> Added a primary key to the dbo.ParamChangeLog table.
									Deleted old data.
	AssemblyInfo.cs				--> Increased AssemblyVersion and AssemblyFileVersion from 1.4.0.2 to 1.4.0.3

### 10/26/20 - 1.4.0.2 - NJ

	Files Changed:
	AssemblyInfo.cs				--> Increased AssemblyVersion and AssemblyFileVersion from 1.4.0.1 to 1.4.0.2
	PlotPropForm.cs				--> Changed logic to better format the YMin and YMax values displayed in the Data Plot Properties form.
	EditCycleForm.cs			--> Now logs changes to Edit Cycle parameters to a new required database table.
									This required table has been added to the VtiData.mdf file included in the Data folder of this library.
									Also logs when a model is added/deleted.
									The new table should be added to the local VtiData database.
									If using the VtiLib.ConnectionString2, which is set when the software writes the UutRecords and UutRecordDetails to a remote server (in addition to its usual local database), the new dbo.ParamChangeLog table must also be added to the remote server.
									The script for the new dbo.ParamChangeLog table is located here:
									\\SVR-STORAGE\Central\I-Teams\Software I-Team\CSharp Development\_Procedures and Notes\ParamChangeLogScript.sql
									Column names in the dbo.ParamChangeLog table are:
									DateTime, OpID, OverrideOpID, SystemID, ParameterSectionName, ParameterName, OldValue, NewValue.
									If the currently logged in operator does not have permission to view the Edit Cycle form, the key pad is displayed asking for the password of a user who DOES have permission to view the Edit Cycle form. If the entered password corresponding to the user has permission to view the Edit Cycle form, that user's OpID will be recorded as OverrideOpID in the table if they change an Edit Cycle parameter.
	VtiDataContect.cs			--> Records the Override user when a satisfactory password is entered (see above).
	GraphControl.cs				--> Changed logic to better format the YMin, YMax, and in-between tick mark values displayed on the y-axis of the data plot.
									Also, the y-axis panel now grows in width if the y-axis labels get too long.
	ChangedParameter.cs			--> New class, used for the Parameter Change Log feature.
	AllUsersSettingsProvider.cs --> Checks if variable is null before continuing in order to not rely on a try/catch for it.
	VtiLibLocalization.resx		--> Added "NewModelAdded" and "ModelDeleted."
	VtiLibLocalization.es.resx	--> Added "NewModelAdded" and "ModelDeleted."
	VtiLib.cs					--> Added new variable, User overrideUser.
	VtiData.mdf/VtiData_log.ldf	--> Added the new required table dbo.ParamChangeLog
	
### 9/30/20 - 1.4.0.1 - NJ

	Files Changed:
	AboutBoxForm.cs				--> Added a new property "PLCLibVersion", added a new label to the AboutBoxForm which displays the current PLC or MCC Library version.
	AboutBox.cs					--> Added code to get the current PLC or MCC Library version and display it on the AboutBoxForm.

### 9/30/20 - 1.4.0.0 - NJ

	Files Changed:
	Settings.settings			--> new setting: "SortCommonEditCycleParams" (true/false)
	AssemblyInfo.cs				--> Increase AssemblyVersion and AssemblyFileVersion from 1.3.41.0 to 1.4.0.0
	EditCycleForm.cs			--> Added code to sort the Common Edit Cycle parameters aphabetically when they are displayed if the "SortCommonEditCycleParams" setting is set to true.
	AboutBoxForm.cs				--> Added a property "ControlLibVersion" and a new label on the form for the current Control Library version.
	AboutBox.cs					--> Added a line to set the new Control Library version label to the current Control Library version.
	AllUsersSettingsProvider.cs	--> commented-out the previously-used Edit Cycle config file path and use the pre-defined path defined in AllUsersSettingsProvider.cs instead. 
									This is done so that the Edit Cycle config file path is only defined once in the library.
	VtiEventLog.cs				--> commented-out the previously-used Edit Cycle config file path and use the pre-defined path defined in AllUsersSettingsProvider.cs instead.
	ObjectConfiguration.cs		--> commented-out the previously-used Edit Cycle config file path and use the pre-defined path defined in AllUsersSettingsProvider.cs instead.

### 9/29/20 - "CLEANED" - 1.3.41.0 - NJ

	Ran the Visual Studio extension "CodeMaid" on the library. This removes unused using statements and reformats code.

### 9/29/20 - 1.3.41.0 - NJ

	Files Changed:
	DefectCodes.cs				--> Search for the DefectCodes file in more folders than just the bin\Debug folder.

### 6/1/20 - NJ

	Files Changed:
	OptimaOP900Scale.cs				--> Added this file, new SerialIO component: Scale
	EditCycleForm.cs				--> Added MessageBox warning to tell the developer that there may be duplicate IO names in their VtiPLCInterface.config file. 
										Before this, it was hard to find what the error was.
	InficonLDS3000ASCIIMDB.cs		--> Commented out logging communication error to avoid filling up the Event Log so quickly
	VtiStandardMessages.Designer.cs	-->	Removed BasePressureCheck messages
	VtiStandardMessages.resx		-->	Removed BasePressureCheck messages
		
### 2/13/20

	Files Changed: 
	InficonLDS3000ASCIIMDB.cs	--> Added Thread.Sleep(50) to SendACommandString() so that the H2Hs Pro can work with the LDS3000Form.
	DigitalIOForm.cs		--> Does not add a checkbox entry to the form if Description of Input or Output == string.Empty. (now done for the Active AND Inactive version of the form.)

### 11/27/19

	Files Changed: TorrconIII.cs

	Added Gain, Offset, etc. parameters. This was used for a manual command feature to auto-adjust the Torrcon gain/offset to reach a setpoint pressure.

### 10/31/19

	Files Changed:
	DefaultTraceColors.cs       --> Change default DataPlot colors.
	IDigitalIO.cs               --> Changed to Description {get; set} from just get
	InficonLDS3000ASCIIMDB.cs   --> In SendACommandString(), changed byte array to size 100 instead of 50.
	InficonP3000ASCIIMDB.cs     --> return CommErr instead of ERROR.
	SoloTempController.cs	    --> Changed Description {get; private set} to Description {get; set}
	UnavailableDigitalInput.cs  --> Changed to Description {get; set} from just get
	UnavailableDigitalOutput.cs --> Changed to Description {get; set} from just get
	InquireForm.cs		    --> Automatically sorts UUTRecords by DateTime descend and Details by DateTime ascend.
	InquireForm.Designer	    --> (same)
	InquireForm.resx	    --> (same)
	DigitalIOForm.cs      	    --> Does not add a checkbox entry to the form if Description of Input or Output == string.Empty.

### 7/26/19

	Changed VtiEventLog.cs and SystemDiagnostics.cs to change Trace Level order to Off, Info, Error, Warning, Verbose, and added filtering such that the Error selection includes Error and Info messages, Warning includes Warning, Error, and Info messages, etc.

### 7/22/19 

	Changed VtiEventLog.cs, DataPlotControl.Designer.cs, and GraphControl.cs. 
	Changed Event Log functions "WriteError, WriteWarning, WriteVerbose" to point to "WriteInfo", since this is the only one that works since we have switched to Notepad/text Event Log.
	Changed Data plot to format Y-Axis label numbers better.
	
### 4/29/19

	Changed HLD6000 file and added "LegendForPrintedPlot" variable to enable/disable showing the legend when the Data Plot is printed.

### ### 4/26/19

	Added increment and decrement labels to data plot arrows. Also switched position of data plot arrows to be more intuitive.

### 4/24/19

	Changed .config file location to C:\VTI PC\Config

### 4/17/19

	Changed "Please contact VTI." to "Please try again and contact VTI if problem persists."

### 1/10/2019 

	Added units to initialization for Athena controllers.

### 9-12-2018

	Bug fix to OpformDualNested2 

### 6-11-2018 VTIWindowsControlLibrary 1.4_Signal Resize Edits

	OpformSingleNested (Updates Only)
	Allows the System Signal Panel Width and Fonts to be adjusted from Client
	Allows the Test History Font to be adjusted
	Allows the undocked row and column count to be adjusted
	Currently works with OperatorFormSingleNested ONLY
Working on OpformDualNested2 7/18/18

### 10-9-2017 VTIWindowsControlLibrary 1.4

	Added TestHistoryControl ToolTip (Optional parameter for AddEntry)
	Added ToolTip to ValvespanelControl

### 8/16/2017 VTIWindowsControlLibrary 1.4

	Added mouse wheel scroll to manual commands
	fixed errors shown in logs at startup due to database schema updates

### 5/23/2017 VTIWindowsControlLibrary 1.4

	Added SchematicLabel.cs control
	Modified SchematicPanel for resize and positioning of Schematic Label
	Use a tick event on the schematic to update the label when form is visible (timer 500 ms)
	see VTI MRLTS for example or LANL 15140
	must add a schematicForm_VisibleChanged event 
```
	private void SchematicForm_VisibleChanged(object sender, EventArgs e)
        {
            if (!timer1.Enabled && this.Visible) timer1.Enabled = true;
        }
```

### 2/1/2017

	1.3 Uses Windows Event Log - Discontinue use going forward
	
Added VTIWindowsControl Library 1.4 - Text only event logging
Has copy Good config file on startup error if config file is corrupt.  Must manually create Good file.

```
private void GetConfigurationGroupAndSection(
.
.
// Read the configuration file
      //****mdb 3/4/16
      try
      {
          config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
      }
      catch
      {
          fileMap.ExeConfigFilename = configurationFile + "Good";
          config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
      }
      //****mdb 3/4/a6
```
	
### 10/11/2016 Update
	Bug Fixes/Changes:
		DefectentryForm was made public and no defect button has public access
		Added internal leak set to LDS3000
	Adders:
		HLD6000 module
		
### 8/2/2016 Updates
	Bug Fixes/Changes:
		Digital IO changes trigger manual mode
	Adders:
		Added OperatorFormTrippeNestedTwo - Test history displayed next to operator form, dataplots are tabbed.  Used on Robertshaw MRLTS
		Minor fixes to TestHistory title on SingleNested and DualNested forms
		Added SRSPTC10 temperature controller, MKS670, TerraNova934

	
### 10-5-2015 Synced update to fix hide/show model parameters in new models other than default

	Contained minor changes to:LDS3000ASCIIMDB status, FormQuad nested included but not in csproj, PlotProp, SelectModelForm, SchematicFormBase
	New cs files for MKS 670, SRG3, SRS PTC10, Terranova 934 > See Pantex RGA for implementation
Place next revision note at top of list.

### 04/22/2015 JRT - Add Auto Save button to DataPlot.  Allows users to specify how many points to collect within dataplot window before DataPlot automatically creates an AIO file at C:\DataPlot containing the current dataplot points.  The DataPlot then stops and restarts the DataPlot

### 04/01/2015 JRT - Fixed bug in TimeDelayParameter Minutes

### 03/11/2015 Starting point