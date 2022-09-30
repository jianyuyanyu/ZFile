using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramework.Camera.Model
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  zt
    /// 创建时间    ：  2022/9/30 16:51:14 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public enum EGrabResult
    {
        Success, Error, Timeout, NotConnected
    }

    public struct GrabInfo
    {
        private EGrabResult _result;
        public EGrabResult Result
        {
            get
            {
                return _result;
            }
        }

        private int _width;
        public int Width
        {
            get
            {
                return _width;
            }
        }

        private int _height;
        public int Height
        {
            get
            {
                return _height;
            }
        }

        private int _channels;
        public int Channels
        {
            get
            {
                return _channels;
            }
        }

        private byte[] _data;
        public byte[] Data
        {
            get
            {
                return _data;
            }
        }

        public GrabInfo(EGrabResult result)
        {
            _result = result;
            _width = -1;
            _height = -1;
            _channels = -1;
            _data = null;
        }

        public GrabInfo(EGrabResult result, int width, int height, int channels, byte[] data)
        {
            _result = result;
            _width = width;
            _height = height;
            _channels = channels;
            _data = data;
        }
    }
}
