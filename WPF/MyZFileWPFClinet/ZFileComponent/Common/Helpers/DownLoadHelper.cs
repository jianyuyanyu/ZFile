using Component.Common.Service;
using Component.ViewModelBase;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using Component.Dto;
using System.Windows.Threading;
using Component.Api;
using ZFileComponent.Internal;

namespace Component.Common.Helpers
{
    public class DownLoadHelper : BaseViewModel
    {
        ObservableCollection<DownLoadInfo> CurrentDownFileitem { get; set; } = new ObservableCollection<DownLoadInfo>();
        ObservableCollection<UploadInfo> CurrentUploadItem { get; set; } = new ObservableCollection<UploadInfo>();

        DownService service;

        public DownLoadHelper(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {

            service = provider.Resolve<DownService>();
            LoadInfo();

        }
        public Action<double, int> UpdateSunProgreesAct;
        double SunSize;
        int SunProgressValues;
        string DownInfoFath = AppDomain.CurrentDomain.BaseDirectory + "SystemConfig";
        public ObservableCollection<DownLoadInfo> GetAllDownInfo() => CurrentDownFileitem;
        public ObservableCollection<UploadInfo> GetAllUploadInfo() => CurrentUploadItem;
        public bool GetFileInfo(string Id) => CurrentDownFileitem.Any(o => o.Id == Id);
        public void LoadInfo()
        {
            if (!Directory.Exists(DownInfoFath)) Directory.CreateDirectory(DownInfoFath);
            if (File.Exists(DownInfoFath + "\\SYSTEM.xml"))
            {
                var model = XmlHelper.SerializerXMLToObject<ObservableCollection<DownLoadInfo>>(DownInfoFath + "\\SYSTEM.xml");
                foreach (var item in model)
                {
                    item.state = DownState.Suspend;
                    DownloadPatch(item);
                }
            }
        }
        void DownSave()
        {
            if (!Directory.Exists(DownInfoFath)) Directory.CreateDirectory(DownInfoFath);
            XmlHelper.SerializeToXmlFile(CurrentDownFileitem, DownInfoFath + "\\SYSTEM.xml");
        }
        void UploadSave()
        {
            if (!Directory.Exists(DownInfoFath)) Directory.CreateDirectory(DownInfoFath);
            XmlHelper.SerializeToXmlFile(CurrentUploadItem, DownInfoFath + "\\UploadSYSTEM.xml");
        }
        void downDisk(DownSplitDto data, DownLoadInfo info)
        {
            var buffer = new byte[data.data.Length];
            MemoryStream stream = new MemoryStream(data.data);
            stream.Read(buffer, 0, buffer.Length);
            var md5 = MD5Helper.GetMd5(buffer);
            var filePath = string.Format("{0}\\{1}\\{2:D10}.{3}", "", info.Id, data.Index, md5);
            string Path2 = AppDomain.CurrentDomain.BaseDirectory;
            using (var fileStream = new FileStream(Path2 + filePath, FileMode.Create, FileAccess.Write))
            {
                stream.Seek(0, SeekOrigin.Begin);
                stream.CopyTo(fileStream);
                fileStream.Close();
            }

            UpdateSunProgress();
            TimeSpan time = TimeSpan.Zero;
            info.CurrentDownSpeed = ByteConvert.GetSpeed((data.Index - 1) * 1000 * 1024, data.Index * 1000 * 1024, (int)info.Size, ref time);
            info.RemainingTime = time;
            info.ProgressValues = info.CurrnetCount * 1000 * 1024;
        }

        void UpdateSunProgress()
        {
            int SUMcOUNT = 0;
            foreach (var item in CurrentDownFileitem)
            {
                SUMcOUNT += item.CurrnetCount;
            }
            SunProgressValues = SUMcOUNT * 1000 * 1024;
            UpdateSunProgreesAct?.Invoke(SunSize, SunProgressValues);
            DownSave();
        }
        public object locks = new object();
        public async void DownloadPatch(DownLoadInfo info)
        {
            if (CurrentDownFileitem.Any(o => o.Id == info.Id)) return;
            SunSize += info.Size;
            CurrentDownFileitem.Add(info);
            string rootPath2 = AppDomain.CurrentDomain.BaseDirectory + info.Id;
            if (Directory.Exists(rootPath2))
            {
                string[] files = Directory.GetFiles(rootPath2);
                int num = 0;
                foreach (var item in files)
                {
                    num = Convert.ToInt32(item.Substring(item.LastIndexOf("\\") + 1, (item.LastIndexOf(".") - item.LastIndexOf("\\") - 1)));
                }
                info.CurrnetCount = num;
            }
            else
            {
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + info.Id);
            }

            List<Task> tasks = new List<Task>();

            while (info.CurrnetCount < info.SumCount)
            {
                switch (info.state)
                {
                    case DownState.Start:
                        var item = await service.DownloadFile(info.Id, info.CurrnetCount);
                        if (item.statusCode != 200) continue;
                        info.CurrnetCount++;
                        tasks.Add(Task.Run(()=> downDisk(item.data,info)));
                        break;
                    case DownState.Suspend:
                        await Task.Delay(100);
                        break;
                    case DownState.Delete:
                        Directory.Delete(rootPath2, true);
                        SunSize = SunSize - info.Size;
                        SunProgressValues = SunProgressValues - (int)info.Size;
                        UpdateSunProgreesAct?.Invoke(SunSize, SunProgressValues);
                        return;
                    default:
                        break;
                }
            }
            await Task.WhenAll(tasks.ToArray());

            var dir = Path.GetDirectoryName(rootPath2);
            string[] chunkFiles = Directory.GetFiles(rootPath2);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            if (File.Exists(info.Name + "." + info.format))
                File.Delete(info.Name + "." + info.format);

            using (Stream stream = new FileStream(info.SaveFile, FileMode.Create, FileAccess.Write))
            {
                foreach (string chunkFile in chunkFiles)
                {
                    using (Stream input = File.OpenRead(chunkFile))
                    {
                        input.CopyTo(stream);
                    }
                }
            }
            Directory.Delete(rootPath2, true);
            CurrentDownFileitem.Remove(info);
            SunSize = SunSize - info.Size;
            SunProgressValues = SunProgressValues - (int)info.Size;
        
            UpdateSunProgreesAct?.Invoke(SunSize, SunProgressValues);
            UpdateSunProgress();
            Notice.Show("下载成功", info.Name, 8, MessageBoxIcon.Success);
            info = null;
            //MessageBox.Show("下载成功");
        }
        public async Task ExcuVerify(string safeFileName, string FilePATH)
        {
            try
            {
                long Size;
                string Name, Type, md5;
                Name = safeFileName;
                string[] TempExtra = Name.Split('.');
                Type = TempExtra[TempExtra.Length - 1];
                int iFileSize = 0;
                iFileSize = 1000 * 1024;
                //开始分片
                FileStream fst = new FileStream(FilePATH, FileMode.Open);
                int iFileCount = (int)(fst.Length / iFileSize);
                if (fst.Length % iFileSize != 0) iFileCount++;
                Size = fst.Length;
                //创建MD5
                md5 = MD5Helper.CreateFileMd5(fst);
                fst.Dispose();
                fst.Close();
                //验证服务端是否存在此文件分片
                var CheckwholeMod = await service.CheckwholeFile(md5);
                if (CheckwholeMod.statusCode != 200) return;
                if (CheckwholeMod.success)
                {
                    MessageBox.Show("服务端文件已经存在");
                    return;
                }
                int ChunkCount = CheckwholeMod.data.chunkMd5s.Count;
                var item = new UploadInfo() { Name = Name, UoloadPath = FilePATH, format = Type, Size = Size, CurrnetCount = ChunkCount, state = DownState.Start, SumCount = iFileCount, md5 = md5 };
                CurrentUploadItem.Add(item);
                await UploadPatch(FilePATH, md5, Name, ChunkCount, iFileSize, iFileCount, Type, Size);
            }
            catch (Exception)
            {
                MessageBox.Show("文件不存在");
            }

        }
        public async Task UploadPatch(string FilePath, string md5, string Name, int ChunkCount, int iFileSize, int iFileCount, string Type, long Size)
        {
            var item = CurrentUploadItem.Where(o => o.md5 == md5).FirstOrDefault();
            if (item == null)
            {

                MessageBox.Show("文件不存在");
                return;
            }
            FileStream SplitFileStream = new FileStream(FilePath, FileMode.Open);
            SplitFileStream.Seek(ChunkCount * iFileSize, SeekOrigin.Begin);
            //以FileStream文件流来初始化BinaryReader文件阅读器
            BinaryReader SplitFileReader = new BinaryReader(SplitFileStream);

            while (ChunkCount< iFileCount)
            {
                switch (item.state)
                {
                    case DownState.Start:
                        var models = await service.UploadServer(
                      new fileuploadDto()
                      {
                          name = Name,
                          Filetype = Type,
                          chunk = ChunkCount,
                          size = Size,
                          fileMd5 = md5,
                          chunks = iFileCount,
                          spacecode = "qycode",
                          upinfo = SplitFileReader.ReadBytes(iFileSize)
                      });
                        if (models.statusCode != 200)
                        {
                            SplitFileReader.Close();
                            SplitFileStream.Close();
                            MessageBox.Show("异常");
                            return;
                        }
                        ChunkCount++;
                        item.CurrnetCount = ChunkCount;
                        UploadSave();
                        TimeSpan time = TimeSpan.Zero;
                        item.CurrentDownSpeed = ByteConvert.GetSpeed((item.CurrnetCount - 1) * 1000 * 1024, item.CurrnetCount * 1000 * 1024, (int)item.Size, ref time);
                        item.RemainingTime = time;
                        item.ProgressValues = item.CurrnetCount * 1000 * 1024;
                        break;
                    case DownState.Suspend:
                        await Task.Delay(100);
                        break;
                    case DownState.Delete:
                        break;
                    case DownState.None:
                        break;
                    default:
                        break;
                }
            }

            SplitFileReader.Close();
            SplitFileStream.Close();
            var model = await service.UploadfileMergeServer(new fileuploadDto()
            {
                name = Name,
                Filetype = Type,
                chunk = ChunkCount++,
                size = Size,
                fileMd5 = md5,
                chunks = iFileCount,
                spacecode = "qycode",
                upinfo = null
            });
            if (model.statusCode != 200)
            {
                MessageBox.Show("异常");
                return;
            }
            string Zid = model.data.ToString();
            List<UploadSuccessDto> dtos = new List<UploadSuccessDto>();
            dtos.Add(new UploadSuccessDto()
            {
                filename = Name,
                md5 = md5,
                filesize = Size,
                zyid = Zid,
                FolderID = 2
            });

            model = await service.UploadfileSuccessServer(dtos);
            if (model.statusCode != 200) return;
            CurrentUploadItem.Remove(item);
            UploadSave();
            Notice.Show("上传成功", Name, 8, MessageBoxIcon.Success);
       
        }
    }
    public class DownLoadInfo : BindableBase
    {
        public DownLoadInfo()
        {
            CurrnetCount = 0;
            ProgressValues = 0;
            state = DownState.Start;
        }
        private int _CurrnetCount;
        public int CurrnetCount
        {
            get { return _CurrnetCount; }
            set { SetProperty(ref _CurrnetCount, value); }
        }
        public string Id { get; set; }

        public string Name { get; set; }

        public string format { get; set; }
        public long Size { get; set; }

        public int SumCount { get; set; }

        private DownState _state;
        public DownState state
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private int _ProgressValues;
        public int ProgressValues
        {
            get { return _ProgressValues; }
            set { SetProperty(ref _ProgressValues, value); }
        }

        private string _CurrentDownSpeed;
        public string CurrentDownSpeed
        {
            get { return _CurrentDownSpeed; }
            set { SetProperty(ref _CurrentDownSpeed, value); }
        }

        private TimeSpan _RemainingTime;
        public TimeSpan RemainingTime
        {
            get { return _RemainingTime; }
            set { SetProperty(ref _RemainingTime, value); }
        }

        public string SaveFile { get; set; }
    }
    public class UploadInfo : BindableBase
    {
        public UploadInfo()
        {
            CurrnetCount = 0;
            ProgressValues = 0;
            state = DownState.Start;
        }
        public string UoloadPath { get; set; }
        private int _CurrnetCount;
        public int CurrnetCount
        {
            get { return _CurrnetCount; }
            set { SetProperty(ref _CurrnetCount, value); }
        }
        public string md5 { get; set; }

        public string Name { get; set; }

        public string format { get; set; }
        public long Size { get; set; }

        public int SumCount { get; set; }

        private DownState _state;
        public DownState state
        {
            get { return _state; }
            set { SetProperty(ref _state, value); }
        }

        private int _ProgressValues;
        public int ProgressValues
        {
            get { return _ProgressValues; }
            set { SetProperty(ref _ProgressValues, value); }
        }

        private string _CurrentDownSpeed;
        public string CurrentDownSpeed
        {
            get { return _CurrentDownSpeed; }
            set { SetProperty(ref _CurrentDownSpeed, value); }
        }

        private TimeSpan _RemainingTime;
        public TimeSpan RemainingTime
        {
            get { return _RemainingTime; }
            set { SetProperty(ref _RemainingTime, value); }
        }
    }
    public class DownSplitDto
    {
        public byte[] data { get; set; }
        public int Index { get; set; }
    }
    public class CheckWholeDto
    {
        public CheckWholeDto()
        {
            chunkMd5s = new List<string>();
        }
        public List<string> chunkMd5s { get; set; }
    }
    public enum DownState
    {
        Start,
        Suspend,
        Delete,
        None
    }

}
