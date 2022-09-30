using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Model;

namespace ZTAppFramework.Camera.Camera.interfaces
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:52:20 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public interface ICameraFactory
    {
        IEnumerable<CameraDataInfo> GetDevices();
        ICamera Connect(CameraDataInfo cameraInfo);
        bool IsExist(CameraDataInfo cameraInfo);
    }
}
