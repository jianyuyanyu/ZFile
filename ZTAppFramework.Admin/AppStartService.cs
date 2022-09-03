using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZTAppFramework.Admin
{
    public class AppStartService
    {

        Application app;
        public void Exit()
        {
            Environment.Exit(0);
        }


        public Task<Window> CreateShell(Application application)
        {
            this.app = application;
            var container= ContainerLocator.Container;

            return null;
        }
    }
}
