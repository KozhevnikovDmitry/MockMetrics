using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.UI.ViewModel.ValueConverters
{
    [ValueConversion(typeof (object), typeof (bool))]
    public class NullVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return new BoolVisibilityConverter().Convert(value != null, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
