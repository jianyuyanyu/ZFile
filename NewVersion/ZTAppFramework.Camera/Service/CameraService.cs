using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Camera.Hiks;
using ZTAppFramework.Camera.Camera.interfaces;
using ZTAppFramework.Camera.Enums;
using ZTAppFramework.Camera.Model;
using static MvCamCtrl.NET.MyCamera;

namespace ZTAppFramework.Camera.Service
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:20:07 
    /// Description   ：  相机服务
    ///********************************************/
    /// </summary>
    public class CameraService
    {
        private static ICameraFactory _hikFactory = new HikCameraFactory();


        private bool _grabbing;
        private GrabInfo _grabInfo;
        private ICamera _camera;
        private CameraDataInfo _cameraInfo;

        public Action<CameraDataInfo, GrabInfo> ImageGrabbed { get; set; }
        public Action<CameraDataInfo> GrabDone { get; set; }
        public Action<CameraDataInfo> GrabStarted { get; set; }

        /// <summary>
        /// 参数发生改变时
        /// </summary>
        public Action<CameraParameterInfo> ParameterChanged { get; set; }
        /// <summary>
        /// 获取设备信息
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<CameraDataInfo> GetDeviceInfos()
        {
            var Infos = new List<CameraDataInfo>();
            try
            {
                Infos.AddRange(_hikFactory.GetDevices());
            }
            catch (Exception ex)
            {

            }

            return Infos;
        }

        public bool IsConnected()
        {
            if (_camera != null)
                return _camera.IsConnected();

            return false;
        }

        public async Task<GrabInfo?> Grab()
        {
            try
            {
                if (_camera == null || _grabbing)
                    return null;

                _grabbing = true;
                if (_camera.StartGrab(1) == false)
                {
                    Stop();
                    return null;
                }

                while (_grabbing)
                    await Task.Delay(1);

                return _grabInfo;
            }
            catch (Exception e)
            {
                _grabbing = false;
                //_logger.Error(e);
                return null;
            }
        }

        public bool StartGrab(int grabCount = -1)
        {
            if (_camera != null)
                return _camera.StartGrab(grabCount);

            return false;
        }

        public bool Stop()
        {
            _grabbing = false;

            if (_camera != null)
                return _camera.Stop();

            return false;
        }

        public bool SetParameter(ECameraParameter parameter, double value)
        {
            if (_camera != null)
            {
                if (_camera.SetParameter(parameter, value))
                {
                    if (ParameterChanged != null)
                        ParameterChanged(GetParameterInfo());

                    return true;
                }
            }

            return false;
        }

        public bool SetTriggerMode(bool isTriggerMode)
        {
            if (_camera != null)
            {
                if (_camera.SetTriggerMode(isTriggerMode))
                {
                    if (ParameterChanged != null)
                        ParameterChanged(GetParameterInfo());

                    return true;
                }
            }


            return false;
        }

        public bool SetPixelFormat(MvGvspPixelType type)
        {
            if (_camera != null)
            {
                if (_camera.SetPixelFormat(type))
                {
                    if (ParameterChanged != null)
                        ParameterChanged(GetParameterInfo());

                    return true;
                }
            }


            return false;
        }

        public bool SetAuto(ECameraAutoType type, ECameraAutoValue value)
        {
            if (_camera != null)
            {
                if (_camera.SetAuto(type, value))
                {
                    if (ParameterChanged != null)
                        ParameterChanged(GetParameterInfo());

                    return true;
                }
            }

            return false;
        }

        public bool Connect(CameraDataInfo info)
        {
            Disconnect();

            switch (info.Manufacturer)
            {
                case ECameraManufacturer.Hik:
                    if (_hikFactory.IsExist(info) == false)
                        return false;
                    _camera = _hikFactory.Connect(info);
                    break;
                case ECameraManufacturer.Basler:
                    break;
                case ECameraManufacturer.iDS:
                    break;
                default:
                    break;
            }

            if (_camera == null)
                return false;

            _cameraInfo = info;

            _camera.ImageGrabbed = Grabbed;
            _camera.GrabDone = Done;
            _camera.GrabStarted = Started;
            return true;
        }

        public CameraParameterInfo GetParameterInfo()
        {
            if (_camera != null && _camera.IsConnected())
                return _camera.GetParameterInfo();

            return null;
        }
        public void Disconnect()
        {
            if (_camera != null)
            {
                _camera.Disconnect();
                _camera = null;

                _cameraInfo = null;
                //_logger.Info("Disconnect");
            }
        }

        private void Grabbed(GrabInfo grabInfo)
        {
            if (_grabbing)
            {
                _grabInfo = grabInfo;
                _grabbing = false;
            }

            if (ImageGrabbed != null)
                ImageGrabbed(_cameraInfo, grabInfo);
        }

        private void Done()
        {
            if (GrabDone != null)
                GrabDone(_cameraInfo);
        }

        private void Started()
        {
            if (GrabStarted != null)
                GrabStarted(_cameraInfo);
        }

    }
}
