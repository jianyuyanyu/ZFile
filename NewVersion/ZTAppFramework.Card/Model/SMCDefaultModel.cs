using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Card.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/10/10 9:03:26 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class SMCDefaultModel
    {

        /// <summary>
        /// 起始速度
        /// </summary>
        public double StartSpeed { get; set; }

        /// <summary>
        /// 运行速度
        /// </summary>
        public double RunSpeed { get; set; }

        /// <summary>
        /// 停止速度
        /// </summary>
        public double StopSpeed { get; set; }

        /// <summary>
        /// 加速时间
        /// </summary>
        public double AccelTime { get; set; }

        /// <summary>
        /// 减速时间
        /// </summary>
        public double DecelerationTime { get; set; }

        /// <summary>
        /// 运动距离
        /// </summary>
        public double DisplacementThreshold { get; set; }

        /// <summary>
        /// S段时间
        /// </summary>
        public double Sprofile { get; set; }
    }
}
