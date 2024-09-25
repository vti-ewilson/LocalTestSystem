using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace VTIWindowsControlLibrary.Classes.DefectTracking
{
    /// <summary>
    /// This class contains a static <see cref="DefectCodeList">DefectCodeList</see> property
    /// that deserializes the "VtiDefectCodes.xml" file and returns a list of
    /// <see cref="DefectCode">DefectCodes</see>.
    /// </summary>
    public class DefectCodes
    {
        private static List<DefectCode> _defectCodeList;

        /// <summary>
        /// Compares two <see cref="DefectCode">DefectCode</see> objects by comparing their <see cref="DefectCode.Description">Descriptions</see>
        /// </summary>
        public class DefectCodeDescriptionComparer : IComparer<DefectCode>
        {
            /// <summary>
            /// Compares two <see cref="DefectCode">DefectCode</see> objects by comparing their <see cref="DefectCode.Description">Descriptions</see>
            /// </summary>
            /// <param name="x">DefectCode</param>
            /// <param name="y">DefectCode</param>
            /// <returns></returns>
            public int Compare(DefectCode x, DefectCode y)
            {
                return (x.Description.CompareTo(y.Description));
            }
        }

        /// <summary>
        /// Deserializes the "VtiDefectCodes.xml" file and returns a list of <see cref="DefectCode">DefectCodes</see>
        /// </summary>
        public static List<DefectCode> DefectCodeList
        {
            get
            {
                if (_defectCodeList == null)
                {
                    try
                    {
                        XmlSerializer x = new XmlSerializer(typeof(List<DefectCode>));
                        string path = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.Parent.FullName.ToString();
                        FileInfo fileInfo = new FileInfo(System.IO.Directory.GetFiles(path, "VtiDefectCodes.xml", SearchOption.AllDirectories).OrderBy(s => s.Length).FirstOrDefault());
                        using (StreamReader s = new StreamReader(fileInfo.FullName))
                        {
                            _defectCodeList = (List<DefectCode>)x.Deserialize(s);
                        }
                    }
                    catch (Exception e)
                    {
                        VtiEvent.Log.WriteError("Error reading VtiDefectCodes.xml",
                            VTIWindowsControlLibrary.Enums.VtiEventCatType.Application_Error,
                            e.Message.ToString());
                    }
                }
                return _defectCodeList;
            }
        }
    }
}