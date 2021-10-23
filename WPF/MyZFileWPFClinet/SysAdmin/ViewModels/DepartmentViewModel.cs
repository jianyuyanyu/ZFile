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
    public class DepartmentViewModel : BaseViewModel
    {
        public DepartmentService Service { get; }

        private readonly IDialogService _dialogService;
        public DepartmentViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<DepartmentService>();
            _dialogService = dialogService;
        }

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);
        
        public DelegateCommand OpenAddDepartmentCommnad => new DelegateCommand(OpenAddDepartment);

        public DelegateCommand<SysOrganize> OpenEditDepartmentCommnad => new DelegateCommand<SysOrganize>(OpenEditDepartment);

        public DelegateCommand DataGridCheckCommand => new DelegateCommand(DataGridCheck);
        public DelegateCommand DataGridUncheckedCommand => new DelegateCommand(DataGridUnchecked);

        public DelegateCommand DelDataGridCommand => new DelegateCommand(DelDataGrid);

        private async void DelDataGrid()
        {

            var resut = System.Windows.MessageBox.Show("确定要批量删除吗", "提示", System.Windows.MessageBoxButton.OKCancel);
            if (resut== System.Windows.MessageBoxResult.OK)
            {
                string str = "";
                foreach (var item in SysOrganizes)
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

        private void DataGridUnchecked() {
            IsUpdateChilderCheck = false;
            IsSelectAllCheck = false;
        } 
     
        private void DataGridCheck()
        {
            if (!SysOrganizes.Any(o => o.IsCheck == false)) {
                IsUpdateChilderCheck = false;
                IsSelectAllCheck = true;
                
            } 
        }



        private ObservableCollection<SysOrganize> _sysOrganizes;
        public ObservableCollection<SysOrganize> SysOrganizes
        {
            get { return _sysOrganizes; }
            set { SetProperty(ref _sysOrganizes, value); }
        }
     
        private ObservableCollection<SysOrganizeTree> _sysOrganizesTrees;
        public ObservableCollection<SysOrganizeTree> SysOrganizesTrees
        {
            get { return _sysOrganizesTrees; }
            set { SetProperty(ref _sysOrganizesTrees, value); }
        }


        private bool _IsSelectAllCheck;
        public bool IsSelectAllCheck
        {
            get { return _IsSelectAllCheck; }
            set {

                if (SetProperty(ref _IsSelectAllCheck, value)&& IsUpdateChilderCheck)
                {
                    foreach (var item in SysOrganizes)
                        item.IsCheck = value;
                }
                IsUpdateChilderCheck = true;
            }
        }
        public bool IsUpdateChilderCheck { get; set; } = true;


        private async void Loaded()
        {
            var ApiRquest = await Service.Gettree();
            if (ApiRquest.success&&ApiRquest.statusCode==200)
            {
                SysOrganizesTrees = new ObservableCollection<SysOrganizeTree>();
                SysOrganizesTrees.AddRange(ApiRquest.data);
            }
            var pagesRequset = await Service.Getpages(1, 15);
            if (pagesRequset.msg== "success")
            {
                SysOrganizes = new ObservableCollection<SysOrganize>();
                SysOrganizes.AddRange(pagesRequset.data);
            }
        }

        private void OpenEditDepartment(SysOrganize organize)
        {
            DialogParameters param = new DialogParameters();
            param.Add("tree", SysOrganizesTrees);
            param.Add("Data", organize);
            param.Add("Titel", "修改组织机构");
            param.Add("Type", "Edit");
            _dialogService.ShowDialog("AddDepartment", param,

                  (result) =>
                  {
                      if (result.Result == ButtonResult.Yes)
                      {
                          System.Windows.MessageBox.Show("修改成功");
                          Loaded();
                      }

                  });
        }
        private void OpenAddDepartment()
        {
            DialogParameters param = new DialogParameters();
            param.Add("tree", SysOrganizesTrees);
            param.Add("Titel", "添加组织机构");
            param.Add("Type", "Add");
            _dialogService.ShowDialog("AddDepartment", param,

                    (result) =>
                    {
                        if (result.Result == ButtonResult.Yes)
                        {
                            System.Windows.MessageBox.Show("添加成功");
                            Loaded();
                        }

                    });
        }
    }
}
