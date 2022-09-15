﻿using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ZTAppFreamework.Stared.Service;
using ZTAppFreamework.Stared;
using Prism.Regions;

namespace ZTAppFramework.Admin
{

    /// <summary>
    /// 应用关系管理
    /// </summary>
    public class AppStartService
    {
        System.Windows.Application app;
        public void Exit()
        {
            Environment.Exit(0);
        }

        public async Task<Window> CreateShell(System.Windows.Application application)
        {
            this.app = application;
            var container = ContainerLocator.Container;
            if (!Authorization()) ExitApplication();
            var shell = container.Resolve<object>(AppView.MainName);

            if (shell is Window view)
            {
                var regionManager = container.Resolve<IRegionManager>();
                RegionManager.SetRegionManager(view, regionManager);
                RegionManager.UpdateRegions();
                regionManager.Regions[AppView.MainName].RequestNavigate(AppView.HomeName);
                if (view.DataContext is INavigationAware navigationAware)
                {
                    navigationAware.OnNavigatedTo(null);
                    return (Window)shell;
                }
            }
            return null;
        }

        public static bool Authorization()
        {
            var dialogService = ContainerLocator.Container.Resolve<IHostDialogService>();
            return dialogService.ShowWindow(AppView.LoginName).Result == Prism.Services.Dialogs.ButtonResult.OK;
            //  return false;
        }
        public static void ExitApplication() => Environment.Exit(0);
    }
}
