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
    /// 创建时间    ：  2022/10/10 9:12:10 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CardInfo
    {
        public CardInfo()
        {
            RunStatus = RunStatus.Normal;
         
        }

        public ushort _ConnectNo = 0;//控制卡号

        public bool IsGoHome = false;

        public RunStatus RunStatus { get;  set; }//控制卡运行状态
        public bool CardConnectStatus { get; set; }//控制卡链接状态

        public double[] m_CurPos = new double[6]; // 轴号坐标

        public int[] m_CurPosStatus = new int[6]; // 轴号状态

        public double[] PostionsSpeed=new double[6];//轴速度
    }
}
