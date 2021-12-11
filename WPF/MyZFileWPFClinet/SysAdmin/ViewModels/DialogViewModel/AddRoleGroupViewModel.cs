using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class AddRoleGroupViewModel : BaseViewModel, IDialogAware
    {
        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;

        public string ActType { get; set; }

        private string _RoleGroupName;
        public string RoleGroupName
        {
            get { return _RoleGroupName; }
            set { SetProperty(ref _RoleGroupName, value); }
        }


        private int _RoleSort;
        public int RoleGroupSort
        {
            get { return _RoleSort; }
            set { SetProperty(ref _RoleSort, value); }
        }

        public DelegateCommand<string> CloseDialogCommand => new DelegateCommand<string>(ExecuteCloseDialogCommand);


        SysRole Model;
        public RoleService Service { get; }

        public AddRoleGroupViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<RoleService>();
        }
        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }
        private async void ExecuteCloseDialogCommand(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.Yes;
                if (ActType == "Add")
                {
                    if (!await AddSysRoleAct())
                        return;
                }
                else if (ActType == "Edit")
                {
                    if (!await EditRoleGroupAct())
                        return;
                }
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.No;
            RaiseRequestClose(new DialogResult(result));
        }

        async Task<bool> EditRoleGroupAct()
        {
           Model.Name = RoleGroupName;
            Model.Sort = RoleGroupSort;
            var Request = await Service.Edit(Model);
            if (Request.success && Request.statusCode == 200)
            {
                return true;
            }
            return false;
        }
      
        async Task<bool> AddSysRoleAct()  
        {
            Model = new SysRole();
            Model.Codes = "";
            Model.DepartmentGroup = "";
            Model.DepartmentGuid = "";
            Model.DepartmentName = "";
            Model.ParentGuid = "";
            Model.Guid = "";
            Model.IsSystem = false;
            Model.Level = 0;
            Model.Name = RoleGroupName;
            Model.Sort = RoleGroupSort;
            Model.Summary = "";
            var Request = await Service.Add(Model);
            if (Request.success && Request.statusCode == 200)
               return true;

            return false;
        }

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {

        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            Model = null;
            Title = parameters.GetValue<string>("Titel");
            ActType = parameters.GetValue<string>("Type");
            var data = parameters.GetValue<SysRole>("Data");
            if (data == null) return;
            RoleGroupName = data.Name;
            RoleGroupSort = data.Sort;
            Model = data;
        }


    }
}
