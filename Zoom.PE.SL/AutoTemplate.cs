using System;
using System.Globalization;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Zoom.PE
{
    public static class AutoTemplate
    {
        private sealed class DelegateConverter : IValueConverter
        {
            readonly Func<object, object> convert;

            public DelegateConverter(Func<object,object> convert)
            {
                this.convert = convert;
            }

            object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
            {
                return convert(value);
            }

            object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) { throw new NotSupportedException(); }
        }

        public static bool GetIsEnabled(ContentControl obj) { return (bool)obj.GetValue(IsEnabledProperty); }
        public static void SetIsEnabled(ContentControl obj, bool value) { obj.SetValue(IsEnabledProperty, value); }
        #region IsEnabledProperty = DependencyProperty.RegisterAttached(...)
        public static readonly DependencyProperty IsEnabledProperty = DependencyProperty.RegisterAttached(
            "IsEnabled",
            typeof(bool),
            typeof(AutoTemplate),
            new PropertyMetadata((sender, e) => OnIsEnabledChanged((ContentControl)sender, (bool)e.OldValue)));
        #endregion

        private static void OnIsEnabledChanged(ContentControl sender, bool oldIsEnabled)
        {
            if (sender == null)
                return;

            bool newIsEnabled = GetIsEnabled(sender);

            if (newIsEnabled)
            {
                var binding = new Binding("Content")
                {
                    Source = sender,
                    Converter = new DelegateConverter(
                        content => FindTemplate(sender, content)),
                    Mode = BindingMode.OneWay
                };

                sender.SetBinding(
                    ContentControl.ContentTemplateProperty,
                    binding);
            }
            else
            {
                sender.ClearValue(ContentControl.ContentTemplateProperty);
            }
        }

        private static object FindTemplate(ContentControl contentControl, object content)
        {
            return null;
        }
    }
}
