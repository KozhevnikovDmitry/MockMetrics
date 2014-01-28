using System;
using System.Windows.Data;
using System.Globalization;
using GU.Archive.DataModel;

namespace GU.Archive.UI.ViewModel.ValueConverters
{
    [ValueConversion(typeof(Sex), typeof(int))]
    public class SexIntegerValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? (object) System.Convert.ToInt32(value) : null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value != null ? (object) ((Sex) value) : null;
        }
    }
}
