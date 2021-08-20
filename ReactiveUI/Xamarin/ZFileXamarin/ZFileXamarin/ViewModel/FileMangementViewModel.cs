using ReactiveUI;
using Splat;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZFileXamarin.ViewModel
{
    public class FileMangementViewModel : ReactiveObject
    {
        public FileMangementViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            
        }

        public IScreen HostScreen  {get;}
    }
}
