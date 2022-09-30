using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Enums;
using ZTAppFramework.Camera.Model;

namespace ZTAppFramework.Camera.Camera.interfaces
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:50:50 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public interface ICamera
    {
        Action<GrabInfo> ImageGrabbed { get; set; }
        Action GrabStarted { get; set; }
        Action GrabDone { get; set; }

        bool StartGrab(int grabCount = -1);
        bool Stop();
        bool Disconnect();
        bool IsConnected();

        bool SetParameter(ECameraParameter parameter, double value);
        bool SetTriggerMode(bool isTriggerMode);
        bool SetAuto(ECameraAutoType type, ECameraAutoValue value);
        bool SetROI(uint x, uint y, uint width, uint height);

        CameraParameterInfo GetParameterInfo();
    }
}
