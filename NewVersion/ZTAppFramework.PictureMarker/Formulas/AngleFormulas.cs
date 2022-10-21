using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZTAppFramework.PictureMarker
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 11:04:28 
    /// Description   ：  角度计算帮助
    ///********************************************/
    /// </summary>
    public class AngleFormulas
    {
        public Point ap { get; set; }

        public Point bp { get; set; }

        public AngleFormulas()
        {

        }
        
        public AngleFormulas(Point aP, Point bp)
        {
            this.ap = ap;
            this.bp = bp;
        }

        /// <summary>
        /// 三点计算角度
        /// </summary>
        /// <param name="cen"></param>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public  double Angle(Point cen, Point first, Point second)
        {
            const double M_PI = 3.1415926535897;

            double ma_x = first.X - cen.X;
            double ma_y = first.Y - cen.Y;
            double mb_x = second.X - cen.X;
            double mb_y = second.Y - cen.Y;
            double v1 = (ma_x * mb_x) + (ma_y * mb_y);
            double ma_val = Math.Sqrt(ma_x * ma_x + ma_y * ma_y);
            double mb_val = Math.Sqrt(mb_x * mb_x + mb_y * mb_y);
            double cosM = v1 / (ma_val * mb_val);
            double angleAMB = Math.Acos(cosM) * 180 / M_PI;

            return angleAMB;
        }


        public double GetCosaAngle()
        {
            double a = bp.Y - ap.Y;
            double b = bp.X - ap.X;
            double c = Math.Sqrt(a * a + b * b);// 勾股定理 c^2 = a^2 + b^2

            double cosA = (b * b + c * c - a * a) / (2 * b * c);// 2.求角a的cos值
            double rotate = Math.Acos(cosA) * (180 / Math.PI);
            return rotate;
        }
    }
}
