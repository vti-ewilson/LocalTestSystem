using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Net;
using System.ComponentModel.Design.Serialization;
using System.Reflection;

namespace VTIWindowsControlLibrary.Classes.TypeUtils
{
    public class IPAddressTypeConverter : TypeConverter
    {
        #region ConvertFrom

        public override bool CanConvertFrom(ITypeDescriptorContext context, System.Type sourceType)
        {
            if (sourceType == typeof(String)) return true;
            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            IPAddress address = null;

            if (value is string) IPAddress.TryParse(value as string, out address);

            return address;
        }

        #endregion

        #region ConvertTo

        public override bool CanConvertTo(ITypeDescriptorContext context, System.Type destinationType)
        {
            if (destinationType == typeof(String)) return true;
            return base.CanConvertTo(context, destinationType);
        }

        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, System.Type destinationType)
        {
            if (value is IPAddress) return (value as IPAddress).ToString();
            return base.ConvertTo(context, culture, value, destinationType);
        }

        #endregion
    }
}
