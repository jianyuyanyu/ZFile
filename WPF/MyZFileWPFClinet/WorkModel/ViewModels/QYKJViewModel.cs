using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
        public QYKJViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            UserFolderItem = new ObservableCollection<FolderModel>();
            NavTabFileInfo = new ObservableCollection<FolderModel>() { new FolderModel() { Id = 2, Name = "我的网盘>" } };

            service = provider.Resolve<WorkService>();
        }
    }
}
