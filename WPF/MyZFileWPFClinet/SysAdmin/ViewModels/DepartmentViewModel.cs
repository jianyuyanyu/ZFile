using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SysAdmin.ViewModels
{
    public class DepartmentViewModel : BaseViewModel
    {

        //LoadedCommand

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        public DepartmentService Service { get; }

        private async void Loaded()
        {
            var ApiRquest = await Service.Gettree();
        }
        public DepartmentViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<DepartmentService>();
        }

        public DepartmentViewModel()
        {

        }
    }
}
