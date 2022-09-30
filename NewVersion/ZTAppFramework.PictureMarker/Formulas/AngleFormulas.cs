using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZTAppFramework.PictureMarker.Formulas
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
