using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace VTIPLCInterface
{
    public class MccDaq
    {
        public class MccBoard
        {
            int _Boardnum = 0;

            public MccBoard()
            {

            }
            public MccBoard(int boardNum)
            {
                _Boardnum = boardNum;
            }

            public int BoardNum { get {return _Boardnum;} }
            public string BoardName { get { return "PLC"; } }
        }

        // Summary:
        //     Specifies the set of ranges for A/D and D/A operations.
        public enum Range
        {
            // Summary:
            //     No range is specified.
            NotUsed = -1,
            //
            // Summary:
            //     The range is ±5.0 V.
            Bip5Volts = 0,
            //
            // Summary:
            //     The range is ±10 V.
            Bip10Volts = 1,
            //
            // Summary:
            //     The range is ±2.5 V.
            Bip2Pt5Volts = 2,
            //
            // Summary:
            //     The range is ±1.25 V.
            Bip1Pt25Volts = 3,
            //
            // Summary:
            //     The range is ±1.0 V.
            Bip1Volts = 4,
            //
            // Summary:
            //     The range is ±0.625 V.
            BipPt625Volts = 5,
            //
            // Summary:
            //     The range is ±0.5 V.
            BipPt5Volts = 6,
            //
            // Summary:
            //     The range is ±0.1 V.
            BipPt1Volts = 7,
            //
            // Summary:
            //     The range is ±0.05 V.
            BipPt05Volts = 8,
            //
            // Summary:
            //     The range is ±0.01 V.
            BipPt01Volts = 9,
            //
            // Summary:
            //     The range is ±0.005 V.
            BipPt005Volts = 10,
            //
            // Summary:
            //     The range is ±1.67 V.
            Bip1Pt67Volts = 11,
            //
            // Summary:
            //     The range is ±0.25 V.
            BipPt25Volts = 12,
            //
            // Summary:
            //     The range is ±0.2 V.
            BipPt2Volts = 13,
            //
            // Summary:
            //     The range is ±2.0 V.
            Bip2Volts = 14,
            //
            // Summary:
            //     The range is ±20.0 V.
            Bip20Volts = 15,
            //
            // Summary:
            //     The range is ±4.0 V.
            Bip4Volts = 16,
            //
            // Summary:
            //     The range is ±0.3125 V.
            BipPt312Volts = 17,
            //
            // Summary:
            //     The range is ±0.15625 V.
            BipPt156Volts = 18,
            //
            // Summary:
            //     The range is ±0.078125 V.
            BipPt078Volts = 19,
            //
            // Summary:
            //     The range is ±60.0 V.
            Bip60Volts = 20,
            //
            // Summary:
            //     The range is ±15.0 V.
            Bip15Volts = 21,
            //
            // Summary:
            //     The range is ±0.125 V.
            BipPt125Volts = 22,
            //
            // Summary:
            //     The range is ±0.125 V.
            Bip30Volts = 23,
            //
            // Summary:
            //     The range is 0 - 10 volts.
            Uni10Volts = 100,
            //
            // Summary:
            //     The range is 0 - 5.0 volts.
            Uni5Volts = 101,
            //
            // Summary:
            //     The range is 0 - 2.5 volts.
            Uni2Pt5Volts = 102,
            //
            // Summary:
            //     The range is 0 - 2.0 volts.
            Uni2Volts = 103,
            //
            // Summary:
            //     The range is 0 - 1.25 volts.
            Uni1Pt25Volts = 104,
            //
            // Summary:
            //     The range is 0 - 1.0 volt.
            Uni1Volts = 105,
            //
            // Summary:
            //     The range is 0 - 0.1 volt.
            UniPt1Volts = 106,
            //
            // Summary:
            //     The range is 0 - 0.01 volt.
            UniPt01Volts = 107,
            //
            // Summary:
            //     The range is 0 - 0.02 volt.
            UniPt02Volts = 108,
            //
            // Summary:
            //     The range is 0 - 1.67 volts.
            Uni1Pt67Volts = 109,
            //
            // Summary:
            //     The range is 0 - 0.5 volt.
            UniPt5Volts = 110,
            //
            // Summary:
            //     The range is 0 - 0.25 volt.
            UniPt25Volts = 111,
            //
            // Summary:
            //     The range is 0 - 0.2 volt.
            UniPt2Volts = 112,
            //
            // Summary:
            //     The range is 0 - 0.05 volt.
            UniPt05Volts = 113,
            //
            // Summary:
            //     The range is 0 - 4.0 volts.
            Uni4Volts = 114,
            //
            // Summary:
            //     The range is 4 to 20 mA.
            Ma4To20 = 200,
            //
            // Summary:
            //     The range is 2 to 10 mA.
            Ma2To10 = 201,
            //
            // Summary:
            //     The range is 1 to 5 mA.
            Ma1To5 = 202,
            //
            // Summary:
            //     The range is 0.5 to 2.5 mA.
            MaPt5To2Pt5 = 203,
            //
            // Summary:
            //     The range is 0 to 20 mA.
            Ma0To20 = 204,
            //
            // Summary:
            //     The range is -0.025 to 0.02520 Amps.
            BipPt025Amps = 205,
            //
            // Summary:
            //     The range is ±0.025 V/V.
            BipPt025VoltsPerVolt = 400,
        }


        // Summary:
        //     Specifies the names of the digital port types.
        public enum DigitalPortType
        {
            // Summary:
            //     Specifies an AuxPort digital port.
            AuxPort = 1,
            //
            // Summary:
            //     Specifies a FirstPortA digital port.
            FirstPortA = 10,
            //
            // Summary:
            //     Specifies a FirstPortB digital port.
            FirstPortB = 11,
            //
            // Summary:
            //     Specifies a FirstPortCL digital port.
            FirstPortCL = 12,
            //
            // Summary:
            //     Specifies a FirstPortC digital port.
            FirstPortC = 12,
            //
            // Summary:
            //     Specifies a FirstPortCH digital port.
            FirstPortCH = 13,
            //
            // Summary:
            //     Specifies a SecondPortA digital port.
            SecondPortA = 14,
            //
            // Summary:
            //     Specifies a SecondPortB digital port.
            SecondPortB = 15,
            //
            // Summary:
            //     Specifies a SecondPortCL digital port.
            SecondPortCL = 16,
            //
            // Summary:
            //     Specifies a SecondPortCH digital port.
            SecondPortCH = 17,
            //
            // Summary:
            //     Specifies a ThirdPortA digital port.
            ThirdPortA = 18,
            //
            // Summary:
            //     Specifies a ThirdPortA digital port.
            ThirdPortB = 19,
            //
            // Summary:
            //     Specifies a ThirdPortA digital port.
            ThirdPortCL = 20,
            //
            // Summary:
            //     Specifies a ThirdPortA digital port.
            ThirdPortCH = 21,
            //
            // Summary:
            //     Specifies a FourthPortA digital port.
            FourthPortA = 22,
            //
            // Summary:
            //     Specifies a FourthPortB digital port.
            FourthPortB = 23,
            //
            // Summary:
            //     Specifies a FourthPortCL digital port.
            FourthPortCL = 24,
            //
            // Summary:
            //     Specifies a FourthPortCH digital port.
            FourthPortCH = 25,
            //
            // Summary:
            //     Specifies a FifthPortA digital port.
            FifthPortA = 26,
            //
            // Summary:
            //     Specifies a FifthPortB digital port.
            FifthPortB = 27,
            //
            // Summary:
            //     Specifies a FifthPortCL digital port.
            FifthPortCL = 28,
            //
            // Summary:
            //     Specifies a FifthPortCH digital port.
            FifthPortCH = 29,
            //
            // Summary:
            //     Specifies a SixthPortA digital port.
            SixthPortA = 30,
            //
            // Summary:
            //     Specifies a SixthPortB digital port.
            SixthPortB = 31,
            //
            // Summary:
            //     Specifies a SixthPortCL digital port.
            SixthPortCL = 32,
            //
            // Summary:
            //     Specifies a SixthPortCH digital port.
            SixthPortCH = 33,
            //
            // Summary:
            //     Specifies a SeventhPortA digital port.
            SeventhPortA = 34,
            //
            // Summary:
            //     Specifies a SeventhPortB digital port.
            SeventhPortB = 35,
            //
            // Summary:
            //     Specifies a SeventhPortCL digital port.
            SeventhPortCL = 36,
            //
            // Summary:
            //     Specifies a SeventhPortCH digital port.
            SeventhPortCH = 37,
            //
            // Summary:
            //     Specifies an EightPortA digital port.
            EighthPortA = 38,
            //
            // Summary:
            //     Specifies an EightPortB digital port.
            EighthPortB = 39,
            //
            // Summary:
            //     Specifies an EightPortCL digital port.
            EighthPortCL = 40,
            //
            // Summary:
            //     Specifies an EightPortCH digital port.
            EighthPortCH = 41,
        }

        // Summary:
        //     Specifies the direction of a digital I/O port.
        public enum DigitalPortDirection
        {
            // Summary:
            //     The direction of a digital I/O port is output.
            DigitalOut = 1,
            //
            // Summary:
            //     The direction of a digital I/O port is input.
            DigitalIn = 2,
        }



    }




}
