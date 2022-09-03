using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZTAppFramework.Admin.ViewModels;
using ZTAppFramework.Admin.Views;
using ZTAppFreamework.Stared;

namespace ZTAppFramework.Admin
{
    public class AdminModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry services)
        {
            services.RegisterDialog<LoginView, LoginViewModel>(AppView.LoginName);
        }
    }
}