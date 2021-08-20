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
        [Reactive]
        public TabItem SelectItem { get; set; }
        public Dictionary<string, object> PagesItem = new Dictionary<string, object>(); 
        public HomeViewModel(IScreen screen = null)
        {
            HostScreen = screen ?? Locator.Current.GetService<IScreen>();
            TabItemList = new ObservableCollection<TabItem>() {
            new TabItem(){ PageName=typeof(FileMangementPage).Name,Name="网盘" },
            new TabItem(){ PageName="",Name="上传"},
            new TabItem(){ PageName="",Name="下载"}
            };
            SelectItem = TabItemList[0];
            NvaPageGo<FileMangementPage, FileMangementViewModel>();
        }


        public void NvaPageGo<TView,TViewmodel>() where TView:new() where TViewmodel:new() 
        {
            if (PagesItem.ContainsKey(typeof(TView).Name))
            {
                SelectPage = PagesItem[typeof(TView).Name];
            }
            else
            {
                var page = new TView();
                (page as ContentView).BindingContext = new TViewmodel();
                PagesItem.Add(typeof(TView).Name, page);
                SelectPage = page;
            };
           
        }
    }
}
