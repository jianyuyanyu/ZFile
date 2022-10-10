using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Card.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/10/10 8:57:52 
    /// Description   ：  轴参数配置
    ///********************************************/
    /// </summary>
    public class AxisParm
    {

        public int[] m_PulseMode { get; set; } = new int[6]; // 脉冲模式 0 脉冲方向 1 正交脉冲
        public int[] m_PulseDir { get; set; } = new int[6]; // 脉冲方向,0正向/1反向
        public double[] m_PulseEquivalent { get; set; } = new double[6]; // 脉冲当量
        public int[] m_HomeDir { get; set; } = new int[6]; // 回零方向 1：正方；0：负方向
        public int[] m_HomeSignalEdge { get; set; } = new int[6]; // 回零边沿 0 下降沿/1 上升沿
        public int[] m_HomePriority { get; set; } = new int[6]; // 回零优先级  
        public int[] m_SoftLimitEn { get; set; } = new int[6];// 软限位开关 0 关/1 开
        public double[] m_SoftPLimit { get; set; } = new double[6]; // 正软件限位(mm)
        public double[] m_SoftNLimit { get; set; } = new double[6]; // 负软件限位(mm)
        public int[] m_HardPLimitEn { get; set; } = new int[6];// 正硬限位开关 0 关/1 开
        public int[] m_HardNLimitEn { get; set; } = new int[6];// 负硬限位开关 0 关/1 开
        public int[] m_HardLimitPolarity { get; set; } = new int[6];// 硬限位极性 0 低电平/1 高电平
        public int[] m_ServoAlarmEn { get; set; } = new int[6];// 伺服报警使能 0 关/1 开
        public int[] m_ServoAlarmPolarity { get; set; } = new int[6];// 伺服报警信号极性 0 低电平/1 高电平
        public double[] m_RefPos { get; set; } = new double[6]; // 参考点(mm)
    }
}
