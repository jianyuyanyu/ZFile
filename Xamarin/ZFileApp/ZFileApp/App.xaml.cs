using Prism;
using Prism.Behaviors;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using ZFileApp.Behaviors;
using ZFileApp.Services;
using ZFileApp.ViewModels;
using ZFileApp.Views;

namespace ZFileApp
{
    public partial class App
    {
        public App()
        {
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            var result = await NavigationService.NavigateAsync("LoginPage");

            if (!result.Success)
            {
                System.Diagnostics.Debugger.Break();
            }
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<NetDisListkPage, NetDisListkPageViewModel>();

            //containerRegistry.RegisterForNavigation<UserPage, UserPageViewModel>();

            ///×¢²á·þÎñ
            containerRegistry.RegisterSingleton<ILoginService, LoginService>();
            containerRegistry.RegisterSingleton<IFolderService, FolderService>();
        }
    }
}
