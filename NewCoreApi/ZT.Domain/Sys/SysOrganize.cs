using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZT.Domain.Sys
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/8 13:44:33 
    /// Description   ：  组织机构表
    ///********************************************/
    /// </summary>
    [SugarTable("sys_organize")]
    public class SysOrganize : Entity
    {
        /// <summary>
        /// 父节点
        /// </summary>
        [Required]
        public long ParentId { get; set; }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Required]
        [StringLength(30)]
        public string Name { get; set; }

        /// <summary>
        /// 机构编码
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 父节点集合
        /// </summary>
        [SugarColumn(IsJson = true)]
        public List<string> ParentIdList { get; set; }

        /// <summary>
        /// 部门层级
        /// </summary>
        [Required]
        public int Layer { get; set; } = 0;

        /// <summary>
        /// 排序
        /// </summary>
        [Required]
        public int Sort { get; set; } = 1;

        /// <summary>
        /// 状态
        /// </summary>
        [Required]
        public bool Status { get; set; } = true;

        /// <summary>
        /// 删除状态
        /// </summary>
        [Required]
        public bool IsDel { get; set; } = false;

        /// <summary>
        /// 部门负责人
        /// </summary>
        public string LeaderUser { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string LeaderMobile { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        public string LeaderEmail { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [Required]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        public string UpdateUser { get; set; }


    }
}
