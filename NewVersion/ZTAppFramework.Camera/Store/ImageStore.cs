using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using ZTAppFramework.Camera.Extensions;
using ZTAppFramework.Camera.Model;
using ZTAppFramework.Camera.Service;

namespace ZTAppFramework.Camera.Store
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/10/8 9:25:15 
    /// Description   ：  图片存储
    ///********************************************/
    /// </summary>
    public class ImageStore
    {

        private Dictionary<int, BitmapSource> _sourceDictionary;
        private Dictionary<int, GrabInfo> _grabInfoDictionary;
        private Dictionary<int, Mat> _matDictionary;

        private Dictionary<int, Action<BitmapSource>> _sourceCreated;

        private CameraStore _cameraStore;

        public ImageStore(CameraServiceMediator mediator,
            CameraStore cameraStore)
        {
            _cameraStore = cameraStore;

            _sourceDictionary = new Dictionary<int, BitmapSource>();
            _grabInfoDictionary = new Dictionary<int, GrabInfo>();
            _matDictionary = new Dictionary<int, Mat>();

            _sourceCreated = new Dictionary<int, Action<BitmapSource>>();

            mediator.ImageGrabbed += ImageGrabbed;
            mediator.GrabDone += GrabDone;

        }
        private void GrabDone(int id)
        {
            //if (_cameraStore.Cameras.Any(c => c.Info == cameraInfo) == false)
            //    return;

            //var id = _cameraStore.Cameras.First(c => c.Info == cameraInfo).ID;

            //_sourceDictionary[id] = grabInfo.ToImageSource();
        }

        private void ImageGrabbed(int id, GrabInfo grabInfo)
        {
            _grabInfoDictionary[id] = grabInfo;
            _sourceDictionary[id] = grabInfo.ToImageSource();
            _matDictionary[id] = grabInfo.ToMat();

            if (_sourceCreated.ContainsKey(id))
                _sourceCreated[id]?.Invoke(_sourceDictionary[id]);
        }


        public void Subscribe(int id, Action<BitmapSource> action)
        {
            if (_sourceCreated.ContainsKey(id))
                _sourceCreated[id] += action;
            else
                _sourceCreated.Add(id, action);
        }

        public void Unsubscribe(int id, Action<BitmapSource> action)
        {
            if (_sourceCreated.ContainsKey(id))
                _sourceCreated[id] -= action;
        }

        public BitmapSource GetSouce(int id)
        {
            if (_sourceDictionary.ContainsKey(id))
                return _sourceDictionary[id];

            return null;
        }

        public GrabInfo GetGrabInfo(int id)
        {
            if (_grabInfoDictionary.ContainsKey(id))
                return _grabInfoDictionary[id];

            return new GrabInfo();
        }

        public Mat GetMat(int id)
        {
            if (_matDictionary.ContainsKey(id))
                return _matDictionary[id];

            return null;
        }

    }
}
