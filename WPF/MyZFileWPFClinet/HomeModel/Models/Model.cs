﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace HomeModel
{
    public class Menu
    {
        public Menu()
        {
            ChilderList = new ObservableCollection<SysMenuDto>();
        }
        public SysMenuDto Head { get; set; }

        public ObservableCollection<SysMenuDto> ChilderList  { get; set; }
    }


    /// <summary>
    /// 管理员登录，获得菜单权限列表
    /// </summary>
    public class SysMenuDto
    {
        /// <summary>
        /// Desc:唯一标识Guid
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string guid { get; set; }

        /// <summary>
        /// Desc:菜单父级Guid
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parentGuid { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string parentName { get; set; }

        /// <summary>
        /// Desc:菜单名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string name { get; set; }

        /// <summary>
        /// Desc:菜单名称标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string nameCode { get; set; }

        /// <summary>
        /// Desc:所属父级的集合
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string parentGuidList { get; set; }

        /// <summary>
        /// Desc:菜单深度
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int layer { get; set; }

        /// <summary>
        /// Desc:菜单Url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string urls { get; set; }

        /// <summary>
        /// Desc:菜单图标Class
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string icon { get; set; }

        /// <summary>
        /// Desc:菜单图标Class
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string btnJson { get; set; }

        /// <summary>
        /// Desc:菜单排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int sort { get; set; }

        /// <summary>
        /// Desc:权限操作是否选中
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public bool isChecked { get; set; } = false;

        /// <summary>
        /// 当前菜单的功能列表
        /// </summary>
        public List<SysCodeDto> btnFun { get; set; }
    }

    /// <summary>
    /// 角色授权显示权限值
    /// </summary>
    public class SysCodeDto
    {
        public string guid { get; set; }

        public string name { get; set; }

        public string codeType { get; set; }

        public bool status { get; set; } = false;
    }
}
