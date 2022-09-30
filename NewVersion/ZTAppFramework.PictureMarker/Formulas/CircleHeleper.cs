using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZTAppFramework.PictureMarker.Model;

namespace ZTAppFramework.PictureMarker.Formulas
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/9/29 16:27:21 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CircleHeleper
    {
        #region 属性
        Dictionary<int, Point> Points = new Dictionary<int, Point>();
        #endregion
        public CircleHeleper()
        {

        }

        public bool IsMeetCirclMethod()
        {
            if (Points.Count >= 3)
                return true;
            return false;
        }
        public void AddPoint(Point point)
        {
            if (Points.Count >= 3)
                throw new Exception("Point More than three"); ;
            Points.Add(Points.Count, point);
        }

        public CircleData Start_Compute_Three_Point_Draw_Cirle()
        {
            double a = Points[0].X - Points[1].X;// X1-X2
            double b = Points[0].Y - Points[1].Y;//Y1-Y2
            double c = Points[0].X - Points[2].X;//X1-X3
            double d = Points[0].Y - Points[2].Y;//Y1-Y3
            double aa = Math.Pow(Points[0].X, 2) - Math.Pow(Points[1].X, 2);//X1^2-X2^2
            double bb = Math.Pow(Points[1].Y, 2) - Math.Pow(Points[0].Y, 2);//Y2^2-Y1^2
            double cc = Math.Pow(Points[0].X, 2) - Math.Pow(Points[2].X, 2);//X1^2-X3^2
            double dd = Math.Pow(Points[2].Y, 2) - Math.Pow(Points[0].Y, 2);//Y3^2-Y1^2
            double E = (aa - bb) / 2;
            double F = (cc - dd) / 2;
            double resultY = (a * F - c * E) / (a * d - b * c);
            double resultX = (F * b - E * d) / (b * c - a * d);
            double resultR = Math.Sqrt((Math.Pow((Points[0].X - resultX), 2)) + (Math.Pow((Points[0].Y - resultY), 2)));

            Points.Clear();
            return new CircleData(resultX, resultY, resultR);
        }



    }
}
