using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
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
            _dialogService.Show("AddDepartment", param,
                    (result) =>
                    {

                    });
        }

        public DepartmentService Service { get; }

        private readonly IDialogService _dialogService;

        private async void Loaded()
        {
            var ApiRquest = await Service.Gettree();
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
