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

        public DelegateCommand<SysOrganizeModel> ModifCommand { get; }

        public DelegateCommand<SysOrganizeModel> DeleteSeifCommand { get; }

        public DelegateCommand DeleteSelectCommand { get; }
        #endregion


        #region Service
        private readonly OrganizeService _organizeService;
        #endregion


        public OrganizeViewModel(OrganizeService organizeService)
        {
            _organizeService = organizeService;
            AddCommand = new DelegateCommand(Add);
            ModifCommand = new DelegateCommand<SysOrganizeModel>(Modif);
            DeleteSeifCommand = new DelegateCommand<SysOrganizeModel>(DeleteSeif);
            DeleteSelectCommand = new DelegateCommand(DeleteSelect);
        }
        #region Event




        private void DeleteSelect()
        {

        }
        private void Modif(SysOrganizeModel Param)
        {
            ZTDialogParameter dialogParameter = new ZTDialogParameter();
            dialogParameter.Add("Title", "编辑");
            dialogParameter.Add("Param", Param);
            ZTDialog.ShowDialogWindow(AppView.OrganizeModifyName, dialogParameter, async x =>
            {
                if (x.Result == ZTAppFrameword.Template.Enums.ButtonResult.Yes)
                {
                    await GetOrganizeInfo();
                }
            });
        }
        private void DeleteSeif(SysOrganizeModel Param)
        {
            ShowDialog("提示", "确定要删除码", async x =>
            {
                if (x.Result == ZTAppFrameword.Template.Enums.ButtonResult.Yes)
                {
                   await  GetOrganizeInfo();
                }
            }, System.Windows.MessageBoxButton.YesNo);
        }

        private void Add()
        {
            ZTDialogParameter dialogParameter = new ZTDialogParameter();
            dialogParameter.Add("Title", "添加");
            ZTDialog.ShowDialogWindow(AppView.OrganizeModifyName, dialogParameter);
        }

        async Task GetOrganizeInfo(string Query="")
        {
            var r = await _organizeService.GetOrganizeList(Query);
            if (r.Success)
                OrganizesList = Map<List<SysOrganizeModel>>(r.data);
        }
        #endregion

        #region Override
        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            await GetOrganizeInfo();
        }


        #endregion

    }
}
