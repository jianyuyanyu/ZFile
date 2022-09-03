using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using ZTAppFramework.Admin;

namespace ZTAppFramework
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {

        protected override Window CreateShell() => null;

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }


        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<AdminModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
        protected override void ConfigureRegionAdapterMappings(RegionAdapterMappings regionAdapterMappings)
        {
            base.ConfigureRegionAdapterMappings(regionAdapterMappings);
            //regionAdapterMappings.ConfigurationAdapters(Container);
        }
        protected override async void OnInitialized()
        {
            var appStart = ContainerLocator.Container.Resolve<AppStartService>();
            MainWindow = await appStart.CreateShell(this);
            base.OnInitialized();
        }
    }
}
