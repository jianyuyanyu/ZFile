using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Model;

namespace ZTAppFramework.Camera.Store
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:18:18 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CameraInfoWrapper
    {
        public int ID { get; set; }

        public CameraDataInfo Info { get; }

        public CameraInfoWrapper(CameraDataInfo info)
        {

            Info = info;
        }

        public CameraInfoWrapper(int id, CameraDataInfo info)
        {
            ID = id;
            Info = info;
        }
    }
}
