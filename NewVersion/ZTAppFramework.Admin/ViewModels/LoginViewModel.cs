using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {

        private UserLoginModel _Login;
        public UserLoginModel Login
        {
            get { return _Login; }
            set { SetProperty(ref _Login, value); }
        }

        private string _Name;

        public string NameUser
        {
            get { return _Name; }
            set { SetProperty(ref _Name, value); }
        }


        public DelegateCommand<string> ExecuteCommand { get; }
      

        public LoginViewModel()
        {
            Login=new UserLoginModel();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private  void Execute(string parm)
        {
            switch (parm)
            {
                case "LoginUser":
                     LoginUserAsync();
                    break;
                default:
                    break;
            }
        }

        private void  LoginUserAsync()
        {
            if (!Verify(Login).IsValid) return;
            OnDialogClosed();
        }

        public override void Cancel()
        {
           
        }

        public override void OnSave()
        {
           
        }
    }
}
