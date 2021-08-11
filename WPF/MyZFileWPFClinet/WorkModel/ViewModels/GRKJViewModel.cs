using Component;
using Component.Common.Helpers;
using Component.Dto;
using Component.ViewModelBase;
using Microsoft.Win32;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace WorkModel.ViewModels
{
    public class GRKJViewModel : BaseViewModel
    {
        private WorkService service;

        private DownLoadHelper downLoadHelper;

        private ObservableCollection<FolderModel> _UserFolderItem;
        public ObservableCollection<FolderModel> UserFolderItem
        {
            get { return _UserFolderItem; }
            set { SetProperty(ref _UserFolderItem, value); }
        }

        private ObservableCollection<FolderModel> _NavTabFileInfo;

        public ObservableCollection<FolderModel> NavTabFileInfo
        {
            get { return _NavTabFileInfo; }
            set
            {
                _NavTabFileInfo = value;
            }
        }

        private FolderModel _SelectItem;
        public FolderModel SelectItem
        {
            get { return _SelectItem; }
            set { SetProperty(ref _SelectItem, value); }
        }

        public GRKJViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            UserFolderItem = new ObservableCollection<FolderModel>();
            NavTabFileInfo = new ObservableCollection<FolderModel>() { new FolderModel() { Id = 2, Name = "我的网盘>" } };
            service = provider.Resolve<WorkService>();
            downLoadHelper = provider.Resolve<DownLoadHelper>();

        }
        public DelegateCommand LoadedCommand => new DelegateCommand(LoadMenu);
        public DelegateCommand AddFileInfoCommand => new DelegateCommand(AddFileInfo);
        public DelegateCommand<FolderModel> OpenFloderCommand => new DelegateCommand<FolderModel>((o) => OpenFloder(o));
        public DelegateCommand DeleteItemCommand => new DelegateCommand(DeleteItem);

        private async void DeleteItem()
        {
            if (SelectItem == null)
            {
                MessageBox.Show("请选择一个文件");
                return;
            }
            List<DelFile> items = new List<DelFile>();
            DelFile model = new DelFile() { Id = SelectItem.Id, Type = SelectItem.Type, };
            items.Add(model);
            var ResqustData = await service.DelFileRequst(items);
            if (ResqustData.statusCode != 200)
            {
                MessageBox.Show("删除异常");
                return;
            }
            MessageBox.Show("删除成功");
            await GetFloderInfo();
        }

        private async void OpenFloder(FolderModel o)
        {


            if (o.Type == 1)
                DownFile(o);
            else
            {
                if (NavTabFileInfo.Contains(o))
                {
                    int Index = NavTabFileInfo.IndexOf(o);
                    for (int i = Index; i < NavTabFileInfo.Count; i++)
                    {
                        if (Index + 1 < NavTabFileInfo.Count)
                        {
                            Index = Index + 1;
                            NavTabFileInfo.Remove(NavTabFileInfo[Index]);
                        }
                    }
                }
                else
                {
                    NavTabFileInfo.Add(o);
                }
                await GetFloderInfo(o.Id);
            }

        }

        async void DownFile(FolderModel o)
        {
            var ApiResData = await service.GetDownFileInfo(o.Id);
            if (ApiResData.statusCode != 200) return;
            string Id = ApiResData.data.FileData;
            int Size = int.Parse(ApiResData.data.FileSize);
            int Count = ApiResData.data.FileCount;
            var model = new DownLoadInfo() { Id = Id, Size = Size, SumCount = Count, format = o.format, Name = o.Name };
            downLoadHelper.DownloadPatch(model);
        }

        private async void AddFileInfo()
        {
            //获取用户文件权限
            var model = await service.CheckAuth();
            if (model.statusCode != 200) return;
            OpenFileDialog opdialog = new OpenFileDialog();
            opdialog.Multiselect = false;
            Nullable<bool> result = opdialog.ShowDialog();
            if (result == true)
            {
                await downLoadHelper.ExcuVerify(opdialog.SafeFileName, opdialog.FileNames[0]);
                await GetFloderInfo();
            }
        }

        private async void LoadMenu()
        {
            await GetFloderInfo();
        }
        async Task GetFloderInfo(int FloderID = 2, int Id = 2)
        {
            UserFolderItem.Clear();
            var model = await service.GetFolderInfo(Id, FloderID);
            if (model != null)
            {
                model.data.FileInfo.ForEach(o => UserFolderItem.Add(new FolderModel() { CRTime = o.CRDate.Value, format = o.FileExtendName, Size = ByteConvert.GetSize(Convert.ToInt64(o.FileSize)), Id = o.ID, Name = o.Name, Type = 1 }));
                model.data.FolderInfo.ForEach(o => UserFolderItem.Add(new FolderModel() { CRTime = o.CRDate.Value, format = "文件", Size = "", Id = o.ID, Name = o.Name, Type = 2 }));
            }
        }
    }
}
