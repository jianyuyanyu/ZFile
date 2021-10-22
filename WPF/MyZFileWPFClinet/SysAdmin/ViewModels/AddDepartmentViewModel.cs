using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels
{
    public class AddDepartmentViewModel : BaseViewModel,IDialogAware
    {

        private string _Department;
        public string Department
        {
            get { return _Department; }
            set { SetProperty(ref _Department, value); }
        }

        private string _DepartmentName;
        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { SetProperty(ref _DepartmentName, value); }
        }

        private int _DepartmentSort;
        public int DepartmentSort
        {
            get { return _DepartmentSort; }
            set { SetProperty(ref _DepartmentSort, value); }
        }

        private bool _IsActivation;
        public bool IsActivation
        {
            get { return _IsActivation; }
            set { SetProperty(ref _IsActivation, value); }
        }


        public AddDepartmentViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<DepartmentService>();
        }
       
        public string Title { get; set; }

        public string ActType { get; set; }

        public event Action<IDialogResult> RequestClose;

        private DelegateCommand<string> _closeDialogCommand;
        public DelegateCommand<string> CloseDialogCommand =>
            _closeDialogCommand ?? (_closeDialogCommand = new DelegateCommand<string>(ExecuteCloseDialogCommand));

        public DepartmentService Service { get; }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
        }

       async void ExecuteCloseDialogCommand(string parameter)
        {
            ButtonResult result = ButtonResult.None;
            if (parameter?.ToLower() == "true")
            {
                result = ButtonResult.Yes;
                if (!await AddSysOrganizeAct()) return;
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.No;
            RaiseRequestClose(new DialogResult(result));
        }


        async Task<bool>  AddSysOrganizeAct()
        {
            AddSysOrganize Model = new AddSysOrganize();
            Model.Guid = "";
            Model.Name = DepartmentName;
            Model.ParentGuid = SelectItem?.id==null?"": SelectItem?.id;
            Model.ParentName = SelectItem?.title == null ? "" : SelectItem?.title; 
            Model.Sort = DepartmentSort;
            Model.Status = IsActivation;
            var Request = await Service.Add(Model);
            if (Request.success&&Request.statusCode==200)
            {
                return true;
            }
            return false;
        }
        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
           
        }

        private ObservableCollection<SysOrganizeTree> _sysOrganizeTrees;
        public ObservableCollection<SysOrganizeTree> sysOrganizeTrees
        {
            get { return _sysOrganizeTrees; }
            set { SetProperty(ref _sysOrganizeTrees, value); }
        }

        private SysOrganizeTree _SelectItem;
        public SysOrganizeTree SelectItem
        {
            get { return _SelectItem; }
            set { SetProperty(ref _SelectItem, value); }
        }

        

        public void OnDialogOpened(IDialogParameters parameters)
        {
            sysOrganizeTrees = new ObservableCollection<SysOrganizeTree>();
            sysOrganizeTrees = parameters.GetValue<ObservableCollection<SysOrganizeTree>>("tree");
            Title = parameters.GetValue<string>("Titel");
            ActType = parameters.GetValue<string>("Type");
            var data= parameters.GetValue<SysOrganize>("Data");
            if (data == null) return;
            DepartmentName = data.Name;
            DepartmentSort = data.Sort;
            IsActivation = data.Status;

        }

       
    }
}
