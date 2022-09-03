using FluentValidation;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFramework.Admin.Validations.Users;

namespace ZTAppFramework.Admin
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间      ：  2022/9/3 17:26:46 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public static class AdminValidatorExtensions
    {
        /// <summary>
        /// 注册验证
        /// </summary>
        /// <param name="services"></param>
        public static void RegisterValidator(this IContainerRegistry services)
        {
            services.RegisterScoped<IValidator<UserLoginModel>, LoginValidator>();
        }

    }
}
