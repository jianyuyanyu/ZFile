using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;
using ZFileComponent;

namespace Component
{
    #region Double -> GridLength
    internal class GridLengthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var length = value?.ToString();
            return GridLengthUtil.ConvertToGridLength(length);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    #endregion

    #region object try to convert to image (if is uri)
    internal class IconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || !(value.GetType() == typeof(string)))
                return value;

            var iconString = value as string;

            if (!string.IsNullOrEmpty(iconString) && Uri.IsWellFormedUriString(iconString, UriKind.RelativeOrAbsolute))
            {
                var image = new System.Windows.Controls.Image()
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(iconString, UriKind.RelativeOrAbsolute)),
                };
                System.Windows.Media.RenderOptions.SetBitmapScalingMode(image, System.Windows.Media.BitmapScalingMode.HighQuality);
                return image;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }
    }
    #endregion
}
