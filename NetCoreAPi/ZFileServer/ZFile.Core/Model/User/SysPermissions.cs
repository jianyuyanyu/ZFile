﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace ZFile.Core.Model.User
{

    /// <summary>
    /// 权限角色管理菜单表
    /// </summary>
    [SugarTable("Sys_Permissions")]
    public partial class SysPermissions
    {
        /// <summary>
        /// 角色GUID
        /// </summary>
        public string RoleGuid { get; set; }

        /// <summary>
        /// 管理员编号
        /// </summary>
        public string AdminGuid { get; set; }
        
        /// <summary>
        /// 菜单Guid
        /// </summary>
        public string MenuGuid { get; set; }

        /// <summary>
        /// 角色-菜单-权限按钮Json
        /// </summary>
        public string BtnFunJson { get; set; }

        /// <summary>
        /// 授权类型1=角色-菜单 2=用户-角色 3=角色-菜单-按钮功能
        /// 默认=1
        /// </summary>
        public int Types { get; set; } = 1;

        /// <summary>
        /// 用户授权角色 状态  和数据库字段没关系
        /// 默认=1
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool status { get; set; } = false;


    }
}
