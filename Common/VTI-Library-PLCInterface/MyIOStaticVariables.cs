using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTIPLCInterface
{
    public class MyIOStaticVariables
    {// static members can be accessed without instantiating the class and are always available.
        //Static members are shared (via the class name) between all objects derived from this class.

        //****write the values for regulator
        public static string strBlueRegulatorSet = "";
        public static string strWhiteRegulatorSet = "";

        public static bool ReadDigIOOnly = false;
        public static bool DoNotWriteDigitalOutputs = false;
        public static bool DoNotWriteAnalogOutputs = false;

        public static int Test_Date_Time_Year = 0;
        public static int Test_Date_Time_Month = 0;
        public static int Test_Date_Time_Day = 0;
        public static int Test_Date_Time_Hour = 0;
        public static int Test_Date_Time_Minute = 0;
        public static int Test_Date_Time_Second = 0;

        public static int Passed_Test = 0;
        public static int Test_Number = 0;
        public static int Station_Number = 0;
        public static int HOLD_START_TIME = 0;
        public static int TOTAL_TEST_TIME = 0;
        public static int Micron_At_10sec_B4_Test_Cir1 = 0;
        public static int Micron_At_10sec_B4_Test_Cir2 = 0;
        public static int Micron_At_5min_Hold_Cir1 = 0;
        public static int Micron_At_5min_Hold_Cir2 = 0;
        public static int Micron_At_End_Test_Cir1 = 0;
        public static int Micron_At_End_Test_Cir2 = 0;

        public static string Unit_Serial_Num = "";
        public static int Unit_Serial_Num_Len = 0;

        public static int IntSaveIt = 0;

        public static int IntSavedOnce = 0;

        public static int ClearIt = 0;

		//****Used to write a string to an Allen Bradley PLC tag
		public static int IntSendBlueString;//set to 1 to send
		public static int IntSendWhiteString;//set to 1 to send
		public static string strBlueTagName;
		public static string strBlueStringToSend;
		public static string strWhiteTagName;
		public static string strWhiteStringToSend;
		//****


	}
}
