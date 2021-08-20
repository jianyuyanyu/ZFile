using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using Splat;
using System;
using System.Collections.Generic;

using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using ZFileXamarin.Pages.TabPages;

using System.Collections.ObjectModel;
using ZFileXamarin.Models;
using ReactiveUI.XamForms;

namespace ZFileXamarin.ViewModel
{
   public class HomeViewModel : ReactiveObject, IRoutableViewModel
    {
        public string UrlPathSegment => "Home";

        public IScreen HostScreen { get; }
        [Reactive]
        public object SelectPage { get; set; }
        [Reactive]
        public ObservableCollection<TabItem> TabItemList { get; set; }
        public HomeViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            SelectPage = new FileMangementPage();
            TabItemList = new ObservableCollection<TabItem>() {
            new TabItem(){ ID=2,Name="网盘" },
            new TabItem(){ ID=3,Name="上传"},
            new TabItem(){ ID=4,Name="下载"}
            };
            

        }
    }
}
