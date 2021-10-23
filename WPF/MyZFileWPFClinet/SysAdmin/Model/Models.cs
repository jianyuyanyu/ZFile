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
}
