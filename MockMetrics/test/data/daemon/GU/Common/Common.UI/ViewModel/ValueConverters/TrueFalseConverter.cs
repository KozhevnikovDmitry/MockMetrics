using System;
using System.Windows.Data;
using System.Globalization;

namespace Common.UI.ViewModel.ValueConverters
{
    /// <summary>
    /// Класс-конвертер, инвертирующий входное значение <c>bool</c>.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class TrueFalseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return !((bool)value);
        }
    }
}
