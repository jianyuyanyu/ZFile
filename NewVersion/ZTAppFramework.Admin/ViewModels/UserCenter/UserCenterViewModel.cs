using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class UserCenterViewModel : NavigationViewModel
    {

        #region UI
        private OperatorWorkModel _OperatorWorkModel;
        public OperatorWorkModel OperatorWorkModel
        {
            get { return _OperatorWorkModel; }
            set { SetProperty(ref _OperatorWorkModel, value); }
        }
        #endregion

        #region Command

        #endregion

        #region Serivce
        private readonly OperatorService _operatorService;
        #endregion
        public UserCenterViewModel(OperatorService operatorService)
        {
            _operatorService = operatorService;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            var r = await _operatorService.GetUserWordInfo();
            if (r.Success)
                OperatorWorkModel = Map<OperatorWorkModel>(r.data);
        }
    }
}
