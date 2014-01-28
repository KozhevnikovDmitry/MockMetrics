using System;
using System.Windows.Data;
using System.Globalization;

namespace Common.UI.ViewModel.ValueConverters
{
    /// <summary>
    /// Класс-конвертер для работы с строками задаваемыми по маске. Выкидывает из входной строки различные символы.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class MaskedNumberTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null)
            {
                return value.ToString().Replace(".", string.Empty)
                                       .Replace("(", string.Empty)
                                       .Replace(")", string.Empty)
                                       .Replace("-", string.Empty)
                                       .Replace(" ", string.Empty);
            }
            else
            {
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
