using System;
using Prism.Navigation;

namespace ZFileApp.ViewModels
{
    public abstract class NavigationViewModelBase : ViewModelBase, INavigationAware
    {
        public virtual void OnNavigatedFrom(INavigationParameters parameters) { }

        public virtual void OnNavigatedTo(INavigationParameters parameters) { }
    }
}
