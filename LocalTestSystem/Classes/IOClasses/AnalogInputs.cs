using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VTIWindowsControlLibrary.Classes.IO;
using VTIWindowsControlLibrary.Classes.IO.Interfaces;

namespace LocalTestSystem.Classes.IOClasses
{
    /// <summary>
    /// AnalogInputs class
    /// 
    /// Subclass of the IOSubSystemBase class
    /// Contains fields for the analog inputs.  The Analog Inputs
    /// are the "raw values" (i.e. volts, milliamps, etc)
    /// Each field is of type IAnalogInput, which is an interface
    /// in the VTIWindowsControlLibrary.  At runtime, the IOSubSystemBase
    /// class constructor locates the analog input in the I/O Interface
    /// that matches the field name.
    /// </summary>
    public class AnalogInputs : IOSubSystemBase<IAnalogInput>
    {
        public AnalogInputs(IOInterface IOInterface) : base(IOInterface) { }

        public IAnalogInput CDG10TorrPressureSensorVolts;
        public IAnalogInput VacuumManifold100PSIGTransducerVolts;

		//public IAnalogInput DegasManifold100PSIGTransducerVolts;
		//public IAnalogInput ScaleVolts;

		#region


		#endregion

	}
}
