using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace ZFile.Core.Model.User
{
    [SugarTable("Sys_CodeType")]
    public partial class SysCodeType
    {
        /// <summary>
        /// 唯一标识符号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Guid { get; set; }

        /// <summary>
        /// 字典类型父级
        /// </summary>
        public string ParentGuid { get; set; }

        /// <summary>
        /// 深度
        /// </summary>
        public int Layer { get; set; }

        /// <summary>
        /// 字典类型名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典类型排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditTime { get; set; }
        
        /// <summary>
        /// 归属公司和站点
        /// </summary>
        public string SiteGuid { get; set; }

    }
}
