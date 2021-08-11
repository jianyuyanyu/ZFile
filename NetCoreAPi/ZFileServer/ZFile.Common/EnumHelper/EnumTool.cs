using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Common.EnumHelper
{
    /// <summary>
    /// 方式
    /// </summary>
    public enum DbOrderEnum
    {
        /// <summary>
        /// 打折
        /// </summary>
        [Text("排序Asc")]
        Asc = 1,

        /// <summary>
        /// 满减
        /// </summary>
        [Text("排序Desc")]
        Desc = 2
    }
}
