using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZTAppFrameword.Template.Control
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/29 8:33:42 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    public class ZTTreeViewItem: TreeViewItem
    {
        /// <summary>
        /// 重写自定义指定项子控件
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is ZTTreeViewItem;
        }

        /// <summary>
        /// 抓取指定项控件并返回定制项控件
        /// </summary>
        /// <returns></returns>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new ZTTreeViewItem();
        }

    }
}
