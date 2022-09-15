using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ZTAppFrameword.Template.Global
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/15 9:07:22 
    /// Description   ：  Windows弹窗接口
    ///********************************************/
    /// </summary>
    public interface IZTDialogWindow
    {

        /// <summary>
        /// Dialog content.
        /// </summary>
        object Content { get; set; }

        /// <summary>
        /// Close the window.
        /// </summary>
        void Close();

        /// <summary>
        /// The window's owner.
        /// </summary>
        Window Owner { get; set; }

        /// <summary>
        /// Show a non-modal dialog.
        /// </summary>
        void Show();

        /// <summary>
        /// Show a modal dialog.
        /// </summary>
        /// <returns></returns>
        bool? ShowDialog();

        /// <summary>
        /// The data context of the window.
        /// </summary>
        /// <remarks>
        /// The data context must implement <see cref="IDialogAware"/>.
        /// </remarks>
        object DataContext { get; set; }

        /// <summary>
        /// Called when the window is loaded.
        /// </summary>
        event RoutedEventHandler Loaded;

        /// <summary>
        /// Called when the window is closed.
        /// </summary>
        event EventHandler Closed;

        /// <summary>
        /// Called when the window is closing.
        /// </summary>
        event CancelEventHandler Closing;

        /// <summary>
        /// The result of the dialog.
        /// </summary>
        IZTDialogResult Result { get; set; }

        /// <summary>
        /// The window style.
        /// </summary>
        Style Style { get; set; }
    }
}
