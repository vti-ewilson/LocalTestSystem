using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LocalTestSystem.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.Configuration;
using VTIWindowsControlLibrary.Classes.IO.SignalConverters;

namespace LocalTestSystem.Classes.IOClasses
{
    public class AnalogSignalConverters
    {
        public LinearSignal ExampleConverter =
                   new LinearSignal
                   {
                   };

        public LinearSignal CounterConverter = new LinearSignal
        {
            FullScale = 65536,
            Offset = 32767,
            InputMinimum = 0.0,
            InputMaximum = 10.0,
        };
        public LinearSignal CDGConverter = new LinearSignal
        {
            FullScale = 10.0,
            Offset = 0.0,
            InputMinimum = 0.0,
            InputMaximum = 10.0,
        };
		public LinearSignal PressureTransducerConverter = new LinearSignal
		{
			FullScale = 100.0,
			Offset = 0.0,
			InputMinimum = 1.0,
			InputMaximum = 5.0,
		};
	}
}
