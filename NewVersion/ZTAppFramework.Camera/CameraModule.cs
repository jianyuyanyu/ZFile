using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZTAppFramework.Camera.Service;
using ZTAppFramework.Camera.Store;
using ZTAppFramework.Camera.Views;

namespace ZTAppFramework.Camera
{
    public class CameraModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<CameraServiceMediator>();
            containerRegistry.RegisterSingleton<CameraStore>();
            containerRegistry.RegisterSingleton<ImageStore>();
        }
    }
}