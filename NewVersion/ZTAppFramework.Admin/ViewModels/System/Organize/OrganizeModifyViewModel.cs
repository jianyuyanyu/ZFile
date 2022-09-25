using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class OrganizeModifyViewModel : ZTDialogViewModel
    {
        public OrganizeModifyViewModel()
        {

        }

        public override void Cancel()
        {
            OnDialogClosed();
        }

        public override void OnSave()
        {
            OnDialogClosed();
        }
    }
}
