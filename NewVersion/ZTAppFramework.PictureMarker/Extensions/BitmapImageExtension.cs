using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ZTAppFramework.PictureMarker.Extensions
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 13:29:55 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class BitmapImageExtension
    {
        public static BitmapImage ImgDataTranformBit(this byte[] data, bool freeze = true, int decodeWidth = 0)
        {
            if (data == null || data.Length == 0) return null;
            BitmapImage bitmap = new BitmapImage();
            bitmap.BeginInit();
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data);
            //bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            bitmap.StreamSource = ms;
            if (decodeWidth > 0)
                bitmap.DecodePixelWidth = decodeWidth;
            bitmap.CacheOption = BitmapCacheOption.OnLoad;
            bitmap.EndInit();
            if (freeze)
                bitmap.Freeze();
            return bitmap;
        }

        public static BitmapImage ToBitmapImage(this Bitmap ImageOriginal, int decodeWidth = 0)
        {

            Bitmap ImageOriginalBase = new Bitmap(ImageOriginal);
            BitmapImage bitmapImage = new BitmapImage();
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ImageOriginalBase.Save(ms, ImageOriginal.RawFormat);
            bitmapImage.BeginInit();
            bitmapImage.StreamSource = ms;
            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
            if (decodeWidth > 0)
                bitmapImage.DecodePixelWidth = decodeWidth;
            bitmapImage.EndInit();
            bitmapImage.Freeze();
            return bitmapImage;
        }

        /// <summary>
        /// 获取图片大小
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static Size? GetJpgSize(this byte[] data)
        {
            Size? result = null;
            if (data == null)
            {
                return result;
            }

            int i = 0;
            int ff = data[i++];
            int type = data[i++];
            if (ff != 0xff || type != 0xd8)
            {//非JPG文件
                return result;
            }
            int ps = 0;
            do
            {
                do
                {
                    ff = data[i++];
                    if (ff < 0)
                    //文件结束
                    {
                        return result;
                    }
                } while (ff != 0xff);

                do
                {
                    type = data[i++];
                } while (type == 0xff);

                ps = i - 1;
                switch (type)
                {
                    case 0x00:
                    case 0x01:
                    case 0xD0:
                    case 0xD1:
                    case 0xD2:
                    case 0xD3:
                    case 0xD4:
                    case 0xD5:
                    case 0xD6:
                    case 0xD7:
                    case 0xE0:
                        break;
                    case 0xc0:
                        //SOF0段
                        ps = data[i++] << 8;
                        ps = i + ps + data[i++] - 3; //加段长度

                        i++;
                        //丢弃精度数据
                        //高度
                        Size JpgSize = new Size
                        {
                            Height = data[i++] << 8
                        };

                        JpgSize.Height = JpgSize.Height + data[i++];
                        //宽度
                        JpgSize.Width = data[i++] << 8;
                        JpgSize.Width = JpgSize.Width + data[i++];
                        result = JpgSize;
                        break;
                    default:
                        //别的段都跳过////////////////
                        ps = data[i++] << 8;
                        ps = ps + data[i++] - 3; //加段长度
                        ps += i;
                        break;
                }
                if (ps + 1 >= data.Length)
                //文件结束
                {
                    return result;
                }
                i = ps; //移动指针
            } while (type != 0xda); // 扫描行开始
            return result;
        }
    }
}
