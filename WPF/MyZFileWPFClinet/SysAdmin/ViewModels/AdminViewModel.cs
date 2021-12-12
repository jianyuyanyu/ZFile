using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections.ObjectModel;
namespace SysAdmin.ViewModels
{
    public class AdminViewModel : BindableBase
    {

        private ObservableCollection<SysAdmin> _Admins;

        public ObservableCollection<SysAdmin> Admins
        {
            get { return _Admins; }
            set
            {
                SetProperty(ref _Admins, value);
            }
        }



        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        private async void Loaded()
        {

        }

        public AdminViewModel()
        {

        }
    }
}
