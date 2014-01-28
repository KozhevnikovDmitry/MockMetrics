﻿using System;
using System.Windows.Data;
using System.Globalization;

namespace Common.UI.ViewModel.ValueConverters
{
    /// <summary>
    /// Класс-мультиковертер, возвращает логическое "И" от двух входных <c>bool</c>
    /// </summary>
    public class AndBoolConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)values[0] && (bool)values[1]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
