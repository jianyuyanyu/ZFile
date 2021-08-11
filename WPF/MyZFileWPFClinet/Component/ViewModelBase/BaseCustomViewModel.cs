using Component.Interfaces;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Component.ViewModelBase
{
   public class BaseCustomViewModel:BaseViewModel
    {
        #region Fields

        private bool dialogIsOpen;
        private bool isBusy;

        #endregion Fields


        #region Properties

        /// <summary>
        /// 退出命令
        /// </summary>
        public DelegateCommand ExitCommand { get; private set; }

        /// <summary>
        /// DataGrid编辑器命令
        /// </summary>
        //public DelegateCommand<SfDataGrid> DataGridEditorCommand { get; private set; }

        /// <summary>
        /// 窗口是否显示
        /// </summary>
        public bool DialogIsOpen { get => dialogIsOpen; set => SetProperty(ref dialogIsOpen, value); }

        /// <summary>
        /// 数据刷新动画启动标记
        /// </summary>
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// 默认构造器
        /// </summary>
        public BaseCustomViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            ExitCommand = new DelegateCommand(Exit);
            //DataGridEditorCommand = new RelayCommand<SfDataGrid>(Editor);
        }

        #endregion Constructors

        #region Methods

        

        /// <summary>
        /// 传递True代表需要确认用户是否关闭,你可以选择传递false强制关闭
        /// </summary>
        public virtual void Exit()
        {
            Environment.Exit(0);
        }


        /// <summary>
        /// 在UI线程执行动作
        /// </summary>
        /// <param name="action"> 要执行的动作 </param>
        public void RunTask(Action action)
        {
            if (action == null) return;

            try
            {
                UpdateLoading(true);
                if (IsInMainThread)
                    action.Invoke();
                else
                    System.Windows.Application.Current.Dispatcher.Invoke(action);
            }
            catch (Exception ex)
            {
                //Msg.Error(ex.Message);
                Applog.Error(ex.Message);
            }
            finally
            {
                UpdateLoading(false);
            }
        }

        /// <summary>
        /// 显示/取消更新弹窗
        /// </summary>
        /// <param name="isOpen"> 是否开启 </param>
        /// <param name="msg"> 附带的提示消息 </param>
        public void UpdateLoading(bool isOpen, string msg = "")
        {
            IsBusy = isOpen;
            //RunUi(() =>
            //{
            //    //WeakReferenceMessenger.Default.Send(new MsgInfo()
            //    //{
            //    //    IsOpen = isOpen,
            //    //    Message = msg
            //    //}, "UpdateDialog");
            //});
        }

        #endregion
    }
}
