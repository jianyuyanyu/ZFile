using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class PersonalInfoViewModel : NavigationViewModel
    {
        #region UI
        private OperatorWorkModel _OperatorWorkModel;
        public OperatorWorkModel OperatorWorkModel
        {
            get { return _OperatorWorkModel; }
            set { SetProperty(ref _OperatorWorkModel, value); }
        }

        #endregion
        public PersonalInfoViewModel()
        {

        }
       
        public override Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            OperatorWorkModel= navigationContext.Parameters["OperatorWorkModel"] as OperatorWorkModel;
            return Task.CompletedTask;
        }
    }
}
