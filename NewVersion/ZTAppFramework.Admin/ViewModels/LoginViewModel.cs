using Prism.Commands;
using Prism.Mvvm;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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




        private ObservableCollection<UserLoginModel> _AccountList;

        public ObservableCollection<UserLoginModel> AccountList
        {
            get { return _AccountList; }
            set { SetProperty(ref _AccountList, value); }
        }


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


        public LoginViewModel(UserService userLoginService)
        {
            AccountList = new ObservableCollection<UserLoginModel>();
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
                await Task.Delay(1000);
                var res = await _userLoginService.LoginServer(Map<UserInfoDto>(Login));
                if (!res.Success) return;
                OnDialogClosed();
            });
        }


        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            var result = await _userLoginService.GetLocalAccountList();
            if (result.Success)
                AccountList.AddRange(Map<List<UserLoginModel>>(result.data));
        }
        public override void Cancel()
        {

        }

        public override void OnSave()
        {

        }
    }
}
