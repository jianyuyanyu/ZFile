using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ZTAppFramework.Admin.Model.Menus;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class HomeViewModel : NavigationViewModel
    {
        private readonly MenuService _menuService;

        private List<MenuModel> _MenuList;
        public List<MenuModel> MenuList
        {
            get { return _MenuList; }
            set { SetProperty(ref _MenuList, value); }
        }
        public HomeViewModel(MenuService menuService)
        {
            _menuService = menuService;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            var Ar = await _menuService.GetMenuList();
            if (Ar.Success)
                MenuList = Map<List<MenuModel>>(Ar.data);
        }
    }
}
