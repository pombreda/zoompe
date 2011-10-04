using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using Mi.PE.PEFormat;

namespace PEViewer.SL
{
    public sealed class PEHeaderOffsetConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || Equals(value, string.Empty))
                return string.Empty;

            ulong num = System.Convert.ToUInt64(value, culture);

            num += DosHeader.HeaderSize;

            return num.ToString("X4", culture) + "h";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
