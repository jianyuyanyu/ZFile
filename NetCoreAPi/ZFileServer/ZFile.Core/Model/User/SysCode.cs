using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace ZFile.Core.Model.User
{
    ///<summary>
    /// 字典值
    ///</summary>
    [SugarTable("Sys_Code")]
    public partial class SysCode
    {
        /// <summary>
        /// 唯一标识符
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public string Guid { get; set; }

        /// <summary>
        /// 字典类型标识
        /// </summary>
        public string ParentGuid { get; set; }

        /// <summary>
        /// 字典值——类型
        /// </summary>
        public string CodeType { get; set; }


        /// <summary>
        /// 字典值——名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字典值——英文名称
        /// </summary>
        public string EnName { get; set; }

        /// <summary>
        /// 字典值——排序
        /// </summary>           
        public int Sort { get; set; } = 0;

        /// <summary>
        /// 字典值——状态
        /// </summary>
        public bool Status { get; set; } = true;

        /// <summary>
        /// 字典值——描述
        /// </summary>
        public string Summary { get; set; }
        
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddTime { get; set; } = DateTime.Now;
       
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime EditTime { get; set; } = DateTime.Now;
    }
}
