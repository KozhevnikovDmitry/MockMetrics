using System;
using System.Windows.Data;

namespace SpecManager.UI.ViewModel.ValueConverter
{
    class ActiveDocumentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DockableVm)
                return value;

            return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is DockableVm)
                return value;

            return Binding.DoNothing;
        }
    }
}
