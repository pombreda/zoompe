using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows;

namespace Zoom.PE
{
    public sealed class ProportionalConverter : DependencyObject, IValueConverter
    {
        public double Proportion { get { return (double)GetValue(ProportionProperty); } set { SetValue(ProportionProperty, value); } }
        #region ProportionProperty = DependencyProperty.Register(...)
        public static readonly DependencyProperty ProportionProperty = DependencyProperty.Register(
            "Proportion",
            typeof(double),
            typeof(ProportionalConverter),
            new PropertyMetadata(1.0));
        #endregion

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double typedValue = System.Convert.ToDouble(value, culture);
            double converted = typedValue * this.Proportion;
            return System.Convert.ChangeType(converted, targetType, culture);
        }

        object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
    }
}