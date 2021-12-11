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
            get { return _sysRoles; ; }
            set { SetProperty(ref _sysRoles, value); }
        }

        private bool _IsSelectAllCheck;
        public bool IsSelectAllCheck
        {
            get { return _IsSelectAllCheck; }
            set
            {

                if (SetProperty(ref _IsSelectAllCheck, value) && IsUpdateChilderCheck)
                {
                    foreach (var item in SysRoles)
                        item.IsCheck = value;
                }
                IsUpdateChilderCheck = true;
            }
        }
        public bool IsUpdateChilderCheck { get; set; } = true;


        public RoleViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<RoleService>();
            _dialogService = dialogService;
        }

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        public DelegateCommand OpenAddRoleGroupDialogCommand => new DelegateCommand(OpenAddRoleGroupDialog);
        public DelegateCommand<SysRole> OpenEditRoleGroupCommnad => new DelegateCommand<SysRole>(OpenEditRoleGroup);

      

        public DelegateCommand OpenAddRoleDialogCommand => new DelegateCommand(OpenAddRoleDialog);

      

        public DelegateCommand DataGridCheckCommand => new DelegateCommand(DataGridCheck);
        public DelegateCommand DelDataGridCommand => new DelegateCommand(DelDataGrid);
        public DelegateCommand DataGridUncheckedCommand => new DelegateCommand(DataGridUnchecked);

      
        private async void DelDataGrid()
        {
            var resut = System.Windows.MessageBox.Show("确定要批量删除吗", "提示", System.Windows.MessageBoxButton.OKCancel);
            if (resut == System.Windows.MessageBoxResult.OK)
            {
                string str = "";
                foreach (var item in SysRoles)
                {
                    if (item.IsCheck)
                    {
                        str += item.Guid + ",";
                    }

                }
                var parm = new { parm = str };

                var ApiResquest = await Service.Del(parm);
                if (ApiResquest.success && ApiResquest.statusCode == 200)
                {
                    System.Windows.MessageBox.Show("删除成功");
                    Loaded();
                }
            }
        }

        private void DataGridUnchecked()
        {
            IsUpdateChilderCheck = false;
            IsSelectAllCheck = false;
        }

        private void DataGridCheck()
        {
            if (!SysRoles.Any(o => o.IsCheck == false))
            {
                IsUpdateChilderCheck = false;
                IsSelectAllCheck = true;

            }
        }



        void OpenAddRoleDialog()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Titel", "添加角色");
            param.Add("Type", "Add");
            _dialogService.ShowDialog("AddRole", param,
                (result) =>
                {
                    if (result.Result == ButtonResult.Yes)
                    {
                        System.Windows.MessageBox.Show("添加成功");
                        Loaded();
                    }
                });
        }

        void OpenAddRoleGroupDialog()
        {
            DialogParameters param = new DialogParameters();
            param.Add("Titel", "添加角色组");
            param.Add("Type", "Add");
            _dialogService.ShowDialog("AddRoleGroup", param,
                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          System.Windows.MessageBox.Show("添加成功");
                          Loaded();
                      }
                  });
        }

        private void OpenEditRoleGroup(SysRole parm)
        {
            DialogParameters param = new DialogParameters();
            param.Add("Data", parm);
            param.Add("Titel", "修改角色组机");
            param.Add("Type", "Edit");
            _dialogService.ShowDialog("AddRoleGroup", param,

                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          System.Windows.MessageBox.Show("修改成功");
                          Loaded();
                      }

                  });
        }

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
