using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZFileApp.ViewModels;

namespace ZFileApp.Views
{
    public partial class LoginPage : ContentPageBase<LoginPageViewModel>
    {
        public LoginPage()
        {
            InitializeComponent();

            this.Bind(ViewModel, x => x.Username, x => x.Username.Text)
              .DisposeWith(ViewBindings);

            this.Bind(ViewModel, x => x.Password, x => x.Password.Text)
                .DisposeWith(ViewBindings);

            this.BindCommand(ViewModel, x => x.Login, x => x.LoginButton)
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.Loading.IsVisible)
                .DisposeWith(ViewBindings);

            this.OneWayBind(ViewModel, x => x.IsLoading, x => x.Loading.IsRunning)
                .DisposeWith(ViewBindings);

            this.WhenAnyObservable(x => x.ViewModel.Login.CanExecute)
                .StartWith(false)
                .DistinctUntilChanged()
                .Subscribe(canExecute =>
                {
                    LoginButton.BackgroundColor = canExecute ? Color.FromHex("#3897F0") : Color.FromHex("#AFD5F9");
                    LoginButton.TextColor = canExecute ? Color.White : Color.Black;
                });
        }
    }
}
