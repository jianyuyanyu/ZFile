using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZTAppFramework.Camera.Model;
using ZTAppFramework.Camera.Service;

namespace ZTAppFramework.Camera.Store
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:17:47 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CameraStore
    {
        public ObservableCollection<CameraInfoWrapper> Cameras { get; private set; }
        public CameraStore()
        {
            Initialize();
            Refresh();
        }

        public void Add(CameraDataInfo info) => Cameras.Add(new CameraInfoWrapper(info));
        public void Add(int ID, CameraDataInfo info) => Cameras.Add(new CameraInfoWrapper(ID, info));

        public void Refresh()
        {
            int NUM = 0;
            foreach (var item in CameraService.GetDeviceInfos())
            {      
                Add(NUM, item);
                NUM++;
            }

        }

        void Initialize()
        {
            Cameras = new ObservableCollection<CameraInfoWrapper>();
           
        }

    }
}
