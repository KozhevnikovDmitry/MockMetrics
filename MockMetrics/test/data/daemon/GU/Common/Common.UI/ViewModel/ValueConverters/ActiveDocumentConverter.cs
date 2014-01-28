using System;
using System.Windows.Data;

using Common.UI.ViewModel.Interfaces;

namespace Common.UI.ViewModel.ValueConverters
{
    class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IDockableVM)
                return value;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IDockableVM)
                return value;

            return Binding.DoNothing;
        }
    }
}
