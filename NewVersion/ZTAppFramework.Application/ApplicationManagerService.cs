using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Application.Service;

namespace ZTAppFramework.Application
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/6 10:50:00 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class ApplicationManagerService
    {
        public static void RegisterApplicationManager(this IContainerRegistry services)
        {
            services.RegisterScoped<UserService>();
            services.RegisterScoped<MenuService>();
        }
      
    }
}
