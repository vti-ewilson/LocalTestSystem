using System;

namespace LocalTestSystem.Classes
{
    public class TestInfo
    {
        #region common
        public bool ReadyToTest { get; set; }
        public string SerialNumber { get; set; }
        public string Result { get; set; }
        public string ResultTH { get; set; }
        public DateTime StartCycleTime { get; set; }
        public bool SkipAcknowledge { get; set; }
        //public List<string> OriginalSeqTextList { get; set; }
        public bool WriteCyclePass { get; set; }
        public bool WriteCycleNoTest { get; set; }
        public bool WriteCycleFail { get; set; }

		#endregion

		public bool CycleInProcess { get; set; }


		// Cycle Vars
		public bool SupplyInProcess { get; set; } 
        public bool bLoadCounterPrimary { get; set; }
        public double CounterValueToLoadPrimary { get; set; }
        public bool bLoadCounterLimitPrimary { get; set; }
        public double CounterLimitValueToLoadPrimary { get; set; }
        public bool bLoadCounterSecondary { get; set; }
        public double CounterValueToLoadSecondary { get; set; }
        public bool bLoadCounterLimitSecondary { get; set; }
        public double CounterLimitValueToLoadSecondary { get; set; }
       
        public string ModelNumberScanned { get; internal set; }
        public DateTime startTime { get; set; }

        public int testCounter { get; set; }
    }

    public class queryResultModelXRef
    {
        public long ID { get; set; }
        public string ScannedChars { get; set; }
        public string ModelNo { get; set; }
    }

    public class queryResulModelParameters
    {
        public long ID { get; set; }
        public string ModelNo { get; set; }
        public string ParameterID { get; set; }
        public string ProcessValue { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModified { get; set; }
    }

}
