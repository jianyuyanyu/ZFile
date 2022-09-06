using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using ZTAppFramework.Admin.Model.Menus;

namespace ZTAppFramework.Admin.Selector
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间      ：  2022/9/6 11:24:09 
    /// Description   ：  
    ///********************************************/
    /// </summary>
    /// 
    public class MenuItemDataTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (item != null)
            {
                var fe = container as FrameworkElement;
                MenuModel menuItem = item as MenuModel;
                if (menuItem.Childer.Count()>0)
                    return fe.FindResource("MenuItemsDataTemplate") as DataTemplate;
                else
                    return fe.FindResource("MenuItemDataTemplate") as DataTemplate;
            }
            return null;
        }
    }
   
}
