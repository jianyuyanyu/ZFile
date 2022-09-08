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
using ZTAppFramewrok.Application.Stared;

using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class LoginViewModel : DialogViewModel
    {


        private readonly UserService _userLoginService;

        #region UI
        private ObservableCollection<UserLoginModel> _AccountList;
        private UserLoginModel _Login;
        private bool _IsSavePwd;
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

        public bool IsSavePwd
        {
            get { return _IsSavePwd; }
            set { SetProperty(ref _IsSavePwd, value); }
        }

        #endregion

        #region Command
        public DelegateCommand<string> ExecuteCommand { get; }
        #endregion

        public LoginViewModel(UserService userLoginService)
        {
            AccountList = new ObservableCollection<UserLoginModel>();
            _userLoginService = userLoginService;
            Login = new UserLoginModel();
            ExecuteCommand = new DelegateCommand<string>(Execute);
        }

        /// <summary>
        /// Cmmand 处理逻辑
        /// </summary>
        /// <param name="parm"></param>
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

        /// <summary>
        /// 登入
        /// </summary>
        /// <returns></returns>
        private async Task LoginUserAsync()
        {
            if (!Verify(Login).IsValid) return;
            LodingMessage = "登入中";
            await SetBusyAsync(async () =>
            {
                await Task.Delay(1000);
                var res = await _userLoginService.LoginServer(Map<LoginParam>(Login));
                if (!res.Success) return;
                await _userLoginService.SaveLocalAccountInfo(IsSavePwd, Map<LoginParam>(Login));
                OnDialogClosed();
            });
        }


        public override async void OnDialogOpened(IDialogParameters parameters)
        {
            var result = await _userLoginService.GetLocalAccountList();
            if (result.Success)
                AccountList.AddRange(Map<List<UserLoginModel>>(result.data));
            var r = await _userLoginService.GetLocalAccountInfo();
            if (r.Success)
            {
                IsSavePwd = r.data.Check;
                if (string.IsNullOrEmpty(r.data.Values)) return;
                var mod = AccountList.FirstOrDefault(x => x.UserName == r.data.Values);
                Login.UserName = mod.UserName;
                if (IsSavePwd)
                    Login.Password = mod.Password;

            }
        }

        public override void Cancel()
        {

        }

        public override void OnSave()
        {

        }
    }
}
