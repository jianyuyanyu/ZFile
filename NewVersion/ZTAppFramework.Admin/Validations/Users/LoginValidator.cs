using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Users;
using ZTAppFreamework.Stared.Validations;

namespace ZTAppFramework.Admin.Validations.Users
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间      ：  2022/9/3 17:23:07 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class LoginValidator : AbstractValidator<UserLoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.UserName).IsRequired("账号不能为空").MaxLength(256);
            RuleFor(x => x.Password).IsRequired("密码不能为空").MaxLength(256);
        }
    }
}
