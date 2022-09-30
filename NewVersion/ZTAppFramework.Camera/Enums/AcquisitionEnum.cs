using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Camera.Enums
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:51:52 
    /// Description   ：  采集模式
    ///********************************************/
    /// </summary>
    public enum AcquisitionEnum
    {
        ContinuesMode,
        TreeModel
    }


    public enum ECameraParameter
    {
        Width, Height, OffsetX, OffsetY, Exposure, Gain, FrameRate, TriggerDelay
    }

    public enum ECameraAutoType
    {
        Exposure, Gain, WhiteBalance
    }

    public enum ECameraAutoValue
    {
        Off, Once, Continuous
    }
}
