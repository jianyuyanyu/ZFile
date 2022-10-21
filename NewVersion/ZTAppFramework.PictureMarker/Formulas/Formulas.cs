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
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/21 9:17:52 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class FormulasHeleper
    {

        /// <summary>
        /// 推导坐标旋转公式
        /// </summary>
        /// <param name="Rate">旋转角度</param>
        /// <param name="CirPoint">圆心坐标</param>
        /// <param name="MovePoint">移动的坐标</param>
        /// <returns></returns>
        public static Point FormulasAngleNewPoint(double Rate,Point CirPoint,Point MovePoint)
        {
            double Rage2=Rate/180*Math.PI;

            int newX=(int)((MovePoint.X-CirPoint.X)*Math.Cos(Rage2)-(MovePoint.Y-CirPoint.Y)*Math.Sin(Rage2));

            int newy = (int)((MovePoint.Y - CirPoint.Y) * Math.Cos(Rage2)+(MovePoint.X-CirPoint.X)*Math.Sin(Rage2));

            Point newPoint=new Point(CirPoint.X+newX,CirPoint.Y+newy);
            return newPoint;
        }
    }
}
