using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace ZFile.Core.Model.User
{
    /// <summary>
    /// 权限角色表
    /// </summary>
    [SugarTable("Sys_Role")]
    public class SysRole
    {
        [SugarColumn(IsPrimaryKey = true)]
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

        /// <summary>
        /// Desc:添加时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime AddTime { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime EditTime { get; set; }

    }
}
