using System;
using System.Globalization;
using System.Windows.Data;

namespace Common.UI.ViewModel.ValueConverters
{
    /// <summary>
    /// Класс-конвертер для обрезания входной строки до нужного кол-ва знаков. Кол-во знаков задаётся параметром.
    /// </summary>
    [ValueConversion(typeof(string), typeof(string))]
    public class StringTruncateValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {            
            int len = System.Convert.ToInt32(parameter);
            if (value.ToString().Length > len)
            {
                return value.ToString().Substring(0, len);
            }
            return value.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
