using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class UserEditPasswordViewModel : NavigationViewModel
    {

        #region UI
        private UserEditPwdModel _userEditPwd;

        public UserEditPwdModel UserEditPwd
        {
            get { return _userEditPwd; }
            set { SetProperty(ref _userEditPwd, value); }
        }
        #endregion

        #region Command

        public DelegateCommand ModifPasswordCommand { get;  }
        #endregion

        #region Service

        #endregion


        public UserEditPasswordViewModel()
        {
            UserEditPwd=new UserEditPwdModel();

            ModifPasswordCommand = new DelegateCommand(ModifPassword);
        }

        private void ModifPassword()
        {
           
        }
    }
}
