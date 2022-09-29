using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using ZTAppFrameword.Template.Global;
using ZTAppFramework.PictureMarker.Views;
using ZTAppFreamework.Stared;

namespace ZTAppFramework.PictureMarker
{
    public class PictureMarkerModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {

        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            ZTDialog.RegisterDialog<PicturePreView>(AppView.PicturePreName);
        }
    }
}