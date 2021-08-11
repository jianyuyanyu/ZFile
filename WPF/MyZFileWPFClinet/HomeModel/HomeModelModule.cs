using Component.Common;
using HomeModel.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace HomeModel
{
    public class HomeModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
            var regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(SystemResource.Nav_MainContent, typeof(Home));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<MenuService>();
         
        }
    }
}