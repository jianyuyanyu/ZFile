using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZTAppFrameword.Template.Enums
{

    public enum InputType
    {
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 数字
        /// </summary>
        Number,
        /// <summary>
        /// 电话
        /// </summary>
        Phone,
        /// <summary>
        /// 正则表达式
        /// </summary>
        Regex
    }

    #region 按钮
    public enum ButtonStyle
    {
        /// <summary>
        /// 原始
        /// </summary>
        Primary,
        /// <summary>
        /// 默认
        /// </summary>
        Default,
        /// <summary>
        /// 百搭
        /// </summary>
        Normal,
        /// <summary>
        /// 暖色
        /// </summary>
        Warm,
        /// <summary>
        /// 警告
        /// </summary>
        Danger,

    }


    public enum ButtonType
    {
        Standard,
        Link,
    }


    #endregion
}
