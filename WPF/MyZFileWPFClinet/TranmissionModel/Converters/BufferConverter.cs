using Component.Common.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace TranmissionModel.Converters
{
    public class BufferConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                long b = int.Parse(value.ToString());
                return ByteConvert.GetSize(b);
            }
            catch (Exception)
            {
                return value;
            }          
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
