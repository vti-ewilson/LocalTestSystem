using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;

namespace VTIWindowsControlLibrary.Forms
{
    public partial class LDS3000CommandForm : Form
    {
        private Label[] _LabelDiscriptions, _LabelValues, _LabelUnits;
        private CheckBox[] _UpdateCheckboxes;
        private bool[] _WhichCheckboxesChecked;
        private string[] ReadingsInficonCommands;
        private string[] ConfigurationInficonCommands;
        private TextBox[] ConfigTextBoxes;
        private Label[] ConfigLabels;
        private Button[] ConfigReadButtons;
        private Button[] ConfigSetButtons;
        private string trigger1unit;
        private string trigger2unit;
        private string trigger3unit;
        private int ReadingIndex = 0;
        private int LastReadingIndex = 0;
        private int CalHistoryIndex = 11;
        private int ErrorIndex = 16;
        private Dictionary<string, string> ErrorMessages;
        private Dictionary<string, string> ErrorCauses;
        private Dictionary<string, string> ErrorLimitValues;
        private int ConfigurationIndex = 1;
        private bool readallconfig;
        private bool readallreadings;
        private string filepath;
        private bool throughonce;

        public InficonLDS3000ANSIIMDB serialInputs { get; private set; }

        public LDS3000CommandForm(InficonLDS3000ANSIIMDB SnifferSerialInputs)
        {
            serialInputs = SnifferSerialInputs;
            InitializeComponent();
            # region Make array with description Labels
            _LabelDiscriptions = new Label[]
            {
                LabD1,
                LabD2,
                LabD3,
                LabD4,
                LabD5,
                LabD6,
                LabD7,
                LabD8,
                LabD9,
                LabD10,
                LabD11,
                LabD12,
                LabD13,
                LabD14,
                LabD15,
                LabD16,
                LabD17,
                LabD18,
                LabD19,
                LabD20,
                LabD21,
                LabD22,
                LabD23,
                LabD24,
                LabD25,
                LabD26,
                LabD27,
                LabD28,
                LabD29,
                LabD30};
            #endregion
            # region Make array with value labels
            _LabelValues = new Label[]
            {
                LabV1,
                LabV2,
                LabV3,
                LabV4,
                LabV5,
                LabV6,
                LabV7,
                LabV8,
                LabV9,
                LabV10,
                LabV11,
                LabV12,
                LabV13,
                LabV14,
                LabV15,
                LabV16,
                LabV17,
                LabV18,
                LabV19,
                LabV20,
                LabV21,
                LabV22,
                LabV23,
                LabV24,
                LabV25,
                LabV26,
                LabV27,
                LabV28,
                LabV29,
                LabV30};
            # endregion
            # region Make array with Unit labels
            _LabelUnits = new Label[]
            {
                LabU1,
                LabU2,
                LabU3,
                LabU4,
                LabU5,
                LabU6,
                LabU7,
                LabU8,
                LabU9,
                LabU10,
                LabU11,
                LabU12,
                LabU13,
                LabU14,
                LabU15,
                LabU16,
                LabU17,
                LabU18,
                LabU19,
                LabU20,
                LabU21,
                LabU22,
                LabU23,
                LabU24,
                LabU25,
                LabU26,
                LabU27,
                LabU28,
                LabU29,
                LabU30};
            # endregion
            # region Make array with Update Checkboxes
            _UpdateCheckboxes = new CheckBox[]
            {
                checkBox1,
                checkBox2,
                checkBox3,
                checkBox4,
                checkBox5,
                checkBox6,
                checkBox7,
                checkBox8,
                checkBox9,
                checkBox10,
                checkBox11,
                checkBox12,
                checkBox13,
                checkBox14,
                checkBox15,
                checkBox16,
                checkBox17,
                checkBox18,
                checkBox19,
                checkBox20,
                checkBox21,
                checkBox22,
                checkBox23,
                checkBox24,
                checkBox25,
                checkBox26,
                checkBox27,
                checkBox28,
                checkBox29,
                checkBox30};
            # endregion
            #region make array of Configuration Text boxs
            ConfigTextBoxes = new TextBox[]
            {
                textBox1,
                textBox2,
                textBox3,
                textBox4,
                textBox5,
                textBox6,
                textBox7,
                textBox8,
                textBox9,
                textBox10,
                textBox11,
                textBox12,
                textBox13,
                textBox14,
                textBox15,
                textBox16,
                textBox17,
                textBox18,
                textBox19,
                textBox20,
                textBox21,
                textBox22,
                textBox24,
                textBox28,
                textBox27,
                textBox26,
                textBox25,
                textBox30,
                textBox31
            };
            #endregion
            #region make array of Configuration Labels
            ConfigLabels = new Label[]
            {
                label9,
                label11,
                label14,
                label15,
                label16,
                label17,
                label18,
                label19,
                label20,
                label21,
                label22,
                label23,
                label24,
                label25,
                label26,
                label27,
                label29,
                label30,
                label31,
                label32,
                label33,
                label34,
                label36,
                label40,
                label39,
                label38,
                label37,
                label51,
                label52
            };
            #endregion
            #region make array of Configuration Set buttons
            ConfigSetButtons = new Button[]
            {
            button1,
            button2,
            button3,
            button4,
            button5,
            button6,
            button7,
            button8,
            button9,
            button10,
            button11,
            button12,
            button13,
            button14,
            button15,
            button16,
            button17,
            button18,
            button19,
            button20,
            button21,
            button22,
            button45,
            button47,
            button48,
            button49,
            button50,
            button57,
            button59
            };
            #endregion
            #region make array of Configuration Read buttons
            ConfigReadButtons = new Button[]
            {
                button23,
                button24,
                button25,
                button26,
                button27,
                button28,
                button29,
                button30,
                button31,
                button32,
                button33,
                button34,
                button35,
                button36,
                button37,
                button38,
                button39,
                button40,
                button41,
                button42,
                button43,
                button44,
                button51,
                button52,
                button53,
                button54,
                button55,
                button58,
                button60
            };
            #endregion
            #region make array of inficon Commands for the configuration tab
            ConfigurationInficonCommands = new string[]
            {
                "*conf:cal:int",
                "*conf:cal:extv",
                "*conf:cal:exts",
                "*conf:mass",
                "*conf:trigger1:",
                "*conf:trigger2:",
                "*conf:trigger3:",
                "*conf:unit:lrv",
                "*conf:unit:lrs",
                "*conf:unit:P",
                "*conf:zerot",
                "*conf:speedtmp",
                "*hour:date",
                "*hour:dev",
                "*hour:pow",
                "*hour:time",
                "*hour:turbo",
                "*hour:tc",
                "*fac:facs",
                "*fac:facm",
                "*fac:cals",
                "*fac:calv",
                "*idn:version",
                "*idn:serial",
                "*idn:cuversion",
                "*idn:ioversion",
                "*idn:tchardware",
                "*conf:cat",
                "*stat:valve:tl"
            };
            #endregion
            #region Initialize Values
            #region initialize _WhichCheckboxesChecked
            _WhichCheckboxesChecked = new bool[30]
                {
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false,
                    false
                };
            #endregion
            #region initialize dictionaries
            ErrorMessages = new Dictionary<string, string>();
            ErrorMessages.Add("WRN102", "Timeout EEPROM MSB box (number of parameters)");
            ErrorMessages.Add("WRN104", "One EEPROM paramater initi- alized, (number of parameter)");
            ErrorMessages.Add("WRN106", "EEPROM paramaters initia- lized, (number of parameter)");
            ErrorMessages.Add("WRN110", "Clock is not set");
            ErrorMessages.Add("WRN122", "No answer from bus module");
            ErrorMessages.Add("WRN123", "Configuration INFICON not supported by BM1000");
            ErrorMessages.Add("WRN125", "I/O module disconnected");
            ErrorMessages.Add("WRN127", "Wrong bootloader version");
            ErrorMessages.Add("ERR130", "Sniffer not connected");
            ErrorMessages.Add("WRN132", "SL3000 not supported");
            ErrorMessages.Add("WRN150", "Pressure sensor 2 not connected");
            ErrorMessages.Add("WRN201", "U24_MSB too low");
            ErrorMessages.Add("WRN202", "U24_MSB too high");
            ErrorMessages.Add("WRN203", "Output voltage 24V PWR12 out of range, (TL_valve/ GB_valve)");
            ErrorMessages.Add("WRN204", "Output voltage 24V PWR34 out of range, (valve 3/4)");
            ErrorMessages.Add("WRN205", "Output voltage 24V PWR56 out of range, (Sniff_valve/ valve6)");
            ErrorMessages.Add("WRN221", "Internal voltage 24V_RC out of range");
            ErrorMessages.Add("WRN222", "Internal voltage 24V_IO out of range");
            ErrorMessages.Add("WRN223", "Internal voltage 24V_TMP out of range");
            ErrorMessages.Add("WRN224", "Internal voltage 24V_1 out of range (Pirani)");
            ErrorMessages.Add("WRN240", "Voltage +15V out of range");
            ErrorMessages.Add("WRN241", "Voltage -15V out of range");
            ErrorMessages.Add("ERR242", "Voltage +15V or -15V shor- tened");
            ErrorMessages.Add("WRN250", "Voltage REF5V out of range");
            ErrorMessages.Add("ERR252", "Voltage REF5V shortened");
            ErrorMessages.Add("WRN300", "Anode voltage too low");
            ErrorMessages.Add("WRN301", "Anode voltage too high");
            ErrorMessages.Add("WRN302", "Suppressor voltage too low");
            ErrorMessages.Add("WRN303", "Suppressor voltage too high");
            ErrorMessages.Add("WRN304", "Anode-cathode voltage too low");
            ErrorMessages.Add("WRN305", "Anode-cathode voltage too high");
            ErrorMessages.Add("ERR306", "Anode voltage wrong");
            ErrorMessages.Add("WRN310", "Cathode 1 broken");
            ErrorMessages.Add("WRN311", "Cathode 2 broken");
            ErrorMessages.Add("ERR312", "Cathodes broken");
            ErrorMessages.Add("ERR340", "Emission error");
            ErrorMessages.Add("WRN342", "Cathodes not connected");
            ErrorMessages.Add("WRN350", "Suppressor not connected");
            ErrorMessages.Add("WRN352", "Preamplifier not connected");
            ErrorMessages.Add("WRN360", "Preamp output too low");
            ErrorMessages.Add("WRN361", "Preamp offset too high");
            ErrorMessages.Add("WRN362", "Preamp range error");
            ErrorMessages.Add("WRN390", "500 G out of range");
            ErrorMessages.Add("ERR400", "Error number of TMP");
            ErrorMessages.Add("WRN401", "Warning number of the TMP");
            ErrorMessages.Add("ERR402", "No communication with TMP");
            ErrorMessages.Add("ERR403", "TMP speed too low");
            ErrorMessages.Add("ERR404", "TMP current too high");
            ErrorMessages.Add("WRN405", "No runup TMP");
            ErrorMessages.Add("ERR410", "TMP temperature too high");
            ErrorMessages.Add("WRN411", "TMP temperature high");
            ErrorMessages.Add("ERR420", "TMP voltage too high");
            ErrorMessages.Add("WRN421", "TMP voltage too low");
            ErrorMessages.Add("ERR422", "TMP no run-up");
            ErrorMessages.Add("ERR423", "TMP pressure rise");
            ErrorMessages.Add("WRN500", "Pressure sensor not connected");
            ErrorMessages.Add("WRN502", "Pressure sensor 2 not connected");
            ErrorMessages.Add("WRN520", "Pressure too high");
            ErrorMessages.Add("WRN521", "Pressure rise, anode breakdown");
            ErrorMessages.Add("WRN522", "Pressure rise, emission break down");
            ErrorMessages.Add("WRN540", "Pressure too low, sniffer blocked");
            ErrorMessages.Add("ERR541", "Sniffer blocked (p1)");
            ErrorMessages.Add("WRN542", "Sniffer broken");
            ErrorMessages.Add("WRN550", "Pressure too low, XL sniffer blocked");
            ErrorMessages.Add("WRN552", "XL sniffer broken");
            ErrorMessages.Add("WRN554", "XL sniffer P2 too low");
            ErrorMessages.Add("WRN600", "Calfac too low");
            ErrorMessages.Add("WRN601", "Calfac too high");
            ErrorMessages.Add("WRN602", "Calfac lower than with last calibration");
            ErrorMessages.Add("WRN603", "Calfac higher than with last calibration");
            ErrorMessages.Add("WRN604", "No int cal due to valve control");
            ErrorMessages.Add("WRN605", "Signal difference too small");
            ErrorMessages.Add("WRN610", "Machine factor too low");
            ErrorMessages.Add("WRN611", "Machine factor too high");
            ErrorMessages.Add("WRN612", "Machine factor lower than last time");
            ErrorMessages.Add("WRN613", "Machine factor higher than last time");
            ErrorMessages.Add("WRN625", "Int. test leak not set");
            ErrorMessages.Add("WRN626", "Ext. Test leak not set");
            ErrorMessages.Add("WRN630", "Calibration request");
            ErrorMessages.Add("WRN650", "Calibration is not recommended during first 20 minutes");
            ErrorMessages.Add("WRN670", "Calibration error");
            ErrorMessages.Add("WRN671", "Peak not found");
            ErrorMessages.Add("WRN680", "Difference from calibration");
            ErrorMessages.Add("WRN700", "Preamplifier temp. too low");
            ErrorMessages.Add("WRN702", "Preamplifier temp. too high");
            ErrorMessages.Add("WRN710", "MSB temperature too high");
            ErrorMessages.Add("ERR711", "Max. MSB temperature exceeded");
            ErrorMessages.Add("WRN901", "Maintenance bearing/lubricant");
            ErrorMessages.Add("WRN910", "Maintenance membrane pump");
            ErrorCauses = new Dictionary<string, string>();
            ErrorCauses.Add("WRN102", "EEPROM on IF board or MSB defective");
            ErrorCauses.Add("WRN104", "Following software update or EEPROM defective");
            ErrorCauses.Add("WRN106", "Following software update or EEPROM defective");
            ErrorCauses.Add("WRN110", "Jumper for clock not set, battery drained, clock defective");
            ErrorCauses.Add("WRN122", "Connection to BUS module interrupted");
            ErrorCauses.Add("WRN123", "The selected configuration INFICON is not supported by the connected BM1000 field bus type.");
            ErrorCauses.Add("WRN125", "Connection to I/O module interrupted");
            ErrorCauses.Add("WRN127", "Boot loader not compatible with application");
            ErrorCauses.Add("ERR130", "The sniffer line is not electrical connected.");
            ErrorCauses.Add("WRN132", "Only the SL3000XL must be used with the XL sniffer adapter");
            ErrorCauses.Add("WRN150", "Connecting pressure sensor PSG500 to a FINE connection.");
            ErrorCauses.Add("WRN201", "24 V power supply pack");
            ErrorCauses.Add("WRN202", "24 V power supply pack");
            ErrorCauses.Add("WRN203", "Short circuit at valve 1 (calibration leak) or valve 2 (gas ballast)");
            ErrorCauses.Add("WRN204", "Short circuit at valve 3 or valve 4");
            ErrorCauses.Add("WRN205", "Short circuit at valve 5 (sniff) or valve 6");
            ErrorCauses.Add("WRN221", "Short circuit 24 V on the control unit output");
            ErrorCauses.Add("WRN222", "Short circuit 24 V at IO output");
            ErrorCauses.Add("WRN223", "Short circuit 24 V of the TMP");
            ErrorCauses.Add("WRN224", "Short circuit 24 V Pressure sensor PSG500 (1,2,3), sniffer line");
            ErrorCauses.Add("WRN240", "+15 V too low, IF board or MSB defective");
            ErrorCauses.Add("WRN241", "-15 V too low, short circuit at preamplifier, IF board or MSB defective");
            ErrorCauses.Add("ERR242", "+15 V or -15 V too low, short circuit at preamplifier, IF board or MSB defective");
            ErrorCauses.Add("WRN250", "+15 V or 5 V too low, short circuit at preamplifier, IF board or MSB defective");
            ErrorCauses.Add("ERR252", "+15 V or REV5V too low, short circuit at preamplifier, IF board or MSB defective");
            ErrorCauses.Add("WRN300", "Short circuit anode voltage, pressure in mass spectrometer too high, IF board, MSB or ion source defective");
            ErrorCauses.Add("WRN301", "MSB defective");
            ErrorCauses.Add("WRN302", "Short circuit suppressor, IF board or MSB defective");
            ErrorCauses.Add("WRN303", "MSB defective");
            ErrorCauses.Add("WRN304", "Short circuit anode-cathode, IF board or MSB defective");
            ErrorCauses.Add("WRN305", "MSB defective");
            ErrorCauses.Add("ERR306", "The anode voltage does not comply with the default value or the default value is outside of the permissible setting range.");
            ErrorCauses.Add("WRN310", "Cathode defective, line to cathode interrupted, IF board or MSB defective");
            ErrorCauses.Add("WRN311", "Cathode defective, line to cathode interrupted, IF board or MSB defective");
            ErrorCauses.Add("ERR312", "Cathode defective, line to cathode interrupted, IF board or MSB defective");
            ErrorCauses.Add("ERR340", "Emission was stable previously, pressure probably too high, message after 15 s");
            ErrorCauses.Add("WRN342", "Both cathodes defective during self-testing or plug not connected");
            ErrorCauses.Add("WRN350", "Suppressor cable during self- testing not connected or defective");
            ErrorCauses.Add("WRN352", "Preamplifier defective, cable not plugged in");
            ErrorCauses.Add("WRN360", "Poor ion source or contaminated mass spectrometer");
            ErrorCauses.Add("WRN361", "Preamplifier defective");
            ErrorCauses.Add("WRN362", "Preamplifier or MSB box defective");
            ErrorCauses.Add("WRN390", "Preamplifier defective, error at the suppressor, IF board or MSB defective");
            ErrorCauses.Add("ERR400", "na");
            ErrorCauses.Add("WRN401", "na");
            ErrorCauses.Add("ERR402", "Cable to TMP / TMP defective, IF board or MSB defective");
            ErrorCauses.Add("ERR403", "Pressure too high, TMP defective");
            ErrorCauses.Add("ERR404", "na");
            ErrorCauses.Add("WRN405", "Pressure too high, TMP faulty");
            ErrorCauses.Add("ERR410", "Cooling failed, operating conditions check MSB-module");
            ErrorCauses.Add("WRN411", "Cooling failed, operating conditions check MSB-module");
            ErrorCauses.Add("ERR420", "TMP power supply unit defective");
            ErrorCauses.Add("WRN421", "Wire cross-section 24-V supply for MSB-modules too low, output voltage of 24-V power supply unit too low (I <10 A), power supply unit defective, TMP defective");
            ErrorCauses.Add("ERR422", "Input pressure TMP too high, end pressure VV pump too high, leaking high vacuum system, deluge valve not closed, bearing damage TMP, TMP defective");
            ErrorCauses.Add("ERR423", "Air infiltration, deluge valve defective or incorrectly dimensioned");
            ErrorCauses.Add("WRN500", "Pressure sensor PSG500 not connected, IF board or MSB defective");
            ErrorCauses.Add("WRN502", "Pressure sensor PSG500 P2 not connected, IF board or MSB defective");
            ErrorCauses.Add("WRN520", "Pressure p1 too high");
            ErrorCauses.Add("WRN521", "Pressure p1 too high, message after 1.4 s");
            ErrorCauses.Add("WRN522", "Emission was stable previously, pressure p1 too high, message after 5 s");
            ErrorCauses.Add("WRN540", "Sniffer clogged, sniffer valve defective,filter clogged");
            ErrorCauses.Add("ERR541", "Sniffer blocked, sniffer valve defective (pressure lower than half of the configured warning value), filter clogged");
            ErrorCauses.Add("WRN542", "Sniffer broken");
            ErrorCauses.Add("WRN550", "Clean or replace the high flow capillary of the sniffer line. Replaced soiled filter");
            ErrorCauses.Add("WRN552", "Replace the high flow capillary of the sniffer line.");
            ErrorCauses.Add("WRN554", "Pressure on SL3000XL too low in low flow.");
            ErrorCauses.Add("WRN600", "Calibration leak or machine factor set incorrectly");
            ErrorCauses.Add("WRN601", "Calibration leak or machine factor set incorrectly, split flow factor too high");
            ErrorCauses.Add("WRN602", "Calibration leak, machine factor or split flow factor has changed");
            ErrorCauses.Add("WRN603", "Calibration leak, machine factor or split flow factor has changed");
            ErrorCauses.Add("WRN604", "Test leak is not enabled");
            ErrorCauses.Add("WRN605", "Test leak defective or signal too weak.");
            ErrorCauses.Add("WRN610", "Machine factor adjustment inaccurate");
            ErrorCauses.Add("WRN611", "Machine factor adjustment inaccurate, split flow factor too high");
            ErrorCauses.Add("WRN612", "Split flow factor has changed");
            ErrorCauses.Add("WRN613", "Split flow factor has changed");
            ErrorCauses.Add("WRN625", "Leakage rate of int. test leak is still set to factory setting");
            ErrorCauses.Add("WRN626", "Leakage rate of test leak is still set to factory setting");
            ErrorCauses.Add("WRN630", "Temperature change of 5 °C, speed was changed since last calibration, 30-minute switch- on time and still no calibration conducted");
            ErrorCauses.Add("WRN650", "A calibration is not recommended during the first 20 minutes after the start (warm-up phase) of the leak detector.");
            ErrorCauses.Add("WRN670", "You have to recalibrate because there was a problem during calibration.");
            ErrorCauses.Add("WRN671", "The signal was too irregular during the peak search. Calibration was canceled");
            ErrorCauses.Add("WRN680", "A check of the calibration has shown that you should recalibrate.");
            ErrorCauses.Add("WRN700", "Temperature too low");
            ErrorCauses.Add("WRN702", "Temperature too high");
            ErrorCauses.Add("WRN710", "Temperature too high");
            ErrorCauses.Add("ERR711", "Temperature too high");
            ErrorCauses.Add("WRN901", "TMP maintenance necessary");
            ErrorCauses.Add("WRN910", "8000 hour maintenance of diaphragm pump required");
            ErrorLimitValues = new Dictionary<string, string>();
            ErrorLimitValues.Add("WRN102", "na");
            ErrorLimitValues.Add("WRN104", "na");
            ErrorLimitValues.Add("WRN106", "na");
            ErrorLimitValues.Add("WRN110", "na");
            ErrorLimitValues.Add("WRN122", "na");
            ErrorLimitValues.Add("WRN123", "na");
            ErrorLimitValues.Add("WRN125", "na");
            ErrorLimitValues.Add("WRN127", "na");
            ErrorLimitValues.Add("ERR130", "na");
            ErrorLimitValues.Add("WRN132", "na");
            ErrorLimitValues.Add("WRN150", "na");
            ErrorLimitValues.Add("WRN201", "21.6 V");
            ErrorLimitValues.Add("WRN202", "26.4 V");
            ErrorLimitValues.Add("WRN203", "20 V 30V");
            ErrorLimitValues.Add("WRN204", "20 V 30V");
            ErrorLimitValues.Add("WRN205", "20 V 30V");
            ErrorLimitValues.Add("WRN221", "20 V 30V");
            ErrorLimitValues.Add("WRN222", "20 V 30V");
            ErrorLimitValues.Add("WRN223", "20 V 30V");
            ErrorLimitValues.Add("WRN224", "20 V 30V");
            ErrorLimitValues.Add("WRN240", "na");
            ErrorLimitValues.Add("WRN241", "na");
            ErrorLimitValues.Add("ERR242", "na");
            ErrorLimitValues.Add("WRN250", "4.5 V 5.5 V");
            ErrorLimitValues.Add("ERR252", "na");
            ErrorLimitValues.Add("WRN300", "7 V < of the set point");
            ErrorLimitValues.Add("WRN301", "7 V > of the set point");
            ErrorLimitValues.Add("WRN302", "297 V");
            ErrorLimitValues.Add("WRN303", "363 V");
            ErrorLimitValues.Add("WRN304", "40 V");
            ErrorLimitValues.Add("WRN305", "140 V");
            ErrorLimitValues.Add("ERR306", "40 V deviation from default value");
            ErrorLimitValues.Add("WRN310", "na");
            ErrorLimitValues.Add("WRN311", "na");
            ErrorLimitValues.Add("ERR312", "na");
            ErrorLimitValues.Add("ERR340", "< 90% of the set point >110% of the set point");
            ErrorLimitValues.Add("WRN342", "na");
            ErrorLimitValues.Add("WRN350", "na");
            ErrorLimitValues.Add("WRN352", "na");
            ErrorLimitValues.Add("WRN360", "<-70mV at 500 GΩ");
            ErrorLimitValues.Add("WRN361", ">+/-50 mC at 500 GΩ >+/-10 mV at 15 GΩ <+/-10 mV at 470 GΩ <+/-9 mV at 13 MΩ");
            ErrorLimitValues.Add("WRN362", "na");
            ErrorLimitValues.Add("WRN390", "450 GΩ 550 GΩ");
            ErrorLimitValues.Add("ERR400", "na");
            ErrorLimitValues.Add("WRN401", "na");
            ErrorLimitValues.Add("ERR402", "na");
            ErrorLimitValues.Add("ERR403", "< 95% of the set point");
            ErrorLimitValues.Add("ERR404", "3A");
            ErrorLimitValues.Add("WRN405", "5 min.");
            ErrorLimitValues.Add("ERR410", "61 °C");
            ErrorLimitValues.Add("WRN411", "60 °C");
            ErrorLimitValues.Add("ERR420", "na");
            ErrorLimitValues.Add("WRN421", "na");
            ErrorLimitValues.Add("ERR422", "8 min.");
            ErrorLimitValues.Add("ERR423", "na");
            ErrorLimitValues.Add("WRN500", "0.5 V");
            ErrorLimitValues.Add("WRN502", "na");
            ErrorLimitValues.Add("WRN520", "18 mbar");
            ErrorLimitValues.Add("WRN521", "< Set point - 20V");
            ErrorLimitValues.Add("WRN522", "< 90% of the set point > 110% of the set point");
            ErrorLimitValues.Add("WRN540", "Parameters sniffer flow warning");
            ErrorLimitValues.Add("ERR541", "na");
            ErrorLimitValues.Add("WRN542", "na");
            ErrorLimitValues.Add("WRN550", "na");
            ErrorLimitValues.Add("WRN552", "na");
            ErrorLimitValues.Add("WRN554", "na");
            ErrorLimitValues.Add("WRN600", "0.01");
            ErrorLimitValues.Add("WRN601", "10000");
            ErrorLimitValues.Add("WRN602", "< 50% of the old value");
            ErrorLimitValues.Add("WRN603", "> 200% of the old value");
            ErrorLimitValues.Add("WRN604", "na");
            ErrorLimitValues.Add("WRN605", "na");
            ErrorLimitValues.Add("WRN610", "0.0001");
            ErrorLimitValues.Add("WRN611", "10000");
            ErrorLimitValues.Add("WRN612", "< 50% of the old value");
            ErrorLimitValues.Add("WRN613", "> 200% of the old value");
            ErrorLimitValues.Add("WRN625", "na");
            ErrorLimitValues.Add("WRN626", "na");
            ErrorLimitValues.Add("WRN630", "na");
            ErrorLimitValues.Add("WRN650", "na");
            ErrorLimitValues.Add("WRN670", "na");
            ErrorLimitValues.Add("WRN671", "na");
            ErrorLimitValues.Add("WRN680", "na");
            ErrorLimitValues.Add("WRN700", "2 °C");
            ErrorLimitValues.Add("WRN702", "60 °C");
            ErrorLimitValues.Add("WRN710", "55 °C");
            ErrorLimitValues.Add("ERR711", "65 °C");
            ErrorLimitValues.Add("WRN901", "3 years");
            ErrorLimitValues.Add("WRN910", "na");

            #endregion
            #endregion
            for (int i = 0; i < _LabelDiscriptions.Length; i++)
            {
                if (_UpdateCheckboxes[i].Checked) _WhichCheckboxesChecked[i] = true;
                else _WhichCheckboxesChecked[i] = false;
            }
            #region Make array of Readings Inficon Commands
            ReadingsInficonCommands = new string[]
            {
                "*READ:ATM*cc/s?",
                "*MEAS:P1:TORR?",
                "*MEAS:P2:TORR?",
                "*MEAS:P3?",
                "*MEAS:P4?",
                "*MEAS:UVV?",
                "*MEAS:MIAP?",
                "*MEAS:MIKP?",
                "*MEAS:MISP?",
                "*MEAS:MIAKP?",
                "*MEAS:U15N?",
                "*MEAS:U15P?",
                "*MEAS:U24?",
                "*MEAS:U24IO?",
                "*MEAS:U24IO_OUT?",
                "*MEAS:U24PI?",
                "*MEAS:U24PWR1_2?",
                "*MEAS:U24PWR5_6?",
                "*MEAS:U5?",
                "*MEAS:TEMPeratur:Amplifier?",
                "*MEAS:TEMPeratur:Electronic?",
                "*MEAS:TEMPeratur:TCElectronic?",
                "*MEAS:TEMPeratur:TCPump?",
                "*MEAS:TEMPeratur:TCBearing?",
                "*MEAS:TEMPeratur:TCMotor?",
                "*MEAS:TURBO:Frequency?",
                "*MEAS:TURBO:Voltage?",
                "*MEAS:TURBO:Current?",
                "*MEAS:TURBO:Power?",
                "*MEAS:U24RC?"};
            # endregion
            comboBox1.SelectedIndex = 0;
            trigger1unit = "atm*cc/s";
            comboBox2.SelectedIndex = 0;
            trigger2unit = "atm*cc/s";
            comboBox3.SelectedIndex = 0;
            trigger3unit = "atm*cc/s";
            readallconfig = false;
            readallreadings = false;
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {
        }

        private void checkBox_CheckedChanged(object sender, System.EventArgs e)
        {
            for (int i = 0; i < _LabelDiscriptions.Length; i++)
            {
                if (_UpdateCheckboxes[i].Checked) _WhichCheckboxesChecked[i] = true;
                else _WhichCheckboxesChecked[i] = false;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            switch (Tabs.SelectedTab.ToString())
            {
                # region Readings Tab
                case "TabPage: {Readings}":
                    if (_WhichCheckboxesChecked.Contains(true))
                    {
                        try
                        {
                            _LabelValues[LastReadingIndex].ForeColor = System.Drawing.Color.Black;
                            if (serialInputs.ReturnString != "")
                            {
                                _LabelValues[ReadingIndex].Text = serialInputs.ReturnString;
                                _LabelValues[ReadingIndex].ForeColor = System.Drawing.Color.Green;
                                if (readallreadings)
                                {
                                    try
                                    {
                                        using (System.IO.StreamWriter stw = new System.IO.StreamWriter(filepath, true))
                                        {
                                            stw.WriteLine(_LabelDiscriptions[ReadingIndex].Text + ":" + serialInputs.ReturnString);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                            }

                            for (int i = ReadingIndex + 1; i <= _LabelDiscriptions.Length + ReadingIndex; i++)
                            {
                                int iterate = i <= _LabelDiscriptions.Length - 1 ? i : i - _LabelDiscriptions.Length;
                                if (_WhichCheckboxesChecked[iterate] == true)
                                {
                                    LastReadingIndex = ReadingIndex;
                                    ReadingIndex = iterate;
                                    if (i > _LabelDiscriptions.Length - 1 && !throughonce) readallreadings = false;
                                    else if (throughonce) throughonce = false;
                                    break;
                                }
                            }

                            serialInputs.CommandString = ReadingsInficonCommands[ReadingIndex];
                            serialInputs.SendCommandString = true;
                        }
                        catch
                        {
                        }
                    }
                    break;
                #endregion

                #region Calibration Tab
                case "TabPage: {Calibration}":
                    if (CalHistoryIndex < 10)
                    {
                        if (serialInputs.ReturnString != "")
                        {
                            if (serialInputs.ReturnString.Substring(0, 3) == "Fac")
                            {
                                int rowadded;
                                var vari = serialInputs.ReturnString;
                                String[] result = vari.Split(' ');
                                if (result.Length == 15) dataGridView1.Rows.Add(result[1], result[3], result[5], result[6], result[9], result[10], result[12], result[14].Substring(0, 3));
                                else dataGridView1.Rows.Add(result[1], result[3], result[5], result[6], result[8], result[9], result[11], result[13].Substring(0, 3));
                                rowadded = dataGridView1.Rows.Count - 1;
                                if (result.Length == 15)
                                {
                                    if (result[14].Substring(0, 3) == "000") dataGridView1.Rows[rowadded].DefaultCellStyle.ForeColor = Color.Black;
                                    else dataGridView1.Rows[rowadded].DefaultCellStyle.ForeColor = Color.Red;
                                }
                                else
                                {
                                    if (result[13].Substring(0, 3) == "000") dataGridView1.Rows[rowadded].DefaultCellStyle.ForeColor = Color.Black;
                                    else dataGridView1.Rows[rowadded].DefaultCellStyle.ForeColor = Color.Red;
                                }
                                dataGridView1.Refresh();
                                CalHistoryIndex++;
                            }
                        }
                        serialInputs.CommandString = "*STAT:CALH " + (CalHistoryIndex + 1).ToString() + "?";
                        serialInputs.SendCommandString = true;
                    }
                    break;
                #endregion

                #region Errors tab
                case "TabPage: {Errors}":
                    if (ErrorIndex < 16)
                    {
                        ErrorFormActionTxt.Text = "Updating Error History";
                if (serialInputs.ReturnString != "" && !(serialInputs.ReturnString.Substring(0,1) == "E" && serialInputs.ReturnString.Length ==3) && serialInputs.ReturnString.Length >=5 && serialInputs.ReturnString.Substring(0,5)!="ERROR")
                        {
                            if (serialInputs.ReturnString.Substring(0, 3) == "WRN" || serialInputs.ReturnString.Substring(0, 3) == "ERR")
                            {
                                int rowadded;
                                var vari = serialInputs.ReturnString;
                                String[] result1 = vari.Split(' ');
                                String[] result = new String[20];
                                int ind = 0;
                                for (int i = 0; i < 20; i++)
                                {
                                    try
                                    {
                                        if (result1[i] != "")
                                        {
                                            result[ind] = result1[i];
                                            ind++;
                                        }
                                    }
                                    catch
                                    {
                                    }
                                }
                                string errmess, errlmtvalues, errcause;
                                ErrorMessages.TryGetValue(result[0].ToUpper(), out errmess);
                                ErrorLimitValues.TryGetValue(result[0].ToUpper(), out errlmtvalues);
                                ErrorCauses.TryGetValue(result[0].ToUpper(), out errcause);

                                ErrorsdataGridView2.Rows.Add(result[0].Substring(0, 3), result[0].Substring(3, 3), result[1], result[2], result[3], result[5], result[7], errmess, errlmtvalues, errcause);
                                ErrorsdataGridView2.Refresh();
                                rowadded = ErrorsdataGridView2.Rows.Count - 1;
                                if (serialInputs.ReturnString.Substring(0, 3) == "WRN") ErrorsdataGridView2.Rows[rowadded].DefaultCellStyle.BackColor = Color.Orange;
                                else ErrorsdataGridView2.Rows[rowadded].DefaultCellStyle.BackColor = Color.Red;
                                ErrorIndex++;
                            }
                        }
                        serialInputs.CommandString = "*STAT:ERRH " + (ErrorIndex + 1).ToString() + "?";
                        serialInputs.SendCommandString = true;
                    }
                    else
                    {
                        switch (ErrorIndex.ToString())
                        {
                            case "16":
                                ErrorFormActionTxt.Text = "Updating Current Error Number";
                                serialInputs.CommandString = "*STAT:ERR?";
                                serialInputs.SendCommandString = true;
                                ErrorIndex++;
                                break;

                            case "17":
                                if (serialInputs.ReturnString != "" && !(serialInputs.ReturnString.Substring(0, 1) == "E" && serialInputs.ReturnString.Length == 3))
                                {
                                    ErrorFormActionTxt.Text = "Error Updated";
                                    ErrorNumberlabel.Text = serialInputs.ReturnString;
                                }
                                ErrorIndex = 16;
                                break;

                            case "18":
                                ErrorFormActionTxt.Text = "Clearing Error";
                                serialInputs.CommandString = "*CLS";
                                serialInputs.SendCommandString = true;
                                ErrorIndex++;
                                break;

                            case "19":
                                if (serialInputs.ReturnString == "OK")
                                {
                                    ErrorFormActionTxt.Text = "Error Cleared";
                                }
                                else
                                {
                                    ErrorFormActionTxt.Text = "Failed to Clear Error";
                                }
                                ErrorIndex = 16;
                                break;

                            default:
                                break;
                        }
                    }
                    break;
                #endregion

                #region Configuration Tab
                case "TabPage: {Configuration}":
                    if (ConfigurationIndex <= (2 * ConfigSetButtons.Length) + 1 && ConfigurationIndex != 1)
                    {
                        //Set Buttons
                        if (ConfigurationIndex % 2 == 0)
                        {
                            //send command
                            if (ConfigurationIndex < 10)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text;
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == 10)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + trigger1unit + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text;
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == 12)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + trigger2unit + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text;
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == 14)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + trigger3unit + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text;
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == 26)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text.Replace(".", ",");
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == 32)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text.Replace(":", ",");
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[(ConfigurationIndex / 2) - 1] + " " + ConfigTextBoxes[(ConfigurationIndex / 2) - 1].Text;
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                        }
                        else
                        {
                            //Receive Return String
                            int whichtextbox = ((ConfigurationIndex - 1) / 2) - 1;
                            if (serialInputs.ReturnString != "")
                            {
                                ConfigTextBoxes[whichtextbox].Text = serialInputs.ReturnString;
                                ConfigurationIndex = 1;
                            }
                        }
                    }
                    else if (ConfigurationIndex > (2 * ConfigSetButtons.Length) + 1)
                    {
                        //Read Buttons
                        if (ConfigurationIndex % 2 == 0)
                        {
                            //send command
                            if (ConfigurationIndex < (ConfigSetButtons.Length * 2) + 10)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[((ConfigurationIndex - (ConfigSetButtons.Length * 2)) / 2) - 1] + "?";
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == (ConfigSetButtons.Length * 2) + 10)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[((ConfigurationIndex - (ConfigSetButtons.Length * 2)) / 2) - 1] + trigger1unit + "?";
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == (ConfigSetButtons.Length * 2) + 12)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[((ConfigurationIndex - (ConfigSetButtons.Length * 2)) / 2) - 1] + trigger2unit + "?";
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else if (ConfigurationIndex == (ConfigSetButtons.Length * 2) + 14)
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[((ConfigurationIndex - (ConfigSetButtons.Length * 2)) / 2) - 1] + trigger3unit + "?";
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                            else
                            {
                                serialInputs.CommandString = ConfigurationInficonCommands[((ConfigurationIndex - (ConfigSetButtons.Length * 2)) / 2) - 1] + "?";
                                serialInputs.SendCommandString = true;
                                ConfigurationIndex = ConfigurationIndex + 1;
                            }
                        }
                        else
                        {
                            //receive string
                            int whichtextbox = (((ConfigurationIndex - 1) - (ConfigSetButtons.Length * 2)) / 2) - 1;
                            if (serialInputs.ReturnString != "")
                            {
                                ConfigTextBoxes[whichtextbox].Text = serialInputs.ReturnString;
                                if (readallconfig)
                                {
                                    try
                                    {
                                        using (System.IO.StreamWriter stw = new System.IO.StreamWriter(filepath, true))
                                        {
                                            stw.WriteLine(ConfigLabels[whichtextbox].Text + ":" + serialInputs.ReturnString);
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    if (ConfigurationIndex != (ConfigSetButtons.Length * 4) + 1) ConfigurationIndex = ConfigurationIndex + 1;
                                    else ConfigurationIndex = 1;
                                }
                                else ConfigurationIndex = 1;
                            }
                        }
                    }
                    break;
                #endregion

                default:
                    break;
            }
        }

        private void label11_Click(object sender, EventArgs e)
        {
        }

        private void label24_Click(object sender, EventArgs e)
        {
        }

        private void label44_Click(object sender, EventArgs e)
        {
        }

        private void label53_Click(object sender, EventArgs e)
        {
        }

        private void label59_Click(object sender, EventArgs e)
        {
        }

        private void label69_Click(object sender, EventArgs e)
        {
        }

        private void label86_Click(object sender, EventArgs e)
        {
        }

        private void LabD4_Click(object sender, EventArgs e)
        {
        }

        private void LabU4_Click(object sender, EventArgs e)
        {
        }

        private void atmccs_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radiobuttonflowunit = sender as RadioButton;
            if (radiobuttonflowunit.Checked)
            {
                _LabelUnits[0].Text = radiobuttonflowunit.Text;
                ReadingsInficonCommands[0] = "*READ:" + radiobuttonflowunit.Text + "?";
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radiobuttonpressunit = sender as RadioButton;
            if (radiobuttonpressunit.Checked)
            {
                _LabelUnits[1].Text = radiobuttonpressunit.Text;
                _LabelUnits[2].Text = radiobuttonpressunit.Text;
                ReadingsInficonCommands[1] = "*MEAS:P1:" + radiobuttonpressunit.Text + "?";
                ReadingsInficonCommands[2] = "*MEAS:P2:" + radiobuttonpressunit.Text + "?";
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void UpdateCalibration_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Refresh();
            CalHistoryIndex = 0;
        }

        private void ErrorsdataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void ErrorsUpdateButton_Click(object sender, EventArgs e)
        {
            ErrorsdataGridView2.Rows.Clear();
            ErrorsdataGridView2.Refresh();
            ErrorIndex = 0;
        }

        private void ClearErrorButton_Click(object sender, EventArgs e)
        {
            ErrorIndex = 18;
        }

        private void splitContainer1_Panel2_Paint(object sender, PaintEventArgs e)
        {
        }

        private void label15_Click(object sender, EventArgs e)
        {
        }

        private void textBox20_TextChanged(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            readallconfig = false;
            int whichbutton = -1;
            Button buttonclicked = (Button)sender;
            for (int i = 0; i < ConfigSetButtons.Length; i++)
            {
                if (ConfigSetButtons[i] == buttonclicked) whichbutton = i + 1;
            }
            ConfigurationIndex = 2 * whichbutton;
        }

        private void button23_Click(object sender, EventArgs e)
        {
            readallconfig = false;
            int whichbutton = -1;
            Button buttonclicked = (Button)sender;
            for (int i = 0; i < ConfigReadButtons.Length; i++)
            {
                if (ConfigReadButtons[i] == buttonclicked) whichbutton = i + 1;
            }
            ConfigurationIndex = (2 * whichbutton) + (ConfigSetButtons.Length * 2);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            trigger1unit = comboBox1.SelectedItem.ToString();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            trigger2unit = comboBox2.SelectedItem.ToString();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            trigger3unit = comboBox3.SelectedItem.ToString();
        }

        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }

        private void button46_Click(object sender, EventArgs e)
        {
            filepath = textBox23.Text + "LDS3000Config" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".txt";

            readallconfig = true;
            ConfigurationIndex = 2 + (ConfigSetButtons.Length * 2);
        }

        private void label37_Click(object sender, EventArgs e)
        {
        }

        private void button56_Click(object sender, EventArgs e)
        {
            filepath = textBox23.Text + "LDS3000Readings" + DateTime.Now.Day.ToString() + DateTime.Now.Month.ToString() + DateTime.Now.Year.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() + ".txt";
            for (int i = 0; i < _LabelDiscriptions.Length; i++)
            {
                if (!_UpdateCheckboxes[i].Checked)
                {
                    _UpdateCheckboxes[i].Checked = true;
                    _WhichCheckboxesChecked[i] = true;
                }
            }
            readallreadings = true;
            throughonce = true;
            ReadingIndex = _LabelDiscriptions.Length - 1;
        }
    }
}