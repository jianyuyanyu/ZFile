using Component;
using Component.Api;
using Component.ViewModelBase;
using Microsoft.Toolkit.Mvvm.Input;
using Microsoft.Toolkit.Mvvm.Messaging;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZFileWPFClinet.Service;

namespace ZFileWPFClient.ViewModels
{
    public class LoginViewModel : BaseCustomViewModel
    {

        #region Fields

        private readonly ILoginService service;


        #endregion Fields

        private string _Version;

        public string Version
        {
            get { return _Version; }
            set { SetProperty(ref _Version, value); }
        }

        private string _UserName;

        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }

        private string _PassWord;

        public string PassWord
        {
            get { return _PassWord; }
            set { SetProperty(ref _PassWord, value); }
        }

        private string _Msg;

        public string Msg
        {
            get { return _Msg; }
            set { SetProperty(ref _Msg, value); }
        }


        public RelayCommand LoginCommand { get; private set; }

        public LoginViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            this.service = provider.Resolve<ILoginService>();
            Version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
            LoginCommand = new RelayCommand(Login);
            UserName = "demo888";
            PassWord = "demo888";
        }

        private async void Login()
        {
            try
            {
                if (DialogIsOpen) return;
                if (string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(PassWord))
                {
                    return;
                }
                DialogIsOpen = true;
                Msg = "正在登入。。。";
                var r = await service.LoginAsync(UserName, PassWord);

                if (r == null || !r.success || r.statusCode != 200)
                {
                    SnackBar(r == null ? "连接异常,请稍后重试!" : r.message);
                    //   WeakReferenceMessenger.Default.Send("", "NavigationPage");
                    return;
                }
                RestSharpCertificateMethod.Token = r.data.ToString();
                Msg = "登入成功获取用户信息中。。。";
                //var USE = await service.GetUserInfoAsync();
                //if (USE == null || !USE.success || USE.statusCode != 200)
                //{
                //    SnackBar(r == null ? "获取用户信息异常,请稍后重试!" : r.message);
                //    return;
                //}

                //Contract.UserInfo = new UserInfo()
                //{
                //    id = USE.data.Result.id,
                //    pasd = USE.data.Result.pasd,
                //    role = USE.data.Result.role,
                //    space = USE.data.Result.space,
                //    username = USE.data.Result.username,
                //    userRealName = USE.data.Result.userRealName
                //};
                //Contract.Title = USE.data.Result1;
                //Contract.Company = USE.data.Result2;
                //Contract.qycodeDto = new QycodeDto()
                //{
                //    Code = USE.data.Result4.code,
                //    Description = USE.data.Result4.description,
                //    crdate = USE.data.Result4.crdate,
                //    filecount = USE.data.Result4.filecount,
                //    ID = USE.data.Result4.id,
                //    IsUsed = USE.data.Result4.isUsed,
                //    Name = USE.data.Result4.name,
                //    secret = USE.data.Result4.secret,
                //    space = USE.data.Result4.space,
                //    updatetime = USE.data.Result4.updatetime,
                //    yyspace = USE.data.Result4.yyspace
                //};

                await Task.Delay(1000);
                WeakReferenceMessenger.Default.Send("", "NavigationPage");
            }
            catch (Exception)
            {

            }
            finally
            {
                DialogIsOpen = false;
            }
        }
    }

   
}
