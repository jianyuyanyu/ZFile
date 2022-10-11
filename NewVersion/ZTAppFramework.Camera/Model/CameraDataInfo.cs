using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Camera.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:52:47 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public enum ECameraType
    {
        USB, GIGE
    }

    public enum ECameraManufacturer
    {
        Hik, Basler, iDS
    }


    public class CameraDataInfo : IEquatable<CameraDataInfo>
    {

        public int Index { get; set; }
        /// <summary>
        /// 制造商
        /// </summary>
        public ECameraManufacturer Manufacturer { get; set; }
        /// <summary>
        /// 相机类型
        /// </summary>
        public ECameraType CameraType { get; set; }
        public string ModelName { get; set; }
        public string SerialNo { get; set; }
        public string ChUserDefinedName { get; set; }
        public string ChDeviceVersion { get; set; }
        public string ChManufacturerSpecificInfo { get; set; }
        public string IPAddress { get; set; }
        public string CurrentSubNetMask { get; set; }
        public string DefultGateWay { get; set; }

        public CameraDataInfo()
        {

        }

        public CameraDataInfo(ECameraManufacturer manufacturer, ECameraType cameraType, string modelName, string serialNo)
        {
            Manufacturer = manufacturer;
            CameraType = cameraType;
            ModelName = modelName;
            SerialNo = serialNo;
        }

        public CameraDataInfo(ECameraManufacturer manufacturer, ECameraType cameraType, string modelName, string serialNo, string chUserDefinedName, string chDeviceVersion, string chManufacturerSpecificInfo, string IPAddress, string CurrentSubNetMask, string DefultGateWay) : this(manufacturer, cameraType, modelName, serialNo)
        {

            ChUserDefinedName = chUserDefinedName;
            ChDeviceVersion = chDeviceVersion;
            ChManufacturerSpecificInfo = chManufacturerSpecificInfo;
            this.IPAddress = IPAddress;
            this.CurrentSubNetMask = CurrentSubNetMask;
            this.DefultGateWay = DefultGateWay;
        }

        public CameraDataInfo(ECameraManufacturer manufacturer, ECameraType cameraType, string modelName, string serialNo, string chUserDefinedName, string chDeviceVersion) : this(manufacturer, cameraType, modelName, serialNo)
        {
        }

        public override int GetHashCode()
        {
            return Manufacturer.GetHashCode()
                ^ CameraType.GetHashCode()
                ^ ModelName.GetHashCode()
                ^ SerialNo.GetHashCode()
                ^ ChUserDefinedName.GetHashCode()
                ^ ChDeviceVersion.GetHashCode();
        }

        public bool Equals(CameraDataInfo other)
        {
            if (Manufacturer == other.Manufacturer
                && CameraType == other.CameraType
                && ModelName == other.ModelName
                && SerialNo == other.SerialNo
                && ChUserDefinedName == other.ChUserDefinedName
                && ChDeviceVersion == other.ChDeviceVersion
                )
                return true;

            return false;
        }
    }
}
