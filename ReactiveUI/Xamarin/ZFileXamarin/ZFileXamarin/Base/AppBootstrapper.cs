using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;
using ReactiveUI.XamForms;
using Splat;
using Xamarin.Forms;
using ZFileXamarin.Pages;
using ZFileXamarin.Pages.TabPages;
using ZFileXamarin.Services;
using ZFileXamarin.ViewModel;

namespace ZFileXamarin.Base
{
    public class AppBootstrapper : ReactiveObject, IScreen
    {
        public RoutingState Router { get; }

        public AppBootstrapper()
        {
            Router = new RoutingState();
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));
            RegisterServices();
            RegisterViews();
            Router.Navigate.Execute(new LogViewModel());
           
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        private void RegisterViews()
        {
            Locator.CurrentMutable.Register(() => new LoginPage(), typeof(IViewFor<LogViewModel>));
            Locator.CurrentMutable.Register(() => new HomePage(), typeof(IViewFor<HomeViewModel>));
            Locator.CurrentMutable.Register(() => new FileMangementPage(), typeof(IViewFor<FileMangementViewModel>));
        }

        /// <summary>
        /// 注册服务
        /// </summary>
        private void RegisterServices()
        {
            Locator.CurrentMutable.Register(()=>new LoginService(),typeof(ILoginService));
        
        }

        internal Page CreateMainPage()
        {
            return new RoutedViewHost();
        }
    }
}
