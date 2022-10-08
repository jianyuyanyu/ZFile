using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace ZTAppFramework.Camera.Extensions
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:28:16 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class MatExtensions
    {
        public static ImageSource ToImageSource(this Mat mat)
        {
            ImageSource source = null;
            var size = mat.Rows * mat.Cols * mat.Channels();

            switch (mat.Channels())
            {
                case 1:
                    source = BitmapSource.Create(mat.Cols, mat.Rows, 96, 96, PixelFormats.Gray8, null, mat.Data, size, mat.Cols);
                    break;
                case 3:
                    source = BitmapSource.Create(mat.Cols, mat.Rows, 96, 96, PixelFormats.Rgb24, null, mat.Data, size, mat.Cols * 3);
                    break;
            }

            source?.Freeze();

            return source;
        }
    }
}
