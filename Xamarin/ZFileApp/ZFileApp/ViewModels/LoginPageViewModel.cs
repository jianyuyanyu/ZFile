using System;
using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Prism.Navigation;
using Prism.Services;
using ReactiveUI;
using ZFileApp.Services;

namespace ZFileApp.ViewModels
{
    public class LoginPageViewModel : NavigationViewModelBase
    {
        private readonly ObservableAsPropertyHelper<bool> _isLoading;
        private readonly INavigationService _navigationService;
        private readonly ILoginService _loginService;
        private readonly IPageDialogService _pageDialogService;
        private string _username="admin";
        private string _password="123abc";
        public LoginPageViewModel(INavigationService navigationService,ILoginService loginService, IPageDialogService pageDialogService)
        {
            _navigationService = navigationService;
            _loginService = loginService;
            _pageDialogService = pageDialogService;
            // set the canExecute to an observable of two properties selector.
            var canExecuteLogin =
                this.WhenAnyValue(
                    x => x.Username,
                    x => x.Password,
                    (username, password) =>
                        ValidateEmail(username) && ValidatePassword(password));

            Login = ReactiveCommand.CreateFromTask(ExecuteLogin, canExecuteLogin);

            _isLoading =
                this.WhenAnyObservable(x => x.Login.IsExecuting)
                    .ToProperty(this, x => x.IsLoading, initialValue: false);

        }

        public ReactiveCommand<Unit, Unit> Login { get; set; }

        public bool IsLoading => _isLoading.Value;

        public string Password
        {
            get => _password;
            set => this.RaiseAndSetIfChanged(ref _password, value);
        }

        public string Username
        {
            get => _username;
            set => this.RaiseAndSetIfChanged(ref _username, value);
        }

        public override void OnNavigatedFrom(INavigationParameters parameters)
        {
            Username = string.Empty;
            Password = string.Empty;
        }

        private async Task ExecuteLogin()
        {
            await Observable.Return(Unit.Default).Delay(TimeSpan.FromSeconds(3));

            var LogResqust = await _loginService.LoginAsync(Username, Password);
            if (LogResqust.statusCode==200)
            {
                await _navigationService.NavigateAsync($"NavigationPage/MainPage");
            }
            else
            {
                if (LogResqust.statusCode == 406)
                {
                    await _pageDialogService.DisplayAlertAsync("提示", LogResqust.message, "ok");
                    return;
                }

                await _pageDialogService.DisplayAlertAsync("提示", $"编号:{LogResqust.statusCode},未知错误{LogResqust.message}" , "ok");
            }
        }

        private static bool ValidateEmail(string UserName) => !string.IsNullOrEmpty(UserName) && UserName.Length > 2;

        private static bool ValidatePassword(string password) => !string.IsNullOrEmpty(password) && password.Length > 5;
    }
}
