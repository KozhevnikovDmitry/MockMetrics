using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Common.UI.ViewModel.ValueConverters
{
    /// <summary>
    /// Класс конвертер отображающий входное значение <c>bool</c> в значение перечисления <c>Visibility</c>.
    /// </summary>|
    /// <remarks>
    /// Переводит <c>true</c> в <c>Visible</c>
    /// По умолчанию переводит <c>false</c> в <c>Hidden</c>. C помощью параметра можно задавать, переводить с <c>Collapsed</c> или в <c>Hidden</c>
    /// </remarks>
    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class BoolVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isChecked = (bool)value;
            if (isChecked)
            {
                return Visibility.Visible;
            }
            else
            {
                string unvisibleType = parameter != null ? parameter.ToString() : "Hidden";
                if (unvisibleType == "Collapsed")
                {
                    return Visibility.Collapsed;
                }
                else
                {
                    return Visibility.Hidden;
                }
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Visibility vis = (Visibility)value;
            if (vis == Visibility.Visible)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    [ValueConversion(typeof(bool), typeof(Visibility))]
    public class UnBoolVisibilityConverter : IValueConverter
    {
        private readonly BoolVisibilityConverter _boolVisConverter;

        public UnBoolVisibilityConverter()
        {
            _boolVisConverter = new BoolVisibilityConverter();
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var isChecked = (bool) value;
            return _boolVisConverter.Convert(!isChecked, targetType, parameter, culture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var vis = (Visibility)value;
            if (vis == Visibility.Visible)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
