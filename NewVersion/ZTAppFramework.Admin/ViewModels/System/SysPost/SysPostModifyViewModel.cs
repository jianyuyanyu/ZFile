using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class SysPostModifyViewModel : ZTDialogViewModel
    {
        public SysPostModifyViewModel()
        {

        }

        public override void Cancel()
        {
            OnDialogClosed(ZTAppFrameword.Template.Enums.ButtonResult.No);
        }

        public override void OnSave()
        {
            OnDialogClosed();
        }
    }
}
