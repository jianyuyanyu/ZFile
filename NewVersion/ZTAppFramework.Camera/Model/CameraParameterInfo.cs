using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Camera.Enums;

namespace ZTAppFramework.Camera.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/30 16:54:00 
    /// Description   ：  HIK相机参数
    ///********************************************/
    /// </summary>
    public class CameraParameterInfo
    {
        public bool OnTriggerMode { get; set; }

        private IDictionary<ECameraParameter, CameraParameter> _parameters;
        public IDictionary<ECameraParameter, CameraParameter> Parameters
        {
            get
            {
                return _parameters;
            }
        }

        private IDictionary<ECameraAutoType, ECameraAutoValue> _autoValues;
        public IDictionary<ECameraAutoType, ECameraAutoValue> AutoValues
        {
            get
            {
                return _autoValues;
            }
        }

        public CameraParameterInfo(
        bool onTriggerMode,
            IDictionary<ECameraParameter, CameraParameter> parameters,
            IDictionary<ECameraAutoType, ECameraAutoValue> autoValues)
        {
            OnTriggerMode = onTriggerMode;
            _parameters = parameters;
            _autoValues = autoValues;
        }
    }


    public class CameraParameter
    {
        public double Current { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public CameraParameter(double current, double min, double max)
        {
            Current = current;
            Min = min;
            Max = max;
        }
    }
}
