using Component.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Data;

namespace TranmissionModel.Converters
{
    public class DownStateChanageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                DownState state = (DownState)value;
                switch (state)
                {
                    case DownState.Start:
                        return "\xe65a";
                    case DownState.Suspend:
                        return "\xe74f";
                    default:
                        return "\xe74f";
                }
                
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
