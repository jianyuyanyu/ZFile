using MvCamCtrl.NET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Camera.interfaces;
using ZTAppFramework.Camera.Model;

namespace ZTAppFramework.Camera.Camera.Hiks
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:56:46 
    /// Description   ：  海康相机工厂
    ///********************************************/
    /// </summary>
    public class HikCameraFactory : ICameraFactory
    {
        public IEnumerable<CameraDataInfo> GetDevices()
        {
            var cameraInfos = new List<CameraDataInfo>();

            var stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();

            if (MyCamera.MV_OK != MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDevList))
                return cameraInfos;

            for (Int32 i = 0; i < stDevList.nDeviceNum; i++)
            {
                var stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

                if (MyCamera.MV_GIGE_DEVICE == stDevInfo.nTLayerType)
                {
                    var stGigEDeviceInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));
                    if (stGigEDeviceInfo.chManufacturerName == "Hikrobot")
                        cameraInfos.Add(new CameraDataInfo(
                            ECameraManufacturer.Hik,
                            ECameraType.GIGE,
                            stGigEDeviceInfo.chModelName,
                            stGigEDeviceInfo.chSerialNumber,
                            stGigEDeviceInfo.chUserDefinedName,
                            stGigEDeviceInfo.chDeviceVersion,
                            stGigEDeviceInfo.chManufacturerSpecificInfo,
                            GetIPStrFromUint(stGigEDeviceInfo.nCurrentIp),
                            GetIPStrFromUint(stGigEDeviceInfo.nCurrentSubNetMask),
                            GetIPStrFromUint(stGigEDeviceInfo.nDefultGateWay)
                            ));
                }
                else if (MyCamera.MV_USB_DEVICE == stDevInfo.nTLayerType)
                {
                    var stUsb3DeviceInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));
                    if (stUsb3DeviceInfo.chManufacturerName == "Hikvision")
                        cameraInfos.Add(new CameraDataInfo(
                            ECameraManufacturer.Hik,
                            ECameraType.USB,
                            stUsb3DeviceInfo.chModelName,
                            stUsb3DeviceInfo.chSerialNumber,
                            stUsb3DeviceInfo.chUserDefinedName,
                            stUsb3DeviceInfo.chDeviceVersion


                            ));
                }
            }

            return cameraInfos;
        }

        public ICamera Connect(CameraDataInfo cameraInfo)
        {
            if (IsExist(cameraInfo) == false)
                return null;

            MyCamera.MV_CC_DEVICE_INFO stDevInfo;
            if (GetHikDeviceInfo(cameraInfo, out stDevInfo) == false)
                return null;

            var device = new MyCamera();

            if (MyCamera.MV_OK != device.MV_CC_CreateDevice_NET(ref stDevInfo))
                return null;

            if (MyCamera.MV_OK != device.MV_CC_OpenDevice_NET())
                return null;

            if (cameraInfo.CameraType == ECameraType.GIGE)
            {
                int nPacketSize = device.MV_CC_GetOptimalPacketSize_NET();
                if (nPacketSize > 0)
                {
                    if (MyCamera.MV_OK != device.MV_CC_SetIntValue_NET("GevSCPSPacketSize", (uint)nPacketSize))
                        return null;
                }
                else
                {
                    return null;
                }
            }

            var info = new MyCamera.MV_IMAGE_BASIC_INFO();

            if (MyCamera.MV_OK != device.MV_CC_GetImageInfo_NET(ref info))
                return null;

            var camera = new HikCamera(device, info);

            if (MyCamera.MV_OK != device.MV_CC_RegisterImageCallBackEx_NET(HikCamera.ImageCallback, GCHandle.ToIntPtr(camera.Handle)))
                return null;

            return camera;
        }

        public bool IsExist(CameraDataInfo cameraInfo)
        {
            if (cameraInfo.Manufacturer != ECameraManufacturer.Hik)
                return false;
            MyCamera.MV_CC_DEVICE_INFO stDevInfo;
            return GetHikDeviceInfo(cameraInfo, out stDevInfo);
        }

        private static bool GetHikDeviceInfo(CameraDataInfo cameraInfo, out MyCamera.MV_CC_DEVICE_INFO stDevInfo)
        {
            stDevInfo = new MyCamera.MV_CC_DEVICE_INFO();

            var stDevList = new MyCamera.MV_CC_DEVICE_INFO_LIST();
            var nRet = MyCamera.MV_CC_EnumDevices_NET(MyCamera.MV_GIGE_DEVICE | MyCamera.MV_USB_DEVICE, ref stDevList);
            if (MyCamera.MV_OK != nRet)
                return false;

            for (Int32 i = 0; i < stDevList.nDeviceNum; i++)
            {
                stDevInfo = (MyCamera.MV_CC_DEVICE_INFO)Marshal.PtrToStructure(stDevList.pDeviceInfo[i], typeof(MyCamera.MV_CC_DEVICE_INFO));

                switch (cameraInfo.CameraType)
                {
                    case ECameraType.USB:
                        if (MyCamera.MV_USB_DEVICE == stDevInfo.nTLayerType)
                        {
                            var stUsb3DeviceInfo = (MyCamera.MV_USB3_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stUsb3VInfo, typeof(MyCamera.MV_USB3_DEVICE_INFO));

                            if (cameraInfo.ModelName == stUsb3DeviceInfo.chModelName
                                && cameraInfo.SerialNo == stUsb3DeviceInfo.chSerialNumber)
                            {
                                return true;
                            }
                        }
                        break;
                    case ECameraType.GIGE:
                        if (MyCamera.MV_GIGE_DEVICE == stDevInfo.nTLayerType)
                        {
                            var stGigEDeviceInfo = (MyCamera.MV_GIGE_DEVICE_INFO)MyCamera.ByteToStruct(stDevInfo.SpecialInfo.stGigEInfo, typeof(MyCamera.MV_GIGE_DEVICE_INFO));

                            if (cameraInfo.ModelName == stGigEDeviceInfo.chModelName
                                && cameraInfo.SerialNo == stGigEDeviceInfo.chSerialNumber)
                            {
                                return true;
                            }
                        }
                        break;
                }
            }

            return false;
        }

        private static string GetIPStrFromUint(uint ip)
        {
            byte first = (byte)(ip >> 24);
            byte second = (byte)((ip - (first << 24)) >> 16);
            byte third = (byte)((ip - (second << 16) - (first << 24)) >> 8);
            byte four = (byte)(ip - (second << 16) - (first << 24) - (third << 8));
            return String.Format("{0}.{1}.{2}.{3}", first, second, third, four);
        }
    }
}
