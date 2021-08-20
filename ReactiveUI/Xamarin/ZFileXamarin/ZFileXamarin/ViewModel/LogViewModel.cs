using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using Xamarin.Forms;
using ZFileXamarin.Services;

namespace ZFileXamarin.ViewModel
{
    public class LogViewModel : ReactiveObject, IRoutableViewModel
    {

        [Reactive]
        public string UserName { get; set; } = "admin";
        [Reactive]
        public string Password { get; set; } = "abc123";
        [Reactive]
        public string Msg { get; set; }

        [Reactive]
        public bool DialogIsOpen { get; set; } = false;


        public string UrlPathSegment => "欢迎使用ZFile网盘";

        public IScreen HostScreen { get; }

        private readonly ILoginService _loginService;

        public ICommand LoginCommand { get; }
   


        public LogViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            _loginService = _loginService ?? (ILoginService)Splat.Locator.Current.GetService(typeof(ILoginService));
           //var NavigateCommand = ReactiveCommand.CreateFromObservable(() =>
           // {
           //     return HostScreen.Router.Navigate.Execute(new HomeViewModel());
           // });
       
            var canExecuteLogin = this.WhenAnyValue(vm => vm.UserName, vm => vm.Password, (U, p) =>
            {
                if (string.IsNullOrEmpty(U) && p.Length > 3)
                {
                    return false;
                }
                return true;
            });

               
            LoginCommand = ReactiveCommand.Create(Login, canExecuteLogin);
        }

        async void Login()
        {
            DialogIsOpen = true;
               Msg = "正在登入";
            var LoginApiReust = await _loginService.LoginAsync(UserName, Password);
            if (LoginApiReust.statusCode!=200)
            {
                Msg = "登入异常！";
                await Task.Delay(1000);
                
                DialogIsOpen = false;
                return;
            }
            Msg = "登入成功！";
            await Task.Delay(1000);
            DialogIsOpen = false;
            await HostScreen.Router.NavigateAndReset.Execute(new HomeViewModel());


            //HostScreen.Router.Navigate.Execute();
            
        }
       
    }
}
