using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LocalTestSystem.Classes.Configuration;
using VTIWindowsControlLibrary.Classes;
using VTIWindowsControlLibrary.Classes.IO.EthernetIO;
using VTIWindowsControlLibrary.Classes.IO.SerialIO;

namespace LocalTestSystem.Classes.IOClasses
{
    public class SerialInputs
    {
        public BarcodeScanner Scanner;
        public OptimaOP909Scale Scale;  // Set C18 = 3, command request mode

        public SerialInputs()
        {
            //Scanner = new BarcodeScanner
            //{
            //    SerialPortParameter = Config.Control.ScannerPort,
            //};
            //Scanner.SerialPort.ReadTimeout = 1000;
            //Scanner.Start();

            

        }
    }
}
