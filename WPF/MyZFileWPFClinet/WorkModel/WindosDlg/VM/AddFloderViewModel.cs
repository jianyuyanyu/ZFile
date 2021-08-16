using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZFileComponent.Themes.ControlHelper;

namespace WorkModel.WindosDlg.VM
{
    public class AddFloderViewModel : BindableBase
    {
        private readonly WorkService service;
        public AddFloderViewModel(IContainerProvider provider, FolderModel folder)
        {
            service = provider.Resolve<WorkService>();
            this.provider = provider;
            this.folder = folder;
        }

        private string _FolderName;
        private IContainerProvider provider;
        private FolderModel folder;

        public string FolderName
        {
            get { return _FolderName; }
            set { SetProperty(ref _FolderName, value); }
        }

        public DelegateCommand AddFloderCommand => new DelegateCommand(AddFloderClick);

        private async void AddFloderClick()
        {
            folder.Name = FolderName;
            var ApiRes = await service.CreateFolderRequst(folder);
            if (ApiRes.statusCode == 200)
            {
                DialogBox.CloseDialogCommand.Execute("添加成功", null);
                return;
            }

        }
    }
}
