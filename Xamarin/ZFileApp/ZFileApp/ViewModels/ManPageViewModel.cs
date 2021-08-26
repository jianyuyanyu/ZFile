using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ZFileApp.ViewModels
{
    public class ManPageViewModel : NavigationViewModelBase
    {
        private readonly INavigationService _navigationService;
        public ManPageViewModel(INavigationService navigationService)
        {
           
            _navigationService = navigationService;
            NavigateCommand = new DelegateCommand<string>(async (s) => await OnNavigateTapped(s));
        }

        private async Task OnNavigateTapped(string s)
        {
            await _navigationService.NavigateAsync($"ManPage/NavigationPage/BottomBarPage?selectedTab{s}");
        }

        public DelegateCommand<string> NavigateCommand { get; set; }

    }
}
