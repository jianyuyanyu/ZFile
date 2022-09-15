using Prism.Commands;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ZTAppFrameword.Template.Global;

namespace ZTAppFreamework.Stared.ViewModels
{
    /// <summary>
    ///********************************************
    /// 创建人        ：  ZT
    /// 创建时间    ：  2022/9/15 10:19:04 
    /// Description   ：  自定义弹窗类
    ///********************************************/
    /// </summary>
    public abstract class ZTDialogViewModel : ViewModelBase, IZTDialogWindowAware
    {
      
        #region UI
        private string _Title;
        public string Title
        {
            get { return _Title; }
            set { SetProperty(ref _Title, value); }
        }
        private string _Messgae;
        public string Messgae
        {
            get { return _Messgae; }
            set { SetProperty(ref _Messgae, value); }
        }
        #endregion

        #region Command
        public DelegateCommand SaveCommand { get; set; }

        public DelegateCommand CancelCommand { get; set; }
        #endregion


        public ZTDialogViewModel()
        {
            SaveCommand = new DelegateCommand(OnSave);
            CancelCommand = new DelegateCommand(Cancel);
        }


        public event Action<IZTDialogResult> RequestClose;
        public abstract void Cancel();

        public abstract void OnSave();

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed(ZTAppFrameword.Template.Enums.ButtonResult result)
        {
            ZTDialogResult dialogResult = new ZTDialogResult();
            dialogResult.Result = result;
            RequestClose?.Invoke(dialogResult);
        }

        public void OnDialogClosed() => OnDialogClosed(ZTAppFrameword.Template.Enums.ButtonResult.Yes);
        public void OnDialogOpened(IZTDialogParameter parameters)
        {
            Title = parameters.GetValue<string>("Title");
            Messgae= parameters.GetValue<string>("Messgae");
        }
    }
}
