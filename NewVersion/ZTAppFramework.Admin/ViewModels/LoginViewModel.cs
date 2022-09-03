using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {

        public DelegateCommand<string> ExecuteCommand { get; }
        public LoginViewModel()
        {
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
