using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Enums;
using ZTAppFramework.Camera.Model;
using static MvCamCtrl.NET.MyCamera;

namespace ZTAppFramework.Camera.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 11:39:37 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class CameraServiceMediator
    {
        Dictionary<CameraDataInfo, CameraService> _serviceDictionary;

        public Action<int, GrabInfo> ImageGrabbed { get; set; }
        public Action<int> GrabDone { get; set; }

        DelegateDictionary _delegateDictionary;

        public CameraServiceMediator()
        {
            _serviceDictionary = new Dictionary<CameraDataInfo, CameraService>();
            _delegateDictionary = new DelegateDictionary();
        }

        public void Connect(int id, CameraDataInfo info)
        {
            if (_serviceDictionary.ContainsKey(info) == false)
                _serviceDictionary[info] = new CameraService();

            if (_serviceDictionary[info].IsConnected() == false)
            {
                if (_serviceDictionary[info].Connect(info) == false)
                    return;
            }

            if (_serviceDictionary.ContainsKey(info))
            {
                var grabbed = _delegateDictionary.GetDelegate(id, (cameraInfo, grabInfo) => ImageGrabbed?.Invoke(id, grabInfo));
                if (_serviceDictionary[info].ImageGrabbed == null)
                    _serviceDictionary[info].ImageGrabbed += grabbed;
                else if (_serviceDictionary[info].ImageGrabbed.GetInvocationList().Contains(grabbed) == false)
                    _serviceDictionary[info].ImageGrabbed += grabbed;

                var grabDone = _delegateDictionary.GetDelegate(id, (cameraInfo) => GrabDone?.Invoke(id));
                if (_serviceDictionary[info].GrabDone == null)
                    _serviceDictionary[info].GrabDone += grabDone;
                if (_serviceDictionary[info].GrabDone.GetInvocationList().Contains(grabDone) == false)
                    _serviceDictionary[info].GrabDone += grabDone;
            }
        }


        public void Disconnect()
        {
            foreach (var service in _serviceDictionary.Values)
                service.Disconnect();

            _serviceDictionary.Clear();
        }

        public bool SetExposure(CameraDataInfo info, double value)
        {
            return _serviceDictionary[info].SetParameter(ECameraParameter.Exposure, value);
        }


        public bool SetGain(CameraDataInfo info, double value)
        {
            return _serviceDictionary[info].SetParameter(ECameraParameter.Gain, value);
        }

        public bool SetPixelFormat(CameraDataInfo info, int type)
        {
            return _serviceDictionary[info].SetPixelFormat((MvGvspPixelType)type);
        }

        public (double Cur, double Min, double Max) GetExposure(CameraDataInfo info)
        {
            var parameterInfo = _serviceDictionary[info].GetParameterInfo();
            if (parameterInfo == null)
                return (0, 0, 0);

            var exposure = parameterInfo.Parameters[ECameraParameter.Exposure];

            return (exposure.Current, exposure.Min, exposure.Max);
        }

        public CameraParameterInfo GetCameraParameInfo(CameraDataInfo info)
        {
            var parameterInfo = _serviceDictionary[info].GetParameterInfo();
            if (parameterInfo == null) return null;
            return parameterInfo;
        }

        public (double Cur, double Min, double Max) GetGain(CameraDataInfo info)
        {
            var parameterInfo = _serviceDictionary[info].GetParameterInfo();
            if (parameterInfo == null)
                return (0, 0, 0);

            var gain = parameterInfo.Parameters[ECameraParameter.Gain];

            return (gain.Current, gain.Min, gain.Max);
        }

        public async Task<GrabInfo?> Grab(CameraDataInfo info)
        {
            if (_serviceDictionary.ContainsKey(info) == false)
                return null;

            return await _serviceDictionary[info].Grab();
        }

        public void Stop(CameraDataInfo info)
        {
            if (_serviceDictionary.ContainsKey(info) == false)
                return;

            _serviceDictionary[info].Stop();
        }

        public void Live(CameraDataInfo info)
        {
            if (_serviceDictionary.ContainsKey(info) == false)
                return;

            _serviceDictionary[info].StartGrab();
        }
    }

    class DelegateDictionary
    {
        private IDictionary<int, Action<CameraDataInfo, GrabInfo>> _grabbedDictionary;
        private IDictionary<int, Action<CameraDataInfo>> _donedDictionary;

        public DelegateDictionary()
        {
            _grabbedDictionary = new Dictionary<int, Action<CameraDataInfo, GrabInfo>>();
            _donedDictionary = new Dictionary<int, Action<CameraDataInfo>>();
        }

        public Action<CameraDataInfo, GrabInfo> GetDelegate(int id, Action<CameraDataInfo, GrabInfo> action)
        {
            if (_grabbedDictionary.ContainsKey(id))
                return _grabbedDictionary[id];

            _grabbedDictionary[id] = action;
            return action;
        }

        public Action<CameraDataInfo> GetDelegate(int id, Action<CameraDataInfo> action)
        {
            if (_donedDictionary.ContainsKey(id))
                return _donedDictionary[id];

            _donedDictionary[id] = action;
            return action;
        }
    }
}
