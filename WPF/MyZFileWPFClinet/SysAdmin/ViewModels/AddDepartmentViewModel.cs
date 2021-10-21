using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SysAdmin.ViewModels
{
    public class AddDepartmentViewModel : IDialogAware
    {
        public string Title => "添加组织机构";

        public event Action<IDialogResult> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            
        }
    }
}
