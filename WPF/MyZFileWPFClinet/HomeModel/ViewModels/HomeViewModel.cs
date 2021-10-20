using Component;
using Component.Common;
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

namespace HomeModel.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly MenuService Service;

        private ObservableCollection<Menu> _MenuItems;
        public ObservableCollection<Menu>  MenuItems
        {
            get { return _MenuItems; }
            set { SetProperty(ref _MenuItems, value); }
        }

        private Menu _Seleitem;
        public Menu Seleitem
        {
            get { return _Seleitem; }
            set
            {
                if (SetProperty(ref _Seleitem, value))
                    NvChangagePage(SystemResource.Nav_HomeContent,  "View");
            }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }


        public HomeViewModel(IContainerProvider provider, IRegionManager regionManager) : base(provider, regionManager)
        {
            this.Service = provider.Resolve<MenuService>();
        }
        public DelegateCommand LoadedCommand => new DelegateCommand(LoadMenu);
        public async void LoadMenu()
        {
            var model = await Service.GetMenuInfo();
            if (model.success && model.statusCode == 200)
            {
                MenuItems = new ObservableCollection<Menu>();
                model.data.ForEach(o =>
                {
                    if (o.layer == 2)
                        MenuItems.Add(new Menu() { Head=o});
                    if (o.layer == 3)
                    {
                        var AddVar = MenuItems.Where(c => c.Head.guid == o.parentGuid).FirstOrDefault();
                        if (AddVar != null)
                            AddVar.ChilderList.Add(o);
                    }
                });
            }


            //MenuItems.AddRange(model.data.Where(o => o.PModelCode == "WORK").ToArray()) ;
            //MenuItems.Add(new Menu() { IsSys = 1, PModelCode = "WORK", ModelName = "上传", ModelCode = "Upload" });
            //MenuItems.Add(new Menu() { IsSys = 1, PModelCode = "WORK", ModelName = "下载",ModelCode="Down" });
            //Seleitem = MenuItems[0];
            //NvChangagePage(SystemResource.Nav_HomeContent, Seleitem.ModelCode + "View");
            //UserName = Contract.UserInfo.username;
        }
    }
}
