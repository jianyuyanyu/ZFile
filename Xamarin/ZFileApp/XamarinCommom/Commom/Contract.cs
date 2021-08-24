using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinCommom.Commom
{
    public static class Contract
    {
        public static readonly string webUrl = "https://10.0.2.2:5001/";

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

      
    }
}
