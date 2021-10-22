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

        //LoadedCommand

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        public DelegateCommand OpenAddDepartmentCommnad => new DelegateCommand(OpenAddDepartment);

        private void OpenAddDepartment()
        {
            DialogParameters param = new DialogParameters();
            param.Add("tree",SysOrganizesTrees);
            param.Add("Titel","添加组织机构");
            param.Add("Type", "Add");
            _dialogService.ShowDialog("AddDepartment", param,

                    (result) =>
                    {
                        if (result.Result==ButtonResult.Yes)
                        {
                            System.Windows.MessageBox.Show("添加成功");
                            Loaded();
                        }
                       
                    });
        }

        public DepartmentService Service { get; }

        private readonly IDialogService _dialogService;

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
        public DepartmentViewModel(IContainerProvider provider, IRegionManager regionManager,IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<DepartmentService>();
            _dialogService = dialogService;
        }

        public DepartmentViewModel()
        {

        }
    }
}
