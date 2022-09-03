using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFreamework.Stared.Service;

namespace ZTAppFreamework.Stared
{
    public static class AppStaredManagerService
    {
        public static void RegisterStaredManager(this IContainerRegistry services)
        {
            services.RegisterSingleton<IHostDialogService, DialogHostService>();
        }


    }
}
