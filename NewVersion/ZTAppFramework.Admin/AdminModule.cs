using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZTAppFramework.Admin.ViewModels;
using ZTAppFramework.Admin.Views;
using ZTAppFreamework.Stared;
using ZTAppFreamework.Stared.Service;

namespace ZTAppFramework.Admin
{
    public class AdminModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        /// <summary>
        /// 注册服务
        /// </summary>
        /// <param name="services"></param>
        public void RegisterTypes(IContainerRegistry services)
        {
            //服务
            services.RegisterSingleton<AppStartService>();
            services.RegisterStaredManager();

            //dialog窗口
            services.RegisterDialog<LoginView, LoginViewModel>(AppView.LoginName);
            //页面
            services.RegisterForNavigation<MainView, MainViewModel>(AppView.MainName);

        }
    }
}