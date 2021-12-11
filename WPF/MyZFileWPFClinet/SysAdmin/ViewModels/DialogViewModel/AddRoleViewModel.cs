using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Regions;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SysAdmin.ViewModels.DialogViewModel
{
    public class AddRoleViewModel : BaseViewModel, IDialogAware
    {


        public string ActType { get; private set; }

        public string Title { get; set; }

        public event Action<IDialogResult> RequestClose;


        private bool _isSystem;

        public bool IsSystem
        {
            get { return _isSystem; }
            set
            {
                SetProperty(ref _isSystem, value);
            }
        }

        private string _RoleName;

        public string RoleName
        {
            get { return _RoleName; }
            set
            {
                SetProperty(ref _RoleName, value);
            }
        }

        private int _RoleSort;

        public int RoleSort
        {
            get { return _RoleSort; }
            set
            {
                SetProperty(ref _RoleSort, value);
            }
        }

        private string _summary="";

        public string summary
        {
            get { return _summary; }
            set
            {
                SetProperty(ref _summary, value);
            }
        }

        private string _DepartmentParentName;

        public string DepartmentParentName
        {
            get { return _DepartmentParentName; }
            set
            {
                SetProperty(ref _DepartmentParentName, value);
            }
        }



        private ObservableCollection<SysRole> _sysRoles;

        public ObservableCollection<SysRole> SysRoles
        {
            get { return _sysRoles; }
            set
            {
                SetProperty(ref _sysRoles, value);
            }
        }

        private ObservableCollection<SysOrganizeTree> _sysOrganizeTrees;
        public ObservableCollection<SysOrganizeTree> SysOrganizesTrees
        {
            get { return _sysOrganizeTrees; }
            set { SetProperty(ref _sysOrganizeTrees, value); }
        }

        private SysRole _SelctRoleGrour;

        public SysRole SelctRoleGrour
        {
            get { return _SelctRoleGrour; }
            set
            {
                SetProperty(ref _SelctRoleGrour, value);
            }
        }


        private bool _IsOpenPopup;
        public bool IsOpenPopup
        {
            get { return _IsOpenPopup; }
            set { SetProperty(ref _IsOpenPopup, value); }
        }

        SysRole Model = new SysRole();
        public AddRoleViewModel(IContainerProvider provider, IRegionManager regionManager, IDialogService dialogService) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<RoleService>();
        }

        public DelegateCommand<string> CloseDialogCommand => new DelegateCommand<string>(ExecuteCloseDialogCommand);

        public DelegateCommand LoadedCommand => new DelegateCommand(Loaded);

        public DelegateCommand<SysOrganizeTree> SelectedItemChangedCommand => new DelegateCommand<SysOrganizeTree>(SelectedItemChanged);
        public DelegateCommand TextboxPrMouseDownCommand => new DelegateCommand(TextboxPrMouseDown);

        private void TextboxPrMouseDown() => IsOpenPopup = true;
        private void SelectedItemChanged(SysOrganizeTree param)
        {
            IsOpenPopup = false;
            Model.DepartmentGuid = param.id;
            DepartmentParentName= Model.DepartmentName = param.title;
        }

        private async void Loaded()
        {
            var res = await Service.Getpages(0, 1000);
            if (res.code == 0)
            {
                SysRoles = new ObservableCollection<SysRole>();
                SysRoles.AddRange(res.data.Where(o=>o.Level==0).ToList());
            }

            var ApiRquest = await Service.Gettree();
            if (ApiRquest.success && ApiRquest.statusCode == 200)
            {
                SysOrganizesTrees = new ObservableCollection<SysOrganizeTree>();
                SysOrganizesTrees.AddRange(ApiRquest.data);
            }
        }

        public RoleService Service { get; }

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
            }
            else if (parameter?.ToLower() == "false")
                result = ButtonResult.No;
            RaiseRequestClose(new DialogResult(result));
        }

        private async Task<bool> EditRoleGroupAct()
        {


            return false;
        }

        private async Task<bool> AddSysRoleAct()
        {

            Model.Codes = "";
            Model.DepartmentGroup = "";
            Model.ParentGuid = "";
            Model.Guid = "";
            Model.IsSystem = false;
            Model.Level = 1;
            Model.Name = RoleName;
            Model.Sort = 1;
            Model.Summary = summary==null?"":summary;
            Model.ParentGuid = SelctRoleGrour.Guid;
            var Request = await Service.Add(Model);
            if (Request.success && Request.statusCode == 200)
                return true;

            return false;
        }

        public virtual void RaiseRequestClose(IDialogResult dialogResult)
        {
            RequestClose?.Invoke(dialogResult);
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
            Title = parameters.GetValue<string>("Titel");
            ActType = parameters.GetValue<string>("Type");
        }
    }
}
