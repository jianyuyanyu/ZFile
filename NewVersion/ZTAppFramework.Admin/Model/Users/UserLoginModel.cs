using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using ZTAppFramework.Admin.Validations.Users;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.Model.Users
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/3 17:20:00 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class UserLoginModel : PropertyViewModel
    {
        private string _UserName;//用户

        private string _Password;//密码

        public UserLoginModel()
        {
            UserName = "";
            Password = "";
        }

        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; RaisePropertyChanged("UserName"); }
        }

        public string Password
        {
            get { return _Password; }
            set { _Password = value; RaisePropertyChanged("Password"); }
        }

        public override string this[string columnName] { get => VerifyTostring(this,columnName); }
    }
}
