using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Navigation;
using ZTAppFramework.Admin.Model.Menus;
using ZTAppFramework.Application.Service;
using ZTAppFreamework.Stared;
using ZTAppFreamework.Stared.ViewModels;

namespace ZTAppFramework.Admin.ViewModels
{
    public class HomeViewModel : NavigationViewModel
    {
        private readonly MenuService _menuService;
        public IRegionManager _RegionManager { get; set; }
        public NavigationService NavigationService { get; set; }
        #region UI
        private List<MenuModel> _MenuList;
        public List<MenuModel> MenuList
        {
            get { return _MenuList; }
            set { SetProperty(ref _MenuList, value); }
        }


        private MenuModel _SelectPage;
        public MenuModel SelectPage
        {
            get { return _SelectPage; }
            set
            {
                if (SetProperty(ref _SelectPage, value))
                {
                    switch (value.name)
                    {
                        case "组织机构":
                            _RegionManager?.Regions[AppView.HomeName]?.RequestNavigate(AppView.OrganizeName); break;
                        case "工作台":
                            _RegionManager?.Regions[AppView.HomeName]?.RequestNavigate(AppView.WorkbenchName); break;
                        default:
                            break;
                    }

                   
                }
            }
        }
        #endregion

        #region Command
        public DelegateCommand<MenuModel> GoPageCommand { get; set; }
        #endregion
        public HomeViewModel(MenuService menuService, IRegionManager regionManager)
        {
            _menuService = menuService;
            _RegionManager = regionManager;
            GoPageCommand = new DelegateCommand<MenuModel>(GoPage);
        }

        private void GoPage(MenuModel Parm)
        {
            SelectPage = Parm;// MenuList.First().Childer.First();
            SelectPage.IsSelected = true;
        }

        public override async Task OnNavigatedToAsync(NavigationContext navigationContext = null)
        {
            var Ar = await _menuService.GetMenuList();
            if (Ar.Success)
            {
                MenuList = Map<List<MenuModel>>(Ar.data);
                //MenuList.First().Childer.Sort((x, y) => y.sort.CompareTo(x.sort));
                SelectPage = MenuList.First().Childer.First();
                MenuList.First().IsSelected = true;
                SelectPage.IsSelected = true;
            
            }

        }
    }
}
