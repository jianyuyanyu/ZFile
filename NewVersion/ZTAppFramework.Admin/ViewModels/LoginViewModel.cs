using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFramework.Application.Service;
using ZTAppFramewrok.Application.Stared.DTO;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {

        private UserLoginModel _Login;
        private readonly UserService _userLoginService;

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


        public LoginViewModel(UserService  userLoginService)
        {
            _userLoginService = userLoginService;
            Login = new UserLoginModel();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        private async void Execute(string parm)
        {
            switch (parm)
            {
                case "LoginUser":
                    await LoginUserAsync();
                    break;
                default:
                    break;
            }
        }

        private async Task LoginUserAsync()
        {
            if (!Verify(Login).IsValid) return;

            LodingMessage = "登入中";
            await SetBusyAsync(async () =>
              {
                  await _userLoginService.LoginServer(Map<UserInfoDto>(Login));
                  await Task.Delay(3000);
                  LodingMessage = "导入台账中";
                  await Task.Delay(3000);
                  LodingMessage = "导入界面中";
                  await Task.Delay(3000);
              });
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
