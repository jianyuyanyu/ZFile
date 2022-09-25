using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFrameword.Template.Global;
using ZTAppFramework.Admin.Model.Sys;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{

    /// <summary>
    /// 组织页面
    /// </summary>
    public class OrganizeViewModel : NavigationViewModel
    {

        #region UI
        private List<SysOrganizeModel> _OrganizesList;
        public List<SysOrganizeModel> OrganizesList
        {
            get { return _OrganizesList; }
            set { SetProperty(ref _OrganizesList, value); }
        }
        #endregion

        #region Command
        public DelegateCommand AddCommand { get; }
        #endregion


        #region Service
        private readonly OrganizeService _organizeService;
        #endregion


        public OrganizeViewModel(OrganizeService organizeService)
        {
            _organizeService = organizeService;
            AddCommand = new DelegateCommand(Add);
        }

        private void Add()
        {
            ZTDialogParameter dialogParameter = new ZTDialogParameter();
            dialogParameter.Add("Title", "添加");
            ZTDialog.ShowDialogWindow(AppView.OrganizeModifyName, dialogParameter);
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            var r = await _organizeService.GetOrganizeList("");
            if (r.Success)
                OrganizesList = Map<List<SysOrganizeModel>>(r.data);
        }
    }
}
