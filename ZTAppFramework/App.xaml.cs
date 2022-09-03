using Prism.DryIoc;
using Prism.Ioc;
using Prism.Modularity;
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
            //moduleCatalog.AddModule<SharedModule>();
            base.ConfigureModuleCatalog(moduleCatalog);
        }
    }
}
