using Component.Common.Helpers;
using Component.Common.Service;
using Component.Interfaces;
using HomeModel;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System.Windows;
using WorkModel;
using ZFileWPFClient.ViewModels;
using ZFileWPFClient.Views;
using ZFileWPFClinet.Service;

namespace ZFileWPFClient
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void OnInitialized()
        {
            var win = Container.Resolve<LoginView>();
            var res = win.ShowDialog();
            if (res.Value)
            {
                (MainWindow.DataContext as MainWindowViewModel).Init();
                base.OnInitialized();
            }
        }
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<ILog, Log>();
            containerRegistry.Register<ILoginService, LoginService>();
            containerRegistry.Register<DownService>();
            containerRegistry.RegisterInstance<DownLoadHelper>(new DownLoadHelper(this.Container,null));
            
        }

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<HomeModule>();
            moduleCatalog.AddModule<WorkModule>();
            moduleCatalog.AddModule<TranmissionModel.TranmissionModelModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
