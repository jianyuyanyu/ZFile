using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZTAppFramework.Admin.ViewModels;
using ZTAppFramework.Admin.Views;
using ZTAppFramework.Application;
using ZTAppFramework.Application.Service;
using ZTAppFramework.SqliteCore;
using ZTAppFramewrok.Application.Stared.HttpManager;
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

            //验证器
            services.RegisterValidator();
    
            //服务
            services.RegisterSingleton<AppStartService>();
            services.RegisterSingleton<AccessTokenManager>();
            services.RegisterSingleton<ApiClinetRepository>();
            //应用逻辑
            services.RegisterApplicationManager();
            services.RegisterStaredManager();
            services.RegisterSqliteManager ();

            //dialog窗口
            services.RegisterDialog<LoginView, LoginViewModel>(AppView.LoginName);
            //页面
            services.RegisterForNavigation<MainView, MainViewModel>(AppView.MainName);
            services.RegisterForNavigation<HomeView, HomeViewModel>(AppView.HomeName);

        }
    }
}