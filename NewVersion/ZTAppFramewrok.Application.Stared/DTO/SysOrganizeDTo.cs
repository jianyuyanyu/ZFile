using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFramewrok.Application.Stared.DTO
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  WeiXiaolei
    /// 创建时间    ：  2022/9/7 8:33:53 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class SysOrganizeDTo
    {
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
