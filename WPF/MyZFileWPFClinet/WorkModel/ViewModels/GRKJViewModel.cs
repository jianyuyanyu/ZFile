using Component;
using Component.Common;
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
using System.Windows.Media;
using System.Windows.Threading;
using WorkModel.WindosDlg;
using ZFileComponent.Themes.ControlHelper;

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
            set { SetProperty(ref _NavTabFileInfo, value); }
        }

        private FolderModel _SelectItem;
        public FolderModel SelectItem
        {
            get { return _SelectItem; }
            set
            {

                SetProperty(ref _SelectItem, value);

            }
        }

        public List<FolderModel> SelectCheckItems { get; set; }


        IContainerProvider _provider;
        IRegionManager _regionManager;
        public GRKJViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            SelectCheckItems = new List<FolderModel>();
            UserFolderItem = new ObservableCollection<FolderModel>();
            NavTabFileInfo = new ObservableCollection<FolderModel>() { new FolderModel() { Id = 2, Name = "我的网盘" } };
            service = provider.Resolve<WorkService>();
            downLoadHelper = provider.Resolve<DownLoadHelper>();
            _provider = provider;
            _regionManager = regionManager;
            CurrnetFolder = new FolderModel();
            LoadMenu();



        }
        //public DelegateCommand LoadedCommand => new DelegateCommand(LoadMenu);
        public DelegateCommand AddFileInfoCommand => new DelegateCommand(AddFileInfo);
        public DelegateCommand<FolderModel> OpenFloderCommand => new DelegateCommand<FolderModel>((o) => OpenFloder(o));
        public DelegateCommand DeleteItemCommand => new DelegateCommand(DeleteItem);

        public DelegateCommand AddFolderCommand => new DelegateCommand(AddFolder);

        public DelegateCommand PASTEITEMCommand => new DelegateCommand(PASTEITEM);

        public DelegateCommand<FolderModel> CheckCommand => new DelegateCommand<FolderModel>(Check);

        public DelegateCommand<string> SelectMenuCommand => new DelegateCommand<string>(SelectMenu);
        private void SelectMenu(string obj)
        {

        }

        private void Check(FolderModel obj)
        {
            if (obj.IsCheck)
                SelectCheckItems.Add(obj);
            else
                SelectCheckItems.Remove(obj);
        }

        private void AddFolder()
        {
            var content = new AddFloder(_provider, CurrnetFolder);
            DialogBox.Show(SystemResource.Nav_MainContent, content, "创建文件夹", null, DialogClose);
            
        }



        private void PASTEITEM()
        {
            if (SelectCheckItems.Count == 0) return;
            PasteitemsDto dto = new PasteitemsDto();
            SelectCheckItems.ForEach(item => dto.Child.Add(new PasteitemsChild() { itemId = item.Id, ItemType = item.Type }));
            var content = new PasteItem(_provider, dto);
            DialogBox.Show(SystemResource.Nav_MainContent, content, "文件迁移", null, DialogClose);
        }

        private async void DialogClose(DialogBox arg1, object arg2)
        {
            if (arg2 == null) return;
            MessageBox.Show(arg2.ToString());
            await GetFloderInfo(CurrnetFolder.Id);
        }

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


        FolderModel CurrnetFolder;
        private async void OpenFloder(FolderModel o)
        {
            if (o == null) return;
            if (o.Type == 1)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FilterIndex = 1;
                sfd.RestoreDirectory = true;
                sfd.FileName = o.Name + "." + o.format;
                Nullable<bool> result = sfd.ShowDialog();
                if (result == true)
                {
                    DownFile(o, sfd.FileName);
                }
                else
                {
                    return;
                }

            }
            else
            {
                if (CurrnetFolder.Id == o.Id) return;

                int index = NavTabFileInfo.IndexOf(o);
                if (index >= 0)
                {
                    int count = NavTabFileInfo.Count - (index + 1);
                    while (count != 0)
                    {
                        NavTabFileInfo.Remove(NavTabFileInfo[NavTabFileInfo.Count - 1]);
                        count--;
                    }
                }
                else
                {
                    NavTabFileInfo.Add(o);
                }
                CurrnetFolder.Remark = o.Remark;
                await GetFloderInfo(o.Id);
            }

        }

        async void DownFile(FolderModel o, string SaveFile)
        {
            var ApiResData = await service.GetDownFileInfo(o.Id);
            if (ApiResData.statusCode != 200) return;
            string Id = ApiResData.data.FileData;
            int Size = int.Parse(ApiResData.data.FileSize);
            int Count = ApiResData.data.FileCount;
            var model = new DownLoadInfo() { Id = Id, Size = Size, SumCount = Count, format = o.format, Name = o.Name, SaveFile = SaveFile };
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
            CurrnetFolder.Type = 2;
            CurrnetFolder.Id = 2;
            CurrnetFolder.Remark = "2";
        }

        async Task GetFloderInfo(int FloderID = 2, int Id = 2)
        {
            UserFolderItem.Clear();
            var model = await service.GetSpaceInfo(Id, FloderID);
            if (model != null)
            {
                CurrnetFolder.Id = FloderID;
                model.data.FileInfo.ForEach(o => UserFolderItem.Add(new FolderModel() { CRTime = o.CRDate.Value, format = o.FileExtendName, Size = ByteConvert.GetSize(Convert.ToInt64(o.FileSize)), Id = o.ID, Name = o.Name, Type = 1 }));
                model.data.FolderInfo.ForEach(o => UserFolderItem.Add(new FolderModel() { CRTime = o.CRDate.Value, format = "文件", Size = "", Id = o.ID, Name = o.Name, Type = 2, Remark = o.Remark }));
            }
        }
    }
}
