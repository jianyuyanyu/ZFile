using GRKJ.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace GRKJ
{
    public class GRKJModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<GRKJView>(typeof(GRKJView).Name);
        }
    }
}