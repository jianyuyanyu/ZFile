using Newtonsoft.Json;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace SysAdmin
{

    public class SysOrganizeTree
    {
        public string id { get; set; }
        public string title { get; set; }
        public List<SysOrganizeTree> children { get; set; }
        public bool spread { get; set; } = true;
    }

    public class AddSysOrganize
    {
        public string Guid { get; set; }

        public string Name { get; set; }

        public string ParentGuid { get; set; }

        public string ParentName { get; set; }
        public int Sort { get; set; }

        public bool Status { get; set; }
    }

    public  class SysOrganize: BindableBase
    {
        private bool _IsCheck;
        public bool IsCheck
        {
            get { return _IsCheck; }
            set { SetProperty(ref _IsCheck, value); }
        }

        /// <summary>
        /// 唯一编码主键 
        /// </summary>
        public string Guid { get; set; }

        /// <summary>
        /// 站点编码
        /// </summary>
        public string SiteGuid { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public string ParentGuid { get; set; }

        /// <summary>
        /// 组织名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父节点名称
        /// </summary>
        public string ParentName { get; set; }


        /// <summary>
        /// 层级
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 父节点集合
        /// </summary>
        public string ParentGuidList { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditTime { get; set; }

    }

    public class SysRole : BindableBase
    {
        private bool _IsCheck;
        public bool IsCheck
        {
            get { return _IsCheck; }
            set { SetProperty(ref _IsCheck, value); }
        }
        public string Guid { get; set; }
        /// <summary>
        /// 角色父级，角色组
        /// </summary>
        public string ParentGuid { get; set; }

        /// <summary>
        /// 层级  0=角色组  1=角色值
        /// </summary>
        public int Level { get; set; } = 0;

        /// <summary>
        /// 部门ID
        /// </summary>
        public string DepartmentGuid { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        public string DepartmentName { get; set; }

        /// <summary>
        /// 归属于角色组
        /// </summary>
        public string DepartmentGroup { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 角色编号
        /// </summary>
        public string Codes { get; set; }

        /// <summary>
        /// 是否是超级管理员
        /// </summary>
        public bool IsSystem { get; set; }

        public int Sort { get; set; } = 1;

        /// <summary>
        /// 角色描述
        /// </summary>
        public string Summary { get; set; }



    }


    public class SysAdmin
    {

      
        public string Guid { get; set; }

        public string LoginName { get; set; }

        public string LoginPwd { get; set; }

        public string TrueName { get; set; }

        public string Number { get; set; }
        public string HeadPic { get; set; }

        public string Sex { get; set; }

        public string Mobile { get; set; }

        public string UserRealName { get; set; }

        /// <summary>
        /// 归属角色
        /// </summary>
        public string RoleGuid { get; set; }

        ///// <summary>
        ///// 返回角色列表
        ///// </summary>
       
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

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Summary { get; set; }

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
