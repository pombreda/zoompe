using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Markup;
using Mi.PE.PEFormat;

namespace PEViewer
{
    public sealed class PEHeaderOffsetConverter : MarkupExtension, IValueConverter
    {
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || Equals(value, string.Empty))
                return string.Empty;

            ulong num = System.Convert.ToUInt64(value);

            num += DosHeader.HeaderSize;

            return num.ToString("X4") + "h";
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}