using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using ZTAppFramework.Admin.Model.Sys;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class SyslogViewModel : NavigationViewModel
    {
        #region Ui
        private List<SysLogMenuModel> _MenuList;
        public List<SysLogMenuModel> MenuList
        {
            get { return _MenuList; }
            set { SetProperty(ref _MenuList, value); }
        }
        #endregion

        #region Command

        #endregion

        #region Service

        #endregion

        #region 属性

        #endregion
        public SyslogViewModel()
        {
            MenuList=new List<SysLogMenuModel>();
            CreateMenu();
        }

        #region Event
        void CreateMenu()
        {
            MenuList.Add(new SysLogMenuModel()
            {
                Name = "日志级别",
                Childer = new List<SysLogMenuModel>()
                {
                    new SysLogMenuModel(){  Name="Debug"} ,
                    new SysLogMenuModel(){  Name="Info"} ,
                    new SysLogMenuModel(){  Name="Warn"} ,
                    new SysLogMenuModel(){  Name="Error"} ,
                    new SysLogMenuModel(){  Name="Fatal"} ,
                }
            });

            MenuList.Add(new SysLogMenuModel() { Name = "日志类型",
                Childer=new List<SysLogMenuModel>() 
                {
                  new SysLogMenuModel(){  Name="登入日志"} ,
                  new SysLogMenuModel(){  Name="操作日志"} ,
                }
            
            });
        }
        #endregion
        #region override

        #endregion

    }
}
