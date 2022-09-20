using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    /// <summary>
    /// 工作台页面
    /// </summary>
    public class WorkbenchViewModel : NavigationViewModel
    {
        #region UI
        private DateTime _CurrentTime;//当前时间
        private double _CpuValues;//CPU使用率
        public DateTime CurrentTime
        {
            get { return _CurrentTime; }
            set { SetProperty(ref _CurrentTime, value); }
        }

        public double CpuValues
        {
            get { return _CpuValues; }
            set { SetProperty(ref _CpuValues, value); }
        }
        #endregion

        #region Command

        #endregion

        #region Service
        private readonly WorkbenchService _workbenchService;
        #endregion

        #region 属性
        DispatcherTimer Timer = new DispatcherTimer();
      
        #endregion

        public WorkbenchViewModel(WorkbenchService workbenchService)
        {
            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick; ;
            _workbenchService = workbenchService;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CurrentTime = DateTime.Now;
        }

        public override void OnNavigatedFrom(NavigationContext navigationContext)
        {
            Timer.Stop();
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            Timer.Start();
            CurrentTime = DateTime.Now;
            var r = await _workbenchService.GetMachineUse();
            if (r.Success)
            {
             //   CpuValues = r.data;
            }       
        }
    }
}
