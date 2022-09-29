using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.PictureMarker.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 16:37:24 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CircleData
    {
        public CircleData(double resultX, double resultY, double resultR)
        {
            CircleX = resultX;
            CircleY = resultY;
            CircleR = resultR;
        }

        public double CircleX { get; set; }
        public double CircleY { get; set; }

        public double CircleR { get; set; }
    }
}
