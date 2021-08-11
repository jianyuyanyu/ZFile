using Component.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace TranmissionModel.Converters
{
    public class DownStateVisBilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DownState state = (DownState)value;
                DownState state1 = (DownState)int.Parse(parameter.ToString()) ;
                if (state == state1) return Visibility.Collapsed;
                return Visibility.Visible;
            }
            catch (Exception)
            {
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
