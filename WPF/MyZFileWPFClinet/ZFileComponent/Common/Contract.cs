using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public static class Contract
    {
        public static readonly string webUrl = "https://localhost:5001/";

        #region  用户信息
        /// <summary>
        /// 登录名
        /// </summary>
        public static string Account = string.Empty;

        /// <summary>
        /// 用户名
        /// </summary>
        public static string UserName = string.Empty;

        /// <summary>
        /// 是否属于管理员
        /// </summary>
        public static bool IsAdmin;
        #endregion
        /// <summary>
        /// 标题
        /// </summary>
        public static string Title;

        /// <summary>
        /// 公司
        /// </summary>
        public static string Company;

        public static QycodeDto qycodeDto;

        public static UserInfo UserInfo;
    }

    public class UserInfo
    {
        public string username { get; set; }
        public string pasd { get; set; }
        public string id { get; set; }
        public string userRealName { get; set; }
        public string role { get; set; }
        public string space { get; set; }
    }
}
