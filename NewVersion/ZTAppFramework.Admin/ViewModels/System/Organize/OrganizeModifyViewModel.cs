using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFrameword.Template.Global;
using ZTAppFramework.Admin.Model.Sys;
using ZTAppFramework.Application.Service;
using ZTAppFramewrok.Application.Stared;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class OrganizeModifyViewModel : ZTDialogViewModel
    {
        #region UI

        private SysOrganizeModel _OrganizeModel;

        public SysOrganizeModel OrganizeModel
        {
            get { return _OrganizeModel; }
            set { SetProperty(ref _OrganizeModel, value); }
        }

        #endregion

        #region  Command

        #endregion

        #region Service
        private readonly OrganizeService _organizeService;
        #endregion
        public OrganizeModifyViewModel(OrganizeService organizeService)
        {
            _organizeService = organizeService;
        }

        #region 属性
        public bool IsEdit { get; set; }

        #endregion
        #region override


        public override void Cancel()
        {
            if (IsBusy) return;
            OnDialogClosed(ZTAppFrameword.Template.Enums.ButtonResult.No);
        }

        public override async void OnSave()
        {
            await SetBusyAsync(async () =>
            {
                await Task.Delay(1000);
                if (IsEdit)
                {
                    if (!await Modif())
                        return;
                }
                else
                {
                    if (!await Add())
                        return;
                }
                OnDialogClosed();
            });
        }

        async Task<bool> Add()
        {
            var r = await _organizeService.ModifOrganize(Map<SysOrganizeDto>(OrganizeModel));
            if (r.Success)
            {
                Show("提示", r.Message);
                return true;
            }
            return false;
        }

        async Task<bool> Modif()
        {
            var r = await _organizeService.ModifOrganize(Map<SysOrganizeDto>(OrganizeModel));
            if (r.Success)
            {
                Show("提示", r.Message);
                return true;
            }
            return false;
        }

        public override void OnDialogOpened(IZTDialogParameter parameters)
        {
            base.OnDialogOpened(parameters);
            IsEdit = true;
            var Model = parameters.GetValue<SysOrganizeModel>("Param");
            if (Model == null)
            {
                OrganizeModel = new SysOrganizeModel();
                IsEdit = false;
            }
            else
            {
                OrganizeModel = DeepCopy<SysOrganizeModel>(Model);
            }
        }

        #endregion
    }
}
