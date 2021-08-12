using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Component
{
    public class FormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string Formatstr="";
            switch (value.ToString())
            {
                case "zip":
                    Formatstr = "\xe522";
                    break;
                case "mp4":
                    Formatstr = "\xe605";
                    break;
                case "avi":
                    Formatstr = "\xe611";
                    break;
                case "exe":
                    Formatstr = "\xe517";
                    break;
                default:
                    Formatstr = "\xe603";
                    break;
            }
            return Formatstr;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
