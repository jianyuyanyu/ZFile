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
        public ObservableCollection<Menu> MenuItems
        {
            get { return _MenuItems; }
            set { SetProperty(ref _MenuItems, value); }
        }

        private SysMenuDto _Seleitem;
        public SysMenuDto Seleitem
        {
            get { return _Seleitem; }
            set
            {
                if (SetProperty(ref _Seleitem, value))
                    NvChangagePage(SystemResource.Nav_HomeContent, _Seleitem.nameCode+"View");
            }
        }
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
            set { SetProperty(ref _UserName, value); }
        }


        public DelegateCommand<object> SelectItemChangeCommand => new DelegateCommand<object>(SelectItemChange);
        private void SelectItemChange(object obj)
        {
            if (obj == null ) return;
            var Menu = obj as SysMenuDto;
            if (Menu.layer == 2) return;
            Seleitem = Menu;

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
                        MenuItems.Add(new Menu() {
                            btnFun = o.btnFun, 
                            btnJson=o.btnJson,
                            isChecked=o.isChecked,
                            nameCode=o.nameCode,
                            guid=o.guid,
                            icon=o.icon,
                            layer=o.layer,
                            name=o.name,
                            parentGuid=o.parentGuid,
                            parentGuidList=o.parentGuidList,
                            parentName=o.parentName,
                            sort=o.sort,
                            urls=o.urls
                        });
                    if (o.layer == 3)
                    {
                        var AddVar = MenuItems.Where(c => c.guid == o.parentGuid).FirstOrDefault();
                        if (AddVar != null)
                            AddVar.ChilderList.Add(o);
                    }
                });
            }

            Menu FileMenu = new Menu() { name = "文件管理" };
            FileMenu.ChilderList.Add(new SysMenuDto() { name = "个人空间", nameCode = "GRKJ" });
            FileMenu.ChilderList.Add(new SysMenuDto() { name="上传",nameCode= "Upload" });
            FileMenu.ChilderList.Add(new SysMenuDto() { name = "下载", nameCode = "Down" });

            MenuItems.Add(FileMenu);

            //MenuItems.Add(new Menu() { name = "上传", nameCode = "Upload" });
            //MenuItems.Add(new Menu() { name = "上传",nameCode= "Upload" });
            //MenuItems.Add(new Menu() { name = "下载", nameCode = "Down" });
            //MenuItems.AddRange(model.data.Where(o => o.PModelCode == "WORK").ToArray()) ;
            //MenuItems.Add(new Menu() { IsSys = 1, PModelCode = "WORK", ModelName = "上传", ModelCode = "Upload" });
            //MenuItems.Add(new Menu() { IsSys = 1, PModelCode = "WORK", ModelName = "下载",ModelCode="Down" });
            //Seleitem = MenuItems[0];
            //NvChangagePage(SystemResource.Nav_HomeContent, Seleitem.ModelCode + "View");
            //UserName = Contract.UserInfo.username;
        }
    }
}
