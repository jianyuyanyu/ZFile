using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFrameword.Template.Global;
using ZTAppFramework.Admin.Model.Sys;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class SysAdminModifyViewModel : ZTDialogViewModel
    {

        #region UI
        private List<SysOrganizeModel> _OrganizeMenu;

        public List<SysOrganizeModel> OrganizeMenu
        {
            get { return _OrganizeMenu; }
            set { SetProperty(ref _OrganizeMenu, value); }
        }
        private SysOrganizeModel _SelectOrganizeModel;

        public SysOrganizeModel SelectOrganizeModel
        {
            get { return _SelectOrganizeModel; }
            set { SetProperty(ref _SelectOrganizeModel, value); }
        }

        private List<SysPostModel> _SysPostMenu;
        public List<SysPostModel> SysPostMenu
        {
            get { return _SysPostMenu; }
            set { SetProperty(ref _SysPostMenu, value); }
        }

        private List<SysPostModel> _SelectPostItems = new List<SysPostModel>();

        public List<SysPostModel> SelectPostItems
        {
            get { return _SelectPostItems; }
            set { SetProperty(ref _SelectPostItems, value); }
        }

        private List<SysRoleModel> _SysRoleMenu;

        public List<SysRoleModel> SysRoleMenu
        {
            get { return _SysRoleMenu; }
            set { SetProperty(ref _SysRoleMenu, value); }
        }

        private List<SysRoleModel> _SelectRoleItems = new List<SysRoleModel>();
        public List<SysRoleModel> SelectRoleItems
        {
            get { return _SelectRoleItems; }
            set { SetProperty(ref _SelectRoleItems, value); }
        }
        #endregion

        #region Command

        #endregion

        #region Service
        private readonly OrganizeService _organizeService;
        private readonly SysPostService _sysPostService;
        private readonly RoleService _SysRoleService;
        #endregion
        public SysAdminModifyViewModel(OrganizeService organizeService, SysPostService sysPostService, RoleService SysRoleService)
        {
            _organizeService = organizeService;
            _sysPostService = sysPostService;
            _SysRoleService = SysRoleService;
        }
        async Task GetOrganizeInfo(string Query = "")
        {
            var r = await _organizeService.GetList(Query);
            if (r.Success)
                OrganizeMenu = Map<List<SysOrganizeModel>>(r.data).OrderBy(X => X.Sort).ToList();
            foreach (var item in OrganizeMenu)
            {
                string Name = "";
                List<string> hasOrganizeName = new List<string>();
                if (item.ParentIdList.Count > 0)
                {
                    for (int i = 0; i < item.ParentIdList.Count(); i++)
                    {
                        if (long.Parse(item.ParentIdList[i]) == item.Id) continue;
                        var info = OrganizeMenu.FirstOrDefault(x => x.Id == long.Parse(item.ParentIdList[i]));
                        if (info != null)
                        {
                            if (hasOrganizeName.Contains(info.Name)) continue;
                            hasOrganizeName.Add(info.Name);
                            Name += info.Name + "/";
                        }
                    }
                }
                if (string.IsNullOrEmpty(Name)) continue;
                item.Name = Name + item.Name;
            }
        }
        async Task GetSysPostInfo()
        {
            var r = await _sysPostService.GetPostList(new ZTAppFramewrok.Application.Stared.PageParam());
            if (r.Success)
                SysPostMenu = Map<List<SysPostModel>>(r.data.Items).OrderBy(X => X.Sort).ToList();
        }

        async Task GetRoleInfo()
        {
            var r = await _SysRoleService.GetList();
            if (r.Success)
                SysRoleMenu = Map<List<SysRoleModel>>(r.data).OrderBy(X => X.Sort).ToList();
        }
        #region Override


        public override void Cancel()
        {
            OnDialogClosed(ZTAppFrameword.Template.Enums.ButtonResult.No);
        }

        public override void OnSave()
        {
            OnDialogClosed();
        }

        public override async void OnDialogOpened(IZTDialogParameter parameters)
        {
            base.OnDialogOpened(parameters);

            await GetOrganizeInfo();
            await GetSysPostInfo();
            await GetRoleInfo();
        }

        #endregion
    }
}
