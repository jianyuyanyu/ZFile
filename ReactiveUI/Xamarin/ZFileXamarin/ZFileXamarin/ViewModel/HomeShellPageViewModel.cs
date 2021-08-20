using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFileXamarin.ViewModel
{
    class HomeShellPageViewModel : ReactiveObject, IRoutableViewModel
    {
        public HomeShellPageViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
        }
        public string UrlPathSegment => "123";

        public IScreen HostScreen {get;}
}
}
