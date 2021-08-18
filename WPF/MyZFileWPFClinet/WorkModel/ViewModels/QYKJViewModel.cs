using Component.Common.Helpers;
using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace WorkModel.ViewModels
{
    public class QYKJViewModel : BaseViewModel
    {
        private WorkService service;

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
            set { _NavTabFileInfo = value; }
        }

        FolderModel CurrnetFolder;
        public QYKJViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            UserFolderItem = new ObservableCollection<FolderModel>();
            NavTabFileInfo = new ObservableCollection<FolderModel>() { new FolderModel() { Id = 2, Name = "企业知识库" } };
            CurrnetFolder = new FolderModel();
            service = provider.Resolve<WorkService>();
            LoadFile();
        }

        private async void LoadFile()
        {
           await GetFloderInfo(1);
        }
        async Task GetFloderInfo(int FloderID = 2, int Id = 1)
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
