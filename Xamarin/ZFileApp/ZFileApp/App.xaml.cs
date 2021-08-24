using Prism;
using Prism.Behaviors;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using ZFileApp.Behaviors;
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
            containerRegistry.RegisterForNavigation<TabbedPage>();
            //containerRegistry.RegisterSingleton<IPageBehaviorFactory, SampleBehaviorFactory>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
            containerRegistry.RegisterForNavigation<NetDiskPage, NetDiskPageViewModel>();
            containerRegistry.RegisterForNavigation<UserPage, UserPageViewModel>();
        }
    }
}
