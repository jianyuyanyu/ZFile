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
    public class AddDepartmentViewModel : BaseViewModel, IDialogAware
    {

        public string Guid { get; set; } = "";

        private string _Department;
        public string DepartmentParentName
        {
            get { return _Department; }
            set { SetProperty(ref _Department, value); }
        }

        private string _DepartmentParentId;
        public string DepartmentParentId
        {
            get { return _DepartmentParentId; }
            set { SetProperty(ref _DepartmentParentId, value); }
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

        private bool _IsOpenPopup;
        public bool IsOpenPopup
        {
            get { return _IsOpenPopup; }
            set { SetProperty(ref _IsOpenPopup, value); }
        }

        private ObservableCollection<SysOrganizeTree> _sysOrganizeTrees;
        public ObservableCollection<SysOrganizeTree> sysOrganizeTrees
        {
            get { return _sysOrganizeTrees; }
            set { SetProperty(ref _sysOrganizeTrees, value); }
        }


        public AddDepartmentViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<DepartmentService>();
        }

        public string Title { get; set; }

        public string ActType { get; set; }


        public event Action<IDialogResult> RequestClose;

        public DelegateCommand<string> CloseDialogCommand => new DelegateCommand<string>(ExecuteCloseDialogCommand);

        public DelegateCommand<SysOrganizeTree> SelectedItemChangedCommand => new DelegateCommand<SysOrganizeTree>(SelectedItemChanged);

        public DelegateCommand TextboxPrMouseDownCommand => new DelegateCommand(TextboxPrMouseDown);

        private void TextboxPrMouseDown() => IsOpenPopup = true;

        private void SelectedItemChanged(SysOrganizeTree parm)
        {
            IsOpenPopup = false;
            DepartmentParentName = parm.title;
            DepartmentParentId = parm.id;
        }

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
                if (ActType == "Add")
                {
                    if (!await AddSysOrganizeAct())
                        return;
                }
                else if (ActType == "Edit")
                {
                    if (!await EditSysOrganizeAct())
                        return;
                }



            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.No;
            RaiseRequestClose(new DialogResult(result));
        }

        async Task<bool> EditSysOrganizeAct()
        {
            AddSysOrganize Model = new AddSysOrganize();
            Model.Guid = Guid;
            Model.Name = DepartmentName;
            Model.ParentGuid = DepartmentParentId == null ? "" : DepartmentParentId;
            Model.ParentName = DepartmentParentName == null ? "" : DepartmentParentName;
            Model.Sort = DepartmentSort;
            Model.Status = IsActivation;
            
            var Request = await Service.Edit(Model);
            if (Request.success && Request.statusCode == 200)
            {
                return true;
            }
            return false;
        }


        async Task<bool> AddSysOrganizeAct()
        {
            AddSysOrganize Model = new AddSysOrganize();
            Model.Guid = "";
            Model.Name = DepartmentName;
            Model.ParentGuid = DepartmentParentId == null ? "" : DepartmentParentId;
            Model.ParentName = DepartmentParentName == null ? "" : DepartmentParentName;
            Model.Sort = DepartmentSort;
            Model.Status = IsActivation;
            var Request = await Service.Add(Model);
            if (Request.success && Request.statusCode == 200)
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



        public void OnDialogOpened(IDialogParameters parameters)
        {
            sysOrganizeTrees = new ObservableCollection<SysOrganizeTree>();
            sysOrganizeTrees = parameters.GetValue<ObservableCollection<SysOrganizeTree>>("tree");
            Title = parameters.GetValue<string>("Titel");
            ActType = parameters.GetValue<string>("Type");
            var data = parameters.GetValue<SysOrganize>("Data");
            if (data == null) return;
            DepartmentName = data.Name;
            DepartmentSort = data.Sort;
            IsActivation = data.Status;
            Guid = data.Guid;
            DepartmentParentName = data.ParentName;
            DepartmentParentId = data.ParentGuid;

        }


    }
}
