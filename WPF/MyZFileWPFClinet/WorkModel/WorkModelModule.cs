using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using WorkModel.Views;

namespace WorkModel
{
    public class WorkModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<GRKJ>(typeof(GRKJ).Name + "View");
            containerRegistry.RegisterForNavigation<QYKJ>(typeof(QYKJ).Name + "View");
            containerRegistry.RegisterForNavigation<YHGX>(typeof(YHGX).Name + "View");
            containerRegistry.Register<WorkService>();
        }
    }
}