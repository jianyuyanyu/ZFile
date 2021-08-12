using Component.Interfaces;
using Microsoft.Toolkit.Mvvm.Messaging;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModelBase
{
   public class BaseViewModel : BaseObservableObject
    {
        #region Fields

        public readonly ILog Applog;
        private readonly IRegionManager regionManager;

        #endregion Fields

        #region Constructors

        public BaseViewModel()
        {
            ExecuteCommand = new DelegateCommand<string>(Execute, f => !string.IsNullOrWhiteSpace(f));
        }

     

        public BaseViewModel(IContainerProvider provider,IRegionManager regionManager) : this()
        {
            Applog = provider.Resolve<ILog>();
            this.regionManager = regionManager;
        }


        public void NvChangagePage(string RequesControl, string PageKey)
        {
            regionManager.RequestNavigate(RequesControl, PageKey);
        }


        #endregion Constructors


        public DelegateCommand<string> ExecuteCommand { get; private set; }


        /// <summary>
        /// 是否在主线程（UI线程）
        /// </summary>
        public static bool IsInMainThread => System.Threading.Thread.CurrentThread.ManagedThreadId == System.Windows.Application.Current.Dispatcher.Thread.ManagedThreadId;

        /// <summary>
        /// 显示无打扰的通知消息
        /// </summary>
        /// <param name="msg"> 要通知的消息 </param>
        public static void SnackBar(string msg)
        {
            WeakReferenceMessenger.Default.Send(msg, "MainSnackbar");
        }

        /// <summary>
        /// 在UI线程执行动作
        /// </summary>
        /// <param name="action"> 要执行的动作 </param>
        public static void RunUi(Action action)
        {
            if (action == null) return;
            if (IsInMainThread)
            {
                action.Invoke();
            }
            else
            {
                System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
        }

      

        /// <summary>
        /// 在UI线程执行方法，并返回数据
        /// </summary>
        /// <typeparam name="T">数据类型</typeparam>
        /// <param name="func">带结果的方法</param>
        /// <returns></returns>
        public static T RunUi<T>(Func<T> func)
        {
            if (func == null) return default;
            if (IsInMainThread)
            {
                return func.Invoke();
            }
            else
            {
                return System.Windows.Application.Current.Dispatcher.Invoke(func);
            }
        }


        /// <summary>
        /// 处理出现的异常
        /// </summary>
        /// <param name="ex">异常内容</param>
        public void HandleError(Exception ex)
        {
            //Msg?.Error(ex.Message);
            Applog?.Error(ex.Message);
#if DEBUG
            System.Diagnostics.Debug.Write(ex);
#endif
        }

        public virtual void Execute(string args)
        {
#if DEBUG
            System.Diagnostics.Debug.WriteLine($"Execute: {args}");
            
#endif
        }
    }
}
