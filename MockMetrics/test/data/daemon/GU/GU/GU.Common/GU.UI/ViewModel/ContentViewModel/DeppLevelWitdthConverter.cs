using System;
using System.Globalization;
using System.Windows.Data;

namespace GU.UI.ViewModel.ContentViewModel
{
    [ValueConversion(typeof(int), typeof(int))]
    internal class DeppLevelWitdthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var adj = System.Convert.ToInt32(parameter ?? 0);

            return 800 - System.Convert.ToInt32(value) * 24 + adj;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}