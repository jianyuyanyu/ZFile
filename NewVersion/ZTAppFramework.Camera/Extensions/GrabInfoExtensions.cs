using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using ZTAppFramework.Camera.Model;
using OpenCvSharp;

namespace ZTAppFramework.Camera.Extensions
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:26:19 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class GrabInfoExtensions
    {
        public static BitmapSource ToImageSource(this GrabInfo grabInfo)
        {
            BitmapSource source = null;
            switch (grabInfo.Channels)
            {
                case 1:
                    source = BitmapSource.Create(grabInfo.Width, grabInfo.Height, 96, 96, PixelFormats.Gray8, null, grabInfo.Data, grabInfo.Width);
                    break;
                case 3:
                    source = BitmapSource.Create(grabInfo.Width, grabInfo.Height, 96, 96, PixelFormats.Rgb24, null, grabInfo.Data, grabInfo.Width * 3);
                    break;
            }

            if (source != null)
                source.Freeze();

            return source;
        }

        public static Mat ToMat(this GrabInfo grabInfo)
        {
            switch (grabInfo.Channels)
            {
                case 1:
                    return new Mat(grabInfo.Height, grabInfo.Width, MatType.CV_8UC1, grabInfo.Data);
                case 3:
                    return new Mat(grabInfo.Height, grabInfo.Width, MatType.CV_8UC3, grabInfo.Data);
            }

            throw new NotImplementedException();
        }
    }
}
