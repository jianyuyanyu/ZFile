using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
using Component.ViewModelBase;
using Prism.Ioc;
using Prism.Services.Dialogs;
using Prism.Regions;
using SysAdmin.Service;

namespace SysAdmin.ViewModels
{
    public class AdminViewModel : BaseViewModel
    {

        private ObservableCollection<SysAdmin> _Admins;

        public ObservableCollection<SysAdmin> AdminList
        {
            get { return _Admins; }
            set
            {
                SetProperty(ref _Admins, value);
            }
        }



        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        public AdminService Service { get; }

        private async void Loaded()
        {
            var ApiRquest = await Service.Getpages(1,15);
            if (ApiRquest.msg == "success")
            {
                AdminList = new ObservableCollection<SysAdmin>();
                AdminList.AddRange(ApiRquest.data);
            }
        }

        public AdminViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<AdminService>();
        }
    }
}
