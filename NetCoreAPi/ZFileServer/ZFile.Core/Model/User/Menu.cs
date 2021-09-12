using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Core.Model.User
{   
    
    ///<summary>
     /// 系统菜单表
     ///</summary>
    [SugarTable("Sys_Menu")]
    public class Menu
    {
        /// <summary>
        /// Desc:唯一标识Guid
        /// Default:
        /// Nullable:False
        /// </summary> 
        [SugarColumn(IsPrimaryKey = true, IsIdentity = true)]
        public string Guid { get; set; }

        /// <summary>
        /// Desc:所属站点或菜单
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string SiteGuid { get; set; }

        /// <summary>
        /// Desc:菜单父级Guid
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentGuid { get; set; }

        /// <summary>
        /// 父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// Desc:菜单名称
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string Name { get; set; }

        /// <summary>
        /// Desc:菜单名称标识
        /// Default:
        /// Nullable:False
        /// </summary>           
        public string NameCode { get; set; }

        /// <summary>
        /// Desc:所属父级的集合
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ParentGuidList { get; set; }

        /// <summary>
        /// Desc:菜单深度
        /// Default:
        /// Nullable:False
        /// </summary>           
        public int Layer { get; set; }

        /// <summary>
        /// Desc:菜单Url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Urls { get; set; }

        /// <summary>
        /// Desc:菜单图标Class
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Icon { get; set; }

        /// <summary>
        /// Desc:菜单排序
        /// Default:0
        /// Nullable:False
        /// </summary>           
        public int Sort { get; set; }

        /// <summary>
        /// Desc:菜单状态 true=正常 false=不显示
        /// Default:b'1'
        /// Nullable:False
        /// </summary>           
        public bool Status { get; set; } = true;

        /// <summary>
        /// 菜单按钮功能的值，关联Code
        /// </summary>
        public string BtnFunJson { get; set; }

        /// <summary>
        /// Desc:修改时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime EditTime { get; set; } = DateTime.Now;

        /// <summary>
        /// Desc:添加时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime AddTIme { get; set; } = DateTime.Now;


        /// <summary>
        /// 授权功能  和数据库没关系
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public List<string> cbks { get; set; }
    }
}
