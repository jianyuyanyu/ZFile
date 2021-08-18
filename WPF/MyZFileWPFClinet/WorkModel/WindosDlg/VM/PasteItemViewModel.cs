using Component.Common;
using Component.Common.Helpers;
using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZFileComponent.Themes.ControlHelper;

namespace WorkModel.WindosDlg.VM
{
    public class PasteItemViewModel : BindableBase
    {
        private IContainerProvider provider;
        private PasteitemsDto folder;
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
            set { SetProperty(ref _NavTabFileInfo, value); }
        }
        
        FolderModel CurrnetFolder;
        public PasteItemViewModel(IContainerProvider provider, PasteitemsDto folder)
        {
            this.provider = provider;
            this.folder = folder;
            service = provider.Resolve<WorkService>();
            UserFolderItem = new ObservableCollection<FolderModel>();
            NavTabFileInfo = new ObservableCollection<FolderModel>() { new FolderModel() { Id = 2, Name = "根目录" } };
            CurrnetFolder = new FolderModel();
            LoadMenu();
        }

        public DelegateCommand<FolderModel> OpenFloderCommand => new DelegateCommand<FolderModel>((o) => OpenFloder(o));

        public DelegateCommand CloseCommand => new DelegateCommand(Close);
        public DelegateCommand PasteitemCommand => new DelegateCommand(Pasteitem);

        private async void Pasteitem()
        {
            folder.Pid = CurrnetFolder.Id;
           var ApiRes= await service.PasteitemRequst(folder);
            if (ApiRes.statusCode!=200)
            {
                MessageBox.Show("迁移失败");
                return;
            }
            DialogBox.CloseDialogCommand.Execute("迁移成功", null);
        }

        private void Close()=> DialogBox.CloseDialogCommand.Execute(null, null);
   

        private async void LoadMenu()
        {
            await GetFloderInfo();
            CurrnetFolder.Type = 2;
            CurrnetFolder.Id = 2;
            CurrnetFolder.Remark = "2";
        }

        private async void OpenFloder(FolderModel o)
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

        async Task GetFloderInfo(int FloderID = 2, int Id = 2)
        {
            UserFolderItem.Clear();
            var model = await service.GetFolderInfo(Id, FloderID);
            if (model != null)
            {
                CurrnetFolder.Id = FloderID;
                model.data.ForEach(o => UserFolderItem.Add(new FolderModel() { CRTime = o.CRDate.Value, format = "文件", Size = "", Id = o.ID, Name = o.Name, Type = 2, Remark = o.Remark }));
            }
        }
    }
}
