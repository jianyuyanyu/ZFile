using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZFile.Common.EnumHelper
{
    public enum LogEnum
    {
        /// <summary>
        /// 保存或添加
        /// </summary>
        [Text("保存或添加")]
        ADD = 1,

        /// <summary>
        /// 更新
        /// </summary>
        [Text("更新/修改")]
        UPDATE = 2,

        /// <summary>
        /// 更新
        /// </summary>
        [Text("审核")]
        AUDIT = 3,

        /// <summary>
        /// 删除
        /// </summary>
        [Text("删除")]
        DELETE = 4,

        /// <summary>
        /// 读取/查询
        /// </summary>
        [Text("读取/查询")]
        RETRIEVE = 5,

        /// <summary>
        /// 登录
        /// </summary>
        [Text("登录")]
        LOGIN = 6,

        /// <summary>
        /// 查看
        /// </summary>
        [Text("查看")]
        LOOK = 7,

        /// <summary>
        /// 更改状态
        /// </summary>
        [Text("更改状态")]
        STATUS = 8,

        /// <summary>
        /// 授权
        /// </summary>
        [Text("授权")]
        AUTHORIZE = 9,

        /// <summary>
        /// 退出登录
        /// </summary>
        [Text("退出登录")]
        LOGOUT = 10,

        /// <summary>
        /// 同步到微信
        /// </summary>
        [Text("同步到微信")]
        ASYWX = 11,

        /// <summary>
        /// 任务
        /// </summary>
        [Text("自动任务")]
        TASK = 12
    }

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
