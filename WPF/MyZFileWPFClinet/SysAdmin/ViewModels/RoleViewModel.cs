using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace SysAdmin.ViewModels
{
    public class RoleViewModel : BaseViewModel
    {
        public RoleService Service { get; }
        public IDialogService _dialogService { get; }

        private ObservableCollection<SysOrganizeTree> _sysOrganizesTrees;
        public ObservableCollection<SysOrganizeTree> SysOrganizesTrees
        {
            get { return _sysOrganizesTrees; }
            set { SetProperty(ref _sysOrganizesTrees, value); }
        }

        private ObservableCollection<SysRole> _sysRoles;
        public ObservableCollection<SysRole> SysRoles
        {
            get { return _sysRoles;; }
            set { SetProperty(ref _sysRoles, value); }
        }
        public RoleViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<RoleService>();
            _dialogService = dialogService;
        }

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        private async void Loaded()
        {
            var ApiRquest = await Service.Gettree();
            if (ApiRquest.success && ApiRquest.statusCode == 200)
            {
                SysOrganizesTrees = new ObservableCollection<SysOrganizeTree>();
                SysOrganizesTrees.AddRange(ApiRquest.data);
            }

            var pagesRequset = await Service.Getpages(1, 15);
            if (pagesRequset.msg == "success")
            {
                SysRoles = new ObservableCollection<SysRole>();
                SysRoles.AddRange(pagesRequset.data);
            }
        }
    }
}
