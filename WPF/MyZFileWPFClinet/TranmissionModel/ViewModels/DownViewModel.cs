using Component.Common.Helpers;
using Component.ViewModelBase;
using Prism.Commands;
using Prism.Ioc;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace TranmissionModel.ViewModels
{
    public class DownViewModel :  BaseViewModel
    {

        private ObservableCollection<DownLoadInfo> _CurrnetDowmFileInfoitem;
        private readonly DownLoadHelper downLoadHelper;

        public ObservableCollection<DownLoadInfo> CurrnetDowmFileInfoitem
        {
            get { return _CurrnetDowmFileInfoitem; }
            set { SetProperty(ref _CurrnetDowmFileInfoitem, value); }
        }

        private double _SumSize;
        public double SumSize
        {
            get { return _SumSize; }
            set { SetProperty(ref _SumSize, value); }
        }

        private int _ProgressValues;
        public int ProgressValues
        {
            get { return _ProgressValues; }
            set { SetProperty(ref _ProgressValues, value); }
        }

        public DownViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            CurrnetDowmFileInfoitem = new ObservableCollection<DownLoadInfo>();
            downLoadHelper = provider.Resolve<DownLoadHelper>();       
        }

        public DelegateCommand LoadedCommand => new DelegateCommand(LoadDownInfo);

        public DelegateCommand<DownLoadInfo> ContinueOrSuspendCommand => new DelegateCommand<DownLoadInfo>(ContinueOrSuspend);

        public DelegateCommand AllSupendCommand => new DelegateCommand(AllSupend);

        public DelegateCommand AllStartCommand => new DelegateCommand(AllStart);

        private void AllStart()
        {
            foreach (var item in CurrnetDowmFileInfoitem)
                item.state = DownState.Start;
        }

        private void AllSupend()
        {
            foreach (var item in CurrnetDowmFileInfoitem)
                item.state = DownState.Suspend;
        }

        private void ContinueOrSuspend(DownLoadInfo obj)
        {
            switch (obj.state)
            {
                case DownState.Start:
                    obj.state = DownState.Suspend;
                    break;
                case DownState.Suspend:
                    obj.state = DownState.Start;
                    break;
                default:
                    break;
            }        
        }

        private void LoadDownInfo()
        {
            CurrnetDowmFileInfoitem = downLoadHelper.GetAllDownInfo();
            if(CurrnetDowmFileInfoitem?.Count!=0)
            CurrnetDowmFileInfoitem[0].state = DownState.Suspend;
            downLoadHelper.UpdateSunProgreesAct += UpdateSumProgrees;
        }

        private void UpdateSumProgrees(double Size, int Progrees)
        {
            SumSize = Size;
            ProgressValues = Progrees;
        }
    }
}
