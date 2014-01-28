using System;
using System.Windows.Data;
using System.Globalization;

namespace GU.MZ.UI.ViewModel.ValueConverters
{
    [ValueConversion(typeof(DateTime?), typeof(string))]
    class DateMaskedStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null) return null;
            DateTime? dt = System.Convert.ToDateTime(value);
            string s = dt.Value.ToShortDateString();
            return s.Replace(".",string.Empty);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string valueStr = value.ToString();
            if (string.IsNullOrEmpty(valueStr) || valueStr == "__.__.____") return null;            
                
            DateTime? dt;
            try
            {
                dt = System.Convert.ToDateTime(value);
            }
            catch (Exception)
            {
                dt = DateTime.Now;
            }
            return dt;
        }
    }
}
