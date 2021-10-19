using Newtonsoft.Json;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.User
{
    [SugarTable("Sys_Admin")]
    public class SysAdmin
    {
   
        [SugarColumn(IsPrimaryKey = true)]
        public string Guid { get; set; }

        public string LoginName { get; set; }

        public string LoginPwd { get; set; }

        public string UserRealName { get; set; }

        /// <summary>
        /// 归属角色
        /// </summary>
        public string RoleGuid { get; set; }

        ///// <summary>
        ///// 返回角色列表
        ///// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<AdminToRoleList> RoleList
        {
            get
            {
                var role = new List<AdminToRoleList>();
                if (!string.IsNullOrEmpty(RoleGuid))
                {
                    role = JsonConvert.DeserializeObject<List<AdminToRoleList>>(RoleGuid);
                }
                return role;
            }
        }


        ///// <summary>
        ///// 归属部门名
        ///// </summary>
        public string DepartmentName { get; set; }

        ///// <summary>
        ///// 归属部门
        ///// </summary>
        public string DepartmentGuid { get; set; }

        ///// <summary>
        ///// 部门集合
        ///// </summary>
        public string DepartmentGuidList { get; set; }

        ///// <summary>
        ///// 状态 1=整除 0=不允许登录
        ///// </summary>
        public bool Status { get; set; }

        ///// <summary>
        ///// 添加时间
        ///// </summary>
        public DateTime AddDate { get; set; } = DateTime.Now;

        ///// <summary>
        ///// 当前登录时间
        ///// </summary>
        public DateTime LoginDate { get; set; } = DateTime.Now;

        ///// <summary>
        ///// 上次登录时间
        ///// </summary>
        public DateTime UpLoginDate { get; set; } = DateTime.Now;

        ///// <summary>
        ///// 登录次数
        ///// </summary>
        //public int LoginSum { get; set; } = 1;

        /// <summary>
        /// 空间大小
        /// </summary>
        public int Space { get; set; }
        /// <summary>
        /// 是否系统管理员
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsSystem { get; set; } = false;

    }

    /// <summary>
    /// 用户关联角色
    /// </summary>
    public class AdminToRoleList
    {
        public string name { get; set; }

        public string guid { get; set; }
    }
}
