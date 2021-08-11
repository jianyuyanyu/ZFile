using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using TranmissionModel.Views;

namespace TranmissionModel
{
    public class TranmissionModelModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<Down>(typeof(Down).Name + "View");
            containerRegistry.RegisterForNavigation<Upload>(typeof(Upload).Name + "View");
        }
    }
}